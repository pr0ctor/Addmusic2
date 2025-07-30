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
                var songLineItems = line.Split(" ", 1).ToList();
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
                    ? songPath.LastIndexOf("/")
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
            var sfxNumberSet = new HashSet<string>();
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

                var matches = sfxLineRegex.Match(line);
                if(matches.Success)
                {
                    var sfxListItem = new SfxListItem();
                    var matchGroup = matches.Groups[1];

                    var sfxNumber = matchGroup.Captures[0].Value;

                    if (sfxNumberSet.Contains(sfxNumber))
                    {
                        // todo write exception error for this case as there is a duplicate entry
                        throw new Exception();
                    }
                    else
                    {
                        sfxNumberSet.Add(sfxNumber);
                    }

                    var sfxName = matchGroup.Captures[2].Value;
                    var sfxPath = (inSFX1DF9)
                        ? FileNames.FolderNames.Sfx1DF9 + "/" + matchGroup.Captures[2].Value
                        : FileNames.FolderNames.Sfx1DFC + "/" + matchGroup.Captures[2].Value;
                    var sfxType = (inSFX1DF9)
                        ? SfxListItemType.Sfx1DF9
                        : SfxListItemType.Sfx1DFC;

                    if (matchGroup.Captures[1].Value.Length > 0)
                    {
                        var settings = new SfxSettings();

                        settings.Loop = (matchGroup.Captures[1].Value.IndexOf("?") != -1)
                            ? true
                            : false;

                        settings.Pointer = (matchGroup.Captures[1].Value.IndexOf("*") != -1)
                            ? true
                            : false;

                        sfxListItem.Settings = settings;
                    }

                    if(inSFX1DF9)
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

    }
}
