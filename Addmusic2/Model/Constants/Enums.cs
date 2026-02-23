using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Constants
{
    public enum HexCommands
    {
        DDPitchBlend,
        EAVibratoFade,
        DFVibratoEnd,
        E4GlobalTranspose,
        E5Tremolo,
        EBPitchEnvelopeRelease,
        ECPitchEnvelopeAttack,
        EDCustomASDR,
        EDCustomGAIN,
        EFEcho1,
        F0EchoOff,
        F1Echo2,
        F2EchoFade,
        // F3SampleLoad,
        F4EnableYoshiDrumsChannel5,
        F4ToggleLegato,
        F4LightStacctao,
        F4EchoToggle,
        F4SNESSync,
        F4EnableYoshiDrums,
        F4TempoHikeOff,
        F4NSPCVelocityTable,
        F4RestoreInstrument,
        F5FIRFilter,
        F6DSPWrite,
        F8EnableNoise,
        F9DataSend,
        FAPitchModulation,
        FACurrentChannelGain,
        FASemitoneTune,
        FAAmplify,
        FAEchoBufferReserve,
        FAHotPatchPreset,
        FAHotPatchToggleBits,
        FBTrill,
        FBGlissando,
        FBEnableArgeggio,
        FCHexRemoteCommand,
        FCHexRemoteGain,
        FDTremoloOff,
        FEPitchEnvelopeOff,
        // Sound Effect Specific
        E0SfxPriority,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SongListItemType
    {
        [EnumMember(Value = "NA")]
        NA,
        [EnumMember(Value = "Original")]
        Original,
        [EnumMember(Value = "Custom")]
        Custom,
        [EnumMember(Value = "UserDefined")]
        UserDefined,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SfxListItemType
    {
        [EnumMember(Value = "NA")]
        NA,
        [EnumMember(Value = "1DF9")]
        Sfx1DF9,
        [EnumMember(Value = "1DFC")]
        Sfx1DFC,
        [EnumMember(Value = "UserDefined")]
        UserDefined,
    }
    public enum SongScope
    {
        Global,
        Local,
    }

    public enum ResultType
    {
        Success,
        Warning,
        Failure,
        Error,
        Skip,
    }

    public enum AmkType
    {
        Amk,
        Amm,
    }

    public enum AddmusicKVersion
    {
        Undefined,
        Version1,
        Version2,
        Version3, // unused
        Version4,
        AMM,
        AM4,
    }

    public enum OptionType
    {
        TempoImmunity,
        DivideTempo,
        Smwvtable,
        Nspcvtable,
        Noloop,
        Amk109hotpatch,
    }

    public enum VelocityTable
    {
        SmwVTable,
        NspcVTable,
    }
}
