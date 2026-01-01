// Generated from d:/Projects/Visual Studio/Addmusic2/Addmusic2/Addmusic2/Mml.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast", "CheckReturnValue"})
public class MmlParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.13.1", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		POUND=1, DOLLAR=2, COMMAT=3, AMPER=4, BANG=5, PERCENT=6, STAR=7, LBRACE=8, 
		RBRACE=9, LPAREN=10, RPAREN=11, LBRACK=12, RBRACK=13, L2BRACK=14, R2BRACK=15, 
		DQUOTE=16, SQUOTE=17, SHARP=18, FLAT=19, DOT=20, TIE=21, GT=22, LT=23, 
		COMMA=24, FSLASH=25, QMARK=26, EQUAL=27, SEMICOLON=28, ReplacementText=29, 
		StringLiteral=30, PercentNumber=31, Note=32, Rest=33, Octave=34, Noise=35, 
		Tempo=36, Volume=37, Tune=38, Length=39, Quantization=40, GlobalVolume=41, 
		Pan=42, Vibrato=43, Tie=44, Question=45, Instrument=46, Pipe=47, LoopName=48, 
		RemoteCodeName=49, StopRemoteCode=50, CallRemoteCode=51, CallPreviousLoop=52, 
		LoadSample=53, AmkV1=54, Amk=55, Amm=56, Am4=57, Samples=58, Instruments=59, 
		Spc=60, SpcAuthor=61, SpcGame=62, SpcComment=63, SpcTitle=64, SpcLength=65, 
		Pad=66, Path=67, Halvetempo=68, Option=69, Louder=70, Tempoimmunity=71, 
		Dividetempo=72, Smwvtable=73, Nspcvtable=74, Noloop=75, Amk109hotpatch=76, 
		Channel=77, SampleOptimization=78, N00=79, N01=80, N02=81, N03=82, N04=83, 
		N05=84, N06=85, N07=86, N08=87, N09=88, N7F=89, N80=90, N81=91, NFE=92, 
		NDA=93, NDB=94, NDC=95, NDD=96, NDE=97, NDF=98, NE0=99, NE1=100, NE2=101, 
		NE3=102, NE4=103, NE5=104, NE6=105, NE7=106, NE8=107, NE9=108, NEA=109, 
		NEB=110, NEC=111, NED=112, NEE=113, NEF=114, NF0=115, NF1=116, NF2=117, 
		NF3=118, NF4=119, NF5=120, NF6=121, NF7=122, NF8=123, NF9=124, NFA=125, 
		NFB=126, NFC=127, NFD=128, NUMBERS=129, UNUMBERS=130, HexNumber=131, WHITESPACE=132, 
		Comment=133;
	public static final int
		RULE_song = 0, RULE_songElement = 1, RULE_specialDirective = 2, RULE_samples = 3, 
		RULE_samplesList = 4, RULE_instruments = 5, RULE_instrumentsList = 6, 
		RULE_spc = 7, RULE_spcList = 8, RULE_pad = 9, RULE_path = 10, RULE_halvetempo = 11, 
		RULE_option = 12, RULE_optionItem = 13, RULE_amk = 14, RULE_amm = 15, 
		RULE_am4 = 16, RULE_amkVersion = 17, RULE_soundChannel = 18, RULE_introEnd = 19, 
		RULE_channelContents = 20, RULE_atomics = 21, RULE_note = 22, RULE_rest = 23, 
		RULE_octave = 24, RULE_lowerOctave = 25, RULE_raiseOctave = 26, RULE_noiseNote = 27, 
		RULE_volumeCommand = 28, RULE_tuneCommand = 29, RULE_quantization = 30, 
		RULE_panCommand = 31, RULE_vibratoCommand = 32, RULE_pitchslide = 33, 
		RULE_triplet = 34, RULE_defaultLength = 35, RULE_globalVolumeCommand = 36, 
		RULE_tempoCommand = 37, RULE_instrumentCommand = 38, RULE_nakedTie = 39, 
		RULE_qmark = 40, RULE_pipe = 41, RULE_loopers = 42, RULE_logicControls = 43, 
		RULE_logicCalls = 44, RULE_remoteLogicCalls = 45, RULE_superLoop = 46, 
		RULE_superLoopContents = 47, RULE_simpleLoop = 48, RULE_simpleLoopContents = 49, 
		RULE_terminalSuperLoop = 50, RULE_terminalSuperLoopContents = 51, RULE_terminalSimpleLoop = 52, 
		RULE_terminalSimpleLoopContents = 53, RULE_remoteCode = 54, RULE_remoteCodeContents = 55, 
		RULE_callLoop = 56, RULE_callRemoteCode = 57, RULE_stopRemoteCode = 58, 
		RULE_callPreviousLoop = 59, RULE_noloopCommand = 60, RULE_sampleLoadCommand = 61, 
		RULE_replacements = 62, RULE_globalHexCommands = 63, RULE_channelHexCommands = 64, 
		RULE_daInstrument = 65, RULE_dbPan = 66, RULE_dcPanFade = 67, RULE_ddPitchBlendCommand = 68, 
		RULE_ddPitchBlendItems = 69, RULE_deVibratoStart = 70, RULE_eaVibratoFade = 71, 
		RULE_dfVibratoEnd = 72, RULE_e0GlobalVolume = 73, RULE_e1GlobalVolumeFade = 74, 
		RULE_e2Tempo = 75, RULE_e3TempoFade = 76, RULE_e4GlobalTranspose = 77, 
		RULE_e5Tremolo = 78, RULE_e6SubloopStart = 79, RULE_e6SubloopEnd = 80, 
		RULE_e7Volume = 81, RULE_e8VolumeFade = 82, RULE_ebPitchEnvelopeRelease = 83, 
		RULE_ecPitchEnvelopeAttack = 84, RULE_edCustomADSROrGain = 85, RULE_eeTuneChannel = 86, 
		RULE_efEcho1 = 87, RULE_f0EchoOff = 88, RULE_f1Echo2 = 89, RULE_f2EchoFade = 90, 
		RULE_f3SampleLoad = 91, RULE_f4GlobalItems = 92, RULE_f4ChannelItems = 93, 
		RULE_f5FIRFilter = 94, RULE_f6DSPWrite = 95, RULE_f8EnableNoise = 96, 
		RULE_f9DataSend = 97, RULE_faChannelItems = 98, RULE_faGlobalItems = 99, 
		RULE_fbItems = 100, RULE_fcItems = 101, RULE_fdTremoloOff = 102, RULE_fePitchEnvelopeOff = 103, 
		RULE_hexNumber = 104;
	private static String[] makeRuleNames() {
		return new String[] {
			"song", "songElement", "specialDirective", "samples", "samplesList", 
			"instruments", "instrumentsList", "spc", "spcList", "pad", "path", "halvetempo", 
			"option", "optionItem", "amk", "amm", "am4", "amkVersion", "soundChannel", 
			"introEnd", "channelContents", "atomics", "note", "rest", "octave", "lowerOctave", 
			"raiseOctave", "noiseNote", "volumeCommand", "tuneCommand", "quantization", 
			"panCommand", "vibratoCommand", "pitchslide", "triplet", "defaultLength", 
			"globalVolumeCommand", "tempoCommand", "instrumentCommand", "nakedTie", 
			"qmark", "pipe", "loopers", "logicControls", "logicCalls", "remoteLogicCalls", 
			"superLoop", "superLoopContents", "simpleLoop", "simpleLoopContents", 
			"terminalSuperLoop", "terminalSuperLoopContents", "terminalSimpleLoop", 
			"terminalSimpleLoopContents", "remoteCode", "remoteCodeContents", "callLoop", 
			"callRemoteCode", "stopRemoteCode", "callPreviousLoop", "noloopCommand", 
			"sampleLoadCommand", "replacements", "globalHexCommands", "channelHexCommands", 
			"daInstrument", "dbPan", "dcPanFade", "ddPitchBlendCommand", "ddPitchBlendItems", 
			"deVibratoStart", "eaVibratoFade", "dfVibratoEnd", "e0GlobalVolume", 
			"e1GlobalVolumeFade", "e2Tempo", "e3TempoFade", "e4GlobalTranspose", 
			"e5Tremolo", "e6SubloopStart", "e6SubloopEnd", "e7Volume", "e8VolumeFade", 
			"ebPitchEnvelopeRelease", "ecPitchEnvelopeAttack", "edCustomADSROrGain", 
			"eeTuneChannel", "efEcho1", "f0EchoOff", "f1Echo2", "f2EchoFade", "f3SampleLoad", 
			"f4GlobalItems", "f4ChannelItems", "f5FIRFilter", "f6DSPWrite", "f8EnableNoise", 
			"f9DataSend", "faChannelItems", "faGlobalItems", "fbItems", "fcItems", 
			"fdTremoloOff", "fePitchEnvelopeOff", "hexNumber"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, "'#'", "'$'", "'@'", "'&'", "'!'", "'%'", "'*'", "'{'", "'}'", 
			"'('", "')'", "'['", "']'", "'[['", "']]'", "'\"'", "'''", "'+'", "'-'", 
			"'.'", "'^'", "'>'", "'<'", "','", "'/'", "'?'", "'='", "';'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "POUND", "DOLLAR", "COMMAT", "AMPER", "BANG", "PERCENT", "STAR", 
			"LBRACE", "RBRACE", "LPAREN", "RPAREN", "LBRACK", "RBRACK", "L2BRACK", 
			"R2BRACK", "DQUOTE", "SQUOTE", "SHARP", "FLAT", "DOT", "TIE", "GT", "LT", 
			"COMMA", "FSLASH", "QMARK", "EQUAL", "SEMICOLON", "ReplacementText", 
			"StringLiteral", "PercentNumber", "Note", "Rest", "Octave", "Noise", 
			"Tempo", "Volume", "Tune", "Length", "Quantization", "GlobalVolume", 
			"Pan", "Vibrato", "Tie", "Question", "Instrument", "Pipe", "LoopName", 
			"RemoteCodeName", "StopRemoteCode", "CallRemoteCode", "CallPreviousLoop", 
			"LoadSample", "AmkV1", "Amk", "Amm", "Am4", "Samples", "Instruments", 
			"Spc", "SpcAuthor", "SpcGame", "SpcComment", "SpcTitle", "SpcLength", 
			"Pad", "Path", "Halvetempo", "Option", "Louder", "Tempoimmunity", "Dividetempo", 
			"Smwvtable", "Nspcvtable", "Noloop", "Amk109hotpatch", "Channel", "SampleOptimization", 
			"N00", "N01", "N02", "N03", "N04", "N05", "N06", "N07", "N08", "N09", 
			"N7F", "N80", "N81", "NFE", "NDA", "NDB", "NDC", "NDD", "NDE", "NDF", 
			"NE0", "NE1", "NE2", "NE3", "NE4", "NE5", "NE6", "NE7", "NE8", "NE9", 
			"NEA", "NEB", "NEC", "NED", "NEE", "NEF", "NF0", "NF1", "NF2", "NF3", 
			"NF4", "NF5", "NF6", "NF7", "NF8", "NF9", "NFA", "NFB", "NFC", "NFD", 
			"NUMBERS", "UNUMBERS", "HexNumber", "WHITESPACE", "Comment"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}

	@Override
	public String getGrammarFileName() { return "Mml.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public MmlParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SongContext extends ParserRuleContext {
		public List<SongElementContext> songElement() {
			return getRuleContexts(SongElementContext.class);
		}
		public SongElementContext songElement(int i) {
			return getRuleContext(SongElementContext.class,i);
		}
		public TerminalNode EOF() { return getToken(MmlParser.EOF, 0); }
		public SongContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_song; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterSong(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitSong(this);
		}
	}

	public final SongContext song() throws RecognitionException {
		SongContext _localctx = new SongContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_song);
		int _la;
		try {
			setState(216);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case FSLASH:
			case QMARK:
			case ReplacementText:
			case StringLiteral:
			case Tempo:
			case Length:
			case GlobalVolume:
			case Question:
			case RemoteCodeName:
			case Amk:
			case Amm:
			case Am4:
			case Samples:
			case Instruments:
			case Spc:
			case Pad:
			case Path:
			case Halvetempo:
			case Option:
			case Channel:
			case N00:
			case N01:
			case N02:
			case N03:
			case N04:
			case N05:
			case N06:
			case N07:
			case N08:
			case N09:
			case N7F:
			case N80:
			case N81:
			case NFE:
			case NDA:
			case NDB:
			case NDC:
			case NDD:
			case NDE:
			case NDF:
			case NE0:
			case NE1:
			case NE2:
			case NE3:
			case NE4:
			case NE5:
			case NE6:
			case NE7:
			case NE8:
			case NE9:
			case NEA:
			case NEB:
			case NEC:
			case NED:
			case NEE:
			case NEF:
			case NF0:
			case NF1:
			case NF2:
			case NF3:
			case NF4:
			case NF5:
			case NF6:
			case NF7:
			case NF8:
			case NF9:
			case NFA:
			case NFB:
			case NFC:
			case NFD:
			case HexNumber:
				enterOuterAlt(_localctx, 1);
				{
				setState(211); 
				_errHandler.sync(this);
				_la = _input.LA(1);
				do {
					{
					{
					setState(210);
					songElement();
					}
					}
					setState(213); 
					_errHandler.sync(this);
					_la = _input.LA(1);
				} while ( ((((_la - 25)) & ~0x3f) == 0 && ((1L << (_la - 25)) & -13477745869633485L) != 0) || ((((_la - 89)) & ~0x3f) == 0 && ((1L << (_la - 89)) & 5497558138879L) != 0) );
				}
				break;
			case EOF:
				enterOuterAlt(_localctx, 2);
				{
				setState(215);
				match(EOF);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SongElementContext extends ParserRuleContext {
		public SpecialDirectiveContext specialDirective() {
			return getRuleContext(SpecialDirectiveContext.class,0);
		}
		public SoundChannelContext soundChannel() {
			return getRuleContext(SoundChannelContext.class,0);
		}
		public RemoteCodeContext remoteCode() {
			return getRuleContext(RemoteCodeContext.class,0);
		}
		public DefaultLengthContext defaultLength() {
			return getRuleContext(DefaultLengthContext.class,0);
		}
		public GlobalVolumeCommandContext globalVolumeCommand() {
			return getRuleContext(GlobalVolumeCommandContext.class,0);
		}
		public TempoCommandContext tempoCommand() {
			return getRuleContext(TempoCommandContext.class,0);
		}
		public ReplacementsContext replacements() {
			return getRuleContext(ReplacementsContext.class,0);
		}
		public NoloopCommandContext noloopCommand() {
			return getRuleContext(NoloopCommandContext.class,0);
		}
		public TerminalNode StringLiteral() { return getToken(MmlParser.StringLiteral, 0); }
		public GlobalHexCommandsContext globalHexCommands() {
			return getRuleContext(GlobalHexCommandsContext.class,0);
		}
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public IntroEndContext introEnd() {
			return getRuleContext(IntroEndContext.class,0);
		}
		public QmarkContext qmark() {
			return getRuleContext(QmarkContext.class,0);
		}
		public SongElementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_songElement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterSongElement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitSongElement(this);
		}
	}

	public final SongElementContext songElement() throws RecognitionException {
		SongElementContext _localctx = new SongElementContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_songElement);
		try {
			setState(232);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,2,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(218);
				specialDirective();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(219);
				soundChannel();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(220);
				remoteCode();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(221);
				defaultLength();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(222);
				globalVolumeCommand();
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(223);
				tempoCommand();
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(224);
				replacements();
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(225);
				noloopCommand();
				}
				break;
			case 9:
				enterOuterAlt(_localctx, 9);
				{
				setState(226);
				match(StringLiteral);
				}
				break;
			case 10:
				enterOuterAlt(_localctx, 10);
				{
				setState(227);
				globalHexCommands();
				}
				break;
			case 11:
				enterOuterAlt(_localctx, 11);
				{
				setState(228);
				hexNumber();
				}
				break;
			case 12:
				enterOuterAlt(_localctx, 12);
				{
				setState(229);
				introEnd();
				}
				break;
			case 13:
				enterOuterAlt(_localctx, 13);
				{
				setState(230);
				remoteCode();
				}
				break;
			case 14:
				enterOuterAlt(_localctx, 14);
				{
				setState(231);
				qmark();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SpecialDirectiveContext extends ParserRuleContext {
		public AmkContext amk() {
			return getRuleContext(AmkContext.class,0);
		}
		public SpcContext spc() {
			return getRuleContext(SpcContext.class,0);
		}
		public SamplesContext samples() {
			return getRuleContext(SamplesContext.class,0);
		}
		public InstrumentsContext instruments() {
			return getRuleContext(InstrumentsContext.class,0);
		}
		public PathContext path() {
			return getRuleContext(PathContext.class,0);
		}
		public PadContext pad() {
			return getRuleContext(PadContext.class,0);
		}
		public HalvetempoContext halvetempo() {
			return getRuleContext(HalvetempoContext.class,0);
		}
		public OptionContext option() {
			return getRuleContext(OptionContext.class,0);
		}
		public SpecialDirectiveContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_specialDirective; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterSpecialDirective(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitSpecialDirective(this);
		}
	}

	public final SpecialDirectiveContext specialDirective() throws RecognitionException {
		SpecialDirectiveContext _localctx = new SpecialDirectiveContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_specialDirective);
		try {
			setState(242);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Amk:
			case Amm:
			case Am4:
				enterOuterAlt(_localctx, 1);
				{
				setState(234);
				amk();
				}
				break;
			case Spc:
				enterOuterAlt(_localctx, 2);
				{
				setState(235);
				spc();
				}
				break;
			case Samples:
				enterOuterAlt(_localctx, 3);
				{
				setState(236);
				samples();
				}
				break;
			case Instruments:
				enterOuterAlt(_localctx, 4);
				{
				setState(237);
				instruments();
				}
				break;
			case Path:
				enterOuterAlt(_localctx, 5);
				{
				setState(238);
				path();
				}
				break;
			case Pad:
				enterOuterAlt(_localctx, 6);
				{
				setState(239);
				pad();
				}
				break;
			case Halvetempo:
				enterOuterAlt(_localctx, 7);
				{
				setState(240);
				halvetempo();
				}
				break;
			case Option:
				enterOuterAlt(_localctx, 8);
				{
				setState(241);
				option();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SamplesContext extends ParserRuleContext {
		public TerminalNode Samples() { return getToken(MmlParser.Samples, 0); }
		public TerminalNode LBRACE() { return getToken(MmlParser.LBRACE, 0); }
		public SamplesListContext samplesList() {
			return getRuleContext(SamplesListContext.class,0);
		}
		public TerminalNode RBRACE() { return getToken(MmlParser.RBRACE, 0); }
		public SamplesContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_samples; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterSamples(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitSamples(this);
		}
	}

	public final SamplesContext samples() throws RecognitionException {
		SamplesContext _localctx = new SamplesContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_samples);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(244);
			match(Samples);
			setState(245);
			match(LBRACE);
			setState(246);
			samplesList();
			setState(247);
			match(RBRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SamplesListContext extends ParserRuleContext {
		public List<TerminalNode> SampleOptimization() { return getTokens(MmlParser.SampleOptimization); }
		public TerminalNode SampleOptimization(int i) {
			return getToken(MmlParser.SampleOptimization, i);
		}
		public List<TerminalNode> StringLiteral() { return getTokens(MmlParser.StringLiteral); }
		public TerminalNode StringLiteral(int i) {
			return getToken(MmlParser.StringLiteral, i);
		}
		public SamplesListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_samplesList; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterSamplesList(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitSamplesList(this);
		}
	}

	public final SamplesListContext samplesList() throws RecognitionException {
		SamplesListContext _localctx = new SamplesListContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_samplesList);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(252);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==SampleOptimization) {
				{
				{
				setState(249);
				match(SampleOptimization);
				}
				}
				setState(254);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(258);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==StringLiteral) {
				{
				{
				setState(255);
				match(StringLiteral);
				}
				}
				setState(260);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class InstrumentsContext extends ParserRuleContext {
		public TerminalNode Instruments() { return getToken(MmlParser.Instruments, 0); }
		public TerminalNode LBRACE() { return getToken(MmlParser.LBRACE, 0); }
		public TerminalNode RBRACE() { return getToken(MmlParser.RBRACE, 0); }
		public List<InstrumentsListContext> instrumentsList() {
			return getRuleContexts(InstrumentsListContext.class);
		}
		public InstrumentsListContext instrumentsList(int i) {
			return getRuleContext(InstrumentsListContext.class,i);
		}
		public InstrumentsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_instruments; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterInstruments(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitInstruments(this);
		}
	}

	public final InstrumentsContext instruments() throws RecognitionException {
		InstrumentsContext _localctx = new InstrumentsContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_instruments);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(261);
			match(Instruments);
			setState(262);
			match(LBRACE);
			setState(266);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (((((_la - 30)) & ~0x3f) == 0 && ((1L << (_la - 30)) & -9223372036854710239L) != 0)) {
				{
				{
				setState(263);
				instrumentsList();
				}
				}
				setState(268);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(269);
			match(RBRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class InstrumentsListContext extends ParserRuleContext {
		public InstrumentsListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_instrumentsList; }
	 
		public InstrumentsListContext() { }
		public void copyFrom(InstrumentsListContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class InstrumentListItemContext extends InstrumentsListContext {
		public InstrumentCommandContext instrumentCommand() {
			return getRuleContext(InstrumentCommandContext.class,0);
		}
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public InstrumentListItemContext(InstrumentsListContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterInstrumentListItem(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitInstrumentListItem(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class NamedInstrumentListItemContext extends InstrumentsListContext {
		public TerminalNode StringLiteral() { return getToken(MmlParser.StringLiteral, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public NamedInstrumentListItemContext(InstrumentsListContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterNamedInstrumentListItem(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitNamedInstrumentListItem(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class NoiseInstrumentListItemContext extends InstrumentsListContext {
		public NoiseNoteContext noiseNote() {
			return getRuleContext(NoiseNoteContext.class,0);
		}
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public NoiseInstrumentListItemContext(InstrumentsListContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterNoiseInstrumentListItem(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitNoiseInstrumentListItem(this);
		}
	}

	public final InstrumentsListContext instrumentsList() throws RecognitionException {
		InstrumentsListContext _localctx = new InstrumentsListContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_instrumentsList);
		try {
			int _alt;
			setState(289);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case StringLiteral:
				_localctx = new NamedInstrumentListItemContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(271);
				match(StringLiteral);
				setState(273); 
				_errHandler.sync(this);
				_alt = 1;
				do {
					switch (_alt) {
					case 1:
						{
						{
						setState(272);
						hexNumber();
						}
						}
						break;
					default:
						throw new NoViableAltException(this);
					}
					setState(275); 
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,7,_ctx);
				} while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER );
				}
				break;
			case Instrument:
			case NDA:
				_localctx = new InstrumentListItemContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(277);
				instrumentCommand();
				setState(279); 
				_errHandler.sync(this);
				_alt = 1;
				do {
					switch (_alt) {
					case 1:
						{
						{
						setState(278);
						hexNumber();
						}
						}
						break;
					default:
						throw new NoViableAltException(this);
					}
					setState(281); 
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,8,_ctx);
				} while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER );
				}
				break;
			case Noise:
				_localctx = new NoiseInstrumentListItemContext(_localctx);
				enterOuterAlt(_localctx, 3);
				{
				setState(283);
				noiseNote();
				setState(285); 
				_errHandler.sync(this);
				_alt = 1;
				do {
					switch (_alt) {
					case 1:
						{
						{
						setState(284);
						hexNumber();
						}
						}
						break;
					default:
						throw new NoViableAltException(this);
					}
					setState(287); 
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,9,_ctx);
				} while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER );
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SpcContext extends ParserRuleContext {
		public TerminalNode Spc() { return getToken(MmlParser.Spc, 0); }
		public TerminalNode LBRACE() { return getToken(MmlParser.LBRACE, 0); }
		public TerminalNode RBRACE() { return getToken(MmlParser.RBRACE, 0); }
		public List<SpcListContext> spcList() {
			return getRuleContexts(SpcListContext.class);
		}
		public SpcListContext spcList(int i) {
			return getRuleContext(SpcListContext.class,i);
		}
		public SpcContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_spc; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterSpc(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitSpc(this);
		}
	}

	public final SpcContext spc() throws RecognitionException {
		SpcContext _localctx = new SpcContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_spc);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(291);
			match(Spc);
			setState(292);
			match(LBRACE);
			setState(296);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (((((_la - 61)) & ~0x3f) == 0 && ((1L << (_la - 61)) & 31L) != 0)) {
				{
				{
				setState(293);
				spcList();
				}
				}
				setState(298);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(299);
			match(RBRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SpcListContext extends ParserRuleContext {
		public TerminalNode SpcAuthor() { return getToken(MmlParser.SpcAuthor, 0); }
		public TerminalNode StringLiteral() { return getToken(MmlParser.StringLiteral, 0); }
		public TerminalNode SpcGame() { return getToken(MmlParser.SpcGame, 0); }
		public TerminalNode SpcComment() { return getToken(MmlParser.SpcComment, 0); }
		public TerminalNode SpcTitle() { return getToken(MmlParser.SpcTitle, 0); }
		public TerminalNode SpcLength() { return getToken(MmlParser.SpcLength, 0); }
		public SpcListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_spcList; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterSpcList(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitSpcList(this);
		}
	}

	public final SpcListContext spcList() throws RecognitionException {
		SpcListContext _localctx = new SpcListContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_spcList);
		try {
			setState(311);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case SpcAuthor:
				enterOuterAlt(_localctx, 1);
				{
				setState(301);
				match(SpcAuthor);
				setState(302);
				match(StringLiteral);
				}
				break;
			case SpcGame:
				enterOuterAlt(_localctx, 2);
				{
				setState(303);
				match(SpcGame);
				setState(304);
				match(StringLiteral);
				}
				break;
			case SpcComment:
				enterOuterAlt(_localctx, 3);
				{
				setState(305);
				match(SpcComment);
				setState(306);
				match(StringLiteral);
				}
				break;
			case SpcTitle:
				enterOuterAlt(_localctx, 4);
				{
				setState(307);
				match(SpcTitle);
				setState(308);
				match(StringLiteral);
				}
				break;
			case SpcLength:
				enterOuterAlt(_localctx, 5);
				{
				setState(309);
				match(SpcLength);
				setState(310);
				match(StringLiteral);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class PadContext extends ParserRuleContext {
		public TerminalNode Pad() { return getToken(MmlParser.Pad, 0); }
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public PadContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_pad; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterPad(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitPad(this);
		}
	}

	public final PadContext pad() throws RecognitionException {
		PadContext _localctx = new PadContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_pad);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(313);
			match(Pad);
			setState(314);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class PathContext extends ParserRuleContext {
		public TerminalNode Path() { return getToken(MmlParser.Path, 0); }
		public TerminalNode StringLiteral() { return getToken(MmlParser.StringLiteral, 0); }
		public PathContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_path; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterPath(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitPath(this);
		}
	}

	public final PathContext path() throws RecognitionException {
		PathContext _localctx = new PathContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_path);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(316);
			match(Path);
			setState(317);
			match(StringLiteral);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class HalvetempoContext extends ParserRuleContext {
		public TerminalNode Halvetempo() { return getToken(MmlParser.Halvetempo, 0); }
		public HalvetempoContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_halvetempo; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterHalvetempo(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitHalvetempo(this);
		}
	}

	public final HalvetempoContext halvetempo() throws RecognitionException {
		HalvetempoContext _localctx = new HalvetempoContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_halvetempo);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(319);
			match(Halvetempo);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class OptionContext extends ParserRuleContext {
		public OptionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_option; }
	 
		public OptionContext() { }
		public void copyFrom(OptionContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class OptionGroupContext extends OptionContext {
		public TerminalNode Option() { return getToken(MmlParser.Option, 0); }
		public TerminalNode LBRACE() { return getToken(MmlParser.LBRACE, 0); }
		public TerminalNode RBRACE() { return getToken(MmlParser.RBRACE, 0); }
		public List<TerminalNode> POUND() { return getTokens(MmlParser.POUND); }
		public TerminalNode POUND(int i) {
			return getToken(MmlParser.POUND, i);
		}
		public List<OptionItemContext> optionItem() {
			return getRuleContexts(OptionItemContext.class);
		}
		public OptionItemContext optionItem(int i) {
			return getRuleContext(OptionItemContext.class,i);
		}
		public OptionGroupContext(OptionContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterOptionGroup(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitOptionGroup(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class SingleOptionContext extends OptionContext {
		public TerminalNode Option() { return getToken(MmlParser.Option, 0); }
		public OptionItemContext optionItem() {
			return getRuleContext(OptionItemContext.class,0);
		}
		public SingleOptionContext(OptionContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterSingleOption(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitSingleOption(this);
		}
	}

	public final OptionContext option() throws RecognitionException {
		OptionContext _localctx = new OptionContext(_ctx, getState());
		enterRule(_localctx, 24, RULE_option);
		int _la;
		try {
			setState(333);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,14,_ctx) ) {
			case 1:
				_localctx = new OptionGroupContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(321);
				match(Option);
				setState(322);
				match(LBRACE);
				setState(327);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==POUND) {
					{
					{
					setState(323);
					match(POUND);
					setState(324);
					optionItem();
					}
					}
					setState(329);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(330);
				match(RBRACE);
				}
				break;
			case 2:
				_localctx = new SingleOptionContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(331);
				match(Option);
				setState(332);
				optionItem();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class OptionItemContext extends ParserRuleContext {
		public TerminalNode Tempoimmunity() { return getToken(MmlParser.Tempoimmunity, 0); }
		public TerminalNode Dividetempo() { return getToken(MmlParser.Dividetempo, 0); }
		public TerminalNode NUMBERS() { return getToken(MmlParser.NUMBERS, 0); }
		public TerminalNode Smwvtable() { return getToken(MmlParser.Smwvtable, 0); }
		public TerminalNode Nspcvtable() { return getToken(MmlParser.Nspcvtable, 0); }
		public TerminalNode Noloop() { return getToken(MmlParser.Noloop, 0); }
		public TerminalNode Amk109hotpatch() { return getToken(MmlParser.Amk109hotpatch, 0); }
		public TerminalNode StringLiteral() { return getToken(MmlParser.StringLiteral, 0); }
		public OptionItemContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_optionItem; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterOptionItem(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitOptionItem(this);
		}
	}

	public final OptionItemContext optionItem() throws RecognitionException {
		OptionItemContext _localctx = new OptionItemContext(_ctx, getState());
		enterRule(_localctx, 26, RULE_optionItem);
		try {
			setState(345);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,15,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(335);
				match(Tempoimmunity);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(336);
				match(Dividetempo);
				setState(337);
				match(NUMBERS);
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(338);
				match(Smwvtable);
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(339);
				match(Nspcvtable);
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(340);
				match(Noloop);
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(341);
				match(Amk109hotpatch);
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(342);
				match(StringLiteral);
				setState(343);
				match(NUMBERS);
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(344);
				match(StringLiteral);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class AmkContext extends ParserRuleContext {
		public AmkContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_amk; }
	 
		public AmkContext() { }
		public void copyFrom(AmkContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class AmmVersionContext extends AmkContext {
		public AmmContext amm() {
			return getRuleContext(AmmContext.class,0);
		}
		public AmmVersionContext(AmkContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterAmmVersion(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitAmmVersion(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class Am4VersionContext extends AmkContext {
		public Am4Context am4() {
			return getRuleContext(Am4Context.class,0);
		}
		public Am4VersionContext(AmkContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterAm4Version(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitAm4Version(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class GeneralAmkVersionContext extends AmkContext {
		public TerminalNode Amk() { return getToken(MmlParser.Amk, 0); }
		public AmkVersionContext amkVersion() {
			return getRuleContext(AmkVersionContext.class,0);
		}
		public GeneralAmkVersionContext(AmkContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterGeneralAmkVersion(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitGeneralAmkVersion(this);
		}
	}

	public final AmkContext amk() throws RecognitionException {
		AmkContext _localctx = new AmkContext(_ctx, getState());
		enterRule(_localctx, 28, RULE_amk);
		try {
			setState(351);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Amk:
				_localctx = new GeneralAmkVersionContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(347);
				match(Amk);
				setState(348);
				amkVersion();
				}
				break;
			case Amm:
				_localctx = new AmmVersionContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(349);
				amm();
				}
				break;
			case Am4:
				_localctx = new Am4VersionContext(_localctx);
				enterOuterAlt(_localctx, 3);
				{
				setState(350);
				am4();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class AmmContext extends ParserRuleContext {
		public TerminalNode Amm() { return getToken(MmlParser.Amm, 0); }
		public AmmContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_amm; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterAmm(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitAmm(this);
		}
	}

	public final AmmContext amm() throws RecognitionException {
		AmmContext _localctx = new AmmContext(_ctx, getState());
		enterRule(_localctx, 30, RULE_amm);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(353);
			match(Amm);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Am4Context extends ParserRuleContext {
		public TerminalNode Am4() { return getToken(MmlParser.Am4, 0); }
		public Am4Context(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_am4; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterAm4(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitAm4(this);
		}
	}

	public final Am4Context am4() throws RecognitionException {
		Am4Context _localctx = new Am4Context(_ctx, getState());
		enterRule(_localctx, 32, RULE_am4);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(355);
			match(Am4);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class AmkVersionContext extends ParserRuleContext {
		public TerminalNode AmkV1() { return getToken(MmlParser.AmkV1, 0); }
		public TerminalNode NUMBERS() { return getToken(MmlParser.NUMBERS, 0); }
		public AmkVersionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_amkVersion; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterAmkVersion(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitAmkVersion(this);
		}
	}

	public final AmkVersionContext amkVersion() throws RecognitionException {
		AmkVersionContext _localctx = new AmkVersionContext(_ctx, getState());
		enterRule(_localctx, 34, RULE_amkVersion);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(357);
			_la = _input.LA(1);
			if ( !(_la==AmkV1 || _la==NUMBERS) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SoundChannelContext extends ParserRuleContext {
		public TerminalNode Channel() { return getToken(MmlParser.Channel, 0); }
		public List<ChannelContentsContext> channelContents() {
			return getRuleContexts(ChannelContentsContext.class);
		}
		public ChannelContentsContext channelContents(int i) {
			return getRuleContext(ChannelContentsContext.class,i);
		}
		public SoundChannelContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_soundChannel; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterSoundChannel(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitSoundChannel(this);
		}
	}

	public final SoundChannelContext soundChannel() throws RecognitionException {
		SoundChannelContext _localctx = new SoundChannelContext(_ctx, getState());
		enterRule(_localctx, 36, RULE_soundChannel);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(359);
			match(Channel);
			setState(363);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,17,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(360);
					channelContents();
					}
					} 
				}
				setState(365);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,17,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class IntroEndContext extends ParserRuleContext {
		public TerminalNode FSLASH() { return getToken(MmlParser.FSLASH, 0); }
		public IntroEndContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_introEnd; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterIntroEnd(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitIntroEnd(this);
		}
	}

	public final IntroEndContext introEnd() throws RecognitionException {
		IntroEndContext _localctx = new IntroEndContext(_ctx, getState());
		enterRule(_localctx, 38, RULE_introEnd);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(366);
			match(FSLASH);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ChannelContentsContext extends ParserRuleContext {
		public AtomicsContext atomics() {
			return getRuleContext(AtomicsContext.class,0);
		}
		public LoopersContext loopers() {
			return getRuleContext(LoopersContext.class,0);
		}
		public SampleLoadCommandContext sampleLoadCommand() {
			return getRuleContext(SampleLoadCommandContext.class,0);
		}
		public ChannelHexCommandsContext channelHexCommands() {
			return getRuleContext(ChannelHexCommandsContext.class,0);
		}
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public ChannelContentsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_channelContents; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterChannelContents(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitChannelContents(this);
		}
	}

	public final ChannelContentsContext channelContents() throws RecognitionException {
		ChannelContentsContext _localctx = new ChannelContentsContext(_ctx, getState());
		enterRule(_localctx, 40, RULE_channelContents);
		try {
			setState(373);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,18,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(368);
				atomics();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(369);
				loopers();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(370);
				sampleLoadCommand();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(371);
				channelHexCommands();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(372);
				hexNumber();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class AtomicsContext extends ParserRuleContext {
		public PitchslideContext pitchslide() {
			return getRuleContext(PitchslideContext.class,0);
		}
		public NoteContext note() {
			return getRuleContext(NoteContext.class,0);
		}
		public RestContext rest() {
			return getRuleContext(RestContext.class,0);
		}
		public OctaveContext octave() {
			return getRuleContext(OctaveContext.class,0);
		}
		public LowerOctaveContext lowerOctave() {
			return getRuleContext(LowerOctaveContext.class,0);
		}
		public RaiseOctaveContext raiseOctave() {
			return getRuleContext(RaiseOctaveContext.class,0);
		}
		public NoiseNoteContext noiseNote() {
			return getRuleContext(NoiseNoteContext.class,0);
		}
		public TripletContext triplet() {
			return getRuleContext(TripletContext.class,0);
		}
		public VolumeCommandContext volumeCommand() {
			return getRuleContext(VolumeCommandContext.class,0);
		}
		public TuneCommandContext tuneCommand() {
			return getRuleContext(TuneCommandContext.class,0);
		}
		public InstrumentCommandContext instrumentCommand() {
			return getRuleContext(InstrumentCommandContext.class,0);
		}
		public QuantizationContext quantization() {
			return getRuleContext(QuantizationContext.class,0);
		}
		public PanCommandContext panCommand() {
			return getRuleContext(PanCommandContext.class,0);
		}
		public VibratoCommandContext vibratoCommand() {
			return getRuleContext(VibratoCommandContext.class,0);
		}
		public TempoCommandContext tempoCommand() {
			return getRuleContext(TempoCommandContext.class,0);
		}
		public IntroEndContext introEnd() {
			return getRuleContext(IntroEndContext.class,0);
		}
		public NakedTieContext nakedTie() {
			return getRuleContext(NakedTieContext.class,0);
		}
		public QmarkContext qmark() {
			return getRuleContext(QmarkContext.class,0);
		}
		public PipeContext pipe() {
			return getRuleContext(PipeContext.class,0);
		}
		public AtomicsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_atomics; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterAtomics(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitAtomics(this);
		}
	}

	public final AtomicsContext atomics() throws RecognitionException {
		AtomicsContext _localctx = new AtomicsContext(_ctx, getState());
		enterRule(_localctx, 42, RULE_atomics);
		try {
			setState(394);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,19,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(375);
				pitchslide();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(376);
				note();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(377);
				rest();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(378);
				octave();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(379);
				lowerOctave();
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(380);
				raiseOctave();
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(381);
				noiseNote();
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(382);
				triplet();
				}
				break;
			case 9:
				enterOuterAlt(_localctx, 9);
				{
				setState(383);
				volumeCommand();
				}
				break;
			case 10:
				enterOuterAlt(_localctx, 10);
				{
				setState(384);
				tuneCommand();
				}
				break;
			case 11:
				enterOuterAlt(_localctx, 11);
				{
				setState(385);
				instrumentCommand();
				}
				break;
			case 12:
				enterOuterAlt(_localctx, 12);
				{
				setState(386);
				quantization();
				}
				break;
			case 13:
				enterOuterAlt(_localctx, 13);
				{
				setState(387);
				panCommand();
				}
				break;
			case 14:
				enterOuterAlt(_localctx, 14);
				{
				setState(388);
				vibratoCommand();
				}
				break;
			case 15:
				enterOuterAlt(_localctx, 15);
				{
				setState(389);
				tempoCommand();
				}
				break;
			case 16:
				enterOuterAlt(_localctx, 16);
				{
				setState(390);
				introEnd();
				}
				break;
			case 17:
				enterOuterAlt(_localctx, 17);
				{
				setState(391);
				nakedTie();
				}
				break;
			case 18:
				enterOuterAlt(_localctx, 18);
				{
				setState(392);
				qmark();
				}
				break;
			case 19:
				enterOuterAlt(_localctx, 19);
				{
				setState(393);
				pipe();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class NoteContext extends ParserRuleContext {
		public TerminalNode Note() { return getToken(MmlParser.Note, 0); }
		public NoteContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_note; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterNote(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitNote(this);
		}
	}

	public final NoteContext note() throws RecognitionException {
		NoteContext _localctx = new NoteContext(_ctx, getState());
		enterRule(_localctx, 44, RULE_note);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(396);
			match(Note);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class RestContext extends ParserRuleContext {
		public TerminalNode Rest() { return getToken(MmlParser.Rest, 0); }
		public RestContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_rest; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterRest(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitRest(this);
		}
	}

	public final RestContext rest() throws RecognitionException {
		RestContext _localctx = new RestContext(_ctx, getState());
		enterRule(_localctx, 46, RULE_rest);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(398);
			match(Rest);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class OctaveContext extends ParserRuleContext {
		public TerminalNode Octave() { return getToken(MmlParser.Octave, 0); }
		public OctaveContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_octave; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterOctave(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitOctave(this);
		}
	}

	public final OctaveContext octave() throws RecognitionException {
		OctaveContext _localctx = new OctaveContext(_ctx, getState());
		enterRule(_localctx, 48, RULE_octave);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(400);
			match(Octave);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class LowerOctaveContext extends ParserRuleContext {
		public TerminalNode LT() { return getToken(MmlParser.LT, 0); }
		public LowerOctaveContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_lowerOctave; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterLowerOctave(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitLowerOctave(this);
		}
	}

	public final LowerOctaveContext lowerOctave() throws RecognitionException {
		LowerOctaveContext _localctx = new LowerOctaveContext(_ctx, getState());
		enterRule(_localctx, 50, RULE_lowerOctave);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(402);
			match(LT);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class RaiseOctaveContext extends ParserRuleContext {
		public TerminalNode GT() { return getToken(MmlParser.GT, 0); }
		public RaiseOctaveContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_raiseOctave; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterRaiseOctave(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitRaiseOctave(this);
		}
	}

	public final RaiseOctaveContext raiseOctave() throws RecognitionException {
		RaiseOctaveContext _localctx = new RaiseOctaveContext(_ctx, getState());
		enterRule(_localctx, 52, RULE_raiseOctave);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(404);
			match(GT);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class NoiseNoteContext extends ParserRuleContext {
		public TerminalNode Noise() { return getToken(MmlParser.Noise, 0); }
		public NoiseNoteContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_noiseNote; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterNoiseNote(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitNoiseNote(this);
		}
	}

	public final NoiseNoteContext noiseNote() throws RecognitionException {
		NoiseNoteContext _localctx = new NoiseNoteContext(_ctx, getState());
		enterRule(_localctx, 54, RULE_noiseNote);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(406);
			match(Noise);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class VolumeCommandContext extends ParserRuleContext {
		public VolumeCommandContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_volumeCommand; }
	 
		public VolumeCommandContext() { }
		public void copyFrom(VolumeCommandContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class HexVolumeContext extends VolumeCommandContext {
		public E7VolumeContext e7Volume() {
			return getRuleContext(E7VolumeContext.class,0);
		}
		public E8VolumeFadeContext e8VolumeFade() {
			return getRuleContext(E8VolumeFadeContext.class,0);
		}
		public HexVolumeContext(VolumeCommandContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterHexVolume(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitHexVolume(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class VolumeContext extends VolumeCommandContext {
		public TerminalNode Volume() { return getToken(MmlParser.Volume, 0); }
		public VolumeContext(VolumeCommandContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterVolume(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitVolume(this);
		}
	}

	public final VolumeCommandContext volumeCommand() throws RecognitionException {
		VolumeCommandContext _localctx = new VolumeCommandContext(_ctx, getState());
		enterRule(_localctx, 56, RULE_volumeCommand);
		try {
			setState(413);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Volume:
				_localctx = new VolumeContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(408);
				match(Volume);
				}
				break;
			case NE7:
			case NE8:
				_localctx = new HexVolumeContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(411);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case NE7:
					{
					setState(409);
					e7Volume();
					}
					break;
				case NE8:
					{
					setState(410);
					e8VolumeFade();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class TuneCommandContext extends ParserRuleContext {
		public TuneCommandContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_tuneCommand; }
	 
		public TuneCommandContext() { }
		public void copyFrom(TuneCommandContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class HexTuneContext extends TuneCommandContext {
		public EeTuneChannelContext eeTuneChannel() {
			return getRuleContext(EeTuneChannelContext.class,0);
		}
		public HexTuneContext(TuneCommandContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterHexTune(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitHexTune(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class TuneContext extends TuneCommandContext {
		public TerminalNode Tune() { return getToken(MmlParser.Tune, 0); }
		public TuneContext(TuneCommandContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterTune(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitTune(this);
		}
	}

	public final TuneCommandContext tuneCommand() throws RecognitionException {
		TuneCommandContext _localctx = new TuneCommandContext(_ctx, getState());
		enterRule(_localctx, 58, RULE_tuneCommand);
		try {
			setState(417);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Tune:
				_localctx = new TuneContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(415);
				match(Tune);
				}
				break;
			case NEE:
				_localctx = new HexTuneContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(416);
				eeTuneChannel();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class QuantizationContext extends ParserRuleContext {
		public TerminalNode Quantization() { return getToken(MmlParser.Quantization, 0); }
		public QuantizationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_quantization; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterQuantization(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitQuantization(this);
		}
	}

	public final QuantizationContext quantization() throws RecognitionException {
		QuantizationContext _localctx = new QuantizationContext(_ctx, getState());
		enterRule(_localctx, 60, RULE_quantization);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(419);
			match(Quantization);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class PanCommandContext extends ParserRuleContext {
		public PanCommandContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_panCommand; }
	 
		public PanCommandContext() { }
		public void copyFrom(PanCommandContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class HexPanContext extends PanCommandContext {
		public DbPanContext dbPan() {
			return getRuleContext(DbPanContext.class,0);
		}
		public DcPanFadeContext dcPanFade() {
			return getRuleContext(DcPanFadeContext.class,0);
		}
		public HexPanContext(PanCommandContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterHexPan(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitHexPan(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class PanContext extends PanCommandContext {
		public TerminalNode Pan() { return getToken(MmlParser.Pan, 0); }
		public PanContext(PanCommandContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterPan(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitPan(this);
		}
	}

	public final PanCommandContext panCommand() throws RecognitionException {
		PanCommandContext _localctx = new PanCommandContext(_ctx, getState());
		enterRule(_localctx, 62, RULE_panCommand);
		try {
			setState(426);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Pan:
				_localctx = new PanContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(421);
				match(Pan);
				}
				break;
			case NDB:
			case NDC:
				_localctx = new HexPanContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(424);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case NDB:
					{
					setState(422);
					dbPan();
					}
					break;
				case NDC:
					{
					setState(423);
					dcPanFade();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class VibratoCommandContext extends ParserRuleContext {
		public VibratoCommandContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_vibratoCommand; }
	 
		public VibratoCommandContext() { }
		public void copyFrom(VibratoCommandContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class VibratoContext extends VibratoCommandContext {
		public TerminalNode Vibrato() { return getToken(MmlParser.Vibrato, 0); }
		public VibratoContext(VibratoCommandContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterVibrato(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitVibrato(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class HexVibratoContext extends VibratoCommandContext {
		public DeVibratoStartContext deVibratoStart() {
			return getRuleContext(DeVibratoStartContext.class,0);
		}
		public HexVibratoContext(VibratoCommandContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterHexVibrato(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitHexVibrato(this);
		}
	}

	public final VibratoCommandContext vibratoCommand() throws RecognitionException {
		VibratoCommandContext _localctx = new VibratoCommandContext(_ctx, getState());
		enterRule(_localctx, 64, RULE_vibratoCommand);
		try {
			setState(430);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Vibrato:
				_localctx = new VibratoContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(428);
				match(Vibrato);
				}
				break;
			case NDE:
				_localctx = new HexVibratoContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(429);
				deVibratoStart();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class PitchslideContext extends ParserRuleContext {
		public List<TerminalNode> Note() { return getTokens(MmlParser.Note); }
		public TerminalNode Note(int i) {
			return getToken(MmlParser.Note, i);
		}
		public List<TerminalNode> Rest() { return getTokens(MmlParser.Rest); }
		public TerminalNode Rest(int i) {
			return getToken(MmlParser.Rest, i);
		}
		public List<TerminalNode> AMPER() { return getTokens(MmlParser.AMPER); }
		public TerminalNode AMPER(int i) {
			return getToken(MmlParser.AMPER, i);
		}
		public PitchslideContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_pitchslide; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterPitchslide(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitPitchslide(this);
		}
	}

	public final PitchslideContext pitchslide() throws RecognitionException {
		PitchslideContext _localctx = new PitchslideContext(_ctx, getState());
		enterRule(_localctx, 66, RULE_pitchslide);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(432);
			_la = _input.LA(1);
			if ( !(_la==Note || _la==Rest) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			setState(435); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(433);
				match(AMPER);
				setState(434);
				_la = _input.LA(1);
				if ( !(_la==Note || _la==Rest) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				}
				}
				setState(437); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( _la==AMPER );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class TripletContext extends ParserRuleContext {
		public TerminalNode LBRACE() { return getToken(MmlParser.LBRACE, 0); }
		public TerminalNode RBRACE() { return getToken(MmlParser.RBRACE, 0); }
		public List<NoteContext> note() {
			return getRuleContexts(NoteContext.class);
		}
		public NoteContext note(int i) {
			return getRuleContext(NoteContext.class,i);
		}
		public List<RestContext> rest() {
			return getRuleContexts(RestContext.class);
		}
		public RestContext rest(int i) {
			return getRuleContext(RestContext.class,i);
		}
		public TripletContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_triplet; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterTriplet(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitTriplet(this);
		}
	}

	public final TripletContext triplet() throws RecognitionException {
		TripletContext _localctx = new TripletContext(_ctx, getState());
		enterRule(_localctx, 68, RULE_triplet);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(439);
			match(LBRACE);
			setState(442);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Note:
				{
				setState(440);
				note();
				}
				break;
			case Rest:
				{
				setState(441);
				rest();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(446);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Note:
				{
				setState(444);
				note();
				}
				break;
			case Rest:
				{
				setState(445);
				rest();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(450);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Note:
				{
				setState(448);
				note();
				}
				break;
			case Rest:
				{
				setState(449);
				rest();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(452);
			match(RBRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class DefaultLengthContext extends ParserRuleContext {
		public TerminalNode Length() { return getToken(MmlParser.Length, 0); }
		public DefaultLengthContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_defaultLength; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterDefaultLength(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitDefaultLength(this);
		}
	}

	public final DefaultLengthContext defaultLength() throws RecognitionException {
		DefaultLengthContext _localctx = new DefaultLengthContext(_ctx, getState());
		enterRule(_localctx, 70, RULE_defaultLength);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(454);
			match(Length);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class GlobalVolumeCommandContext extends ParserRuleContext {
		public GlobalVolumeCommandContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_globalVolumeCommand; }
	 
		public GlobalVolumeCommandContext() { }
		public void copyFrom(GlobalVolumeCommandContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class GlobalVolumeContext extends GlobalVolumeCommandContext {
		public TerminalNode GlobalVolume() { return getToken(MmlParser.GlobalVolume, 0); }
		public GlobalVolumeContext(GlobalVolumeCommandContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterGlobalVolume(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitGlobalVolume(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class HexGlobalVolumeContext extends GlobalVolumeCommandContext {
		public E0GlobalVolumeContext e0GlobalVolume() {
			return getRuleContext(E0GlobalVolumeContext.class,0);
		}
		public E1GlobalVolumeFadeContext e1GlobalVolumeFade() {
			return getRuleContext(E1GlobalVolumeFadeContext.class,0);
		}
		public HexGlobalVolumeContext(GlobalVolumeCommandContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterHexGlobalVolume(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitHexGlobalVolume(this);
		}
	}

	public final GlobalVolumeCommandContext globalVolumeCommand() throws RecognitionException {
		GlobalVolumeCommandContext _localctx = new GlobalVolumeCommandContext(_ctx, getState());
		enterRule(_localctx, 72, RULE_globalVolumeCommand);
		try {
			setState(461);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case GlobalVolume:
				_localctx = new GlobalVolumeContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(456);
				match(GlobalVolume);
				}
				break;
			case NE0:
			case NE1:
				_localctx = new HexGlobalVolumeContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(459);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case NE0:
					{
					setState(457);
					e0GlobalVolume();
					}
					break;
				case NE1:
					{
					setState(458);
					e1GlobalVolumeFade();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class TempoCommandContext extends ParserRuleContext {
		public TempoCommandContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_tempoCommand; }
	 
		public TempoCommandContext() { }
		public void copyFrom(TempoCommandContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class TempoContext extends TempoCommandContext {
		public TerminalNode Tempo() { return getToken(MmlParser.Tempo, 0); }
		public TempoContext(TempoCommandContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterTempo(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitTempo(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class HexTempoContext extends TempoCommandContext {
		public E2TempoContext e2Tempo() {
			return getRuleContext(E2TempoContext.class,0);
		}
		public E3TempoFadeContext e3TempoFade() {
			return getRuleContext(E3TempoFadeContext.class,0);
		}
		public HexTempoContext(TempoCommandContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterHexTempo(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitHexTempo(this);
		}
	}

	public final TempoCommandContext tempoCommand() throws RecognitionException {
		TempoCommandContext _localctx = new TempoCommandContext(_ctx, getState());
		enterRule(_localctx, 74, RULE_tempoCommand);
		try {
			setState(468);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Tempo:
				_localctx = new TempoContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(463);
				match(Tempo);
				}
				break;
			case NE2:
			case NE3:
				_localctx = new HexTempoContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(466);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case NE2:
					{
					setState(464);
					e2Tempo();
					}
					break;
				case NE3:
					{
					setState(465);
					e3TempoFade();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class InstrumentCommandContext extends ParserRuleContext {
		public InstrumentCommandContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_instrumentCommand; }
	 
		public InstrumentCommandContext() { }
		public void copyFrom(InstrumentCommandContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class HexInstrumentContext extends InstrumentCommandContext {
		public DaInstrumentContext daInstrument() {
			return getRuleContext(DaInstrumentContext.class,0);
		}
		public HexInstrumentContext(InstrumentCommandContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterHexInstrument(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitHexInstrument(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class InstrumentContext extends InstrumentCommandContext {
		public TerminalNode Instrument() { return getToken(MmlParser.Instrument, 0); }
		public InstrumentContext(InstrumentCommandContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterInstrument(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitInstrument(this);
		}
	}

	public final InstrumentCommandContext instrumentCommand() throws RecognitionException {
		InstrumentCommandContext _localctx = new InstrumentCommandContext(_ctx, getState());
		enterRule(_localctx, 76, RULE_instrumentCommand);
		try {
			setState(472);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Instrument:
				_localctx = new InstrumentContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(470);
				match(Instrument);
				}
				break;
			case NDA:
				_localctx = new HexInstrumentContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(471);
				daInstrument();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class NakedTieContext extends ParserRuleContext {
		public TerminalNode Tie() { return getToken(MmlParser.Tie, 0); }
		public NakedTieContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_nakedTie; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterNakedTie(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitNakedTie(this);
		}
	}

	public final NakedTieContext nakedTie() throws RecognitionException {
		NakedTieContext _localctx = new NakedTieContext(_ctx, getState());
		enterRule(_localctx, 78, RULE_nakedTie);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(474);
			match(Tie);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class QmarkContext extends ParserRuleContext {
		public TerminalNode Question() { return getToken(MmlParser.Question, 0); }
		public QmarkContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_qmark; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterQmark(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitQmark(this);
		}
	}

	public final QmarkContext qmark() throws RecognitionException {
		QmarkContext _localctx = new QmarkContext(_ctx, getState());
		enterRule(_localctx, 80, RULE_qmark);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(476);
			match(Question);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class PipeContext extends ParserRuleContext {
		public TerminalNode Pipe() { return getToken(MmlParser.Pipe, 0); }
		public PipeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_pipe; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterPipe(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitPipe(this);
		}
	}

	public final PipeContext pipe() throws RecognitionException {
		PipeContext _localctx = new PipeContext(_ctx, getState());
		enterRule(_localctx, 82, RULE_pipe);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(478);
			match(Pipe);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class LoopersContext extends ParserRuleContext {
		public LogicControlsContext logicControls() {
			return getRuleContext(LogicControlsContext.class,0);
		}
		public LogicCallsContext logicCalls() {
			return getRuleContext(LogicCallsContext.class,0);
		}
		public LoopersContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_loopers; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterLoopers(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitLoopers(this);
		}
	}

	public final LoopersContext loopers() throws RecognitionException {
		LoopersContext _localctx = new LoopersContext(_ctx, getState());
		enterRule(_localctx, 84, RULE_loopers);
		try {
			setState(482);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,35,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(480);
				logicControls();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(481);
				logicCalls();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class LogicControlsContext extends ParserRuleContext {
		public SuperLoopContext superLoop() {
			return getRuleContext(SuperLoopContext.class,0);
		}
		public SimpleLoopContext simpleLoop() {
			return getRuleContext(SimpleLoopContext.class,0);
		}
		public LogicControlsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_logicControls; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterLogicControls(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitLogicControls(this);
		}
	}

	public final LogicControlsContext logicControls() throws RecognitionException {
		LogicControlsContext _localctx = new LogicControlsContext(_ctx, getState());
		enterRule(_localctx, 86, RULE_logicControls);
		try {
			setState(486);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case L2BRACK:
				enterOuterAlt(_localctx, 1);
				{
				setState(484);
				superLoop();
				}
				break;
			case LBRACK:
			case LoopName:
				enterOuterAlt(_localctx, 2);
				{
				setState(485);
				simpleLoop();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class LogicCallsContext extends ParserRuleContext {
		public CallLoopContext callLoop() {
			return getRuleContext(CallLoopContext.class,0);
		}
		public RemoteLogicCallsContext remoteLogicCalls() {
			return getRuleContext(RemoteLogicCallsContext.class,0);
		}
		public LogicCallsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_logicCalls; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterLogicCalls(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitLogicCalls(this);
		}
	}

	public final LogicCallsContext logicCalls() throws RecognitionException {
		LogicCallsContext _localctx = new LogicCallsContext(_ctx, getState());
		enterRule(_localctx, 88, RULE_logicCalls);
		try {
			setState(490);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case LoopName:
				enterOuterAlt(_localctx, 1);
				{
				setState(488);
				callLoop();
				}
				break;
			case STAR:
			case StopRemoteCode:
			case CallRemoteCode:
				enterOuterAlt(_localctx, 2);
				{
				setState(489);
				remoteLogicCalls();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class RemoteLogicCallsContext extends ParserRuleContext {
		public CallRemoteCodeContext callRemoteCode() {
			return getRuleContext(CallRemoteCodeContext.class,0);
		}
		public CallPreviousLoopContext callPreviousLoop() {
			return getRuleContext(CallPreviousLoopContext.class,0);
		}
		public StopRemoteCodeContext stopRemoteCode() {
			return getRuleContext(StopRemoteCodeContext.class,0);
		}
		public RemoteLogicCallsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_remoteLogicCalls; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterRemoteLogicCalls(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitRemoteLogicCalls(this);
		}
	}

	public final RemoteLogicCallsContext remoteLogicCalls() throws RecognitionException {
		RemoteLogicCallsContext _localctx = new RemoteLogicCallsContext(_ctx, getState());
		enterRule(_localctx, 90, RULE_remoteLogicCalls);
		try {
			setState(495);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case CallRemoteCode:
				enterOuterAlt(_localctx, 1);
				{
				setState(492);
				callRemoteCode();
				}
				break;
			case STAR:
				enterOuterAlt(_localctx, 2);
				{
				setState(493);
				callPreviousLoop();
				}
				break;
			case StopRemoteCode:
				enterOuterAlt(_localctx, 3);
				{
				setState(494);
				stopRemoteCode();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SuperLoopContext extends ParserRuleContext {
		public TerminalNode L2BRACK() { return getToken(MmlParser.L2BRACK, 0); }
		public TerminalNode R2BRACK() { return getToken(MmlParser.R2BRACK, 0); }
		public List<SuperLoopContentsContext> superLoopContents() {
			return getRuleContexts(SuperLoopContentsContext.class);
		}
		public SuperLoopContentsContext superLoopContents(int i) {
			return getRuleContext(SuperLoopContentsContext.class,i);
		}
		public TerminalNode NUMBERS() { return getToken(MmlParser.NUMBERS, 0); }
		public SuperLoopContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_superLoop; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterSuperLoop(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitSuperLoop(this);
		}
	}

	public final SuperLoopContext superLoop() throws RecognitionException {
		SuperLoopContext _localctx = new SuperLoopContext(_ctx, getState());
		enterRule(_localctx, 92, RULE_superLoop);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(497);
			match(L2BRACK);
			setState(501);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 3937896646054272L) != 0) || ((((_la - 79)) & ~0x3f) == 0 && ((1L << (_la - 79)) & 5629499534213119L) != 0)) {
				{
				{
				setState(498);
				superLoopContents();
				}
				}
				setState(503);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(504);
			match(R2BRACK);
			setState(506);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NUMBERS) {
				{
				setState(505);
				match(NUMBERS);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SuperLoopContentsContext extends ParserRuleContext {
		public AtomicsContext atomics() {
			return getRuleContext(AtomicsContext.class,0);
		}
		public TerminalSimpleLoopContext terminalSimpleLoop() {
			return getRuleContext(TerminalSimpleLoopContext.class,0);
		}
		public LogicCallsContext logicCalls() {
			return getRuleContext(LogicCallsContext.class,0);
		}
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public SuperLoopContentsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_superLoopContents; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterSuperLoopContents(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitSuperLoopContents(this);
		}
	}

	public final SuperLoopContentsContext superLoopContents() throws RecognitionException {
		SuperLoopContentsContext _localctx = new SuperLoopContentsContext(_ctx, getState());
		enterRule(_localctx, 94, RULE_superLoopContents);
		try {
			setState(512);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,41,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(508);
				atomics();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(509);
				terminalSimpleLoop();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(510);
				logicCalls();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(511);
				hexNumber();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SimpleLoopContext extends ParserRuleContext {
		public TerminalNode LBRACK() { return getToken(MmlParser.LBRACK, 0); }
		public TerminalNode RBRACK() { return getToken(MmlParser.RBRACK, 0); }
		public TerminalNode LoopName() { return getToken(MmlParser.LoopName, 0); }
		public List<SimpleLoopContentsContext> simpleLoopContents() {
			return getRuleContexts(SimpleLoopContentsContext.class);
		}
		public SimpleLoopContentsContext simpleLoopContents(int i) {
			return getRuleContext(SimpleLoopContentsContext.class,i);
		}
		public TerminalNode NUMBERS() { return getToken(MmlParser.NUMBERS, 0); }
		public SimpleLoopContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_simpleLoop; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterSimpleLoop(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitSimpleLoop(this);
		}
	}

	public final SimpleLoopContext simpleLoop() throws RecognitionException {
		SimpleLoopContext _localctx = new SimpleLoopContext(_ctx, getState());
		enterRule(_localctx, 96, RULE_simpleLoop);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(515);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LoopName) {
				{
				setState(514);
				match(LoopName);
				}
			}

			setState(517);
			match(LBRACK);
			setState(521);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 3656421669355904L) != 0) || ((((_la - 79)) & ~0x3f) == 0 && ((1L << (_la - 79)) & 5629499534213119L) != 0)) {
				{
				{
				setState(518);
				simpleLoopContents();
				}
				}
				setState(523);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(524);
			match(RBRACK);
			setState(526);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NUMBERS) {
				{
				setState(525);
				match(NUMBERS);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SimpleLoopContentsContext extends ParserRuleContext {
		public AtomicsContext atomics() {
			return getRuleContext(AtomicsContext.class,0);
		}
		public TerminalSuperLoopContext terminalSuperLoop() {
			return getRuleContext(TerminalSuperLoopContext.class,0);
		}
		public RemoteLogicCallsContext remoteLogicCalls() {
			return getRuleContext(RemoteLogicCallsContext.class,0);
		}
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public SimpleLoopContentsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_simpleLoopContents; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterSimpleLoopContents(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitSimpleLoopContents(this);
		}
	}

	public final SimpleLoopContentsContext simpleLoopContents() throws RecognitionException {
		SimpleLoopContentsContext _localctx = new SimpleLoopContentsContext(_ctx, getState());
		enterRule(_localctx, 98, RULE_simpleLoopContents);
		try {
			setState(532);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,45,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(528);
				atomics();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(529);
				terminalSuperLoop();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(530);
				remoteLogicCalls();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(531);
				hexNumber();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class TerminalSuperLoopContext extends ParserRuleContext {
		public TerminalNode L2BRACK() { return getToken(MmlParser.L2BRACK, 0); }
		public TerminalNode R2BRACK() { return getToken(MmlParser.R2BRACK, 0); }
		public List<TerminalSuperLoopContentsContext> terminalSuperLoopContents() {
			return getRuleContexts(TerminalSuperLoopContentsContext.class);
		}
		public TerminalSuperLoopContentsContext terminalSuperLoopContents(int i) {
			return getRuleContext(TerminalSuperLoopContentsContext.class,i);
		}
		public TerminalNode NUMBERS() { return getToken(MmlParser.NUMBERS, 0); }
		public TerminalSuperLoopContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_terminalSuperLoop; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterTerminalSuperLoop(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitTerminalSuperLoop(this);
		}
	}

	public final TerminalSuperLoopContext terminalSuperLoop() throws RecognitionException {
		TerminalSuperLoopContext _localctx = new TerminalSuperLoopContext(_ctx, getState());
		enterRule(_localctx, 100, RULE_terminalSuperLoop);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(534);
			match(L2BRACK);
			setState(538);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 3937896646050176L) != 0) || ((((_la - 79)) & ~0x3f) == 0 && ((1L << (_la - 79)) & 5629499534213119L) != 0)) {
				{
				{
				setState(535);
				terminalSuperLoopContents();
				}
				}
				setState(540);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(541);
			match(R2BRACK);
			setState(543);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NUMBERS) {
				{
				setState(542);
				match(NUMBERS);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class TerminalSuperLoopContentsContext extends ParserRuleContext {
		public AtomicsContext atomics() {
			return getRuleContext(AtomicsContext.class,0);
		}
		public LogicCallsContext logicCalls() {
			return getRuleContext(LogicCallsContext.class,0);
		}
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public TerminalSuperLoopContentsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_terminalSuperLoopContents; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterTerminalSuperLoopContents(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitTerminalSuperLoopContents(this);
		}
	}

	public final TerminalSuperLoopContentsContext terminalSuperLoopContents() throws RecognitionException {
		TerminalSuperLoopContentsContext _localctx = new TerminalSuperLoopContentsContext(_ctx, getState());
		enterRule(_localctx, 102, RULE_terminalSuperLoopContents);
		try {
			setState(548);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,48,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(545);
				atomics();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(546);
				logicCalls();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(547);
				hexNumber();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class TerminalSimpleLoopContext extends ParserRuleContext {
		public TerminalNode LBRACK() { return getToken(MmlParser.LBRACK, 0); }
		public TerminalNode RBRACK() { return getToken(MmlParser.RBRACK, 0); }
		public TerminalNode LoopName() { return getToken(MmlParser.LoopName, 0); }
		public List<TerminalSimpleLoopContentsContext> terminalSimpleLoopContents() {
			return getRuleContexts(TerminalSimpleLoopContentsContext.class);
		}
		public TerminalSimpleLoopContentsContext terminalSimpleLoopContents(int i) {
			return getRuleContext(TerminalSimpleLoopContentsContext.class,i);
		}
		public TerminalNode NUMBERS() { return getToken(MmlParser.NUMBERS, 0); }
		public TerminalSimpleLoopContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_terminalSimpleLoop; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterTerminalSimpleLoop(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitTerminalSimpleLoop(this);
		}
	}

	public final TerminalSimpleLoopContext terminalSimpleLoop() throws RecognitionException {
		TerminalSimpleLoopContext _localctx = new TerminalSimpleLoopContext(_ctx, getState());
		enterRule(_localctx, 104, RULE_terminalSimpleLoop);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(551);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LoopName) {
				{
				setState(550);
				match(LoopName);
				}
			}

			setState(553);
			match(LBRACK);
			setState(557);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 3656421669339520L) != 0) || ((((_la - 79)) & ~0x3f) == 0 && ((1L << (_la - 79)) & 5629499534213119L) != 0)) {
				{
				{
				setState(554);
				terminalSimpleLoopContents();
				}
				}
				setState(559);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(560);
			match(RBRACK);
			setState(562);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NUMBERS) {
				{
				setState(561);
				match(NUMBERS);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class TerminalSimpleLoopContentsContext extends ParserRuleContext {
		public AtomicsContext atomics() {
			return getRuleContext(AtomicsContext.class,0);
		}
		public RemoteLogicCallsContext remoteLogicCalls() {
			return getRuleContext(RemoteLogicCallsContext.class,0);
		}
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public TerminalSimpleLoopContentsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_terminalSimpleLoopContents; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterTerminalSimpleLoopContents(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitTerminalSimpleLoopContents(this);
		}
	}

	public final TerminalSimpleLoopContentsContext terminalSimpleLoopContents() throws RecognitionException {
		TerminalSimpleLoopContentsContext _localctx = new TerminalSimpleLoopContentsContext(_ctx, getState());
		enterRule(_localctx, 106, RULE_terminalSimpleLoopContents);
		try {
			setState(567);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,52,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(564);
				atomics();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(565);
				remoteLogicCalls();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(566);
				hexNumber();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class RemoteCodeContext extends ParserRuleContext {
		public TerminalNode RemoteCodeName() { return getToken(MmlParser.RemoteCodeName, 0); }
		public TerminalNode LBRACK() { return getToken(MmlParser.LBRACK, 0); }
		public TerminalNode RBRACK() { return getToken(MmlParser.RBRACK, 0); }
		public List<RemoteCodeContentsContext> remoteCodeContents() {
			return getRuleContexts(RemoteCodeContentsContext.class);
		}
		public RemoteCodeContentsContext remoteCodeContents(int i) {
			return getRuleContext(RemoteCodeContentsContext.class,i);
		}
		public RemoteCodeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_remoteCode; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterRemoteCode(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitRemoteCode(this);
		}
	}

	public final RemoteCodeContext remoteCode() throws RecognitionException {
		RemoteCodeContext _localctx = new RemoteCodeContext(_ctx, getState());
		enterRule(_localctx, 108, RULE_remoteCode);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(569);
			match(RemoteCodeName);
			setState(570);
			match(LBRACK);
			setState(572); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(571);
				remoteCodeContents();
				}
				}
				setState(574); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & 14791879950336L) != 0) || ((((_la - 79)) & ~0x3f) == 0 && ((1L << (_la - 79)) & 5629499534213119L) != 0) );
			setState(576);
			match(RBRACK);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class RemoteCodeContentsContext extends ParserRuleContext {
		public OctaveContext octave() {
			return getRuleContext(OctaveContext.class,0);
		}
		public LowerOctaveContext lowerOctave() {
			return getRuleContext(LowerOctaveContext.class,0);
		}
		public RaiseOctaveContext raiseOctave() {
			return getRuleContext(RaiseOctaveContext.class,0);
		}
		public VolumeCommandContext volumeCommand() {
			return getRuleContext(VolumeCommandContext.class,0);
		}
		public TuneCommandContext tuneCommand() {
			return getRuleContext(TuneCommandContext.class,0);
		}
		public QuantizationContext quantization() {
			return getRuleContext(QuantizationContext.class,0);
		}
		public PanCommandContext panCommand() {
			return getRuleContext(PanCommandContext.class,0);
		}
		public VibratoCommandContext vibratoCommand() {
			return getRuleContext(VibratoCommandContext.class,0);
		}
		public TempoCommandContext tempoCommand() {
			return getRuleContext(TempoCommandContext.class,0);
		}
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public RemoteCodeContentsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_remoteCodeContents; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterRemoteCodeContents(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitRemoteCodeContents(this);
		}
	}

	public final RemoteCodeContentsContext remoteCodeContents() throws RecognitionException {
		RemoteCodeContentsContext _localctx = new RemoteCodeContentsContext(_ctx, getState());
		enterRule(_localctx, 110, RULE_remoteCodeContents);
		try {
			setState(588);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,54,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(578);
				octave();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(579);
				lowerOctave();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(580);
				raiseOctave();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(581);
				volumeCommand();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(582);
				tuneCommand();
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(583);
				quantization();
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(584);
				panCommand();
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(585);
				vibratoCommand();
				}
				break;
			case 9:
				enterOuterAlt(_localctx, 9);
				{
				setState(586);
				tempoCommand();
				}
				break;
			case 10:
				enterOuterAlt(_localctx, 10);
				{
				setState(587);
				hexNumber();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class CallLoopContext extends ParserRuleContext {
		public TerminalNode LoopName() { return getToken(MmlParser.LoopName, 0); }
		public TerminalNode NUMBERS() { return getToken(MmlParser.NUMBERS, 0); }
		public CallLoopContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_callLoop; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterCallLoop(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitCallLoop(this);
		}
	}

	public final CallLoopContext callLoop() throws RecognitionException {
		CallLoopContext _localctx = new CallLoopContext(_ctx, getState());
		enterRule(_localctx, 112, RULE_callLoop);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(590);
			match(LoopName);
			setState(592);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NUMBERS) {
				{
				setState(591);
				match(NUMBERS);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class CallRemoteCodeContext extends ParserRuleContext {
		public TerminalNode CallRemoteCode() { return getToken(MmlParser.CallRemoteCode, 0); }
		public CallRemoteCodeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_callRemoteCode; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterCallRemoteCode(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitCallRemoteCode(this);
		}
	}

	public final CallRemoteCodeContext callRemoteCode() throws RecognitionException {
		CallRemoteCodeContext _localctx = new CallRemoteCodeContext(_ctx, getState());
		enterRule(_localctx, 114, RULE_callRemoteCode);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(594);
			match(CallRemoteCode);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StopRemoteCodeContext extends ParserRuleContext {
		public TerminalNode StopRemoteCode() { return getToken(MmlParser.StopRemoteCode, 0); }
		public StopRemoteCodeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_stopRemoteCode; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterStopRemoteCode(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitStopRemoteCode(this);
		}
	}

	public final StopRemoteCodeContext stopRemoteCode() throws RecognitionException {
		StopRemoteCodeContext _localctx = new StopRemoteCodeContext(_ctx, getState());
		enterRule(_localctx, 116, RULE_stopRemoteCode);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(596);
			match(StopRemoteCode);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class CallPreviousLoopContext extends ParserRuleContext {
		public TerminalNode STAR() { return getToken(MmlParser.STAR, 0); }
		public TerminalNode NUMBERS() { return getToken(MmlParser.NUMBERS, 0); }
		public CallPreviousLoopContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_callPreviousLoop; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterCallPreviousLoop(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitCallPreviousLoop(this);
		}
	}

	public final CallPreviousLoopContext callPreviousLoop() throws RecognitionException {
		CallPreviousLoopContext _localctx = new CallPreviousLoopContext(_ctx, getState());
		enterRule(_localctx, 118, RULE_callPreviousLoop);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(598);
			match(STAR);
			setState(600);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NUMBERS) {
				{
				setState(599);
				match(NUMBERS);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class NoloopCommandContext extends ParserRuleContext {
		public TerminalNode QMARK() { return getToken(MmlParser.QMARK, 0); }
		public NoloopCommandContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_noloopCommand; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterNoloopCommand(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitNoloopCommand(this);
		}
	}

	public final NoloopCommandContext noloopCommand() throws RecognitionException {
		NoloopCommandContext _localctx = new NoloopCommandContext(_ctx, getState());
		enterRule(_localctx, 120, RULE_noloopCommand);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(602);
			match(QMARK);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SampleLoadCommandContext extends ParserRuleContext {
		public SampleLoadCommandContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_sampleLoadCommand; }
	 
		public SampleLoadCommandContext() { }
		public void copyFrom(SampleLoadCommandContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class HexSampleLoadContext extends SampleLoadCommandContext {
		public F3SampleLoadContext f3SampleLoad() {
			return getRuleContext(F3SampleLoadContext.class,0);
		}
		public HexSampleLoadContext(SampleLoadCommandContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterHexSampleLoad(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitHexSampleLoad(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class SampleLoadContext extends SampleLoadCommandContext {
		public TerminalNode LoadSample() { return getToken(MmlParser.LoadSample, 0); }
		public SampleLoadContext(SampleLoadCommandContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterSampleLoad(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitSampleLoad(this);
		}
	}

	public final SampleLoadCommandContext sampleLoadCommand() throws RecognitionException {
		SampleLoadCommandContext _localctx = new SampleLoadCommandContext(_ctx, getState());
		enterRule(_localctx, 122, RULE_sampleLoadCommand);
		try {
			setState(606);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case LoadSample:
				_localctx = new SampleLoadContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(604);
				match(LoadSample);
				}
				break;
			case NF3:
				_localctx = new HexSampleLoadContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(605);
				f3SampleLoad();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ReplacementsContext extends ParserRuleContext {
		public TerminalNode ReplacementText() { return getToken(MmlParser.ReplacementText, 0); }
		public ReplacementsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_replacements; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterReplacements(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitReplacements(this);
		}
	}

	public final ReplacementsContext replacements() throws RecognitionException {
		ReplacementsContext _localctx = new ReplacementsContext(_ctx, getState());
		enterRule(_localctx, 124, RULE_replacements);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(608);
			match(ReplacementText);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class GlobalHexCommandsContext extends ParserRuleContext {
		public F5FIRFilterContext f5FIRFilter() {
			return getRuleContext(F5FIRFilterContext.class,0);
		}
		public E4GlobalTransposeContext e4GlobalTranspose() {
			return getRuleContext(E4GlobalTransposeContext.class,0);
		}
		public F4GlobalItemsContext f4GlobalItems() {
			return getRuleContext(F4GlobalItemsContext.class,0);
		}
		public EfEcho1Context efEcho1() {
			return getRuleContext(EfEcho1Context.class,0);
		}
		public F1Echo2Context f1Echo2() {
			return getRuleContext(F1Echo2Context.class,0);
		}
		public F0EchoOffContext f0EchoOff() {
			return getRuleContext(F0EchoOffContext.class,0);
		}
		public F2EchoFadeContext f2EchoFade() {
			return getRuleContext(F2EchoFadeContext.class,0);
		}
		public F6DSPWriteContext f6DSPWrite() {
			return getRuleContext(F6DSPWriteContext.class,0);
		}
		public F9DataSendContext f9DataSend() {
			return getRuleContext(F9DataSendContext.class,0);
		}
		public FaGlobalItemsContext faGlobalItems() {
			return getRuleContext(FaGlobalItemsContext.class,0);
		}
		public GlobalHexCommandsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_globalHexCommands; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterGlobalHexCommands(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitGlobalHexCommands(this);
		}
	}

	public final GlobalHexCommandsContext globalHexCommands() throws RecognitionException {
		GlobalHexCommandsContext _localctx = new GlobalHexCommandsContext(_ctx, getState());
		enterRule(_localctx, 126, RULE_globalHexCommands);
		try {
			setState(620);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case NF5:
				enterOuterAlt(_localctx, 1);
				{
				setState(610);
				f5FIRFilter();
				}
				break;
			case NE4:
				enterOuterAlt(_localctx, 2);
				{
				setState(611);
				e4GlobalTranspose();
				}
				break;
			case NF4:
				enterOuterAlt(_localctx, 3);
				{
				setState(612);
				f4GlobalItems();
				}
				break;
			case NEF:
				enterOuterAlt(_localctx, 4);
				{
				setState(613);
				efEcho1();
				}
				break;
			case NF1:
				enterOuterAlt(_localctx, 5);
				{
				setState(614);
				f1Echo2();
				}
				break;
			case NF0:
				enterOuterAlt(_localctx, 6);
				{
				setState(615);
				f0EchoOff();
				}
				break;
			case NF2:
				enterOuterAlt(_localctx, 7);
				{
				setState(616);
				f2EchoFade();
				}
				break;
			case NF6:
				enterOuterAlt(_localctx, 8);
				{
				setState(617);
				f6DSPWrite();
				}
				break;
			case NF9:
				enterOuterAlt(_localctx, 9);
				{
				setState(618);
				f9DataSend();
				}
				break;
			case NFA:
				enterOuterAlt(_localctx, 10);
				{
				setState(619);
				faGlobalItems();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ChannelHexCommandsContext extends ParserRuleContext {
		public E5TremoloContext e5Tremolo() {
			return getRuleContext(E5TremoloContext.class,0);
		}
		public FbItemsContext fbItems() {
			return getRuleContext(FbItemsContext.class,0);
		}
		public EaVibratoFadeContext eaVibratoFade() {
			return getRuleContext(EaVibratoFadeContext.class,0);
		}
		public DfVibratoEndContext dfVibratoEnd() {
			return getRuleContext(DfVibratoEndContext.class,0);
		}
		public EbPitchEnvelopeReleaseContext ebPitchEnvelopeRelease() {
			return getRuleContext(EbPitchEnvelopeReleaseContext.class,0);
		}
		public EcPitchEnvelopeAttackContext ecPitchEnvelopeAttack() {
			return getRuleContext(EcPitchEnvelopeAttackContext.class,0);
		}
		public EdCustomADSROrGainContext edCustomADSROrGain() {
			return getRuleContext(EdCustomADSROrGainContext.class,0);
		}
		public F0EchoOffContext f0EchoOff() {
			return getRuleContext(F0EchoOffContext.class,0);
		}
		public F2EchoFadeContext f2EchoFade() {
			return getRuleContext(F2EchoFadeContext.class,0);
		}
		public F4ChannelItemsContext f4ChannelItems() {
			return getRuleContext(F4ChannelItemsContext.class,0);
		}
		public F6DSPWriteContext f6DSPWrite() {
			return getRuleContext(F6DSPWriteContext.class,0);
		}
		public F8EnableNoiseContext f8EnableNoise() {
			return getRuleContext(F8EnableNoiseContext.class,0);
		}
		public F9DataSendContext f9DataSend() {
			return getRuleContext(F9DataSendContext.class,0);
		}
		public FaChannelItemsContext faChannelItems() {
			return getRuleContext(FaChannelItemsContext.class,0);
		}
		public FcItemsContext fcItems() {
			return getRuleContext(FcItemsContext.class,0);
		}
		public FdTremoloOffContext fdTremoloOff() {
			return getRuleContext(FdTremoloOffContext.class,0);
		}
		public FePitchEnvelopeOffContext fePitchEnvelopeOff() {
			return getRuleContext(FePitchEnvelopeOffContext.class,0);
		}
		public ChannelHexCommandsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_channelHexCommands; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterChannelHexCommands(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitChannelHexCommands(this);
		}
	}

	public final ChannelHexCommandsContext channelHexCommands() throws RecognitionException {
		ChannelHexCommandsContext _localctx = new ChannelHexCommandsContext(_ctx, getState());
		enterRule(_localctx, 128, RULE_channelHexCommands);
		try {
			setState(640);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,59,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(622);
				e5Tremolo();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(623);
				fbItems();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(624);
				eaVibratoFade();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(625);
				dfVibratoEnd();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(626);
				ebPitchEnvelopeRelease();
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(627);
				ecPitchEnvelopeAttack();
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(628);
				edCustomADSROrGain();
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(629);
				f0EchoOff();
				}
				break;
			case 9:
				enterOuterAlt(_localctx, 9);
				{
				setState(630);
				f2EchoFade();
				}
				break;
			case 10:
				enterOuterAlt(_localctx, 10);
				{
				setState(631);
				f4ChannelItems();
				}
				break;
			case 11:
				enterOuterAlt(_localctx, 11);
				{
				setState(632);
				f6DSPWrite();
				}
				break;
			case 12:
				enterOuterAlt(_localctx, 12);
				{
				setState(633);
				f8EnableNoise();
				}
				break;
			case 13:
				enterOuterAlt(_localctx, 13);
				{
				setState(634);
				f9DataSend();
				}
				break;
			case 14:
				enterOuterAlt(_localctx, 14);
				{
				setState(635);
				faChannelItems();
				}
				break;
			case 15:
				enterOuterAlt(_localctx, 15);
				{
				setState(636);
				fbItems();
				}
				break;
			case 16:
				enterOuterAlt(_localctx, 16);
				{
				setState(637);
				fcItems();
				}
				break;
			case 17:
				enterOuterAlt(_localctx, 17);
				{
				setState(638);
				fdTremoloOff();
				}
				break;
			case 18:
				enterOuterAlt(_localctx, 18);
				{
				setState(639);
				fePitchEnvelopeOff();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class DaInstrumentContext extends ParserRuleContext {
		public TerminalNode NDA() { return getToken(MmlParser.NDA, 0); }
		public TerminalNode NUMBERS() { return getToken(MmlParser.NUMBERS, 0); }
		public DaInstrumentContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_daInstrument; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterDaInstrument(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitDaInstrument(this);
		}
	}

	public final DaInstrumentContext daInstrument() throws RecognitionException {
		DaInstrumentContext _localctx = new DaInstrumentContext(_ctx, getState());
		enterRule(_localctx, 130, RULE_daInstrument);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(642);
			match(NDA);
			setState(643);
			match(NUMBERS);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class DbPanContext extends ParserRuleContext {
		public TerminalNode NDB() { return getToken(MmlParser.NDB, 0); }
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public DbPanContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_dbPan; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterDbPan(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitDbPan(this);
		}
	}

	public final DbPanContext dbPan() throws RecognitionException {
		DbPanContext _localctx = new DbPanContext(_ctx, getState());
		enterRule(_localctx, 132, RULE_dbPan);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(645);
			match(NDB);
			setState(646);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class DcPanFadeContext extends ParserRuleContext {
		public TerminalNode NDC() { return getToken(MmlParser.NDC, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public DcPanFadeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_dcPanFade; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterDcPanFade(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitDcPanFade(this);
		}
	}

	public final DcPanFadeContext dcPanFade() throws RecognitionException {
		DcPanFadeContext _localctx = new DcPanFadeContext(_ctx, getState());
		enterRule(_localctx, 134, RULE_dcPanFade);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(648);
			match(NDC);
			setState(649);
			hexNumber();
			setState(650);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class DdPitchBlendCommandContext extends ParserRuleContext {
		public TerminalNode NDD() { return getToken(MmlParser.NDD, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public DdPitchBlendItemsContext ddPitchBlendItems() {
			return getRuleContext(DdPitchBlendItemsContext.class,0);
		}
		public DdPitchBlendCommandContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_ddPitchBlendCommand; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterDdPitchBlendCommand(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitDdPitchBlendCommand(this);
		}
	}

	public final DdPitchBlendCommandContext ddPitchBlendCommand() throws RecognitionException {
		DdPitchBlendCommandContext _localctx = new DdPitchBlendCommandContext(_ctx, getState());
		enterRule(_localctx, 136, RULE_ddPitchBlendCommand);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(652);
			match(NDD);
			setState(653);
			hexNumber();
			setState(656);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case N00:
			case N01:
			case N02:
			case N03:
			case N04:
			case N05:
			case N06:
			case N07:
			case N08:
			case N09:
			case N7F:
			case N80:
			case N81:
			case NFE:
			case NDA:
			case NDB:
			case NDC:
			case NDD:
			case NDE:
			case NDF:
			case NE0:
			case NE1:
			case NE2:
			case NE3:
			case NE4:
			case NE5:
			case NE6:
			case NE7:
			case NE8:
			case NE9:
			case NEA:
			case NEB:
			case NEC:
			case NED:
			case NEE:
			case NEF:
			case NF0:
			case NF1:
			case NF2:
			case NF3:
			case NF4:
			case NF5:
			case NF6:
			case NF7:
			case NF8:
			case NF9:
			case NFA:
			case NFB:
			case NFC:
			case NFD:
			case HexNumber:
				{
				setState(654);
				hexNumber();
				}
				break;
			case GT:
			case LT:
			case Note:
			case Octave:
				{
				setState(655);
				ddPitchBlendItems();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class DdPitchBlendItemsContext extends ParserRuleContext {
		public NoteContext note() {
			return getRuleContext(NoteContext.class,0);
		}
		public List<OctaveContext> octave() {
			return getRuleContexts(OctaveContext.class);
		}
		public OctaveContext octave(int i) {
			return getRuleContext(OctaveContext.class,i);
		}
		public List<RaiseOctaveContext> raiseOctave() {
			return getRuleContexts(RaiseOctaveContext.class);
		}
		public RaiseOctaveContext raiseOctave(int i) {
			return getRuleContext(RaiseOctaveContext.class,i);
		}
		public List<LowerOctaveContext> lowerOctave() {
			return getRuleContexts(LowerOctaveContext.class);
		}
		public LowerOctaveContext lowerOctave(int i) {
			return getRuleContext(LowerOctaveContext.class,i);
		}
		public DdPitchBlendItemsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_ddPitchBlendItems; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterDdPitchBlendItems(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitDdPitchBlendItems(this);
		}
	}

	public final DdPitchBlendItemsContext ddPitchBlendItems() throws RecognitionException {
		DdPitchBlendItemsContext _localctx = new DdPitchBlendItemsContext(_ctx, getState());
		enterRule(_localctx, 138, RULE_ddPitchBlendItems);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(663);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 17192452096L) != 0)) {
				{
				setState(661);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case Octave:
					{
					setState(658);
					octave();
					}
					break;
				case GT:
					{
					setState(659);
					raiseOctave();
					}
					break;
				case LT:
					{
					setState(660);
					lowerOctave();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				setState(665);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(666);
			note();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class DeVibratoStartContext extends ParserRuleContext {
		public TerminalNode NDE() { return getToken(MmlParser.NDE, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public DeVibratoStartContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_deVibratoStart; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterDeVibratoStart(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitDeVibratoStart(this);
		}
	}

	public final DeVibratoStartContext deVibratoStart() throws RecognitionException {
		DeVibratoStartContext _localctx = new DeVibratoStartContext(_ctx, getState());
		enterRule(_localctx, 140, RULE_deVibratoStart);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(668);
			match(NDE);
			setState(669);
			hexNumber();
			setState(670);
			hexNumber();
			setState(671);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class EaVibratoFadeContext extends ParserRuleContext {
		public TerminalNode NEA() { return getToken(MmlParser.NEA, 0); }
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public EaVibratoFadeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_eaVibratoFade; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterEaVibratoFade(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitEaVibratoFade(this);
		}
	}

	public final EaVibratoFadeContext eaVibratoFade() throws RecognitionException {
		EaVibratoFadeContext _localctx = new EaVibratoFadeContext(_ctx, getState());
		enterRule(_localctx, 142, RULE_eaVibratoFade);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(673);
			match(NEA);
			setState(674);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class DfVibratoEndContext extends ParserRuleContext {
		public TerminalNode NDF() { return getToken(MmlParser.NDF, 0); }
		public DfVibratoEndContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_dfVibratoEnd; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterDfVibratoEnd(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitDfVibratoEnd(this);
		}
	}

	public final DfVibratoEndContext dfVibratoEnd() throws RecognitionException {
		DfVibratoEndContext _localctx = new DfVibratoEndContext(_ctx, getState());
		enterRule(_localctx, 144, RULE_dfVibratoEnd);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(676);
			match(NDF);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class E0GlobalVolumeContext extends ParserRuleContext {
		public TerminalNode NE0() { return getToken(MmlParser.NE0, 0); }
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public E0GlobalVolumeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_e0GlobalVolume; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterE0GlobalVolume(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitE0GlobalVolume(this);
		}
	}

	public final E0GlobalVolumeContext e0GlobalVolume() throws RecognitionException {
		E0GlobalVolumeContext _localctx = new E0GlobalVolumeContext(_ctx, getState());
		enterRule(_localctx, 146, RULE_e0GlobalVolume);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(678);
			match(NE0);
			setState(679);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class E1GlobalVolumeFadeContext extends ParserRuleContext {
		public TerminalNode NE1() { return getToken(MmlParser.NE1, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public E1GlobalVolumeFadeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_e1GlobalVolumeFade; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterE1GlobalVolumeFade(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitE1GlobalVolumeFade(this);
		}
	}

	public final E1GlobalVolumeFadeContext e1GlobalVolumeFade() throws RecognitionException {
		E1GlobalVolumeFadeContext _localctx = new E1GlobalVolumeFadeContext(_ctx, getState());
		enterRule(_localctx, 148, RULE_e1GlobalVolumeFade);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(681);
			match(NE1);
			setState(682);
			hexNumber();
			setState(683);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class E2TempoContext extends ParserRuleContext {
		public TerminalNode NE2() { return getToken(MmlParser.NE2, 0); }
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public E2TempoContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_e2Tempo; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterE2Tempo(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitE2Tempo(this);
		}
	}

	public final E2TempoContext e2Tempo() throws RecognitionException {
		E2TempoContext _localctx = new E2TempoContext(_ctx, getState());
		enterRule(_localctx, 150, RULE_e2Tempo);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(685);
			match(NE2);
			setState(686);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class E3TempoFadeContext extends ParserRuleContext {
		public TerminalNode NE3() { return getToken(MmlParser.NE3, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public E3TempoFadeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_e3TempoFade; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterE3TempoFade(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitE3TempoFade(this);
		}
	}

	public final E3TempoFadeContext e3TempoFade() throws RecognitionException {
		E3TempoFadeContext _localctx = new E3TempoFadeContext(_ctx, getState());
		enterRule(_localctx, 152, RULE_e3TempoFade);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(688);
			match(NE3);
			setState(689);
			hexNumber();
			setState(690);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class E4GlobalTransposeContext extends ParserRuleContext {
		public TerminalNode NE4() { return getToken(MmlParser.NE4, 0); }
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public E4GlobalTransposeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_e4GlobalTranspose; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterE4GlobalTranspose(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitE4GlobalTranspose(this);
		}
	}

	public final E4GlobalTransposeContext e4GlobalTranspose() throws RecognitionException {
		E4GlobalTransposeContext _localctx = new E4GlobalTransposeContext(_ctx, getState());
		enterRule(_localctx, 154, RULE_e4GlobalTranspose);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(692);
			match(NE4);
			setState(693);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class E5TremoloContext extends ParserRuleContext {
		public TerminalNode NE5() { return getToken(MmlParser.NE5, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public E5TremoloContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_e5Tremolo; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterE5Tremolo(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitE5Tremolo(this);
		}
	}

	public final E5TremoloContext e5Tremolo() throws RecognitionException {
		E5TremoloContext _localctx = new E5TremoloContext(_ctx, getState());
		enterRule(_localctx, 156, RULE_e5Tremolo);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(695);
			match(NE5);
			setState(696);
			hexNumber();
			setState(697);
			hexNumber();
			setState(698);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class E6SubloopStartContext extends ParserRuleContext {
		public TerminalNode NE6() { return getToken(MmlParser.NE6, 0); }
		public TerminalNode N00() { return getToken(MmlParser.N00, 0); }
		public E6SubloopStartContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_e6SubloopStart; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterE6SubloopStart(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitE6SubloopStart(this);
		}
	}

	public final E6SubloopStartContext e6SubloopStart() throws RecognitionException {
		E6SubloopStartContext _localctx = new E6SubloopStartContext(_ctx, getState());
		enterRule(_localctx, 158, RULE_e6SubloopStart);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(700);
			match(NE6);
			setState(701);
			match(N00);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class E6SubloopEndContext extends ParserRuleContext {
		public TerminalNode NE6() { return getToken(MmlParser.NE6, 0); }
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public E6SubloopEndContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_e6SubloopEnd; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterE6SubloopEnd(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitE6SubloopEnd(this);
		}
	}

	public final E6SubloopEndContext e6SubloopEnd() throws RecognitionException {
		E6SubloopEndContext _localctx = new E6SubloopEndContext(_ctx, getState());
		enterRule(_localctx, 160, RULE_e6SubloopEnd);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(703);
			match(NE6);
			setState(704);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class E7VolumeContext extends ParserRuleContext {
		public TerminalNode NE7() { return getToken(MmlParser.NE7, 0); }
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public E7VolumeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_e7Volume; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterE7Volume(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitE7Volume(this);
		}
	}

	public final E7VolumeContext e7Volume() throws RecognitionException {
		E7VolumeContext _localctx = new E7VolumeContext(_ctx, getState());
		enterRule(_localctx, 162, RULE_e7Volume);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(706);
			match(NE7);
			setState(707);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class E8VolumeFadeContext extends ParserRuleContext {
		public TerminalNode NE8() { return getToken(MmlParser.NE8, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public E8VolumeFadeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_e8VolumeFade; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterE8VolumeFade(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitE8VolumeFade(this);
		}
	}

	public final E8VolumeFadeContext e8VolumeFade() throws RecognitionException {
		E8VolumeFadeContext _localctx = new E8VolumeFadeContext(_ctx, getState());
		enterRule(_localctx, 164, RULE_e8VolumeFade);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(709);
			match(NE8);
			setState(710);
			hexNumber();
			setState(711);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class EbPitchEnvelopeReleaseContext extends ParserRuleContext {
		public TerminalNode NEB() { return getToken(MmlParser.NEB, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public EbPitchEnvelopeReleaseContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_ebPitchEnvelopeRelease; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterEbPitchEnvelopeRelease(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitEbPitchEnvelopeRelease(this);
		}
	}

	public final EbPitchEnvelopeReleaseContext ebPitchEnvelopeRelease() throws RecognitionException {
		EbPitchEnvelopeReleaseContext _localctx = new EbPitchEnvelopeReleaseContext(_ctx, getState());
		enterRule(_localctx, 166, RULE_ebPitchEnvelopeRelease);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(713);
			match(NEB);
			setState(714);
			hexNumber();
			setState(715);
			hexNumber();
			setState(716);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class EcPitchEnvelopeAttackContext extends ParserRuleContext {
		public TerminalNode NEC() { return getToken(MmlParser.NEC, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public EcPitchEnvelopeAttackContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_ecPitchEnvelopeAttack; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterEcPitchEnvelopeAttack(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitEcPitchEnvelopeAttack(this);
		}
	}

	public final EcPitchEnvelopeAttackContext ecPitchEnvelopeAttack() throws RecognitionException {
		EcPitchEnvelopeAttackContext _localctx = new EcPitchEnvelopeAttackContext(_ctx, getState());
		enterRule(_localctx, 168, RULE_ecPitchEnvelopeAttack);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(718);
			match(NEC);
			setState(719);
			hexNumber();
			setState(720);
			hexNumber();
			setState(721);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class EdCustomADSROrGainContext extends ParserRuleContext {
		public EdCustomADSROrGainContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_edCustomADSROrGain; }
	 
		public EdCustomADSROrGainContext() { }
		public void copyFrom(EdCustomADSROrGainContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class EDCustomASDRContext extends EdCustomADSROrGainContext {
		public TerminalNode NED() { return getToken(MmlParser.NED, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public EDCustomASDRContext(EdCustomADSROrGainContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterEDCustomASDR(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitEDCustomASDR(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class EDCustomGAINContext extends EdCustomADSROrGainContext {
		public TerminalNode NED() { return getToken(MmlParser.NED, 0); }
		public TerminalNode N80() { return getToken(MmlParser.N80, 0); }
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public EDCustomGAINContext(EdCustomADSROrGainContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterEDCustomGAIN(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitEDCustomGAIN(this);
		}
	}

	public final EdCustomADSROrGainContext edCustomADSROrGain() throws RecognitionException {
		EdCustomADSROrGainContext _localctx = new EdCustomADSROrGainContext(_ctx, getState());
		enterRule(_localctx, 170, RULE_edCustomADSROrGain);
		try {
			setState(730);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,63,_ctx) ) {
			case 1:
				_localctx = new EDCustomGAINContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(723);
				match(NED);
				setState(724);
				match(N80);
				setState(725);
				hexNumber();
				}
				break;
			case 2:
				_localctx = new EDCustomASDRContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(726);
				match(NED);
				setState(727);
				hexNumber();
				setState(728);
				hexNumber();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class EeTuneChannelContext extends ParserRuleContext {
		public TerminalNode NEE() { return getToken(MmlParser.NEE, 0); }
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public EeTuneChannelContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_eeTuneChannel; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterEeTuneChannel(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitEeTuneChannel(this);
		}
	}

	public final EeTuneChannelContext eeTuneChannel() throws RecognitionException {
		EeTuneChannelContext _localctx = new EeTuneChannelContext(_ctx, getState());
		enterRule(_localctx, 172, RULE_eeTuneChannel);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(732);
			match(NEE);
			setState(733);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class EfEcho1Context extends ParserRuleContext {
		public TerminalNode NEF() { return getToken(MmlParser.NEF, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public EfEcho1Context(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_efEcho1; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterEfEcho1(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitEfEcho1(this);
		}
	}

	public final EfEcho1Context efEcho1() throws RecognitionException {
		EfEcho1Context _localctx = new EfEcho1Context(_ctx, getState());
		enterRule(_localctx, 174, RULE_efEcho1);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(735);
			match(NEF);
			setState(736);
			hexNumber();
			setState(737);
			hexNumber();
			setState(738);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class F0EchoOffContext extends ParserRuleContext {
		public TerminalNode NF0() { return getToken(MmlParser.NF0, 0); }
		public F0EchoOffContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_f0EchoOff; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterF0EchoOff(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitF0EchoOff(this);
		}
	}

	public final F0EchoOffContext f0EchoOff() throws RecognitionException {
		F0EchoOffContext _localctx = new F0EchoOffContext(_ctx, getState());
		enterRule(_localctx, 176, RULE_f0EchoOff);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(740);
			match(NF0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class F1Echo2Context extends ParserRuleContext {
		public TerminalNode NF1() { return getToken(MmlParser.NF1, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public F1Echo2Context(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_f1Echo2; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterF1Echo2(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitF1Echo2(this);
		}
	}

	public final F1Echo2Context f1Echo2() throws RecognitionException {
		F1Echo2Context _localctx = new F1Echo2Context(_ctx, getState());
		enterRule(_localctx, 178, RULE_f1Echo2);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(742);
			match(NF1);
			setState(743);
			hexNumber();
			setState(744);
			hexNumber();
			setState(745);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class F2EchoFadeContext extends ParserRuleContext {
		public TerminalNode NF2() { return getToken(MmlParser.NF2, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public F2EchoFadeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_f2EchoFade; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterF2EchoFade(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitF2EchoFade(this);
		}
	}

	public final F2EchoFadeContext f2EchoFade() throws RecognitionException {
		F2EchoFadeContext _localctx = new F2EchoFadeContext(_ctx, getState());
		enterRule(_localctx, 180, RULE_f2EchoFade);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(747);
			match(NF2);
			setState(748);
			hexNumber();
			setState(749);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class F3SampleLoadContext extends ParserRuleContext {
		public TerminalNode NF3() { return getToken(MmlParser.NF3, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public F3SampleLoadContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_f3SampleLoad; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterF3SampleLoad(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitF3SampleLoad(this);
		}
	}

	public final F3SampleLoadContext f3SampleLoad() throws RecognitionException {
		F3SampleLoadContext _localctx = new F3SampleLoadContext(_ctx, getState());
		enterRule(_localctx, 182, RULE_f3SampleLoad);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(751);
			match(NF3);
			setState(752);
			hexNumber();
			setState(753);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class F4GlobalItemsContext extends ParserRuleContext {
		public F4GlobalItemsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_f4GlobalItems; }
	 
		public F4GlobalItemsContext() { }
		public void copyFrom(F4GlobalItemsContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class F4LightStaccatoContext extends F4GlobalItemsContext {
		public TerminalNode NF4() { return getToken(MmlParser.NF4, 0); }
		public TerminalNode N02() { return getToken(MmlParser.N02, 0); }
		public F4LightStaccatoContext(F4GlobalItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterF4LightStaccato(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitF4LightStaccato(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class F4EnableYoshiDrumsChannel5Context extends F4GlobalItemsContext {
		public TerminalNode NF4() { return getToken(MmlParser.NF4, 0); }
		public TerminalNode N00() { return getToken(MmlParser.N00, 0); }
		public F4EnableYoshiDrumsChannel5Context(F4GlobalItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterF4EnableYoshiDrumsChannel5(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitF4EnableYoshiDrumsChannel5(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class F4NSPCVelocityTableContext extends F4GlobalItemsContext {
		public TerminalNode NF4() { return getToken(MmlParser.NF4, 0); }
		public TerminalNode N08() { return getToken(MmlParser.N08, 0); }
		public F4NSPCVelocityTableContext(F4GlobalItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterF4NSPCVelocityTable(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitF4NSPCVelocityTable(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class F4SNESSyncContext extends F4GlobalItemsContext {
		public TerminalNode NF4() { return getToken(MmlParser.NF4, 0); }
		public TerminalNode N05() { return getToken(MmlParser.N05, 0); }
		public F4SNESSyncContext(F4GlobalItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterF4SNESSync(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitF4SNESSync(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class F4TempoHikeOffContext extends F4GlobalItemsContext {
		public TerminalNode NF4() { return getToken(MmlParser.NF4, 0); }
		public TerminalNode N07() { return getToken(MmlParser.N07, 0); }
		public F4TempoHikeOffContext(F4GlobalItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterF4TempoHikeOff(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitF4TempoHikeOff(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class F4ToggleLegatoContext extends F4GlobalItemsContext {
		public TerminalNode NF4() { return getToken(MmlParser.NF4, 0); }
		public TerminalNode N01() { return getToken(MmlParser.N01, 0); }
		public F4ToggleLegatoContext(F4GlobalItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterF4ToggleLegato(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitF4ToggleLegato(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class F4EnableYoshiDrumsContext extends F4GlobalItemsContext {
		public TerminalNode NF4() { return getToken(MmlParser.NF4, 0); }
		public TerminalNode N06() { return getToken(MmlParser.N06, 0); }
		public F4EnableYoshiDrumsContext(F4GlobalItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterF4EnableYoshiDrums(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitF4EnableYoshiDrums(this);
		}
	}

	public final F4GlobalItemsContext f4GlobalItems() throws RecognitionException {
		F4GlobalItemsContext _localctx = new F4GlobalItemsContext(_ctx, getState());
		enterRule(_localctx, 184, RULE_f4GlobalItems);
		try {
			setState(769);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,64,_ctx) ) {
			case 1:
				_localctx = new F4EnableYoshiDrumsChannel5Context(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(755);
				match(NF4);
				setState(756);
				match(N00);
				}
				break;
			case 2:
				_localctx = new F4ToggleLegatoContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(757);
				match(NF4);
				setState(758);
				match(N01);
				}
				break;
			case 3:
				_localctx = new F4LightStaccatoContext(_localctx);
				enterOuterAlt(_localctx, 3);
				{
				setState(759);
				match(NF4);
				setState(760);
				match(N02);
				}
				break;
			case 4:
				_localctx = new F4SNESSyncContext(_localctx);
				enterOuterAlt(_localctx, 4);
				{
				setState(761);
				match(NF4);
				setState(762);
				match(N05);
				}
				break;
			case 5:
				_localctx = new F4EnableYoshiDrumsContext(_localctx);
				enterOuterAlt(_localctx, 5);
				{
				setState(763);
				match(NF4);
				setState(764);
				match(N06);
				}
				break;
			case 6:
				_localctx = new F4TempoHikeOffContext(_localctx);
				enterOuterAlt(_localctx, 6);
				{
				setState(765);
				match(NF4);
				setState(766);
				match(N07);
				}
				break;
			case 7:
				_localctx = new F4NSPCVelocityTableContext(_localctx);
				enterOuterAlt(_localctx, 7);
				{
				setState(767);
				match(NF4);
				setState(768);
				match(N08);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class F4ChannelItemsContext extends ParserRuleContext {
		public F4ChannelItemsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_f4ChannelItems; }
	 
		public F4ChannelItemsContext() { }
		public void copyFrom(F4ChannelItemsContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class F4RestoreInstrumentContext extends F4ChannelItemsContext {
		public TerminalNode NF4() { return getToken(MmlParser.NF4, 0); }
		public TerminalNode N09() { return getToken(MmlParser.N09, 0); }
		public F4RestoreInstrumentContext(F4ChannelItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterF4RestoreInstrument(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitF4RestoreInstrument(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class F4EchoToggleContext extends F4ChannelItemsContext {
		public TerminalNode NF4() { return getToken(MmlParser.NF4, 0); }
		public TerminalNode N03() { return getToken(MmlParser.N03, 0); }
		public F4EchoToggleContext(F4ChannelItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterF4EchoToggle(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitF4EchoToggle(this);
		}
	}

	public final F4ChannelItemsContext f4ChannelItems() throws RecognitionException {
		F4ChannelItemsContext _localctx = new F4ChannelItemsContext(_ctx, getState());
		enterRule(_localctx, 186, RULE_f4ChannelItems);
		try {
			setState(775);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,65,_ctx) ) {
			case 1:
				_localctx = new F4EchoToggleContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(771);
				match(NF4);
				setState(772);
				match(N03);
				}
				break;
			case 2:
				_localctx = new F4RestoreInstrumentContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(773);
				match(NF4);
				setState(774);
				match(N09);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class F5FIRFilterContext extends ParserRuleContext {
		public TerminalNode NF5() { return getToken(MmlParser.NF5, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public F5FIRFilterContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_f5FIRFilter; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterF5FIRFilter(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitF5FIRFilter(this);
		}
	}

	public final F5FIRFilterContext f5FIRFilter() throws RecognitionException {
		F5FIRFilterContext _localctx = new F5FIRFilterContext(_ctx, getState());
		enterRule(_localctx, 188, RULE_f5FIRFilter);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(777);
			match(NF5);
			setState(778);
			hexNumber();
			setState(779);
			hexNumber();
			setState(780);
			hexNumber();
			setState(781);
			hexNumber();
			setState(782);
			hexNumber();
			setState(783);
			hexNumber();
			setState(784);
			hexNumber();
			setState(785);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class F6DSPWriteContext extends ParserRuleContext {
		public TerminalNode NF6() { return getToken(MmlParser.NF6, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public F6DSPWriteContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_f6DSPWrite; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterF6DSPWrite(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitF6DSPWrite(this);
		}
	}

	public final F6DSPWriteContext f6DSPWrite() throws RecognitionException {
		F6DSPWriteContext _localctx = new F6DSPWriteContext(_ctx, getState());
		enterRule(_localctx, 190, RULE_f6DSPWrite);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(787);
			match(NF6);
			setState(788);
			hexNumber();
			setState(789);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class F8EnableNoiseContext extends ParserRuleContext {
		public TerminalNode NF8() { return getToken(MmlParser.NF8, 0); }
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public F8EnableNoiseContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_f8EnableNoise; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterF8EnableNoise(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitF8EnableNoise(this);
		}
	}

	public final F8EnableNoiseContext f8EnableNoise() throws RecognitionException {
		F8EnableNoiseContext _localctx = new F8EnableNoiseContext(_ctx, getState());
		enterRule(_localctx, 192, RULE_f8EnableNoise);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(791);
			match(NF8);
			setState(792);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class F9DataSendContext extends ParserRuleContext {
		public TerminalNode NF9() { return getToken(MmlParser.NF9, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public F9DataSendContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_f9DataSend; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterF9DataSend(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitF9DataSend(this);
		}
	}

	public final F9DataSendContext f9DataSend() throws RecognitionException {
		F9DataSendContext _localctx = new F9DataSendContext(_ctx, getState());
		enterRule(_localctx, 194, RULE_f9DataSend);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(794);
			match(NF9);
			setState(795);
			hexNumber();
			setState(796);
			hexNumber();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class FaChannelItemsContext extends ParserRuleContext {
		public FaChannelItemsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_faChannelItems; }
	 
		public FaChannelItemsContext() { }
		public void copyFrom(FaChannelItemsContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class FACurrentChannelGainContext extends FaChannelItemsContext {
		public TerminalNode NFA() { return getToken(MmlParser.NFA, 0); }
		public TerminalNode N01() { return getToken(MmlParser.N01, 0); }
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public FACurrentChannelGainContext(FaChannelItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterFACurrentChannelGain(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitFACurrentChannelGain(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class FAPitchModulationContext extends FaChannelItemsContext {
		public TerminalNode NFA() { return getToken(MmlParser.NFA, 0); }
		public TerminalNode N00() { return getToken(MmlParser.N00, 0); }
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public FAPitchModulationContext(FaChannelItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterFAPitchModulation(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitFAPitchModulation(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class FASemitoneTuneContext extends FaChannelItemsContext {
		public TerminalNode NFA() { return getToken(MmlParser.NFA, 0); }
		public TerminalNode N02() { return getToken(MmlParser.N02, 0); }
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public FASemitoneTuneContext(FaChannelItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterFASemitoneTune(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitFASemitoneTune(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class FAAmplifyContext extends FaChannelItemsContext {
		public TerminalNode NFA() { return getToken(MmlParser.NFA, 0); }
		public TerminalNode N03() { return getToken(MmlParser.N03, 0); }
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public FAAmplifyContext(FaChannelItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterFAAmplify(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitFAAmplify(this);
		}
	}

	public final FaChannelItemsContext faChannelItems() throws RecognitionException {
		FaChannelItemsContext _localctx = new FaChannelItemsContext(_ctx, getState());
		enterRule(_localctx, 196, RULE_faChannelItems);
		try {
			setState(810);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,66,_ctx) ) {
			case 1:
				_localctx = new FAPitchModulationContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(798);
				match(NFA);
				setState(799);
				match(N00);
				setState(800);
				hexNumber();
				}
				break;
			case 2:
				_localctx = new FACurrentChannelGainContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(801);
				match(NFA);
				setState(802);
				match(N01);
				setState(803);
				hexNumber();
				}
				break;
			case 3:
				_localctx = new FASemitoneTuneContext(_localctx);
				enterOuterAlt(_localctx, 3);
				{
				setState(804);
				match(NFA);
				setState(805);
				match(N02);
				setState(806);
				hexNumber();
				}
				break;
			case 4:
				_localctx = new FAAmplifyContext(_localctx);
				enterOuterAlt(_localctx, 4);
				{
				setState(807);
				match(NFA);
				setState(808);
				match(N03);
				setState(809);
				hexNumber();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class FaGlobalItemsContext extends ParserRuleContext {
		public FaGlobalItemsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_faGlobalItems; }
	 
		public FaGlobalItemsContext() { }
		public void copyFrom(FaGlobalItemsContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class FAEchoBufferReserveContext extends FaGlobalItemsContext {
		public TerminalNode NFA() { return getToken(MmlParser.NFA, 0); }
		public TerminalNode N04() { return getToken(MmlParser.N04, 0); }
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public FAEchoBufferReserveContext(FaGlobalItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterFAEchoBufferReserve(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitFAEchoBufferReserve(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class FAHotPatchPresetContext extends FaGlobalItemsContext {
		public TerminalNode NFA() { return getToken(MmlParser.NFA, 0); }
		public TerminalNode N7F() { return getToken(MmlParser.N7F, 0); }
		public HexNumberContext hexNumber() {
			return getRuleContext(HexNumberContext.class,0);
		}
		public FAHotPatchPresetContext(FaGlobalItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterFAHotPatchPreset(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitFAHotPatchPreset(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class FAHotPatchToggleBitsContext extends FaGlobalItemsContext {
		public TerminalNode NFA() { return getToken(MmlParser.NFA, 0); }
		public TerminalNode NFE() { return getToken(MmlParser.NFE, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public FAHotPatchToggleBitsContext(FaGlobalItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterFAHotPatchToggleBits(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitFAHotPatchToggleBits(this);
		}
	}

	public final FaGlobalItemsContext faGlobalItems() throws RecognitionException {
		FaGlobalItemsContext _localctx = new FaGlobalItemsContext(_ctx, getState());
		enterRule(_localctx, 198, RULE_faGlobalItems);
		try {
			int _alt;
			setState(827);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,68,_ctx) ) {
			case 1:
				_localctx = new FAEchoBufferReserveContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(812);
				match(NFA);
				setState(813);
				match(N04);
				setState(814);
				hexNumber();
				}
				break;
			case 2:
				_localctx = new FAHotPatchPresetContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(815);
				match(NFA);
				setState(816);
				match(N7F);
				setState(817);
				hexNumber();
				}
				break;
			case 3:
				_localctx = new FAHotPatchToggleBitsContext(_localctx);
				enterOuterAlt(_localctx, 3);
				{
				setState(818);
				match(NFA);
				setState(819);
				match(NFE);
				setState(820);
				hexNumber();
				setState(824);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,67,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(821);
						hexNumber();
						}
						} 
					}
					setState(826);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,67,_ctx);
				}
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class FbItemsContext extends ParserRuleContext {
		public FbItemsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_fbItems; }
	 
		public FbItemsContext() { }
		public void copyFrom(FbItemsContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class FBEnableArgeggioContext extends FbItemsContext {
		public TerminalNode NFB() { return getToken(MmlParser.NFB, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public FBEnableArgeggioContext(FbItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterFBEnableArgeggio(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitFBEnableArgeggio(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class FBTrillContext extends FbItemsContext {
		public TerminalNode NFB() { return getToken(MmlParser.NFB, 0); }
		public TerminalNode N80() { return getToken(MmlParser.N80, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public FBTrillContext(FbItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterFBTrill(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitFBTrill(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class FBGlissandoContext extends FbItemsContext {
		public TerminalNode NFB() { return getToken(MmlParser.NFB, 0); }
		public TerminalNode N81() { return getToken(MmlParser.N81, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public FBGlissandoContext(FbItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterFBGlissando(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitFBGlissando(this);
		}
	}

	public final FbItemsContext fbItems() throws RecognitionException {
		FbItemsContext _localctx = new FbItemsContext(_ctx, getState());
		enterRule(_localctx, 200, RULE_fbItems);
		try {
			setState(843);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,69,_ctx) ) {
			case 1:
				_localctx = new FBTrillContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(829);
				match(NFB);
				setState(830);
				match(N80);
				setState(831);
				hexNumber();
				setState(832);
				hexNumber();
				}
				break;
			case 2:
				_localctx = new FBGlissandoContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(834);
				match(NFB);
				setState(835);
				match(N81);
				setState(836);
				hexNumber();
				setState(837);
				hexNumber();
				}
				break;
			case 3:
				_localctx = new FBEnableArgeggioContext(_localctx);
				enterOuterAlt(_localctx, 3);
				{
				setState(839);
				match(NFB);
				setState(840);
				hexNumber();
				setState(841);
				hexNumber();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class FcItemsContext extends ParserRuleContext {
		public FcItemsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_fcItems; }
	 
		public FcItemsContext() { }
		public void copyFrom(FcItemsContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class FCHexRemoteCommandContext extends FcItemsContext {
		public TerminalNode NFC() { return getToken(MmlParser.NFC, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public FCHexRemoteCommandContext(FcItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterFCHexRemoteCommand(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitFCHexRemoteCommand(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class FCHexRemoteGainContext extends FcItemsContext {
		public TerminalNode NFC() { return getToken(MmlParser.NFC, 0); }
		public List<HexNumberContext> hexNumber() {
			return getRuleContexts(HexNumberContext.class);
		}
		public HexNumberContext hexNumber(int i) {
			return getRuleContext(HexNumberContext.class,i);
		}
		public TerminalNode N01() { return getToken(MmlParser.N01, 0); }
		public FCHexRemoteGainContext(FcItemsContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterFCHexRemoteGain(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitFCHexRemoteGain(this);
		}
	}

	public final FcItemsContext fcItems() throws RecognitionException {
		FcItemsContext _localctx = new FcItemsContext(_ctx, getState());
		enterRule(_localctx, 202, RULE_fcItems);
		try {
			setState(857);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,70,_ctx) ) {
			case 1:
				_localctx = new FCHexRemoteGainContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(845);
				match(NFC);
				setState(846);
				hexNumber();
				setState(847);
				match(N01);
				setState(848);
				hexNumber();
				setState(849);
				hexNumber();
				}
				break;
			case 2:
				_localctx = new FCHexRemoteCommandContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(851);
				match(NFC);
				setState(852);
				hexNumber();
				setState(853);
				hexNumber();
				setState(854);
				hexNumber();
				setState(855);
				hexNumber();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class FdTremoloOffContext extends ParserRuleContext {
		public TerminalNode NFD() { return getToken(MmlParser.NFD, 0); }
		public FdTremoloOffContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_fdTremoloOff; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterFdTremoloOff(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitFdTremoloOff(this);
		}
	}

	public final FdTremoloOffContext fdTremoloOff() throws RecognitionException {
		FdTremoloOffContext _localctx = new FdTremoloOffContext(_ctx, getState());
		enterRule(_localctx, 204, RULE_fdTremoloOff);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(859);
			match(NFD);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class FePitchEnvelopeOffContext extends ParserRuleContext {
		public TerminalNode NFE() { return getToken(MmlParser.NFE, 0); }
		public FePitchEnvelopeOffContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_fePitchEnvelopeOff; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterFePitchEnvelopeOff(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitFePitchEnvelopeOff(this);
		}
	}

	public final FePitchEnvelopeOffContext fePitchEnvelopeOff() throws RecognitionException {
		FePitchEnvelopeOffContext _localctx = new FePitchEnvelopeOffContext(_ctx, getState());
		enterRule(_localctx, 206, RULE_fePitchEnvelopeOff);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(861);
			match(NFE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class HexNumberContext extends ParserRuleContext {
		public TerminalNode N00() { return getToken(MmlParser.N00, 0); }
		public TerminalNode N01() { return getToken(MmlParser.N01, 0); }
		public TerminalNode N02() { return getToken(MmlParser.N02, 0); }
		public TerminalNode N03() { return getToken(MmlParser.N03, 0); }
		public TerminalNode N04() { return getToken(MmlParser.N04, 0); }
		public TerminalNode N05() { return getToken(MmlParser.N05, 0); }
		public TerminalNode N06() { return getToken(MmlParser.N06, 0); }
		public TerminalNode N07() { return getToken(MmlParser.N07, 0); }
		public TerminalNode N08() { return getToken(MmlParser.N08, 0); }
		public TerminalNode N09() { return getToken(MmlParser.N09, 0); }
		public TerminalNode N7F() { return getToken(MmlParser.N7F, 0); }
		public TerminalNode N80() { return getToken(MmlParser.N80, 0); }
		public TerminalNode N81() { return getToken(MmlParser.N81, 0); }
		public TerminalNode NFE() { return getToken(MmlParser.NFE, 0); }
		public TerminalNode NDA() { return getToken(MmlParser.NDA, 0); }
		public TerminalNode NDB() { return getToken(MmlParser.NDB, 0); }
		public TerminalNode NDC() { return getToken(MmlParser.NDC, 0); }
		public TerminalNode NDD() { return getToken(MmlParser.NDD, 0); }
		public TerminalNode NDE() { return getToken(MmlParser.NDE, 0); }
		public TerminalNode NDF() { return getToken(MmlParser.NDF, 0); }
		public TerminalNode NE0() { return getToken(MmlParser.NE0, 0); }
		public TerminalNode NE1() { return getToken(MmlParser.NE1, 0); }
		public TerminalNode NE2() { return getToken(MmlParser.NE2, 0); }
		public TerminalNode NE3() { return getToken(MmlParser.NE3, 0); }
		public TerminalNode NE4() { return getToken(MmlParser.NE4, 0); }
		public TerminalNode NE5() { return getToken(MmlParser.NE5, 0); }
		public TerminalNode NE6() { return getToken(MmlParser.NE6, 0); }
		public TerminalNode NE7() { return getToken(MmlParser.NE7, 0); }
		public TerminalNode NE8() { return getToken(MmlParser.NE8, 0); }
		public TerminalNode NE9() { return getToken(MmlParser.NE9, 0); }
		public TerminalNode NEA() { return getToken(MmlParser.NEA, 0); }
		public TerminalNode NEB() { return getToken(MmlParser.NEB, 0); }
		public TerminalNode NEC() { return getToken(MmlParser.NEC, 0); }
		public TerminalNode NED() { return getToken(MmlParser.NED, 0); }
		public TerminalNode NEE() { return getToken(MmlParser.NEE, 0); }
		public TerminalNode NEF() { return getToken(MmlParser.NEF, 0); }
		public TerminalNode NF0() { return getToken(MmlParser.NF0, 0); }
		public TerminalNode NF1() { return getToken(MmlParser.NF1, 0); }
		public TerminalNode NF2() { return getToken(MmlParser.NF2, 0); }
		public TerminalNode NF3() { return getToken(MmlParser.NF3, 0); }
		public TerminalNode NF4() { return getToken(MmlParser.NF4, 0); }
		public TerminalNode NF5() { return getToken(MmlParser.NF5, 0); }
		public TerminalNode NF6() { return getToken(MmlParser.NF6, 0); }
		public TerminalNode NF7() { return getToken(MmlParser.NF7, 0); }
		public TerminalNode NF8() { return getToken(MmlParser.NF8, 0); }
		public TerminalNode NF9() { return getToken(MmlParser.NF9, 0); }
		public TerminalNode NFA() { return getToken(MmlParser.NFA, 0); }
		public TerminalNode NFB() { return getToken(MmlParser.NFB, 0); }
		public TerminalNode NFC() { return getToken(MmlParser.NFC, 0); }
		public TerminalNode NFD() { return getToken(MmlParser.NFD, 0); }
		public TerminalNode HexNumber() { return getToken(MmlParser.HexNumber, 0); }
		public HexNumberContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_hexNumber; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).enterHexNumber(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof MmlListener ) ((MmlListener)listener).exitHexNumber(this);
		}
	}

	public final HexNumberContext hexNumber() throws RecognitionException {
		HexNumberContext _localctx = new HexNumberContext(_ctx, getState());
		enterRule(_localctx, 208, RULE_hexNumber);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(863);
			_la = _input.LA(1);
			if ( !(((((_la - 79)) & ~0x3f) == 0 && ((1L << (_la - 79)) & 5629499534213119L) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static final String _serializedATN =
		"\u0004\u0001\u0085\u0362\u0002\u0000\u0007\u0000\u0002\u0001\u0007\u0001"+
		"\u0002\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002\u0004\u0007\u0004"+
		"\u0002\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0002\u0007\u0007\u0007"+
		"\u0002\b\u0007\b\u0002\t\u0007\t\u0002\n\u0007\n\u0002\u000b\u0007\u000b"+
		"\u0002\f\u0007\f\u0002\r\u0007\r\u0002\u000e\u0007\u000e\u0002\u000f\u0007"+
		"\u000f\u0002\u0010\u0007\u0010\u0002\u0011\u0007\u0011\u0002\u0012\u0007"+
		"\u0012\u0002\u0013\u0007\u0013\u0002\u0014\u0007\u0014\u0002\u0015\u0007"+
		"\u0015\u0002\u0016\u0007\u0016\u0002\u0017\u0007\u0017\u0002\u0018\u0007"+
		"\u0018\u0002\u0019\u0007\u0019\u0002\u001a\u0007\u001a\u0002\u001b\u0007"+
		"\u001b\u0002\u001c\u0007\u001c\u0002\u001d\u0007\u001d\u0002\u001e\u0007"+
		"\u001e\u0002\u001f\u0007\u001f\u0002 \u0007 \u0002!\u0007!\u0002\"\u0007"+
		"\"\u0002#\u0007#\u0002$\u0007$\u0002%\u0007%\u0002&\u0007&\u0002\'\u0007"+
		"\'\u0002(\u0007(\u0002)\u0007)\u0002*\u0007*\u0002+\u0007+\u0002,\u0007"+
		",\u0002-\u0007-\u0002.\u0007.\u0002/\u0007/\u00020\u00070\u00021\u0007"+
		"1\u00022\u00072\u00023\u00073\u00024\u00074\u00025\u00075\u00026\u0007"+
		"6\u00027\u00077\u00028\u00078\u00029\u00079\u0002:\u0007:\u0002;\u0007"+
		";\u0002<\u0007<\u0002=\u0007=\u0002>\u0007>\u0002?\u0007?\u0002@\u0007"+
		"@\u0002A\u0007A\u0002B\u0007B\u0002C\u0007C\u0002D\u0007D\u0002E\u0007"+
		"E\u0002F\u0007F\u0002G\u0007G\u0002H\u0007H\u0002I\u0007I\u0002J\u0007"+
		"J\u0002K\u0007K\u0002L\u0007L\u0002M\u0007M\u0002N\u0007N\u0002O\u0007"+
		"O\u0002P\u0007P\u0002Q\u0007Q\u0002R\u0007R\u0002S\u0007S\u0002T\u0007"+
		"T\u0002U\u0007U\u0002V\u0007V\u0002W\u0007W\u0002X\u0007X\u0002Y\u0007"+
		"Y\u0002Z\u0007Z\u0002[\u0007[\u0002\\\u0007\\\u0002]\u0007]\u0002^\u0007"+
		"^\u0002_\u0007_\u0002`\u0007`\u0002a\u0007a\u0002b\u0007b\u0002c\u0007"+
		"c\u0002d\u0007d\u0002e\u0007e\u0002f\u0007f\u0002g\u0007g\u0002h\u0007"+
		"h\u0001\u0000\u0004\u0000\u00d4\b\u0000\u000b\u0000\f\u0000\u00d5\u0001"+
		"\u0000\u0003\u0000\u00d9\b\u0000\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0003\u0001\u00e9"+
		"\b\u0001\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001"+
		"\u0002\u0001\u0002\u0001\u0002\u0003\u0002\u00f3\b\u0002\u0001\u0003\u0001"+
		"\u0003\u0001\u0003\u0001\u0003\u0001\u0003\u0001\u0004\u0005\u0004\u00fb"+
		"\b\u0004\n\u0004\f\u0004\u00fe\t\u0004\u0001\u0004\u0005\u0004\u0101\b"+
		"\u0004\n\u0004\f\u0004\u0104\t\u0004\u0001\u0005\u0001\u0005\u0001\u0005"+
		"\u0005\u0005\u0109\b\u0005\n\u0005\f\u0005\u010c\t\u0005\u0001\u0005\u0001"+
		"\u0005\u0001\u0006\u0001\u0006\u0004\u0006\u0112\b\u0006\u000b\u0006\f"+
		"\u0006\u0113\u0001\u0006\u0001\u0006\u0004\u0006\u0118\b\u0006\u000b\u0006"+
		"\f\u0006\u0119\u0001\u0006\u0001\u0006\u0004\u0006\u011e\b\u0006\u000b"+
		"\u0006\f\u0006\u011f\u0003\u0006\u0122\b\u0006\u0001\u0007\u0001\u0007"+
		"\u0001\u0007\u0005\u0007\u0127\b\u0007\n\u0007\f\u0007\u012a\t\u0007\u0001"+
		"\u0007\u0001\u0007\u0001\b\u0001\b\u0001\b\u0001\b\u0001\b\u0001\b\u0001"+
		"\b\u0001\b\u0001\b\u0001\b\u0003\b\u0138\b\b\u0001\t\u0001\t\u0001\t\u0001"+
		"\n\u0001\n\u0001\n\u0001\u000b\u0001\u000b\u0001\f\u0001\f\u0001\f\u0001"+
		"\f\u0005\f\u0146\b\f\n\f\f\f\u0149\t\f\u0001\f\u0001\f\u0001\f\u0003\f"+
		"\u014e\b\f\u0001\r\u0001\r\u0001\r\u0001\r\u0001\r\u0001\r\u0001\r\u0001"+
		"\r\u0001\r\u0001\r\u0003\r\u015a\b\r\u0001\u000e\u0001\u000e\u0001\u000e"+
		"\u0001\u000e\u0003\u000e\u0160\b\u000e\u0001\u000f\u0001\u000f\u0001\u0010"+
		"\u0001\u0010\u0001\u0011\u0001\u0011\u0001\u0012\u0001\u0012\u0005\u0012"+
		"\u016a\b\u0012\n\u0012\f\u0012\u016d\t\u0012\u0001\u0013\u0001\u0013\u0001"+
		"\u0014\u0001\u0014\u0001\u0014\u0001\u0014\u0001\u0014\u0003\u0014\u0176"+
		"\b\u0014\u0001\u0015\u0001\u0015\u0001\u0015\u0001\u0015\u0001\u0015\u0001"+
		"\u0015\u0001\u0015\u0001\u0015\u0001\u0015\u0001\u0015\u0001\u0015\u0001"+
		"\u0015\u0001\u0015\u0001\u0015\u0001\u0015\u0001\u0015\u0001\u0015\u0001"+
		"\u0015\u0001\u0015\u0003\u0015\u018b\b\u0015\u0001\u0016\u0001\u0016\u0001"+
		"\u0017\u0001\u0017\u0001\u0018\u0001\u0018\u0001\u0019\u0001\u0019\u0001"+
		"\u001a\u0001\u001a\u0001\u001b\u0001\u001b\u0001\u001c\u0001\u001c\u0001"+
		"\u001c\u0003\u001c\u019c\b\u001c\u0003\u001c\u019e\b\u001c\u0001\u001d"+
		"\u0001\u001d\u0003\u001d\u01a2\b\u001d\u0001\u001e\u0001\u001e\u0001\u001f"+
		"\u0001\u001f\u0001\u001f\u0003\u001f\u01a9\b\u001f\u0003\u001f\u01ab\b"+
		"\u001f\u0001 \u0001 \u0003 \u01af\b \u0001!\u0001!\u0001!\u0004!\u01b4"+
		"\b!\u000b!\f!\u01b5\u0001\"\u0001\"\u0001\"\u0003\"\u01bb\b\"\u0001\""+
		"\u0001\"\u0003\"\u01bf\b\"\u0001\"\u0001\"\u0003\"\u01c3\b\"\u0001\"\u0001"+
		"\"\u0001#\u0001#\u0001$\u0001$\u0001$\u0003$\u01cc\b$\u0003$\u01ce\b$"+
		"\u0001%\u0001%\u0001%\u0003%\u01d3\b%\u0003%\u01d5\b%\u0001&\u0001&\u0003"+
		"&\u01d9\b&\u0001\'\u0001\'\u0001(\u0001(\u0001)\u0001)\u0001*\u0001*\u0003"+
		"*\u01e3\b*\u0001+\u0001+\u0003+\u01e7\b+\u0001,\u0001,\u0003,\u01eb\b"+
		",\u0001-\u0001-\u0001-\u0003-\u01f0\b-\u0001.\u0001.\u0005.\u01f4\b.\n"+
		".\f.\u01f7\t.\u0001.\u0001.\u0003.\u01fb\b.\u0001/\u0001/\u0001/\u0001"+
		"/\u0003/\u0201\b/\u00010\u00030\u0204\b0\u00010\u00010\u00050\u0208\b"+
		"0\n0\f0\u020b\t0\u00010\u00010\u00030\u020f\b0\u00011\u00011\u00011\u0001"+
		"1\u00031\u0215\b1\u00012\u00012\u00052\u0219\b2\n2\f2\u021c\t2\u00012"+
		"\u00012\u00032\u0220\b2\u00013\u00013\u00013\u00033\u0225\b3\u00014\u0003"+
		"4\u0228\b4\u00014\u00014\u00054\u022c\b4\n4\f4\u022f\t4\u00014\u00014"+
		"\u00034\u0233\b4\u00015\u00015\u00015\u00035\u0238\b5\u00016\u00016\u0001"+
		"6\u00046\u023d\b6\u000b6\f6\u023e\u00016\u00016\u00017\u00017\u00017\u0001"+
		"7\u00017\u00017\u00017\u00017\u00017\u00017\u00037\u024d\b7\u00018\u0001"+
		"8\u00038\u0251\b8\u00019\u00019\u0001:\u0001:\u0001;\u0001;\u0003;\u0259"+
		"\b;\u0001<\u0001<\u0001=\u0001=\u0003=\u025f\b=\u0001>\u0001>\u0001?\u0001"+
		"?\u0001?\u0001?\u0001?\u0001?\u0001?\u0001?\u0001?\u0001?\u0003?\u026d"+
		"\b?\u0001@\u0001@\u0001@\u0001@\u0001@\u0001@\u0001@\u0001@\u0001@\u0001"+
		"@\u0001@\u0001@\u0001@\u0001@\u0001@\u0001@\u0001@\u0001@\u0003@\u0281"+
		"\b@\u0001A\u0001A\u0001A\u0001B\u0001B\u0001B\u0001C\u0001C\u0001C\u0001"+
		"C\u0001D\u0001D\u0001D\u0001D\u0003D\u0291\bD\u0001E\u0001E\u0001E\u0005"+
		"E\u0296\bE\nE\fE\u0299\tE\u0001E\u0001E\u0001F\u0001F\u0001F\u0001F\u0001"+
		"F\u0001G\u0001G\u0001G\u0001H\u0001H\u0001I\u0001I\u0001I\u0001J\u0001"+
		"J\u0001J\u0001J\u0001K\u0001K\u0001K\u0001L\u0001L\u0001L\u0001L\u0001"+
		"M\u0001M\u0001M\u0001N\u0001N\u0001N\u0001N\u0001N\u0001O\u0001O\u0001"+
		"O\u0001P\u0001P\u0001P\u0001Q\u0001Q\u0001Q\u0001R\u0001R\u0001R\u0001"+
		"R\u0001S\u0001S\u0001S\u0001S\u0001S\u0001T\u0001T\u0001T\u0001T\u0001"+
		"T\u0001U\u0001U\u0001U\u0001U\u0001U\u0001U\u0001U\u0003U\u02db\bU\u0001"+
		"V\u0001V\u0001V\u0001W\u0001W\u0001W\u0001W\u0001W\u0001X\u0001X\u0001"+
		"Y\u0001Y\u0001Y\u0001Y\u0001Y\u0001Z\u0001Z\u0001Z\u0001Z\u0001[\u0001"+
		"[\u0001[\u0001[\u0001\\\u0001\\\u0001\\\u0001\\\u0001\\\u0001\\\u0001"+
		"\\\u0001\\\u0001\\\u0001\\\u0001\\\u0001\\\u0001\\\u0001\\\u0003\\\u0302"+
		"\b\\\u0001]\u0001]\u0001]\u0001]\u0003]\u0308\b]\u0001^\u0001^\u0001^"+
		"\u0001^\u0001^\u0001^\u0001^\u0001^\u0001^\u0001^\u0001_\u0001_\u0001"+
		"_\u0001_\u0001`\u0001`\u0001`\u0001a\u0001a\u0001a\u0001a\u0001b\u0001"+
		"b\u0001b\u0001b\u0001b\u0001b\u0001b\u0001b\u0001b\u0001b\u0001b\u0001"+
		"b\u0003b\u032b\bb\u0001c\u0001c\u0001c\u0001c\u0001c\u0001c\u0001c\u0001"+
		"c\u0001c\u0001c\u0005c\u0337\bc\nc\fc\u033a\tc\u0003c\u033c\bc\u0001d"+
		"\u0001d\u0001d\u0001d\u0001d\u0001d\u0001d\u0001d\u0001d\u0001d\u0001"+
		"d\u0001d\u0001d\u0001d\u0003d\u034c\bd\u0001e\u0001e\u0001e\u0001e\u0001"+
		"e\u0001e\u0001e\u0001e\u0001e\u0001e\u0001e\u0001e\u0003e\u035a\be\u0001"+
		"f\u0001f\u0001g\u0001g\u0001h\u0001h\u0001h\u0000\u0000i\u0000\u0002\u0004"+
		"\u0006\b\n\f\u000e\u0010\u0012\u0014\u0016\u0018\u001a\u001c\u001e \""+
		"$&(*,.02468:<>@BDFHJLNPRTVXZ\\^`bdfhjlnprtvxz|~\u0080\u0082\u0084\u0086"+
		"\u0088\u008a\u008c\u008e\u0090\u0092\u0094\u0096\u0098\u009a\u009c\u009e"+
		"\u00a0\u00a2\u00a4\u00a6\u00a8\u00aa\u00ac\u00ae\u00b0\u00b2\u00b4\u00b6"+
		"\u00b8\u00ba\u00bc\u00be\u00c0\u00c2\u00c4\u00c6\u00c8\u00ca\u00cc\u00ce"+
		"\u00d0\u0000\u0003\u0002\u000066\u0081\u0081\u0001\u0000 !\u0002\u0000"+
		"O\u0080\u0083\u0083\u03a1\u0000\u00d8\u0001\u0000\u0000\u0000\u0002\u00e8"+
		"\u0001\u0000\u0000\u0000\u0004\u00f2\u0001\u0000\u0000\u0000\u0006\u00f4"+
		"\u0001\u0000\u0000\u0000\b\u00fc\u0001\u0000\u0000\u0000\n\u0105\u0001"+
		"\u0000\u0000\u0000\f\u0121\u0001\u0000\u0000\u0000\u000e\u0123\u0001\u0000"+
		"\u0000\u0000\u0010\u0137\u0001\u0000\u0000\u0000\u0012\u0139\u0001\u0000"+
		"\u0000\u0000\u0014\u013c\u0001\u0000\u0000\u0000\u0016\u013f\u0001\u0000"+
		"\u0000\u0000\u0018\u014d\u0001\u0000\u0000\u0000\u001a\u0159\u0001\u0000"+
		"\u0000\u0000\u001c\u015f\u0001\u0000\u0000\u0000\u001e\u0161\u0001\u0000"+
		"\u0000\u0000 \u0163\u0001\u0000\u0000\u0000\"\u0165\u0001\u0000\u0000"+
		"\u0000$\u0167\u0001\u0000\u0000\u0000&\u016e\u0001\u0000\u0000\u0000("+
		"\u0175\u0001\u0000\u0000\u0000*\u018a\u0001\u0000\u0000\u0000,\u018c\u0001"+
		"\u0000\u0000\u0000.\u018e\u0001\u0000\u0000\u00000\u0190\u0001\u0000\u0000"+
		"\u00002\u0192\u0001\u0000\u0000\u00004\u0194\u0001\u0000\u0000\u00006"+
		"\u0196\u0001\u0000\u0000\u00008\u019d\u0001\u0000\u0000\u0000:\u01a1\u0001"+
		"\u0000\u0000\u0000<\u01a3\u0001\u0000\u0000\u0000>\u01aa\u0001\u0000\u0000"+
		"\u0000@\u01ae\u0001\u0000\u0000\u0000B\u01b0\u0001\u0000\u0000\u0000D"+
		"\u01b7\u0001\u0000\u0000\u0000F\u01c6\u0001\u0000\u0000\u0000H\u01cd\u0001"+
		"\u0000\u0000\u0000J\u01d4\u0001\u0000\u0000\u0000L\u01d8\u0001\u0000\u0000"+
		"\u0000N\u01da\u0001\u0000\u0000\u0000P\u01dc\u0001\u0000\u0000\u0000R"+
		"\u01de\u0001\u0000\u0000\u0000T\u01e2\u0001\u0000\u0000\u0000V\u01e6\u0001"+
		"\u0000\u0000\u0000X\u01ea\u0001\u0000\u0000\u0000Z\u01ef\u0001\u0000\u0000"+
		"\u0000\\\u01f1\u0001\u0000\u0000\u0000^\u0200\u0001\u0000\u0000\u0000"+
		"`\u0203\u0001\u0000\u0000\u0000b\u0214\u0001\u0000\u0000\u0000d\u0216"+
		"\u0001\u0000\u0000\u0000f\u0224\u0001\u0000\u0000\u0000h\u0227\u0001\u0000"+
		"\u0000\u0000j\u0237\u0001\u0000\u0000\u0000l\u0239\u0001\u0000\u0000\u0000"+
		"n\u024c\u0001\u0000\u0000\u0000p\u024e\u0001\u0000\u0000\u0000r\u0252"+
		"\u0001\u0000\u0000\u0000t\u0254\u0001\u0000\u0000\u0000v\u0256\u0001\u0000"+
		"\u0000\u0000x\u025a\u0001\u0000\u0000\u0000z\u025e\u0001\u0000\u0000\u0000"+
		"|\u0260\u0001\u0000\u0000\u0000~\u026c\u0001\u0000\u0000\u0000\u0080\u0280"+
		"\u0001\u0000\u0000\u0000\u0082\u0282\u0001\u0000\u0000\u0000\u0084\u0285"+
		"\u0001\u0000\u0000\u0000\u0086\u0288\u0001\u0000\u0000\u0000\u0088\u028c"+
		"\u0001\u0000\u0000\u0000\u008a\u0297\u0001\u0000\u0000\u0000\u008c\u029c"+
		"\u0001\u0000\u0000\u0000\u008e\u02a1\u0001\u0000\u0000\u0000\u0090\u02a4"+
		"\u0001\u0000\u0000\u0000\u0092\u02a6\u0001\u0000\u0000\u0000\u0094\u02a9"+
		"\u0001\u0000\u0000\u0000\u0096\u02ad\u0001\u0000\u0000\u0000\u0098\u02b0"+
		"\u0001\u0000\u0000\u0000\u009a\u02b4\u0001\u0000\u0000\u0000\u009c\u02b7"+
		"\u0001\u0000\u0000\u0000\u009e\u02bc\u0001\u0000\u0000\u0000\u00a0\u02bf"+
		"\u0001\u0000\u0000\u0000\u00a2\u02c2\u0001\u0000\u0000\u0000\u00a4\u02c5"+
		"\u0001\u0000\u0000\u0000\u00a6\u02c9\u0001\u0000\u0000\u0000\u00a8\u02ce"+
		"\u0001\u0000\u0000\u0000\u00aa\u02da\u0001\u0000\u0000\u0000\u00ac\u02dc"+
		"\u0001\u0000\u0000\u0000\u00ae\u02df\u0001\u0000\u0000\u0000\u00b0\u02e4"+
		"\u0001\u0000\u0000\u0000\u00b2\u02e6\u0001\u0000\u0000\u0000\u00b4\u02eb"+
		"\u0001\u0000\u0000\u0000\u00b6\u02ef\u0001\u0000\u0000\u0000\u00b8\u0301"+
		"\u0001\u0000\u0000\u0000\u00ba\u0307\u0001\u0000\u0000\u0000\u00bc\u0309"+
		"\u0001\u0000\u0000\u0000\u00be\u0313\u0001\u0000\u0000\u0000\u00c0\u0317"+
		"\u0001\u0000\u0000\u0000\u00c2\u031a\u0001\u0000\u0000\u0000\u00c4\u032a"+
		"\u0001\u0000\u0000\u0000\u00c6\u033b\u0001\u0000\u0000\u0000\u00c8\u034b"+
		"\u0001\u0000\u0000\u0000\u00ca\u0359\u0001\u0000\u0000\u0000\u00cc\u035b"+
		"\u0001\u0000\u0000\u0000\u00ce\u035d\u0001\u0000\u0000\u0000\u00d0\u035f"+
		"\u0001\u0000\u0000\u0000\u00d2\u00d4\u0003\u0002\u0001\u0000\u00d3\u00d2"+
		"\u0001\u0000\u0000\u0000\u00d4\u00d5\u0001\u0000\u0000\u0000\u00d5\u00d3"+
		"\u0001\u0000\u0000\u0000\u00d5\u00d6\u0001\u0000\u0000\u0000\u00d6\u00d9"+
		"\u0001\u0000\u0000\u0000\u00d7\u00d9\u0005\u0000\u0000\u0001\u00d8\u00d3"+
		"\u0001\u0000\u0000\u0000\u00d8\u00d7\u0001\u0000\u0000\u0000\u00d9\u0001"+
		"\u0001\u0000\u0000\u0000\u00da\u00e9\u0003\u0004\u0002\u0000\u00db\u00e9"+
		"\u0003$\u0012\u0000\u00dc\u00e9\u0003l6\u0000\u00dd\u00e9\u0003F#\u0000"+
		"\u00de\u00e9\u0003H$\u0000\u00df\u00e9\u0003J%\u0000\u00e0\u00e9\u0003"+
		"|>\u0000\u00e1\u00e9\u0003x<\u0000\u00e2\u00e9\u0005\u001e\u0000\u0000"+
		"\u00e3\u00e9\u0003~?\u0000\u00e4\u00e9\u0003\u00d0h\u0000\u00e5\u00e9"+
		"\u0003&\u0013\u0000\u00e6\u00e9\u0003l6\u0000\u00e7\u00e9\u0003P(\u0000"+
		"\u00e8\u00da\u0001\u0000\u0000\u0000\u00e8\u00db\u0001\u0000\u0000\u0000"+
		"\u00e8\u00dc\u0001\u0000\u0000\u0000\u00e8\u00dd\u0001\u0000\u0000\u0000"+
		"\u00e8\u00de\u0001\u0000\u0000\u0000\u00e8\u00df\u0001\u0000\u0000\u0000"+
		"\u00e8\u00e0\u0001\u0000\u0000\u0000\u00e8\u00e1\u0001\u0000\u0000\u0000"+
		"\u00e8\u00e2\u0001\u0000\u0000\u0000\u00e8\u00e3\u0001\u0000\u0000\u0000"+
		"\u00e8\u00e4\u0001\u0000\u0000\u0000\u00e8\u00e5\u0001\u0000\u0000\u0000"+
		"\u00e8\u00e6\u0001\u0000\u0000\u0000\u00e8\u00e7\u0001\u0000\u0000\u0000"+
		"\u00e9\u0003\u0001\u0000\u0000\u0000\u00ea\u00f3\u0003\u001c\u000e\u0000"+
		"\u00eb\u00f3\u0003\u000e\u0007\u0000\u00ec\u00f3\u0003\u0006\u0003\u0000"+
		"\u00ed\u00f3\u0003\n\u0005\u0000\u00ee\u00f3\u0003\u0014\n\u0000\u00ef"+
		"\u00f3\u0003\u0012\t\u0000\u00f0\u00f3\u0003\u0016\u000b\u0000\u00f1\u00f3"+
		"\u0003\u0018\f\u0000\u00f2\u00ea\u0001\u0000\u0000\u0000\u00f2\u00eb\u0001"+
		"\u0000\u0000\u0000\u00f2\u00ec\u0001\u0000\u0000\u0000\u00f2\u00ed\u0001"+
		"\u0000\u0000\u0000\u00f2\u00ee\u0001\u0000\u0000\u0000\u00f2\u00ef\u0001"+
		"\u0000\u0000\u0000\u00f2\u00f0\u0001\u0000\u0000\u0000\u00f2\u00f1\u0001"+
		"\u0000\u0000\u0000\u00f3\u0005\u0001\u0000\u0000\u0000\u00f4\u00f5\u0005"+
		":\u0000\u0000\u00f5\u00f6\u0005\b\u0000\u0000\u00f6\u00f7\u0003\b\u0004"+
		"\u0000\u00f7\u00f8\u0005\t\u0000\u0000\u00f8\u0007\u0001\u0000\u0000\u0000"+
		"\u00f9\u00fb\u0005N\u0000\u0000\u00fa\u00f9\u0001\u0000\u0000\u0000\u00fb"+
		"\u00fe\u0001\u0000\u0000\u0000\u00fc\u00fa\u0001\u0000\u0000\u0000\u00fc"+
		"\u00fd\u0001\u0000\u0000\u0000\u00fd\u0102\u0001\u0000\u0000\u0000\u00fe"+
		"\u00fc\u0001\u0000\u0000\u0000\u00ff\u0101\u0005\u001e\u0000\u0000\u0100"+
		"\u00ff\u0001\u0000\u0000\u0000\u0101\u0104\u0001\u0000\u0000\u0000\u0102"+
		"\u0100\u0001\u0000\u0000\u0000\u0102\u0103\u0001\u0000\u0000\u0000\u0103"+
		"\t\u0001\u0000\u0000\u0000\u0104\u0102\u0001\u0000\u0000\u0000\u0105\u0106"+
		"\u0005;\u0000\u0000\u0106\u010a\u0005\b\u0000\u0000\u0107\u0109\u0003"+
		"\f\u0006\u0000\u0108\u0107\u0001\u0000\u0000\u0000\u0109\u010c\u0001\u0000"+
		"\u0000\u0000\u010a\u0108\u0001\u0000\u0000\u0000\u010a\u010b\u0001\u0000"+
		"\u0000\u0000\u010b\u010d\u0001\u0000\u0000\u0000\u010c\u010a\u0001\u0000"+
		"\u0000\u0000\u010d\u010e\u0005\t\u0000\u0000\u010e\u000b\u0001\u0000\u0000"+
		"\u0000\u010f\u0111\u0005\u001e\u0000\u0000\u0110\u0112\u0003\u00d0h\u0000"+
		"\u0111\u0110\u0001\u0000\u0000\u0000\u0112\u0113\u0001\u0000\u0000\u0000"+
		"\u0113\u0111\u0001\u0000\u0000\u0000\u0113\u0114\u0001\u0000\u0000\u0000"+
		"\u0114\u0122\u0001\u0000\u0000\u0000\u0115\u0117\u0003L&\u0000\u0116\u0118"+
		"\u0003\u00d0h\u0000\u0117\u0116\u0001\u0000\u0000\u0000\u0118\u0119\u0001"+
		"\u0000\u0000\u0000\u0119\u0117\u0001\u0000\u0000\u0000\u0119\u011a\u0001"+
		"\u0000\u0000\u0000\u011a\u0122\u0001\u0000\u0000\u0000\u011b\u011d\u0003"+
		"6\u001b\u0000\u011c\u011e\u0003\u00d0h\u0000\u011d\u011c\u0001\u0000\u0000"+
		"\u0000\u011e\u011f\u0001\u0000\u0000\u0000\u011f\u011d\u0001\u0000\u0000"+
		"\u0000\u011f\u0120\u0001\u0000\u0000\u0000\u0120\u0122\u0001\u0000\u0000"+
		"\u0000\u0121\u010f\u0001\u0000\u0000\u0000\u0121\u0115\u0001\u0000\u0000"+
		"\u0000\u0121\u011b\u0001\u0000\u0000\u0000\u0122\r\u0001\u0000\u0000\u0000"+
		"\u0123\u0124\u0005<\u0000\u0000\u0124\u0128\u0005\b\u0000\u0000\u0125"+
		"\u0127\u0003\u0010\b\u0000\u0126\u0125\u0001\u0000\u0000\u0000\u0127\u012a"+
		"\u0001\u0000\u0000\u0000\u0128\u0126\u0001\u0000\u0000\u0000\u0128\u0129"+
		"\u0001\u0000\u0000\u0000\u0129\u012b\u0001\u0000\u0000\u0000\u012a\u0128"+
		"\u0001\u0000\u0000\u0000\u012b\u012c\u0005\t\u0000\u0000\u012c\u000f\u0001"+
		"\u0000\u0000\u0000\u012d\u012e\u0005=\u0000\u0000\u012e\u0138\u0005\u001e"+
		"\u0000\u0000\u012f\u0130\u0005>\u0000\u0000\u0130\u0138\u0005\u001e\u0000"+
		"\u0000\u0131\u0132\u0005?\u0000\u0000\u0132\u0138\u0005\u001e\u0000\u0000"+
		"\u0133\u0134\u0005@\u0000\u0000\u0134\u0138\u0005\u001e\u0000\u0000\u0135"+
		"\u0136\u0005A\u0000\u0000\u0136\u0138\u0005\u001e\u0000\u0000\u0137\u012d"+
		"\u0001\u0000\u0000\u0000\u0137\u012f\u0001\u0000\u0000\u0000\u0137\u0131"+
		"\u0001\u0000\u0000\u0000\u0137\u0133\u0001\u0000\u0000\u0000\u0137\u0135"+
		"\u0001\u0000\u0000\u0000\u0138\u0011\u0001\u0000\u0000\u0000\u0139\u013a"+
		"\u0005B\u0000\u0000\u013a\u013b\u0003\u00d0h\u0000\u013b\u0013\u0001\u0000"+
		"\u0000\u0000\u013c\u013d\u0005C\u0000\u0000\u013d\u013e\u0005\u001e\u0000"+
		"\u0000\u013e\u0015\u0001\u0000\u0000\u0000\u013f\u0140\u0005D\u0000\u0000"+
		"\u0140\u0017\u0001\u0000\u0000\u0000\u0141\u0142\u0005E\u0000\u0000\u0142"+
		"\u0147\u0005\b\u0000\u0000\u0143\u0144\u0005\u0001\u0000\u0000\u0144\u0146"+
		"\u0003\u001a\r\u0000\u0145\u0143\u0001\u0000\u0000\u0000\u0146\u0149\u0001"+
		"\u0000\u0000\u0000\u0147\u0145\u0001\u0000\u0000\u0000\u0147\u0148\u0001"+
		"\u0000\u0000\u0000\u0148\u014a\u0001\u0000\u0000\u0000\u0149\u0147\u0001"+
		"\u0000\u0000\u0000\u014a\u014e\u0005\t\u0000\u0000\u014b\u014c\u0005E"+
		"\u0000\u0000\u014c\u014e\u0003\u001a\r\u0000\u014d\u0141\u0001\u0000\u0000"+
		"\u0000\u014d\u014b\u0001\u0000\u0000\u0000\u014e\u0019\u0001\u0000\u0000"+
		"\u0000\u014f\u015a\u0005G\u0000\u0000\u0150\u0151\u0005H\u0000\u0000\u0151"+
		"\u015a\u0005\u0081\u0000\u0000\u0152\u015a\u0005I\u0000\u0000\u0153\u015a"+
		"\u0005J\u0000\u0000\u0154\u015a\u0005K\u0000\u0000\u0155\u015a\u0005L"+
		"\u0000\u0000\u0156\u0157\u0005\u001e\u0000\u0000\u0157\u015a\u0005\u0081"+
		"\u0000\u0000\u0158\u015a\u0005\u001e\u0000\u0000\u0159\u014f\u0001\u0000"+
		"\u0000\u0000\u0159\u0150\u0001\u0000\u0000\u0000\u0159\u0152\u0001\u0000"+
		"\u0000\u0000\u0159\u0153\u0001\u0000\u0000\u0000\u0159\u0154\u0001\u0000"+
		"\u0000\u0000\u0159\u0155\u0001\u0000\u0000\u0000\u0159\u0156\u0001\u0000"+
		"\u0000\u0000\u0159\u0158\u0001\u0000\u0000\u0000\u015a\u001b\u0001\u0000"+
		"\u0000\u0000\u015b\u015c\u00057\u0000\u0000\u015c\u0160\u0003\"\u0011"+
		"\u0000\u015d\u0160\u0003\u001e\u000f\u0000\u015e\u0160\u0003 \u0010\u0000"+
		"\u015f\u015b\u0001\u0000\u0000\u0000\u015f\u015d\u0001\u0000\u0000\u0000"+
		"\u015f\u015e\u0001\u0000\u0000\u0000\u0160\u001d\u0001\u0000\u0000\u0000"+
		"\u0161\u0162\u00058\u0000\u0000\u0162\u001f\u0001\u0000\u0000\u0000\u0163"+
		"\u0164\u00059\u0000\u0000\u0164!\u0001\u0000\u0000\u0000\u0165\u0166\u0007"+
		"\u0000\u0000\u0000\u0166#\u0001\u0000\u0000\u0000\u0167\u016b\u0005M\u0000"+
		"\u0000\u0168\u016a\u0003(\u0014\u0000\u0169\u0168\u0001\u0000\u0000\u0000"+
		"\u016a\u016d\u0001\u0000\u0000\u0000\u016b\u0169\u0001\u0000\u0000\u0000"+
		"\u016b\u016c\u0001\u0000\u0000\u0000\u016c%\u0001\u0000\u0000\u0000\u016d"+
		"\u016b\u0001\u0000\u0000\u0000\u016e\u016f\u0005\u0019\u0000\u0000\u016f"+
		"\'\u0001\u0000\u0000\u0000\u0170\u0176\u0003*\u0015\u0000\u0171\u0176"+
		"\u0003T*\u0000\u0172\u0176\u0003z=\u0000\u0173\u0176\u0003\u0080@\u0000"+
		"\u0174\u0176\u0003\u00d0h\u0000\u0175\u0170\u0001\u0000\u0000\u0000\u0175"+
		"\u0171\u0001\u0000\u0000\u0000\u0175\u0172\u0001\u0000\u0000\u0000\u0175"+
		"\u0173\u0001\u0000\u0000\u0000\u0175\u0174\u0001\u0000\u0000\u0000\u0176"+
		")\u0001\u0000\u0000\u0000\u0177\u018b\u0003B!\u0000\u0178\u018b\u0003"+
		",\u0016\u0000\u0179\u018b\u0003.\u0017\u0000\u017a\u018b\u00030\u0018"+
		"\u0000\u017b\u018b\u00032\u0019\u0000\u017c\u018b\u00034\u001a\u0000\u017d"+
		"\u018b\u00036\u001b\u0000\u017e\u018b\u0003D\"\u0000\u017f\u018b\u0003"+
		"8\u001c\u0000\u0180\u018b\u0003:\u001d\u0000\u0181\u018b\u0003L&\u0000"+
		"\u0182\u018b\u0003<\u001e\u0000\u0183\u018b\u0003>\u001f\u0000\u0184\u018b"+
		"\u0003@ \u0000\u0185\u018b\u0003J%\u0000\u0186\u018b\u0003&\u0013\u0000"+
		"\u0187\u018b\u0003N\'\u0000\u0188\u018b\u0003P(\u0000\u0189\u018b\u0003"+
		"R)\u0000\u018a\u0177\u0001\u0000\u0000\u0000\u018a\u0178\u0001\u0000\u0000"+
		"\u0000\u018a\u0179\u0001\u0000\u0000\u0000\u018a\u017a\u0001\u0000\u0000"+
		"\u0000\u018a\u017b\u0001\u0000\u0000\u0000\u018a\u017c\u0001\u0000\u0000"+
		"\u0000\u018a\u017d\u0001\u0000\u0000\u0000\u018a\u017e\u0001\u0000\u0000"+
		"\u0000\u018a\u017f\u0001\u0000\u0000\u0000\u018a\u0180\u0001\u0000\u0000"+
		"\u0000\u018a\u0181\u0001\u0000\u0000\u0000\u018a\u0182\u0001\u0000\u0000"+
		"\u0000\u018a\u0183\u0001\u0000\u0000\u0000\u018a\u0184\u0001\u0000\u0000"+
		"\u0000\u018a\u0185\u0001\u0000\u0000\u0000\u018a\u0186\u0001\u0000\u0000"+
		"\u0000\u018a\u0187\u0001\u0000\u0000\u0000\u018a\u0188\u0001\u0000\u0000"+
		"\u0000\u018a\u0189\u0001\u0000\u0000\u0000\u018b+\u0001\u0000\u0000\u0000"+
		"\u018c\u018d\u0005 \u0000\u0000\u018d-\u0001\u0000\u0000\u0000\u018e\u018f"+
		"\u0005!\u0000\u0000\u018f/\u0001\u0000\u0000\u0000\u0190\u0191\u0005\""+
		"\u0000\u0000\u01911\u0001\u0000\u0000\u0000\u0192\u0193\u0005\u0017\u0000"+
		"\u0000\u01933\u0001\u0000\u0000\u0000\u0194\u0195\u0005\u0016\u0000\u0000"+
		"\u01955\u0001\u0000\u0000\u0000\u0196\u0197\u0005#\u0000\u0000\u01977"+
		"\u0001\u0000\u0000\u0000\u0198\u019e\u0005%\u0000\u0000\u0199\u019c\u0003"+
		"\u00a2Q\u0000\u019a\u019c\u0003\u00a4R\u0000\u019b\u0199\u0001\u0000\u0000"+
		"\u0000\u019b\u019a\u0001\u0000\u0000\u0000\u019c\u019e\u0001\u0000\u0000"+
		"\u0000\u019d\u0198\u0001\u0000\u0000\u0000\u019d\u019b\u0001\u0000\u0000"+
		"\u0000\u019e9\u0001\u0000\u0000\u0000\u019f\u01a2\u0005&\u0000\u0000\u01a0"+
		"\u01a2\u0003\u00acV\u0000\u01a1\u019f\u0001\u0000\u0000\u0000\u01a1\u01a0"+
		"\u0001\u0000\u0000\u0000\u01a2;\u0001\u0000\u0000\u0000\u01a3\u01a4\u0005"+
		"(\u0000\u0000\u01a4=\u0001\u0000\u0000\u0000\u01a5\u01ab\u0005*\u0000"+
		"\u0000\u01a6\u01a9\u0003\u0084B\u0000\u01a7\u01a9\u0003\u0086C\u0000\u01a8"+
		"\u01a6\u0001\u0000\u0000\u0000\u01a8\u01a7\u0001\u0000\u0000\u0000\u01a9"+
		"\u01ab\u0001\u0000\u0000\u0000\u01aa\u01a5\u0001\u0000\u0000\u0000\u01aa"+
		"\u01a8\u0001\u0000\u0000\u0000\u01ab?\u0001\u0000\u0000\u0000\u01ac\u01af"+
		"\u0005+\u0000\u0000\u01ad\u01af\u0003\u008cF\u0000\u01ae\u01ac\u0001\u0000"+
		"\u0000\u0000\u01ae\u01ad\u0001\u0000\u0000\u0000\u01afA\u0001\u0000\u0000"+
		"\u0000\u01b0\u01b3\u0007\u0001\u0000\u0000\u01b1\u01b2\u0005\u0004\u0000"+
		"\u0000\u01b2\u01b4\u0007\u0001\u0000\u0000\u01b3\u01b1\u0001\u0000\u0000"+
		"\u0000\u01b4\u01b5\u0001\u0000\u0000\u0000\u01b5\u01b3\u0001\u0000\u0000"+
		"\u0000\u01b5\u01b6\u0001\u0000\u0000\u0000\u01b6C\u0001\u0000\u0000\u0000"+
		"\u01b7\u01ba\u0005\b\u0000\u0000\u01b8\u01bb\u0003,\u0016\u0000\u01b9"+
		"\u01bb\u0003.\u0017\u0000\u01ba\u01b8\u0001\u0000\u0000\u0000\u01ba\u01b9"+
		"\u0001\u0000\u0000\u0000\u01bb\u01be\u0001\u0000\u0000\u0000\u01bc\u01bf"+
		"\u0003,\u0016\u0000\u01bd\u01bf\u0003.\u0017\u0000\u01be\u01bc\u0001\u0000"+
		"\u0000\u0000\u01be\u01bd\u0001\u0000\u0000\u0000\u01bf\u01c2\u0001\u0000"+
		"\u0000\u0000\u01c0\u01c3\u0003,\u0016\u0000\u01c1\u01c3\u0003.\u0017\u0000"+
		"\u01c2\u01c0\u0001\u0000\u0000\u0000\u01c2\u01c1\u0001\u0000\u0000\u0000"+
		"\u01c3\u01c4\u0001\u0000\u0000\u0000\u01c4\u01c5\u0005\t\u0000\u0000\u01c5"+
		"E\u0001\u0000\u0000\u0000\u01c6\u01c7\u0005\'\u0000\u0000\u01c7G\u0001"+
		"\u0000\u0000\u0000\u01c8\u01ce\u0005)\u0000\u0000\u01c9\u01cc\u0003\u0092"+
		"I\u0000\u01ca\u01cc\u0003\u0094J\u0000\u01cb\u01c9\u0001\u0000\u0000\u0000"+
		"\u01cb\u01ca\u0001\u0000\u0000\u0000\u01cc\u01ce\u0001\u0000\u0000\u0000"+
		"\u01cd\u01c8\u0001\u0000\u0000\u0000\u01cd\u01cb\u0001\u0000\u0000\u0000"+
		"\u01ceI\u0001\u0000\u0000\u0000\u01cf\u01d5\u0005$\u0000\u0000\u01d0\u01d3"+
		"\u0003\u0096K\u0000\u01d1\u01d3\u0003\u0098L\u0000\u01d2\u01d0\u0001\u0000"+
		"\u0000\u0000\u01d2\u01d1\u0001\u0000\u0000\u0000\u01d3\u01d5\u0001\u0000"+
		"\u0000\u0000\u01d4\u01cf\u0001\u0000\u0000\u0000\u01d4\u01d2\u0001\u0000"+
		"\u0000\u0000\u01d5K\u0001\u0000\u0000\u0000\u01d6\u01d9\u0005.\u0000\u0000"+
		"\u01d7\u01d9\u0003\u0082A\u0000\u01d8\u01d6\u0001\u0000\u0000\u0000\u01d8"+
		"\u01d7\u0001\u0000\u0000\u0000\u01d9M\u0001\u0000\u0000\u0000\u01da\u01db"+
		"\u0005,\u0000\u0000\u01dbO\u0001\u0000\u0000\u0000\u01dc\u01dd\u0005-"+
		"\u0000\u0000\u01ddQ\u0001\u0000\u0000\u0000\u01de\u01df\u0005/\u0000\u0000"+
		"\u01dfS\u0001\u0000\u0000\u0000\u01e0\u01e3\u0003V+\u0000\u01e1\u01e3"+
		"\u0003X,\u0000\u01e2\u01e0\u0001\u0000\u0000\u0000\u01e2\u01e1\u0001\u0000"+
		"\u0000\u0000\u01e3U\u0001\u0000\u0000\u0000\u01e4\u01e7\u0003\\.\u0000"+
		"\u01e5\u01e7\u0003`0\u0000\u01e6\u01e4\u0001\u0000\u0000\u0000\u01e6\u01e5"+
		"\u0001\u0000\u0000\u0000\u01e7W\u0001\u0000\u0000\u0000\u01e8\u01eb\u0003"+
		"p8\u0000\u01e9\u01eb\u0003Z-\u0000\u01ea\u01e8\u0001\u0000\u0000\u0000"+
		"\u01ea\u01e9\u0001\u0000\u0000\u0000\u01ebY\u0001\u0000\u0000\u0000\u01ec"+
		"\u01f0\u0003r9\u0000\u01ed\u01f0\u0003v;\u0000\u01ee\u01f0\u0003t:\u0000"+
		"\u01ef\u01ec\u0001\u0000\u0000\u0000\u01ef\u01ed\u0001\u0000\u0000\u0000"+
		"\u01ef\u01ee\u0001\u0000\u0000\u0000\u01f0[\u0001\u0000\u0000\u0000\u01f1"+
		"\u01f5\u0005\u000e\u0000\u0000\u01f2\u01f4\u0003^/\u0000\u01f3\u01f2\u0001"+
		"\u0000\u0000\u0000\u01f4\u01f7\u0001\u0000\u0000\u0000\u01f5\u01f3\u0001"+
		"\u0000\u0000\u0000\u01f5\u01f6\u0001\u0000\u0000\u0000\u01f6\u01f8\u0001"+
		"\u0000\u0000\u0000\u01f7\u01f5\u0001\u0000\u0000\u0000\u01f8\u01fa\u0005"+
		"\u000f\u0000\u0000\u01f9\u01fb\u0005\u0081\u0000\u0000\u01fa\u01f9\u0001"+
		"\u0000\u0000\u0000\u01fa\u01fb\u0001\u0000\u0000\u0000\u01fb]\u0001\u0000"+
		"\u0000\u0000\u01fc\u0201\u0003*\u0015\u0000\u01fd\u0201\u0003h4\u0000"+
		"\u01fe\u0201\u0003X,\u0000\u01ff\u0201\u0003\u00d0h\u0000\u0200\u01fc"+
		"\u0001\u0000\u0000\u0000\u0200\u01fd\u0001\u0000\u0000\u0000\u0200\u01fe"+
		"\u0001\u0000\u0000\u0000\u0200\u01ff\u0001\u0000\u0000\u0000\u0201_\u0001"+
		"\u0000\u0000\u0000\u0202\u0204\u00050\u0000\u0000\u0203\u0202\u0001\u0000"+
		"\u0000\u0000\u0203\u0204\u0001\u0000\u0000\u0000\u0204\u0205\u0001\u0000"+
		"\u0000\u0000\u0205\u0209\u0005\f\u0000\u0000\u0206\u0208\u0003b1\u0000"+
		"\u0207\u0206\u0001\u0000\u0000\u0000\u0208\u020b\u0001\u0000\u0000\u0000"+
		"\u0209\u0207\u0001\u0000\u0000\u0000\u0209\u020a\u0001\u0000\u0000\u0000"+
		"\u020a\u020c\u0001\u0000\u0000\u0000\u020b\u0209\u0001\u0000\u0000\u0000"+
		"\u020c\u020e\u0005\r\u0000\u0000\u020d\u020f\u0005\u0081\u0000\u0000\u020e"+
		"\u020d\u0001\u0000\u0000\u0000\u020e\u020f\u0001\u0000\u0000\u0000\u020f"+
		"a\u0001\u0000\u0000\u0000\u0210\u0215\u0003*\u0015\u0000\u0211\u0215\u0003"+
		"d2\u0000\u0212\u0215\u0003Z-\u0000\u0213\u0215\u0003\u00d0h\u0000\u0214"+
		"\u0210\u0001\u0000\u0000\u0000\u0214\u0211\u0001\u0000\u0000\u0000\u0214"+
		"\u0212\u0001\u0000\u0000\u0000\u0214\u0213\u0001\u0000\u0000\u0000\u0215"+
		"c\u0001\u0000\u0000\u0000\u0216\u021a\u0005\u000e\u0000\u0000\u0217\u0219"+
		"\u0003f3\u0000\u0218\u0217\u0001\u0000\u0000\u0000\u0219\u021c\u0001\u0000"+
		"\u0000\u0000\u021a\u0218\u0001\u0000\u0000\u0000\u021a\u021b\u0001\u0000"+
		"\u0000\u0000\u021b\u021d\u0001\u0000\u0000\u0000\u021c\u021a\u0001\u0000"+
		"\u0000\u0000\u021d\u021f\u0005\u000f\u0000\u0000\u021e\u0220\u0005\u0081"+
		"\u0000\u0000\u021f\u021e\u0001\u0000\u0000\u0000\u021f\u0220\u0001\u0000"+
		"\u0000\u0000\u0220e\u0001\u0000\u0000\u0000\u0221\u0225\u0003*\u0015\u0000"+
		"\u0222\u0225\u0003X,\u0000\u0223\u0225\u0003\u00d0h\u0000\u0224\u0221"+
		"\u0001\u0000\u0000\u0000\u0224\u0222\u0001\u0000\u0000\u0000\u0224\u0223"+
		"\u0001\u0000\u0000\u0000\u0225g\u0001\u0000\u0000\u0000\u0226\u0228\u0005"+
		"0\u0000\u0000\u0227\u0226\u0001\u0000\u0000\u0000\u0227\u0228\u0001\u0000"+
		"\u0000\u0000\u0228\u0229\u0001\u0000\u0000\u0000\u0229\u022d\u0005\f\u0000"+
		"\u0000\u022a\u022c\u0003j5\u0000\u022b\u022a\u0001\u0000\u0000\u0000\u022c"+
		"\u022f\u0001\u0000\u0000\u0000\u022d\u022b\u0001\u0000\u0000\u0000\u022d"+
		"\u022e\u0001\u0000\u0000\u0000\u022e\u0230\u0001\u0000\u0000\u0000\u022f"+
		"\u022d\u0001\u0000\u0000\u0000\u0230\u0232\u0005\r\u0000\u0000\u0231\u0233"+
		"\u0005\u0081\u0000\u0000\u0232\u0231\u0001\u0000\u0000\u0000\u0232\u0233"+
		"\u0001\u0000\u0000\u0000\u0233i\u0001\u0000\u0000\u0000\u0234\u0238\u0003"+
		"*\u0015\u0000\u0235\u0238\u0003Z-\u0000\u0236\u0238\u0003\u00d0h\u0000"+
		"\u0237\u0234\u0001\u0000\u0000\u0000\u0237\u0235\u0001\u0000\u0000\u0000"+
		"\u0237\u0236\u0001\u0000\u0000\u0000\u0238k\u0001\u0000\u0000\u0000\u0239"+
		"\u023a\u00051\u0000\u0000\u023a\u023c\u0005\f\u0000\u0000\u023b\u023d"+
		"\u0003n7\u0000\u023c\u023b\u0001\u0000\u0000\u0000\u023d\u023e\u0001\u0000"+
		"\u0000\u0000\u023e\u023c\u0001\u0000\u0000\u0000\u023e\u023f\u0001\u0000"+
		"\u0000\u0000\u023f\u0240\u0001\u0000\u0000\u0000\u0240\u0241\u0005\r\u0000"+
		"\u0000\u0241m\u0001\u0000\u0000\u0000\u0242\u024d\u00030\u0018\u0000\u0243"+
		"\u024d\u00032\u0019\u0000\u0244\u024d\u00034\u001a\u0000\u0245\u024d\u0003"+
		"8\u001c\u0000\u0246\u024d\u0003:\u001d\u0000\u0247\u024d\u0003<\u001e"+
		"\u0000\u0248\u024d\u0003>\u001f\u0000\u0249\u024d\u0003@ \u0000\u024a"+
		"\u024d\u0003J%\u0000\u024b\u024d\u0003\u00d0h\u0000\u024c\u0242\u0001"+
		"\u0000\u0000\u0000\u024c\u0243\u0001\u0000\u0000\u0000\u024c\u0244\u0001"+
		"\u0000\u0000\u0000\u024c\u0245\u0001\u0000\u0000\u0000\u024c\u0246\u0001"+
		"\u0000\u0000\u0000\u024c\u0247\u0001\u0000\u0000\u0000\u024c\u0248\u0001"+
		"\u0000\u0000\u0000\u024c\u0249\u0001\u0000\u0000\u0000\u024c\u024a\u0001"+
		"\u0000\u0000\u0000\u024c\u024b\u0001\u0000\u0000\u0000\u024do\u0001\u0000"+
		"\u0000\u0000\u024e\u0250\u00050\u0000\u0000\u024f\u0251\u0005\u0081\u0000"+
		"\u0000\u0250\u024f\u0001\u0000\u0000\u0000\u0250\u0251\u0001\u0000\u0000"+
		"\u0000\u0251q\u0001\u0000\u0000\u0000\u0252\u0253\u00053\u0000\u0000\u0253"+
		"s\u0001\u0000\u0000\u0000\u0254\u0255\u00052\u0000\u0000\u0255u\u0001"+
		"\u0000\u0000\u0000\u0256\u0258\u0005\u0007\u0000\u0000\u0257\u0259\u0005"+
		"\u0081\u0000\u0000\u0258\u0257\u0001\u0000\u0000\u0000\u0258\u0259\u0001"+
		"\u0000\u0000\u0000\u0259w\u0001\u0000\u0000\u0000\u025a\u025b\u0005\u001a"+
		"\u0000\u0000\u025by\u0001\u0000\u0000\u0000\u025c\u025f\u00055\u0000\u0000"+
		"\u025d\u025f\u0003\u00b6[\u0000\u025e\u025c\u0001\u0000\u0000\u0000\u025e"+
		"\u025d\u0001\u0000\u0000\u0000\u025f{\u0001\u0000\u0000\u0000\u0260\u0261"+
		"\u0005\u001d\u0000\u0000\u0261}\u0001\u0000\u0000\u0000\u0262\u026d\u0003"+
		"\u00bc^\u0000\u0263\u026d\u0003\u009aM\u0000\u0264\u026d\u0003\u00b8\\"+
		"\u0000\u0265\u026d\u0003\u00aeW\u0000\u0266\u026d\u0003\u00b2Y\u0000\u0267"+
		"\u026d\u0003\u00b0X\u0000\u0268\u026d\u0003\u00b4Z\u0000\u0269\u026d\u0003"+
		"\u00be_\u0000\u026a\u026d\u0003\u00c2a\u0000\u026b\u026d\u0003\u00c6c"+
		"\u0000\u026c\u0262\u0001\u0000\u0000\u0000\u026c\u0263\u0001\u0000\u0000"+
		"\u0000\u026c\u0264\u0001\u0000\u0000\u0000\u026c\u0265\u0001\u0000\u0000"+
		"\u0000\u026c\u0266\u0001\u0000\u0000\u0000\u026c\u0267\u0001\u0000\u0000"+
		"\u0000\u026c\u0268\u0001\u0000\u0000\u0000\u026c\u0269\u0001\u0000\u0000"+
		"\u0000\u026c\u026a\u0001\u0000\u0000\u0000\u026c\u026b\u0001\u0000\u0000"+
		"\u0000\u026d\u007f\u0001\u0000\u0000\u0000\u026e\u0281\u0003\u009cN\u0000"+
		"\u026f\u0281\u0003\u00c8d\u0000\u0270\u0281\u0003\u008eG\u0000\u0271\u0281"+
		"\u0003\u0090H\u0000\u0272\u0281\u0003\u00a6S\u0000\u0273\u0281\u0003\u00a8"+
		"T\u0000\u0274\u0281\u0003\u00aaU\u0000\u0275\u0281\u0003\u00b0X\u0000"+
		"\u0276\u0281\u0003\u00b4Z\u0000\u0277\u0281\u0003\u00ba]\u0000\u0278\u0281"+
		"\u0003\u00be_\u0000\u0279\u0281\u0003\u00c0`\u0000\u027a\u0281\u0003\u00c2"+
		"a\u0000\u027b\u0281\u0003\u00c4b\u0000\u027c\u0281\u0003\u00c8d\u0000"+
		"\u027d\u0281\u0003\u00cae\u0000\u027e\u0281\u0003\u00ccf\u0000\u027f\u0281"+
		"\u0003\u00ceg\u0000\u0280\u026e\u0001\u0000\u0000\u0000\u0280\u026f\u0001"+
		"\u0000\u0000\u0000\u0280\u0270\u0001\u0000\u0000\u0000\u0280\u0271\u0001"+
		"\u0000\u0000\u0000\u0280\u0272\u0001\u0000\u0000\u0000\u0280\u0273\u0001"+
		"\u0000\u0000\u0000\u0280\u0274\u0001\u0000\u0000\u0000\u0280\u0275\u0001"+
		"\u0000\u0000\u0000\u0280\u0276\u0001\u0000\u0000\u0000\u0280\u0277\u0001"+
		"\u0000\u0000\u0000\u0280\u0278\u0001\u0000\u0000\u0000\u0280\u0279\u0001"+
		"\u0000\u0000\u0000\u0280\u027a\u0001\u0000\u0000\u0000\u0280\u027b\u0001"+
		"\u0000\u0000\u0000\u0280\u027c\u0001\u0000\u0000\u0000\u0280\u027d\u0001"+
		"\u0000\u0000\u0000\u0280\u027e\u0001\u0000\u0000\u0000\u0280\u027f\u0001"+
		"\u0000\u0000\u0000\u0281\u0081\u0001\u0000\u0000\u0000\u0282\u0283\u0005"+
		"]\u0000\u0000\u0283\u0284\u0005\u0081\u0000\u0000\u0284\u0083\u0001\u0000"+
		"\u0000\u0000\u0285\u0286\u0005^\u0000\u0000\u0286\u0287\u0003\u00d0h\u0000"+
		"\u0287\u0085\u0001\u0000\u0000\u0000\u0288\u0289\u0005_\u0000\u0000\u0289"+
		"\u028a\u0003\u00d0h\u0000\u028a\u028b\u0003\u00d0h\u0000\u028b\u0087\u0001"+
		"\u0000\u0000\u0000\u028c\u028d\u0005`\u0000\u0000\u028d\u0290\u0003\u00d0"+
		"h\u0000\u028e\u0291\u0003\u00d0h\u0000\u028f\u0291\u0003\u008aE\u0000"+
		"\u0290\u028e\u0001\u0000\u0000\u0000\u0290\u028f\u0001\u0000\u0000\u0000"+
		"\u0291\u0089\u0001\u0000\u0000\u0000\u0292\u0296\u00030\u0018\u0000\u0293"+
		"\u0296\u00034\u001a\u0000\u0294\u0296\u00032\u0019\u0000\u0295\u0292\u0001"+
		"\u0000\u0000\u0000\u0295\u0293\u0001\u0000\u0000\u0000\u0295\u0294\u0001"+
		"\u0000\u0000\u0000\u0296\u0299\u0001\u0000\u0000\u0000\u0297\u0295\u0001"+
		"\u0000\u0000\u0000\u0297\u0298\u0001\u0000\u0000\u0000\u0298\u029a\u0001"+
		"\u0000\u0000\u0000\u0299\u0297\u0001\u0000\u0000\u0000\u029a\u029b\u0003"+
		",\u0016\u0000\u029b\u008b\u0001\u0000\u0000\u0000\u029c\u029d\u0005a\u0000"+
		"\u0000\u029d\u029e\u0003\u00d0h\u0000\u029e\u029f\u0003\u00d0h\u0000\u029f"+
		"\u02a0\u0003\u00d0h\u0000\u02a0\u008d\u0001\u0000\u0000\u0000\u02a1\u02a2"+
		"\u0005m\u0000\u0000\u02a2\u02a3\u0003\u00d0h\u0000\u02a3\u008f\u0001\u0000"+
		"\u0000\u0000\u02a4\u02a5\u0005b\u0000\u0000\u02a5\u0091\u0001\u0000\u0000"+
		"\u0000\u02a6\u02a7\u0005c\u0000\u0000\u02a7\u02a8\u0003\u00d0h\u0000\u02a8"+
		"\u0093\u0001\u0000\u0000\u0000\u02a9\u02aa\u0005d\u0000\u0000\u02aa\u02ab"+
		"\u0003\u00d0h\u0000\u02ab\u02ac\u0003\u00d0h\u0000\u02ac\u0095\u0001\u0000"+
		"\u0000\u0000\u02ad\u02ae\u0005e\u0000\u0000\u02ae\u02af\u0003\u00d0h\u0000"+
		"\u02af\u0097\u0001\u0000\u0000\u0000\u02b0\u02b1\u0005f\u0000\u0000\u02b1"+
		"\u02b2\u0003\u00d0h\u0000\u02b2\u02b3\u0003\u00d0h\u0000\u02b3\u0099\u0001"+
		"\u0000\u0000\u0000\u02b4\u02b5\u0005g\u0000\u0000\u02b5\u02b6\u0003\u00d0"+
		"h\u0000\u02b6\u009b\u0001\u0000\u0000\u0000\u02b7\u02b8\u0005h\u0000\u0000"+
		"\u02b8\u02b9\u0003\u00d0h\u0000\u02b9\u02ba\u0003\u00d0h\u0000\u02ba\u02bb"+
		"\u0003\u00d0h\u0000\u02bb\u009d\u0001\u0000\u0000\u0000\u02bc\u02bd\u0005"+
		"i\u0000\u0000\u02bd\u02be\u0005O\u0000\u0000\u02be\u009f\u0001\u0000\u0000"+
		"\u0000\u02bf\u02c0\u0005i\u0000\u0000\u02c0\u02c1\u0003\u00d0h\u0000\u02c1"+
		"\u00a1\u0001\u0000\u0000\u0000\u02c2\u02c3\u0005j\u0000\u0000\u02c3\u02c4"+
		"\u0003\u00d0h\u0000\u02c4\u00a3\u0001\u0000\u0000\u0000\u02c5\u02c6\u0005"+
		"k\u0000\u0000\u02c6\u02c7\u0003\u00d0h\u0000\u02c7\u02c8\u0003\u00d0h"+
		"\u0000\u02c8\u00a5\u0001\u0000\u0000\u0000\u02c9\u02ca\u0005n\u0000\u0000"+
		"\u02ca\u02cb\u0003\u00d0h\u0000\u02cb\u02cc\u0003\u00d0h\u0000\u02cc\u02cd"+
		"\u0003\u00d0h\u0000\u02cd\u00a7\u0001\u0000\u0000\u0000\u02ce\u02cf\u0005"+
		"o\u0000\u0000\u02cf\u02d0\u0003\u00d0h\u0000\u02d0\u02d1\u0003\u00d0h"+
		"\u0000\u02d1\u02d2\u0003\u00d0h\u0000\u02d2\u00a9\u0001\u0000\u0000\u0000"+
		"\u02d3\u02d4\u0005p\u0000\u0000\u02d4\u02d5\u0005Z\u0000\u0000\u02d5\u02db"+
		"\u0003\u00d0h\u0000\u02d6\u02d7\u0005p\u0000\u0000\u02d7\u02d8\u0003\u00d0"+
		"h\u0000\u02d8\u02d9\u0003\u00d0h\u0000\u02d9\u02db\u0001\u0000\u0000\u0000"+
		"\u02da\u02d3\u0001\u0000\u0000\u0000\u02da\u02d6\u0001\u0000\u0000\u0000"+
		"\u02db\u00ab\u0001\u0000\u0000\u0000\u02dc\u02dd\u0005q\u0000\u0000\u02dd"+
		"\u02de\u0003\u00d0h\u0000\u02de\u00ad\u0001\u0000\u0000\u0000\u02df\u02e0"+
		"\u0005r\u0000\u0000\u02e0\u02e1\u0003\u00d0h\u0000\u02e1\u02e2\u0003\u00d0"+
		"h\u0000\u02e2\u02e3\u0003\u00d0h\u0000\u02e3\u00af\u0001\u0000\u0000\u0000"+
		"\u02e4\u02e5\u0005s\u0000\u0000\u02e5\u00b1\u0001\u0000\u0000\u0000\u02e6"+
		"\u02e7\u0005t\u0000\u0000\u02e7\u02e8\u0003\u00d0h\u0000\u02e8\u02e9\u0003"+
		"\u00d0h\u0000\u02e9\u02ea\u0003\u00d0h\u0000\u02ea\u00b3\u0001\u0000\u0000"+
		"\u0000\u02eb\u02ec\u0005u\u0000\u0000\u02ec\u02ed\u0003\u00d0h\u0000\u02ed"+
		"\u02ee\u0003\u00d0h\u0000\u02ee\u00b5\u0001\u0000\u0000\u0000\u02ef\u02f0"+
		"\u0005v\u0000\u0000\u02f0\u02f1\u0003\u00d0h\u0000\u02f1\u02f2\u0003\u00d0"+
		"h\u0000\u02f2\u00b7\u0001\u0000\u0000\u0000\u02f3\u02f4\u0005w\u0000\u0000"+
		"\u02f4\u0302\u0005O\u0000\u0000\u02f5\u02f6\u0005w\u0000\u0000\u02f6\u0302"+
		"\u0005P\u0000\u0000\u02f7\u02f8\u0005w\u0000\u0000\u02f8\u0302\u0005Q"+
		"\u0000\u0000\u02f9\u02fa\u0005w\u0000\u0000\u02fa\u0302\u0005T\u0000\u0000"+
		"\u02fb\u02fc\u0005w\u0000\u0000\u02fc\u0302\u0005U\u0000\u0000\u02fd\u02fe"+
		"\u0005w\u0000\u0000\u02fe\u0302\u0005V\u0000\u0000\u02ff\u0300\u0005w"+
		"\u0000\u0000\u0300\u0302\u0005W\u0000\u0000\u0301\u02f3\u0001\u0000\u0000"+
		"\u0000\u0301\u02f5\u0001\u0000\u0000\u0000\u0301\u02f7\u0001\u0000\u0000"+
		"\u0000\u0301\u02f9\u0001\u0000\u0000\u0000\u0301\u02fb\u0001\u0000\u0000"+
		"\u0000\u0301\u02fd\u0001\u0000\u0000\u0000\u0301\u02ff\u0001\u0000\u0000"+
		"\u0000\u0302\u00b9\u0001\u0000\u0000\u0000\u0303\u0304\u0005w\u0000\u0000"+
		"\u0304\u0308\u0005R\u0000\u0000\u0305\u0306\u0005w\u0000\u0000\u0306\u0308"+
		"\u0005X\u0000\u0000\u0307\u0303\u0001\u0000\u0000\u0000\u0307\u0305\u0001"+
		"\u0000\u0000\u0000\u0308\u00bb\u0001\u0000\u0000\u0000\u0309\u030a\u0005"+
		"x\u0000\u0000\u030a\u030b\u0003\u00d0h\u0000\u030b\u030c\u0003\u00d0h"+
		"\u0000\u030c\u030d\u0003\u00d0h\u0000\u030d\u030e\u0003\u00d0h\u0000\u030e"+
		"\u030f\u0003\u00d0h\u0000\u030f\u0310\u0003\u00d0h\u0000\u0310\u0311\u0003"+
		"\u00d0h\u0000\u0311\u0312\u0003\u00d0h\u0000\u0312\u00bd\u0001\u0000\u0000"+
		"\u0000\u0313\u0314\u0005y\u0000\u0000\u0314\u0315\u0003\u00d0h\u0000\u0315"+
		"\u0316\u0003\u00d0h\u0000\u0316\u00bf\u0001\u0000\u0000\u0000\u0317\u0318"+
		"\u0005{\u0000\u0000\u0318\u0319\u0003\u00d0h\u0000\u0319\u00c1\u0001\u0000"+
		"\u0000\u0000\u031a\u031b\u0005|\u0000\u0000\u031b\u031c\u0003\u00d0h\u0000"+
		"\u031c\u031d\u0003\u00d0h\u0000\u031d\u00c3\u0001\u0000\u0000\u0000\u031e"+
		"\u031f\u0005}\u0000\u0000\u031f\u0320\u0005O\u0000\u0000\u0320\u032b\u0003"+
		"\u00d0h\u0000\u0321\u0322\u0005}\u0000\u0000\u0322\u0323\u0005P\u0000"+
		"\u0000\u0323\u032b\u0003\u00d0h\u0000\u0324\u0325\u0005}\u0000\u0000\u0325"+
		"\u0326\u0005Q\u0000\u0000\u0326\u032b\u0003\u00d0h\u0000\u0327\u0328\u0005"+
		"}\u0000\u0000\u0328\u0329\u0005R\u0000\u0000\u0329\u032b\u0003\u00d0h"+
		"\u0000\u032a\u031e\u0001\u0000\u0000\u0000\u032a\u0321\u0001\u0000\u0000"+
		"\u0000\u032a\u0324\u0001\u0000\u0000\u0000\u032a\u0327\u0001\u0000\u0000"+
		"\u0000\u032b\u00c5\u0001\u0000\u0000\u0000\u032c\u032d\u0005}\u0000\u0000"+
		"\u032d\u032e\u0005S\u0000\u0000\u032e\u033c\u0003\u00d0h\u0000\u032f\u0330"+
		"\u0005}\u0000\u0000\u0330\u0331\u0005Y\u0000\u0000\u0331\u033c\u0003\u00d0"+
		"h\u0000\u0332\u0333\u0005}\u0000\u0000\u0333\u0334\u0005\\\u0000\u0000"+
		"\u0334\u0338\u0003\u00d0h\u0000\u0335\u0337\u0003\u00d0h\u0000\u0336\u0335"+
		"\u0001\u0000\u0000\u0000\u0337\u033a\u0001\u0000\u0000\u0000\u0338\u0336"+
		"\u0001\u0000\u0000\u0000\u0338\u0339\u0001\u0000\u0000\u0000\u0339\u033c"+
		"\u0001\u0000\u0000\u0000\u033a\u0338\u0001\u0000\u0000\u0000\u033b\u032c"+
		"\u0001\u0000\u0000\u0000\u033b\u032f\u0001\u0000\u0000\u0000\u033b\u0332"+
		"\u0001\u0000\u0000\u0000\u033c\u00c7\u0001\u0000\u0000\u0000\u033d\u033e"+
		"\u0005~\u0000\u0000\u033e\u033f\u0005Z\u0000\u0000\u033f\u0340\u0003\u00d0"+
		"h\u0000\u0340\u0341\u0003\u00d0h\u0000\u0341\u034c\u0001\u0000\u0000\u0000"+
		"\u0342\u0343\u0005~\u0000\u0000\u0343\u0344\u0005[\u0000\u0000\u0344\u0345"+
		"\u0003\u00d0h\u0000\u0345\u0346\u0003\u00d0h\u0000\u0346\u034c\u0001\u0000"+
		"\u0000\u0000\u0347\u0348\u0005~\u0000\u0000\u0348\u0349\u0003\u00d0h\u0000"+
		"\u0349\u034a\u0003\u00d0h\u0000\u034a\u034c\u0001\u0000\u0000\u0000\u034b"+
		"\u033d\u0001\u0000\u0000\u0000\u034b\u0342\u0001\u0000\u0000\u0000\u034b"+
		"\u0347\u0001\u0000\u0000\u0000\u034c\u00c9\u0001\u0000\u0000\u0000\u034d"+
		"\u034e\u0005\u007f\u0000\u0000\u034e\u034f\u0003\u00d0h\u0000\u034f\u0350"+
		"\u0005P\u0000\u0000\u0350\u0351\u0003\u00d0h\u0000\u0351\u0352\u0003\u00d0"+
		"h\u0000\u0352\u035a\u0001\u0000\u0000\u0000\u0353\u0354\u0005\u007f\u0000"+
		"\u0000\u0354\u0355\u0003\u00d0h\u0000\u0355\u0356\u0003\u00d0h\u0000\u0356"+
		"\u0357\u0003\u00d0h\u0000\u0357\u0358\u0003\u00d0h\u0000\u0358\u035a\u0001"+
		"\u0000\u0000\u0000\u0359\u034d\u0001\u0000\u0000\u0000\u0359\u0353\u0001"+
		"\u0000\u0000\u0000\u035a\u00cb\u0001\u0000\u0000\u0000\u035b\u035c\u0005"+
		"\u0080\u0000\u0000\u035c\u00cd\u0001\u0000\u0000\u0000\u035d\u035e\u0005"+
		"\\\u0000\u0000\u035e\u00cf\u0001\u0000\u0000\u0000\u035f\u0360\u0007\u0002"+
		"\u0000\u0000\u0360\u00d1\u0001\u0000\u0000\u0000G\u00d5\u00d8\u00e8\u00f2"+
		"\u00fc\u0102\u010a\u0113\u0119\u011f\u0121\u0128\u0137\u0147\u014d\u0159"+
		"\u015f\u016b\u0175\u018a\u019b\u019d\u01a1\u01a8\u01aa\u01ae\u01b5\u01ba"+
		"\u01be\u01c2\u01cb\u01cd\u01d2\u01d4\u01d8\u01e2\u01e6\u01ea\u01ef\u01f5"+
		"\u01fa\u0200\u0203\u0209\u020e\u0214\u021a\u021f\u0224\u0227\u022d\u0232"+
		"\u0237\u023e\u024c\u0250\u0258\u025e\u026c\u0280\u0290\u0295\u0297\u02da"+
		"\u0301\u0307\u032a\u0338\u033b\u034b\u0359";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}