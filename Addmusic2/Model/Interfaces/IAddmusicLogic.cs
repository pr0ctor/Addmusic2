using Addmusic2.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Interfaces
{
    internal interface IAddmusicLogic
    {
        public void Run();
        public void RunSingleSong(string fileData);
        public string PreProcessSong(string fileData);
        //public void ProcessSong(string fileData, SongScope songScope);
        public Song ProcessSong(string fileData, SongScope songScope);
        public void PostProcessSong();
    }
}
