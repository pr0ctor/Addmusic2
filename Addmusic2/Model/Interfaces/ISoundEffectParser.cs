using Addmusic2.Model.SongTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Interfaces
{
    internal interface ISoundEffectParser
    {
        public SoundEffectData SoundEffectData { get; set; }
        public SoundEffectData ParseSoundEffectNodes(List<ISongNode> nodes);
        public void ParseNode(SongNode node);
        public IValidationResult ValidateNode(ISongNode node);
        public void EvaluateNode(ISongNode node);
        public void CompileAsmElements(SoundEffectData soundEffectData);
    }
}
