using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Constants
{
    public enum SongNodeType
    {
        // Overall Types
        Root,
        Atomic,
        Composite,
        Directive,
        Loop,
        Hex,
        Empty,
        // Atomic
        Note,
        Rest,
        Tie,
        Tune,
        Octave,
        LowerOctave,
        RaiseOctave,
        DefaultLength,
        Instrument,
        Volume,
        GlobalVolume,
        Pan,
        Quantization,
        Tempo,
        Vibrato,
        Noise,
        NoLoopCommand,
        QuestionMark,
        // Composite
        Triplet,
        PitchSlide,
        HexCommand,
        SampleLoad,
        Intro,
        // Directive
        Amk,
        Samples,
        Instruments,
        Pad,
        SPC,
        Path,
        Halvetempo,
        Option,
        OptionGroup,
        Channel,
        // Loop
        SimpleLoop,
        SuperLoop,
        RemoteCode,
        CallLoop,
        CallPreviousLoop,
        CallRemoteCode,
        StopRemoteCode,
        // Other
        Replacement,
    }
}
