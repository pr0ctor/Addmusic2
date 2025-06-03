using Addmusic2.Model;
using Addmusic2.Model.Interfaces;
using Addmusic2.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Addmusic2.Model.SongTree;
using Addmusic2.Validators;
using Microsoft.Extensions.Logging;
using Addmusic2.Model.Localization;
using Addmusic2.Localization;

namespace Addmusic2.Logic
{
    internal class SongParser : ISongParser
    {
        private readonly ILogger<IAddmusicLogic> _logger;
        private readonly MessageService _messageService;

        public SongData SongData { get; set; } = new SongData();

        private List<byte> CurrentChannelData = new List<byte>();
        private List<byte> CurrentLoopData = new List<byte>();
        private List<byte> CurrentSubLoopData = new List<byte>();
        private Dictionary<int, double> ChannelLength = new();
        private Dictionary<string, LoopData> RemoteCodeDefinitions = new();
        private Dictionary<string, LoopData> NamedLoopDefinitions = new();

        private int DefaultNoteLength { get; set; } = MagicNumbers.DefaultValues.InitialDefaultNoteLength;
        private int CurrentOctave { get; set; } = MagicNumbers.DefaultValues.StartingOctave;
        private int CurrentChannel { get; set; } = 0;
        private int TempoRatio { get; set; } = MagicNumbers.DefaultValues.InitialTempoRatio;
        private int HTranspose { get; set; } = 0;
        private bool UsingHTranspose { get; set; } = false;
        private bool TempoDefined { get; set; } = false;
        private bool InActiveLoop { get; set; } = false;
        private double ActiveLoopLength { get; set; } = 0;
        private bool InActiveSubLoop { get; set; } = false;
        private double ActiveSubLoopLength { get; set; } = 0;
        private bool InActiveSimpleLoop { get; set; } = false;
        private bool InActiveSuperLoop { get; set; } = false;

        private LoopNode PreviousLoop { get; set; }

        public SongParser(ILogger<IAddmusicLogic> logger, MessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }

        public SongData ParseSongNodes(List<ISongNode> nodes)
        {
            CatalogueUserDefinedInformation(nodes);

            var channels = nodes
                .Where(n => ((SongNode)n).NodeType == SongNodeType.Channel)
                .ToList();
            var specialDirectives = nodes
                .Where(n =>
                    ((SongNode)n).NodeType != SongNodeType.Channel &&
                    n.GetType() == typeof(DirectiveNode)
                ).ToList();

            // Parse the SpecialDirectives before anything else

            foreach (SongNode directive in specialDirectives)
            {
                ParseNode(directive);
            }

            /*var instruments = specialDirectives
                .Where(n => n.NodeType == SongNodeType.Instruments)
                .ToList();
            var samples = specialDirectives
                .Where(n => n.NodeType == SongNodeType.Samples)
                .ToList();*/

            // Parse the other nodes before the Channel data

            var otherNodes = nodes
                .Except(specialDirectives)
                .Except(channels)
                .ToList();

            foreach (SongNode node in otherNodes)
            {
                ParseNode(node);
            }

            // Parse the Channel data 

            foreach (SongNode node in channels)
            {
                ParseNode(node);
            }

            return SongData;
        }

        public void ParseChannel(DirectiveNode channel)
        {
            var channelPayload = channel.Payload as ChannelPayload;

            if (channelPayload == null)
            {
                throw new Exception();
            }

            CurrentChannel = channelPayload.ChannelNumber;

            if (!SongData.ChannelData.ContainsKey(channelPayload.ChannelNumber))
            {
                SongData.ChannelData[channelPayload.ChannelNumber] = new();
            }

            foreach (SongNode node in channel.Children)
            {
                ParseNode(node);
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
                // handle failure cases
            }
            else if (validationResult.Type == ValidationResult.ResultType.Warning ||
                validationResult.Type == ValidationResult.ResultType.Error)
            {
                // handle error cases
            }

            EvaluateNode(node);
        }

        public IValidationResult ValidateNode(ISongNode songNode)
        {
            return songNode switch
            {
                DirectiveNode => ValidateSpecialDirective(songNode as DirectiveNode),
                AtomicNode => ValidateAtomicNode(songNode as AtomicNode),
                CompositeNode => ValidateCompositeNode(songNode as CompositeNode),
                LoopNode => ValidateLoopNode(songNode as LoopNode),
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
                    EvaluateLoopNode(songNode as LoopNode);
                    break;
                default:
                    throw new Exception();
            }
        }

        public void CatalogueUserDefinedInformation(List<ISongNode> nodes)
        {
            // Get the names and associate loops of the named loops in the tree
            // Get the definition names of the remote code definitions in the tree
            ushort loopPointer = 0x0000;
            foreach (SongNode node in nodes)
            {
                if(loopPointer >= MagicNumbers.SixteenBitMaximum)
                {
                    _messageService.GetErrorMaximumAllowedNumberOfLoopsReachedMessage();
                }

                if (node.GetType() == typeof(LoopNode))
                {
                    if (node.NodeType == SongNodeType.SimpleLoop)
                    {
                        var loopName = ((LoopNode)node).LoopName;
                        var hasLoopName = (loopName.Length > 0) ? true : false;
                        if (hasLoopName)
                        {
                            if (NamedLoopDefinitions.ContainsKey(loopName))
                            {
                                _messageService.GetErrorDuplicateLoopNameDefinedMessage(loopName);
                            }
                            NamedLoopDefinitions.Add(loopName, new LoopData
                            {
                                LoopId = loopPointer++,
                                LoopNode = (LoopNode)node,
                            });
                        }

                        CatalogueUserDefinedInformation(((LoopNode)node).LoopContents);
                    }
                    else if (node.NodeType == SongNodeType.RemoteCode)
                    {
                        var payload = node.Payload as RemoteCodeDefinitionPayload;
                        var definitionName = payload.DefinitionName;
                        if (NamedLoopDefinitions.ContainsKey(definitionName))
                        {
                            _messageService.GetErrorDuplicateRemoteCodeDefinitionNameDefinedMessage(definitionName);
                        }
                        RemoteCodeDefinitions.Add(definitionName, new LoopData
                        {
                            LoopId = loopPointer++,
                            LoopNode = (LoopNode)node,
                        });
                    }
                }
            }
        }

        #region Node Evalulators

