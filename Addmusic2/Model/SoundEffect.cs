using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Addmusic2.Model.SongTree;
using Addmusic2.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class SoundEffect
    {
        public ISongNode RootNode { get; set; }
        public ISoundEffectParser Parser { get; set; }
        public string SoundEffectText { get; set; } = string.Empty;

        public SfxListItem Configuration { get; set; } = new();

        public SoundEffectData SoundEffectData { get; set; } = new();

        public SoundEffect() { }

        public SoundEffect(SoundEffectParser parser)
        {
            Parser = parser;
        }

        public SoundEffect(SoundEffectParser parser, ISongNode rootNode)
        {
            Parser = parser;
            RootNode = rootNode;
        }

        public void ParseSoundEffect()
        {
            if (RootNode == null)
            {
                throw new Exception();
            }

            var rootNode = RootNode as SongNode;

            if (rootNode == null || rootNode.NodeType != SongNodeType.Root)
            {
                throw new Exception();
            }

            SoundEffectData = Parser.ParseSoundEffectNodes(rootNode.Children);
        }
    }



/*    internal class SoundEffect
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string PointName { get; set; }
        public bool Add0 { get; set; } = true;
        public bool Exists { get; set; } = false;
        public int Bank { get; set; }
        public int Index { get; set; }
        public int PointsTo { get; set; } = 0;
        public int PositionInARAM { get; set; }
        public byte[] Data { get; set; }

        public List<string> DefineStrings { get; set; } = new List<string>();
        public List<string> AsmStrings { get; set; } = new List<string>();
        public List<byte[]> Code { get; set; } = new List<byte[]>();
        public List<string> AsmNames { get; set; } = new List<string>();
        public List<string> JmpNames { get; set; } = new List<string>();
        public List<int> JmpPositions { get; set; } = new List<int>();

        public SoundEffect()
        {
            Add0 = true;
            Exists = false;
            PointsTo = 0;
        }

        static int pos;
        static int line;
        static bool triplet;
        static int defaultNoteValue;
        static bool inDefineBlock;

        public string GetEffectiveName()
        {
            return (Name.Length == 0) ? PointName : Name;
        }

        public int GetHexadecimal()
        {
            int i = 0;
            int d = 0;
            int j;

            while (pos < Text.Length)
            {
                if ('0' <= Text[pos] && Text[pos] <= '9') j = Text[pos] - 0x30;
                else if ('A' <= Text[pos] && Text[pos] <= 'F') j = Text[pos] - 0x37;
                else if ('a' <= Text[pos] && Text[pos] <= 'f') j = Text[pos] - 0x57;
                else break;
                pos++;
                d++;
                i = (i * 16) + j;
            }

            return (d == 0) ? -1 : i;
        }

        public int GetInteger()
        {
            if (pos >= Text.Length) return -1;
            //if (text[pos] == '$') { pos++; return getHex(); }	// Allow for things such as t$20 instead of t32.

            int i = 0;
            int d = 0;

            while (Text[pos] >= '0' && Text[pos] <= '9')
            {
                d++;
                i = (i * 10) + Text[pos] - '0';
                pos++;
                if (pos >= Text.Length) break;
            }

            return (d == 0) ? -1 : i;
        }

        public int GetPitch(int letter, int octave)
        {
            int[] pitches = MagicNumbers.ValidPitches;

            letter = pitches[letter - 0x61] + (octave - 1) * 12 + 0x80;

            pos++;
            if (Text[pos] == '+') { letter++; pos++; }
            else if (Text[pos] == '-') { letter--; pos++; }
            if (letter < 0x80)
                return -1;
            if (letter >= 0xC6)
                return -2;

            return letter;
        }

        public int GetNoteLength(int note)
        {
            return 0;
        }

        public void Compile()
        {

        }

        public void ParseASM()
        {

        }

        public void CompileASM()
        {

        }

        public void ParseJSR()
        {

        }

        public void ParseDefine()
        {

        }

        public void ParseIfdef()
        {

        }

        public void ParseIfndef()
        {

        }

        public void ParseEndIf()
        {

        }

        public void PraseUndef()
        {
            pos += 6;
            if (inDefineBlock == false)
            {
                var x = 1;
            }
            //error2("#endif was found without a matching #ifdef or #ifndef");
            else
                inDefineBlock = false;
        }
    }*/
}
