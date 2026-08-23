using Addmusic2.Model;
using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Addmusic2.Services;
using Microsoft.Extensions.Logging;
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
                throw new Exception($"Text({textBeforeHexValue}) not found in the source text.");
            }

            return sourceText;
        }

        public static bool IsHexInRange(byte hexValue)
        {
            return hexValue < 0 || hexValue > MagicNumbers.HexCommandMaximum ? false : true;
        }

        public static int GetLastDirectorySeparatorIndex(string path)
        {
            return (path.Contains(@"\"))
                ? path.LastIndexOf(@"\")
                : (path.Contains(@"/"))
                    ? path.LastIndexOf(@"/")
                    : 0;
        }

        public static string StandardizeFileDirectoryDelimiters(string path)
        {
            var pathPieces = path.Split([@"\", @"/"], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            return Path.Combine(pathPieces);
        }

        public static void LoadSampleGroupToCache(IAddmusicLogger logger, IFileCachingService fileCache, AddmusicSampleGroup sampleGroup, string subDirectory = "")
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
                    logger.LogError(LogLevel.Debug, $"Sample({sample.Name}) in Sample Group({sampleGroup.Name}) not found.", true);
                    throw new FileNotFoundException($"Sample({sample.Name}) in Sample Group({sampleGroup.Name}) not found.");
                }

                fileCache.AddToCache(sample.Path, fullPath);
            }
        }

        public static void LoadSampleToCache(IAddmusicLogger logger, IFileCachingService fileCache, Sample sample, string subDirectory = "")
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
                logger.LogError(LogLevel.Debug, $"Sample({sample.Name}) not found.", true);
                throw new FileNotFoundException($"Sample({sample.Name}) not found.");
            }

            fileCache.AddToCache(sample.Path, fullPath);
        }

        public static byte[] GetSampleDataFromCache(IAddmusicLogger logger, IFileCachingService fileCache, Sample sample)
        {
            var (IsFound, Filename) = fileCache.CheckCacheContains(sample.Path);
            if(IsFound == true)
            {
                var dataStream = fileCache.GetFromCache(Filename);
                return dataStream!.ToArray();
            }
            else
            {
                logger.LogError(LogLevel.Debug, $"Sample({sample.Name}) not found.", true);
                throw new FileNotFoundException($"Sample({sample.Name}) not found.");
            }
        }

        public static int GetSampleDataLengthFromCache(IAddmusicLogger logger, IFileCachingService fileCache, Sample sample)
        {
            var (IsFound, Filename) = fileCache.CheckCacheContains(sample.Path);
            if (IsFound == true)
            {
                var dataStream = fileCache.GetFromCache(Filename);
                return dataStream!.ToArray().Length;
            }
            else
            {
                logger.LogError(LogLevel.Debug, $"Sample({sample.Name}) not found.", true);
                throw new FileNotFoundException($"Sample({sample.Name}) not found.");
            }
        }

        public static string ParseComparisonOperatorToString(ComparisonOperators comparisonOperator) => comparisonOperator switch
        {
            ComparisonOperators.EqualTo => "=",
            ComparisonOperators.NotEqualTo => "!=",
            ComparisonOperators.GreaterThan => ">",
            ComparisonOperators.GreaterThanEqualTo => ">=",
            ComparisonOperators.LessThan => "<",
            ComparisonOperators.LessThanEqualTo => "<=",
            _ => throw new ArgumentOutOfRangeException("Invalid Comparison Operator.")

        };

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

        public static int IndexOfSequenceOptimized<T>(List<T> source, List<T> sequence, int startIndex = 0)
        {
            if (sequence.Count == 0) return 0;

            int limit = source.Count - sequence.Count;
            for (int i = startIndex; i <= limit; i++)
            {
                bool match = true;
                for (int j = 0; j < sequence.Count; j++)
                {
                    if (!EqualityComparer<T>.Default.Equals(source[i + j], sequence[j]))
                    {
                        match = false;
                        break;
                    }
                }
                if (match) return i;
            }
            return -1;
        }

    }
}
