using Addmusic2.Exceptions;
using Addmusic2.Helpers;
using Addmusic2.Localization;
using Addmusic2.Model;
using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Addmusic2.Model.Localization;
using Addmusic2.Model.SongTree;
using Addmusic2.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Addmusic2.Parsers
{
    internal class SongParser : ISongParser
    {
        private readonly IAddmusicLogger _logger;
        private readonly MessageService _messageService;
        // private readonly SongListItem _songListItem;
        private readonly GlobalSettings _globalSettings;
        private readonly IFileCachingService _fileCachingService;
        private readonly SongScope _songScope;
        public SongData SongData { get; set; } = new SongData();

        public AddmusicKVersion AddmusicKVersion { get; set; } = AddmusicKVersion.Undefined;

        private SampleInstrumentManager SampleInstrumentManager { get; set; } = new();

        private List<ChannelInformation> Channels { get; set; } = new();
        private ChannelInformation LoopChannel { get; set; } = new();
        private List<byte> CurrentLoopData = new();
        private List<byte> CurrentSubLoopData = new();
        private Dictionary<string, LoopInformation> RemoteCodeDefinitions = new();
        private Dictionary<string, LoopInformation> NamedLoopDefinitions = new();
        private Dictionary<ushort, LoopInformation> UnnamedLoopDefinitions = new();
        private List<(double ChannelTick, int TempoChange)> TempoChanges = new();

        private ChannelInformation CurrentChannel { get; set; } = new();
        private List<byte> PreChannelData { get; set; } = new();

        private LoopNode PreviousLoop { get; set; } = new();
        private int PreviousLoopIndex { get; set; }
        private int PreviousNoteLength { get; set; }

        private int DefaultNoteLength { get; set; } = MagicNumbers.DefaultValues.InitialDefaultNoteLength;
        private int CurrentOctave { get; set; } = MagicNumbers.DefaultValues.StartingOctave;
        private int Tempo { get; set; } = MagicNumbers.DefaultValues.InitialTempoValue;
        private int TempoRatio { get; set; } = MagicNumbers.DefaultValues.InitialTempoRatio;
        private int HTranspose { get; set; } = 0;
        private bool UsingHTranspose { get; set; } = false;
        private bool TempoDefined { get; set; } = false;
        private bool InActiveLoop { get; set; } = false;
        private LoopInformation ActiveLoopInformation { get; set; } = new();
        private double ActiveLoopLength { get; set; } = 0;
        private bool InActiveSubLoop { get; set; } = false;
        private LoopInformation ActiveSubLoopInformation { get; set; } = new();
        private double ActiveSubLoopLength { get; set; } = 0;
        private bool InActiveSimpleLoop { get; set; } = false;
        private bool InActiveSuperLoop { get; set; } = false;
        private bool InPitchSlide { get; set; } = false;
        private bool ToggleLowNoteWarning { get; set; } = true;


        public SongParser(
            IAddmusicLogger logger,
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

            var amkVersion = nodes
                .Where(n => n.NodeType == SongNodeType.Amk)
                .ToList();
            var sampleNode = nodes
                .Where(n => n.NodeType == SongNodeType.Samples)
                .ToList();
            var channels = nodes
                .Where(n => n.NodeType == SongNodeType.Channel)
                .ToList();
            var specialDirectives = nodes
                .Where(n => n.NodeType != SongNodeType.Channel &&
                    n.NodeType != SongNodeType.Amk &&
                    n.NodeType != SongNodeType.Samples &&
                    n.GetType() == typeof(DirectiveNode)
                ).ToList();

            // If there is an AMK node, use that to determine the current version of AddmusicK to use
            //      if not default to <>
            
            if (amkVersion.Count == 1)
            {
                ParseNode((DirectiveNode)amkVersion.First());
            }
            else if (amkVersion.Count > 1)
            {
                // warning

                ParseNode((DirectiveNode)amkVersion.First());
            }
            else
            {
                AddmusicKVersion = AddmusicKVersion.Version4;
                SongData.VelocityTable = VelocityTable.NspcVTable;
            }

            if(sampleNode.Count == 0)
            {
                var samplesNode = new DirectiveNode
                {
                    NodeType = SongNodeType.Samples,
                    Payload = new SamplesPayload
                    {
                        SampleGroupPaths = ["#default"]
                    },
                };
                ValidateAndProcessSamplesDirectiveNode(samplesNode);
            }
            else if(sampleNode.Count > 1)
            {
                // todo throw error
                throw new AddmusicParserException();
            }
            else
            {
                ValidateAndProcessSamplesDirectiveNode((DirectiveNode)(sampleNode.First()));
            }


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
                ParseChannel(node, (Channels.Count == 0));
            }

            // Finish by transferring required information to the songdata object
            LoopChannel.ChannelNumber = 8;
            Channels.Add(LoopChannel);

            SongData.ChannelData = Channels;
            SongData.SampleInstrumentManager = SampleInstrumentManager;
            SongData.TempoChanges = TempoChanges;

            // Calculate the first pass pointers

            if (SongData.SongScope == SongScope.Local)
            {
                AddVersionAndEchoAdjustments();
            }

            // CalculateFirstPassPointers(SongData);

            return SongData;
        }

        public void ParseChannel(DirectiveNode channel, bool addPreChannelInfo = false)
        {
            var channelPayload = channel.Payload as ChannelPayload ?? throw new AddmusicParserException("Null Payload found");

            CurrentChannel = new ChannelInformation
            {
                ChannelNumber = channelPayload.ChannelNumber,
            };

            if (Channels.Exists(c => c.ChannelNumber == CurrentChannel.ChannelNumber))
            {
                _logger.LogError(LogLevel.Error, _messageService.GetErrorDuplicateChannelNumberFoundMessage(channelPayload.ChannelNumber.ToString()), true);
                throw new AddmusicParserException(_messageService.GetErrorDuplicateChannelNumberFoundMessage(channelPayload.ChannelNumber.ToString()));
            }

            // Add the prechannel information if previously defined
            if (addPreChannelInfo == true && Channels.Count == 0)
            {
                CurrentChannel.ChannelData.AddRange(PreChannelData);
            }

            Channels.Add(CurrentChannel);
            // Reset Octave as previous versions of Addmusic would carry over the octave
            //      from the previous channel
            // CurrentOctave = MagicNumbers.DefaultValues.StartingOctave;

            foreach (SongNode node in channel.Children)
            {
                ParseNode(node);
            }

            // add final 0byte to channel to indicate end
            AddDataToChannel(0);
        }

        public void ParseNode(SongNode node)
        {
            var validationResult = ValidateNode(node);

            if (validationResult.Type == ResultType.Skip)
            {
                _logger.LogWarning(LogLevel.Trace, _messageService.GetInfoLineAndColumnStringMessage(node.LineNumber.ToString(), node.ColumnNumber.ToString()) + " " + _messageService.GetErrorNodeValidationResultSkipMessage(node.NodeType.ToString(), node.ToString()));
                return;
            }
            else if (validationResult.Type == ResultType.Failure)
            {
                // Handles a case where a failure is returned by the validator

                _logger.LogError(LogLevel.Trace, _messageService.GetInfoLineAndColumnStringMessage(node.LineNumber.ToString(), node.ColumnNumber.ToString()) + " " + _messageService.GetErrorNodeValidationResultFailureMessage(node.NodeType.ToString(), node.ToString()));

                foreach (var message in validationResult.Message)
                {
                    _logger.LogError(LogLevel.Error, message, true);
                }
                return;
            }
            else if (validationResult.Type == ResultType.Warning)
            {
                // Handles a case where a warning is returned by the validator

                _logger.LogWarning(LogLevel.Trace, _messageService.GetInfoLineAndColumnStringMessage(node.LineNumber.ToString(), node.ColumnNumber.ToString()) + " " + _messageService.GetErrorNodeValidationResultWarningMessage(node.NodeType.ToString(), node.ToString()));

                foreach (var message in validationResult.Message)
                {
                    _logger.LogWarning(LogLevel.Warning, message, true);
                }
            }
            else if (validationResult.Type == ResultType.Error)
            {
                // Handles a case where an error is returned by the validator

                _logger.LogError(LogLevel.Trace, _messageService.GetInfoLineAndColumnStringMessage(node.LineNumber.ToString(), node.ColumnNumber.ToString()) + " " + _messageService.GetErrorNodeValidationResultErrorMessage(node.NodeType.ToString(), node.ToString()));

                foreach (var message in validationResult.Message)
                {
                    _logger.LogError(LogLevel.Error, message, true);
                }
                return;
            }

            EvaluateNode(node);
        }

        public IValidationResult ValidateNode(ISongNode songNode)
        {
            return songNode switch
            {
                null => throw new ArgumentNullException(nameof(songNode)),
                DirectiveNode => ValidateSpecialDirective((DirectiveNode)songNode),
                AtomicNode => ValidateAtomicNode((AtomicNode)songNode),
                CompositeNode => ValidateCompositeNode((CompositeNode)songNode),
                LoopNode => ValidateLoopNode((LoopNode)songNode),
                HexNode => ValidateHexNode((HexNode)songNode),
                _ => new ValidationResult
                {
                    Type = ResultType.Skip,
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
                    EvaluateSpecialDirective((DirectiveNode)songNode);
                    break;
                case AtomicNode:
                    EvaluateAtomicNode((AtomicNode)songNode);
                    break;
                case CompositeNode:
                    EvaluateCompositeNode((CompositeNode)songNode);
                    break;
                case LoopNode:
                    EvaluateLoopNode((LoopNode)songNode);
                    break;
                case HexNode:
                    EvaluateHexNode((HexNode)songNode);
                    break;
                default:
                    throw new AddmusicParserException("Unknown SongNode found");
            }
        }

        private ushort _loopPointer = 0x0000;
        public void CatalogueUserDefinedInformation(List<ISongNode> nodes)
        {
            // Get the names and associate loops of the named loops in the tree
            // Get the definition names of the remote code definitions in the tree
            foreach (SongNode node in nodes)
            {
                if (_loopPointer >= MagicNumbers.SixteenBitMaximum)
                {
                    _logger.LogError(LogLevel.Error, _messageService.GetErrorMaximumAllowedNumberOfLoopsReachedMessage(), true);
                    throw new AddmusicParserException(_messageService.GetErrorMaximumAllowedNumberOfLoopsReachedMessage());
                }

                if(node is LoopNode)
                {
                    if (node.NodeType == SongNodeType.SimpleLoop)
                    {
                        var loopName = ((LoopNode)node).LoopName;
                        var hasLoopName = loopName.Length > 0;
                        if (hasLoopName)
                        {
                            if (NamedLoopDefinitions.ContainsKey(loopName))
                            {
                                _logger.LogError(LogLevel.Error, _messageService.GetErrorDuplicateLoopNameDefinedMessage(loopName), true);
                                throw new AddmusicParserException(_messageService.GetErrorDuplicateLoopNameDefinedMessage(loopName));
                            }
                            NamedLoopDefinitions.Add(loopName, new LoopInformation
                            {
                                LoopId = _loopPointer,
                                LoopNode = (LoopNode)node,
                            });
                        }
                        else
                        {
                            UnnamedLoopDefinitions.Add(_loopPointer, new LoopInformation
                            {
                                LoopId = _loopPointer,
                                LoopNode = (LoopNode)node,
                            });
                        }

                        _loopPointer = (ushort)(_loopPointer + 1);
                        CatalogueUserDefinedInformation(((LoopNode)node).LoopContents);
                    }
                    else if (node.NodeType == SongNodeType.SuperLoop)
                    {
                        CatalogueUserDefinedInformation(((LoopNode)node).LoopContents);
                    }
                    else if (node.NodeType == SongNodeType.RemoteCode)
                    {
                        var payload = node.Payload as RemoteCodeDefinitionPayload;
                        var definitionName = payload!.DefinitionName;
                        if (NamedLoopDefinitions.ContainsKey(definitionName))
                        {
                            _logger.LogError(LogLevel.Error, _messageService.GetErrorDuplicateRemoteCodeDefinitionNameDefinedMessage(definitionName), true);
                            throw new AddmusicParserException(_messageService.GetErrorDuplicateRemoteCodeDefinitionNameDefinedMessage(definitionName));
                        }
                        RemoteCodeDefinitions.Add(definitionName, new LoopInformation
                        {
                            LoopId = _loopPointer,
                            LoopNode = (LoopNode)node,
                        });
                        _loopPointer = (ushort)(_loopPointer + 1);
                    }
                }
                else
                {
                    if(node.Children != null && node.Children.Count > 0)
                    {
                        CatalogueUserDefinedInformation(node.Children);
                    }
                }
            }
        }

        private void AddVersionAndEchoAdjustments()
        {
            var firstChannel = SongData.ChannelData.First();
            var loopAdjustmentAmount = 0;

            if(SongData.AmkVersion > AddmusicKVersion.Version1)
            {
                firstChannel.ChannelData = MagicNumbers.ChannelAdjustmentBytes.Concat(firstChannel.ChannelData).ToList();
                loopAdjustmentAmount = MagicNumbers.ChannelAdjustmentBytes.Count;
            }

            if(SongData.AmkVersion == AddmusicKVersion.Version1)
            {
                firstChannel.ChannelData = MagicNumbers.AmkVersion1ChannelAdjustmentBytes.Concat(firstChannel.ChannelData).ToList();
                loopAdjustmentAmount = MagicNumbers.AmkVersion1ChannelAdjustmentBytes.Count;
            }
            else if(SongData.AmkParserVersion == AddmusicKParserVersion.Version1)
            {
                firstChannel.ChannelData = MagicNumbers.AmkParserVersion1ChannelAdjustmentBytes.Concat(firstChannel.ChannelData).ToList();
                loopAdjustmentAmount = MagicNumbers.AmkParserVersion1ChannelAdjustmentBytes.Count;
            }
            else if (SongData.AmkParserVersion == AddmusicKParserVersion.Version2)
            {
                firstChannel.ChannelData = MagicNumbers.AmkParserVersion2ChannelAdjustmentBytes.Concat(firstChannel.ChannelData).ToList();
                loopAdjustmentAmount = MagicNumbers.AmkParserVersion2ChannelAdjustmentBytes.Count;
            }


            if (SongData.EchoBufferSize > 0 || !SongData.EchoBufferAllocVCMDIsSet || SongData.HasEchoBufferCommand)
            {
                //Just put the VCMD in its default place: no need to move it around.
                //In particular, the $F1 command means that echo writes have been enabled, meaning the special case is irrelevant.
                firstChannel.ChannelData = MagicNumbers.EchoBufferAdjustmentBytes(Convert.ToByte(SongData.EchoBufferSize)).Concat(firstChannel.ChannelData).ToList();
                loopAdjustmentAmount += MagicNumbers.EchoBufferAdjustmentBytes(Convert.ToByte(SongData.EchoBufferSize)).Count;
            }
            else
            {
                var echoBufferChannel = SongData.ChannelData.Find(c => c.ChannelNumber == SongData.EchoBufferAllocVCMDIChannel) ?? throw new AddmusicParserException("Echo Channel missing");
                var echoAdjustmentBytes = MagicNumbers.EchoBufferAdjustmentBytes(Convert.ToByte(SongData.EchoBufferSize));
                echoBufferChannel.ChannelData.InsertRange(
                    SongData.EchoBufferAllocVCMDILocation + echoAdjustmentBytes.Count,
                    echoAdjustmentBytes
                );

                echoBufferChannel.LoopLocations.ForEach(ll => ll += (byte)echoAdjustmentBytes.Count);
                echoBufferChannel.PhraseLocation += (byte)echoAdjustmentBytes.Count;
                echoBufferChannel.IntroLocation += (byte)echoAdjustmentBytes.Count;

            }

            firstChannel.LoopLocations.ForEach(ll => ll += (byte)loopAdjustmentAmount);

            foreach (var channel in SongData.ChannelData)
            {
                channel.PhraseLocation += (byte)loopAdjustmentAmount;
                channel.IntroLocation += (byte)loopAdjustmentAmount;
            }
        }

        public void CalculateFirstPassPointers(SongData songData)
        {
            var channels = songData.ChannelData;
            var firstChannel = channels.First();
            var channelsWithNoData = channels.FindAll(c => c.ChannelData.Count == 0);
            if(channelsWithNoData.Count == channels.Count)
            {
                _logger.LogError(LogLevel.Error, _messageService.GetErrorNoSongChannelDataToExportMessage(songData.Name), true);
                throw new AddmusicParserException(_messageService.GetErrorNoSongChannelDataToExportMessage(songData.Name));
            }

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
                    var channel = songData.ChannelData.Find(c => c.ChannelNumber == i) ?? throw new AddmusicParserException("Channel missing"); // should never throw this exception
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

            var combinedData = Enumerable.Repeat((byte)0, offset).Cast<byte>().ToList();

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
                combinedData[offset - 4] = (byte)(MagicNumbers.ByteHexMaximum - 1);
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
                .Select(s => ( MagicNumbers.SampleSCRNTableSize + s.Data.Count ))
                .Sum();
            
            // todo Generate statistics file

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
                    throw new AddmusicParserException("Invalid Atomic Node Type found");
            }
        }

        public void EvaluateInstrumentNode(AtomicNode instrumentNode)
        {
            var instrumentPayload = instrumentNode.Payload as InstrumentPayload ?? throw new AddmusicParserException("Null Payload found");

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

                var instrumentDefined = SampleInstrumentManager.ContainsInstrument(instrumentNumber);

                if(!instrumentDefined)
                {
                    _logger.LogError(LogLevel.Error, _messageService.GetErrorUndefinedInstrumentMessage(instrumentNumber.ToString()), true);
                    throw new AddmusicParserException(_messageService.GetErrorUndefinedInstrumentMessage(instrumentNumber.ToString()));
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
                        _logger.LogError(LogLevel.Error, _messageService.GetErrorUndefinedInstrumentMessage(instrumentNumber.ToString()), true);
                        throw new AddmusicParserException(_messageService.GetErrorUndefinedInstrumentMessage(instrumentNumber.ToString()));
                    }
                }
                else if (instrumentNumber >= MagicNumbers.StartingCustomInstrumentNumber)
                {
                    var useInstrument = SampleInstrumentManager.UseInstrument(instrumentNumber);

                    if (!useInstrument)
                    {
                        _logger.LogError(LogLevel.Error, _messageService.GetErrorUndefinedInstrumentMessage(instrumentNumber.ToString()), true);
                        throw new AddmusicParserException(_messageService.GetErrorUndefinedInstrumentMessage(instrumentNumber.ToString()));
                    }
                }

                if (AddmusicKVersion == AddmusicKVersion.Version1)
                {
                    CurrentChannel.IgnoreTuning = false;
                }

                AddDataToChannel(MagicNumbers.CommandValues.Instrument);
                AddDataToChannel(Convert.ToByte(instrumentNumber));
            }

            SetCurrentInstrument(instrumentNumber);

            if (AddmusicKVersion == AddmusicKVersion.Version2 && instrumentNumber <= 18)
            {
                HTranspose = 0;
                UsingHTranspose = false;
                SongData.TransposeMap[instrumentNumber] = MagicNumbers.TempTrans[instrumentNumber];
            }
        }

        public void EvaluateNoteNode(AtomicNode noteNode, bool inTriplet = false, bool inPitchSlide = false, bool isNextForDDPitchSlide = false)
        {
            CurrentChannel.HasNoteData = true;
            var notePayload = noteNode.Payload as NotePayload ?? throw new AddmusicParserException("Null Payload found");

            var noteValueChar = (int)notePayload.NoteValue[0];
            var note = GetPitchValue(noteValueChar, notePayload.Accidental);
            var currentInstrument = GetCurrentInstrument();
            if (UsingHTranspose)
            {
                note += HTranspose;
            }
            else
            {
                if(CurrentChannel.IgnoreTuning != true)
                {
                    note -= SongData.TransposeMap[currentInstrument];
                }
            }

            if (note < MagicNumbers.NoteLengthMaxBeforeSplit)
            {
                if (AddmusicKVersion != AddmusicKVersion.Version4)
                {
                    if(ToggleLowNoteWarning)
                    {
                        _logger.LogWarning(LogLevel.Warning, _messageService.GetWarningOldAddmusicLowNoteMessage());
                        ToggleLowNoteWarning = false;
                    }
                }
                else
                {
                    note = MagicNumbers.CommandValues.Rest;
                }
            }
            else if (note >= MagicNumbers.CommandValues.Tie)
            {
                _logger.LogError(LogLevel.Error, _messageService.GetErrorNotePitchTooLowMessage());
                throw new AddmusicParserException(_messageService.GetErrorNotePitchTooLowMessage());
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

            // get the starting Note Length of the current note

            var tempLength = GetNoteLength(noteNode, notePayload.Duration, notePayload.DotCount, inTriplet, true, notePayload.HasEquals, notePayload.UseDefaultLength);

            foreach (var tieNode in notePayload.ConnectedTies)
            {
                var tiePayload = tieNode.Payload as TiePayload ?? throw new AddmusicParserException("Null Payload found");

                if(tiePayload.TieList.Count > 0)
                {
                    foreach (var tie in tiePayload.TieList)
                    {
                        var tieDuration = (tie.UseDefaultNoteLength == true) ? DefaultNoteLength : tie.Duration;
                        tempLength += GetNoteLength(tieNode, tieDuration, tie.DotCount, inTriplet, true, tie.HasEquals, tie.UseDefaultNoteLength);
                    }
                }
                else
                {
                    var tieDuration = (tiePayload.UseDefaultNoteLength == true) ? DefaultNoteLength : tiePayload.Duration;
                    tempLength += GetNoteLength(tieNode, tieDuration, tiePayload.DotCount, inTriplet, true, tiePayload.HasEquals, tiePayload.UseDefaultNoteLength);
                }
            }

            tempLength = DivideByTempoRatio(noteNode, tempLength, true);

            AddNoteLength(tempLength);

            ApplyTempoRateAdjustmentAndQuantization(noteNode, Convert.ToByte(note), tempLength);
        }

        public void EvaluateTieNode(AtomicNode tieNode, bool inTriplet = false, bool inPitchSlide = false, bool isNextForDDPitchSlide = false)
        {
            CurrentChannel.HasNoteData = true;
            var tiePayload = tieNode.Payload as TiePayload ?? throw new AddmusicParserException("Null Payload found");
            var tempLength = 0;
            if (tiePayload.TieList.Count > 0)
            {
                foreach (var tie in tiePayload.TieList)
                {
                    var tieDuration = (tie.UseDefaultNoteLength == true) ? DefaultNoteLength : tie.Duration;
                    tempLength += GetNoteLength(tieNode, tieDuration, tie.DotCount, inTriplet, true, tie.HasEquals, tie.UseDefaultNoteLength);
                }
            }
            else
            {
                var tieDuration = (tiePayload.UseDefaultNoteLength == true) ? DefaultNoteLength : tiePayload.Duration;
                tempLength += GetNoteLength(tieNode, tieDuration, tiePayload.DotCount, inTriplet, true, tiePayload.HasEquals, tiePayload.UseDefaultNoteLength);
            }

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

            tempLength = DivideByTempoRatio(tieNode, tempLength, true);

            AddNoteLength(tempLength);

            ApplyTempoRateAdjustmentAndQuantization(tieNode, MagicNumbers.CommandValues.Tie, tempLength);
        }

        public void EvaluateRestNode(AtomicNode restNode, bool inTriplet = false, bool inPitchSlide = false, bool isNextForDDPitchSlide = false)
        {
            CurrentChannel.HasNoteData = true;
            var restPayload = restNode.Payload as RestPayload ?? throw new AddmusicParserException("Null Payload found");

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

            var tempLength = 0;

            if(restPayload.ConnectedRests.Count == 0)
            {
                // get the starting Note Length of the current note
                tempLength += GetNoteLength(restNode, restPayload.Duration, restPayload.DotCount, inTriplet, true, restPayload.HasEquals, restPayload.UseDefaultLength);

                foreach (var tieNode in restPayload.ConnectedTies)
                {
                    var tiePayload = tieNode.Payload as TiePayload ?? throw new AddmusicParserException("Null Payload found");

                    if (tiePayload.TieList.Count > 0)
                    {
                        foreach (var tie in tiePayload.TieList)
                        {
                            var tieDuration = (tie.UseDefaultNoteLength == true) ? DefaultNoteLength : tie.Duration;
                            tempLength += GetNoteLength(tieNode, tieDuration, tie.DotCount, inTriplet, true, tie.HasEquals, tie.UseDefaultNoteLength);
                        }
                    }
                    else
                    {
                        var tieDuration = (tiePayload.UseDefaultNoteLength == true) ? DefaultNoteLength : tiePayload.Duration;
                        tempLength += GetNoteLength(tieNode, tieDuration, tiePayload.DotCount, inTriplet, true, tiePayload.HasEquals, tiePayload.UseDefaultNoteLength);
                    }
                }

                //tempLength = DivideByTempoRatio(restNode, tempLength, true);
            }
            else
            {
                foreach (var connectedRest in  restPayload.ConnectedRests)
                {
                    var connectedRestPayload = connectedRest.Payload as RestPayload ?? throw new AddmusicParserException("Null Payload found");

                    tempLength += GetNoteLength(connectedRest, connectedRestPayload.Duration, connectedRestPayload.DotCount, inTriplet, true, connectedRestPayload.HasEquals, connectedRestPayload.UseDefaultLength);

                    foreach (var tieNode in connectedRestPayload.ConnectedTies)
                    {
                        var tiePayload = tieNode.Payload as TiePayload ?? throw new AddmusicParserException("Null Payload found");

                        if (tiePayload.TieList.Count > 0)
                        {
                            foreach (var tie in tiePayload.TieList)
                            {
                                var tieDuration = (tie.UseDefaultNoteLength == true) ? DefaultNoteLength : tie.Duration;
                                tempLength += GetNoteLength(tieNode, tieDuration, tie.DotCount, inTriplet, true, tie.HasEquals, tie.UseDefaultNoteLength);
                            }
                        }
                        else
                        {
                            var tieDuration = (tiePayload.UseDefaultNoteLength == true) ? DefaultNoteLength : tiePayload.Duration;
                            tempLength += GetNoteLength(tieNode, tieDuration, tiePayload.DotCount, inTriplet, true, tiePayload.HasEquals, tiePayload.UseDefaultNoteLength);
                        }
                    }

                    //tempLength = DivideByTempoRatio(connectedRest, tempLength, true);
                }
            }

            tempLength = DivideByTempoRatio(restNode, tempLength, true);

            AddNoteLength(tempLength);

            ApplyTempoRateAdjustmentAndQuantization(restNode, MagicNumbers.CommandValues.Rest, tempLength);
        }

        public void EvaluateDefaultLengthNode(AtomicNode defaultLengthNode)
        {
            var defaultLengthPayload = defaultLengthNode.Payload as DefaultLengthPayload ?? throw new AddmusicParserException("Null Payload found");

            if (defaultLengthPayload.UsedEquals && AddmusicKVersion >= AddmusicKVersion.Version4)
            {
                DefaultNoteLength = defaultLengthPayload.Length;
            }
            else
            {
                DefaultNoteLength = MagicNumbers.NoteLengthMaximum / defaultLengthPayload.Length;
            }

            if(AddmusicKVersion == AddmusicKVersion.Version4)
            {
                DefaultNoteLength = GetNoteLengthModifier(defaultLengthNode, DefaultNoteLength, default, false, false);
            }
        }

        public void EvaluateGlobalVolumeNode(AtomicNode globalVolumeNode)
        {
            var globalVolumePayload = globalVolumeNode.Payload as VolumePayload ?? throw new AddmusicParserException("Null Payload found");

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
            var volumePayload = volumeNode.Payload as VolumePayload ?? throw new AddmusicParserException("Null Payload found");

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
                _logger.LogWarning(LogLevel.Warning, _messageService.GetWarningOctaveDroppedTooLowMessage());
            }
        }

        public void EvaluateOctaveNode(AtomicNode octaveNode)
        {
            var octavePayload = octaveNode.Payload as OctavePayload ?? throw new AddmusicParserException("Null Payload found");

            CurrentOctave = octavePayload.OctaveNumber;
        }

        public void EvaluateNoiseNode(AtomicNode noiseNode)
        {
            var noisePayload = noiseNode.Payload as NoisePayload ?? throw new AddmusicParserException("Null Payload found");

            AddDataToChannel(MagicNumbers.CommandValues.Noise);
            AddDataToChannel(Convert.ToByte(noisePayload.NoiseValue));
        }

        public void EvaluatePanNode(AtomicNode panNode)
        {
            var panPayload = panNode.Payload as PanPayload ?? throw new AddmusicParserException("Null Payload found");

            if(panPayload.HexSourced == false)
            {
                var panValue = panPayload.PanPosition;

                if(panPayload.HasSurroundSound == true)
                {
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
                }

                AddDataToChannel(MagicNumbers.CommandValues.Pan);
                AddDataToChannel(Convert.ToByte(panValue));
            }
            else
            {
                if(panPayload.IsHexPanFade)
                {
                    AddDataToChannel(MagicNumbers.CommandValues.PanFade);
                    AddDataToChannel((byte)panPayload.HexDuration);
                    AddDataToChannel((byte)panPayload.HexFinalPanValue);
                }
                else
                {
                    AddDataToChannel(MagicNumbers.CommandValues.Pan);
                    AddDataToChannel((byte)panPayload.HexDuration);
                }
                
            }
        }

        public void EvaluateQuantizationNode(AtomicNode quantizationNode)
        {
            var quantizationPayload = quantizationNode.Payload as QuantizationPayload ?? throw new AddmusicParserException("Null Payload found");

            // If there is a volume node then we need to only use the delay
            // If there is no volume node then the quantization value is a HexNumber since the delay value is limited to 0->7
            var quantizationValue = quantizationPayload.VolumeNode == null
                ? Convert.ToByte($"{quantizationPayload.DelayValue}{quantizationPayload.VolumeValue}", 16)
                : Convert.ToByte(quantizationPayload.DelayValue);

            if (InActiveLoop)
            {
                if (InActiveSubLoop)
                {
                    ActiveSubLoopInformation.CurrentQuantization = quantizationValue;
                    ActiveSubLoopInformation.UpdateQuantization = true;
                }
                else
                {
                    ActiveLoopInformation.CurrentQuantization = quantizationValue;
                    ActiveLoopInformation.UpdateQuantization = true;
                }
            }

            // Always update the current channel's quantization when using a quantization node
            CurrentChannel.CurrentQuantization = quantizationValue;
            CurrentChannel.UpdateQuantization = true;
        }

        // Maybe not used anymore???
        public void EvaluateQuestionMarkOrNoLoopNode(AtomicNode questionMarkNode)
        {
            if (questionMarkNode.NodeType == SongNodeType.NoLoopCommand)
            {
                SongData.DoesntLoop = true;
                return;
            }

            var questionMarkPayload = questionMarkNode.Payload as QuestionMarkPayload ?? throw new AddmusicParserException("Null Payload found");

            if (questionMarkPayload.MarkNumber == 0)
            {
                SongData.DoesntLoop = true;
            }
            else if (questionMarkPayload.MarkNumber == 1)
            {
                SongData.NoMusic[CurrentChannel.ChannelNumber, 0] = true;
                CurrentChannel.NoMusic = true;
            }
            else if (questionMarkPayload.MarkNumber == 2)
            {
                SongData.NoMusic[CurrentChannel.ChannelNumber, 1] = true;
                CurrentChannel.NoMusic = true;
            }
        }

        public void EvaluateRaiseOctaveNode(AtomicNode raiseOctaveNode)
        {
            CurrentOctave++;
            if (CurrentOctave > MagicNumbers.OctaveMaximum)
            {
                CurrentOctave = MagicNumbers.OctaveMaximum;
                _logger.LogWarning(LogLevel.Warning, _messageService.GetWarningOctaveRaisedTooHighMessage());
            }
        }

        public void EvaluateTempoNode(AtomicNode tempoNode)
        {
            var tempoPayload = tempoNode.Payload as TempoPayload ?? throw new AddmusicParserException("Null Payload found");

            var tempoValue = tempoPayload.Tempo;
            var tempoDuration = tempoPayload.FadeValue;

            var tempo = DivideByTempoRatio(tempoNode, tempoValue, false);

            if (tempo == 0)
            {
                _logger.LogWarning(LogLevel.Warning, _messageService.GetWarningTempoZeroedByOptionMessage());
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
            var tunePayload = tuneNode.Payload as TunePayload ?? throw new AddmusicParserException("Null Payload found");

            HTranspose = tunePayload.TuneValue;
            UsingHTranspose = true;
        }

        public void EvaluateVibratoNode(AtomicNode vibratoNode)
        {
            var vibratoPayload = vibratoNode.Payload as VibratoPayload ?? throw new AddmusicParserException("Null Payload found");

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
                    EvaluateTripletNode(compositeNode);
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
                    throw new AddmusicParserException("Invalid Note Type Found");
            }
        }

        public void EvaluateHexCommand(CompositeNode node)
        {
            var hexPayload = node.Payload as HexNumberPayload ?? throw new AddmusicParserException("Null Payload found");

            var byteData = Convert.ToByte(hexPayload.HexValue, 16);

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

            PreviousNoteLength = -1;

            SongData.HasIntro = true;
            SongData.IntroLength = (int)CurrentChannel.ChannelLength;

            CurrentChannel.HasIntro = true;
            CurrentChannel.IntroLocation = (byte)CurrentChannel.ChannelData.Count;
            CurrentChannel.IntroLength = (int)CurrentChannel.ChannelLength;
        }

        public void EvaluatePitchSlideNode(CompositeNode node)
        {
            var pitchSlidePayload = node.Payload as PitchSlidePayload ?? throw new AddmusicParserException("Null Payload found");

            foreach (SongNode songNode in pitchSlidePayload.Nodes)
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
            var sampleloadPayload = node.Payload as SampleLoadPayload ?? throw new AddmusicParserException("Null Payload found");

            byte finalSampleIndex = 0x00;

            // The Sample Load command was passed a named sample
            if (sampleloadPayload.SampleNumber == -1)
            {
                var standardizedPath = Helpers.Helpers.StandardizeFileDirectoryDelimiters(sampleloadPayload.SampleName);
                var lastDirectorySeparator = Helpers.Helpers.GetLastDirectorySeparatorIndex(standardizedPath);
                var sampleName = standardizedPath[lastDirectorySeparator..];
                var samplePath = Path.Combine(SongData.SongPath, sampleName);

                SampleInstrumentManager.AddNewSampleName(sampleName);
                var sampleData = new Sample
                {
                    Name = sampleName,
                    Path = samplePath,
                    IsImportant = false,
                    IsLooping = false,
                };
                Helpers.Helpers.LoadSampleToCache(_logger, _fileCachingService, sampleData);
                var data = _fileCachingService.GetFromCache(sampleName);
                SampleInstrumentManager.AddNewSample(sampleData);
                SampleInstrumentManager.UseSample(sampleData);

                var sampleIndex = SampleInstrumentManager.Samples.FindIndex(s => s.Name == sampleName);

                if (sampleIndex == -1)
                {
                    // todo throw error // this should never get caught because other errors should happen first
                    throw new Exception("Null Payload found");
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

        public void EvaluateTripletNode(CompositeNode node)
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
                    throw new AddmusicParserException("Invalid Loop Node Type found");
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
            var remoteCodePayload = callRemoteCodeNode.Payload as CallRemoteCodePayload ?? throw new AddmusicParserException("Null Payload found");

            var definitionName = remoteCodePayload.DefinitionName;
            if (!RemoteCodeDefinitions.TryGetValue(definitionName, out LoopInformation? loopValue))
            {
                _logger.LogError(LogLevel.Error, _messageService.GetErrorUndefinedRemoteCodeCallMessage(), true);
                throw new AddmusicParserException(_messageService.GetErrorUndefinedRemoteCodeCallMessage());
            }

            var eventNumber = remoteCodePayload.EventType;

            var databyte = 0;
            if (eventNumber == 1 || eventNumber == 2)
            {
                if (remoteCodePayload.IntArgument == -1)
                {
                    databyte = Convert.ToByte(remoteCodePayload.HexArgument, 16);
                }
                else
                {
                    databyte = Convert.ToByte(GetNoteLengthModifier(callRemoteCodeNode, remoteCodePayload.IntArgument, 0, false, false));
                    if (databyte > MagicNumbers.HexCommandMaximum)
                    {
                        _logger.LogError(LogLevel.Error, _messageService.GetErrorHexCommandValueOutOfRangeMessage(remoteCodePayload.HexArgument, 0, MagicNumbers.HexCommandMaximum));
                        throw new AddmusicParserException(_messageService.GetErrorHexCommandValueOutOfRangeMessage(remoteCodePayload.HexArgument, 0, MagicNumbers.HexCommandMaximum));
                    }
                    else if(databyte == MagicNumbers.HexCommandMaximum)
                    {
                        databyte = 0;
                    }
                }
            }

            var remoteCodeLocation = loopValue.LoopId;
            AddDataToChannel(MagicNumbers.CommandValues.RemoteCode);
            //CurrentChannel.LoopLocations.Add(Convert.ToByte(CurrentChannel.ChannelData.Count));
            CurrentChannel.LoopLocations.Add((ushort)CurrentChannel.ChannelData.Count);
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

            //EvaluateSimpleLoopNode(previousNode);

            var loopInformation = (previousNode.LoopName.Length > 0)
                ? NamedLoopDefinitions[previousNode.LoopName]
                : UnnamedLoopDefinitions.Values.Where(d => d.LoopNode == PreviousLoop).FirstOrDefault() ?? throw new AddmusicParserException("Uncatalogued LoopNode");

            CurrentChannel.UpdateQuantization = true;

            if (InActiveLoop == true)
            {
                ActiveLoopInformation.UpdateQuantization = true;
            }

            var loopLocation = loopInformation.LoopLocation;
            AddDataToChannel(MagicNumbers.CommandValues.Loop);
            //CurrentChannel.LoopLocations.Add(Convert.ToByte(CurrentChannel.ChannelData.Count));
            CurrentChannel.LoopLocations.Add((ushort)CurrentChannel.ChannelData.Count);
            AddDataToChannel((byte)(loopLocation & MagicNumbers.HexCommandMaximum));
            AddDataToChannel((byte)(loopLocation >> 8));
            AddDataToChannel((byte)(previousNode.Iterations));
        }

        public void EvaluateSimpleLoopNode(LoopNode simpleLoopNode)
        {
            // Begin Simple Loop
            if (InActiveLoop == true)
            {
                InActiveSubLoop = true;
            }
            InActiveLoop = true;
            InActiveSimpleLoop = true;

            // get the current LoopInformation that corresponds to this LoopNode

            var loopName = simpleLoopNode.LoopName;
            var loopInformation = (loopName.Length > 0)
                ? NamedLoopDefinitions[loopName]
                : UnnamedLoopDefinitions.Values.Where(d => d.LoopNode == simpleLoopNode).FirstOrDefault() ?? throw new AddmusicParserException("Uncatalogued LoopNode");
            // This is a new loop definition so store the current size of the loop channel as the location
            //      of this loop's data so it can be referenced later
            loopInformation.LoopLocation = LoopChannel.ChannelData.Count;
            loopInformation.CurrentInstrument = CurrentChannel.CurrentInstrument;

            // Toggle Quanitization on current channel
            loopInformation.UpdateQuantization = true; // likely redundant
            loopInformation.CurrentQuantization = CurrentChannel.CurrentQuantization;
            CurrentChannel.UpdateQuantization = true;

            if (InActiveSubLoop == true)
            {
                ActiveLoopInformation.UpdateQuantization = true;
                ActiveSubLoopInformation = loopInformation;
            }
            else
            {
                ActiveLoopInformation = loopInformation;
            }

            // Evaluate the contents of the loop

            foreach (var node in simpleLoopNode.LoopContents)
            {
                EvaluateNode(node);
            }

            // Add a 0 to the end of this loop definition
            AddDataToChannel(0);

            // Clean up state and finish loop evaluation
            if (InActiveSubLoop == true)
            {
                InActiveSubLoop = false;
                ActiveSubLoopLength = 0;
                LoopChannel.ChannelData.AddRange(CurrentSubLoopData);
                // Reset this collection since the list items have been transfered
                CurrentSubLoopData.Clear();
            }
            else
            {
                InActiveLoop = false;
                ActiveLoopLength = 0;
                LoopChannel.ChannelData.AddRange(CurrentLoopData);
                // Reset this collection since the list items have been transfered
                CurrentLoopData.Clear();
            }
            InActiveSimpleLoop = false;
            PreviousLoop = simpleLoopNode;
            PreviousLoopIndex = LoopChannel.ChannelData.Count;

            // Finish loop and store reference info in source channel
            var loopLocation = loopInformation.LoopLocation;
            AddDataToChannel(MagicNumbers.CommandValues.Loop);
            //CurrentChannel.LoopLocations.Add(Convert.ToByte(CurrentChannel.ChannelData.Count));
            // Store the location of the loop invocation in the current channel
            CurrentChannel.LoopLocations.Add((ushort)LoopChannel.ChannelData.Count);
            AddDataToChannel((byte)(loopLocation & MagicNumbers.HexCommandMaximum));
            AddDataToChannel((byte)(loopLocation >> 8));
            AddDataToChannel((byte)(simpleLoopNode.Iterations));
        }

        public void EvaluateSuperLoopNode(LoopNode superLoopNode)
        {
            var superLoopPayload = superLoopNode.Payload as SuperLoopPayload ?? throw new AddmusicParserException("Null Payload found");

            // Begin Super Loop
            if (InActiveLoop == true)
            {
                InActiveSubLoop = true;
            }
            InActiveLoop = true;
            InActiveSuperLoop = true;

            var loopInformation = new LoopInformation
            {
                LoopNode = superLoopNode,
                CurrentInstrument = CurrentChannel.CurrentInstrument
            };
            // If the current channel has updates to the quantization, carry that over to the super loop contents
            loopInformation.UpdateQuantization = true;
            ;
            if (InActiveSubLoop == true)
            {
                ActiveSubLoopInformation = loopInformation;
                loopInformation.CurrentQuantization = ActiveLoopInformation.CurrentQuantization;
            }
            else
            {
                ActiveLoopInformation = loopInformation;
                loopInformation.CurrentQuantization = CurrentChannel.CurrentQuantization;
            }

            // Add data to the current channel before handling super loop

            AddDataToChannel(MagicNumbers.CommandValues.SuperLoop);
            AddDataToChannel(0x00);

            // Evaluate the contents of the loop

            foreach (var node in superLoopNode.LoopContents)
            {
                EvaluateNode(node);
            }

            // Finish loop
            AddDataToChannel(MagicNumbers.CommandValues.SuperLoop);
            // Super Loop's Iteration Number **does** need to be decremented by 1 unless initialized from a hex command
            if(superLoopPayload.FromHex == true)
            {
                if(InActiveSubLoop)
                {
                    ActiveLoopInformation.UpdateQuantization = false;
                }
                else
                {
                    CurrentChannel.UpdateQuantization = false;
                }
                AddDataToChannel((byte)(superLoopNode.Iterations));
            }
            else
            {
                if (InActiveSubLoop)
                {
                    ActiveLoopInformation.UpdateQuantization = true;
                }
                else
                {
                    CurrentChannel.UpdateQuantization = true;
                }
                AddDataToChannel((byte)(superLoopNode.Iterations - 1));
            }

            // Clean up state and finish loop evaluation
            if (InActiveSubLoop == true)
            {
                InActiveSubLoop = false;
                ActiveSubLoopLength = 0;
                // Add the current loop data to the loop that contains this super loop
                CurrentLoopData.AddRange(CurrentSubLoopData);
                CurrentSubLoopData.Clear();
            }
            else
            {
                InActiveLoop = false;
                ActiveLoopLength = 0;
                // Don't add the loop data as it was already inserted into the main channel
            }
            InActiveSuperLoop = false;
            //PreviousLoop = superLoopNode;
            //PreviousLoopIndex = LoopChannel.ChannelData.Count;

            //CurrentChannel.UpdateQuantization = true;
        }

        public void EvaluateCallLoopDefinitionNode(LoopNode callLoopNode)
        {
            var calledLoopInformation = NamedLoopDefinitions[callLoopNode.LoopName];
            var calledLoopNode = (LoopNode)calledLoopInformation.LoopNode.Clone();

            // This loop invocation may have a different number of iterations than the original definition
            //      Potentially none at all, if so then it only needs to be called once
            if (callLoopNode.Iterations <= 1)
            {
                calledLoopNode.Iterations = 1;
            }
            else
            {
                calledLoopNode.Iterations = callLoopNode.Iterations;
            }

            // Toggle Quantization on the Current Channel
            CurrentChannel.UpdateQuantization = true;
            if(InActiveLoop)
            {
                if (InActiveSubLoop == true)
                {
                    ActiveSubLoopInformation.UpdateQuantization = true;
                }
                else
                {
                    ActiveLoopInformation.UpdateQuantization = true;
                }
            }

            //EvaluateLoopNode(calledLoopData);
            var loopLocation = calledLoopInformation.LoopLocation;
            AddDataToChannel(MagicNumbers.CommandValues.Loop);
            //CurrentChannel.LoopLocations.Add(Convert.ToByte(CurrentChannel.ChannelData.Count));
            // Store the location of the loop invocation in the current channel
            CurrentChannel.LoopLocations.Add((ushort)CurrentChannel.ChannelData.Count);
            AddDataToChannel((byte)(loopLocation & MagicNumbers.HexCommandMaximum));
            AddDataToChannel((byte)(loopLocation >> 8));
            AddDataToChannel((byte)(calledLoopNode.Iterations));
        }

        public void EvaluateRemoteCodeDefinitionNode(LoopNode remoteCodeDefinitionNode)
        {
            // todo complete this function for evaluating remote code definitions
        }

        #endregion

        #region Special Directive Evaluators

        public void EvaluateSpecialDirective(DirectiveNode specialDirective)
        {
            switch (specialDirective.NodeType)
            {
                // Skip these since they have already been processed by this point
                case SongNodeType.Channel:
                case SongNodeType.SPC:
                case SongNodeType.Instruments:
                case SongNodeType.Samples:
                case SongNodeType.Path:
                    break;
                case SongNodeType.Amk:
                    EvaluateAmkNode(specialDirective);
                    break;
                case SongNodeType.Pad:
                    EvaluatePadNode(specialDirective);
                    break;
                case SongNodeType.Halvetempo:
                    EvaluateHalveTempoNode(specialDirective);
                    break;
                case SongNodeType.Option:
                    EvaluateOptionNode(specialDirective);
                    break;
                case SongNodeType.OptionGroup:
                    EvaluateOptionGroupNode(specialDirective);
                    break;
                default:
                    throw new AddmusicParserException("Invalid Special Directive Node Type");
            }
        }

        public void EvaluateAmkNode(DirectiveNode amkNode)
        {
            var amkPayload = amkNode.Payload as AmkVersionPayload ?? throw new AddmusicParserException("Null Payload found");

            if (amkPayload.AmkVersionType == AmkType.Am4)
            {
                AddmusicKVersion = AddmusicKVersion.AM4;
                SongData.AmkVersion = AddmusicKVersion.AM4;
                SongData.AmkParserVersion = AddmusicKParserVersion.Version1;
                return;
            }

            if (amkPayload.AmkVersionType == AmkType.Amm)
            {
                AddmusicKVersion = AddmusicKVersion.AMM;
                SongData.VelocityTable = VelocityTable.SmwVTable;
                SongData.AmkVersion = AddmusicKVersion.AMM;
                SongData.AmkParserVersion = AddmusicKParserVersion.Version2;
                return;
            }

            // Otherwise this is an Amk # song

            AddmusicKVersion = amkPayload.AmkVersion switch
            {
                "1" => AddmusicKVersion.Version1,
                "2" => AddmusicKVersion.Version2,
                "3" => throw new AddmusicParserException(_messageService.GetErrorAmkVersion3UnsupportedMessage()),
                "4" => AddmusicKVersion.Version4,
                _ => throw new AddmusicParserException(_messageService.GetErrorInvalidAmkVersionFoundMessage(amkPayload.AmkVersion)),
            };

            SongData.AmkVersion = AddmusicKVersion;
            SongData.AmkParserVersion = AddmusicKParserVersion.Version0;

            // Set the proper default velocity table
            if (AddmusicKVersion == AddmusicKVersion.Version2 || AddmusicKVersion == AddmusicKVersion.Version4)
            {
                SongData.VelocityTable = VelocityTable.NspcVTable;
            }
            else
            {
                SongData.VelocityTable = VelocityTable.SmwVTable;
            }
        }

        public void EvaluatePadNode(DirectiveNode padNode)
        {
            var padPayload = padNode.Payload as PadPayload ?? throw new AddmusicParserException("Null Payload found");

            var padAmount = Convert.ToInt32(padPayload.PadLength, 16);
            SongData.MinSize = padAmount;
        }

        public void EvaluateHalveTempoNode(DirectiveNode halvetempoNode)
        {
            TempoRatio = MultiplyByTempoRatio(halvetempoNode, 2);
        }

        public void EvaluateOptionGroupNode(DirectiveNode optionGroupNode)
        {
            foreach (var option in optionGroupNode.Children)
            {
                EvaluateNode(option);
            }
        }

        public void EvaluateOptionNode(DirectiveNode optionNode)
        {
            var optionPayload = optionNode.Payload as OptionPayload ?? throw new AddmusicParserException("Null Payload found");

            if (optionPayload.Option == OptionType.TempoImmunity)
            {
                EvaluateTempoImmunityNode(optionNode);
            }
            else if (optionPayload.Option == OptionType.Smwvtable)
            {
                EvaluateSMWVTableNode(optionNode);
            }
            else if (optionPayload.Option == OptionType.Nspcvtable)
            {
                EvaluateNSPCVTableNode(optionNode);
            }
            else if (optionPayload.Option == OptionType.Noloop)
            {
                EvaluateNoLoopNode(optionNode);
            }
            else if (optionPayload.Option == OptionType.Amk109hotpatch)
            {
                EvaluateAmk109HotPatchNode(optionNode);
            }
            else if (optionPayload.Option == OptionType.DivideTempo)
            {

            }
        }

        public void EvaluateTempoImmunityNode(DirectiveNode tempoImmunityNode)
        {
            AddDataToChannel(MagicNumbers.CommandValues.TempoImmunity);
            AddDataToChannel(MagicNumbers.CommandValues.SecondaryValues.TempoImmunitySecondary);
        }

        public void EvaluateSMWVTableNode(DirectiveNode smwvTableNode)
        {
            if(SongData.VelocityTable != VelocityTable.SmwVTable)
            {
                AddDataToChannel(MagicNumbers.CommandValues.FAOption);
                AddDataToChannel(MagicNumbers.CommandValues.FAValues.TableType);
                AddDataToChannel(MagicNumbers.CommandValues.FAValues.SmwVTable);
                SongData.VelocityTable = VelocityTable.SmwVTable;
            }
            else
            {
                _logger.LogWarning(LogLevel.Warning, _messageService.GetWarningSmwVelocityTableAlreadyUsedMessage(), true);
            }
        }

        public void EvaluateNSPCVTableNode(DirectiveNode nspcvTableNode)
        {
            AddDataToChannel(MagicNumbers.CommandValues.FAOption);
            AddDataToChannel(MagicNumbers.CommandValues.FAValues.TableType);
            AddDataToChannel(MagicNumbers.CommandValues.FAValues.NspcVTable);
            SongData.VelocityTable = VelocityTable.NspcVTable;

            _logger.LogWarning(LogLevel.Warning, _messageService.GetWarningNspcVelocityTableAlreadyUsedMessage(), true);
        }

        public void EvaluateNoLoopNode(DirectiveNode noLoopNode)
        {
            SongData.DoesntLoop = true;
        }

        public void EvaluateAmk109HotPatchNode(DirectiveNode amk109HotPatchNode)
        {
            AddDataToChannel(MagicNumbers.CommandValues.FAOption);
            AddDataToChannel(MagicNumbers.CommandValues.FAValues.Amk109HotPatch);
            AddDataToChannel(0x01);

            MarkEchoBufferAllocVCMD();
            
            //Prevent an off by one error (normally this is offset by one due to the last hex parameter byte being added after it), but for the #option itself, we shouldn't do this).
            SongData.EchoBufferAllocVCMDILocation--;
        }

        public void EvaluateDivideTempoNode(DirectiveNode divideTempoNode)
        {
            var divideTempoPayload = divideTempoNode.Payload as OptionPayload ?? throw new AddmusicParserException("Null Payload found");

            TempoRatio = (int)divideTempoPayload.OptionValue;

            // todo later do check to see if temporatio is less than one
        }


        #endregion

        #region Hex Command Node Evaluators

        public void EvaluateHexNode(HexNode hexNode)
        {
            switch (hexNode.CommandType)
            {
                case HexCommands.DDPitchBlend:
                    EvaluateDDPitchBlendNode(hexNode);
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

        public void EvaluateDDPitchBlendNode(HexNode pitchBlendNode)
        {
            var ddPitchBlendPayload = pitchBlendNode.Payload as DdPitchBlendPayload ?? throw new AddmusicParserException("Null Payload found");

            // Evaluate the starting items as they are not technically in the pitch blend
            foreach (var noteNode in ddPitchBlendPayload.StartNoteNodeItems)
            {
                EvaluateNode(noteNode);
            }
            // Add the pitch blend hex command
            AddDataToChannel(Convert.ToByte(pitchBlendNode.HexCommand.Replace("$", ""), 16));

            // Add the hex values used for the command
            foreach (var commandValue in ddPitchBlendPayload.HexValues)
            {
                AddDataToChannel(Convert.ToByte(commandValue.Replace("$", ""), 16));
            }

            // Process the blend items (if any)
            var inTriplet = false;
            foreach (var child in ddPitchBlendPayload.BlendItems)
            {
                switch (child.NodeType)
                {
                    case SongNodeType.Note:
                        EvaluateNoteNode((AtomicNode)child, inTriplet, default, true);
                        break;
                    case SongNodeType.Rest:
                        EvaluateRestNode((AtomicNode)child, inTriplet, default, true);
                        break;
                    case SongNodeType.Tie:
                        EvaluateTieNode((AtomicNode)child, inTriplet, default, true);
                        break;
                    default:
                        EvaluateNode(child);
                        break;
                }
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
                    Type = ResultType.Success
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
                _ => throw new AddmusicParserException("Invalid Atomic Node Type found")
            };
        }

        public IValidationResult ValidateDefaultLengthNode(AtomicNode defaultLength)
        {
            var defaultLengthPayload = defaultLength.Payload as DefaultLengthPayload ?? throw new AddmusicParserException("Null Payload found");

            if (defaultLengthPayload.Length < 1 || defaultLengthPayload.Length > MagicNumbers.NoteLengthMaximum)
            {
                return new ValidationResult
                {
                    Type = ResultType.Error,
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
                    Type = ResultType.Warning,
                    Message = new List<string>() {
                        _messageService.GetWarningDefaultLengthValidationMessage()
                    },
                };
            }
            else
            {
                return new ValidationResult
                {
                    Type = ResultType.Success,
                };
            }
        }

        public IValidationResult ValidateVolumeNode(AtomicNode volume)
        {
            var volumePayload = volume.Payload as VolumePayload ?? throw new AddmusicParserException("Null Payload found");

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
                    Type = ResultType.Error,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ResultType.Success,
                };
        }

        public IValidationResult ValidatePanNode(AtomicNode panNode)
        {
            var panPayload = panNode.Payload as PanPayload ?? throw new AddmusicParserException("Null Payload found");

            var messages = new List<string>();

            if (panPayload.HexSourced == true)
            {

                if(panPayload.PanPosition < 0 || panPayload.PanPosition > MagicNumbers.EightBitMaximum)
                {
                    messages.Add(_messageService.GetErrorPanFinalValueOutOfRangeMessage(panPayload.PanPosition.ToString(), 0.ToString(), MagicNumbers.EightBitMaximum.ToString()));
                }
                if(panPayload.PanDuration != -1 && (panPayload.PanDuration < 0 || panPayload.PanDuration > MagicNumbers.EightBitMaximum))
                {
                    messages.Add(_messageService.GetErrorPanDurationOutOfRangeMessage(panPayload.PanDuration.ToString(), 0.ToString(), MagicNumbers.EightBitMaximum.ToString()));
                }
            }
            else
            {
                if (panPayload.PanPosition < 0 || panPayload.PanPosition > MagicNumbers.PanDirectionMaximum)
                {
                    messages.Add(_messageService.GetErrorPanDirectionOutOfRangeMessage(0, MagicNumbers.PanDirectionMaximum, panPayload.PanPosition));
                }

                if (panPayload.HasSurroundSound == true)
                {
                    if (panPayload.SurroundSoundLeft < 0 || panPayload.SurroundSoundLeft > 1)
                    {
                        messages.Add(_messageService.GetErrorInvalidPanSurroundSoundValueMessage(panPayload.SurroundSoundLeft.ToString()));
                    }
                    if (panPayload.SurroundSoundRight < 0 || panPayload.SurroundSoundRight > 1)
                    {
                        messages.Add(_messageService.GetErrorInvalidPanSurroundSoundValueMessage(panPayload.SurroundSoundRight.ToString()));
                    }
                }
            }

            return messages.Count > 0
                ? new ValidationResult
                {
                    Type = ResultType.Error,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ResultType.Success
                };
        }

        public IValidationResult ValidateVibratoNode(AtomicNode vibrato)
        {
            var vibratoPayload = vibrato.Payload as VibratoPayload ?? throw new AddmusicParserException("Null Payload found");

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
                    Type = ResultType.Error,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ResultType.Success
                };
        }

        public IValidationResult ValidateTempoNode(AtomicNode tempo)
        {
            var tempoPayload = tempo.Payload as TempoPayload ?? throw new AddmusicParserException("Null Payload found");

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
                    Type = ResultType.Error,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ResultType.Success
                };
        }

        public IValidationResult ValidateNoiseNode(AtomicNode noise)
        {
            var noisePayload = noise.Payload as NoisePayload ?? throw new AddmusicParserException("Null Payload found");

            var noiseValue = noisePayload.NoiseValue;
            var noiseHexValue = Convert.ToByte(noiseValue);

            if (noiseHexValue < 0 || noiseHexValue > MagicNumbers.NoiseMaximum)
            {
                return new ValidationResult
                {
                    Type = ResultType.Error,
                    Message = new List<string>()
                    {
                        _messageService.GetErrorNoiseValueOutOfRangeMessage(0, MagicNumbers.NoiseMaximum, noiseHexValue),
                    }
                };
            }

            return new ValidationResult
            {
                Type = ResultType.Success,
            };
        }

        public IValidationResult ValidateQuantizationNode(AtomicNode quantization)
        {
            var quantizationPayload = quantization.Payload as QuantizationPayload ?? throw new AddmusicParserException("Null Payload found");

            if (quantizationPayload.VolumeNode != null)
            {
                var volumeNodeValidation = (ValidationResult)ValidateVolumeNode((AtomicNode)(quantizationPayload.VolumeNode));
                if (volumeNodeValidation.Type != ResultType.Success)
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
                Type = ResultType.Success,
            };
        }

        public IValidationResult ValidateInstrumentNode(AtomicNode instrument)
        {
            var instrumentPayload = instrument.Payload as InstrumentPayload ?? throw new AddmusicParserException("Null Payload found");

            var instrumentNumber = instrumentPayload.InstrumentNumber;

            if (instrumentNumber < 0 || instrumentNumber > MagicNumbers.EightBitMaximum)
            {
                return new ValidationResult
                {
                    Type = ResultType.Error,
                    Message = new List<string>
                    {
                        _messageService.GetErrorInstrumentValueOutOfRangeMessage(0, MagicNumbers.EightBitMaximum, instrumentNumber),
                    }
                };
            }

            return new ValidationResult
            {
                Type = ResultType.Success
            };
        }

        public IValidationResult ValidateQuestionMarkNode(AtomicNode questionMark)
        {
            var questionMarkPayload = questionMark.Payload as QuestionMarkPayload ?? throw new AddmusicParserException("Null Payload found");

            return questionMarkPayload.MarkNumber switch
            {
                0 or 1 or 2 => new ValidationResult
                {
                    Type = ResultType.Success,
                },
                _ => new ValidationResult
                {
                    Type = ResultType.Error,
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

                _ => throw new AddmusicParserException("Invalid Composite Node Type found")
            }; ;
        }

        public IValidationResult ValidateIntroNode(CompositeNode introNode)
        {
            // This should never happen due to ANTLR grammar enforcement but just in case
            if (InActiveLoop)
            {
                return new ValidationResult
                {
                    Type = ResultType.Error,
                    Message = new List<string>
                    {
                        _messageService.GetErrorIntroDirectiveFoundInLoopMessage(),
                    }
                };
            }

            return new ValidationResult
            {
                Type = ResultType.Success
            };
        }

        public IValidationResult ValidateTripletNode(CompositeNode tripletNode)
        {
            var messages = new List<string>();
            foreach (SongNode child in tripletNode.Children)
            {
                var validationResult = (ValidationResult)ValidateNode(child);
                if (validationResult.Type != ResultType.Success)
                {
                    messages.AddRange(validationResult.Message);
                }
            }
            return messages.Count > 0
                ? new ValidationResult
                {
                    Type = ResultType.Error,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ResultType.Success
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
                if (validationResult.Type != ResultType.Success)
                {
                    messages.AddRange(validationResult.Message);
                }
            }
            return messages.Count > 0
                ? new ValidationResult
                {
                    Type = ResultType.Error,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ResultType.Success
                };
        }

        public IValidationResult ValidateHexCommand(CompositeNode hexCommand)
        {
            var hexPayload = hexCommand.Payload as HexNumberPayload ?? throw new AddmusicParserException("Null Payload found");

            var hexByte = Convert.ToByte(hexPayload.HexValue, 16);

            if (hexByte >= MagicNumbers.HexCommandMaximum)
            {
                return new ValidationResult
                {
                    Type = ResultType.Error,
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
                    Type = ResultType.Error,
                    Message = new List<string>
                    {
                        _messageService.GetErrorHexCommandValueOutOfRangeMessage(hexPayload.HexValue, 0, MagicNumbers.HexCommandMaximum),
                    }
                };
            }

            return new ValidationResult
            {
                Type = ResultType.Success,
            };
        }

        public IValidationResult ValidateSampleLoadNode(CompositeNode sampleLoadNode)
        {
            var sampleloadPayload = sampleLoadNode.Payload as SampleLoadPayload ?? throw new AddmusicParserException("Null Payload found");

            // todo check for sample name existence and is loaded

            if (sampleloadPayload.SampleNumber == -1)
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
                    var lastDirectorySeparator = Helpers.Helpers.GetLastDirectorySeparatorIndex(standardizedPath);
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
                    Type = ResultType.Error,
                    Message = new List<string>
                    {
                        _messageService.GetErrorSampleLoadTuningValueOutOfRangeMessage(sampleloadPayload.TuningValue, 0, MagicNumbers.EightBitMaximum),
                    }
                };
            }

            return new ValidationResult
            {
                Type = ResultType.Success,
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
                    Type = ResultType.Success
                },
                // Requires Validation
                SongNodeType.SimpleLoop or
                SongNodeType.SuperLoop => ValidateSimpleOrSuperLoopNode(loop),
                SongNodeType.RemoteCode => ValidateRemoteCodeNode(loop),
                SongNodeType.CallLoop => ValidateCallLoopNode(loop),
                SongNodeType.CallPreviousLoop => ValidateCallPreviousLoopNode(loop),
                SongNodeType.CallRemoteCode => ValidateCallRemoteCodeNode(loop),
                _ => throw new AddmusicParserException("Invalid Loop Node Type found")
            };
        }

        public IValidationResult ValidateSimpleOrSuperLoopNode(LoopNode loop)
        {
            var messages = new List<string>();

            var warningMessages = new List<string>();
            var errorMessages = new List<string>();
            var failureMessages = new List<string>();

            var validIterations = CheckLoopIterations(loop, 1, MagicNumbers.EightBitMaximum);
            if (!validIterations)
            {
                messages.Add(_messageService.GetWarningLoopIterationOutOfRangeMessage());
            }

            foreach (var node in loop.Children)
            {
                var nodeValidation = (ValidationResult)ValidateNode(node);

                if(nodeValidation.Type == ResultType.Warning)
                {
                    warningMessages.AddRange(nodeValidation.Message);
                }
                else if (nodeValidation.Type == ResultType.Failure)
                {
                    failureMessages.AddRange(nodeValidation.Message);
                }
                else if (nodeValidation.Type == ResultType.Error)
                {
                    errorMessages.AddRange(nodeValidation.Message);
                }
            }

            messages.AddRange(warningMessages);
            messages.AddRange(failureMessages);
            messages.AddRange(errorMessages);

            var validationResult = new ValidationResult();
            if(failureMessages.Count > 0 || errorMessages.Count > 0)
            {
                validationResult.Type = ResultType.Error;
                validationResult.Message = messages;
            }
            else if(warningMessages.Count > 0)
            {
                validationResult.Type = ResultType.Warning;
                validationResult.Message = messages;
            }
            else
            {
                validationResult.Type = ResultType.Success;
            }

            return validationResult;
        }

        public IValidationResult ValidateRemoteCodeNode(LoopNode remoteCode)
        {
            var remoteCodePayload = remoteCode.Payload as RemoteCodeDefinitionPayload ?? throw new AddmusicParserException("Null Payload found");

            var messages = new List<string>();

            var warningMessages = new List<string>();
            var errorMessages = new List<string>();
            var failureMessages = new List<string>();

            foreach (var node in remoteCode.Children)
            {
                var nodeValidation = (ValidationResult)ValidateNode(node);

                if (nodeValidation.Type == ResultType.Warning)
                {
                    warningMessages.AddRange(nodeValidation.Message);
                }
                else if (nodeValidation.Type == ResultType.Failure)
                {
                    failureMessages.AddRange(nodeValidation.Message);
                }
                else if (nodeValidation.Type == ResultType.Error)
                {
                    errorMessages.AddRange(nodeValidation.Message);
                }
            }

            messages.AddRange(warningMessages);
            messages.AddRange(failureMessages);
            messages.AddRange(errorMessages);

            var validationResult = new ValidationResult();
            if (failureMessages.Count > 0 || errorMessages.Count > 0)
            {
                validationResult.Type = ResultType.Error;
                validationResult.Message = messages;
            }
            else if (warningMessages.Count > 0)
            {
                validationResult.Type = ResultType.Warning;
                validationResult.Message = messages;
            }
            else
            {
                validationResult.Type = ResultType.Success;
            }

            return validationResult;
        }

        public IValidationResult ValidateCallLoopNode(LoopNode callLoopNode)
        {
            var messages = new List<string>();
            if (!NamedLoopDefinitions.ContainsKey(callLoopNode.LoopName))
            {
                return new ValidationResult
                {
                    Type = ResultType.Error,
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
                    Type = ResultType.Warning,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ResultType.Success,
                };
        }

        public IValidationResult ValidateCallPreviousLoopNode(LoopNode callPreviousLoopNode)
        {
            var messages = new List<string>();

            if (PreviousLoop == null)
            {
                // todo double check what to do when this occurs
                return new ValidationResult
                {
                    Type = ResultType.Warning,
                    Message = new List<string>
                    {
                        _messageService.GetErrorRecallLoopUsedBeforeDefinitionMessage(),
                    },
                };
            }

            // This might be able to be considered an enhacement???
            if (PreviousLoop.NodeType == SongNodeType.SuperLoop)
            {
                return new ValidationResult
                {
                    Type = ResultType.Error,
                    Message = new List<string>
                    {
                        _messageService.GetErrorRecallLoopRecallsSuperLoopMessage(),
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
                    Type = ResultType.Warning,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ResultType.Success,
                };
        }

        public IValidationResult ValidateCallRemoteCodeNode(LoopNode callRemoteCodeNode)
        {
            var remoteCodeCallPayload = callRemoteCodeNode.Payload as CallRemoteCodePayload ?? throw new AddmusicParserException("Null Payload found");

            if (!RemoteCodeDefinitions.ContainsKey(remoteCodeCallPayload.DefinitionName))
            {
                return new ValidationResult
                {
                    Type = ResultType.Error,
                    Message = new List<string>
                    {
                        _messageService.GetErrorUndefinedNamedLoopCallMessage(),
                    },
                };
            }
            var messages = new List<string>();

            // todo more processing for the various remote code types

            return messages.Count > 0
                ? new ValidationResult
                {
                    Type = ResultType.Error,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ResultType.Success,
                };
        }

        #endregion

        #region Special Directive Validators

        public IValidationResult ValidateSpecialDirective(DirectiveNode specialDirective)
        {
            return specialDirective.NodeType switch
            {
                // Always Accepted
                SongNodeType.Pad or
                SongNodeType.Halvetempo or
                SongNodeType.Channel => new ValidationResult
                {
                    Type = ResultType.Success
                },
                SongNodeType.Option => ValidateOptionNode(specialDirective),
                SongNodeType.OptionGroup => ValidateOptionGroup(specialDirective),
                SongNodeType.Path => ValidateAndProcessPathNode(specialDirective),
                // Requires Validation
                SongNodeType.Amk => ValidateAMKNode(specialDirective),
                SongNodeType.SPC => ValidateAndProcessSpcDirectiveNode(specialDirective),
                SongNodeType.Instruments => ValidateAndProcessInstrumentDirectiveNode(specialDirective),
                SongNodeType.Samples => ValidateAndProcessSamplesDirectiveNode(specialDirective),
                _ => throw new AddmusicParserException("Invalid Special Directive found")
            };
        }

        public IValidationResult ValidateAMKNode(DirectiveNode amkNode)
        {
            var amkPayload = amkNode.Payload as AmkVersionPayload ?? throw new AddmusicParserException("Null Payload found");

            if(amkPayload.AmkVersionType == AmkType.Amk)
            {
                if(amkPayload.AmkVersion == "3")
                {
                    return new ValidationResult
                    {
                        Type = ResultType.Error,
                        Message = new List<string>()
                        {
                            _messageService.GetErrorAmkVersion3UnsupportedMessage(),
                        }
                    };
                }


                if(!MagicNumbers.StringValues.ValidAmkVersions.Exists(v => v.Equals(amkPayload.AmkVersion, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return new ValidationResult
                    {
                        Type = ResultType.Error,
                        Message = new List<string>()
                        {
                            _messageService.GetErrorInvalidAmkVersionFoundMessage(amkPayload.AmkVersion),
                        }
                    };
                }
            }

            return new ValidationResult
            {
                Type = ResultType.Success,
            };
        }

        public IValidationResult ValidateAndProcessPathNode(DirectiveNode pathNode)
        {
            var pathPayload = pathNode.Payload as PathPayload ?? throw new AddmusicParserException("Null Payload found");

            // no need to process further if there's no value
            if (pathPayload.PathText.Length == 0)
            {
                _logger.LogInformation(LogLevel.Trace, "Empty Path found. Its still valid, just empty.");
                return new ValidationResult
                {
                    Type = ResultType.Skip,
                };
            }

            // parse potential path characters and recombine with the current systems correct directory delimmitters
            var pathValue = pathPayload.PathText;
            var correctedPath = Helpers.Helpers.StandardizeFileDirectoryDelimiters(pathValue);
            SongData.SongPath = correctedPath;

            return new ValidationResult
            {
                Type = ResultType.Success,
            };
        }

        public IValidationResult ValidateAndProcessSpcDirectiveNode(DirectiveNode spcNode)
        {
            var spcPayload = spcNode.Payload as SpcPayload ?? throw new AddmusicParserException("Null Payload found");

            var title = spcPayload.Title;
            var author = spcPayload.Author;
            var game = spcPayload.Game;
            var comment = spcPayload.Comment;
            var length = spcPayload.Length;

            if (game.Length == 0)
            {
                SongData.Game = MagicNumbers.StringValues.DefaultSpcTitleName;
            }

            if (length == "auto")
            {
                SongData.GuessLength = true;
            }
            else if (length.Length > 0)
            {
                if (!length.Contains(':'))
                {
                    return new ValidationResult
                    {
                        Type = ResultType.Error,
                        Message = new List<string>()
                        {
                            _messageService.GetErrorSpcLengthInvalidValueMessage(),
                        }
                    };
                }
                else
                {
                    SongData.GuessLength = false;

                    var lengthTime = length.Split(":").ToList();
                    // The " should be parsed out by now, but just in case they aren't
                    var mins = int.Parse(lengthTime[0].Replace("\"", ""));
                    var secs = int.Parse(lengthTime[1].Replace("\"", ""));
                    var totalSeconds = mins * 60 + secs;

                    if (totalSeconds > 999)
                    {
                        return new ValidationResult
                        {
                            Type = ResultType.Error,
                            Message = new List<string>()
                            {
                                _messageService.GetErrorSpcLengthValueTooLongMessage(),
                            }
                        };
                    }

                    SongData.Seconds = (int)(totalSeconds & MagicNumbers.ThirtytwoBitMaximum);

                    SongData.KnowsLength = true;
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
                    ? ResultType.Warning
                    : ResultType.Success,
                Message = messages,
            };
        }

        public IValidationResult ValidateAndProcessInstrumentDirectiveNode(DirectiveNode instrumentsNode)
        {
            var instrumentPayload = instrumentsNode.Payload as InstrumentsPayload ?? throw new AddmusicParserException("Null Payload found");

            var messages = new List<string>();
            var customInstrumentCount = MagicNumbers.StartingCustomInstrumentNumber;
            foreach (var instrument in instrumentPayload.Instruments)
            {
                var instrumentInformation = new InstrumentInformation()
                {
                    InstrumentNumber = customInstrumentCount
                };

                if (instrument.Type == InstrumentDefinition.InstrumentType.Noise)
                {
                    var noiseValidation = (ValidationResult)ValidateNoiseNode((AtomicNode)instrument.NoiseData);

                    if(noiseValidation.Type == ResultType.Failure 
                        || noiseValidation.Type == ResultType.Error
                        || noiseValidation.Type == ResultType.Warning
                    )
                    {
                        // if there's an error, report that error
                        messages.AddRange(noiseValidation.Message);
                        continue;
                    }

                    var noiseValue = Convert.ToByte(((NoisePayload)((AtomicNode)instrument.NoiseData).Payload).NoiseValue);
                    var finalInstrumentValue = noiseValue | 0x80;

                    instrumentInformation.InstrumentData = finalInstrumentValue;
                }
                else if (instrument.Type == InstrumentDefinition.InstrumentType.Number)
                {
                    var instrumentValidation = (ValidationResult)ValidateInstrumentNode((AtomicNode)instrument.InstrumentNumber);

                    if (instrumentValidation.Type == ResultType.Failure
                        || instrumentValidation.Type == ResultType.Error
                        || instrumentValidation.Type == ResultType.Warning
                    )
                    {
                        // if there's an error, report that error
                        messages.AddRange(instrumentValidation.Message);
                        continue;
                    }

                    var instrumentNumber = ((InstrumentPayload)((AtomicNode)instrument.InstrumentNumber).Payload).InstrumentNumber;
                    
                    if(instrumentNumber >= MagicNumbers.StartingCustomInstrumentNumber)
                    {
                        _logger.LogError(LogLevel.Trace, _messageService.GetErrorInvalidCustomInstrumentBaseMessage($"@{instrumentNumber}"));
                        messages.Add(_messageService.GetErrorInvalidCustomInstrumentBaseMessage($"@{instrumentNumber}"));
                        continue;
                    }

                    instrumentInformation.InstrumentData = MagicNumbers.InstrumentsToSample[instrumentNumber];

                }
                else if (instrument.Type == InstrumentDefinition.InstrumentType.Sample)
                {
                    var sampleName = instrument.SampleName ?? "";
                    if(sampleName == null || sampleName.Length == 0)
                    {
                        messages.Add(_messageService.GetErrorMissingSampleNameTextMessage());
                        continue;
                    }
                    if(!SampleInstrumentManager.ContainsSampleName(sampleName))
                    {
                        messages.Add(_messageService.GetErrorSampleNameNotPreviouslyDefinedMessage(sampleName));
                        continue;
                    }

                    // this doesn't need to be validated due to the previous check
                    SampleInstrumentManager.UseSampleName(sampleName);

                    instrumentInformation.InstrumentSample = sampleName;

                }

                if (instrument.HexSettings.Count != MagicNumbers.ExpectedInstrumentHexByteCount)
                {
                    messages.Add(_messageService.GetErrorInstrumentDefinitionMissingHexValuesMessage());
                    continue;
                }

                var intHexes = new List<int>();
                foreach (var setting in instrument.HexSettings)
                {
                    var hexValue = Convert.ToByte(setting.Replace("$", ""), 16);

                    if (!Helpers.Helpers.IsHexInRange(hexValue))
                    {
                        messages.Add(_messageService.GetErrorInstrumentDefinitionHexValueOutOfRangeMessage(0, MagicNumbers.ByteHexMaximum, hexValue));
                        continue;
                    }
                    intHexes.Add(hexValue);
                }

                instrumentInformation.HexComponents.AddRange(intHexes);

                SampleInstrumentManager.AddInstrument(instrumentInformation);
                customInstrumentCount++;
            }

            return new ValidationResult
            {
                Type = messages.Count > 0
                    ? ResultType.Error
                    : ResultType.Success,
                Message = messages,
            };
        }

        public IValidationResult ValidateAndProcessSamplesDirectiveNode(DirectiveNode samplesNode)
        {
            var samplesPayload = samplesNode.Payload as SamplesPayload ?? throw new AddmusicParserException("Null Payload found");

            // add the default group is none are present

            if (samplesPayload.SampleGroupPaths.Count == 0)
            {
                samplesPayload.SampleGroupPaths.Add("#default");
            }

            var warnings = new List<string>();
            var errors = new List<string>();

            foreach(var sampleGroup in samplesPayload.SampleGroupPaths)
            {
                var group = _globalSettings.ResourceList.SampleGroups.FindAll(g => g.Name.Equals(sampleGroup, StringComparison.InvariantCultureIgnoreCase)).ToList();
                if(group.Count == 0)
                {
                    // Sample Group Name is required when using samples
                    return new ValidationResult
                    {
                        Type = ResultType.Error,
                        Message = new List<string>
                        {
                            _messageService.GetErrorMissingSampleNameTextMessage(),
                        }
                    };
                }
                if(group.Count > 1)
                {
                    // cannot (and will not) attempt to resolve duplicates for sample group names
                    return new ValidationResult
                    {
                        Type = ResultType.Error,
                        Message = new List<string>
                        {
                            _messageService.GetErrorMultipleSameSampleGroupNamesMessage(sampleGroup),
                        }
                    };
                }

                //SampleInstrumentManager.Samples.AddRange(group.First().Samples);

                foreach(var sample in group.First().Samples)
                {
                    var standardizedPath = Helpers.Helpers.StandardizeFileDirectoryDelimiters(sample.Path);
                    var lastDirectorySeparator = Helpers.Helpers.GetLastDirectorySeparatorIndex(standardizedPath);
                    var sampleName = standardizedPath[(lastDirectorySeparator != 0 ? lastDirectorySeparator + 1 : 0 )..];

                    if (SampleInstrumentManager.ContainsSampleName(sampleName))
                    {
                        // todo notify duplicate sample name
                        continue;
                    }
                    else
                    {
                        SampleInstrumentManager.AddNewSampleName(sampleName);
                    }

                    SampleInstrumentManager.AddNewSample(FileConverters.ConvertAddmusicSampleToSample(_messageService, sample));
                    //Helpers.Helpers.LoadSampleToCache(_logger, _fileCachingService, sample);
                }
            }

            foreach (var sample in samplesPayload.Samples)
            {
                // get file extension
                if (sample.LastIndexOf(".") == -1)
                {
                    errors.Add(_messageService.GetErrorSampleNameMissingFileExtensionMessage(sample));
                    continue;
                }

                var fileExtensionStartPosition = sample.LastIndexOf(".");
                var fileExtension = sample[fileExtensionStartPosition..];
                
                if(!FileNames.FileExtensions.ValidSampleExtensions.Contains(fileExtension))
                {
                    errors.Add(_messageService.GetErrorSampleNameHasInvalidFileExtensionMessage(sample, fileExtension, string.Join(", ", FileNames.FileExtensions.ValidSampleExtensions)));
                    continue;
                }

                if(fileExtension == FileNames.FileExtensions.SampleBank)
                {
                    // todo handle deprecated filetype
                    continue;
                }

                if(fileExtension == FileNames.FileExtensions.SampleBrr)
                {
                    var standardizedPath = Helpers.Helpers.StandardizeFileDirectoryDelimiters(sample);
                    var lastDirectorySeparator = Helpers.Helpers.GetLastDirectorySeparatorIndex(standardizedPath);
                    var sampleName = standardizedPath[lastDirectorySeparator..];
                    var samplePath = Path.Combine(SongData.SongPath, sampleName);

                    if(SampleInstrumentManager.ContainsSampleName(sampleName))
                    {
                        warnings.Add(_messageService.GetWarningDuplicateSampleNameMessage(sampleName));
                        continue;
                    }

                    SampleInstrumentManager.AddNewSampleName(sampleName);
                    var sampleData = new AddmusicSample
                    {
                        Name = sampleName,
                        Path = samplePath,
                        IsImportant = false,
                        IsLooping = false,
                    };
                    SampleInstrumentManager.AddNewSample(FileConverters.ConvertAddmusicSampleToSample(_messageService, sampleData));
                    //Helpers.Helpers.LoadSampleToCache(_logger, _fileCachingService, sampleData);
                }
            }

            return (errors.Count > 0)
                ? new ValidationResult
                {
                    Type = ResultType.Error,
                    Message = errors
                }
                : (warnings.Count  > 0)
                    ? new ValidationResult
                    {
                        Type = ResultType.Warning,
                        Message = warnings
                    }
                    : new ValidationResult
                    {
                        Type = ResultType.Success,
                    };
        }

        public IValidationResult ValidateOptionGroup(DirectiveNode optionGroupNode)
        {
            var errors = new List<ValidationResult>();
            var warnings = new List<ValidationResult>();

            foreach(var option in optionGroupNode.Children)
            {
                var validation = (ValidationResult)ValidateNode(option);

                if(validation.Type == ResultType.Error || validation.Type == ResultType.Failure)
                {
                    errors.Add(validation);
                }
                else if (validation.Type == ResultType.Warning)
                {
                    warnings.Add(validation);
                }
            }

            if(errors.Count > 0 && warnings.Count > 0)
            {
                return new ValidationResult
                {
                    Type = ResultType.Error,
                    Message = [.. errors.Concat(warnings).Select(v => v.Message).SelectMany(v => v)],
                };
            }
            else if(errors.Count > 0)
            {
                return new ValidationResult
                {
                    Type = ResultType.Error,
                    Message = [.. errors.SelectMany(v => v.Message)],
                };
            }
            else if (warnings.Count > 0)
            {
                return new ValidationResult
                {
                    Type = ResultType.Warning,
                    Message = [.. warnings.SelectMany(v => v.Message)],
                };
            }

            return new ValidationResult
            {
                Type = ResultType.Success,
            };
        }

        public IValidationResult ValidateOptionNode(DirectiveNode optionNode)
        {
            var optionPayload = optionNode.Payload as OptionPayload ?? throw new AddmusicParserException("Null Payload found");

            return optionPayload.Option switch
            {
                // Always accept because there's nothing to check
                OptionType.Nspcvtable or
                OptionType.Smwvtable or
                OptionType.Noloop or
                OptionType.Amk109hotpatch or
                OptionType.TempoImmunity => new ValidationResult
                {
                    Type = ResultType.Success,
                },
                OptionType.DivideTempo => ValidateDivideTempoNode(optionNode),
                _ => throw new AddmusicParserException($"Unknown OptionType {optionPayload.Option.ToString()}")
            };
        }

        public IValidationResult ValidateDivideTempoNode(DirectiveNode divideTempoNode)
        {
            var divideTempoPayload = divideTempoNode.Payload as OptionPayload ?? throw new AddmusicParserException("Null Payload found");

            if(divideTempoPayload.Option != OptionType.DivideTempo)
            {
                throw new AddmusicParserException($"Option Type is not {OptionType.DivideTempo.ToString()}");
            }

            if(divideTempoPayload.OptionValue.GetType() != typeof(int))
            {
                throw new AddmusicParserException("OptionValue is not an int");
            }

            var divideTempoValue = (int)divideTempoPayload.OptionValue;

            if(divideTempoValue < 0)
            {
                return new ValidationResult
                {
                    Type = ResultType.Error,
                    Message = new List<string>
                    {
                        _messageService.GetErrorInvalidDivideTempoValueMessage()
                    }
                };
            }

            if(divideTempoValue == 1)
            {
                return new ValidationResult
                {
                    Type = ResultType.Warning,
                    Message = new List<string>
                    {
                        _messageService.GetWarningDivideTempoSetTo1Message()
                    }
                };
            }

            return new ValidationResult
            {
                Type = ResultType.Success
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
                var byteValue = Convert.ToByte(hexNumber.Replace("$", ""), 16);
                if (!Helpers.Helpers.IsHexInRange(byteValue))
                {
                    messages.Add(_messageService.GetErrorHexCommandSuppliedValueOutOfRangeMessage(hexNumber, hex.HexCommand, 0, MagicNumbers.HexCommandMaximum));
                }
            }

            return messages.Count > 0
                ? new ValidationResult
                {
                    Type = ResultType.Error,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ResultType.Success,
                };
        }

        public IValidationResult ValidateDDPitchBlend(HexNode pitchBlend)
        {
            var messages = new List<string>();

            var ddPitchBlendPayload = pitchBlend.Payload as DdPitchBlendPayload ?? throw new AddmusicParserException("Null Payload found");

            // Check to see if the starting notes are valid
            foreach (SongNode noteNode in ddPitchBlendPayload.StartNoteNodeItems)
            {
                var validationResult = (ValidationResult)ValidateNode(noteNode);
                if (validationResult.Type != ResultType.Success)
                {
                    messages.AddRange(validationResult.Message);
                }
            }

            // Validate the hex commands used for the command
            foreach (var hexNumber in ddPitchBlendPayload.HexValues)
            {
                if (!Helpers.Helpers.IsHexInRange(Convert.ToByte(hexNumber.Replace("$", ""), 16)))
                {
                    messages.Add(_messageService.GetErrorHexCommandSuppliedValueOutOfRangeMessage(hexNumber, pitchBlend.HexCommand, 0, MagicNumbers.ByteHexMaximum));
                }
            }

            // Validate the blend items that are used after the command (if any)
            //      Disallow any notes if there has been a quantization used immediately before this pitch blend
            foreach (var node in ddPitchBlendPayload.BlendItems)
            {
                var validation = (ValidationResult)ValidateNode(node);
                if (validation.Type == ResultType.Error ||
                    validation.Type == ResultType.Warning)
                {
                    messages.AddRange(validation.Message);
                }
                // check to see if the quantization has been set and fail if the child node is a note
                var hasCurrentQuantization = (InActiveLoop == true)
                    ? ActiveLoopInformation.UpdateQuantization
                    : (InActiveSubLoop == true)
                        ? ActiveSubLoopInformation.UpdateQuantization
                        : CurrentChannel.UpdateQuantization;
                var lastPreItem = ddPitchBlendPayload.StartNoteNodeItems.Last();
                if(lastPreItem.NodeType == SongNodeType.Quantization 
                    && node.NodeType == SongNodeType.Note 
                    && hasCurrentQuantization == true)
                {
                    validation.Type = ResultType.Error;
                    messages.Add(_messageService.GetErrorDDPitchBlendEndNoteHasQuantizationMessage());
                }
            }

            return messages.Count > 0
                ? new ValidationResult
                {
                    Type = ResultType.Error,
                    Message = messages,
                }
                : new ValidationResult
                {
                    Type = ResultType.Success,
                };
        }

        #endregion

        #endregion

        #region Helpers

        private void AddDataToChannel(byte dataToAdd)
        {
            if(InActiveLoop)
            {
                if (InActiveSuperLoop)
                {
                    if (InActiveSubLoop)
                    {
                        CurrentSubLoopData.Add(dataToAdd);
                    }
                    else
                    {
                        var currentChannel = Channels.Where(c => c.ChannelNumber == CurrentChannel.ChannelNumber).First();
                        currentChannel.ChannelData.Add(dataToAdd);
                    }
                }
                else
                {
                    CurrentLoopData.Add(dataToAdd);
                }
            }
            else if (Channels.Count == 0)
            {
                PreChannelData.Add(dataToAdd);
            }
            else
            {
                var currentChannel = Channels.Where(c => c.ChannelNumber == CurrentChannel.ChannelNumber).First();
                currentChannel.ChannelData.Add(dataToAdd);
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
            else if (Channels.Count == 0)
            {
                PreChannelData.AddRange(dataToAdd);
            }
            else
            {
                var currentChannel = Channels.Where(c => c.ChannelNumber == CurrentChannel.ChannelNumber).First();
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
            if (AddmusicKVersion < AddmusicKVersion.Version4 || TempoRatio == MagicNumbers.DefaultValues.InitialTempoRatio)
            {
                return value;
            }

            if (value % TempoRatio != 0)
            {
                if (isFractionalError)
                {
                    _logger.LogWarning(LogLevel.Error, _messageService.GetErrorFractionalTempoRatioMessage(SongData.Name, node.LineNumber, node.ColumnNumber), true);
                    throw new AddmusicParserException(_messageService.GetErrorFractionalTempoRatioMessage(SongData.Name, node.LineNumber, node.ColumnNumber));
                }
                else
                {
                    _logger.LogWarning(LogLevel.Warning, _messageService.GetWarningFactionalTempoRatioValueMessage(SongData.Name, node.LineNumber, node.ColumnNumber));
                }
            }

            return value / TempoRatio;
        }

        private int MultiplyByTempoRatio(SongNode node, int value)
        {
            var result = value * TempoRatio;
            if (TempoRatio >= MagicNumbers.EightBitMaximum)
            {
                _logger.LogError(LogLevel.Error, _messageService.GetErrorTempoRatioValueOverflowMessage(SongData.Name, node.LineNumber, node.ColumnNumber), true);
                throw new AddmusicParserException(_messageService.GetErrorTempoRatioValueOverflowMessage(SongData.Name, node.LineNumber, node.ColumnNumber));
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

        private int GetNoteLength(SongNode node, int noteLength, int dotCount, bool inTriplet, bool allowTriplet, bool hasEquals, bool useDefaultNoteLength)
        {

            var length = noteLength;
            if (hasEquals == true && AddmusicKVersion < AddmusicKVersion.Version4)
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
                    _logger.LogWarning(LogLevel.Warning, _messageService.GetWarningNoteLengthFractionalTickValueMessage(MagicNumbers.NoteLengthMaximum, SongData.Name, node.LineNumber, node.ColumnNumber));
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
                        _logger.LogWarning(LogLevel.Warning, _messageService.GetWarningFractionalTickValueFromDotsMessage(i + 1, SongData.Name, node.LineNumber, node.ColumnNumber));
                    }
                    else
                    {
                        _logger.LogWarning(LogLevel.Warning, _messageService.GetWarningFractionalTickValueFromDotsMessage(1, SongData.Name, node.LineNumber, node.ColumnNumber));
                    }
                }

                fraction = fraction / 2;
                result += fraction;
            }

            if (inTriplet && allowTriplet == true)
            {
                if (fraction % 3 != 0)
                {
                    _logger.LogWarning(LogLevel.Warning, _messageService.GetWarningTripletFractionalTickValueFromDotsMessage(SongData.Name, node.LineNumber, node.ColumnNumber));
                }
                result = (int)Math.Floor(((double)result * 2.0 / 3.0) + 0.5);
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
                        if (noteLength != PreviousNoteLength || ActiveSubLoopInformation.UpdateQuantization)
                        {
                            AddDataToChannel(Convert.ToByte(noteLength));
                        }

                        if (ActiveSubLoopInformation.UpdateQuantization)
                        {   
                            AddDataToChannel(ActiveSubLoopInformation.CurrentQuantization);
                            ActiveSubLoopInformation.UpdateQuantization = false;
                            SongData.NoteParameterByteCount++;
                        }
                    }
                    else
                    {
                        if (noteLength != PreviousNoteLength || ActiveLoopInformation.UpdateQuantization)
                        {
                            AddDataToChannel(Convert.ToByte(noteLength));
                        }

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
                    if (noteLength != PreviousNoteLength || CurrentChannel.UpdateQuantization)
                    {
                        AddDataToChannel(Convert.ToByte(noteLength));
                    }

                    if (CurrentChannel.UpdateQuantization)
                    {
                        AddDataToChannel(CurrentChannel.CurrentQuantization);
                        CurrentChannel.UpdateQuantization = false;
                        SongData.NoteParameterByteCount++;
                    }
                }

                PreviousNoteLength = noteLength;
                AddDataToChannel(noteType);
            }
        }

        private int GetPitchValue(int value, NotePayload.Accidentals accidental)
        {
            var pitchValue = MagicNumbers.ValidPitches[value - MagicNumbers.PitchOffset] + (CurrentOctave - 1) * 12 + 0x80;

            return accidental switch
            {
                NotePayload.Accidentals.Sharp => ++pitchValue,
                NotePayload.Accidentals.Flat => --pitchValue,
                NotePayload.Accidentals.None => pitchValue,
                _ => pitchValue
            };
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
            // Mark the location to generate the echo buffer allocation VCMD.
            // This won't be performed if this is located past a note or a loop marker, since being
            // past a note means a note will key on prior to the song initializing properly and being
            // past a loop marker will mean the VCMD will retrigger, which may or may not end well.
            if (SongData.EchoBufferAllocVCMDIsSet
                || InActiveSuperLoop
                || InActiveLoop
                || SongData.HasEchoBufferCommand
                || CurrentChannel.HasIntro
                || CurrentChannel.HasNoteData)
            {
                // todo handle warning for this case
                _logger.LogWarning(LogLevel.Warning, _messageService.GetWarningMarkEchoBufferAllocVCMDMessage(), true);
                return;
            }

            SongData.EchoBufferAllocVCMDIsSet = true;
            SongData.EchoBufferAllocVCMDILocation = (ushort)(CurrentChannel.ChannelData.Count + 1);
            SongData.EchoBufferAllocVCMDIChannel = CurrentChannel.ChannelNumber;
        }

        #endregion


    }
}
