using Addmusic2.Helpers;
using Addmusic2.Model;
using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Addmusic2.Model.Localization;
using Addmusic2.Model.SongTree;
using Addmusic2.Services;
using AsarCLR.Asar191;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Addmusic2.Parsers
{
    internal class SoundEffectParser : ISoundEffectParser
    {
        private readonly ILogger<IAddmusicLogic> _logger;
        private readonly MessageService _messageService;
        //private readonly SongListItem _songListItem;
        private readonly GlobalSettings _globalSettings;
        private readonly IFileCachingService _fileCachingService;
        private readonly RomOperations _romOperations;
        //private readonly SongScope _songScope;
        public SoundEffectData SoundEffectData { get; set; } = new();

        private List<byte> DataChannel = new();
        private Dictionary<int, string> JsrPositionsAndNames = new();
        private Dictionary<string, string> NamedAsmBlocks = new();

        private int DefaultNoteLength { get; set; } = MagicNumbers.DefaultValues.InitialSfxNoteLength;
        private int CurrentOctave { get; set; } = MagicNumbers.DefaultValues.StartingOctave;
        private int LeftVolume { get; set; } = MagicNumbers.DefaultValues.InitialSfxLeftVolume;
        private int RightVolume { get; set; } = MagicNumbers.DefaultValues.InitialSfxRightVolume;
        private int CurrentInstrument { get; set; } = -1;
        private int PreviousNoteLength { get; set; }
        private bool FirstNote { get; set; } = true;
        private int PreviousNote { get; set; } = -1;
        private bool InPitchSlide { get; set; } = false;

        private bool UpdateVolume { get; set; } = false;

        public SoundEffectParser(
            ILogger<IAddmusicLogic> logger,
            MessageService messageService,
            GlobalSettings globalSettings,
            IFileCachingService fileCachingService,
            RomOperations romOperations
            //SongListItem songItem,
            //SongScope songScope
        )
        {
            _logger = logger;
            _messageService = messageService;
            //_songListItem = songItem;
            _globalSettings = globalSettings;
            _fileCachingService = fileCachingService;
            //_songScope = songScope;
        }

        public SoundEffectData ParseSoundEffectNodes(List<ISongNode> nodes)
        {
            foreach (SongNode node in nodes)
            {
                ParseNode(node);
            }


            SoundEffectData.ChannelData = DataChannel;
            SoundEffectData.JsrPositionsAndNames = JsrPositionsAndNames;
            SoundEffectData.NamedAsmBlocks = NamedAsmBlocks;


            return SoundEffectData;
        }

        // done at a different point in execution because there isn't a value AramPosition during normal parsing
        public void CompileAsmElements(SoundEffectData soundEffectData)
        {
            var tempAsmPath = Path.Combine(FileNames.ExecutionLocations.InstallLocation, FileNames.FolderNames.LogFolder, FileNames.StaticFiles.TempAsmFile);
            var tempBinPath = Path.Combine(FileNames.ExecutionLocations.InstallLocation, FileNames.FolderNames.LogFolder, FileNames.StaticFiles.TempBinFile);
            using var tempAsmWriter = new StreamWriter(tempAsmPath, false);

            // Compile the asm blocks

            foreach (var block in soundEffectData.NamedAsmBlocks)
            {
                var channelDataSize = soundEffectData.ChannelData.Count;

                var aramPosition = soundEffectData.AramPosition + soundEffectData.CompiledAsmCodeBlocks.Count + soundEffectData.ChannelData.Count;
                var sfxPatchString = PatchBuilders.BuildSoundEffectAsmPatch(aramPosition, block.Value);

                tempAsmWriter.Write(sfxPatchString.ToCharArray());
                var isCompileSuccessful = _romOperations.CompileAsmToBin(FileNames.StaticFiles.TempAsmFile, FileNames.StaticFiles.TempBinFile);

                if(!isCompileSuccessful)
                {
                    // todo fix exception
                    throw new Exception();
                }
                using var tempBinFile = File.Open(tempBinPath, FileMode.OpenOrCreate);
                var data = new byte[tempBinFile.Length - MagicNumbers.SfxCompiledBinCodeLocation];
                tempBinFile.Read(data, MagicNumbers.SfxCompiledBinCodeLocation, data.Length);
                var jsrInformation = new JsrInformation
                {
                    JsrName = block.Key,
                    JsrData = data,
                    SequencePosition = soundEffectData.CompiledAsmCodeBlocks.Count,
                };
                soundEffectData.JsrInformation.Add(jsrInformation);
                soundEffectData.CompiledAsmCodeBlocks.Add(block.Key, data);

            }

            // Match Jumps with their respective asm block code

            foreach ( var jsr in soundEffectData.JsrPositionsAndNames)
            {
                var jsrInformation = soundEffectData.JsrInformation.Find(j => j.JsrName == jsr.Value);

                if (jsrInformation == null)
                {
                    // todo fix exception
                    throw new Exception();
                }

                var jsrDataPosition = jsr.Key;
                var adjustmentValue = (byte)(soundEffectData.AramPosition + soundEffectData.ChannelData.Count + jsrInformation.SequencePosition);
                soundEffectData.ChannelData[jsrDataPosition] = (byte)( adjustmentValue & MagicNumbers.ByteHexMaximum );
                soundEffectData.ChannelData[jsrDataPosition + 1] = (byte)( adjustmentValue >> 8 );
            }

        }



        public void ParseNode(SongNode node)
        {
            var validationResult = (ValidationResult)ValidateNode(node);

            if (validationResult.Type == ValidationResult.ResultType.Skip)
            {
                return;
            }
            else if (validationResult.Type == ValidationResult.ResultType.Failure)
            {
                // todo handle failure cases
            }
            else if (validationResult.Type == ValidationResult.ResultType.Warning ||
                validationResult.Type == ValidationResult.ResultType.Error)
            {
                // todo handle error cases
            }

            EvaluateNode(node);
        }

        public IValidationResult ValidateNode(ISongNode songNode)
        {
            return songNode switch
            {
                null => throw new ArgumentNullException(nameof(songNode)),
                DirectiveNode => ValidateSpecialDirective(songNode as DirectiveNode),
                AtomicNode => ValidateAtomicNode(songNode as AtomicNode),
                CompositeNode => ValidateCompositeNode(songNode as CompositeNode),
                LoopNode => throw new Exception(),
                HexNode => ValidateHexNode(songNode as HexNode),
                _ => new ValidationResult
                {
                    Type = ValidationResult.ResultType.Skip,
                },
            };
        }

        public void EvaluateNode(ISongNode songNode)
        {
            switch (songNode)
            {
                case null:
                    throw new ArgumentNullException(nameof(songNode));
                case DirectiveNode:
                    EvaluateSpecialDirective(songNode as DirectiveNode);
                    break;
                case AtomicNode:
                    EvaluateAtomicNode(songNode as AtomicNode);
                    break;
                case CompositeNode:
                    EvaluateCompositeNode(songNode as CompositeNode);
                    break;
                case LoopNode:
                    throw new Exception();
                    break;
                case HexNode:
                    EvaluateHexNode(songNode as HexNode);
                    break;
                default:
                    throw new Exception();
            }
        }

        #region Node Evaluators

        #region Atomic Node Evaluators

        public void EvaluateAtomicNode(AtomicNode atomic)
        {
            switch (atomic.NodeType)
            {
                case SongNodeType.Note:
                    EvaluateNoteNode(atomic);
                    break;
                case SongNodeType.Rest:
                    EvaluateRestNode(atomic);
                    break;
                case SongNodeType.Tie:
                    EvaluateTieNode(atomic);
                    break;
                case SongNodeType.LowerOctave:
                    EvaluateLowerOctaveNode(atomic);
                    break;
                case SongNodeType.RaiseOctave:
                    EvaluateRaiseOctaveNode(atomic);
                    break;
                case SongNodeType.Octave:
                    EvaluateOctaveNode(atomic);
                    break;
                case SongNodeType.DefaultLength:
                    EvaluateDefaultLengthNode(atomic);
                    break;
                case SongNodeType.SfxInstrument:
                    EvaluateSfxInstrumentNode(atomic);
                    break;
                case SongNodeType.SfxVolume:
                    EvaluateVolumeNode(atomic);
                    break;
                case SongNodeType.Pipe:
                    // currently not implemented
                    return;
                default:
                    throw new Exception();
            }
        }

        public void EvaluateNoteNode(AtomicNode noteNode, bool inTriplet = false, bool inPitchSlide = false, bool isNextForDDPitchSlide = false)
        {

            var notePayload = noteNode.Payload as NotePayload;
            var currentInstrument = CurrentInstrument;

            var tempLength = GetNoteLength(noteNode, notePayload!.Duration, notePayload.DotCount, inTriplet, true);

            var noteValueChar = (int)notePayload.NoteValue[0];
            var note = GetPitchValue(noteValueChar, notePayload.Accidental);

            note = (note < 0) ? MagicNumbers.CommandValues.Rest : note;

            if(note == PreviousNoteLength && UpdateVolume == false)
            {
                note = 0;
            }

            if(InPitchSlide == true)
            {
                if(FirstNote == true)
                {
                    if (PreviousNote == -1)
                    {
                        PreviousNote = note;
                    }
                    else
                    {
                        if(tempLength > 0)
                        {
                            AddDataToChannel((byte)tempLength);
                            PreviousNoteLength = tempLength;
                        }
                        UpdateAndAddVolume();

                        AddDataToChannel(MagicNumbers.CommandValues.PitchSlide);
                        AddDataToChannel((byte)PreviousNote);
                        AddDataToChannel(0x00);
                        AddDataToChannel(((byte)PreviousNoteLength));
                        AddDataToChannel((byte)note);
                        FirstNote = false;
                    }
                }
                else
                {
                    if (tempLength > 0)
                    {
                        AddDataToChannel((byte)tempLength);
                        PreviousNoteLength = tempLength;
                    }

                    UpdateAndAddVolume();

                    AddDataToChannel(MagicNumbers.CommandValues.SfxPitchSlide);
                    AddDataToChannel(0x00);
                    AddDataToChannel(((byte)PreviousNoteLength));
                    AddDataToChannel((byte)note);
                }

                if(tempLength < 0)
                {
                    PreviousNoteLength = tempLength;
                }

                // Exit early is resolving a pitch slide
                return;
            }

            if(tempLength >= 0x80)
            {
                AddDataToChannel(0x7F);

                UpdateAndAddVolume();

                AddDataToChannel((byte)note);

                tempLength -= 0x7F;

                while(tempLength > 0x7F)
                {
                    tempLength -= 0x7F;
                    AddDataToChannel(MagicNumbers.CommandValues.Tie);
                }

                if(tempLength > 0)
                {
                    if(tempLength != 0x7F)
                    {
                        AddDataToChannel((byte)tempLength);
                    }

                    AddDataToChannel(MagicNumbers.CommandValues.Tie);
                }

                PreviousNoteLength = tempLength;
            }
            else if(tempLength > 0)
            {
                AddDataToChannel((byte)tempLength);
                PreviousNoteLength = tempLength;
                UpdateAndAddVolume();
                AddDataToChannel((byte)note);
            }
            else
            {
                AddDataToChannel((byte)note);
            }

        }

        public void EvaluateTieNode(AtomicNode tieNode, bool inTriplet = false, bool inPitchSlide = false, bool isNextForDDPitchSlide = false)
        {
            var tiePayload = tieNode.Payload as TiePayload;
            var currentInstrument = CurrentInstrument;

            var tempLength = GetNoteLength(tieNode, tiePayload!.Duration, tiePayload.DotCount, inTriplet, true);

            var note = MagicNumbers.CommandValues.Tie;

            note = (note < 0) ? MagicNumbers.CommandValues.Rest : note;

            if (note == PreviousNoteLength && UpdateVolume == false)
            {
                note = 0;
            }

            FirstNote = true;
            InPitchSlide = false;

            if (tempLength >= 0x80)
            {
                AddDataToChannel(0x7F);

                UpdateAndAddVolume();

                AddDataToChannel((byte)note);

                tempLength -= 0x7F;

                while (tempLength > 0x7F)
                {
                    tempLength -= 0x7F;
                    AddDataToChannel(MagicNumbers.CommandValues.Tie);
                }

                if (tempLength > 0)
                {
                    if (tempLength != 0x7F)
                    {
                        AddDataToChannel((byte)tempLength);
                    }

                    AddDataToChannel(MagicNumbers.CommandValues.Tie);
                }

                PreviousNoteLength = tempLength;
            }
            else if (tempLength > 0)
            {
                AddDataToChannel((byte)tempLength);
                PreviousNoteLength = tempLength;
                UpdateAndAddVolume();
                AddDataToChannel((byte)note);
            }
            else
            {
                AddDataToChannel((byte)note);
            }
        }

        public void EvaluateRestNode(AtomicNode restNode, bool inTriplet = false, bool inPitchSlide = false, bool isNextForDDPitchSlide = false)
        {
            var restPayload = restNode.Payload as TiePayload;
            var currentInstrument = CurrentInstrument;

            var tempLength = GetNoteLength(restNode, restPayload!.Duration, restPayload.DotCount, inTriplet, true);

            var note = MagicNumbers.CommandValues.Rest;

            note = (note < 0) ? MagicNumbers.CommandValues.Rest : note;

            if (note == PreviousNoteLength && UpdateVolume == false)
            {
                note = 0;
            }

            if (tempLength >= 0x80)
            {
                AddDataToChannel(0x7F);

                UpdateAndAddVolume();

                AddDataToChannel((byte)note);

                tempLength -= 0x7F;

                while (tempLength > 0x7F)
                {
                    tempLength -= 0x7F;
                    AddDataToChannel(MagicNumbers.CommandValues.Tie);
                }

                if (tempLength > 0)
                {
                    if (tempLength != 0x7F)
                    {
                        AddDataToChannel((byte)tempLength);
                    }

                    AddDataToChannel(MagicNumbers.CommandValues.Tie);
                }

                PreviousNoteLength = tempLength;
            }
            else if (tempLength > 0)
            {
                AddDataToChannel((byte)tempLength);
                PreviousNoteLength = tempLength;
                UpdateAndAddVolume();
                AddDataToChannel((byte)note);
            }
            else
            {
                AddDataToChannel((byte)note);
            }
        }

        public void EvaluateDefaultLengthNode(AtomicNode defaultLengthNode)
        {
            var defaultLengthPayload = defaultLengthNode.Payload as DefaultLengthPayload;

            if (defaultLengthPayload == null)
            {
                throw new Exception();
            }

            DefaultNoteLength = defaultLengthPayload.Length;
        }

        public void EvaluateSfxInstrumentNode(AtomicNode instrumentNode)
        {
            var sfxInstrumentPayload = instrumentNode.Payload as SfxInstrumentPayload;

            if (sfxInstrumentPayload == null)
            {
                throw new Exception();
            }

            AddDataToChannel(MagicNumbers.CommandValues.Instrument);
            if(sfxInstrumentPayload.NoiseHexValue.Length > 0)
            {
                var noiseValue = Convert.ToByte(sfxInstrumentPayload.NoiseHexValue, 16);
                AddDataToChannel((byte)(0x80 | noiseValue));
            }
            AddDataToChannel((byte)sfxInstrumentPayload.InstrumentNumber);

            CurrentInstrument = sfxInstrumentPayload.InstrumentNumber;
        }

        public void EvaluateLowerOctaveNode(AtomicNode raiseOctaveNode)
        {
            CurrentOctave--;
            if (CurrentOctave < MagicNumbers.OctaveMinimum)
            {
                CurrentOctave = 0;
                var message = _messageService.GetWarningOctaveDroppedTooLowMessage();
            }
        }

        public void EvaluateOctaveNode(AtomicNode octaveNode)
        {
            var octavePayload = octaveNode.Payload as OctavePayload;

            if (octavePayload == null)
            {
                throw new Exception();
            }

            CurrentOctave = octavePayload.OctaveNumber;
        }

        public void EvaluateRaiseOctaveNode(AtomicNode raiseOctaveNode)
        {
            CurrentOctave++;
            if (CurrentOctave > MagicNumbers.OctaveMaximum)
            {
                CurrentOctave = MagicNumbers.OctaveMaximum;
                var message = _messageService.GetWarningOctaveRaisedTooHighMessage();
            }
        }

        public void EvaluateVolumeNode(AtomicNode sfxVolumeNode)
        {
            var sfxVolumePayload = sfxVolumeNode.Payload as SfxVolumePayload;

            if (sfxVolumePayload == null)
            {
                throw new Exception();
            }

            if(sfxVolumePayload.Volume == -1)
            {
                LeftVolume = sfxVolumePayload.LeftVolumeValue;
                RightVolume = sfxVolumePayload.RightVolumeValue;
            }
            else
            {
                LeftVolume = sfxVolumePayload.Volume;
                RightVolume = sfxVolumePayload.Volume;
            }

            UpdateVolume = true;
        }


        #endregion

        #region Composite Node Evaluators

        public void EvaluateCompositeNode(CompositeNode compositeNode)
        {
            switch (compositeNode.NodeType)
            {
                case SongNodeType.Triplet:
                    EvaluateTriplet(compositeNode);
                    break;
                case SongNodeType.PitchSlide:
                    EvaluatePitchSlideNode(compositeNode);
                    break;
                case SongNodeType.HexCommand:
                    EvaluateHexCommand(compositeNode);
                    break;
                default:
                    throw new Exception();
                    break;
            }
        }

        public void EvaluateHexCommand(CompositeNode node)
        {
            var payload = node.Payload as HexNumberPayload;
            var byteData = Convert.ToByte(payload.HexValue, 16);

            AddDataToChannel(byteData);
        }

        public void EvaluatePitchSlideNode(CompositeNode node)
        {
            var payload = node.Payload as PitchSlidePayload;

            foreach (SongNode songNode in payload!.Nodes)
            {
                if (songNode.NodeType == SongNodeType.Empty)
                {
                    InPitchSlide = true;
                }
                else if (songNode.NodeType == SongNodeType.Note)
                {
                    EvaluateNoteNode((AtomicNode)songNode, default, true);
                }
                else if (songNode.NodeType == SongNodeType.Rest)
                {
                    EvaluateRestNode((AtomicNode)songNode, default, true);
                }
                else if (songNode.NodeType == SongNodeType.Tie)
                {
                    EvaluateTieNode((AtomicNode)songNode, default, true);
                }
                else
                {
                    EvaluateNode(songNode);
                }
            }
        }

        public void EvaluateTriplet(CompositeNode node)
        {
            foreach (SongNode songNode in node.Children)
            {
                if (songNode.NodeType == SongNodeType.Note)
                {
                    EvaluateNoteNode((AtomicNode)songNode, true);
                }
                else if (songNode.NodeType == SongNodeType.Rest)
                {
                    EvaluateRestNode((AtomicNode)songNode, true);
                }
                else if (songNode.NodeType == SongNodeType.Tie)
                {
                    EvaluateTieNode((AtomicNode)songNode, true);
                }
                else
                {
                    EvaluateNode(songNode);
                }
            }
        }


        #endregion

        #region Special Directive Evaluators

        public void EvaluateSpecialDirective(DirectiveNode specialDirective)
        {
            switch (specialDirective.NodeType)
            {
                case SongNodeType.Asm:
                    EvaluateAsmNode(specialDirective);
                    break;
                case SongNodeType.Jsr:
                    EvaluateJsrNode(specialDirective);
                    break;
                default:
                    throw new Exception();
            }
        }

        public void EvaluateAsmNode(DirectiveNode asmNode)
        {
            // todo add logic to catch duplicates

            var sfxAsmPayload = asmNode.Payload as SfxAsmPayload;

            NamedAsmBlocks.Add(sfxAsmPayload.JsrLabelName, sfxAsmPayload.AsmContentText);

        }

        public void EvaluateJsrNode(DirectiveNode jsrNode)
        {
            // todo add logic to catch duplicates

            var sfxJsrPayload = jsrNode.Payload as SfxJsrPayload;

            AddDataToChannel(MagicNumbers.CommandValues.SfxJsrCommand);
            JsrPositionsAndNames.Add(DataChannel.Count, sfxJsrPayload.JsrLabelName);
            AddDataToChannel(0x00);
            AddDataToChannel(0x00);
        }

        #endregion

        #region Hex Command Evaluators

        public void EvaluateHexNode(HexNode hexNode)
        {
            switch (hexNode.CommandType)
            {
                case HexCommands.E0SfxPriority:
                    EvaluateE0SfxPriorityNode(hexNode);
                    break;
                default:
                    EvaluateGenericHexCommandNode(hexNode);
                    break;
            }
        }

        public void EvaluateGenericHexCommandNode(HexNode node)
        {
            AddDataToChannel(Convert.ToByte(node.HexCommand.Replace("$", ""), 16));

            foreach (var commandValue in node.HexValues)
            {
                AddDataToChannel(Convert.ToByte(commandValue.Replace("$", ""), 16));
            }
        }

        public void EvaluateE0SfxPriorityNode(HexNode e0SfxPriorityNode)
        {
            AddDataToChannel(Convert.ToByte(e0SfxPriorityNode.HexCommand.Replace("$", ""), 16));

            foreach (var commandValue in e0SfxPriorityNode.HexValues)
            {
                AddDataToChannel(Convert.ToByte(commandValue.Replace("$", ""), 16));
            }
        }


        #endregion


        #endregion

        #region Node Validators

        #region Atomic Node Validators

        public IValidationResult ValidateAtomicNode(AtomicNode atomic)
        {
            return atomic.NodeType switch
            {
                // Always Accepted
                SongNodeType.Note or
                SongNodeType.Rest or
                SongNodeType.Tie or
                SongNodeType.LowerOctave or
                SongNodeType.RaiseOctave or
                SongNodeType.Octave or
                SongNodeType.Pipe => new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success
                },
                // Requires Validation
                SongNodeType.DefaultLength => ValidateDefaultLengthNode(atomic),
                SongNodeType.SfxInstrument => ValidateSfxInstrumentNode(atomic),
                SongNodeType.SfxVolume => ValidateSfxVolumeNode(atomic),
                SongNodeType.Volume => throw new Exception(),
                SongNodeType.Instrument => throw new Exception(),
                _ => throw new Exception()
            };
        }

        public IValidationResult ValidateDefaultLengthNode(AtomicNode defaultLength)
        {
            var defaultLengthPayload = defaultLength.Payload as DefaultLengthPayload;

            if (defaultLengthPayload == null)
            {
                throw new Exception();
            }

            if (defaultLengthPayload.Length < 1 || defaultLengthPayload.Length > MagicNumbers.NoteLengthMaximum)
            {
                return new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = new List<string>() {
                        _messageService.GetDefaultLengthOutOfRangeMessage(1, MagicNumbers.NoteLengthMaximum, defaultLengthPayload.Length)
                    },
                };
            }

            var isFractionalTick = MagicNumbers.NoteLengthMaximum % defaultLengthPayload.Length != 0;

            if (isFractionalTick)
            {
                return new ValidationResult
                {
                    Type = ValidationResult.ResultType.Warning,
                    Message = new List<string>() {
                        _messageService.GetWarningDefaultLengthValidationMessage()
                    },
                };
            }
            else
            {
                return new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success,
                };
            }
        }

        public IValidationResult ValidateSfxInstrumentNode(AtomicNode instrument)
        {
            var sfxInstrumentPayload = instrument.Payload as SfxInstrumentPayload;

            if (sfxInstrumentPayload == null)
            {
                throw new Exception();
            }

            var messages = new List<string>();
            var instrumentNumber = sfxInstrumentPayload.InstrumentNumber;

            if(instrumentNumber < 0 ||  instrumentNumber > MagicNumbers.SfxInstrumentMaximum)
            {
                messages.Add(_messageService.GetErrorSfxInstrumentValueOutOfRangeMessage(0, MagicNumbers.SfxInstrumentMaximum, instrumentNumber));
            }

            if(sfxInstrumentPayload.NoiseHexValue.Length > 0)
            {
                var instrumentNoiseValue = Convert.ToByte(sfxInstrumentPayload.NoiseHexValue);
                if(instrumentNoiseValue < 0 || instrumentNoiseValue > MagicNumbers.NoiseMaximum)
                {
                    messages.Add(_messageService.GetErrorSfxInstrumentNoiseHexValueOutOfRangeMessage(0, MagicNumbers.NoiseMaximum, instrumentNoiseValue));
                }
            }

            return messages.Count > 0
                ? new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success,
                };
        }

        public IValidationResult ValidateSfxVolumeNode(AtomicNode volume)
        {
            var sfxVolumePayload = volume.Payload as SfxVolumePayload;

            if (sfxVolumePayload == null)
            {
                throw new Exception();
            }

            var volumeValue = sfxVolumePayload.Volume;
            var leftVolumeValue = sfxVolumePayload.LeftVolumeValue;
            var rightVolumeValue = sfxVolumePayload.RightVolumeValue;
            var messages = new List<string>();
            if(volumeValue == -1)
            {
                if(leftVolumeValue < -1  || leftVolumeValue > MagicNumbers.SfxVolumeMaximum)
                {
                    messages.Add(_messageService.GetErrorSfxVolumeLeftVolumeValueOutOfRangeMessage(0, MagicNumbers.SfxVolumeMaximum, leftVolumeValue));
                }
                if(rightVolumeValue < -1 || rightVolumeValue > MagicNumbers.SfxVolumeMaximum)
                {
                    messages.Add(_messageService.GetErrorSfxVolumeRightVolumeValueOutOfRangeMessage(0, MagicNumbers.SfxVolumeMaximum, rightVolumeValue));
                }
            }
            else
            {
                if(volumeValue < 0 || volumeValue > MagicNumbers.SfxVolumeMaximum)
                {
                    messages.Add(_messageService.GetErrorSfxVolumeVolumeValueOutOfRangeMessage(0, MagicNumbers.SfxVolumeMaximum, volumeValue));
                }
            }

            return messages.Count > 0
                ? new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success,
                };
        }


        #endregion

        #region Composite Node Validators

        public IValidationResult ValidateCompositeNode(CompositeNode composite)
        {
            return composite.NodeType switch
            {
                SongNodeType.Triplet => ValidateTripletNode(composite),
                SongNodeType.PitchSlide => ValidatePitchSlideNode(composite),
                SongNodeType.HexCommand => ValidateHexCommand(composite),
                _ => throw new Exception()
            }; ;
        }

        public IValidationResult ValidatePitchSlideNode(CompositeNode pitchSlideNode)
        {
            var messages = new List<string>();
            foreach (SongNode child in pitchSlideNode.Children)
            {
                // is this the "&" node? if so skip
                if (child.NodeType == SongNodeType.Empty)
                {
                    continue;
                }
                var validationResult = (ValidationResult)ValidateNode(child);
                if (validationResult.Type != ValidationResult.ResultType.Success)
                {
                    messages.AddRange(validationResult.Message);
                }
            }
            return messages.Count > 0
                ? new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success
                };
        }

        public IValidationResult ValidateHexCommand(CompositeNode hexCommand)
        {
            var hexPayload = hexCommand.Payload as HexNumberPayload;

            if (hexPayload == null)
            {
                throw new Exception();
            }

            var hexByte = byte.Parse(hexPayload.HexValue);

            if (hexByte >= MagicNumbers.HexCommandMaximum)
            {
                return new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = new List<string>
                    {
                        _messageService.GetErrorUnknownHexCommandMessage(hexPayload.HexValue),
                    }
                };
            }

            if (!Helpers.Helpers.IsHexInRange(hexByte))
            {
                return new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = new List<string>
                    {
                        _messageService.GetErrorHexCommandValueOutOfRangeMessage(hexPayload.HexValue, 0, MagicNumbers.HexCommandMaximum),
                    }
                };
            }

            return new ValidationResult
            {
                Type = ValidationResult.ResultType.Success,
            };
        }

        public IValidationResult ValidateTripletNode(CompositeNode tripletNode)
        {
            var messages = new List<string>();
            foreach (SongNode child in tripletNode.Children)
            {
                var validationResult = (ValidationResult)ValidateNode(child);
                if (validationResult.Type != ValidationResult.ResultType.Success)
                {
                    messages.AddRange(validationResult.Message);
                }
            }
            return messages.Count > 0
                ? new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success
                };
        }


        #endregion

        #region Special Directive Validators

        public IValidationResult ValidateSpecialDirective(DirectiveNode specialDirective)
        {
            return specialDirective.NodeType switch
            {
                // Always Accepted
                SongNodeType.Asm or
                SongNodeType.Jsr => new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success
                },
                _ => throw new Exception()
            };
        }

        #endregion

        #region Hex Command Validators

        public IValidationResult ValidateHexNode(HexNode hex)
        {
            return hex.CommandType switch
            {
                HexCommands.E0SfxPriority => ValidateGenericHexCommand(hex),
                _ => ValidateGenericHexCommand(hex)
            };
        }

        public IValidationResult ValidateGenericHexCommand(HexNode hex)
        {
            var messages = new List<string>();

            foreach (var hexNumber in hex.HexValues)
            {
                var byteValue = Convert.ToByte(hexNumber.Replace("$", ""));
                if (!Helpers.Helpers.IsHexInRange(byteValue))
                {
                    messages.Add(_messageService.GetErrorHexCommandSuppliedValueOutOfRangeMessage(hexNumber, hex.HexCommand, 0, MagicNumbers.HexCommandMaximum));
                }
            }

            return messages.Count > 0
                ? new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success,
                };
        }

        #endregion


        #endregion


        #region Helpers

        private void UpdateAndAddVolume()
        {
            if (UpdateVolume == true)
            {
                AddDataToChannel((byte)LeftVolume);
                if (LeftVolume != RightVolume)
                {
                    AddDataToChannel((byte)RightVolume);
                }
                UpdateVolume = false;
            }
        }

        private void AddDataToChannel(byte dataToAdd)
        {
            DataChannel.Add(dataToAdd);
        }


        private int GetNoteLength(SongNode node, int noteLength, int dotCount, bool inTriplet, bool allowTriplet)
        {
            var version = false;
            var length = noteLength;
            if (version == true)
            {
                return noteLength;
            }
            else if (noteLength < 1 || noteLength > MagicNumbers.NoteLengthMaximum)
            {
                length = DefaultNoteLength;
            }
            else
            {
                if (MagicNumbers.NoteLengthMaximum % noteLength == 0)
                {
                    _messageService.GetWarningNoteLengthFractionalTickValueMessage(MagicNumbers.NoteLengthMaximum, SoundEffectData.Name, node.LineNumber, node.ColumnNumber);
                }
                length = MagicNumbers.NoteLengthMaximum / noteLength;
            }

            return GetNoteLengthModifier(node, length, dotCount, inTriplet, allowTriplet);
        }

        private int GetNoteLengthModifier(SongNode node, int noteLength, int dotCount, bool inTriplet, bool allowTriplet)
        {
            int result = noteLength;
            int fraction = noteLength;

            for (int i = 0; i < dotCount; i++)
            {
                if (fraction % 2 != 0)
                {
                    if (i != 0)
                    {
                        _messageService.GetWarningFractionalTickValueFromDotsMessage(i + 1, SoundEffectData.Name, node.LineNumber, node.ColumnNumber);
                    }
                    else
                    {
                        _messageService.GetWarningFractionalTickValueFromDotsMessage(1, SoundEffectData.Name, node.LineNumber, node.ColumnNumber);
                    }
                }

                fraction /= 2;
                result += fraction;
            }

            if (inTriplet && allowTriplet == true)
            {
                if (fraction % 3 != 0)
                {
                    _messageService.GetWarningTripletFractionalTickValueFromDotsMessage(SoundEffectData.Name, node.LineNumber, node.ColumnNumber);
                }
                result = (int)Math.Floor(result * 2.0 / 3.0 + 0.5);
            }
            return result;
        }

        private int GetPitchValue(int value, NotePayload.Accidentals accidental)
        {
            value = MagicNumbers.ValidPitches[value - MagicNumbers.PitchOffset] + (CurrentOctave - 1) * 12 + 0x80;

            if (accidental == NotePayload.Accidentals.Sharp)
            {
                value++;
            }
            else
            {
                value--;
            }

            return value;
        }

        #endregion

    }
}
