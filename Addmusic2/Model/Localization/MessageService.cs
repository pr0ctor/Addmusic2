using Addmusic2.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Localization
{
    internal sealed class MessageService(IStringLocalizer<Messages> _localizer)
    {

        #region Intro Messages

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetIntroAddmusicVersionMessage()
        {
            LocalizedString localizedString = _localizer["IntroAddmusicVersion"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetIntroParserVersionMessage()
        {
            LocalizedString localizedString = _localizer["IntroParserVersion"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetIntroReadTheReadMeMessage()
        {
            LocalizedString localizedString = _localizer["IntroReadTheReadMe"];

            return localizedString ?? "";
        }

        #endregion

        #region Info Messages

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetInfoTotalSpaceUsedBy1DF9SfxMessage(string amount)
        {
            LocalizedString localizedString = _localizer["InfoTotalSpaceUsedBy1DF9Sfx", amount];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetInfoTotalSpaceUsedBy1DFCSfxMessage(string amount)
        {
            LocalizedString localizedString = _localizer["InfoTotalSpaceUsedBy1DFCSfx", amount];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetInfoTotalSpaceUsedByAllSoundEffectsMessage(string amount)
        {
            LocalizedString localizedString = _localizer["InfoTotalSpaceUsedByAllSoundEffects", amount];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetInfoTotalSizeOfProgramMessage(string amount)
        {
            LocalizedString localizedString = _localizer["InfoTotalSizeOfProgram", amount];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetInfoTotalSizeOfProgramWithSfxMessage(string amount)
        {
            LocalizedString localizedString = _localizer["InfoTotalSizeOfProgramWithSfx", amount];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetInfoSoundEffectsNotIncludedMessage()
        {
            LocalizedString localizedString = _localizer["InfoSoundEffectsNotIncluded"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetInfoLineAndColumnStringMessage(string lineNumber, string columnNumber)
        {
            LocalizedString localizedString = _localizer["InfoLineAndColumnString", lineNumber, columnNumber];

            return localizedString ?? "";
        }

        #endregion

        #region Notifications

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetNotificationRomAmkDataVersionMismatchMessage(string version)
        {
            LocalizedString localizedString = _localizer["NotificationRomAmkDataVersionMismatch", version];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetNotificationRomAmkVersionCannotBeDeterminedMessage(string foundIdentifier, string expectedIdentifier)
        {
            LocalizedString localizedString = _localizer["NotificationRomAmkVersionCannotBeDetermined", foundIdentifier, expectedIdentifier];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetNotificationCurrentRomIsCleanRomMessage()
        {
            LocalizedString localizedString = _localizer["NotificationCurrentRomIsCleanRom"];

            return localizedString ?? "";
        }


        #endregion

        #region CLArgs Messages, Names, and Descriptions

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorInvalidCommandLineArgumentMessage(string foundCommand)
        {
            LocalizedString localizedString = _localizer["ErrorInvalidCommandLineArgument", foundCommand];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorMissingRequiredCommandLineArgumentsMessage(string commandList)
        {
            LocalizedString localizedString = _localizer["ErrorMissingRequiredCommandLineArguments", commandList];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgHelpOptionsHeaderMessage()
        {
            LocalizedString localizedString = _localizer["CLArgHelpOptionsHeader"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgRomNameNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgRomNameName"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgRomNameDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgRomNameDescription"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgConvertOldAddmusicNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgConvertOldAddmusicName"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgConvertOldAddmusicDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgConvertOldAddmusicDescription"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgCheckEchoNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgCheckEchoName"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgCheckEchoDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgCheckEchoDescription"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgBankStartNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgBankStartName"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgBankStartDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgBankStartDescription"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgVerboseLoggingNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgVerboseLoggingName"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgVerboseLoggingDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgVerboseLoggingDescription"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgAggressiveFreeSpaceNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgAggressiveFreeSpaceName"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgAggressiveFreeSpaceDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgAggressiveFreeSpaceDescription"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgDuplicateCheckingNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgDuplicateCheckingName"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgDuplicateCheckingDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgDuplicateCheckingDescription"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgHexValidationNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgHexValidationName"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgHexValidationDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgHexValidationDescription"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgCreatePatchNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgCreatePatchName"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgCreatePatchDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgCreatePatchDescription"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgOptimizeSampleUsageNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgOptimizeSampleUsageName"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgOptimizeSampleUsageDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgOptimizeSampleUsageDescription"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgAllowSA1NameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgAllowSA1Name"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgAllowSA1DescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgAllowSA1Description"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgDumpSoundEffectsNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgDumpSoundEffectsName"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgDumpSoundEffectsDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgDumpSoundEffectsDescription"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgVisualizeSPCNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgVisualizeSPCName"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgVisualizeSPCDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgVisualizeSPCDescription"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgRemoveFirstUseNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgRemoveFirstUseName"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgRemoveFirstUseDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgRemoveFirstUseDescription"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgStreamDirectNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgStreamDirectName"];

            return localizedString ?? "";
        }
        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgStreamDirectDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgStreamDirectDescription"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgGenerateSPCNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgGenerateSPCName"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgGenerateSPCDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgGenerateSPCDescription"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgHelpNameMessage()
        {
            LocalizedString localizedString = _localizer["CLArgHelpName"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetCLArgHelpDescriptionMessage()
        {
            LocalizedString localizedString = _localizer["CLArgHelpDescription"];

            return localizedString ?? "";
        }


        #endregion

        #region Warning Messages

        #region Validation Warning Messages

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetWarningDefaultLengthValidationMessage()
        {
            LocalizedString localizedString = _localizer["WarningDefaultLengthValidation"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetWarningSpcTextValueTooLongMessage(string elementName, string maxLength, string truncatedValue)
        {
            LocalizedString localizedString = _localizer["WarningSpcTextValueTooLong", elementName, maxLength, truncatedValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetWarningFactionalTempoRatioValueMessage(string songName, int lineNumber, int columnValue)
        {
            LocalizedString localizedString = _localizer["WarningFactionalTempoRatioValue", songName, lineNumber, columnValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetWarningOctaveRaisedTooHighMessage()
        {
            LocalizedString localizedString = _localizer["WarningOctaveRaisedTooHigh"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetWarningOctaveDroppedTooLowMessage()
        {
            LocalizedString localizedString = _localizer["WarningOctaveDroppedTooLow"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetWarningLoopIterationOutOfRangeMessage()
        {
            LocalizedString localizedString = _localizer["WarningLoopIterationOutOfRange"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetWarningFractionalTickValueFromDotsMessage(int dotCount, string songName, int lineNumber, int columnValue)
        {
            LocalizedString localizedString = _localizer["WarningFractionalTickValueFromDots", dotCount, songName, lineNumber, columnValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetWarningTripletFractionalTickValueFromDotsMessage(string songName, int lineNumber, int columnValue)
        {
            LocalizedString localizedString = _localizer["WarningTripletFractionalTickValueFromDots", songName, lineNumber, columnValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetWarningNoteLengthFractionalTickValueMessage(int divisor, string songName, int lineNumber, int columnValue)
        {
            LocalizedString localizedString = _localizer["WarningNoteLengthFractionalTickValue", divisor, songName, lineNumber, columnValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetWarningMarkEchoBufferAllocVCMDMessage()
        {
            LocalizedString localizedString = _localizer["WarningMarkEchoBufferAllocVCMD"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetWarningDuplicateSampleNameMessage(string sampleName)
        {
            LocalizedString localizedString = _localizer["WarningDuplicateSampleName", sampleName];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetWarningOldAddmusicLowNoteMessage()
        {
            LocalizedString localizedString = _localizer["WarningOldAddmusicLowNote"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetWarningTempoZeroedByOptionMessage()
        {
            LocalizedString localizedString = _localizer["WarningTempoZeroedByOption"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetWarningNspcVelocityTableAlreadyUsedMessage()
        {
            LocalizedString localizedString = _localizer["WarningNspcVelocityTableAlreadyUsed"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetWarningSmwVelocityTableAlreadyUsedMessage()
        {
            LocalizedString localizedString = _localizer["WarningSmwVelocityTableAlreadyUsed"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetWarningDivideTempoSetTo1Message()
        {
            LocalizedString localizedString = _localizer["WarningDivideTempoSetTo1"];

            return localizedString ?? "";
        }


        #endregion


        #region Addmusic Logic Warning Messages

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetWarningNoSfxEnabledAndDumpSfxMessage()
        {
            LocalizedString localizedString = _localizer["WarningNoSfxEnabledAndDumpSfx"];

            return localizedString ?? "";
        }


        #endregion


        #endregion

        #region Error Messages

        #region General Messages

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorCannotFindRomInLocationsMessage(string romName)
        {
            LocalizedString localizedString = _localizer["ErrorCannotFindRomInLocations", romName];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorFoundDuplicateDefaultSampleGroupsMessage()
        {
            LocalizedString localizedString = _localizer["ErrorFoundDuplicateDefaultSampleGroups"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorMissingDefaultSampleGroupMessage()
        {
            LocalizedString localizedString = _localizer["ErrorMissingDefaultSampleGroup"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorFoundDuplicateOptimizedSampleGroupsMessage()
        {
            LocalizedString localizedString = _localizer["ErrorFoundDuplicateOptimizedSampleGroups"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorMissingOptimizedSampleGroupMessage()
        {
            LocalizedString localizedString = _localizer["ErrorMissingOptimizedSampleGroup"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorProgramUploadPositionTextMissingMessage(string tagName, string fileName)
        {
            LocalizedString localizedString = _localizer["ErrorProgramUploadPositionTextMissing", tagName, fileName];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorMainLoopPositionTextMissingMessage(string tagName, string fileName)
        {
            LocalizedString localizedString = _localizer["ErrorMainLoopPositionTextMissing", tagName, fileName];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorReuploadPositionTextMissingMessage(string tagName, string fileName)
        {
            LocalizedString localizedString = _localizer["ErrorReuploadPositionTextMissing", tagName, fileName];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorMusicPointersTextMissingMessage(string tagName, string fileName)
        {
            LocalizedString localizedString = _localizer["ErrorMusicPointersTextMissing", tagName, fileName];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorAsarErrorOccurredMessage()
        {
            LocalizedString localizedString = _localizer["ErrorAsarErrorOccurred"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetConfigErrorDuplicateSongNumberMessage(string songNumber, string fileName)
        {
            LocalizedString localizedString = _localizer["ConfigErrorDuplicateSongNumber", songNumber, fileName];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetConfigErrorDuplicateSfxNumberMessage(string sfxNumber, string fileName)
        {
            LocalizedString localizedString = _localizer["ConfigErrorDuplicateSfxNumber", sfxNumber, fileName];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetConfigErrorMalformedSfxLineMessage(string sfxLine, string fileName)
        {
            LocalizedString localizedString = _localizer["ConfigErrorMalformedSfxLine", sfxLine, fileName];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorRomLessThanMinimumSizeMessage(string romName, string romSize, string romMinimumSize)
        {
            LocalizedString localizedString = _localizer["ErrorRomLessThanMinimumSize", romName, romSize, romMinimumSize];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorInvalidRomFileExtensionMessage(string romName, string validFileExtensions)
        {
            LocalizedString localizedString = _localizer["ErrorInvalidRomFileExtension", romName, validFileExtensions];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorRomFileNotFoundMessage(string romName, string romPath)
        {
            LocalizedString localizedString = _localizer["ErrorRomFileNotFound", romName, romPath];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorRomUnexpectedFileSizeMessage(string romName)
        {
            LocalizedString localizedString = _localizer["ErrorRomUnexpectedFileSize", romName];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetConfigErrorCannotFindConfigurationFileMessage(string fileName, string fileLocation)
        {
            LocalizedString localizedString = _localizer["ConfigErrorCannotFindConfigurationFile", fileName, fileLocation];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorNoSongChannelDataToExportMessage(string songName)
        {
            LocalizedString localizedString = _localizer["ErrorNoSongChannelDataToExport", songName];

            return localizedString ?? "";
        }

        #endregion


        #region Validation Error Messages

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetDefaultLengthOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorDefaultLengthOutOfRange", foundValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorVolumeVolumeValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorVolumeVolumeValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorVolumeFadeValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorVolumeFadeValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorGlobalVolumeVolumeValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorGlobalVolumeVolumeValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorGlobalVolumeFadeValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorGlobalVolumeFadeValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorSfxVolumeLeftVolumeValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorSfxVolumeLeftVolumeValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorSfxVolumeRightVolumeValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorSfxVolumeRightVolumeValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorSfxVolumeVolumeValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorSfxVolumeVolumeValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorPanDirectionOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorPanDirectionOutOfRange", foundValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorVibratoDelayOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorVibratoDelayOutOfRange", foundValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorVibratoRateOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorVibratoRateOutOfRange", foundValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorVibratoExtentOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorVibratoExtentOutOfRange", foundValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorTempoTempoValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorTempoTempoValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorTempoFadeValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorTempoFadeValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorNoiseValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorNoiseValueOutOfRange", foundValue, minValue.ToString("X2"), maxValue.ToString("X")];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorQuantizationVolumeValueOutOfRangeMessage()
        {
            LocalizedString localizedString = _localizer["ErrorQuantizationVolumeValueOutOfRange"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorSpcLengthInvalidValueMessage()
        {
            LocalizedString localizedString = _localizer["ErrorSpcLengthInvalidValue"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorSpcLengthValueTooLongMessage()
        {
            LocalizedString localizedString = _localizer["ErrorSpcLengthValueTooLong"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorInstrumentDefinitionMissingHexValuesMessage()
        {
            LocalizedString localizedString = _localizer["ErrorInstrumentDefinitionMissingHexValues"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorInstrumentDefinitionHexValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorInstrumentDefinitionHexValueOutOfRange", foundValue.ToString("X"), minValue.ToString("X2"), maxValue.ToString("X")];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorFractionalTempoRatioMessage(string songName, int lineNumber, int columnValue)
        {
            LocalizedString localizedString = _localizer["ErrorFractionalTempoRatio", songName, lineNumber, columnValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorAlreadyFractionalTempoRatioMessage(string songName, int lineNumber, int columnValue)
        {
            LocalizedString localizedString = _localizer["ErrorAlreadyFractionalTempoRatio", songName, lineNumber, columnValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorTempoRatioValueOverflowMessage(string songName, int lineNumber, int columnValue)
        {
            LocalizedString localizedString = _localizer["ErrorTempoRatioValueOverflow", songName, lineNumber, columnValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorQuestionMarkValueOutOfRangeMessage()
        {
            LocalizedString localizedString = _localizer["ErrorQuestionMarkValueOutOfRange"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorUndefinedNamedLoopCallMessage()
        {
            LocalizedString localizedString = _localizer["ErrorUndefinedNamedLoopCall"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorUndefinedRemoteCodeCallMessage()
        {
            LocalizedString localizedString = _localizer["ErrorUndefinedRemoteCodeCall"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorHexCommandValueOutOfRangeMessage(string hexValue, int minValue, int maxValue)
        {
            LocalizedString localizedString = _localizer["ErrorHexCommandValueOutOfRange", hexValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorUnknownHexCommandMessage(string hexValue)
        {
            LocalizedString localizedString = _localizer["ErrorUnknownHexCommand", hexValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorSampleLoadTuningValueOutOfRangeMessage(string hexValue, int minValue, int maxValue)
        {
            LocalizedString localizedString = _localizer["ErrorSampleLoadTuningValueOutOfRange", hexValue, minValue, maxValue];

            return localizedString ?? "";
        }


        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorDuplicateLoopNameDefinedMessage(string loopName)
        {
            LocalizedString localizedString = _localizer["ErrorDuplicateLoopNameDefined", loopName];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorDuplicateRemoteCodeDefinitionNameDefinedMessage(string remoteCodeDefinitionName)
        {
            LocalizedString localizedString = _localizer["ErrorDuplicateRemoteCodeDefinitionNameDefined", remoteCodeDefinitionName];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorMaximumAllowedNumberOfLoopsReachedMessage()
        {
            LocalizedString localizedString = _localizer["ErrorMaximumAllowedNumberOfLoopsReached"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorHexCommandSuppliedValueOutOfRangeMessage(string hexValue, string hexCommand, int minValue, int maxValue)
        {
            LocalizedString localizedString = _localizer["ErrorHexCommandSuppliedValueOutOfRange", hexValue, hexCommand, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorIntroDirectiveFoundInLoopMessage()
        {
            LocalizedString localizedString = _localizer["ErrorIntroDirectiveFoundInLoop"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorInstrumentValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorInstrumentValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorSfxInstrumentValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorSfxInstrumentValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorSfxInstrumentNoiseHexValueOutOfRangeMessage(int minValue, int maxValue, int foundValue)
        {
            LocalizedString localizedString = _localizer["ErrorSfxInstrumentNoiseHexValueOutOfRange", foundValue, minValue, maxValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorRecallLoopUsedBeforeDefinitionMessage()
        {
            LocalizedString localizedString = _localizer["ErrorRecallLoopUsedBeforeDefinition"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorRecallLoopRecallsSuperLoopMessage()
        {
            LocalizedString localizedString = _localizer["ErrorRecallLoopRecallsSuperLoop"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorInvalidCustomInstrumentBaseMessage(string instrumentDefinition)
        {
            LocalizedString localizedString = _localizer["ErrorInvalidCustomInstrumentBase", instrumentDefinition];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorMissingSampleNameTextMessage()
        {
            LocalizedString localizedString = _localizer["ErrorMissingSampleNameText"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorSampleNameNotPreviouslyDefinedMessage(string sampleName)
        {
            LocalizedString localizedString = _localizer["ErrorSampleNameNotPreviouslyDefined", sampleName];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorMissingSampleGroupNameDefinitionMessage()
        {
            LocalizedString localizedString = _localizer["ErrorMissingSampleGroupNameDefinition"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorMultipleSampleGroupDefinitionsMessage(string listOfSampleNames)
        {
            LocalizedString localizedString = _localizer["ErrorMultipleSampleGroupDefinitions", listOfSampleNames];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorMultipleSameSampleGroupNamesMessage(string sampleGroupName)
        {
            LocalizedString localizedString = _localizer["ErrorMultipleSameSampleGroupNames", sampleGroupName];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorDuplicateChannelNumberFoundMessage(string duplicateChannelNumber)
        {
            LocalizedString localizedString = _localizer["ErrorDuplicateChannelNumberFound", duplicateChannelNumber];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorNodeValidationResultErrorMessage(string nodeType, string nodeContents)
        {
            LocalizedString localizedString = _localizer["ErrorNodeValidationResultError", nodeType, nodeContents];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorNodeValidationResultFailureMessage(string nodeType, string nodeContents)
        {
            LocalizedString localizedString = _localizer["ErrorNodeValidationResultFailure", nodeType, nodeContents];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorNodeValidationResultWarningMessage(string nodeType, string nodeContents)
        {
            LocalizedString localizedString = _localizer["ErrorNodeValidationResultWarning", nodeType, nodeContents];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorNodeValidationResultSkipMessage(string nodeType, string nodeContents)
        {
            LocalizedString localizedString = _localizer["ErrorNodeValidationResultSkip", nodeType, nodeContents];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorSampleNameMissingFileExtensionMessage(string sampleName)
        {
            LocalizedString localizedString = _localizer["ErrorSampleNameMissingFileExtension", sampleName];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorSampleNameHasInvalidFileExtensionMessage(string sampleName, string fileExtension, string listOfValidExtensions)
        {
            LocalizedString localizedString = _localizer["ErrorSampleNameHasInvalidFileExtension", sampleName, fileExtension, listOfValidExtensions];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorInvalidAmkVersionFoundMessage(string foundAmkVersion)
        {
            LocalizedString localizedString = _localizer["ErrorInvalidAmkVersionFound", foundAmkVersion];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorAmkVersion3UnsupportedMessage()
        {
            LocalizedString localizedString = _localizer["ErrorAmkVersion3Unsupported"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorUndefinedInstrumentMessage(string instrument)
        {
            LocalizedString localizedString = _localizer["ErrorUndefinedInstrument", instrument];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorNotePitchTooLowMessage()
        {
            LocalizedString localizedString = _localizer["ErrorNotePitchTooLow"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorInvalidDivideTempoValueMessage()
        {
            LocalizedString localizedString = _localizer["ErrorInvalidDivideTempoValue"];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorInvalidPanSurroundSoundValueMessage(string panSurroundSoundValue)
        {
            LocalizedString localizedString = _localizer["ErrorInvalidPanSurroundSoundValue", panSurroundSoundValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorPanDurationOutOfRangeMessage(string panDurationValue, string minimumValue, string maximumValue)
        {
            LocalizedString localizedString = _localizer["ErrorPanDurationOutOfRange", panDurationValue, minimumValue, maximumValue];

            return localizedString ?? "";
        }

        [return: NotNullIfNotNull(nameof(_localizer))]
        public string GetErrorPanFinalValueOutOfRangeMessage(string finalPanningValue, string minimumValue, string maximumValue)
        {
            LocalizedString localizedString = _localizer["ErrorPanFinalValueOutOfRange", finalPanningValue, minimumValue, maximumValue];

            return localizedString ?? "";
        }

        #endregion

        #endregion
    }
}