        #region Atomic Node Evaluators

        public void EvaluateAtomicNode(AtomicNode atomic)
        {
            switch (atomic.NodeType)
            {
                // Always Accepted
                case SongNodeType.Note:
                case SongNodeType.Rest:
                case SongNodeType.Tie:
                case SongNodeType.NoLoopCommand:
                case SongNodeType.QuestionMark:
                    EvaluateQuestionMarkNode(atomic);
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
                case SongNodeType.Instrument:
                    (atomic);
                    break;
                case SongNodeType.Volume:
                    EvaluateVolumeNode(atomic);
                    break;
                case SongNodeType.GlobalVolume:
                    EvaluateGlobalVolumeNode(atomic);
                    break;
                case SongNodeType.Pan:
                    EvaluatePanNode(atomic);
                    break;
                case SongNodeType.Quantization:
                    (atomic);
                    break;
                case SongNodeType.Tempo:
                    EvaluateTempoNode(atomic);
                    break;
                case SongNodeType.Vibrato:
                    EvaluateVibratoNode(atomic);
                    break;
                case SongNodeType.Noise:
                    EvaluateNoiseNode(atomic);
                    break;
                case SongNodeType.Tune:
                    EvaluateTuneNode(atomic);
                    break;
                default:
                    throw new Exception();
            }

            /*var x = atomic.NodeType switch
            {
                // Always Accepted
                SongNodeType.Note or
                SongNodeType.Rest or
                SongNodeType.Tie or
                SongNodeType.NoLoopCommand or
                SongNodeType.QuestionMark => EvaluateQuestionMarkNode(atomic),
                SongNodeType.LowerOctave => EvaluateLowerOctaveNode(atomic),
                SongNodeType.RaiseOctave => EvaluateRaiseOctaveNode(atomic),
                SongNodeType.Octave => EvaluateOctaveNode(atomic),
                // Requires Validation
                SongNodeType.DefaultLength => EvaluateDefaultLengthNode(atomic),
                SongNodeType.Instrument => (atomic),
                SongNodeType.Volume => EvaluateVolumeNode(atomic),
                SongNodeType.GlobalVolume => EvaluateGlobalVolumeNode(atomic),
                SongNodeType.Pan => EvaluatePanNode(atomic),
                SongNodeType.Quantization => (atomic),
                SongNodeType.Tempo => EvaluateTempoNode(atomic),
                SongNodeType.Vibrato => EvaluateVibratoNode(atomic),
                SongNodeType.Noise => EvaluateNoiseNode(atomic),
                SongNodeType.Tune => EvaluateTuneNode(atomic),
                _ => throw new Exception()
            };*/
        }

        public void EvaluateRestNode(AtomicNode restNode)
        {
            var restPayload = restNode.Payload as NotePayload;

            var tempLength = GetNoteLength(restNode, restPayload.Duration, restPayload.DotCount);

        }

        public void EvaluateDefaultLengthNode(AtomicNode defaultLengthNode)
        {
            var defaultLengthPayload = defaultLengthNode.Payload as DefaultLengthPayload;

            if (defaultLengthPayload == null)
            {
                throw new Exception();
            }

            if (defaultLengthPayload.UsedEquals)
            {
                DefaultNoteLength = defaultLengthPayload.Length;
            }
            else
            {
                DefaultNoteLength = MagicNumbers.NoteLengthMaximum / defaultLengthPayload.Length;
            }

            // todo check logic here
            //DefaultNoteLength = 
        }

        public void EvaluateGlobalVolumeNode(AtomicNode globalVolumeNode)
        {
            var globalVolumePayload = globalVolumeNode.Payload as VolumePayload;

            if (globalVolumePayload == null)
            {
                throw new Exception();
            }

            if (globalVolumePayload.FadeValue == -1)
            {
                AddDataToChannel(MagicNumbers.CommandValues.GlobalVolume);
                AddDataToChannel(Convert.ToByte(globalVolumePayload.Volume));
            }
            else
            {
                AddDataToChannel(MagicNumbers.CommandValues.GlobalVolumeWithFade);
                AddDataToChannel(Convert.ToByte(DivideByTempoRatio(globalVolumeNode, globalVolumePayload.FadeValue, false)));
                AddDataToChannel(Convert.ToByte(globalVolumePayload.Volume));
            }
        }

