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

        #region CLArgs Messages, Names, and Descriptions
        
        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgRomNameNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgRomNameName"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgRomNameDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgRomNameDescription"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgConvertOldAddmusicNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgConvertOldAddmusicName"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgConvertOldAddmusicDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgConvertOldAddmusicDescription"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgCheckEchoNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgCheckEchoName"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgCheckEchoDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgCheckEchoDescription"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgBankStartNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgBankStartName"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgBankStartDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgBankStartDescription"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgVerboseLoggingNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgVerboseLoggingName"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgVerboseLoggingDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgVerboseLoggingDescription"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgAggressiveFreeSpaceNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgAggressiveFreeSpaceName"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgAggressiveFreeSpaceDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgAggressiveFreeSpaceDescription"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgDuplicateCheckingNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgDuplicateCheckingName"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgDuplicateCheckingDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgDuplicateCheckingDescription"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgHexValidationNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgHexValidationName"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgHexValidationDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgHexValidationDescription"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgCreatePatchNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgCreatePatchName"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgCreatePatchDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgCreatePatchDescription"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgOptimizeSampleUsageNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgOptimizeSampleUsageName"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgOptimizeSampleUsageDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgOptimizeSampleUsageDescription"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgAllowSA1NameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgAllowSA1Name"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgAllowSA1DescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgAllowSA1Description"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgDumpSoundEffectsNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgDumpSoundEffectsName"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgDumpSoundEffectsDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgDumpSoundEffectsDescription"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgVisualizeSPCNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgVisualizeSPCName"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgVisualizeSPCDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgVisualizeSPCDescription"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgRemoveFirstUseNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgRemoveFirstUseName"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgRemoveFirstUseDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgRemoveFirstUseDescription"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgStreamDirectNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgStreamDirectName"];

            return localizedString;
        }
        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgStreamDirectDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgStreamDirectDescription"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgGenerateSPCNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgGenerateSPCName"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgGenerateSPCDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgGenerateSPCDescription"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgHelpNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgHelpName"];

            return localizedString;
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string? GetCLArgHelpDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgHelpDescription"];

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
