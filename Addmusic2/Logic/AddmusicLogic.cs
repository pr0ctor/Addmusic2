using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Addmusic2.Model;
using Addmusic2.Model.Interfaces;
using Addmusic2.Model.Localization;
using Addmusic2.Parsers;
using Addmusic2.Services;
using Addmusic2.Visitors;
using Addmusic2.Model.Constants;
using Antlr4.Runtime;
using Microsoft.Extensions.Logging;
using AsarCLR.Asar191;

namespace Addmusic2.Logic
{
    internal class AddmusicLogic : IAddmusicLogic
    {
        private ILogger<IAddmusicLogic> _logger;
        private MessageService _messageService;
        private FileCachingService _fileService;
        private GlobalSettings _globalSettings;

        public DateTime LastModification { get; set; }

        public AddmusicLogic(ILogger<IAddmusicLogic> logger, MessageService messageService, IFileCachingService fileCachingService, IGlobalSettings globalSettings)
        {
            _logger = logger;
            _messageService = messageService;
            _fileService = (FileCachingService)fileCachingService;
            _globalSettings = (GlobalSettings)globalSettings;
        }

        public void Run()
        {

            LoadRequiredSampleGroups();

            // load global songs
            foreach(var globalSong in _globalSettings.ResourceList.Songs.GlobalSongs)
            {
                var fileData = File.ReadAllText(Path.Combine(FileNames.FolderNames.MusicBase, globalSong.Path));

                var preprocessedFileData = PreProcessSong(fileData);

                var songData = ProcessSong(preprocessedFileData, SongScope.Global);

                PostProcessSong();
            }

            // load local songs
            foreach (var localSong in _globalSettings.ResourceList.Songs.LocalSongs)
            {
                var fileData = File.ReadAllText(Path.Combine(FileNames.FolderNames.MusicBase, localSong.Path));

                var preprocessedFileData = PreProcessSong(fileData);

                var songData = ProcessSong(preprocessedFileData, SongScope.Local);

                PostProcessSong();
            }

            // load 1DF9 sound effects
            foreach(var sfx1DF9 in _globalSettings.ResourceList.SoundEffects.Sfx1DF9)
            {
                var filedata = File.ReadAllText(Path.Combine(FileNames.FolderNames.Sfx1DF9, sfx1DF9.Path));

                var preprocessedSfxFileData = PreprocessSoundEffect(filedata);

                var soundEffectData = ProcessSoundEffect(preprocessedSfxFileData);

                PostProcessSoundEffect();
            }

            // load 1DFC sound effects
            foreach (var sfx1DFC in _globalSettings.ResourceList.SoundEffects.Sfx1DFC)
            {
                var filedata = File.ReadAllText(Path.Combine(FileNames.FolderNames.Sfx1DFC, sfx1DFC.Path));

                var preprocessedSfxFileData = PreprocessSoundEffect(filedata);

                var soundEffectData = ProcessSoundEffect(preprocessedSfxFileData);

                PostProcessSoundEffect();
            }


            //var fileData = File.ReadAllText(@"Samples/Seenpoint Intro.txt");

            //var songData = PreProcessSong(fileData);
            //ProcessSong(songData);

            //PostProcessSong();
        }

        #region Song Processing

        public void RunSingleSong(string fileData)
        {

        }

        public string PreProcessSong(string fileData)
        {
            var replacementsRegex = new Regex(@$"""([^\s=""]+)\s*=\s*([^""]+)""");

            var matches = replacementsRegex.Matches(fileData);

            foreach (Match match in matches)
            {
                var searchValue = match.Groups[1].Value;
                var replaceValue = match.Groups[2].Value;

                fileData = fileData.Replace(searchValue, replaceValue);
            }

            return fileData;
        }

        public Song ProcessSong(string fileData, SongScope songScope)
        {
            var stream = CharStreams.fromString(fileData);

            var lexer = new MmlLexer(stream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new MmlParser(tokenStream);
            var mmlVisitor = new AdvMmlVisitor();

            var songContext = parser.song();

            var rootNode = mmlVisitor.VisitSong(songContext);

            // potentially logic for changing parsers

            var songParser = new SongParser(
                _logger,
                _messageService,
                _globalSettings,
                _fileService,
                songScope
            );

            var song = new Song(songParser, rootNode)
            {
                SongText = fileData,
            };

            //song.ParseSong();

            return song;
        }

        public void PostProcessSong()
        {

        }

        #endregion


        #region Sound Effect Processing

        public string PreprocessSoundEffect(string fileData)
        {
            return fileData;
        }

        public SoundEffect ProcessSoundEffect(string fileData)
        {
            var stream = CharStreams.fromString(fileData);

            var lexer = new SfxLexer(stream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new SfxParser(tokenStream);
            var sfxVisitor = new AdvSfxVisitor();

            var soundEffectContext = parser.soundEffect();

            var rootNode = sfxVisitor.VisitSoundEffect(soundEffectContext);

            var soundEffectParser = new SoundEffectParser(
                _logger,
                _messageService,
                _globalSettings,
                _fileService
            );

            var soundEffect = new SoundEffect(soundEffectParser, rootNode)
            {
                SoundEffectText = fileData,
            };

            // soundEffect.ParseSoundEffect();

            return soundEffect;
        }

        public void PostProcessSoundEffect()
        {

        }


        #endregion


        private void LoadRequiredSampleGroups()
        {
            var sampleGroups = _globalSettings.ResourceList.SampleGroups;

            var defaultGroup = sampleGroups.FindAll(g => g.Name.Equals(FileNames.FolderNames.SamplesDefault, StringComparison.InvariantCultureIgnoreCase));
            var optimizedGroup = sampleGroups.FindAll(g => g.Name.Equals(FileNames.FolderNames.SamplesOptimized, StringComparison.InvariantCultureIgnoreCase));

            if(defaultGroup.Count > 1)
            {
                // todo handle more than one "default" group
            }
            else if(defaultGroup.Count == 0)
            {
                // todo handle missing "default" group
            }

            if (optimizedGroup.Count > 1)
            {
                // todo handle more than one "optimized" group
            }
            else if(optimizedGroup.Count == 0)
            {
                // todo handle missing "optimized" group
            }

            Helpers.Helpers.LoadSampleGroupToCache(_fileService, defaultGroup.First());
            Helpers.Helpers.LoadSampleGroupToCache(_fileService, optimizedGroup.First());
        }
    }
}
