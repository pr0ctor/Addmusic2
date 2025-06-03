using Addmusic2.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.SongTree
{

    #region Special Directive Payloads

    internal class AmkVersionPayload : ISongNodePayload
    {
        public enum AmkType
        {
            Amk,
            Amm,
        }
        public AmkType AmkVersionType { get; set; }
        public string AmkVersion { get; set; }
        public AmkVersionPayload() { }
    }

    internal class SpcPayload : ISongNodePayload
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Game { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public string Length { get; set; } = string.Empty;

        public SpcPayload() { }

        public void AssignValue(string name, string value)
        {
            switch(name.ToLower())
            {
                case "#title": 
                    Title = value;
                    break;
                case "#author":
                    Author = value;
                    break;
                case "#game":
                    Game = value;
                    break;
                case "#comment":
                    Comment = value;
                    break;
                case "#length":
                    Length = value;
                    break;
                default:
                    break;
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if(Title.Length > 0)
            {
                builder.AppendLine($"\t#title {Title}");
            }
            if(Author.Length > 0)
            {
                builder.AppendLine($"\t#author {Author}");
            }
            if (Game.Length > 0)
            {
                builder.AppendLine($"\t#game {Game}");
            }
            if(Comment.Length > 0)
            {
                builder.AppendLine($"\t#comment {Comment}");
            }
            if(Length.Length > 0)
            {
                builder.AppendLine($"\t#length {Length}");
            }

            return builder.ToString();
        }
    }

    internal class SamplesPayload : ISongNodePayload
    {
        public string SampleGroupPath { get; set; }
        public List<string> Samples { get; set; }

        public SamplesPayload()
        {
            Samples = new List<string>();
        }

        public SamplesPayload(string sampleGroupPath, List<string> samples)
        {
            SampleGroupPath = sampleGroupPath;
            Samples = samples;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if(SampleGroupPath.Length > 0)
            {
                builder.AppendLine(SampleGroupPath);
            }

            foreach (var sample in Samples)
            {
                builder.AppendLine(sample);
            }

            return builder.ToString();
        }
    }

    internal class InstrumentsPayload : ISongNodePayload
    {
        public List<InstrumentDefinition> Instruments { get; set; }

        public InstrumentsPayload()
        {
            Instruments = new List<InstrumentDefinition>();
        }
        public InstrumentsPayload(List<InstrumentDefinition> instruments)
        {
            Instruments = instruments ?? new List<InstrumentDefinition>();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach(var instrument in Instruments)
            {
                builder.AppendLine($"\t{instrument}");
            }

            return builder.ToString();
        }
    }

    internal class PadPayload(string PadLength) : ISongNodePayload
    {
        public string PadLength { get; set; } = PadLength;

        public override string ToString()
        {
            return PadLength;
        }
    }

    internal class PathPayload(string PathText) : ISongNodePayload
    {
        public string PathText { get; set; } = PathText;

        public override string ToString()
        {
            return PathText;
        }
    }

    internal class ChannelPayload : ISongNodePayload
    {
        public int ChannelNumber { get; set; }
        public ChannelPayload() { }
    }

    internal class OptionPayload : ISongNodePayload
    {
        public enum OptionType
        {
            TempoImmunity,
            DivideTempo,
            Smwvtable,
            Nspcvtable,
            Noloop,
            Amk109hotpatch,
        }

        public OptionType Option { get; set; }
        public object OptionValue { get; set; }
        public OptionPayload() { }
    }

    #endregion


    #region Atomic Payloads

    internal class DefaultLengthPayload : ISongNodePayload
    {
        public int Length { get; set; }
        public bool UsedEquals { get; set; } = false;

        public DefaultLengthPayload() { }
        public DefaultLengthPayload(int length)
        {
            Length = length;
        }

        public override string ToString()
        {
            return $"l{((UsedEquals) ? "=" : "")}{Length}";
        }
    }

    internal class InstrumentPayload : ISongNodePayload
    {
        public int InstrumentNumber { get; set; }

        public InstrumentPayload() { }
        public InstrumentPayload(int instrumentNumber)
        {
            InstrumentNumber = instrumentNumber;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("@");
            builder.Append(InstrumentNumber);
            return builder.ToString();
        }
    }

    internal class NoisePayload : ISongNodePayload
    {
        public string NoiseValue { get; set; } = string.Empty;

        public NoisePayload() { }
        public NoisePayload(string noiseValue)
        {
            NoiseValue = noiseValue;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("n");
            builder.Append(NoiseValue);
            return builder.ToString();
        }
    }

    internal class NotePayload : ISongNodePayload
    {
        public enum Accidentals
        {
            None,
            Sharp,
            Flat
        }
        public string NoteValue { get; set; } = string.Empty;
        public Accidentals Accidental { get; set; } = Accidentals.None;
        public int Duration { get; set; } = -1;
        public int DotCount { get; set; } = -1;
        public List<SongNode> ConnectedTies { get; set; } = new List<SongNode>();

        public NotePayload() { }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(NoteValue);
            builder.Append(Helpers.Helpers.ParseAccidentalToString(Accidental));
            builder.Append((Duration == -1) ? "" : Duration);
            builder.Append(new string('.', DotCount));
            builder.Append(string.Join("", ConnectedTies));
            return builder.ToString();
        }
    }

    internal class OctavePayload : ISongNodePayload
    {
        public int OctaveNumber { get; set; }

        public OctavePayload() { }
        public OctavePayload(int octaveNumber)
        {
            OctaveNumber = octaveNumber;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("o");
            builder.Append(OctaveNumber);
            return builder.ToString();
        }
    }

    internal class PanPayload : ISongNodePayload
    {
        public int PanPosition { get; set; }
        public int SurroundSoundLeft { get; set; } = -1;
        public int SurroundSoundRight { get; set; } = -1;

        public PanPayload() { }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("y");
            builder.Append(PanPosition);
            if(SurroundSoundLeft > -1)
            {
                builder.Append($",{SurroundSoundLeft}");
            }
            if(SurroundSoundRight > -1)
            {
                builder.Append($",{SurroundSoundRight}");
            }
            return builder.ToString();
        }
    }

    internal class QuantizationPayload : ISongNodePayload
    {
        public int DelayValue { get; set; }
        public string VolumeValue { get; set; }
        public SongNode VolumeNode { get; set; }

        public QuantizationPayload() { }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("q");
            builder.Append(DelayValue);
            if(VolumeNode != null)
            {
                builder.Append(VolumeNode.Payload.ToString());
            }
            else
            {
                builder.Append(VolumeValue);
            }
            return builder.ToString();
        }
    }

    internal class QuestionMarkPayload : ISongNodePayload
    {
        public int MarkNumber { get; set; }
        public QuestionMarkPayload() { }
        public QuestionMarkPayload(int MarkNumber)
        {
            this.MarkNumber = MarkNumber;
        }
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"?{MarkNumber}");
            return builder.ToString();
        }
    }

    internal class TempoPayload : ISongNodePayload
    {
        public int Tempo { get; set; }
        public int FadeValue { get; set; } = -1;

        public TempoPayload() { }
        public TempoPayload(int tempo)
        {
            Tempo = tempo;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append('t');
            builder.Append(Tempo);
            if (FadeValue > -1)
            {
                builder.Append($",{FadeValue}");
            }
            return builder.ToString();
        }
    }

    internal class TiePayload : ISongNodePayload
    {
        public int Duration { get; set; }
        public int DotCount { get; set; } = 0;

        public TiePayload() { }
        public TiePayload(int duration)
        {
            Duration = duration;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append('t');
            builder.Append(Duration);
            if (DotCount > -1)
            {
                builder.Append($",{DotCount}");
            }
            return builder.ToString();
        }
    }

    internal class TunePayload : ISongNodePayload
    {
        public int TuneValue { get; set; }

        public TunePayload() { }
        public TunePayload(int tuneValue)
        {
            TuneValue = tuneValue;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("h");
            builder.Append(TuneValue);
            return builder.ToString();
        }
    }

    internal class VibratoPayload : ISongNodePayload
    {
        // Delay
        public int DelayDurationValue { get; set; } = -1;
        // Speed
        public int RateValue { get; set; }
        // Amplitude
        public int ExtentValue { get; set; }

        public VibratoPayload() { }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("p");
            if(DelayDurationValue > -1)
            {
                builder.Append($"{DelayDurationValue},");
            }
            builder.Append($"{RateValue},");
            builder.Append($"{ExtentValue}");
            return builder.ToString();
        }
    }

    internal class VolumePayload : ISongNodePayload
    {
        public int Volume { get; set; }
        public int FadeValue { get; set; } = -1;

        public VolumePayload() { }
        public VolumePayload(int volume)
        {
            Volume = volume;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Volume);
            if (FadeValue > -1)
            {
                builder.Append($",{FadeValue}");
            }
            return builder.ToString();
        }
    }

    #endregion


    #region Loop Payloads

    internal class CallRemoteCodePayload : ISongNodePayload
    {
        public string DefinitionName { get; set; } = string.Empty;
        public int EventType { get; set; }
        public int IntArgument { get; set; } = -1;
        public string HexArgument { get; set; } = string.Empty;

        public CallRemoteCodePayload() { }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("(!");
            builder.Append(DefinitionName);
            builder.Append($",{EventType}");
            if(IntArgument > -1 || HexArgument.Length > 0)
            {
                builder.Append($",{((IntArgument > 0) ? IntArgument : HexArgument)}");
            }
            builder.Append(")");
            return builder.ToString();
        }
    }

    internal class RemoteCodeDefinitionPayload : ISongNodePayload
    {
        public string DefinitionName { get; set; }
        public int EventType { get; set; }
        public int IntArgument { get; set; } = -1;
        public string HexArgument { get; set; }

        public RemoteCodeDefinitionPayload() { }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("h");
            //builder.Append(TuneValue);
            return builder.ToString();
        }
    }

    #endregion


    #region Composite Payloads

    internal class HexNumberPayload : ISongNodePayload
    {
        public string HexValue { get; set; }

        public HexNumberPayload() { }
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder = builder.Append(HexValue);
            return builder.ToString();
        }
    }

    internal class PitchSlidePayload : ISongNodePayload
    {
        public List<ISongNode> Nodes { get; set; } = new List<ISongNode>();
        public PitchSlidePayload() { }
    }

    internal class SampleLoadPayload : ISongNodePayload
    {
        public string SampleName { get; set; }
        public string TuningValue { get; set; }

        public SampleLoadPayload() { }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"({SampleName},{TuningValue}");
            return builder.ToString();
        }
    }

    #endregion

}