        public void EvaluateVolumeNode(AtomicNode volumeNode)
        {
            var volumePayload = volumeNode.Payload as VolumePayload;

            if (volumePayload == null)
            {
                throw new Exception();
            }

            if (volumePayload.FadeValue == -1)
            {
                AddDataToChannel(MagicNumbers.CommandValues.Volume);
                AddDataToChannel(Convert.ToByte(volumePayload.Volume));
            }
            else
            {
                AddDataToChannel(MagicNumbers.CommandValues.VolumeWithFade);
                AddDataToChannel(Convert.ToByte(DivideByTempoRatio(volumeNode, volumePayload.FadeValue, false)));
                AddDataToChannel(Convert.ToByte(volumePayload.Volume));
            }
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

        public void EvaluateNoiseNode(AtomicNode noiseNode)
        {
            var noisePayload = noiseNode.Payload as NoisePayload;

            if (noisePayload == null)
            {
                throw new Exception();
            }

            AddDataToChannel(MagicNumbers.CommandValues.Noise);
            AddDataToChannel(Convert.ToByte(noisePayload.NoiseValue));
        }

        public void EvaluatePanNode(AtomicNode panNode)
        {
            var panPayload = panNode.Payload as PanPayload;

            if (panPayload == null)
            {
                throw new Exception();
            }

            var panValue = panPayload.PanPosition;
            var panRight = panPayload.SurroundSoundRight;
            var panLeft = panPayload.SurroundSoundLeft;

            if (panLeft != -1)
            {
                panValue |= (panLeft << 7);
            }

            if (panRight != -1)
            {
                panValue |= (panRight << 6);
            }

            AddDataToChannel(MagicNumbers.CommandValues.Pan);
            AddDataToChannel(Convert.ToByte(panValue));

        }

        public void EvaluateQuestionMarkNode(AtomicNode questionMarkNode)
        {
            if (questionMarkNode.NodeType == SongNodeType.NoLoopCommand)
            {
                SongData.DoesntLoop = true;
                return;
            }

            var questionMarkPayload = questionMarkNode.Payload as QuestionMarkPayload;

            if (questionMarkPayload == null)
            {
                throw new Exception();
            }

            if (questionMarkPayload.MarkNumber == 0)
            {
                SongData.DoesntLoop = true;
            }
            else if (questionMarkPayload.MarkNumber == 1)
            {
                SongData.NoMusic[CurrentChannel, 0] = true;
            }
            else if (questionMarkPayload.MarkNumber == 2)
            {
                SongData.NoMusic[CurrentChannel, 1] = true;
            }
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

        public void EvaluateTempoNode(AtomicNode tempoNode)
        {
            var tempoPayload = tempoNode.Payload as TempoPayload;

            if (tempoPayload == null)
            {
                throw new Exception();
            }

            var tempoValue = tempoPayload.Tempo;
            var tempoDuration = tempoPayload.FadeValue;

            var tempo = DivideByTempoRatio(tempoNode, tempoValue, false);

            if (tempo == 0)
            {
                // message
                tempo = tempoValue;
            }

            if (tempoDuration == -1)
            {
                TempoDefined = true;

                if (InActiveLoop)
                {
                    SongData.GuessLength = false;
                }
                else
                {
                    // calculate tempo change
                }

                AddDataToChannel(MagicNumbers.CommandValues.Tempo);
                AddDataToChannel(Convert.ToByte(tempo));
            }
            else
            {
                SongData.GuessLength = false;
                AddDataToChannel(MagicNumbers.CommandValues.TempoWithFade);
                AddDataToChannel(Convert.ToByte(DivideByTempoRatio(tempoNode, tempoDuration, false)));
                AddDataToChannel(Convert.ToByte(tempo));
            }
        }

        public void EvaluateTuneNode(AtomicNode tuneNode)
        {
            var tunePayload = tuneNode.Payload as TunePayload;

            if (tunePayload == null)
            {
                throw new Exception();
            }

            HTranspose = tunePayload.TuneValue;
            UsingHTranspose = true;
        }

        public void EvaluateVibratoNode(AtomicNode vibratoNode)
        {
            var vibratoPayload = vibratoNode.Payload as VibratoPayload;

            if (vibratoPayload == null)
            {
                throw new Exception();
            }

            if (vibratoPayload.DelayDurationValue == -1)
            {
                AddDataToChannel(MagicNumbers.CommandValues.Vibrato);
                AddDataToChannel(Convert.ToByte(00));
                AddDataToChannel(Convert.ToByte(MultiplyByTempoRatio(vibratoNode, vibratoPayload.RateValue)));
                AddDataToChannel(Convert.ToByte(vibratoPayload.ExtentValue));
            }
            else
            {
                AddDataToChannel(MagicNumbers.CommandValues.Vibrato);
                AddDataToChannel(Convert.ToByte(DivideByTempoRatio(vibratoNode, vibratoPayload.DelayDurationValue, false)));
                AddDataToChannel(Convert.ToByte(MultiplyByTempoRatio(vibratoNode, vibratoPayload.RateValue)));
                AddDataToChannel(Convert.ToByte(vibratoPayload.ExtentValue));
            }

        }

        #endregion

        #region Composite Node Evaluators

        #endregion

        #region Loop Node Evaluators

        public void EvaluateLoopNode(LoopNode loopNode)
        {
            switch (loopNode.NodeType)
            {
                case SongNodeType.SimpleLoop:
                    EvaluateSimpleLoopNode(loopNode);
                    break;
                case SongNodeType.SuperLoop:
                    EvaluateSuperLoopNode(loopNode);
                    break;
                case SongNodeType.RemoteCode:
                    EvaluateRemoteCodeDefinitionNode(loopNode);
                    break;
                case SongNodeType.CallLoop:
                    EvaluateCallLoopDefinitionNode(loopNode);
                    break;
                case SongNodeType.CallPreviousLoop:
                    EvaluateCallPreviousLoopNode(loopNode);
                    break;
                case SongNodeType.CallRemoteCode:
                    EvaluateCallRemoteCodeNode(loopNode);
                    break;
                case SongNodeType.StopRemoteCode:
                    EvaluateStopRemoteCodeNode(loopNode);
                    break;
                default:
                    throw new Exception();
            }
        }

        public void EvaluateStopRemoteCodeNode(LoopNode loopNode)
        {
            var eventType = int.Parse(loopNode.LoopName);

            switch (eventType)
            {
                case 0:
                    AddDataToChannel(MagicNumbers.CommandValues.RemoteCode);
                    AddDataToChannel(0x00);
                    AddDataToChannel(0x00);
                    AddDataToChannel(0x00);
                    AddDataToChannel(0x00);
                    break;
                case -1:
                    AddDataToChannel(MagicNumbers.CommandValues.RemoteCode);
                    AddDataToChannel(0x00);
                    AddDataToChannel(0x00);
                    AddDataToChannel(0x08);
                    AddDataToChannel(0x00);
                    break;
                default:
                    AddDataToChannel(MagicNumbers.CommandValues.RemoteCode);
                    AddDataToChannel(0x00);
                    AddDataToChannel(0x00);
                    AddDataToChannel(0x07);
                    AddDataToChannel(0x00);
                    break;
            }
        }

        public void EvaluateCallRemoteCodeNode(LoopNode callRemoteCodeNode)
        {
            var remoteCodePayload = callRemoteCodeNode.Payload as CallRemoteCodePayload;

            if (remoteCodePayload == null)
            {
                throw new Exception();
            }

            var definitionName = remoteCodePayload.DefinitionName;
            if (!RemoteCodeDefinitions.ContainsKey(definitionName))
            {
                _messageService.GetErrorUndefinedRemoteCodeCallMessage();
            }
            var eventNumber = remoteCodePayload.EventType;

            var databyte = 0;
            if (eventNumber == 1 || eventNumber == 2)
            {
                if (remoteCodePayload.IntArgument == -1)
                {
                    databyte = byte.Parse(remoteCodePayload.HexArgument);
                }
                else
                {
                    databyte = Convert.ToByte(GetNoteLengthModifier(callRemoteCodeNode, remoteCodePayload.IntArgument, 0, false, false));
                    if (databyte > MagicNumbers.HexCommandMaximum)
                    {
                        _messageService.GetErrorHexCommandValueOutOfRangeMessage(remoteCodePayload.HexArgument, 0, MagicNumbers.HexCommandMaximum);
                        // todo handle error
                    }
                    /*else if(databyte == MagicNumbers.HexCommandMaximum)
                    {
                        databyte = 0;
                    }*/
                }
            }

            var remoteCodeLocation = RemoteCodeDefinitions[definitionName].LoopId;
            AddDataToChannel(MagicNumbers.CommandValues.RemoteCode);
            // todo update loop locations
            AddDataToChannel((byte)(remoteCodeLocation & MagicNumbers.HexCommandMaximum));
            AddDataToChannel((byte)(remoteCodeLocation >> 8));
            AddDataToChannel(Convert.ToByte(eventNumber));
            AddDataToChannel(Convert.ToByte(databyte));
        }

        public void EvaluateCallPreviousLoopNode(LoopNode callPreviousLoopNode)
        {
            // clone the loop because this call could have a different iteration count than the original definition
            var previousNode = (LoopNode)PreviousLoop.Clone();
            previousNode.Iterations = callPreviousLoopNode.Iterations;

            EvaluateSimpleLoopNode(previousNode);
        }

        public void EvaluateSimpleLoopNode(LoopNode simpleLoopNode)
        {
            // Begin Simple Loop
            var loopName = simpleLoopNode.LoopName;
            if (InActiveLoop == true)
            {
                InActiveSubLoop = true;
            }
            InActiveLoop = true;
            InActiveSimpleLoop = true;

            // Evaluate the contents of the loop

            foreach (var node in simpleLoopNode.LoopContents)
            {
                EvaluateNode(node);
            }

            // Finish loop
            var loopLocation = NamedLoopDefinitions[loopName].LoopId;
            AddDataToChannel(MagicNumbers.CommandValues.Loop);
            // todo update loop locations
            AddDataToChannel((byte)(loopLocation & MagicNumbers.HexCommandMaximum));
            AddDataToChannel((byte)(loopLocation >> 8));
            AddDataToChannel((byte)(simpleLoopNode.Iterations - 1));
            
            // Clean up state and finish loop evaluation
            if (InActiveSubLoop == true)
            {
                InActiveSubLoop = false;
                ActiveSubLoopLength = 0;
            }
            else
            {
                InActiveLoop = false;
                ActiveLoopLength = 0;
            }
            InActiveSimpleLoop = false;
            PreviousLoop = simpleLoopNode;
        }

        public void EvaluateSuperLoopNode(LoopNode superLoopNode)
        {
            // Begin Super Loop
            if(InActiveLoop == true)
            {
                InActiveSubLoop = true;
            }
            InActiveLoop = true;
            InActiveSuperLoop = true;

            AddDataToChannel(MagicNumbers.CommandValues.SuperLoop);
            AddDataToChannel(0x00);

            // Evaluate the contents of the loop

            foreach (var node in superLoopNode.LoopContents)
            {
                EvaluateNode(node);
            }

            // Finish loop
            AddDataToChannel(MagicNumbers.CommandValues.SuperLoop);
            AddDataToChannel((byte)(superLoopNode.Iterations - 1));

            // Clean up state and finish loop evaluation
            if (InActiveSubLoop == true)
            {
                InActiveSubLoop = false;
                ActiveSubLoopLength = 0;
            }
            else
            {
                InActiveLoop = false;
                ActiveLoopLength = 0;
            }
            InActiveSuperLoop = false;
            PreviousLoop = superLoopNode;
        }

        public void EvaluateCallLoopDefinitionNode(LoopNode callLoopNode)
        {

        }

        public void EvaluateRemoteCodeDefinitionNode(LoopNode remoteCodeDefinitionNode)
        {

        }

        #endregion

        #region Sepcial Directive Evaluators

        public void EvaluateSpecialDirective(DirectiveNode specialDirective)
        {
            switch (specialDirective.NodeType)
            {
                case SongNodeType.Amk:
                case SongNodeType.Channel:
                case SongNodeType.SPC:
                case SongNodeType.Instruments:
                case SongNodeType.Samples:
                case SongNodeType.Path:
                    // Skip these since they have already been processed
                    break;
                case SongNodeType.Pad:
                    EvaluatePadNode(specialDirective);
                    break;
                case SongNodeType.Halvetempo:
                    EvaluateHalveTempoNode(specialDirective);
                    break;
                case SongNodeType.Option:
                    (specialDirective);
                    break;
                case SongNodeType.OptionGroup:
                    (specialDirective);
                    break;
                default:
                    throw new Exception();
            }
            /*var x = specialDirective.NodeType switch
            {
                // Always Accepted
                SongNodeType.Amk or
                SongNodeType.Channel or
                SongNodeType.SPC or
                SongNodeType.Instruments or
                SongNodeType.Samples or
                SongNodeType.Path => true,
                // Requires Validation

                SongNodeType.Pad => EvaluatePadNode(specialDirective),
                SongNodeType.Halvetempo => EvaluateHalveTempoNode(specialDirective),
                SongNodeType.Option => EvaluatePadNode(specialDirective),
                SongNodeType.OptionGroup => EvaluatePadNode(specialDirective),
                _ => throw new Exception()
            };*/
        }

        public void EvaluatePadNode(DirectiveNode padNode)
        {
            var padPayload = padNode.Payload as PadPayload;
            var padAmount = int.Parse(padPayload.PadLength);
            SongData.MinSize = padAmount;
        }

        public void EvaluateHalveTempoNode(DirectiveNode halvetempoNode)
        {
            TempoRatio = MultiplyByTempoRatio(halvetempoNode, 2);
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
                SongNodeType.NoLoopCommand or
                SongNodeType.LowerOctave or
                SongNodeType.RaiseOctave or
                SongNodeType.Octave or
                SongNodeType.Tune => new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success
                },
                // Requires Validation
                SongNodeType.DefaultLength => ValidateDefaultLengthNode(atomic),
                SongNodeType.Instrument => ValidateInstrumentNode(atomic),
                SongNodeType.Volume or
                SongNodeType.GlobalVolume => ValidateVolumeNode(atomic),
                SongNodeType.Pan => ValidatePanNode(atomic),
                SongNodeType.Quantization => ValidateQuantizationNode(atomic),
                SongNodeType.Tempo => ValidateTempoNode(atomic),
                SongNodeType.Vibrato => ValidateVibratoNode(atomic),
                SongNodeType.Noise => ValidateNoiseNode(atomic),
                SongNodeType.QuestionMark => ValidateQuestionMarkNode(atomic),
                _ => throw new Exception()
            };
        }

