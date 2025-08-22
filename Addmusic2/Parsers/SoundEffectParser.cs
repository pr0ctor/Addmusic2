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
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Addmusic2.Parsers
{
    internal class SoundEffectParser : ISoundEffectParser
    {
        private readonly ILogger<IAddmusicLogic> _logger;
        private readonly MessageService _messageService;
        //private readonly SongListItem _songListItem;
        private readonly GlobalSettings _globalSettings;
        private readonly FileCachingService _fileCachingService;
        //private readonly SongScope _songScope;
        public SoundEffectData SoundEffectData { get; set; } = new();


        public SoundEffectParser(
            ILogger<IAddmusicLogic> logger,
            MessageService messageService,
            GlobalSettings globalSettings,
            FileCachingService fileCachingService
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
            throw new NotImplementedException();
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

        #endregion

        #region Composite Node Evaluators

        #endregion

        #region Special Directive Evaluators

        #endregion

        #region Hex Command Evaluators

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
                SongNodeType.SfxInstrument => ValidateInstrumentNode(atomic),
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

        #endregion

        #region Special Directive Validators

        #endregion

        #region Hex Command Validators

        #endregion


        #endregion


        #region Helpers


        #endregion

    }
}
