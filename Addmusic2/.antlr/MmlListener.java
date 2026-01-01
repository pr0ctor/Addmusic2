// Generated from d:/Projects/Visual Studio/Addmusic2/Addmusic2/Addmusic2/Mml.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link MmlParser}.
 */
public interface MmlListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link MmlParser#song}.
	 * @param ctx the parse tree
	 */
	void enterSong(MmlParser.SongContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#song}.
	 * @param ctx the parse tree
	 */
	void exitSong(MmlParser.SongContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#songElement}.
	 * @param ctx the parse tree
	 */
	void enterSongElement(MmlParser.SongElementContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#songElement}.
	 * @param ctx the parse tree
	 */
	void exitSongElement(MmlParser.SongElementContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#specialDirective}.
	 * @param ctx the parse tree
	 */
	void enterSpecialDirective(MmlParser.SpecialDirectiveContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#specialDirective}.
	 * @param ctx the parse tree
	 */
	void exitSpecialDirective(MmlParser.SpecialDirectiveContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#samples}.
	 * @param ctx the parse tree
	 */
	void enterSamples(MmlParser.SamplesContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#samples}.
	 * @param ctx the parse tree
	 */
	void exitSamples(MmlParser.SamplesContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#samplesList}.
	 * @param ctx the parse tree
	 */
	void enterSamplesList(MmlParser.SamplesListContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#samplesList}.
	 * @param ctx the parse tree
	 */
	void exitSamplesList(MmlParser.SamplesListContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#instruments}.
	 * @param ctx the parse tree
	 */
	void enterInstruments(MmlParser.InstrumentsContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#instruments}.
	 * @param ctx the parse tree
	 */
	void exitInstruments(MmlParser.InstrumentsContext ctx);
	/**
	 * Enter a parse tree produced by the {@code NamedInstrumentListItem}
	 * labeled alternative in {@link MmlParser#instrumentsList}.
	 * @param ctx the parse tree
	 */
	void enterNamedInstrumentListItem(MmlParser.NamedInstrumentListItemContext ctx);
	/**
	 * Exit a parse tree produced by the {@code NamedInstrumentListItem}
	 * labeled alternative in {@link MmlParser#instrumentsList}.
	 * @param ctx the parse tree
	 */
	void exitNamedInstrumentListItem(MmlParser.NamedInstrumentListItemContext ctx);
	/**
	 * Enter a parse tree produced by the {@code InstrumentListItem}
	 * labeled alternative in {@link MmlParser#instrumentsList}.
	 * @param ctx the parse tree
	 */
	void enterInstrumentListItem(MmlParser.InstrumentListItemContext ctx);
	/**
	 * Exit a parse tree produced by the {@code InstrumentListItem}
	 * labeled alternative in {@link MmlParser#instrumentsList}.
	 * @param ctx the parse tree
	 */
	void exitInstrumentListItem(MmlParser.InstrumentListItemContext ctx);
	/**
	 * Enter a parse tree produced by the {@code NoiseInstrumentListItem}
	 * labeled alternative in {@link MmlParser#instrumentsList}.
	 * @param ctx the parse tree
	 */
	void enterNoiseInstrumentListItem(MmlParser.NoiseInstrumentListItemContext ctx);
	/**
	 * Exit a parse tree produced by the {@code NoiseInstrumentListItem}
	 * labeled alternative in {@link MmlParser#instrumentsList}.
	 * @param ctx the parse tree
	 */
	void exitNoiseInstrumentListItem(MmlParser.NoiseInstrumentListItemContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#spc}.
	 * @param ctx the parse tree
	 */
	void enterSpc(MmlParser.SpcContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#spc}.
	 * @param ctx the parse tree
	 */
	void exitSpc(MmlParser.SpcContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#spcList}.
	 * @param ctx the parse tree
	 */
	void enterSpcList(MmlParser.SpcListContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#spcList}.
	 * @param ctx the parse tree
	 */
	void exitSpcList(MmlParser.SpcListContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#pad}.
	 * @param ctx the parse tree
	 */
	void enterPad(MmlParser.PadContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#pad}.
	 * @param ctx the parse tree
	 */
	void exitPad(MmlParser.PadContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#path}.
	 * @param ctx the parse tree
	 */
	void enterPath(MmlParser.PathContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#path}.
	 * @param ctx the parse tree
	 */
	void exitPath(MmlParser.PathContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#halvetempo}.
	 * @param ctx the parse tree
	 */
	void enterHalvetempo(MmlParser.HalvetempoContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#halvetempo}.
	 * @param ctx the parse tree
	 */
	void exitHalvetempo(MmlParser.HalvetempoContext ctx);
	/**
	 * Enter a parse tree produced by the {@code OptionGroup}
	 * labeled alternative in {@link MmlParser#option}.
	 * @param ctx the parse tree
	 */
	void enterOptionGroup(MmlParser.OptionGroupContext ctx);
	/**
	 * Exit a parse tree produced by the {@code OptionGroup}
	 * labeled alternative in {@link MmlParser#option}.
	 * @param ctx the parse tree
	 */
	void exitOptionGroup(MmlParser.OptionGroupContext ctx);
	/**
	 * Enter a parse tree produced by the {@code SingleOption}
	 * labeled alternative in {@link MmlParser#option}.
	 * @param ctx the parse tree
	 */
	void enterSingleOption(MmlParser.SingleOptionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code SingleOption}
	 * labeled alternative in {@link MmlParser#option}.
	 * @param ctx the parse tree
	 */
	void exitSingleOption(MmlParser.SingleOptionContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#optionItem}.
	 * @param ctx the parse tree
	 */
	void enterOptionItem(MmlParser.OptionItemContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#optionItem}.
	 * @param ctx the parse tree
	 */
	void exitOptionItem(MmlParser.OptionItemContext ctx);
	/**
	 * Enter a parse tree produced by the {@code GeneralAmkVersion}
	 * labeled alternative in {@link MmlParser#amk}.
	 * @param ctx the parse tree
	 */
	void enterGeneralAmkVersion(MmlParser.GeneralAmkVersionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code GeneralAmkVersion}
	 * labeled alternative in {@link MmlParser#amk}.
	 * @param ctx the parse tree
	 */
	void exitGeneralAmkVersion(MmlParser.GeneralAmkVersionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code AmmVersion}
	 * labeled alternative in {@link MmlParser#amk}.
	 * @param ctx the parse tree
	 */
	void enterAmmVersion(MmlParser.AmmVersionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code AmmVersion}
	 * labeled alternative in {@link MmlParser#amk}.
	 * @param ctx the parse tree
	 */
	void exitAmmVersion(MmlParser.AmmVersionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Am4Version}
	 * labeled alternative in {@link MmlParser#amk}.
	 * @param ctx the parse tree
	 */
	void enterAm4Version(MmlParser.Am4VersionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Am4Version}
	 * labeled alternative in {@link MmlParser#amk}.
	 * @param ctx the parse tree
	 */
	void exitAm4Version(MmlParser.Am4VersionContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#amm}.
	 * @param ctx the parse tree
	 */
	void enterAmm(MmlParser.AmmContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#amm}.
	 * @param ctx the parse tree
	 */
	void exitAmm(MmlParser.AmmContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#am4}.
	 * @param ctx the parse tree
	 */
	void enterAm4(MmlParser.Am4Context ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#am4}.
	 * @param ctx the parse tree
	 */
	void exitAm4(MmlParser.Am4Context ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#amkVersion}.
	 * @param ctx the parse tree
	 */
	void enterAmkVersion(MmlParser.AmkVersionContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#amkVersion}.
	 * @param ctx the parse tree
	 */
	void exitAmkVersion(MmlParser.AmkVersionContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#soundChannel}.
	 * @param ctx the parse tree
	 */
	void enterSoundChannel(MmlParser.SoundChannelContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#soundChannel}.
	 * @param ctx the parse tree
	 */
	void exitSoundChannel(MmlParser.SoundChannelContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#introEnd}.
	 * @param ctx the parse tree
	 */
	void enterIntroEnd(MmlParser.IntroEndContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#introEnd}.
	 * @param ctx the parse tree
	 */
	void exitIntroEnd(MmlParser.IntroEndContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#channelContents}.
	 * @param ctx the parse tree
	 */
	void enterChannelContents(MmlParser.ChannelContentsContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#channelContents}.
	 * @param ctx the parse tree
	 */
	void exitChannelContents(MmlParser.ChannelContentsContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#atomics}.
	 * @param ctx the parse tree
	 */
	void enterAtomics(MmlParser.AtomicsContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#atomics}.
	 * @param ctx the parse tree
	 */
	void exitAtomics(MmlParser.AtomicsContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#note}.
	 * @param ctx the parse tree
	 */
	void enterNote(MmlParser.NoteContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#note}.
	 * @param ctx the parse tree
	 */
	void exitNote(MmlParser.NoteContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#rest}.
	 * @param ctx the parse tree
	 */
	void enterRest(MmlParser.RestContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#rest}.
	 * @param ctx the parse tree
	 */
	void exitRest(MmlParser.RestContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#octave}.
	 * @param ctx the parse tree
	 */
	void enterOctave(MmlParser.OctaveContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#octave}.
	 * @param ctx the parse tree
	 */
	void exitOctave(MmlParser.OctaveContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#lowerOctave}.
	 * @param ctx the parse tree
	 */
	void enterLowerOctave(MmlParser.LowerOctaveContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#lowerOctave}.
	 * @param ctx the parse tree
	 */
	void exitLowerOctave(MmlParser.LowerOctaveContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#raiseOctave}.
	 * @param ctx the parse tree
	 */
	void enterRaiseOctave(MmlParser.RaiseOctaveContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#raiseOctave}.
	 * @param ctx the parse tree
	 */
	void exitRaiseOctave(MmlParser.RaiseOctaveContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#noiseNote}.
	 * @param ctx the parse tree
	 */
	void enterNoiseNote(MmlParser.NoiseNoteContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#noiseNote}.
	 * @param ctx the parse tree
	 */
	void exitNoiseNote(MmlParser.NoiseNoteContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Volume}
	 * labeled alternative in {@link MmlParser#volumeCommand}.
	 * @param ctx the parse tree
	 */
	void enterVolume(MmlParser.VolumeContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Volume}
	 * labeled alternative in {@link MmlParser#volumeCommand}.
	 * @param ctx the parse tree
	 */
	void exitVolume(MmlParser.VolumeContext ctx);
	/**
	 * Enter a parse tree produced by the {@code HexVolume}
	 * labeled alternative in {@link MmlParser#volumeCommand}.
	 * @param ctx the parse tree
	 */
	void enterHexVolume(MmlParser.HexVolumeContext ctx);
	/**
	 * Exit a parse tree produced by the {@code HexVolume}
	 * labeled alternative in {@link MmlParser#volumeCommand}.
	 * @param ctx the parse tree
	 */
	void exitHexVolume(MmlParser.HexVolumeContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Tune}
	 * labeled alternative in {@link MmlParser#tuneCommand}.
	 * @param ctx the parse tree
	 */
	void enterTune(MmlParser.TuneContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Tune}
	 * labeled alternative in {@link MmlParser#tuneCommand}.
	 * @param ctx the parse tree
	 */
	void exitTune(MmlParser.TuneContext ctx);
	/**
	 * Enter a parse tree produced by the {@code HexTune}
	 * labeled alternative in {@link MmlParser#tuneCommand}.
	 * @param ctx the parse tree
	 */
	void enterHexTune(MmlParser.HexTuneContext ctx);
	/**
	 * Exit a parse tree produced by the {@code HexTune}
	 * labeled alternative in {@link MmlParser#tuneCommand}.
	 * @param ctx the parse tree
	 */
	void exitHexTune(MmlParser.HexTuneContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#quantization}.
	 * @param ctx the parse tree
	 */
	void enterQuantization(MmlParser.QuantizationContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#quantization}.
	 * @param ctx the parse tree
	 */
	void exitQuantization(MmlParser.QuantizationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Pan}
	 * labeled alternative in {@link MmlParser#panCommand}.
	 * @param ctx the parse tree
	 */
	void enterPan(MmlParser.PanContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Pan}
	 * labeled alternative in {@link MmlParser#panCommand}.
	 * @param ctx the parse tree
	 */
	void exitPan(MmlParser.PanContext ctx);
	/**
	 * Enter a parse tree produced by the {@code HexPan}
	 * labeled alternative in {@link MmlParser#panCommand}.
	 * @param ctx the parse tree
	 */
	void enterHexPan(MmlParser.HexPanContext ctx);
	/**
	 * Exit a parse tree produced by the {@code HexPan}
	 * labeled alternative in {@link MmlParser#panCommand}.
	 * @param ctx the parse tree
	 */
	void exitHexPan(MmlParser.HexPanContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Vibrato}
	 * labeled alternative in {@link MmlParser#vibratoCommand}.
	 * @param ctx the parse tree
	 */
	void enterVibrato(MmlParser.VibratoContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Vibrato}
	 * labeled alternative in {@link MmlParser#vibratoCommand}.
	 * @param ctx the parse tree
	 */
	void exitVibrato(MmlParser.VibratoContext ctx);
	/**
	 * Enter a parse tree produced by the {@code HexVibrato}
	 * labeled alternative in {@link MmlParser#vibratoCommand}.
	 * @param ctx the parse tree
	 */
	void enterHexVibrato(MmlParser.HexVibratoContext ctx);
	/**
	 * Exit a parse tree produced by the {@code HexVibrato}
	 * labeled alternative in {@link MmlParser#vibratoCommand}.
	 * @param ctx the parse tree
	 */
	void exitHexVibrato(MmlParser.HexVibratoContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#pitchslide}.
	 * @param ctx the parse tree
	 */
	void enterPitchslide(MmlParser.PitchslideContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#pitchslide}.
	 * @param ctx the parse tree
	 */
	void exitPitchslide(MmlParser.PitchslideContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#triplet}.
	 * @param ctx the parse tree
	 */
	void enterTriplet(MmlParser.TripletContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#triplet}.
	 * @param ctx the parse tree
	 */
	void exitTriplet(MmlParser.TripletContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#defaultLength}.
	 * @param ctx the parse tree
	 */
	void enterDefaultLength(MmlParser.DefaultLengthContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#defaultLength}.
	 * @param ctx the parse tree
	 */
	void exitDefaultLength(MmlParser.DefaultLengthContext ctx);
	/**
	 * Enter a parse tree produced by the {@code GlobalVolume}
	 * labeled alternative in {@link MmlParser#globalVolumeCommand}.
	 * @param ctx the parse tree
	 */
	void enterGlobalVolume(MmlParser.GlobalVolumeContext ctx);
	/**
	 * Exit a parse tree produced by the {@code GlobalVolume}
	 * labeled alternative in {@link MmlParser#globalVolumeCommand}.
	 * @param ctx the parse tree
	 */
	void exitGlobalVolume(MmlParser.GlobalVolumeContext ctx);
	/**
	 * Enter a parse tree produced by the {@code HexGlobalVolume}
	 * labeled alternative in {@link MmlParser#globalVolumeCommand}.
	 * @param ctx the parse tree
	 */
	void enterHexGlobalVolume(MmlParser.HexGlobalVolumeContext ctx);
	/**
	 * Exit a parse tree produced by the {@code HexGlobalVolume}
	 * labeled alternative in {@link MmlParser#globalVolumeCommand}.
	 * @param ctx the parse tree
	 */
	void exitHexGlobalVolume(MmlParser.HexGlobalVolumeContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Tempo}
	 * labeled alternative in {@link MmlParser#tempoCommand}.
	 * @param ctx the parse tree
	 */
	void enterTempo(MmlParser.TempoContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Tempo}
	 * labeled alternative in {@link MmlParser#tempoCommand}.
	 * @param ctx the parse tree
	 */
	void exitTempo(MmlParser.TempoContext ctx);
	/**
	 * Enter a parse tree produced by the {@code HexTempo}
	 * labeled alternative in {@link MmlParser#tempoCommand}.
	 * @param ctx the parse tree
	 */
	void enterHexTempo(MmlParser.HexTempoContext ctx);
	/**
	 * Exit a parse tree produced by the {@code HexTempo}
	 * labeled alternative in {@link MmlParser#tempoCommand}.
	 * @param ctx the parse tree
	 */
	void exitHexTempo(MmlParser.HexTempoContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Instrument}
	 * labeled alternative in {@link MmlParser#instrumentCommand}.
	 * @param ctx the parse tree
	 */
	void enterInstrument(MmlParser.InstrumentContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Instrument}
	 * labeled alternative in {@link MmlParser#instrumentCommand}.
	 * @param ctx the parse tree
	 */
	void exitInstrument(MmlParser.InstrumentContext ctx);
	/**
	 * Enter a parse tree produced by the {@code HexInstrument}
	 * labeled alternative in {@link MmlParser#instrumentCommand}.
	 * @param ctx the parse tree
	 */
	void enterHexInstrument(MmlParser.HexInstrumentContext ctx);
	/**
	 * Exit a parse tree produced by the {@code HexInstrument}
	 * labeled alternative in {@link MmlParser#instrumentCommand}.
	 * @param ctx the parse tree
	 */
	void exitHexInstrument(MmlParser.HexInstrumentContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#nakedTie}.
	 * @param ctx the parse tree
	 */
	void enterNakedTie(MmlParser.NakedTieContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#nakedTie}.
	 * @param ctx the parse tree
	 */
	void exitNakedTie(MmlParser.NakedTieContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#qmark}.
	 * @param ctx the parse tree
	 */
	void enterQmark(MmlParser.QmarkContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#qmark}.
	 * @param ctx the parse tree
	 */
	void exitQmark(MmlParser.QmarkContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#pipe}.
	 * @param ctx the parse tree
	 */
	void enterPipe(MmlParser.PipeContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#pipe}.
	 * @param ctx the parse tree
	 */
	void exitPipe(MmlParser.PipeContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#loopers}.
	 * @param ctx the parse tree
	 */
	void enterLoopers(MmlParser.LoopersContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#loopers}.
	 * @param ctx the parse tree
	 */
	void exitLoopers(MmlParser.LoopersContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#logicControls}.
	 * @param ctx the parse tree
	 */
	void enterLogicControls(MmlParser.LogicControlsContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#logicControls}.
	 * @param ctx the parse tree
	 */
	void exitLogicControls(MmlParser.LogicControlsContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#logicCalls}.
	 * @param ctx the parse tree
	 */
	void enterLogicCalls(MmlParser.LogicCallsContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#logicCalls}.
	 * @param ctx the parse tree
	 */
	void exitLogicCalls(MmlParser.LogicCallsContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#remoteLogicCalls}.
	 * @param ctx the parse tree
	 */
	void enterRemoteLogicCalls(MmlParser.RemoteLogicCallsContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#remoteLogicCalls}.
	 * @param ctx the parse tree
	 */
	void exitRemoteLogicCalls(MmlParser.RemoteLogicCallsContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#superLoop}.
	 * @param ctx the parse tree
	 */
	void enterSuperLoop(MmlParser.SuperLoopContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#superLoop}.
	 * @param ctx the parse tree
	 */
	void exitSuperLoop(MmlParser.SuperLoopContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#superLoopContents}.
	 * @param ctx the parse tree
	 */
	void enterSuperLoopContents(MmlParser.SuperLoopContentsContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#superLoopContents}.
	 * @param ctx the parse tree
	 */
	void exitSuperLoopContents(MmlParser.SuperLoopContentsContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#simpleLoop}.
	 * @param ctx the parse tree
	 */
	void enterSimpleLoop(MmlParser.SimpleLoopContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#simpleLoop}.
	 * @param ctx the parse tree
	 */
	void exitSimpleLoop(MmlParser.SimpleLoopContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#simpleLoopContents}.
	 * @param ctx the parse tree
	 */
	void enterSimpleLoopContents(MmlParser.SimpleLoopContentsContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#simpleLoopContents}.
	 * @param ctx the parse tree
	 */
	void exitSimpleLoopContents(MmlParser.SimpleLoopContentsContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#terminalSuperLoop}.
	 * @param ctx the parse tree
	 */
	void enterTerminalSuperLoop(MmlParser.TerminalSuperLoopContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#terminalSuperLoop}.
	 * @param ctx the parse tree
	 */
	void exitTerminalSuperLoop(MmlParser.TerminalSuperLoopContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#terminalSuperLoopContents}.
	 * @param ctx the parse tree
	 */
	void enterTerminalSuperLoopContents(MmlParser.TerminalSuperLoopContentsContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#terminalSuperLoopContents}.
	 * @param ctx the parse tree
	 */
	void exitTerminalSuperLoopContents(MmlParser.TerminalSuperLoopContentsContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#terminalSimpleLoop}.
	 * @param ctx the parse tree
	 */
	void enterTerminalSimpleLoop(MmlParser.TerminalSimpleLoopContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#terminalSimpleLoop}.
	 * @param ctx the parse tree
	 */
	void exitTerminalSimpleLoop(MmlParser.TerminalSimpleLoopContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#terminalSimpleLoopContents}.
	 * @param ctx the parse tree
	 */
	void enterTerminalSimpleLoopContents(MmlParser.TerminalSimpleLoopContentsContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#terminalSimpleLoopContents}.
	 * @param ctx the parse tree
	 */
	void exitTerminalSimpleLoopContents(MmlParser.TerminalSimpleLoopContentsContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#remoteCode}.
	 * @param ctx the parse tree
	 */
	void enterRemoteCode(MmlParser.RemoteCodeContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#remoteCode}.
	 * @param ctx the parse tree
	 */
	void exitRemoteCode(MmlParser.RemoteCodeContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#remoteCodeContents}.
	 * @param ctx the parse tree
	 */
	void enterRemoteCodeContents(MmlParser.RemoteCodeContentsContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#remoteCodeContents}.
	 * @param ctx the parse tree
	 */
	void exitRemoteCodeContents(MmlParser.RemoteCodeContentsContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#callLoop}.
	 * @param ctx the parse tree
	 */
	void enterCallLoop(MmlParser.CallLoopContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#callLoop}.
	 * @param ctx the parse tree
	 */
	void exitCallLoop(MmlParser.CallLoopContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#callRemoteCode}.
	 * @param ctx the parse tree
	 */
	void enterCallRemoteCode(MmlParser.CallRemoteCodeContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#callRemoteCode}.
	 * @param ctx the parse tree
	 */
	void exitCallRemoteCode(MmlParser.CallRemoteCodeContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#stopRemoteCode}.
	 * @param ctx the parse tree
	 */
	void enterStopRemoteCode(MmlParser.StopRemoteCodeContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#stopRemoteCode}.
	 * @param ctx the parse tree
	 */
	void exitStopRemoteCode(MmlParser.StopRemoteCodeContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#callPreviousLoop}.
	 * @param ctx the parse tree
	 */
	void enterCallPreviousLoop(MmlParser.CallPreviousLoopContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#callPreviousLoop}.
	 * @param ctx the parse tree
	 */
	void exitCallPreviousLoop(MmlParser.CallPreviousLoopContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#noloopCommand}.
	 * @param ctx the parse tree
	 */
	void enterNoloopCommand(MmlParser.NoloopCommandContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#noloopCommand}.
	 * @param ctx the parse tree
	 */
	void exitNoloopCommand(MmlParser.NoloopCommandContext ctx);
	/**
	 * Enter a parse tree produced by the {@code SampleLoad}
	 * labeled alternative in {@link MmlParser#sampleLoadCommand}.
	 * @param ctx the parse tree
	 */
	void enterSampleLoad(MmlParser.SampleLoadContext ctx);
	/**
	 * Exit a parse tree produced by the {@code SampleLoad}
	 * labeled alternative in {@link MmlParser#sampleLoadCommand}.
	 * @param ctx the parse tree
	 */
	void exitSampleLoad(MmlParser.SampleLoadContext ctx);
	/**
	 * Enter a parse tree produced by the {@code HexSampleLoad}
	 * labeled alternative in {@link MmlParser#sampleLoadCommand}.
	 * @param ctx the parse tree
	 */
	void enterHexSampleLoad(MmlParser.HexSampleLoadContext ctx);
	/**
	 * Exit a parse tree produced by the {@code HexSampleLoad}
	 * labeled alternative in {@link MmlParser#sampleLoadCommand}.
	 * @param ctx the parse tree
	 */
	void exitHexSampleLoad(MmlParser.HexSampleLoadContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#replacements}.
	 * @param ctx the parse tree
	 */
	void enterReplacements(MmlParser.ReplacementsContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#replacements}.
	 * @param ctx the parse tree
	 */
	void exitReplacements(MmlParser.ReplacementsContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#globalHexCommands}.
	 * @param ctx the parse tree
	 */
	void enterGlobalHexCommands(MmlParser.GlobalHexCommandsContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#globalHexCommands}.
	 * @param ctx the parse tree
	 */
	void exitGlobalHexCommands(MmlParser.GlobalHexCommandsContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#channelHexCommands}.
	 * @param ctx the parse tree
	 */
	void enterChannelHexCommands(MmlParser.ChannelHexCommandsContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#channelHexCommands}.
	 * @param ctx the parse tree
	 */
	void exitChannelHexCommands(MmlParser.ChannelHexCommandsContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#daInstrument}.
	 * @param ctx the parse tree
	 */
	void enterDaInstrument(MmlParser.DaInstrumentContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#daInstrument}.
	 * @param ctx the parse tree
	 */
	void exitDaInstrument(MmlParser.DaInstrumentContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#dbPan}.
	 * @param ctx the parse tree
	 */
	void enterDbPan(MmlParser.DbPanContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#dbPan}.
	 * @param ctx the parse tree
	 */
	void exitDbPan(MmlParser.DbPanContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#dcPanFade}.
	 * @param ctx the parse tree
	 */
	void enterDcPanFade(MmlParser.DcPanFadeContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#dcPanFade}.
	 * @param ctx the parse tree
	 */
	void exitDcPanFade(MmlParser.DcPanFadeContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#ddPitchBlendCommand}.
	 * @param ctx the parse tree
	 */
	void enterDdPitchBlendCommand(MmlParser.DdPitchBlendCommandContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#ddPitchBlendCommand}.
	 * @param ctx the parse tree
	 */
	void exitDdPitchBlendCommand(MmlParser.DdPitchBlendCommandContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#ddPitchBlendItems}.
	 * @param ctx the parse tree
	 */
	void enterDdPitchBlendItems(MmlParser.DdPitchBlendItemsContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#ddPitchBlendItems}.
	 * @param ctx the parse tree
	 */
	void exitDdPitchBlendItems(MmlParser.DdPitchBlendItemsContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#deVibratoStart}.
	 * @param ctx the parse tree
	 */
	void enterDeVibratoStart(MmlParser.DeVibratoStartContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#deVibratoStart}.
	 * @param ctx the parse tree
	 */
	void exitDeVibratoStart(MmlParser.DeVibratoStartContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#eaVibratoFade}.
	 * @param ctx the parse tree
	 */
	void enterEaVibratoFade(MmlParser.EaVibratoFadeContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#eaVibratoFade}.
	 * @param ctx the parse tree
	 */
	void exitEaVibratoFade(MmlParser.EaVibratoFadeContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#dfVibratoEnd}.
	 * @param ctx the parse tree
	 */
	void enterDfVibratoEnd(MmlParser.DfVibratoEndContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#dfVibratoEnd}.
	 * @param ctx the parse tree
	 */
	void exitDfVibratoEnd(MmlParser.DfVibratoEndContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#e0GlobalVolume}.
	 * @param ctx the parse tree
	 */
	void enterE0GlobalVolume(MmlParser.E0GlobalVolumeContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#e0GlobalVolume}.
	 * @param ctx the parse tree
	 */
	void exitE0GlobalVolume(MmlParser.E0GlobalVolumeContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#e1GlobalVolumeFade}.
	 * @param ctx the parse tree
	 */
	void enterE1GlobalVolumeFade(MmlParser.E1GlobalVolumeFadeContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#e1GlobalVolumeFade}.
	 * @param ctx the parse tree
	 */
	void exitE1GlobalVolumeFade(MmlParser.E1GlobalVolumeFadeContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#e2Tempo}.
	 * @param ctx the parse tree
	 */
	void enterE2Tempo(MmlParser.E2TempoContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#e2Tempo}.
	 * @param ctx the parse tree
	 */
	void exitE2Tempo(MmlParser.E2TempoContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#e3TempoFade}.
	 * @param ctx the parse tree
	 */
	void enterE3TempoFade(MmlParser.E3TempoFadeContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#e3TempoFade}.
	 * @param ctx the parse tree
	 */
	void exitE3TempoFade(MmlParser.E3TempoFadeContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#e4GlobalTranspose}.
	 * @param ctx the parse tree
	 */
	void enterE4GlobalTranspose(MmlParser.E4GlobalTransposeContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#e4GlobalTranspose}.
	 * @param ctx the parse tree
	 */
	void exitE4GlobalTranspose(MmlParser.E4GlobalTransposeContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#e5Tremolo}.
	 * @param ctx the parse tree
	 */
	void enterE5Tremolo(MmlParser.E5TremoloContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#e5Tremolo}.
	 * @param ctx the parse tree
	 */
	void exitE5Tremolo(MmlParser.E5TremoloContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#e6SubloopStart}.
	 * @param ctx the parse tree
	 */
	void enterE6SubloopStart(MmlParser.E6SubloopStartContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#e6SubloopStart}.
	 * @param ctx the parse tree
	 */
	void exitE6SubloopStart(MmlParser.E6SubloopStartContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#e6SubloopEnd}.
	 * @param ctx the parse tree
	 */
	void enterE6SubloopEnd(MmlParser.E6SubloopEndContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#e6SubloopEnd}.
	 * @param ctx the parse tree
	 */
	void exitE6SubloopEnd(MmlParser.E6SubloopEndContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#e7Volume}.
	 * @param ctx the parse tree
	 */
	void enterE7Volume(MmlParser.E7VolumeContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#e7Volume}.
	 * @param ctx the parse tree
	 */
	void exitE7Volume(MmlParser.E7VolumeContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#e8VolumeFade}.
	 * @param ctx the parse tree
	 */
	void enterE8VolumeFade(MmlParser.E8VolumeFadeContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#e8VolumeFade}.
	 * @param ctx the parse tree
	 */
	void exitE8VolumeFade(MmlParser.E8VolumeFadeContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#ebPitchEnvelopeRelease}.
	 * @param ctx the parse tree
	 */
	void enterEbPitchEnvelopeRelease(MmlParser.EbPitchEnvelopeReleaseContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#ebPitchEnvelopeRelease}.
	 * @param ctx the parse tree
	 */
	void exitEbPitchEnvelopeRelease(MmlParser.EbPitchEnvelopeReleaseContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#ecPitchEnvelopeAttack}.
	 * @param ctx the parse tree
	 */
	void enterEcPitchEnvelopeAttack(MmlParser.EcPitchEnvelopeAttackContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#ecPitchEnvelopeAttack}.
	 * @param ctx the parse tree
	 */
	void exitEcPitchEnvelopeAttack(MmlParser.EcPitchEnvelopeAttackContext ctx);
	/**
	 * Enter a parse tree produced by the {@code EDCustomGAIN}
	 * labeled alternative in {@link MmlParser#edCustomADSROrGain}.
	 * @param ctx the parse tree
	 */
	void enterEDCustomGAIN(MmlParser.EDCustomGAINContext ctx);
	/**
	 * Exit a parse tree produced by the {@code EDCustomGAIN}
	 * labeled alternative in {@link MmlParser#edCustomADSROrGain}.
	 * @param ctx the parse tree
	 */
	void exitEDCustomGAIN(MmlParser.EDCustomGAINContext ctx);
	/**
	 * Enter a parse tree produced by the {@code EDCustomASDR}
	 * labeled alternative in {@link MmlParser#edCustomADSROrGain}.
	 * @param ctx the parse tree
	 */
	void enterEDCustomASDR(MmlParser.EDCustomASDRContext ctx);
	/**
	 * Exit a parse tree produced by the {@code EDCustomASDR}
	 * labeled alternative in {@link MmlParser#edCustomADSROrGain}.
	 * @param ctx the parse tree
	 */
	void exitEDCustomASDR(MmlParser.EDCustomASDRContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#eeTuneChannel}.
	 * @param ctx the parse tree
	 */
	void enterEeTuneChannel(MmlParser.EeTuneChannelContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#eeTuneChannel}.
	 * @param ctx the parse tree
	 */
	void exitEeTuneChannel(MmlParser.EeTuneChannelContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#efEcho1}.
	 * @param ctx the parse tree
	 */
	void enterEfEcho1(MmlParser.EfEcho1Context ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#efEcho1}.
	 * @param ctx the parse tree
	 */
	void exitEfEcho1(MmlParser.EfEcho1Context ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#f0EchoOff}.
	 * @param ctx the parse tree
	 */
	void enterF0EchoOff(MmlParser.F0EchoOffContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#f0EchoOff}.
	 * @param ctx the parse tree
	 */
	void exitF0EchoOff(MmlParser.F0EchoOffContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#f1Echo2}.
	 * @param ctx the parse tree
	 */
	void enterF1Echo2(MmlParser.F1Echo2Context ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#f1Echo2}.
	 * @param ctx the parse tree
	 */
	void exitF1Echo2(MmlParser.F1Echo2Context ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#f2EchoFade}.
	 * @param ctx the parse tree
	 */
	void enterF2EchoFade(MmlParser.F2EchoFadeContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#f2EchoFade}.
	 * @param ctx the parse tree
	 */
	void exitF2EchoFade(MmlParser.F2EchoFadeContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#f3SampleLoad}.
	 * @param ctx the parse tree
	 */
	void enterF3SampleLoad(MmlParser.F3SampleLoadContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#f3SampleLoad}.
	 * @param ctx the parse tree
	 */
	void exitF3SampleLoad(MmlParser.F3SampleLoadContext ctx);
	/**
	 * Enter a parse tree produced by the {@code F4EnableYoshiDrumsChannel5}
	 * labeled alternative in {@link MmlParser#f4GlobalItems}.
	 * @param ctx the parse tree
	 */
	void enterF4EnableYoshiDrumsChannel5(MmlParser.F4EnableYoshiDrumsChannel5Context ctx);
	/**
	 * Exit a parse tree produced by the {@code F4EnableYoshiDrumsChannel5}
	 * labeled alternative in {@link MmlParser#f4GlobalItems}.
	 * @param ctx the parse tree
	 */
	void exitF4EnableYoshiDrumsChannel5(MmlParser.F4EnableYoshiDrumsChannel5Context ctx);
	/**
	 * Enter a parse tree produced by the {@code F4ToggleLegato}
	 * labeled alternative in {@link MmlParser#f4GlobalItems}.
	 * @param ctx the parse tree
	 */
	void enterF4ToggleLegato(MmlParser.F4ToggleLegatoContext ctx);
	/**
	 * Exit a parse tree produced by the {@code F4ToggleLegato}
	 * labeled alternative in {@link MmlParser#f4GlobalItems}.
	 * @param ctx the parse tree
	 */
	void exitF4ToggleLegato(MmlParser.F4ToggleLegatoContext ctx);
	/**
	 * Enter a parse tree produced by the {@code F4LightStaccato}
	 * labeled alternative in {@link MmlParser#f4GlobalItems}.
	 * @param ctx the parse tree
	 */
	void enterF4LightStaccato(MmlParser.F4LightStaccatoContext ctx);
	/**
	 * Exit a parse tree produced by the {@code F4LightStaccato}
	 * labeled alternative in {@link MmlParser#f4GlobalItems}.
	 * @param ctx the parse tree
	 */
	void exitF4LightStaccato(MmlParser.F4LightStaccatoContext ctx);
	/**
	 * Enter a parse tree produced by the {@code F4SNESSync}
	 * labeled alternative in {@link MmlParser#f4GlobalItems}.
	 * @param ctx the parse tree
	 */
	void enterF4SNESSync(MmlParser.F4SNESSyncContext ctx);
	/**
	 * Exit a parse tree produced by the {@code F4SNESSync}
	 * labeled alternative in {@link MmlParser#f4GlobalItems}.
	 * @param ctx the parse tree
	 */
	void exitF4SNESSync(MmlParser.F4SNESSyncContext ctx);
	/**
	 * Enter a parse tree produced by the {@code F4EnableYoshiDrums}
	 * labeled alternative in {@link MmlParser#f4GlobalItems}.
	 * @param ctx the parse tree
	 */
	void enterF4EnableYoshiDrums(MmlParser.F4EnableYoshiDrumsContext ctx);
	/**
	 * Exit a parse tree produced by the {@code F4EnableYoshiDrums}
	 * labeled alternative in {@link MmlParser#f4GlobalItems}.
	 * @param ctx the parse tree
	 */
	void exitF4EnableYoshiDrums(MmlParser.F4EnableYoshiDrumsContext ctx);
	/**
	 * Enter a parse tree produced by the {@code F4TempoHikeOff}
	 * labeled alternative in {@link MmlParser#f4GlobalItems}.
	 * @param ctx the parse tree
	 */
	void enterF4TempoHikeOff(MmlParser.F4TempoHikeOffContext ctx);
	/**
	 * Exit a parse tree produced by the {@code F4TempoHikeOff}
	 * labeled alternative in {@link MmlParser#f4GlobalItems}.
	 * @param ctx the parse tree
	 */
	void exitF4TempoHikeOff(MmlParser.F4TempoHikeOffContext ctx);
	/**
	 * Enter a parse tree produced by the {@code F4NSPCVelocityTable}
	 * labeled alternative in {@link MmlParser#f4GlobalItems}.
	 * @param ctx the parse tree
	 */
	void enterF4NSPCVelocityTable(MmlParser.F4NSPCVelocityTableContext ctx);
	/**
	 * Exit a parse tree produced by the {@code F4NSPCVelocityTable}
	 * labeled alternative in {@link MmlParser#f4GlobalItems}.
	 * @param ctx the parse tree
	 */
	void exitF4NSPCVelocityTable(MmlParser.F4NSPCVelocityTableContext ctx);
	/**
	 * Enter a parse tree produced by the {@code F4EchoToggle}
	 * labeled alternative in {@link MmlParser#f4ChannelItems}.
	 * @param ctx the parse tree
	 */
	void enterF4EchoToggle(MmlParser.F4EchoToggleContext ctx);
	/**
	 * Exit a parse tree produced by the {@code F4EchoToggle}
	 * labeled alternative in {@link MmlParser#f4ChannelItems}.
	 * @param ctx the parse tree
	 */
	void exitF4EchoToggle(MmlParser.F4EchoToggleContext ctx);
	/**
	 * Enter a parse tree produced by the {@code F4RestoreInstrument}
	 * labeled alternative in {@link MmlParser#f4ChannelItems}.
	 * @param ctx the parse tree
	 */
	void enterF4RestoreInstrument(MmlParser.F4RestoreInstrumentContext ctx);
	/**
	 * Exit a parse tree produced by the {@code F4RestoreInstrument}
	 * labeled alternative in {@link MmlParser#f4ChannelItems}.
	 * @param ctx the parse tree
	 */
	void exitF4RestoreInstrument(MmlParser.F4RestoreInstrumentContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#f5FIRFilter}.
	 * @param ctx the parse tree
	 */
	void enterF5FIRFilter(MmlParser.F5FIRFilterContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#f5FIRFilter}.
	 * @param ctx the parse tree
	 */
	void exitF5FIRFilter(MmlParser.F5FIRFilterContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#f6DSPWrite}.
	 * @param ctx the parse tree
	 */
	void enterF6DSPWrite(MmlParser.F6DSPWriteContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#f6DSPWrite}.
	 * @param ctx the parse tree
	 */
	void exitF6DSPWrite(MmlParser.F6DSPWriteContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#f8EnableNoise}.
	 * @param ctx the parse tree
	 */
	void enterF8EnableNoise(MmlParser.F8EnableNoiseContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#f8EnableNoise}.
	 * @param ctx the parse tree
	 */
	void exitF8EnableNoise(MmlParser.F8EnableNoiseContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#f9DataSend}.
	 * @param ctx the parse tree
	 */
	void enterF9DataSend(MmlParser.F9DataSendContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#f9DataSend}.
	 * @param ctx the parse tree
	 */
	void exitF9DataSend(MmlParser.F9DataSendContext ctx);
	/**
	 * Enter a parse tree produced by the {@code FAPitchModulation}
	 * labeled alternative in {@link MmlParser#faChannelItems}.
	 * @param ctx the parse tree
	 */
	void enterFAPitchModulation(MmlParser.FAPitchModulationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code FAPitchModulation}
	 * labeled alternative in {@link MmlParser#faChannelItems}.
	 * @param ctx the parse tree
	 */
	void exitFAPitchModulation(MmlParser.FAPitchModulationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code FACurrentChannelGain}
	 * labeled alternative in {@link MmlParser#faChannelItems}.
	 * @param ctx the parse tree
	 */
	void enterFACurrentChannelGain(MmlParser.FACurrentChannelGainContext ctx);
	/**
	 * Exit a parse tree produced by the {@code FACurrentChannelGain}
	 * labeled alternative in {@link MmlParser#faChannelItems}.
	 * @param ctx the parse tree
	 */
	void exitFACurrentChannelGain(MmlParser.FACurrentChannelGainContext ctx);
	/**
	 * Enter a parse tree produced by the {@code FASemitoneTune}
	 * labeled alternative in {@link MmlParser#faChannelItems}.
	 * @param ctx the parse tree
	 */
	void enterFASemitoneTune(MmlParser.FASemitoneTuneContext ctx);
	/**
	 * Exit a parse tree produced by the {@code FASemitoneTune}
	 * labeled alternative in {@link MmlParser#faChannelItems}.
	 * @param ctx the parse tree
	 */
	void exitFASemitoneTune(MmlParser.FASemitoneTuneContext ctx);
	/**
	 * Enter a parse tree produced by the {@code FAAmplify}
	 * labeled alternative in {@link MmlParser#faChannelItems}.
	 * @param ctx the parse tree
	 */
	void enterFAAmplify(MmlParser.FAAmplifyContext ctx);
	/**
	 * Exit a parse tree produced by the {@code FAAmplify}
	 * labeled alternative in {@link MmlParser#faChannelItems}.
	 * @param ctx the parse tree
	 */
	void exitFAAmplify(MmlParser.FAAmplifyContext ctx);
	/**
	 * Enter a parse tree produced by the {@code FAEchoBufferReserve}
	 * labeled alternative in {@link MmlParser#faGlobalItems}.
	 * @param ctx the parse tree
	 */
	void enterFAEchoBufferReserve(MmlParser.FAEchoBufferReserveContext ctx);
	/**
	 * Exit a parse tree produced by the {@code FAEchoBufferReserve}
	 * labeled alternative in {@link MmlParser#faGlobalItems}.
	 * @param ctx the parse tree
	 */
	void exitFAEchoBufferReserve(MmlParser.FAEchoBufferReserveContext ctx);
	/**
	 * Enter a parse tree produced by the {@code FAHotPatchPreset}
	 * labeled alternative in {@link MmlParser#faGlobalItems}.
	 * @param ctx the parse tree
	 */
	void enterFAHotPatchPreset(MmlParser.FAHotPatchPresetContext ctx);
	/**
	 * Exit a parse tree produced by the {@code FAHotPatchPreset}
	 * labeled alternative in {@link MmlParser#faGlobalItems}.
	 * @param ctx the parse tree
	 */
	void exitFAHotPatchPreset(MmlParser.FAHotPatchPresetContext ctx);
	/**
	 * Enter a parse tree produced by the {@code FAHotPatchToggleBits}
	 * labeled alternative in {@link MmlParser#faGlobalItems}.
	 * @param ctx the parse tree
	 */
	void enterFAHotPatchToggleBits(MmlParser.FAHotPatchToggleBitsContext ctx);
	/**
	 * Exit a parse tree produced by the {@code FAHotPatchToggleBits}
	 * labeled alternative in {@link MmlParser#faGlobalItems}.
	 * @param ctx the parse tree
	 */
	void exitFAHotPatchToggleBits(MmlParser.FAHotPatchToggleBitsContext ctx);
	/**
	 * Enter a parse tree produced by the {@code FBTrill}
	 * labeled alternative in {@link MmlParser#fbItems}.
	 * @param ctx the parse tree
	 */
	void enterFBTrill(MmlParser.FBTrillContext ctx);
	/**
	 * Exit a parse tree produced by the {@code FBTrill}
	 * labeled alternative in {@link MmlParser#fbItems}.
	 * @param ctx the parse tree
	 */
	void exitFBTrill(MmlParser.FBTrillContext ctx);
	/**
	 * Enter a parse tree produced by the {@code FBGlissando}
	 * labeled alternative in {@link MmlParser#fbItems}.
	 * @param ctx the parse tree
	 */
	void enterFBGlissando(MmlParser.FBGlissandoContext ctx);
	/**
	 * Exit a parse tree produced by the {@code FBGlissando}
	 * labeled alternative in {@link MmlParser#fbItems}.
	 * @param ctx the parse tree
	 */
	void exitFBGlissando(MmlParser.FBGlissandoContext ctx);
	/**
	 * Enter a parse tree produced by the {@code FBEnableArgeggio}
	 * labeled alternative in {@link MmlParser#fbItems}.
	 * @param ctx the parse tree
	 */
	void enterFBEnableArgeggio(MmlParser.FBEnableArgeggioContext ctx);
	/**
	 * Exit a parse tree produced by the {@code FBEnableArgeggio}
	 * labeled alternative in {@link MmlParser#fbItems}.
	 * @param ctx the parse tree
	 */
	void exitFBEnableArgeggio(MmlParser.FBEnableArgeggioContext ctx);
	/**
	 * Enter a parse tree produced by the {@code FCHexRemoteGain}
	 * labeled alternative in {@link MmlParser#fcItems}.
	 * @param ctx the parse tree
	 */
	void enterFCHexRemoteGain(MmlParser.FCHexRemoteGainContext ctx);
	/**
	 * Exit a parse tree produced by the {@code FCHexRemoteGain}
	 * labeled alternative in {@link MmlParser#fcItems}.
	 * @param ctx the parse tree
	 */
	void exitFCHexRemoteGain(MmlParser.FCHexRemoteGainContext ctx);
	/**
	 * Enter a parse tree produced by the {@code FCHexRemoteCommand}
	 * labeled alternative in {@link MmlParser#fcItems}.
	 * @param ctx the parse tree
	 */
	void enterFCHexRemoteCommand(MmlParser.FCHexRemoteCommandContext ctx);
	/**
	 * Exit a parse tree produced by the {@code FCHexRemoteCommand}
	 * labeled alternative in {@link MmlParser#fcItems}.
	 * @param ctx the parse tree
	 */
	void exitFCHexRemoteCommand(MmlParser.FCHexRemoteCommandContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#fdTremoloOff}.
	 * @param ctx the parse tree
	 */
	void enterFdTremoloOff(MmlParser.FdTremoloOffContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#fdTremoloOff}.
	 * @param ctx the parse tree
	 */
	void exitFdTremoloOff(MmlParser.FdTremoloOffContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#fePitchEnvelopeOff}.
	 * @param ctx the parse tree
	 */
	void enterFePitchEnvelopeOff(MmlParser.FePitchEnvelopeOffContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#fePitchEnvelopeOff}.
	 * @param ctx the parse tree
	 */
	void exitFePitchEnvelopeOff(MmlParser.FePitchEnvelopeOffContext ctx);
	/**
	 * Enter a parse tree produced by {@link MmlParser#hexNumber}.
	 * @param ctx the parse tree
	 */
	void enterHexNumber(MmlParser.HexNumberContext ctx);
	/**
	 * Exit a parse tree produced by {@link MmlParser#hexNumber}.
	 * @param ctx the parse tree
	 */
	void exitHexNumber(MmlParser.HexNumberContext ctx);
}