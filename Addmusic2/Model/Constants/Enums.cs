using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SongListItemType
    {
        NA,
        Original,
        Custom,
        UserDefined,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SfxListItemType
    {
        NA,
        Sfx1DF9,
        Sfx1DFC,
        UserDefined,
    }
    public enum SongScope
    {
        Global,
        Local,
    }
}