        public IValidationResult ValidateDefaultLengthNode(AtomicNode defaultLength)
        {
            var defaultLengthPayload = defaultLength.Payload as DefaultLengthPayload;

            if(defaultLengthPayload == null)
            {
                throw new Exception();
            }
            
            if(defaultLengthPayload.Length < 1 || defaultLengthPayload.Length > MagicNumbers.NoteLengthMaximum)
            {
                return new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = new List<string>() {
                        _messageService.GetDefaultLengthOutOfRangeMessage(1, MagicNumbers.NoteLengthMaximum, defaultLengthPayload.Length)
                    },
                };
            }

            var isFractionalTick = (MagicNumbers.NoteLengthMaximum % defaultLengthPayload.Length) != 0;

            if(isFractionalTick)
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

        public IValidationResult ValidateVolumeNode(AtomicNode volume)
        {
            var volumePayload = volume.Payload as VolumePayload;

            if(volumePayload == null)
            {
                throw new Exception();
            }

            var fadeValue = volumePayload.FadeValue;
            var volumeValue = volumePayload.Volume;
            var messages = new List<string>();
            if(volumeValue < 0 || volumeValue > MagicNumbers.EightBitMaximum)
            {
                if(volume.NodeType == SongNodeType.Volume)
                {
                    messages.Add(_messageService.GetErrorVolumeVolumeValueOutOfRangeMessage(0, MagicNumbers.EightBitMaximum, volumeValue));
                }
                else
                {
                    messages.Add(_messageService.GetErrorGlobalVolumeVolumeValueOutOfRangeMessage(0, MagicNumbers.EightBitMaximum, volumeValue));
                }
            }
            if(fadeValue != -1 && (fadeValue < 0 || fadeValue > MagicNumbers.EightBitMaximum))
            {
                if(volume.NodeType == SongNodeType.Volume)
                {
                    messages.Add(_messageService.GetErrorVolumeFadeValueOutOfRangeMessage(0, MagicNumbers.EightBitMaximum, fadeValue));
                }
                else
                {
                    messages.Add(_messageService.GetErrorGlobalVolumeFadeValueOutOfRangeMessage(0, MagicNumbers.EightBitMaximum, fadeValue));
                }
            }

            return (messages.Count > 0)
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

        public IValidationResult ValidatePanNode(AtomicNode pan)
        {
            var panPayload = pan.Payload as PanPayload;

            if (panPayload == null)
            {
                throw new Exception();
            }

            if(panPayload.PanPosition < 0 || panPayload.PanPosition > MagicNumbers.PanDirectionMaximum)
            {
                return new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = new List<string>() {
                        _messageService.GetErrorPanDirectionOutOfRangeMessage(0, MagicNumbers.PanDirectionMaximum, panPayload.PanPosition)
                    },
                };
            }

            return new ValidationResult
            {
                Type = ValidationResult.ResultType.Success,
            };
        }

