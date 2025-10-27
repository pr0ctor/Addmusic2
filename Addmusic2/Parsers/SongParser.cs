using Addmusic2.Localization;
using Addmusic2.Model;
using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Addmusic2.Model.Localization;
using Addmusic2.Model.SongTree;
using Addmusic2.Services;
using Addmusic2.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Parsers
{
    internal class SongParser : ISongParser
    {
        private readonly ILogger<IAddmusicLogic> _logger;
        private readonly MessageService _messageService;
        private readonly SongListItem _songListItem;
        private readonly GlobalSettings _globalSettings;
        private readonly IFileCachingService _fileCachingService;
        private readonly SongScope _songScope;
        public SongData SongData { get; set; } = new SongData();

        private SampleInstrumentManager SampleInstrumentManager { get; set; } = new();

        private List<ChannelInformation> Channels { get; set; } = new();
        private List<byte> CurrentLoopData = new List<byte>();
        private List<byte> CurrentSubLoopData = new List<byte>();
        private Dictionary<int, double> ChannelLength = new();
        private Dictionary<string, LoopInformation> RemoteCodeDefinitions = new();
        private Dictionary<string, LoopInformation> NamedLoopDefinitions = new();
        private List<(double ChannelTick, int TempoChange)> TempoChanges = new();

        private ChannelInformation CurrentChannel { get; set; }
        private LoopNode PreviousLoop { get; set; }
        private AtomicNode PreviousNote { get; set; }
        private int PreviousNoteLength { get; set; }

        private int DefaultNoteLength { get; set; } = MagicNumbers.DefaultValues.InitialDefaultNoteLength;
        private int CurrentOctave { get; set; } = MagicNumbers.DefaultValues.StartingOctave;
        private int Tempo { get; set; } = MagicNumbers.DefaultValues.InitialTempoValue;
        private int TempoRatio { get; set; } = MagicNumbers.DefaultValues.InitialTempoRatio;
        private int HTranspose { get; set; } = 0;
        private bool UsingHTranspose { get; set; } = false;
        private bool TempoDefined { get; set; } = false;
        private bool InActiveLoop { get; set; } = false;
        private LoopInformation ActiveLoopInformation { get; set; }
        private double ActiveLoopLength { get; set; } = 0;
        private bool InActiveSubLoop { get; set; } = false;
        private LoopInformation ActiveSubLoopInformation { get; set; }
        private double ActiveSubLoopLength { get; set; } = 0;
        private bool InActiveSimpleLoop { get; set; } = false;
        private bool InActiveSuperLoop { get; set; } = false;


        public SongParser(
            ILogger<IAddmusicLogic> logger,
            MessageService messageService,
            GlobalSettings globalSettings,
            IFileCachingService fileCachingService,
            //SongListItem songItem,
            SongScope songScope
        )
        {
            _logger = logger;
            _messageService = messageService;
            //_songListItem = songItem;
            _globalSettings = globalSettings;
            _fileCachingService = fileCachingService;
            _songScope = songScope;
        }

        public SongData ParseSongNodes(List<ISongNode> nodes)
        {
            SongData.SongScope = _songScope;
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

            foreach (DirectiveNode node in channels)
            {
                ParseChannel(node);
            }

            // Finish by transferring required information to the songdata object

            SongData.ChannelData = Channels;
            SongData.SampleInstrumentManager = SampleInstrumentManager;
            SongData.TempoChanges = TempoChanges;

            // Calculate the first pass pointers

            // CalculateFirstPassPointers(SongData);

            return SongData;
        }

        public void ParseChannel(DirectiveNode channel)
        {
            var channelPayload = channel.Payload as ChannelPayload;

            if (channelPayload == null)
            {
                throw new Exception();
            }

            CurrentChannel = new ChannelInformation
            {
                ChannelNumber = channelPayload.ChannelNumber,
            };

            if (Channels.Exists(c => c.ChannelNumber == CurrentChannel.ChannelNumber))
            {
                // todo throw error for duplicate channel
            }

            Channels.Add(CurrentChannel);
            // Reset Octave as previous versions of Addmusic would carry over the octave
            //      from the previous channel
            CurrentOctave = MagicNumbers.DefaultValues.StartingOctave;

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
                LoopNode => ValidateLoopNode(songNode as LoopNode),
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
                    EvaluateLoopNode(songNode as LoopNode);
                    break;
                case HexNode:
                    EvaluateHexNode(songNode as HexNode);
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
                if (loopPointer >= MagicNumbers.SixteenBitMaximum)
                {
                    _messageService.GetErrorMaximumAllowedNumberOfLoopsReachedMessage();
                }

                if(node is LoopNode)
                //if (node.GetType() == typeof(LoopNode))
                {
                    if (node.NodeType == SongNodeType.SimpleLoop)
                    {
                        var loopName = ((LoopNode)node).LoopName;
                        var hasLoopName = loopName.Length > 0 ? true : false;
                        if (hasLoopName)
                        {
                            if (NamedLoopDefinitions.ContainsKey(loopName))
                            {
                                _messageService.GetErrorDuplicateLoopNameDefinedMessage(loopName);
                            }
                            NamedLoopDefinitions.Add(loopName, new LoopInformation
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
                        var definitionName = payload!.DefinitionName;
                        if (NamedLoopDefinitions.ContainsKey(definitionName))
                        {
                            _messageService.GetErrorDuplicateRemoteCodeDefinitionNameDefinedMessage(definitionName);
                        }
                        RemoteCodeDefinitions.Add(definitionName, new LoopInformation
                        {
                            LoopId = loopPointer++,
                            LoopNode = (LoopNode)node,
                        });
                    }
                }
            }
        }

        public void CalculateFirstPassPointers(SongData songData)
        {
            var channels = songData.ChannelData;
            var firstChannel = songData.ChannelData.First();
            var channelsWithNoData = channels.FindAll(c => c.ChannelData.Count == 0);
            if(channelsWithNoData.Count == channels.Count)
            {
                // todo error out due to there being no data to insert
            }

            var loopAdjustmentAmount = 0;
            if(songData.SongScope == SongScope.Local)
            {

                firstChannel.ChannelData = MagicNumbers.ChannelAdjustmentBytes.Concat(firstChannel.ChannelData).ToList();
                loopAdjustmentAmount = MagicNumbers.ChannelAdjustmentBytes.Count;

                if(songData.EchoBufferSize > 0 || !songData.EchoBufferAlloVCMDIsSet || songData.HasEchoBufferCommand)
                {
                    //Just put the VCMD in its default place: no need to move it around.
                    //In particular, the $F1 command means that echo writes have been enabled, meaning the special case is irrelevant.
                    firstChannel.ChannelData = MagicNumbers.EchoBufferAdjustmentBytes(Convert.ToByte(songData.EchoBufferSize)).Concat(firstChannel.ChannelData).ToList();
                    loopAdjustmentAmount += MagicNumbers.EchoBufferAdjustmentBytes(Convert.ToByte(songData.EchoBufferSize)).Count;
                }
                else
                {
                    var echoBufferChannel = songData.ChannelData.Find(c => c.ChannelNumber == songData.EchoBufferAllocVCMDIChannel);
                    var echoAdjustmentBytes = MagicNumbers.EchoBufferAdjustmentBytes(Convert.ToByte(songData.EchoBufferSize));
                    echoBufferChannel.ChannelData.InsertRange(
                        songData.EchoBufferAllocVCMDILocation + echoAdjustmentBytes.Count,
                        echoAdjustmentBytes
                    );

                    echoBufferChannel.LoopLocations.ForEach(ll => ll += (byte)echoAdjustmentBytes.Count);
                    echoBufferChannel.PhraseLocation += (byte)echoAdjustmentBytes.Count;
                    echoBufferChannel.IntroLocation += (byte)echoAdjustmentBytes.Count;

                }

                firstChannel.LoopLocations.ForEach(ll => ll += (byte)loopAdjustmentAmount);

                foreach( var channel in songData.ChannelData)
                {
                    channel.PhraseLocation += (byte)loopAdjustmentAmount;
                    channel.IntroLocation += (byte)loopAdjustmentAmount;
                }
            }

            // Add 0 to end of each channel's data
            songData.ChannelData.ForEach(c => c.ChannelData.Add(0));

            // optimize samples stuff? not sure if needed yet
            if(_globalSettings.EnableSampleOpimizations)
            {

            }

            // Calculate channel start phrase locations and adjust intros
            var channelNumbers = songData.ChannelData.Select(c => c.ChannelNumber);
            var combinedDataPositions = 0;
            for(int i = 0; i < MagicNumbers.ChannelCount; i++)
            {
                var size = 0;
                if(channelNumbers.Contains(i))
                {
                    var channel = songData.ChannelData.Find(c => c.ChannelNumber == i);
                    channel.PhraseLocation = (byte)combinedDataPositions;
                    channel.IntroLocation += channel.PhraseLocation;
                    size = channel.ChannelData.Count;
                }
                combinedDataPositions += size;
            }

            var spaceForPointersAndIntegers = MagicNumbers.DefaultValues.IntialSpaceForPointersAndStrumentsValue;

            if(songData.HasIntro)
            {
                spaceForPointersAndIntegers += 18;
            }
            if(!songData.DoesntLoop)
            {
                spaceForPointersAndIntegers += 2;
            }

            // the size of an instrument is its numerical value and the hex components
            //      usually (1) + (5) for a space of 6 per instrument but dynamically calculate just in case
            var instrumentSpace = songData.SampleInstrumentManager.GetTotalInstrumentSpace();

            spaceForPointersAndIntegers += instrumentSpace;

            // handle an offset for proper indexing of data
            var offset = (songData.HasIntro ? 2 : 0) + (songData.DoesntLoop ? 0 : 2) + 4;

            var combinedData = new List<byte>();

            // handle first <8 indices of data

            combinedData[0] = (byte)((offset + instrumentSpace) & MagicNumbers.ByteHexMaximum);
            combinedData[1] = (byte)(((offset + instrumentSpace) >> 8) & MagicNumbers.ByteHexMaximum);

            if(songData.DoesntLoop)
            {
                combinedData[offset - 2] = MagicNumbers.ByteHexMaximum;
                combinedData[offset - 1] = MagicNumbers.ByteHexMaximum;
            }
            else
            {
                combinedData[offset - 4] = MagicNumbers.ByteHexMaximum;
                combinedData[offset - 3] = MagicNumbers.ByteHexMaximum;
                combinedData[offset - 2] = (byte)((songData.HasIntro) ? 0xFD : 0xFC);
                combinedData[offset - 1] = MagicNumbers.ByteHexMaximum;
            }

            if (songData.HasIntro)
            {
                combinedData[2] = (byte)((offset + instrumentSpace + 16) & MagicNumbers.ByteHexMaximum);
                combinedData[3] = (byte)((offset + instrumentSpace + 16) >> 8);
            }

            // add in the instrument data after setting those early indexed values
            foreach (var instrument in songData.SampleInstrumentManager.Instruments)
            {
                combinedData.Add((byte)instrument.InstrumentData);
                combinedData.AddRange(instrument.HexComponents.Select(hc => (byte)hc));
            }

            // add in the phrase locations for each channel after the instruments
            //      empty channels still get data
            //      channels with data set values wrt the amount of data
            var phraseData = new List<byte>();
            var introData = new List<byte>();
            for(var i = 0; i < MagicNumbers.ChannelCount; i++)
            {
                if(!channelNumbers.Contains(i))
                {
                    phraseData.Add(0xFB);
                    phraseData.Add(MagicNumbers.ByteHexMaximum);
                    if(songData.HasIntro)
                    {
                        introData.Add(0xFB);
                        introData.Add(MagicNumbers.ByteHexMaximum);
                    }
                    continue;
                }
                var channel = songData.ChannelData.Find(c => c.ChannelNumber == i);
                var phraseValue = (byte)(channel!.PhraseLocation + spaceForPointersAndIntegers);
                phraseData.Add((byte)(phraseValue & MagicNumbers.ByteHexMaximum));
                phraseData.Add((byte)(phraseValue >> 8));
                if(songData.HasIntro)
                {
                    var introValue = (byte)(channel.IntroLocation + spaceForPointersAndIntegers);
                    introData.Add((byte)(introValue & MagicNumbers.ByteHexMaximum));
                    introData.Add((byte)(introValue >> 8));
                }
            }

            // add the phrase and intro data into the combined dataset

            combinedData.AddRange(phraseData);
            if(songData.HasIntro)
            {
                combinedData.AddRange(introData);
            }

            // calculate the total size of the song
            songData.AllPointersAndInstruments = combinedData;
            songData.SpaceForPointersAndInstruments = spaceForPointersAndIntegers;
            songData.TotalSize = songData.ChannelData.Sum(c => c.ChannelData.Count) + spaceForPointersAndIntegers;

            CalculateTotalSongLength(songData);

            // Sum all used samples' datasize and add in the SCRN Table size for each sample
            songData.SpaceUsedBySamples = songData.SampleInstrumentManager.UsedSamples
                .Select(s => ( MagicNumbers.SampleSCRNTableSize + s.SampleDataSize ))
                .Sum();
            
            // Generate statistics file

        }

        public void CalculateTotalSongLength(SongData songData)
        {
            var totalLength = 0;

            var minChannelTickLength = (int)(songData.ChannelData.Min(c => c.ChannelLength));
            totalLength = (minChannelTickLength > -1) ? -1 : minChannelTickLength;
            songData.MainLength = totalLength;

            if (songData.HasIntro)
            {
                songData.MainLength -= songData.IntroLength;
            }

            // estimate the true length of the song

            if (songData.GuessLength)
            {

                var sortedTempoChanges = songData.TempoChanges
                    .OrderBy(tc => tc.ChannelTick)
                    .ThenBy(tc => tc.TempoChange)
                    .ToList();

                var firstTempoChange = sortedTempoChanges.First();
                if (sortedTempoChanges.Count == 0)
                {
                    sortedTempoChanges.Add((0, MagicNumbers.DefaultValues.InitialTempoValue));
                }
                else if (firstTempoChange.ChannelTick != 0)
                {
                    sortedTempoChanges.Add((0, MagicNumbers.DefaultValues.InitialTempoValue));
                }

                sortedTempoChanges.Add((totalLength, 0));

                // If there exists some intro segment
                //      store the info in the intro tracker until the end of the intro is reached
                //      otherwise store the info in the main tracker as the entire data will be in there
                var foundIntroEnd = (songData.HasIntro) ? false : true;
                var introTracker = 0.0;
                var mainTracker = 0.0;
                for (int i = 0; i < sortedTempoChanges.Count; i++)
                {
                    if (sortedTempoChanges[i].ChannelTick > totalLength)
                    {
                        // todo add warning about change after length of song
                        break;
                    }

                    if (sortedTempoChanges[i].TempoChange < 0)
                    {
                        foundIntroEnd = true;
                    }

                    var difference = sortedTempoChanges[i + 1].ChannelTick - sortedTempoChanges[i].ChannelTick;
                    if (foundIntroEnd)
                    {
                        introTracker += difference / (2 * Math.Abs(sortedTempoChanges[i].TempoChange));
                    }
                    else
                    {
                        mainTracker += difference / (2 * Math.Abs(sortedTempoChanges[i].TempoChange));
                    }
                }

                songData.Seconds = (int)(Math.Floor(introTracker + (mainTracker * 2) + 0.5));
                songData.MainSeconds = (int)mainTracker;
                songData.IntroSeconds = (int)introTracker;

                songData.KnowsLength = true;

            }
            // Just in case
            else
            {
                songData.KnowsLength = false;
            }
        }

        #region Node Evalulators

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
                case SongNodeType.NoLoopCommand:
                case SongNodeType.QuestionMark:
                    EvaluateQuestionMarkOrNoLoopNode(atomic);
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
                    EvaluateInstrumentNode(atomic);
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
                    EvaluateQuantizationNode(atomic);
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
                case SongNodeType.Pipe:
                    // currently not implemented
                    return;
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

        public void EvaluateInstrumentNode(AtomicNode instrumentNode)
        {
            var instrumentPayload = instrumentNode.Payload as InstrumentPayload;

            if (instrumentPayload == null)
            {
                throw new Exception();
            }

            var instrumentNumber = instrumentPayload.InstrumentNumber;

            if (instrumentNumber <= 18 || instrumentNumber >= MagicNumbers.StartingCustomInstrumentNumber)
            {
                if (_globalSettings.EnableConversion)
                {
                    if (instrumentNumber >= 0x13 && instrumentNumber < MagicNumbers.StartingCustomInstrumentNumber)
                    {
                        instrumentNumber = instrumentNumber - 0x13 + 30;
                    }
                }
                // Always optimize Samples
                //if (_globalSettings.EnableSampleOpimizations)
                //{

                var instrumentDefined = SampleInstrumentManager.ContainsInstrument(instrumentNumber);

                if(!instrumentDefined)
                {
                    // todo handle undefined instrument error
                }

                if (instrumentNumber < MagicNumbers.StartingCustomInstrumentNumber)
                {
                    var instrumentInfo = new InstrumentInformation 
                    {
                        InstrumentNumber = instrumentNumber,
                        InstrumentData = MagicNumbers.InstrumentsToSample[instrumentNumber]
                    };

                    var useInstrument = SampleInstrumentManager.UseInstrument(instrumentNumber, instrumentInfo);

                    if(!useInstrument)
                    {
                        // todo handle error
                    }

                    //if(!UsedInstruments.Any(i => i.Value.InstrumentNumber == instrumentInfo.InstrumentNumber))
                    //{
                    //    UsedInstruments.Add(instrumentNumber, instrumentInfo);
                    //}
                }
                else if (instrumentNumber >= MagicNumbers.StartingCustomInstrumentNumber)
                {
                    var useInstrument = SampleInstrumentManager.UseInstrument(instrumentNumber);

                    if (!useInstrument)
                    {
                        // todo handle error
                    }
                }
                //}

                // Addmusic parser version 1 compatiblity
                //if (false)
                //{
                //    CurrentChannel.IgnoreTuning = false;
                //}

                AddDataToChannel(MagicNumbers.CommandValues.Instrument);
                AddDataToChannel(Convert.ToByte(instrumentNumber));
            }

            /*if (instrumentNumber < MagicNumbers.StartingCustomInstrumentNumber)
            {
                if (_globalSettings.EnableSampleOpimizations)
                {
                    // todo do something
                }
            }*/

            SetCurrentInstrument(instrumentNumber);

            // ignore transpose map for now
            //if (instrumentNumber < 19)
            //{
            //    HTranspose = 0;
            //    UsingHTranspose = false;
            //    // todo add transposemap logic
            //}
        }

        public void EvaluateNoteNode(AtomicNode noteNode, bool inTriplet = false, bool inPitchSlide = false, bool isNextForDDPitchSlide = false)
        {
            CurrentChannel.HasNoteData = true;
            var notePayload = noteNode.Payload as NotePayload;
            
            var tempLength = GetNoteLength(noteNode, notePayload!.Duration, notePayload.DotCount, inTriplet, true);

            var noteValueChar = (int)notePayload.NoteValue[0];
            var note = GetPitchValue(noteValueChar, notePayload.Accidental);
            var currentInstrument = GetCurrentInstrument();
            if (UsingHTranspose)
            {
                note += HTranspose;
            }
            else
            {
                // todo add and check tuning[] logic
            }

            if (note < MagicNumbers.NoteLengthMaxBeforeSplit)
            {
                // todo add warning for too low note, but may not need
                if (false)
                {

                }
                else
                {
                    note = MagicNumbers.CommandValues.Rest;
                }
            }
            else if (note >= MagicNumbers.CommandValues.Tie)
            {
                // todo add error for note pitch too high
            }
            else if (currentInstrument >= 21 && currentInstrument < MagicNumbers.StartingCustomInstrumentNumber && note < MagicNumbers.CommandValues.Tie)
            {

                note = 0xD0 + (currentInstrument - 21);

                if (MagicNumbers.SfxChannels.Contains(CurrentChannel.ChannelNumber))
                {
                    SetCurrentInstrument(MagicNumbers.ByteHexMaximum);
                }
            }


            if (inPitchSlide)
            {
                AddDataToChannel(Convert.ToByte(PreviousNoteLength));
                AddDataToChannel(Convert.ToByte(note));
            }

            if (isNextForDDPitchSlide)
            {
                AddDataToChannel(Convert.ToByte(note));
                return; // no more logic for this node
            }

            // todo optimize group of connected rests

            foreach (var tie in notePayload.ConnectedTies)
            {
                var tiePayload = tie.Payload as TiePayload;
                tempLength += GetNoteLength(tie, tiePayload.Duration, tiePayload.DotCount, inTriplet, true);
            }

            tempLength = DivideByTempoRatio(noteNode, tempLength, true);

            AddNoteLength(tempLength);

            ApplyTempoRateAdjustmentAndQuantization(noteNode, Convert.ToByte(note), tempLength);
        }

        public void EvaluateTieNode(AtomicNode tieNode, bool inTriplet = false, bool inPitchSlide = false, bool isNextForDDPitchSlide = false)
        {
            CurrentChannel.HasNoteData = true;
            var tiePayload = tieNode.Payload as TiePayload;

            var tempLength = GetNoteLength(tieNode, tiePayload.Duration, tiePayload.DotCount, inTriplet, true);

            if (inPitchSlide)
            {
                AddDataToChannel(Convert.ToByte(PreviousNoteLength));
                AddDataToChannel(MagicNumbers.CommandValues.Tie);
            }

            if (isNextForDDPitchSlide)
            {
                AddDataToChannel(MagicNumbers.CommandValues.Tie);
                return; // no more logic for this node
            }

            ApplyTempoRateAdjustmentAndQuantization(tieNode, MagicNumbers.CommandValues.Tie, tempLength);
        }

        public void EvaluateRestNode(AtomicNode restNode, bool inTriplet = false, bool inPitchSlide = false, bool isNextForDDPitchSlide = false)
        {
            CurrentChannel.HasNoteData = true;
            var restPayload = restNode.Payload as NotePayload;

            var tempLength = GetNoteLength(restNode, restPayload.Duration, restPayload.DotCount, inTriplet, true);

            if (inPitchSlide)
            {
                AddDataToChannel(Convert.ToByte(PreviousNoteLength));
                AddDataToChannel(MagicNumbers.CommandValues.Rest);
            }

            if (isNextForDDPitchSlide)
            {
                AddDataToChannel(MagicNumbers.CommandValues.Rest);
                return; // no more logic for this node
            }

            // todo optimize group of connected rests

            foreach (var tie in restPayload.ConnectedTies)
            {
                var tiePayload = tie.Payload as TiePayload;
                tempLength += GetNoteLength(tie, tiePayload.Duration, tiePayload.DotCount, inTriplet, true);
            }

            tempLength = DivideByTempoRatio(restNode, tempLength, true);

            AddNoteLength(tempLength);

            ApplyTempoRateAdjustmentAndQuantization(restNode, MagicNumbers.CommandValues.Rest, tempLength);
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
                panValue |= panLeft << 7;
            }

            if (panRight != -1)
            {
                panValue |= panRight << 6;
            }

            AddDataToChannel(MagicNumbers.CommandValues.Pan);
            AddDataToChannel(Convert.ToByte(panValue));

        }

        public void EvaluateQuantizationNode(AtomicNode quantizationNode)
        {
            var quantizationPayload = quantizationNode.Payload as QuantizationPayload;

            // If there is a volume node then we need to only use the delay
            // If there is no volume node then the quantization value is a HexNumber since the delay value is limited to 0->7
            var quantizationValue = quantizationPayload.VolumeNode == null
                ? Convert.ToByte($"{quantizationPayload.DelayValue}{quantizationPayload.VolumeValue}", 16)
                : Convert.ToByte(quantizationPayload.DelayValue);

            CurrentChannel.CurrentQuantization = quantizationValue;

            if (InActiveLoop)
            {
                if (InActiveSubLoop)
                {
                    ActiveSubLoopInformation.CurrentQuantization = quantizationValue;
                }
                else
                {
                    ActiveLoopInformation.CurrentQuantization = quantizationValue;
                }
            }
        }

        public void EvaluateQuestionMarkOrNoLoopNode(AtomicNode questionMarkNode)
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
                SongData.NoMusic[CurrentChannel.ChannelNumber, 0] = true;
            }
            else if (questionMarkPayload.MarkNumber == 2)
            {
                SongData.NoMusic[CurrentChannel.ChannelNumber, 1] = true;
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

            // Set global Tempo value to the current tempo
            Tempo = tempo;

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
                    TempoChanges.Add((CurrentChannel.ChannelLength, tempo));
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
                case SongNodeType.SampleLoad:
                    EvaluateSampleLoad(compositeNode);
                    break;
                case SongNodeType.Intro:
                    EvaluateIntro(compositeNode);
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

        public void EvaluateIntro(CompositeNode node)
        {

            if(SongData.HasIntro == false)
            {
                TempoChanges.Add((CurrentChannel.ChannelLength, -(Tempo)));
            }
            else
            {
                var introTempo = TempoChanges.Find(tc => tc.TempoChange < 0);
                introTempo.TempoChange = -(Tempo);
            }

            SongData.HasIntro = true;
            SongData.IntroLength = (int)CurrentChannel.ChannelLength;

            CurrentChannel.HasIntro = true;
            CurrentChannel.IntroLocation = (byte)CurrentChannel.ChannelData.Count;
            CurrentChannel.IntroLength = (int)CurrentChannel.ChannelLength;
        }

        public void EvaluatePitchSlideNode(CompositeNode node)
        {
            var payload = node.Payload as PitchSlidePayload;

            foreach(SongNode songNode in payload!.Nodes)
            {
                if(songNode.NodeType == SongNodeType.Empty)
                {
                    AddDataToChannel(MagicNumbers.CommandValues.PitchSlide);
                    AddDataToChannel(0x00);
                    AddDataToChannel(Convert.ToByte(PreviousNoteLength));
                }
                else if(songNode.NodeType == SongNodeType.Note)
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

        public void EvaluateSampleLoad(CompositeNode node)
        {
            var sampleloadPayload = node.Payload as SampleLoadPayload;

            if (sampleloadPayload == null)
            {
                throw new Exception();
            }

            byte finalSampleIndex = 0x00;

            // The Sample Load command was passed a named sample
            if (sampleloadPayload.SampleNumber == -1)
            {
                var standardizedPath = Helpers.Helpers.StandardizeFileDirectoryDelimiters(sampleloadPayload.SampleName);
                var lastDirectorySeparator = (standardizedPath.Contains(@"\"))
                    ? standardizedPath.LastIndexOf(@"\")
                    : (standardizedPath.Contains(@"/"))
                        ? standardizedPath.LastIndexOf(@"/")
                        : 0;
                var sampleName = standardizedPath[lastDirectorySeparator..];
                var samplePath = Path.Combine(SongData.SongPath, sampleName);

                SampleInstrumentManager.AddNewSampleName(sampleName);
                var sampleData = new AddmusicSample
                {
                    Name = sampleName,
                    Path = samplePath,
                    IsImportant = false,
                    IsLooping = false,
                };
                SampleInstrumentManager.AddNewSample(sampleData);
                SampleInstrumentManager.UseSample(sampleData);
                Helpers.Helpers.LoadSampleToCache(_fileCachingService, sampleData);

                var sampleIndex = SampleInstrumentManager.Samples.FindIndex(s => s.Name == sampleName);

                if (sampleIndex == -1)
                {
                    // todo throw error // this should never get caught because other errors should happen first
                    throw new Exception();
                }

                finalSampleIndex = Convert.ToByte(sampleIndex);
            }
            // the Sample Load command was passed an instrument
            else
            {
                finalSampleIndex = Convert.ToByte(MagicNumbers.InstrumentsToSample[sampleloadPayload.SampleNumber]);
            }
                
            var tuningValue = Convert.ToByte(sampleloadPayload.TuningValue, 16);

            AddDataToChannel(MagicNumbers.CommandValues.SampleLoad);
            AddDataToChannel(finalSampleIndex);
            AddDataToChannel(tuningValue);
        }

        public void EvaluateTriplet(CompositeNode node)
        {
            foreach(SongNode songNode in node.Children)
            {
                if(songNode.NodeType == SongNodeType.Note)
                {
                    EvaluateNoteNode((AtomicNode)songNode, true);
                }
                else if(songNode.NodeType == SongNodeType.Rest)
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
            CurrentChannel.LoopLocations.Add(Convert.ToByte(CurrentChannel.ChannelData.Count));
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
            CurrentChannel.LoopLocations.Add(Convert.ToByte(CurrentChannel.ChannelData.Count));
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
            if (InActiveLoop == true)
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
            var calledLoopData = (LoopNode)NamedLoopDefinitions[callLoopNode.LoopName].LoopNode.Clone();

            // This loop invocation may have a different number of iterations than the original definition
            //      Potentially none at all, if so then it only needs to be called once
            if (callLoopNode.Iterations <= 1)
            {
                calledLoopData.Iterations = 1;
            }
            else
            {
                calledLoopData.Iterations = callLoopNode.Iterations;
            }

            EvaluateLoopNode(calledLoopData);
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
                    // Skip these since they have already been processed by this point
                    break;
                case SongNodeType.Pad:
                    EvaluatePadNode(specialDirective);
                    break;
                case SongNodeType.Halvetempo:
                    EvaluateHalveTempoNode(specialDirective);
                    break;
                case SongNodeType.Option:
                    // todo implement
                    //specialDirective;
                    break;
                case SongNodeType.OptionGroup:
                    // todo implement
                    //specialDirective;
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
            var padAmount = Convert.ToInt32(padPayload.PadLength, 16);
            SongData.MinSize = padAmount;
        }

        public void EvaluateHalveTempoNode(DirectiveNode halvetempoNode)
        {
            TempoRatio = MultiplyByTempoRatio(halvetempoNode, 2);
        }


        #endregion

        #region Hex Command Node Evaluators

        public void EvaluateHexNode(HexNode hexNode)
        {
            switch (hexNode.CommandType)
            {
                case HexCommands.DDPitchBlend:
                    EvaluateDDPitchBlend(hexNode);
                    break;
                case HexCommands.FAHotPatchPreset:
                    break;
                case HexCommands.FAHotPatchToggleBits:
                    break;
                case HexCommands.FAEchoBufferReserve:
                    EvaluateFAEchoBufferReserveNode(hexNode);
                    break;
                case HexCommands.FCHexRemoteCommand:
                    break;
                case HexCommands.FCHexRemoteGain:
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

        public void EvaluateDDPitchBlend(HexNode pitchBlendNode)
        {
            AddDataToChannel(Convert.ToByte(pitchBlendNode.HexCommand.Replace("$", ""), 16));

            foreach (var commandValue in pitchBlendNode.HexValues)
            {
                AddDataToChannel(Convert.ToByte(commandValue.Replace("$", ""), 16));
            }

            foreach (var child in pitchBlendNode.Children)
            {
                EvaluateNode(child);
            }
        }

        public void EvaluateFAEchoBufferReserveNode(HexNode faEchoBufferReserve)
        {
            MarkEchoBufferAllocVCMD();
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
                SongNodeType.Tune or
                SongNodeType.Pipe => new ValidationResult
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

        public IValidationResult ValidateVolumeNode(AtomicNode volume)
        {
            var volumePayload = volume.Payload as VolumePayload;

            if (volumePayload == null)
            {
                throw new Exception();
            }

            var fadeValue = volumePayload.FadeValue;
            var volumeValue = volumePayload.Volume;
            var messages = new List<string>();
            if (volumeValue < 0 || volumeValue > MagicNumbers.EightBitMaximum)
            {
                if (volume.NodeType == SongNodeType.Volume)
                {
                    messages.Add(_messageService.GetErrorVolumeVolumeValueOutOfRangeMessage(0, MagicNumbers.EightBitMaximum, volumeValue));
                }
                else
                {
                    messages.Add(_messageService.GetErrorGlobalVolumeVolumeValueOutOfRangeMessage(0, MagicNumbers.EightBitMaximum, volumeValue));
                }
            }
            if (fadeValue != -1 && (fadeValue < 0 || fadeValue > MagicNumbers.EightBitMaximum))
            {
                if (volume.NodeType == SongNodeType.Volume)
                {
                    messages.Add(_messageService.GetErrorVolumeFadeValueOutOfRangeMessage(0, MagicNumbers.EightBitMaximum, fadeValue));
                }
                else
                {
                    messages.Add(_messageService.GetErrorGlobalVolumeFadeValueOutOfRangeMessage(0, MagicNumbers.EightBitMaximum, fadeValue));
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

        public IValidationResult ValidatePanNode(AtomicNode pan)
        {
            var panPayload = pan.Payload as PanPayload;

            if (panPayload == null)
            {
                throw new Exception();
            }

            if (panPayload.PanPosition < 0 || panPayload.PanPosition > MagicNumbers.PanDirectionMaximum)
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
            if (delayValue != -1 && (delayValue < 0 || delayValue > MagicNumbers.EightBitMaximum))
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

            if (quantizationPayload.VolumeNode != null)
            {
                var volumeNodeValidation = (ValidationResult)ValidateVolumeNode(quantizationPayload.VolumeNode as AtomicNode);
                if (volumeNodeValidation.Type != ValidationResult.ResultType.Success)
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
            var instrumentPayload = instrument.Payload as InstrumentPayload;

            if (instrumentPayload == null)
            {
                throw new Exception();
            }

            var instrumentNumber = instrumentPayload.InstrumentNumber;

            if (instrumentNumber < 0 || instrumentNumber > MagicNumbers.EightBitMaximum)
            {
                return new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = new List<string>
                    {
                        _messageService.GetErrorInstrumentValueOutOfRangeMessage(0, MagicNumbers.EightBitMaximum, instrumentNumber),
                    }
                };
            }

            // todo logic for sample optimization

            return new ValidationResult
            {
                Type = ValidationResult.ResultType.Success
            };
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
                SongNodeType.Intro => ValidateIntroNode(composite),
                // Requires validation
                SongNodeType.Triplet => ValidateTripletNode(composite),
                SongNodeType.PitchSlide => ValidatePitchSlideNode(composite),
                SongNodeType.HexCommand => ValidateHexCommand(composite),
                SongNodeType.SampleLoad => ValidateSampleLoadNode(composite),

                _ => throw new Exception()
            }; ;
        }

        public IValidationResult ValidateIntroNode(CompositeNode introNode)
        {
            // This should never happen due to ANTLR grammer enforcement but just in case
            if (InActiveLoop)
            {
                return new ValidationResult
                {
                    Type = ValidationResult.ResultType.Error,
                    Message = new List<string>
                    {
                        _messageService.GetErrorIntroDirectiveFoundInLoopMessage(),
                    }
                };
            }

            return new ValidationResult
            {
                Type = ValidationResult.ResultType.Success
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

        public IValidationResult ValidateSampleLoadNode(CompositeNode sampleLoadNode)
        {
            var sampleloadPayload = sampleLoadNode.Payload as SampleLoadPayload;

            if (sampleloadPayload == null)
            {
                throw new Exception();
            }

            // todo check for sample name existence and is loaded

            if(sampleloadPayload.SampleNumber == -1)
            {
                if(sampleloadPayload.SampleName.Length == 0)
                {
                    // todo handle error for missing sample name
                }

                // get file extension
                if (sampleloadPayload.SampleName.LastIndexOf(".") == -1)
                {
                    // todo handle missing file extension
                }

                var fileExtensionStartPosition = sampleloadPayload.SampleName.LastIndexOf(".");
                var fileExtension = sampleloadPayload.SampleName[fileExtensionStartPosition..];

                if (!FileNames.FileExtensions.ValidSampleExtensions.Contains(fileExtension))
                {
                    // todo handle invalid file extensions
                }

                if (fileExtension == FileNames.FileExtensions.SampleBank)
                {
                    // todo handle deprecated filetype
                    //continue;
                }

                if (fileExtension == FileNames.FileExtensions.SampleBrr)
                {

                    var standardizedPath = Helpers.Helpers.StandardizeFileDirectoryDelimiters(sampleloadPayload.SampleName);
                    var lastDirectorySeparator = (standardizedPath.Contains(@"\"))
                        ? standardizedPath.LastIndexOf(@"\")
                        : (standardizedPath.Contains(@"/"))
                            ? standardizedPath.LastIndexOf(@"/")
                            : 0;
                    var sampleName = standardizedPath[lastDirectorySeparator..];
                    var samplePath = Path.Combine(SongData.SongPath, sampleName);

                    //if(SampleNames.Contains(sampleName))
                    if (SampleInstrumentManager.ContainsSampleName(sampleName))
                    {
                        // todo notify duplicate sample
                        //continue;
                    }

                }
            }
            else
            {
                if(sampleloadPayload.SampleNumber > MagicNumbers.StartingCustomInstrumentNumber)
                {
                    // todo add warning to just use other stuff
                }
            }

            var tuningValue = Convert.ToByte(sampleloadPayload.TuningValue, 16);


            if (!Helpers.Helpers.IsHexInRange(tuningValue))
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
                if (nodeValidation.Type == ValidationResult.ResultType.Failure ||
                    nodeValidation.Type == ValidationResult.ResultType.Error ||
                    nodeValidation.Type == ValidationResult.ResultType.Warning)
                {
                    messages.AddRange(nodeValidation.Message);
                }
            }

            return messages.Count > 0
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

            return messages.Count > 0
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
            if (!NamedLoopDefinitions.ContainsKey(callLoopNode.LoopName))
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

            return messages.Count > 0
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

            if (PreviousLoop == null)
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
            if (PreviousLoop.NodeType == SongNodeType.SuperLoop)
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
            if (!validIterations)
            {
                messages.Add(_messageService.GetWarningLoopIterationOutOfRangeMessage());
            }

            return messages.Count > 0
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
                SongNodeType.OptionGroup => new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success
                },
                SongNodeType.Path => ValidateAndProcessPathNode(specialDirective),
                // Requires Validation
                SongNodeType.SPC => ValidateAndProcessSpcDirectiveNode(specialDirective),
                SongNodeType.Instruments => ValidateAndProcessInstrumentDirectiveNode(specialDirective),
                SongNodeType.Samples => ValidateAndProcessSamplesDirectiveNode(specialDirective),
                _ => throw new Exception()
            };
        }

        public IValidationResult ValidateAndProcessPathNode(DirectiveNode pathNode)
        {
            var pathPayload = pathNode.Payload as PathPayload;

            if(pathPayload == null)
            {
                throw new Exception();
            }
            // no need to process further
            if(pathPayload.PathText.Length == 0)
            {
                // log notice of empty path
                return new ValidationResult
                {
                    Type = ValidationResult.ResultType.Success,
                };
            }

            // parse potential path characters and recombine with the current systems correct directory delimmitters
            var pathValue = pathPayload.PathText;
            var correctedPath = Helpers.Helpers.StandardizeFileDirectoryDelimiters(pathValue);
            SongData.SongPath = correctedPath;

            return new ValidationResult
            {
                Type = ValidationResult.ResultType.Success,
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

            if (game.Length == 0)
            {
                SongData.Game = Model.Constants.Messages.DefaultSpcGameName;
            }
            if (length == "auto")
            {
                // todo stuff here
            }
            else if (length.Length > 0)
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
                    var totalSeconds = mins * 60 + secs;

                    if (totalSeconds > 999)
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

                    SongData.Seconds = (int)(totalSeconds & MagicNumbers.ThirtytwoBitMaximum);
                }
            }

            var messages = new List<string>();
            if (author.Length > MagicNumbers.SpcTextMaximumLength)
            {
                messages.Add(_messageService.GetWarningSpcTextValueTooLongMessage(nameof(author), MagicNumbers.SpcTextMaximumLength.ToString(), author[0..MagicNumbers.SpcTextMaximumLength]));
                SongData.Author = author[0..MagicNumbers.SpcTextMaximumLength];
            }
            else
            {
                SongData.Author = author;
            }
            if (game.Length > MagicNumbers.SpcTextMaximumLength)
            {
                messages.Add(_messageService.GetWarningSpcTextValueTooLongMessage(nameof(game), MagicNumbers.SpcTextMaximumLength.ToString(), game[0..MagicNumbers.SpcTextMaximumLength]));
                SongData.Game = game[0..MagicNumbers.SpcTextMaximumLength];
            }
            else
            {
                SongData.Game = game;
            }
            if (comment.Length > MagicNumbers.SpcTextMaximumLength)
            {
                messages.Add(_messageService.GetWarningSpcTextValueTooLongMessage(nameof(comment), MagicNumbers.SpcTextMaximumLength.ToString(), comment[0..MagicNumbers.SpcTextMaximumLength]));
                SongData.Comment = comment[0..MagicNumbers.SpcTextMaximumLength];
            }
            else
            {
                SongData.Comment = comment;
            }
            if (title.Length > MagicNumbers.SpcTextMaximumLength)
            {
                messages.Add(_messageService.GetWarningSpcTextValueTooLongMessage(nameof(title), MagicNumbers.SpcTextMaximumLength.ToString(), title[0..MagicNumbers.SpcTextMaximumLength]));
                SongData.Title = title[0..MagicNumbers.SpcTextMaximumLength];
            }
            else
            {
                SongData.Title = title;
            }

                return new ValidationResult
                {
                    Type = messages.Count > 0
                        ? ValidationResult.ResultType.Warning
                        : ValidationResult.ResultType.Success,
                    Message = messages,
                };
        }

        public IValidationResult ValidateAndProcessInstrumentDirectiveNode(DirectiveNode instruments)
        {
            var instrumentPayload = instruments.Payload as InstrumentsPayload;

            if (instrumentPayload == null)
            {
                throw new Exception();
            }
            var messages = new List<string>();
            var customInstrumentCount = MagicNumbers.StartingCustomInstrumentNumber;
            foreach (var instrument in instrumentPayload.Instruments)
            {
                var instrumentInformation = new InstrumentInformation();

                if (instrument.Type == InstrumentDefinition.InstrumentType.Noise)
                {
                    var noiseValidation = (ValidationResult)ValidateNoiseNode(instrument.NoiseData as AtomicNode);

                    if(noiseValidation.Type == ValidationResult.ResultType.Failure 
                        || noiseValidation.Type == ValidationResult.ResultType.Error
                    )
                    {
                        // todo handle error on the noise segment
                    }
                    else if(noiseValidation.Type == ValidationResult.ResultType.Warning)
                    {
                        // todo handle warning
                    }
                    var noiseValue = Convert.ToByte(((NoisePayload)((AtomicNode)instrument.NoiseData).Payload).NoiseValue);
                    var finalInstrumentValue = noiseValue | 0x80;

                    instrumentInformation.InstrumentNumber = customInstrumentCount;
                    instrumentInformation.InstrumentData = finalInstrumentValue;
                }
                else if (instrument.Type == InstrumentDefinition.InstrumentType.Number)
                {
                    var instrumentValidation = (ValidationResult)ValidateInstrumentNode(instrument.InstrumentNumber as AtomicNode);

                    if (instrumentValidation.Type == ValidationResult.ResultType.Failure
                        || instrumentValidation.Type == ValidationResult.ResultType.Error
                    )
                    {
                        // todo handle error on the instrument segment
                    }
                    else if (instrumentValidation.Type == ValidationResult.ResultType.Warning)
                    {
                        // todo handle warning
                    }

                    var instrumentNumber = ((InstrumentPayload)((AtomicNode)instrument.InstrumentNumber).Payload).InstrumentNumber;
                    
                    if(instrumentNumber >= MagicNumbers.StartingCustomInstrumentNumber)
                    {
                        // todo throw error that you cannot have an instrument definition base for custom instruments using a custom instrument
                    }

                    var instrumentToSample = MagicNumbers.InstrumentsToSample[instrumentNumber];

                    instrumentInformation.InstrumentNumber = instrumentNumber;
                    instrumentInformation.InstrumentData = instrumentToSample;

                }
                else if (instrument.Type == InstrumentDefinition.InstrumentType.Sample)
                {
                    var sampleName = instrument.SampleName;
                    if(sampleName == null || sampleName.Length == 0)
                    {
                        // todo message about missing name
                        continue;
                    }
                    //if(!SampleNames.Contains(sampleName))
                    if(!SampleInstrumentManager.ContainsSampleName(sampleName ?? ""))
                    {
                        // todo message about not have this sample previously defined
                    }

                    var useSample = SampleInstrumentManager.UseSampleName(sampleName ?? "");

                    if(!useSample)
                    {
                        // todo handle error
                    }

                    instrumentInformation.InstrumentNumber = customInstrumentCount;
                    instrumentInformation.InstrumentSample = sampleName;

                }

                if (instrument.HexSettings.Count != 5)
                {
                    messages.Add(_messageService.GetErrorInstrumentDefinitionMissingHexValuesMessage());
                    continue;
                }

                var intHexes = new List<int>();
                foreach (var setting in instrument.HexSettings)
                {
                    var hexValue = Convert.ToByte(setting);

                    if (!Helpers.Helpers.IsHexInRange(hexValue))
                    {
                        messages.Add(_messageService.GetErrorInstrumentDefinitionHexValueOutOfRangeMessage(0, MagicNumbers.ByteHexMaximum, hexValue));
                        continue;
                    }
                    intHexes.Add(hexValue);
                }

                instrumentInformation.HexComponents.AddRange(intHexes);

                SampleInstrumentManager.AddInstrument(instrumentInformation);
                //Instruments.Add(instrumentInformation);
                customInstrumentCount++;
            }

            return new ValidationResult
            {
                Type = messages.Count > 0
                    ? ValidationResult.ResultType.Error
                    : ValidationResult.ResultType.Success,
                Message = messages,
            };
        }

        public IValidationResult ValidateAndProcessSamplesDirectiveNode(DirectiveNode samples)
        {
            var samplesPayload = samples.Payload as SamplesPayload;

            if (samplesPayload == null)
            {
                throw new Exception();
            }

            if(samplesPayload.SampleGroupPaths.Count == 0)
            {
                samplesPayload.SampleGroupPaths.Add("#default");
            }

            foreach(var sampleGroup in samplesPayload.SampleGroupPaths)
            {
                var group = _globalSettings.ResourceList.SampleGroups.FindAll(g => g.Name.Equals(sampleGroup, StringComparison.InvariantCultureIgnoreCase)).ToList();
                if(group.Count == 0)
                {

                    // todo throw error due to missing sample group definition
                }
                if(group.Count > 1)
                {
                    // todo throw error due to multiple of the same name and cannot resolve duplicates
                }

                SampleInstrumentManager.Samples.AddRange(group.First().Samples);

                foreach(var sample in group.First().Samples)
                {
                    var standardizedPath = Helpers.Helpers.StandardizeFileDirectoryDelimiters(sample.Path);
                    var lastDirectorySeparator = (standardizedPath.Contains(@"\"))
                        ? standardizedPath.LastIndexOf(@"\")
                        : (standardizedPath.Contains(@"/"))
                            ? standardizedPath.LastIndexOf(@"/")
                            : 0;
                    var sampleName = standardizedPath[lastDirectorySeparator..];

                    //if (SampleNames.Contains(sampleName))
                    if (SampleInstrumentManager.ContainsSampleName(sampleName))
                    {
                        // todo notify duplicate sample name
                        continue;
                    }
                    else
                    {
                        SampleInstrumentManager.AddNewSampleName(sampleName);
                        //SampleNames.Add(sampleName);
                    }

                    SampleInstrumentManager.AddNewSample(sample);
                    //Samples.Add(sample);
                    Helpers.Helpers.LoadSampleToCache(_fileCachingService, sample);

                }
            }

            foreach (var sample in samplesPayload.Samples)
            {
                // get file extension
                if (sample.LastIndexOf(".") == -1)
                {
                    // todo handle missing file extension
                }

                var fileExtensionStartPosition = sample.LastIndexOf(".");
                var fileExtension = sample[fileExtensionStartPosition..];
                
                if(!FileNames.FileExtensions.ValidSampleExtensions.Contains(fileExtension))
                {
                    // todo handle invalid file extensions
                }

                if(fileExtension == FileNames.FileExtensions.SampleBank)
                {
                    // todo handle deprecated filetype
                    continue;
                }

                if(fileExtension == FileNames.FileExtensions.SampleBrr)
                {
                    var standardizedPath = Helpers.Helpers.StandardizeFileDirectoryDelimiters(sample);
                    var lastDirectorySeparator = (standardizedPath.Contains(@"\"))
                        ? standardizedPath.LastIndexOf(@"\")
                        : (standardizedPath.Contains(@"/"))
                            ? standardizedPath.LastIndexOf(@"/")
                            : 0;
                    var sampleName = standardizedPath[lastDirectorySeparator..];
                    var samplePath = Path.Combine(SongData.SongPath, sampleName);

                    //if(SampleNames.Contains(sampleName))
                    if(SampleInstrumentManager.ContainsSampleName(sampleName))
                    {
                        // todo notify duplicate sample
                        continue;
                    }

                    SampleInstrumentManager.AddNewSampleName(sampleName);
                    //SampleNames.Add(sampleName);
                    var sampleData = new AddmusicSample
                    {
                        Name = sampleName,
                        Path = samplePath,
                        IsImportant = false,
                        IsLooping = false,
                    };
                    SampleInstrumentManager.AddNewSample(sampleData);
                    //Samples.Add(sampleData);
                    Helpers.Helpers.LoadSampleToCache(_fileCachingService, sampleData);
                }
            }

            return new ValidationResult
            {
                Type = ValidationResult.ResultType.Success,
            };
        }


        #endregion

        #region Hex Command Validators

        public IValidationResult ValidateHexNode(HexNode hex)
        {
            return hex.CommandType switch
            {
                HexCommands.DDPitchBlend => ValidateDDPitchBlend(hex),
                HexCommands.FAHotPatchPreset or
                HexCommands.FAHotPatchToggleBits or
                HexCommands.FCHexRemoteCommand or
                HexCommands.FCHexRemoteGain => throw new Exception(),
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

        public IValidationResult ValidateDDPitchBlend(HexNode pitchBlend)
        {
            var messages = new List<string>();

            foreach (var hexNumber in pitchBlend.HexValues)
            {
                if (!Helpers.Helpers.IsHexInRange(Convert.ToByte(hexNumber.Replace("$", ""), 16)))
                {
                    messages.Add(_messageService.GetErrorHexCommandSuppliedValueOutOfRangeMessage(hexNumber, pitchBlend.HexCommand, 0, MagicNumbers.ByteHexMaximum));
                }
            }

            foreach (var node in pitchBlend.Children)
            {
                var validation = (ValidationResult)ValidateNode(node);
                if (validation.Type == ValidationResult.ResultType.Error ||
                    validation.Type == ValidationResult.ResultType.Warning)
                {
                    messages.AddRange(validation.Message);
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

        private void AddDataToChannel(byte dataToAdd)
        {
            var currentChannel = Channels.Where(c => c.ChannelNumber == CurrentChannel.ChannelNumber).First();
            if (InActiveLoop)
            {
                if (InActiveSubLoop)
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
                currentChannel.ChannelData.Add(dataToAdd);
            }
        }

        private void AddDataToChannel(List<byte> dataToAdd)
        {
            var currentChannel = Channels.Where(c => c.ChannelNumber == CurrentChannel.ChannelNumber).First();
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
                currentChannel.ChannelData.AddRange(dataToAdd);
            }
        }

        private void AddNoteLength(double ticks)
        {
            if (InActiveLoop)
            {
                if (InActiveSubLoop)
                {
                    ActiveSubLoopLength += ticks;
                }
                else
                {
                    ActiveLoopLength += ticks;
                }
            }
            else
            {
                CurrentChannel.ChannelLength = CurrentChannel.ChannelLength + ticks;
            }
        }

        private int DivideByTempoRatio(SongNode node, int value, bool isFractionalError)
        {
            if (TempoRatio == MagicNumbers.DefaultValues.InitialTempoRatio)
            {
                return value;
            }

            if (value % TempoRatio != 0)
            {
                if (isFractionalError)
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
            if (TempoRatio >= MagicNumbers.EightBitMaximum)
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
            else if (noteLength < 1 || noteLength > MagicNumbers.NoteLengthMaximum)
            {
                length = DefaultNoteLength;
            }
            else
            {
                if (MagicNumbers.NoteLengthMaximum % noteLength == 0)
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

            for (int i = 0; i < dotCount; i++)
            {
                if (fraction % 2 != 0)
                {
                    if (i != 0)
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

            if (inTriplet && allowTriplet == true)
            {
                if (fraction % 3 != 0)
                {
                    _messageService.GetWarningTripletFractionalTickValueFromDotsMessage(SongData.Name, node.LineNumber, node.ColumnNumber);
                }
                result = (int)Math.Floor(result * 2.0 / 3.0 + 0.5);
            }
            return result;
        }

        private void ApplyTempoRateAdjustmentAndQuantization(SongNode node, byte noteType, int noteLength)
        {
            if (noteLength >= DivideByTempoRatio(node, MagicNumbers.NoteLengthMaxBeforeSplit, true))
            {
                AddDataToChannel(Convert.ToByte(DivideByTempoRatio(node, MagicNumbers.NoteLengthDecreaseFactor, true)));

                if (InActiveLoop)
                {
                    if (InActiveSubLoop)
                    {
                        if (ActiveSubLoopInformation.UpdateQuantization)
                        {
                            AddDataToChannel(ActiveSubLoopInformation.CurrentQuantization);
                            ActiveSubLoopInformation.UpdateQuantization = false;
                            SongData.NoteParameterByteCount++;
                        }
                    }
                    else
                    {
                        if (ActiveLoopInformation.UpdateQuantization)
                        {
                            AddDataToChannel(ActiveLoopInformation.CurrentQuantization);
                            ActiveLoopInformation.UpdateQuantization = false;
                            SongData.NoteParameterByteCount++;
                        }
                    }
                }
                else
                {
                    if (CurrentChannel.UpdateQuantization)
                    {
                        AddDataToChannel(CurrentChannel.CurrentQuantization);
                        CurrentChannel.UpdateQuantization = false;
                        SongData.NoteParameterByteCount++;
                    }
                }

                AddDataToChannel(noteType);

                noteLength -= DivideByTempoRatio(node, MagicNumbers.NoteLengthDecreaseFactor, true);

                while (noteLength > DivideByTempoRatio(node, MagicNumbers.NoteLengthDecreaseFactor, true))
                {
                    AddDataToChannel(MagicNumbers.CommandValues.Tie);

                    noteLength -= DivideByTempoRatio(node, MagicNumbers.NoteLengthDecreaseFactor, true);
                }

                if (noteLength > 0)
                {
                    if (noteLength != DivideByTempoRatio(node, MagicNumbers.NoteLengthDecreaseFactor, true))
                    {
                        AddDataToChannel(Convert.ToByte(noteLength));
                    }

                    AddDataToChannel(MagicNumbers.CommandValues.Tie);
                }

                PreviousNoteLength = noteLength;
                return;
            }
            else if (noteLength > 0)
            {
                if (InActiveLoop)
                {
                    if (InActiveSubLoop)
                    {
                        if (ActiveSubLoopInformation.UpdateQuantization)
                        {
                            if (noteLength != PreviousNoteLength)
                            {
                                AddDataToChannel(Convert.ToByte(noteLength));
                            }
                            AddDataToChannel(ActiveSubLoopInformation.CurrentQuantization);
                            ActiveSubLoopInformation.UpdateQuantization = false;
                            SongData.NoteParameterByteCount++;
                        }
                    }
                    else
                    {
                        if (ActiveLoopInformation.UpdateQuantization)
                        {
                            if (noteLength != PreviousNoteLength)
                            {
                                AddDataToChannel(Convert.ToByte(noteLength));
                            }
                            AddDataToChannel(ActiveLoopInformation.CurrentQuantization);
                            ActiveLoopInformation.UpdateQuantization = false;
                            SongData.NoteParameterByteCount++;
                        }
                    }
                }
                else
                {
                    if (CurrentChannel.UpdateQuantization)
                    {
                        if (noteLength != PreviousNoteLength)
                        {
                            AddDataToChannel(Convert.ToByte(noteLength));
                        }
                        AddDataToChannel(CurrentChannel.CurrentQuantization);
                        CurrentChannel.UpdateQuantization = false;
                        SongData.NoteParameterByteCount++;
                    }
                }

                AddDataToChannel(noteType);
            }
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

        private int GetCurrentInstrument()
        {
            if (InActiveLoop)
            {
                if (InActiveSubLoop)
                {
                    return ActiveSubLoopInformation.CurrentInstrument;
                }
                else
                {
                    return ActiveLoopInformation.CurrentInstrument;
                }
            }
            else
            {
                return CurrentChannel.CurrentInstrument;
            }
        }

        private void SetCurrentInstrument(int instrument)
        {
            if (InActiveLoop)
            {
                if (InActiveSubLoop)
                {
                    ActiveSubLoopInformation.CurrentInstrument = instrument;
                }
                else
                {
                    ActiveLoopInformation.CurrentInstrument = instrument;
                }
            }
            else
            {
                CurrentChannel.CurrentInstrument = instrument;
            }
        }

        private void MarkEchoBufferAllocVCMD()
        {
            if(SongData.EchoBufferAlloVCMDIsSet
                || SongData.HasEchoBufferCommand
                || CurrentChannel.HasIntro
                || CurrentChannel.HasNoteData)
            {
                // todo handle warning for this case
                return;
            }

            SongData.EchoBufferAlloVCMDIsSet = true;
            SongData.EchoBufferAllocVCMDILocation = (ushort)(CurrentChannel.ChannelData.Count + 1);
            SongData.EchoBufferAllocVCMDIChannel = CurrentChannel.ChannelNumber;
        }

        #endregion


    }
}
