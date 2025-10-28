using Addmusic2.Model;
using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Addmusic2.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Addmusic2.Model.SongTree.NotePayload;

namespace Addmusic2.Helpers
{
    internal static class Helpers
    {

        public static Regex GetHexValueAfterText(string textBeforeHexValue)
        {
            var regexString = $@"{textBeforeHexValue}\$([a-zA-Z0-9]{{1,5}})";
            return new(regexString);
        }

        public static string SetHexValueAfterText(string sourceText, string textBeforeHexValue, string valueToSet)
        {
            var regexString = $@"{textBeforeHexValue}\$([a-zA-Z0-9]{{1,5}})";

            var match = Regex.Match(sourceText, regexString);

            if(match.Success)
            {
                var foundText = match.Value;
                sourceText = sourceText.Replace(match.Value, $"{textBeforeHexValue}${valueToSet}");
            }
            else
            {
                // todo handle Exception
                throw new Exception();
            }

            return sourceText;
        }

        public static bool IsHexInRange(byte hexValue)
        {
            return hexValue < 0 || hexValue > MagicNumbers.HexCommandMaximum ? false : true;
        }

        public static string StandardizeFileDirectoryDelimiters(string path)
        {
            var pathPieces = path.Split([@"\", @"/"], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            return Path.Combine(pathPieces);
        }

        public static void LoadSampleGroupToCache(IFileCachingService fileCache, AddmusicSampleGroup sampleGroup, string subDirectory = "")
        {
            var intermediaryDirectory = StandardizeFileDirectoryDelimiters(subDirectory);
            foreach (var sample in sampleGroup.Samples)
            {
                var fullPath = "";
                var samplesPath = Path.Combine(FileNames.FolderNames.SamplesBase, sample.Path);
                var songPath = Path.Combine(FileNames.FolderNames.MusicBase, intermediaryDirectory, sample.Path);

                if(Path.Exists(samplesPath))
                {
                    fullPath = samplesPath;
                }
                else if(Path.Exists(songPath))
                {
                    fullPath = songPath;
                }
                else
                {
                    // todo throw exception and handle missing file
                }

                fileCache.AddToCache(sample.Path, fullPath);
            }
        }

        public static void LoadSampleToCache(IFileCachingService fileCache, AddmusicSample sample, string subDirectory = "")
        {
            var intermediaryDirectory = StandardizeFileDirectoryDelimiters(subDirectory);
            var fullPath = "";
            var samplesPath = Path.Combine(FileNames.FolderNames.SamplesBase, sample.Path);
            var songPath = Path.Combine(FileNames.FolderNames.MusicBase, intermediaryDirectory, sample.Path);

            if (Path.Exists(samplesPath))
            {
                fullPath = samplesPath;
            }
            else if (Path.Exists(songPath))
            {
                fullPath = songPath;
            }
            else
            {
                // todo throw exception and handle missing file
            }

            fileCache.AddToCache(sample.Path, fullPath);
        }

        public static byte[] GetSampleDataFromCache(IFileCachingService fileCache, AddmusicSample sample)
        {
            var cacheContains = fileCache.CheckCacheContains(sample.Path);
            if(cacheContains.IsFound == true)
            {
                var dataStream = fileCache.GetFromCache(cacheContains.Filename);
                return dataStream!.ToArray();
            }
            else
            {
                // todo update exception
                throw new Exception();
            }
        }

        public static int GetSampleDataLengthFromCache(IFileCachingService fileCache, AddmusicSample sample)
        {
            var cacheContains = fileCache.CheckCacheContains(sample.Path);
            if (cacheContains.IsFound == true)
            {
                var dataStream = fileCache.GetFromCache(cacheContains.Filename);
                return dataStream!.ToArray().Length;
            }
            else
            {
                // todo update exception
                throw new Exception();
            }
        }

        public static string ParseAccidentalToString(Accidentals accidental) => accidental switch
        {
            Accidentals.None => "",
            Accidentals.Sharp => "+",
            Accidentals.Flat => "-",
            _ => throw new ArgumentOutOfRangeException("Invalid Accidental.")
        };

        public static string ParseSongListItemTypeToString(SongListItemType itemType) => itemType switch
        {
            SongListItemType.Original => FileNames.FolderNames.MusicOriginal,
            SongListItemType.Custom => FileNames.FolderNames.MusicCustom,
            SongListItemType.UserDefined => "UserDefined",
            SongListItemType.NA => "",
            _ => throw new ArgumentOutOfRangeException("Invalid SongListItemType.")
        };

        public static string ParseSfxListItemTypeToString(SfxListItemType itemType) => itemType switch
        {
            SfxListItemType.Sfx1DF9 => FileNames.FolderNames.Sfx1DF9,
            SfxListItemType.Sfx1DFC => FileNames.FolderNames.Sfx1DFC,
            SfxListItemType.UserDefined => "UserDefined",
            SfxListItemType.NA => "",
            _ => throw new ArgumentOutOfRangeException("Invalid SfxListItemType.")
        };

        public static bool isDigits(string s)
        {
            if (s == null || s == "") return false;

            for (int i = 0; i < s.Length; i++)
                if ((s[i] ^ '0') > 9)
                    return false;

            return true;
        }

    }
}