        public IValidationResult ValidateVibratoNode(AtomicNode vibrato)
        {
            var vibratoPayload = vibrato.Payload as VibratoPayload;

            if (vibratoPayload == null)
            {
                throw new Exception();
            }

            var delayValue = vibratoPayload.DelayDurationValue;
            var rateValue = vibratoPayload.RateValue;
            var extentValue = vibratoPayload.ExtentValue;
            var messages = new List<string>();
            if(delayValue != -1 && (delayValue < 0 || delayValue > MagicNumbers.EightBitMaximum))
            {
                messages.Add(_messageService.GetErrorVibratoDelayOutOfRangeMessage(0, MagicNumbers.EightBitMaximum, delayValue));
            }

            if (rateValue < 0 || rateValue > MagicNumbers.EightBitMaximum)
            {
                messages.Add(_messageService.GetErrorVibratoRateOutOfRangeMessage(0, MagicNumbers.EightBitMaximum, rateValue));
            }

            if (extentValue < 0 || extentValue > MagicNumbers.EightBitMaximum)
            {
                messages.Add(_messageService.GetErrorVibratoExtentOutOfRangeMessage(0, MagicNumbers.EightBitMaximum, extentValue));
            }

            return (messages.Count > 0) 
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

        public IValidationResult ValidateTempoNode(AtomicNode tempo)
        {
            var tempoPayload = tempo.Payload as TempoPayload;

            if (tempoPayload == null)
            {
                throw new Exception();
            }

            var tempoValue = tempoPayload.Tempo;
            var fadeValue = tempoPayload.FadeValue;
            var messages = new List<string>();

            if (tempoValue < 0 || tempoValue > MagicNumbers.EightBitMaximum)
            {
                messages.Add(_messageService.GetErrorTempoTempoValueOutOfRangeMessage(0, MagicNumbers.EightBitMaximum, tempoValue));
            }

            if (fadeValue != -1 && (fadeValue < 0 || fadeValue > MagicNumbers.EightBitMaximum))
            {
                messages.Add(_messageService.GetErrorTempoFadeValueOutOfRangeMessage(0, MagicNumbers.EightBitMaximum, fadeValue));
            }

            return (messages.Count > 0)
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

        public IValidationResult ValidateNoiseNode(AtomicNode noise)
        {
            var noisePayload = noise.Payload as NoisePayload;

            if (noisePayload == null)
            {
                throw new Exception();
            }

            var noiseValue = noisePayload.NoiseValue;
            var noiseHexValue = Convert.ToByte(noiseValue);

            if (noiseHexValue < 0 || noiseHexValue > MagicNumbers.NoiseMaximum)
            {
                return new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = new List<string>()
                    {
                        _messageService.GetErrorNoiseValueOutOfRangeMessage(0, MagicNumbers.NoiseMaximum, noiseHexValue),
                    }
                };
            }

            return new ValidationResult
            {
                Type = ValidationResult.ResultType.Success,
            };
        }

        public IValidationResult ValidateQuantizationNode(AtomicNode quantization)
        {
            var quantizationPayload = quantization.Payload as QuantizationPayload;

            if (quantizationPayload == null)
            {
                throw new Exception();
            }

            if(quantizationPayload.VolumeNode != null)
            {
                var volumeNodeValidation = (ValidationResult)ValidateVolumeNode(quantizationPayload.VolumeNode as AtomicNode);
                if(volumeNodeValidation.Type != ValidationResult.ResultType.Success)
                {
                    var messages = new List<string>();
                    messages.Add(_messageService.GetErrorQuantizationVolumeValueOutOfRangeMessage());
                    messages.AddRange(volumeNodeValidation.Message);
                    return new ValidationResult
                    {
                        Type = volumeNodeValidation.Type,
                        Message = messages,
                    };
                }
            }

            return new ValidationResult
            {
                Type = ValidationResult.ResultType.Success,
            };
        }

        public IValidationResult ValidateInstrumentNode(AtomicNode instrument)
        {

        }

        public IValidationResult ValidateQuestionMarkNode(AtomicNode questionMark)
        {
            var questionMarkPayload = questionMark.Payload as QuestionMarkPayload;

            if (questionMarkPayload == null)
            {
                throw new Exception();
            }

            return questionMarkPayload.MarkNumber switch
            {
                0 or 1 or 2 => new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success,
                },
                _ => new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = new List<string>
                    {
                        _messageService.GetErrorQuestionMarkValueOutOfRangeMessage(),
                    }
                }
            };
        }


