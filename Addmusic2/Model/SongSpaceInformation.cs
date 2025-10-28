using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class SongSpaceInformation
    {
        public int SongStartPosition { get; set; }
        public int SongEndPosition { get; set; }
        public int SampleTableStartPosition { get; set; }
        public int SampleTableEndPosition { get; set; }
        public int ImportantSampleCount { get; set; }
        public int EchoBufferStartPosition { get; set; }
        public int EchoBufferEndPosition { get; set; }

        public Dictionary<AddmusicSample, (int StartPosition, int EndPosition)> SamplePositions { get; set; } = new();

    }
}
