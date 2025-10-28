using Addmusic2.Model;
using Addmusic2.Model.Constants;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Addmusic2.Helpers
{
    internal static class FileConverters
    {

        public static void WriteResourceFile(AddmusicSongSfxResources resourceFile, string fileName)
        {
            var serializedString = JsonConvert.SerializeObject(resourceFile, Formatting.Indented);
            File.WriteAllText(fileName, serializedString);
        }

        public static void WriteOptionsFile(AddmusicOptions options, string fileName)
        {
            var serializedString = JsonConvert.SerializeObject(options, Formatting.Indented);
            File.WriteAllText(fileName, serializedString);
        }

        //public static AddmusicOptions ConvertTxtOptionsToJsonOptions(string fileData)
        //{
        //    return new();
        //}


        // Converts the "Addmusic_list.txt" to the new json format
        public static AddmusicSongList ConvertToAddmusicSongList(string fileData)
        {
            var oldSongList = fileData.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            var inGlobals = true;
            var addmusicSongList = new AddmusicSongList();
            var songNumberSet = new HashSet<string>();
            foreach (var line in oldSongList)
            {
                if(line == "Globals:")
                {
                    inGlobals = true;
                    continue;
                }
                else if (line == "Locals:")
                {
                    inGlobals = false;
                    continue;
                }
                // split once to get the number and then process the rest of the line
                var l = Regex.Replace(line, @"\s+", " ");
                var songLineItems = l.Split(" ", 2).ToList();
                var songNumber = songLineItems[0];

                if(songNumberSet.Contains(songNumber))
                {
                    // todo write exception error for this case as there is a duplicate entry
                    throw new Exception();
                }
                else
                {
                    songNumberSet.Add(songNumber);
                }

                var songPath = songLineItems[1];
                var songNameStartIndex = (songPath.LastIndexOf("/") != -1 ) 
                    ? songPath.LastIndexOf("/") + 1
                    : 0;
                var fileExtensionLength = (songPath.LastIndexOf(".") != -1)
                    ? songPath.Length - songPath.LastIndexOf(".")
                    : 0;
                var songName = songPath[songNameStartIndex..^fileExtensionLength];
                var songType = songPath.StartsWith(FileNames.FolderNames.MusicOriginal, StringComparison.InvariantCultureIgnoreCase)
                    ? SongListItemType.Original
                    : (songPath.StartsWith(FileNames.FolderNames.MusicCustom, StringComparison.InvariantCultureIgnoreCase)
                        ? SongListItemType.Custom
                        : SongListItemType.UserDefined);
                var songListItem = new SongListItem
                {
                    Name = songName,
                    Number = songNumber,
                    Path = songPath,
                    Type = songType,
                };
                if (inGlobals)
                {
                    addmusicSongList.GlobalSongs.Add(songListItem);
                }
                else if(!inGlobals)
                {
                    addmusicSongList.LocalSongs.Add(songListItem);
                }
            }

            return addmusicSongList;
        }

        // Converts the "Addmusic_sound effects.txt" to the new json format
        public static AddmusicSfxList ConvertToAddmusicSfxList(string fileData)
        {
            var oldsfxList = fileData.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            var sfxLineRegex = new Regex(@"([a-fA-f0-9]{1,2})\s*([\*\?]{0,2})\s*(.*)");
            var inSFX1DF9 = true;
            // slightly more robust but can likely be condensed
            var sfx1DF9NumberSet = new HashSet<string>();
            var sfx1DFCNumberSet = new HashSet<string>();
            var addmusicSfxList = new AddmusicSfxList();
            foreach (var line in oldsfxList)
            {
                if (line == "SFX1DF9:")
                {
                    inSFX1DF9 = true;
                    continue;
                }
                else if (line == "SFX1DFC:")
                {
                    inSFX1DF9 = false;
                    continue;
                }

                // todo fix this as it might cause issues with filenames that have a sequence of multiple space characters
                var l = Regex.Replace(line, @"\s+", " ");
                var matches = sfxLineRegex.Match(l);
                if(matches.Success)
                {
                    var sfxListItem = new SfxListItem();
                    var numberGroup = matches.Groups[1];
                    var toggleSymbolGroup = matches.Groups[2];
                    var sfxNameGroup = matches.Groups[3];

                    var sfxNumber = numberGroup.Value;

                    if(inSFX1DF9 == true && sfx1DF9NumberSet.Contains(sfxNumber)
                        || inSFX1DF9 == false && sfx1DFCNumberSet.Contains(sfxNumber))
                    {
                        // todo write exception error for this case as there is a duplicate entry
                        throw new Exception();
                    }

                    if(inSFX1DF9 == true)
                    {
                        sfx1DF9NumberSet.Add(sfxNumber);
                    }
                    else if(inSFX1DF9 == false)
                    {
                        sfx1DFCNumberSet.Add(sfxNumber);
                    }

                    var sfxName = sfxNameGroup.Value;
                    var sfxPath = (inSFX1DF9)
                        ? Path.Combine(FileNames.FolderNames.Sfx1DF9, sfxNameGroup.Value)
                        : Path.Combine(FileNames.FolderNames.Sfx1DFC, sfxNameGroup.Value);
                    var sfxType = (inSFX1DF9)
                        ? SfxListItemType.Sfx1DF9
                        : SfxListItemType.Sfx1DFC;

                    sfxListItem.Number = sfxNumber;
                    sfxListItem.Name = sfxName;
                    sfxListItem.Path = sfxPath;
                    sfxListItem.Type = sfxType;

                    if (toggleSymbolGroup != null && toggleSymbolGroup.Value.Length > 0)
                    {
                        var settings = new SfxSettings();

                        settings.Loop = (toggleSymbolGroup.Value.IndexOf("?") != -1)
                            ? true
                            : false;

                        settings.Pointer = (toggleSymbolGroup.Value.IndexOf("*") != -1)
                            ? true
                            : false;

                        if(settings.Pointer == true)
                        {
                            settings.CopyOf = sfxListItem.Name;
                        }

                        sfxListItem.Settings = settings;
                    }

                    if (inSFX1DF9)
                    {
                        addmusicSfxList.Sfx1DF9.Add(sfxListItem);
                    }
                    else if(!inSFX1DF9)
                    {
                        addmusicSfxList.Sfx1DFC.Add(sfxListItem);
                    }
                }
                else
                {
                    // todo write exception for malformed lineitem
                    throw new Exception();
                }

            }

            return addmusicSfxList;
        }

        // Converts the "Addmusic_saqmple groups.txt" to the new json format
        public static List<AddmusicSampleGroup> ConverToAddmusicSampleGroups(string fileData)
        {
            var sampleGroups = new List<AddmusicSampleGroup>();

            var oldSampleGroupList = fileData.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            var groupName = "";
            var sampleGroup = new AddmusicSampleGroup();
            var sampleList = new List<AddmusicSample>();
            foreach( var line in oldSampleGroupList )
            {
                var l = line.Trim();

                // indicates both the start of a sample group and the sample group's name
                if(l.StartsWith("#"))
                {
                    groupName = l[1..];
                    sampleGroup = new AddmusicSampleGroup()
                    {
                        Name = groupName,
                    };
                    sampleList = new();
                    continue;
                }

                // skip this line
                if(l == "{")
                {
                    continue;
                }
                // marks the end of a sample group
                else if(l == "}")
                {
                    sampleGroup.Samples = sampleList;
                    sampleGroups.Add(sampleGroup);
                    continue;
                }

                // remove quotes as they are redundant
                l = l.Replace("\"", "");
                // capture if theres an "!"s
                var important = (l.Contains("!")) ? true : false;
                // then remove it
                l = l.Replace("!", "");
                sampleList.Add(new AddmusicSample()
                {
                    Name = l,
                    Path = l,
                    IsImportant = important,
                });
            }

            return sampleGroups;
        }

    }
}