        #endregion

        #region Composite Node Validators

        public IValidationResult ValidateCompositeNode(CompositeNode composite)
        {
            return composite.NodeType switch
            {
                // Always accepted
                SongNodeType.Intro => new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success
                },
                // Requires validation
                SongNodeType.Triplet => ValidateTripletNode(composite),
                SongNodeType.PitchSlide => ValidatePitchSlideNode(composite),
                SongNodeType.HexCommand => ValidateHexCommand(composite),
                SongNodeType.SampleLoad => ValidateSampleLoadNode(composite),
                
                _ => throw new Exception()
            }; ;
        }

        public IValidationResult ValidateTripletNode(CompositeNode tripletNode)
        {
            var messages = new List<string>();
            foreach(SongNode child in tripletNode.Children)
            {
                var validationResult = (ValidationResult)ValidateNote(child, true);
                if(validationResult.Type != ValidationResult.ResultType.Success)
                {
                    messages.AddRange(validationResult.Message);
                }
            }
            return (messages.Count > 0)
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

        public IValidationResult ValidatePitchSlideNode(CompositeNode pitchSlideNode)
        {
            var messages = new List<string>();
            foreach (SongNode child in pitchSlideNode.Children)
            {
                // is this the "&" node? if so skip
                if(child.NodeType == SongNodeType.Empty)
                {
                    continue;
                }
                var validationResult = (ValidationResult)ValidateNote(child, true);
                if (validationResult.Type != ValidationResult.ResultType.Success)
                {
                    messages.AddRange(validationResult.Message);
                }
            }
            return (messages.Count > 0)
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

            if(hexPayload == null)
            {
                throw new Exception();
            }

            var hexByte = byte.Parse(hexPayload.HexValue);

            if(hexByte >= MagicNumbers.HexCommandMaximum)
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

            if(hexByte < 0 || hexByte > MagicNumbers.HexCommandMaximum)
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

        public IValidationResult ValidateSampleLoadNode(CompositeNode sampleLoadNode)
        {
            var sampleloadPayload = sampleLoadNode.Payload as SampleLoadPayload;

            if(sampleloadPayload == null)
            {
                throw new Exception();
            }

            // check for sample name existence and is loaded

            var tuningValue = byte.Parse(sampleloadPayload.TuningValue);


            if(tuningValue < 0 || tuningValue > MagicNumbers.EightBitMaximum)
            {
                return new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = new List<string>
                    {
                        _messageService.GetErrorSampleLoadTuningValueOutOfRangeMessage(sampleloadPayload.TuningValue, 0, MagicNumbers.EightBitMaximum),
                    }
                };
            }

            return new ValidationResult
            {
                Type = ValidationResult.ResultType.Success,
            };
        }


        #endregion

        #region Loop Node Validators

        public IValidationResult ValidateLoopNode(LoopNode loop)
        {
            return loop.NodeType switch
            {
                // Always Accepted
                SongNodeType.StopRemoteCode => new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success
                },
                // Requires Validation
                SongNodeType.SimpleLoop or
                SongNodeType.SuperLoop => ValidateSimpleOrSuperLoopNode(loop),
                SongNodeType.RemoteCode => ValidateRemoteCodeNode(loop),
                SongNodeType.CallLoop => ValidateCallLoopNode(loop),
                SongNodeType.CallPreviousLoop => ValidateCallPreviousLoopNode(loop),
                SongNodeType.CallRemoteCode => ValidateCallRemoteCodeNode(loop),
                _ => throw new Exception()
            };
        }

        public IValidationResult ValidateSimpleOrSuperLoopNode(LoopNode loop)
        {
            var messages = new List<string>();

            var validIterations = CheckLoopIterations(loop, 1, MagicNumbers.EightBitMaximum);
            if (!validIterations)
            {
                messages.Add(_messageService.GetWarningLoopIterationOutOfRangeMessage());
            }

            foreach (var node in loop.Children)
            {
                var nodeValidation = (ValidationResult)ValidateNode(node);
                if(nodeValidation.Type == ValidationResult.ResultType.Failure ||
                    nodeValidation.Type == ValidationResult.ResultType.Error ||
                    nodeValidation.Type == ValidationResult.ResultType.Warning)
                {
                    messages.AddRange(nodeValidation.Message);
                }
            }

            return (messages.Count > 0)
                ? new ValidationResult
                {
                    Type = ValidationResult.ResultType.Warning,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success,
                };
        }

        public IValidationResult ValidateRemoteCodeNode(LoopNode remoteCode)
        {
            var remoteCodePayload = remoteCode.Payload as RemoteCodeDefinitionPayload;

            if (remoteCodePayload == null)
            {
                throw new Exception();
            }

            var messages = new List<string>();
            foreach (var node in remoteCode.Children)
            {
                var nodeValidation = (ValidationResult)ValidateNode(node);
                if (nodeValidation.Type == ValidationResult.ResultType.Failure ||
                    nodeValidation.Type == ValidationResult.ResultType.Error ||
                    nodeValidation.Type == ValidationResult.ResultType.Warning)
                {
                    messages.AddRange(nodeValidation.Message);
                }
            }

            return (messages.Count > 0)
                ? new ValidationResult
                {
                    Type = ValidationResult.ResultType.Warning,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success,
                };
        }

        public IValidationResult ValidateCallLoopNode(LoopNode callLoopNode)
        {
            var messages = new List<string>();
            if(!NamedLoopDefinitions.ContainsKey(callLoopNode.LoopName))
            {
                return new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = new List<string>
                    {
                        _messageService.GetErrorUndefinedNamedLoopCallMessage(),
                    },
                };
            }

            var validIterations = CheckLoopIterations(callLoopNode, 1, MagicNumbers.EightBitMaximum);
            if (!validIterations)
            {
                messages.Add(_messageService.GetWarningLoopIterationOutOfRangeMessage());
            }

            return (messages.Count > 0)
                ? new ValidationResult
                {
                    Type = ValidationResult.ResultType.Warning,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success,
                };
        }

        public IValidationResult ValidateCallPreviousLoopNode(LoopNode callPreviousLoopNode)
        {
            var messages = new List<string>();

            if(PreviousLoop == null)
            {
                return new ValidationResult
                {
                    Type = ValidationResult.ResultType.Warning,
                    Message = new List<string>
                    {
                        // todo get/generate message for this warning
                    },
                };
            }

            // This might be able to be considered an enhacement???
            if(PreviousLoop.NodeType == SongNodeType.SuperLoop)
            {
                return new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = new List<string>
                    {
                        // todo get/generate message for this error
                    },
                };
            }

            var validIterations = CheckLoopIterations(callPreviousLoopNode, 1, MagicNumbers.EightBitMaximum);
            if(!validIterations)
            {
                messages.Add(_messageService.GetWarningLoopIterationOutOfRangeMessage());
            }

            return (messages.Count > 0)
                ? new ValidationResult
                {
                    Type = ValidationResult.ResultType.Warning,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success,
                };
        }

        public IValidationResult ValidateCallRemoteCodeNode(LoopNode callRemoteCodeNode)
        {
            var remoteCodeCallPayload = callRemoteCodeNode.Payload as CallRemoteCodePayload;

            if (remoteCodeCallPayload == null)
            {
                throw new Exception();
            }

            if (!RemoteCodeDefinitions.ContainsKey(remoteCodeCallPayload.DefinitionName))
            {
                return new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = new List<string>
                    {
                        _messageService.GetErrorUndefinedNamedLoopCallMessage(),
                    },
                };
            }
            var messages = new List<string>();

            // do more processing for the various remote code types

            return (messages.Count > 0)
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

        #region Special Directive Validators

        public IValidationResult ValidateSpecialDirective(DirectiveNode specialDirective)
        {
            return specialDirective.NodeType switch
            {
                // Always Accepted
                SongNodeType.Amk or
                SongNodeType.Pad or
                SongNodeType.Halvetempo or
                SongNodeType.Channel or
                SongNodeType.Option or
                SongNodeType.OptionGroup or
                SongNodeType.Path => new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success
                },
                // Requires Validation
                SongNodeType.SPC => ValidateAndProcessSpcDirectiveNode(specialDirective),
                SongNodeType.Instruments => ValidateAndProcessInstrumentDirectiveNode(specialDirective),
                SongNodeType.Samples => ValidateAndProcessSamplesDirectiveNode(specialDirective),
                _ => throw new Exception()
            };
        }

        public IValidationResult ValidateAndProcessSpcDirectiveNode(DirectiveNode spc)
        {
            var spcPayload = spc.Payload as SpcPayload;

            if (spcPayload == null)
            {
                throw new Exception();
            }

            var title = spcPayload.Title;
            var author = spcPayload.Author;
            var game = spcPayload.Game;
            var comment = spcPayload.Comment;
            var length = spcPayload.Length;

            if(game.Length == 0)
            {
                SongData.Game = Messages.DefaultSpcGameName;
            }
            if(length == "auto")
            {
                // todo stuff here
            }
            else if(length.Length > 0)
            {
                if (!length.Contains(":"))
                {
                    return new ValidationResult
                    {
                        Type = ValidationResult.ResultType.Error,
                        Message = new List<string>()
                        {
                            _messageService.GetErrorSpcLengthInvalidValueMessage(),
                        }
                    };
                }
                else
                {
                    var lengthTime = length.Split(":").ToList();
                    var mins = int.Parse(lengthTime[0]);
                    var secs = int.Parse(lengthTime[1]);
                    var totalSeconds = (mins * 60) + secs;

                    if(totalSeconds > 999)
                    {
                        return new ValidationResult
                        {
                            Type = ValidationResult.ResultType.Error,
                            Message = new List<string>()
                            {
                                _messageService.GetErrorSpcLengthValueTooLongMessage(),
                            }
                        };
                    }

                    SongData.Seconds = (uint)(totalSeconds & 0xFFFFFF);
                }
            }

            var messages = new List<string>();
            if(author.Length > MagicNumbers.SpcTextMaximumLength)
            {
                messages.Add(_messageService.GetWarningSpcTextValueTooLongMessage(nameof(author), author[0..32]));
            }
            if (game.Length > MagicNumbers.SpcTextMaximumLength)
            {
                messages.Add(_messageService.GetWarningSpcTextValueTooLongMessage(nameof(game), game[0..32]));
            }
            if (comment.Length > MagicNumbers.SpcTextMaximumLength)
            {
                messages.Add(_messageService.GetWarningSpcTextValueTooLongMessage(nameof(comment), comment[0..32]));
            }
            if (title.Length > MagicNumbers.SpcTextMaximumLength)
            {
                messages.Add(_messageService.GetWarningSpcTextValueTooLongMessage(nameof(title), title[0..32]));
            }

            return new ValidationResult
            {
                Type = (messages.Count > 0)
                    ? ValidationResult.ResultType.Warning
                    : ValidationResult.ResultType.Success,
                Message = messages,
            };
        }

        public IValidationResult ValidateAndProcessInstrumentDirectiveNode(DirectiveNode instruments)
        {
            var instrumentPayload = instruments.Payload as InstrumentsPayload;

            if(instrumentPayload == null)
            {
                throw new Exception();
            }
            var messages = new List<string>();

            foreach(var instrument in instrumentPayload.Instruments)
            {
                if(instrument.Type == InstrumentDefinition.InstrumentType.Noise)
                {
                    var noiseValidation = ValidateNoiseNode(instrument.NoiseData as AtomicNode);
                }
                else if(instrument.Type == InstrumentDefinition.InstrumentType.Number)
                {
                    var instrumentValidation = ValidateInstrumentNode(instrument.InstrumentNumber as AtomicNode);
                }
                else if(instrument.Type != InstrumentDefinition.InstrumentType.Sample)
                {
                    // todo check sample name
                }

                if(instrument.HexSettings.Count != 5)
                {
                    messages.Add(_messageService.GetErrorInstrumentDefinitionMissingHexValuesMessage());
                    continue;
                }

                foreach(var setting in instrument.HexSettings)
                {
                    var hexValue = Convert.ToByte(setting);

                    if(hexValue < 0 ||  hexValue > MagicNumbers.ByteHexMaximum)
                    {
                        messages.Add(_messageService.GetErrorInstrumentDefinitionHexValueOutOfRangeMessage(0, MagicNumbers.ByteHexMaximum, hexValue));
                        continue;
                    }
                }

            }

            return new ValidationResult
            {
                Type = (messages.Count > 0)
                    ? ValidationResult.ResultType.Error
                    : ValidationResult.ResultType.Success,
                Message = messages,
            };
        }

        public IValidationResult ValidateAndProcessSamplesDirectiveNode(DirectiveNode samples)
        {
            var samplesPayload = samples.Payload as SamplesPayload;

            if(samplesPayload == null)
            {
                throw new Exception();
            }

            foreach(var sample in samplesPayload.Samples)
            {

            }

            return new ValidationResult 
            {
                Type = ValidationResult.ResultType.Success,
            };
        }


        #endregion

        #endregion

        #region Helpers

        private void AddDataToChannel(byte dataToAdd)
        {
            if(InActiveLoop)
            {
                if(InActiveSubLoop)
                {
                    CurrentSubLoopData.Add(dataToAdd);
                }
                else
                {
                    CurrentLoopData.Add(dataToAdd);
                }
            }
            else
            {
                CurrentChannelData.Add(dataToAdd);
            }
        }

        private void AddDataToChannel(List<byte> dataToAdd)
        {
            if (InActiveLoop)
            {
                if (InActiveSubLoop)
                {
                    CurrentSubLoopData.AddRange(dataToAdd);
                }
                else
                {
                    CurrentLoopData.AddRange(dataToAdd);
                }
            }
            else
            {
                CurrentChannelData.AddRange(dataToAdd);
            }
        }

        private void AddNoteLength(double ticks)
        {
            if (InActiveLoop)
            {
                ActiveLoopLength += ticks;
            }
            else if(InActiveSubLoop)
            {
                ActiveSubLoopLength += ticks;
            }
            else
            {
                ChannelLength[CurrentChannel] = ChannelLength[CurrentChannel] + ticks;
            }
        }

        private int DivideByTempoRatio(SongNode node, int value, bool isFractionalError)
        {
            if(TempoRatio == MagicNumbers.DefaultValues.InitialTempoRatio)
            {
                return value;
            }

            if(value % TempoRatio != 0)
            {
                if(isFractionalError)
                {
                    _messageService.GetErrorFractionalTempoRatioMessage(SongData.Name, node.LineNumber, node.ColumnNumber);
                }
                else
                {
                    _messageService.GetWarningFactionalTempoRatioValueMessage(SongData.Name, node.LineNumber, node.ColumnNumber);
                }
            }

            return value / TempoRatio;
        }

        private int MultiplyByTempoRatio(SongNode node, int value)
        {
            var result = value * TempoRatio;
            if(TempoRatio >= MagicNumbers.EightBitMaximum)
            {
                _messageService.GetErrorTempoRatioValueOverflowMessage(SongData.Name, node.LineNumber, node.ColumnNumber);
            }
            return result;
        }

        private bool CheckLoopIterations(LoopNode node, int minIterations, int maxIterations)
        {
            if (node.Iterations < minIterations || node.Iterations > maxIterations)
            {
                if (node.Iterations < minIterations)
                {
                    node.Iterations = minIterations;
                }
                else if (node.Iterations > maxIterations)
                {
                    node.Iterations = maxIterations;
                }
                return false;
            }
            return true;
        }

        private int GetNoteLength(SongNode node, int noteLength, int dotCount, bool inTriplet, bool allowTriplet)
        {
            var version = false;
            var length = noteLength;
            if (version == true)
            {
                return noteLength;
            }
            else if(noteLength < 1 || noteLength > MagicNumbers.NoteLengthMaximum)
            {
                length = DefaultNoteLength;
            }
            else
            {
                if(MagicNumbers.NoteLengthMaximum % noteLength == 0)
                {
                    _messageService.GetWarningNoteLengthFractionalTickValueMessage(MagicNumbers.NoteLengthMaximum, SongData.Name, node.LineNumber, node.ColumnNumber);
                }
                length = MagicNumbers.NoteLengthMaximum / noteLength;
            }

            return GetNoteLengthModifier(node, length, dotCount, inTriplet, allowTriplet);
        }

        private int GetNoteLengthModifier(SongNode node, int noteLength, int dotCount, bool inTriplet, bool allowTriplet)
        {
            int result = noteLength;
            int fraction = noteLength;

            for(int i = 0; i < dotCount; i++)
            {
                if (fraction % 2 != 0)
                {
                    if(i != 0)
                    {
                        _messageService.GetWarningFractionalTickValueFromDotsMessage(i + 1, SongData.Name, node.LineNumber, node.ColumnNumber);
                    }
                    else
                    {
                        _messageService.GetWarningFractionalTickValueFromDotsMessage(1, SongData.Name, node.LineNumber, node.ColumnNumber);
                    }
                }

                fraction /= 2;
                result += fraction;
            }

            if(inTriplet && allowTriplet == true)
            {
                if(fraction % 3 != 0)
                {
                    _messageService.GetWarningTripletFractionalTickValueFromDotsMessage(SongData.Name, node.LineNumber, node.ColumnNumber);
                }
                result = (int)Math.Floor(((double)result * 2.0 / 3.0) + 0.5);
            }
            return result;
        }

        #endregion


    }
}
