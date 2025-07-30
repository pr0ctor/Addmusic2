using Addmusic2.Model.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class AddmusicSongSfxResources
    {
        [JsonProperty("songs", Required = Required.Always)]
        public AddmusicSongList Songs { get; set; } = new();
        [JsonProperty("sfx", Required = Required.Always)]
        public AddmusicSfxList SoundEffects { get; set; } = new();
        [JsonProperty("sampleGroups", Required = Required.Always)]
        public List<AddmusicSampleGroup> SampleGroups { get; set; } = new();
    }

    internal class AddmusicSongList
    {
        [JsonProperty("globals", Required = Required.Always)]
        public List<SongListItem> GlobalSongs { get; set; } = new();
        [JsonProperty("locals", Required = Required.Always)]
        public List<SongListItem> LocalSongs { get; set; } = new();
    }

    internal class SongListItem
    {
        [JsonProperty("number", Required = Required.Always)]
        public string Number { get; set; }
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }
        [JsonProperty("path", Required = Required.Always)]
        public string Path { get; set; }
        [JsonProperty("type")]
        private string ItemType { get; set; }
        [JsonIgnore]
        private SongListItemType TypeTemp { get; set; }
        [JsonIgnore]
        public SongListItemType Type
        {
            get
            {
                if (Enum.TryParse(ItemType, out SongListItemType result))
                {
                    return result;
                }
                else
                {

                    return TypeTemp;
                }
            }
            set
            {
                ItemType = Helpers.Helpers.ParseSongListItemTypeToString(value);
                TypeTemp = value;
            }
        }
    }

    internal class AddmusicSfxList
    {
        [JsonProperty("sfx1DF9", Required = Required.Always)]
        public List<SfxListItem> Sfx1DF9 { get; set; } = new();
        [JsonProperty("sfx1DFC", Required = Required.Always)]
        public List<SfxListItem> Sfx1DFC { get; set; } = new();
    }

    internal class SfxListItem
    {
        [JsonProperty("number", Required = Required.Always)]
        public string Number { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("path", Required = Required.Always)]
        public string Path { get; set; }
        [JsonProperty("type")]
        private string ItemType { get; set; }
        [JsonIgnore]
        private SfxListItemType TypeTemp { get; set; }
        [JsonIgnore]
        public SfxListItemType Type
        {
            get
            {
                if (Enum.TryParse(ItemType, out SfxListItemType result))
                {
                    return result;
                }
                else
                {

                    return TypeTemp;
                }
            }
            set
            {
                ItemType = Helpers.Helpers.ParseSfxListItemTypeToString(value);
                TypeTemp = value;
            }
        }
        [JsonProperty("settings", Required = Required.DisallowNull)]
        public SfxSettings Settings { get; set; } = new();
    }

    internal class SfxSettings
    {
        [JsonProperty("loop", Required = Required.DisallowNull)]
        public bool Loop { get; set; }
        [JsonProperty("pointer", Required = Required.DisallowNull)]
        public bool Pointer { get; set; }
    }

    internal class AddmusicSampleGroup
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set;}
        [JsonProperty("samples")]
        public List<AddmusicSample> Samples { get; set; }
    }

    internal class AddmusicSample
    {
        [JsonProperty("name")]
        public string Name { 
            get
            {
                if(NameValue.Length > 0)
                {
                    return NameValue;
                }
                else
                {
                    var lastDirectorySeparator = (Path.Contains(@"\"))
                        ? Path.LastIndexOf(@"\")
                        : (Path.Contains(@"/"))
                            ? Path.LastIndexOf(@"/")
                            : 0;
                    var lastPeriod = Path.LastIndexOf('.');
                    var fileName = (lastPeriod == -1) 
                        ? Path[lastDirectorySeparator..]
                        : Path[lastDirectorySeparator..lastPeriod];
                    //NameValue = fileName;
                    return fileName;
                }
            }
            set;
        }
        [JsonIgnore]
        private string NameValue { get; set; }
        [JsonProperty("path", Required = Required.Always)]
        public string Path { get; set; }
        [JsonProperty("important", Required = Required.Default)]
        public bool IsImportant { get; set; } = false;
        [JsonProperty("loop", Required = Required.Default)]
        public bool IsLooping { get; set; } = false;
        [JsonIgnore]
        public ushort LoopPoint { get; set; }
    }

}
