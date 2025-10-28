using Addmusic2.Model.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public string Number { get; set; } = string.Empty;
        [JsonIgnore]
        public int IntNumber
        {
            get
            {
                return Convert.ToInt32(Number, 16);
            }
        }
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("path", Required = Required.Always)]
        public string Path { get; set; } = string.Empty;
        [JsonProperty("type")]
        private string ItemType { get; set; } = string.Empty;
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
        public string Number { get; set; } = string.Empty;
        [JsonIgnore]
        public int IntNumber 
        { 
            get
            {
                return Convert.ToInt32(Number, 16);
            }
        }
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("path", Required = Required.Always)]
        public string Path { get; set; } = string.Empty;
        [JsonProperty("type")]
        private string ItemType { get; set; } = string.Empty;
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
        [JsonProperty("copyOf")]
        public string CopyOf { get; set; } = string.Empty;
    }

    internal class AddmusicSampleGroup
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("samples")]
        public List<AddmusicSample> Samples { get; set; } = new();
    }

    internal class AddmusicSample : IEquatable<AddmusicSample>
    {
        [JsonProperty("name")]
        public string Name {
            get => NameValue;
            set
            {
                var lastDirectorySeparator = (value.Contains(@"\"))
                        ? value.LastIndexOf(@"\")
                        : (value.Contains(@"/"))
                            ? value.LastIndexOf(@"/")
                            : 0;
                var lastPeriod = value.LastIndexOf('.');
                var fileName = (lastPeriod == -1)
                    ? value[lastDirectorySeparator..]
                    : value[lastDirectorySeparator..lastPeriod];
                NameValue = fileName;
            }
        }
        [JsonIgnore]
        private string NameValue { get; set; } = string.Empty;
        [JsonProperty("path", Required = Required.Always)]
        public string Path
        {
            get => PathValue;
            set
            {
                var lastDirectorySeparator = (value.Contains(@"\"))
                        ? value.LastIndexOf(@"\") +1
                        : (value.Contains(@"/"))
                            ? value.LastIndexOf(@"/") +1
                            : 0;
                var lastPeriod = value.LastIndexOf('.');
                var fileName = (lastPeriod == -1)
                    ? value[lastDirectorySeparator..]
                    : value[lastDirectorySeparator..lastPeriod];
                NameValue = fileName;
                PathValue = value;
            }
        }
        [JsonIgnore] 
        private string PathValue { get; set; } = string.Empty;
        [JsonProperty("important", Required = Required.Default)]
        public bool IsImportant { get; set; } = false;
        [JsonProperty("loop", Required = Required.Default)]
        public bool IsLooping { get; set; } = false;
        [JsonIgnore]
        public ushort LoopPoint { get; set; }
        [JsonIgnore]
        public int SampleDataSize { get; set; }

        public bool Equals(AddmusicSample? other)
        {
            if (ReferenceEquals(this, other)) return false;
            if (other == null) return false;

            if (this.Name == other.Name && this.SampleDataSize == other.SampleDataSize)
            {
                return true;
            }
            return false;
        }
    }

}
