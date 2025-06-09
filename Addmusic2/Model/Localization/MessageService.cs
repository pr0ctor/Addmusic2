using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Localization
{
    public sealed class MessageService(IStringLocalizer<MessageService> _localizer)
    {

        #region Intro Messages

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetIntroAddmusicVersionMessage()
        {
            LocalizedString localizedString = _localizer["IntroAddmusicVersion"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetIntroParserVersionMessage()
        {
            LocalizedString localizedString = _localizer["IntroParserVersion"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetIntroReadTheReadMeMessage()
        {
            LocalizedString localizedString = _localizer["IntroReadTheReadMe"];

            return localizedString;
        }

        #endregion

        #region Warning Messages

        #region Validation Warning Messages

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetWarningDefaultLengthValidationMessage()
        {
            LocalizedString localizedString = _localizer["WarningDefaultLengthValidation"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetWarningSpcTextValueTooLongMessage(string elementName, string truncatedValue)
        {
            LocalizedString localizedString = _localizer["WarningSpcTextValueTooLong", elementName, truncatedValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetWarningFactionalTempoRatioValueMessage(string songName, int lineNumber, int columnValue)
        {
            LocalizedString localizedString = _localizer["WarningFactionalTempoRatioValue", songName, lineNumber, columnValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetWarningOctaveRaisedTooHighMessage()
        {
            LocalizedString localizedString = _localizer["WarningOctaveRaisedTooHigh"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetWarningOctaveDroppedTooLowMessage()
        {
            LocalizedString localizedString = _localizer["WarningOctaveDroppedTooLow"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetWarningLoopIterationOutOfRangeMessage()
        {
            LocalizedString localizedString = _localizer["WarningLoopIterationOutOfRange"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetWarningFractionalTickValueFromDotsMessage(int dotCount, string songName, int lineNumber, int columnValue)
        {
            LocalizedString localizedString = _localizer["WarningFractionalTickValueFromDots", dotCount, songName, lineNumber, columnValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetWarningTripletFractionalTickValueFromDotsMessage(string songName, int lineNumber, int columnValue)
        {
            LocalizedString localizedString = _localizer["WarningTripletFractionalTickValueFromDots", songName, lineNumber, columnValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetWarningNoteLengthFractionalTickValueMessage(int divisor, string songName, int lineNumber, int columnValue)
        {
            LocalizedString localizedString = _localizer["WarningNoteLengthFractionalTickValue", divisor, songName, lineNumber, columnValue];

            return localizedString;
        }


        #endregion

        #endregion

        #region Error Messages

        #region Validation Error Messages

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetDefaultLengthOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorDefaultLengthOutOfRange", foundValue, minValue, maxValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorVolumeVolumeValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorVolumeVolumeValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorVolumeFadeValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorVolumeFadeValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorGlobalVolumeVolumeValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorGlobalVolumeVolumeValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorGlobalVolumeFadeValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorGlobalVolumeFadeValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorPanDirectionOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorPanDirectionOutOfRange", foundValue, minValue, maxValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorVibratoDelayOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorVibratoDelayOutOfRange", foundValue, minValue, maxValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorVibratoRateOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorVibratoRateOutOfRange", foundValue, minValue, maxValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorVibratoExtentOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorVibratoExtentOutOfRange", foundValue, minValue, maxValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorTempoTempoValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorTempoTempoValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorTempoFadeValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorTempoFadeValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorNoiseValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorNoiseValueOutOfRange", foundValue, minValue.ToString("X2"), maxValue.ToString("X")];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorQuantizationVolumeValueOutOfRangeMessage()
        {
            LocalizedString localizedString = _localizer["ErrorQuantizationVolumeValueOutOfRange"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorSpcLengthInvalidValueMessage()
        {
            LocalizedString localizedString = _localizer["ErrorSpcLengthInvalidValue"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorSpcLengthValueTooLongMessage()
        {
            LocalizedString localizedString = _localizer["ErrorSpcLengthValueTooLong"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorInstrumentDefinitionMissingHexValuesMessage()
        {
            LocalizedString localizedString = _localizer["ErrorInstrumentDefinitionMissingHexValues"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorInstrumentDefinitionHexValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorInstrumentDefinitionHexValueOutOfRange", foundValue.ToString("X"), minValue.ToString("X2"), maxValue.ToString("X")];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorFractionalTempoRatioMessage(string songName, int lineNumber, int columnValue)
        {
            LocalizedString localizedString = _localizer["ErrorFractionalTempoRatio", songName, lineNumber, columnValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorAlreadyFractionalTempoRatioMessage(string songName, int lineNumber, int columnValue)
        {
            LocalizedString localizedString = _localizer["ErrorAlreadyFractionalTempoRatio", songName, lineNumber, columnValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorTempoRatioValueOverflowMessage(string songName, int lineNumber, int columnValue)
        {
            LocalizedString localizedString = _localizer["ErrorTempoRatioValueOverflow", songName, lineNumber, columnValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorQuestionMarkValueOutOfRangeMessage()
        {
            LocalizedString localizedString = _localizer["ErrorQuestionMarkValueOutOfRange"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorUndefinedNamedLoopCallMessage()
        {
            LocalizedString localizedString = _localizer["ErrorUndefinedNamedLoopCall"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorUndefinedRemoteCodeCallMessage()
        {
            LocalizedString localizedString = _localizer["ErrorUndefinedRemoteCodeCall"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorHexCommandValueOutOfRangeMessage(string hexValue, int minValue, int maxValue)
        {
            LocalizedString localizedString = _localizer["ErrorHexCommandValueOutOfRange", hexValue, minValue, maxValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorUnknownHexCommandMessage(string hexValue)
        {
            LocalizedString localizedString = _localizer["ErrorUnknownHexCommand", hexValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorSampleLoadTuningValueOutOfRangeMessage(string hexValue, int minValue, int maxValue)
        {
            LocalizedString localizedString = _localizer["ErrorSampleLoadTuningValueOutOfRange", hexValue, minValue, maxValue];

            return localizedString;
        }


        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorDuplicateLoopNameDefinedMessage(string loopName)
        {
            LocalizedString localizedString = _localizer["ErrorDuplicateLoopNameDefined", loopName];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorDuplicateRemoteCodeDefinitionNameDefinedMessage(string remoteCodeDefinitionName)
        {
            LocalizedString localizedString = _localizer["ErrorDuplicateRemoteCodeDefinitionNameDefined", remoteCodeDefinitionName];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorMaximumAllowedNumberOfLoopsReachedMessage()
        {
            LocalizedString localizedString = _localizer["ErrorMaximumAllowedNumberOfLoopsReached"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorHexCommandSuppliedValueOutOfRangeMessage(string hexValue, string hexCommand, int minValue, int maxValue)
        {
            LocalizedString localizedString = _localizer["ErrorHexCommandSuppliedValueOutOfRange", hexValue, hexCommand, minValue, maxValue];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorIntroDirectiveFoundInLoopMessage()
        {
            LocalizedString localizedString = _localizer["ErrorIntroDirectiveFoundInLoop"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetErrorInstrumentValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorInstrumentValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString;
        }

        #endregion

        #endregion
    }
}
