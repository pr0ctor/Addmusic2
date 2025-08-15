grammar Mml;
options { caseInsensitive=true; }
/*
 * Parser Rules
 */
song: songElement+
    | EOF
    ;

songElement: specialDirective
    | soundChannel
    | remoteCode
    | defaultLength
    | globalVolumeCommand
    | tempoCommand
    | replacements
    | noloopCommand
    | StringLiteral
    | globalHexCommands
    | hexNumber
    | introEnd
    | remoteCode
    | qmark
    ;

// Special Directives

specialDirective: amk
    | spc
    | samples
    | instruments
    | path
    | pad
    | halvetempo
    | option
    ;
    // | preprocessors
    // | define
    // | undef
    // | ifdef
    // | ifndef
    // | error
    //;

// #samples
// {
//         #samplegroup     ;optional
//         "sample1.brr"
//         "sample2.brr"
//         "sample3.brr"
// }
samples : Samples LBRACE samplesList RBRACE ;
samplesList : SampleOptimization* StringLiteral* ;

// #instruments
// {
//         "sample1.brr" $aa $bb $cc $dd $ee
//         "sample2.brr" $aa $bb $cc $dd $ee
//         @0 $aa $bb $cc $dd $ee
//         n1F $aa $bb $cc $dd $ee
// }
instruments : Instruments LBRACE instrumentsList* RBRACE ;
instrumentsList : StringLiteral hexNumber+ # NamedInstrumentListItem
    | instrumentCommand hexNumber+ # InstrumentListItem
    | noiseNote hexNumber+ # NoiseInstrumentListItem
    ;

// #spc
// {
//      #author     "Author's name"
//      #game	    "Game's name"
//      #comment    "A comment"
//      #title	    "Song's name"
// }
spc : Spc LBRACE spcList* RBRACE ;
spcList :  SpcAuthor StringLiteral
    | SpcGame StringLiteral
    | SpcComment StringLiteral
    | SpcTitle StringLiteral
    | SpcLength StringLiteral
    //| POUND Text StringLiteral
    ;

// #pad $2F0
pad : Pad hexNumber ;

// #path "SMB1 Overworld"
path : Path StringLiteral ;

// #halvetempo
halvetempo : Halvetempo ;

// #option noloop
// #option dividetempo 3
// #option
// {
//      #noloop
//      #dividetempo    3
// }
option : Option LBRACE ( POUND optionItem )* RBRACE # OptionGroup
    | Option optionItem # SingleOption
    ;
optionItem : Tempoimmunity
    | Dividetempo NUMBERS
    | Smwvtable
    | Nspcvtable
    | Noloop
    | Amk109hotpatch
    | StringLiteral NUMBERS
    | StringLiteral
    ;

amk : Amk amkVersion # GeneralAmkVersion
    | amm # AmmVersion
    | am4 # Am4Version
    ;
amm : Amm ;
am4 : Am4 ;
amkVersion : AmkV1
    | NUMBERS
    ;

// End Special Directives

// Channels

// soundChannel : Channel introSection channelContents* # ChannelWithIntro
//     | Channel channelContents* # ChannelWithoutIntro
//     ;
// soundChannel : Channel channelContents* /*introEnd?*/ channelContents* ;
soundChannel : Channel channelContents* ;
introEnd : FSLASH ;
channelContents: atomics
    | loopers
    | sampleLoadCommand
    | channelHexCommands
    | hexNumber
    ;
atomics: pitchslide
    | note
    | rest
    | octave
    | lowerOctave
    | raiseOctave
    | noiseNote
    | triplet
    | volumeCommand
    | tuneCommand
    | instrumentCommand
    | quantization
    | panCommand
    | vibratoCommand
    | tempoCommand
    | introEnd
    | nakedTie
    | qmark
    ;

// note : BasicNote ( SHARP | FLAT )? NOTEDURATIONS? DOT* ( TIE NUMBERS )*
//     | BasicNote ( SHARP | FLAT )? NOTEDURATIONS? ( TIE NUMBERS DOT* )*
//     ;
note : Note
    ;
// rest : Rest NOTEDURATIONS? DOT* ( TIE NUMBERS )*
//     | Rest NOTEDURATIONS? ( TIE NUMBERS DOT* )*
//     ;
rest : Rest ;
octave : Octave ;
lowerOctave : LT ;
raiseOctave : GT ;
// noiseNote : Noise HexDigits+ ;
noiseNote : Noise ;
// volume : Volume NUMBERS
//     | Volume NUMBERS COMMA NUMBERS
//     ;
volumeCommand : Volume # Volume
    | ( e7Volume | e8VolumeFade ) # HexVolume
    ;
//tune : Tune UNUMBERS ;
tuneCommand : Tune # Tune
    | eeTuneChannel # HexTune
    ;
// quantization : Quantization QUANTIZATION HexDigits
//     | Quantization QUANTIZATION volume
//     ;
quantization : Quantization ;
// pan : Pan NUMBERS COMMA ( Zero | One ) COMMA ( Zero | One )
//     | Pan NUMBERS
//     ;
panCommand : Pan # Pan
    | ( dbPan | dcPanFade ) # HexPan
    ;
// vibrato : Vibrato NUMBERS COMMA NUMBERS COMMA NUMBERS
//     | Vibrato NUMBERS COMMA NUMBERS
//     ;
vibratoCommand : Vibrato # Vibrato
    | deVibratoStart # HexVibrato
    ;
// pitchslide : ( note | rest ) AMPER pitchslide
//     | ( note | rest ) AMPER ( note | rest )
//     ;( Note | Rest ) ( AMPER ( Note | Rest ) )+ ;
// pitchslide : Pitchslide ;
pitchslide : ( Note | Rest ) ( AMPER ( Note | Rest ) )+ ;

triplet : LBRACE (note | rest) (note | rest) (note | rest) RBRACE
    ;

// defaultLength : Length NOTEDURATIONS ;
defaultLength : Length ;

// globalVolume : GlobalVolume NUMBERS COMMA NUMBERS
//     |   GlobalVolume NUMBERS
//     ;
globalVolumeCommand : GlobalVolume # GlobalVolume
    | ( e0GlobalVolume | e1GlobalVolumeFade ) # HexGlobalVolume
    ;

// tempo : Tempo NUMBERS COMMA NUMBERS
//     | Tempo NUMBERS
//     ;
tempoCommand : Tempo # Tempo
    | ( e2Tempo | e3TempoFade ) # HexTempo
    ;

instrumentCommand : Instrument # Instrument
    | daInstrument # HexInstrument
    ;

nakedTie : Tie ;

qmark : Question ;

// End Channels

// Logic Controls

loopers : logicControls
    | logicCalls
    ;

logicControls : superLoop
    //| namedSimpleLoop
    | simpleLoop
    ;
logicCalls : callLoop
    | remoteLogicCalls
    ;
remoteLogicCalls : callRemoteCode
    | callPreviousLoop
    | stopRemoteCode
    ;

// superloop : L2BRACK (atomics | logicCalls | terminalNamedSimpleloop | terminalSimpleloop )* R2BRACK NUMBERS? ;
superLoop : L2BRACK superLoopContents* R2BRACK NUMBERS? ;
superLoopContents : atomics
    | terminalSimpleLoop
    | logicCalls 
    /* | terminalNamedSimpleLoop */
    | hexNumber
    ;
// namedsimpleloop : LPAREN StringLiteral RPAREN simpleloop
//     | LPAREN NUMBERS RPAREN simpleloop
//     ;
// namedSimpleLoop : LoopName simpleloop ;
// namedSimpleLoop : LPAREN ( NUMBERS | StringLiteral ) RPAREN simpleloop ;
// simpleLoop : ( LPAREN ( NUMBERS | StringLiteral ) RPAREN )? LBRACK (atomics | terminalSuperLoop | remoteLogicCalls | hexNumber)* RBRACK NUMBERS? ;
simpleLoop : ( LoopName )? LBRACK simpleLoopContents* RBRACK NUMBERS? ;
simpleLoopContents : atomics
    | terminalSuperLoop
    | remoteLogicCalls
    | hexNumber
    ;
// simpleloop : LBRACK (atomics | terminalSuperLoop | remoteLogicCalls | hexNumber)* RBRACK NUMBERS? ;

terminalSuperLoop : L2BRACK terminalSuperLoopContents* R2BRACK NUMBERS? ;
terminalSuperLoopContents : atomics
    | logicCalls
    | hexNumber
    ;
//terminalSimpleLoop : ( LPAREN ( NUMBERS | StringLiteral ) RPAREN )? LBRACK (atomics | remoteLogicCalls | hexNumber)* RBRACK NUMBERS? ;
terminalSimpleLoop : ( LoopName )? LBRACK terminalSimpleLoopContents* RBRACK NUMBERS? ;
terminalSimpleLoopContents : atomics
    | remoteLogicCalls
    | hexNumber
    ;
// terminalNamedSimpleLoop : LPAREN ( NUMBERS | StringLiteral ) RPAREN terminalSimpleLoop ;
//terminalNamedSimpleloop : LoopName terminalSimpleloop ;

// remoteCode : LPAREN BANG ( NUMBERS | StringLiteral ) RPAREN LBRACK remoteCodeContents RBRACK;
remoteCode : RemoteCodeName LBRACK remoteCodeContents+ RBRACK;

remoteCodeContents : octave
    | lowerOctave
    | raiseOctave
    | volumeCommand
    | tuneCommand
    // | instrument
    | quantization
    | panCommand
    | vibratoCommand
    | tempoCommand
    | hexNumber
    ;

// callLoop : LPAREN ( NUMBERS | StringLiteral ) RPAREN NUMBERS?;
callLoop : LoopName NUMBERS? ;

// callRemoteCode : LPAREN BANG StringLiteral COMMA REMOTECODENUMBERS (COMMA NUMBERS)? RPAREN 
//     | LPAREN BANG NUMBERS COMMA REMOTECODENUMBERS (COMMA NUMBERS)? RPAREN 
//     ;
callRemoteCode : CallRemoteCode ;
// stopRemoteCode : LPAREN BANG BANG REMOTECODENUMBERS RPAREN ;
stopRemoteCode : StopRemoteCode ;

callPreviousLoop : STAR NUMBERS? ;
// callPreviousLoop : CallPreviousLoop ;

// End Logic Controls

// Miscellaneous Commands

noloopCommand : QMARK ;

// sampleLoad : LPAREN StringLiteral COMMA HexNumber RPAREN ;
sampleLoadCommand : LoadSample # SampleLoad
    | f3SampleLoad # HexSampleLoad
    ;

replacements : ReplacementText ;

// End Miscellaneous Commands

// Hex Commands

globalHexCommands : f5FIRFilter
    | e4GlobalTranspose
    | f4GlobalItems
    | efEcho1
    | f1Echo2
    | f0EchoOff
    | f2EchoFade
    | f6DSPWrite
    | f9DataSend
    | faGlobalItems
    ;
channelHexCommands : e5Tremolo
    | fbItems
    | eaVibratoFade
    | dfVibratoEnd
    | ebPitchEnvelopeRelease
    | ecPitchEnvelopeAttack
    | edCustomADSROrGain
    | f0EchoOff
    | f2EchoFade
    | f4ChannelItems
    | f6DSPWrite
    | f8EnableNoise
    | f9DataSend
    | faChannelItems
    | fbItems
    | fcItems
    | fdTremoloOff
    | fePitchEnvelopeOff
    ;


daInstrument : NDA NUMBERS ;

dbPan : NDB hexNumber ;
dcPanFade : NDC hexNumber hexNumber ;

ddPitchBlendCommand : NDD hexNumber ( hexNumber | ddPitchBlendItems ) ;
ddPitchBlendItems :  ( octave | raiseOctave | lowerOctave )* note ;

deVibratoStart : NDE hexNumber hexNumber hexNumber ;
eaVibratoFade : NEA hexNumber ;
dfVibratoEnd : NDF ;

e0GlobalVolume : NE0 hexNumber ;
e1GlobalVolumeFade : NE1 hexNumber hexNumber ;

e2Tempo : NE2 hexNumber ;
e3TempoFade : NE3 hexNumber hexNumber ;

e4GlobalTranspose : NE4 hexNumber ;

e5Tremolo : NE5 hexNumber hexNumber hexNumber ;

e6SubloopStart : NE6 N00 ;
e6SubloopEnd : NE6 hexNumber ;

e7Volume : NE7 hexNumber ;
e8VolumeFade : NE8 hexNumber hexNumber ;

ebPitchEnvelopeRelease : NEB hexNumber hexNumber hexNumber ;
ecPitchEnvelopeAttack : NEC hexNumber hexNumber hexNumber ;

edCustomADSROrGain : NED N80 hexNumber # EDCustomGAIN
    | NED hexNumber hexNumber # EDCustomASDR
    ;

eeTuneChannel : NEE hexNumber ;

efEcho1 : NEF hexNumber hexNumber hexNumber ;
f0EchoOff : NF0 ;
f1Echo2 : NF1 hexNumber hexNumber hexNumber ;
f2EchoFade : NF2 hexNumber hexNumber ;

f3SampleLoad : NF3 hexNumber hexNumber ;

// f4Items : F4Items N00 #F4EnableYoshiDrumsChannel5
//     | F4Items N01 #F4ToggleLegato
//     | F4Items N02 #F4LightStaccato
//     | F4Items N03 #F4EchoToggle
//     | F4Items N05 #F4SNESSync
//     | F4Items N06 #F4EnableYoshiDrums
//     | F4Items N07 #F4TempoHikeOff
//     | F4Items N08 #F4NSPCVelocityTable
//     | F4Items N09 #F4RestoreInstrument
//     ;

f4GlobalItems : NF4 N00 # F4EnableYoshiDrumsChannel5
    | NF4 N01 # F4ToggleLegato
    | NF4 N02 # F4LightStaccato
    | NF4 N05 # F4SNESSync
    | NF4 N06 # F4EnableYoshiDrums
    | NF4 N07 # F4TempoHikeOff
    | NF4 N08 # F4NSPCVelocityTable
    ;
f4ChannelItems : NF4 N03 # F4EchoToggle
    | NF4 N09 # F4RestoreInstrument
    ;

f5FIRFilter : NF5 hexNumber hexNumber hexNumber hexNumber hexNumber hexNumber hexNumber hexNumber ;

f6DSPWrite : NF6 hexNumber hexNumber ;

f8EnableNoise : NF8 hexNumber ;

f9DataSend : NF9 hexNumber hexNumber ;

// faItems : FAItems N00 hexNumber #FAPitchModulation
//     | FAItems N01 hexNumber #FACurrentChannelGain
//     | FAItems N02 hexNumber #FASemitoneTune
//     | FAItems N03 hexNumber #FAAmplify
//     | FAItems N04 hexNumber #FAEchoBufferReserve
//     | FAItems N7F hexNumber #HotPatchPreset
//     | FAItems NFE hexNumber hexNumber* #HotPatchToggleBits
//     ;
faChannelItems : NFA N00 hexNumber # FAPitchModulation
    | NFA N01 hexNumber # FACurrentChannelGain
    | NFA N02 hexNumber # FASemitoneTune
    | NFA N03 hexNumber # FAAmplify
    ;
faGlobalItems : NFA N04 hexNumber # FAEchoBufferReserve
    | NFA N7F hexNumber # FAHotPatchPreset
    | NFA NFE hexNumber hexNumber* # FAHotPatchToggleBits
    ;

// fbItems : FBItems N80 hexNumber hexNumber #FBTrill
//     | FBItems N81 hexNumber hexNumber #FBGlissando
//     | FBItems hexNumber hexNumber #FBEnableArgeggio
//     ;
fbItems : NFB N80 hexNumber hexNumber # FBTrill
    | NFB N81 hexNumber hexNumber # FBGlissando
    | NFB hexNumber hexNumber # FBEnableArgeggio
    ;

// fcItems : FCItems hexNumber hexNumber hexNumber hexNumber #HexRemoteCommand
//     | FCItems hexNumber N01 hexNumber hexNumber #HexRemoteGain
//     ;
fcItems : NFC hexNumber N01 hexNumber hexNumber # FCHexRemoteGain
    | NFC hexNumber hexNumber hexNumber hexNumber # FCHexRemoteCommand
    ;

fdTremoloOff : NFD ;

fePitchEnvelopeOff : NFE ;
//hexNumber : hexNumberItem ;
//hexNumberItem : N00
hexNumber : N00
    | N01
    | N02
    | N03
    | N04
    | N05
    | N06
    | N07
    | N08
    | N09
    | N7F
    | N80
    | N81
    | NFE
    | NDA
    | NDB
    | NDC
    | NDD
    | NDE
    | NDF
    | NE0
    | NE1
    | NE2
    | NE3
    | NE4
    | NE5
    | NE6
    | NE7
    | NE8
    | NE9
    | NEA
    | NEB
    | NEC
    | NED
    | NEE
    | NEF
    | NF0
    | NF1
    | NF2
    | NF3
    | NF4
    | NF5
    | NF6
    | NF7
    | NF8
    | NF9
    | NFA
    | NFB
    | NFC
    | NFD
    | NFE
    // | DOLLAR HexDigits
    | HexNumber
    ;

// End Hex Commands

/*
 * Lexer Rules
 */
POUND : '#' ;
DOLLAR : '$' ;
COMMAT : '@' ;
AMPER : '&' ;
BANG : '!' ;
PERCENT : '%' ;
STAR : '*' ;
LBRACE : '{' ;
RBRACE : '}' ;
LPAREN : '(' ;
RPAREN : ')' ;
LBRACK : '[' ;
RBRACK : ']' ;
L2BRACK : '[[' ;
R2BRACK : ']]' ;
DQUOTE : '"' ;
SQUOTE : '\'' ;
SHARP : '+' ;
FLAT : '-' ;
DOT : '.' ;
TIE : '^' ;
GT : '>' ;
LT : '<' ;
COMMA : ',' ;
FSLASH : '/' ;
QMARK : '?' ;
EQUAL : '=' ;
SEMICOLON : ';' ;

fragment QMARKVALUES : [0-2] ;
fragment CHANNELS : [0-7] ;
fragment QUANTIZATION : [0-7] ;
fragment OCTAVES : [0-6] ;
// NOTEDURATIONS : ONE | TWO | FOUR | EIGHT | SIXTEEN | THIRTYTWO | SIXTYFOUR ;
fragment REMOTECODENUMBERS : NEGATIVEONE | ZERO | ONE | TWO | THREE | FOUR | SEVEN | EIGHT ; 

ReplacementText : DQUOTE ~[=\n\r"]+ EQUAL ~[\n\r"]+ DQUOTE ;
StringLiteral : DQUOTE ~[\n\r"]+ DQUOTE ;
PercentNumber : PERCENT HexDigits ;
// fragmentText : [a-z0-9]+ ;
// TextStringLiteral : DQUOTE [ a-z0-9]+ DQUOTE ;
// FullStringLiteral : DQUOTE [ a-z0-9_\-.]+ DQUOTE ;
// PathStringLiteral : DQUOTE [ a-z0-9_\-/\\]+ DQUOTE;
// TextStringLiteral : StringLiteral ;
// FullStringLiteral : StringLiteral ;
// PathStringLiteral : StringLiteral ;

fragment A : ('a') ;
fragment B : ('b') ;
fragment C : ('c') ;
fragment D : ('d') ;
fragment E : ('e') ;
fragment F : ('f') ;
fragment G : ('g') ;
fragment N : ('n') ;
fragment O : ('o') ;
fragment T : ('t') ;
fragment R : ('r') ;
fragment V : ('v') ;
fragment H : ('h') ;
fragment L : ('l') ;
fragment Q : ('q') ;
fragment W : ('w') ;
fragment Y : ('y') ;
fragment P : ('p') ;
fragment NEGATIVEONE : ('-1') ;
fragment ZERO : ('0') ;
fragment ONE : ('1') ;
fragment TWO : ('2') ;
fragment THREE : ('3') ;
fragment FOUR : ('4') ;
fragment SEVEN : ('7') ;
fragment EIGHT : ('8') ;
fragment SIXTEEN : ('16') ;
fragment THIRTYTWO : ('32') ;
fragment SIXTYFOUR : ('64') ;
fragment AMKV1 : ('=1') ;

fragment BasicNote : A | B | C | D | E | F | G ;
Note : BasicNote ( SHARP | FLAT )? EQUAL? NUMBERS? DOT* ( Tie )* ;
Rest : R EQUAL? NUMBERS? DOT* ( Tie )* ;
Octave : O OCTAVES ;
Noise : N HexDigits ;
Tempo : T NUMBERS ( COMMA NUMBERS )? ;
Volume : V NUMBERS ( COMMA NUMBERS )? ;
Tune : H UNUMBERS ;
Length : L NUMBERS ;
Quantization : Q QUANTIZATION ( HexDigit | Volume ) ;
GlobalVolume : W NUMBERS ( COMMA NUMBERS )? ;
Pan : Y ( NUMBERS | NUMBERS COMMA ( ZERO | ONE ) COMMA ( ZERO | ONE ) ) ;
Vibrato : P NUMBERS COMMA NUMBERS ( COMMA NUMBERS )? ;
Tie : TIE EQUAL? NUMBERS DOT* ;
Question : QMARK QMARKVALUES ;
// Pitchslide : ( Note | Rest ) ( AMPER ( Note | Rest ) )+ ;
Instrument : COMMAT NUMBERS ;

LoopName : LPAREN ( NUMBERS | StringLiteral ) RPAREN ;
RemoteCodeName : LPAREN BANG ( NUMBERS | StringLiteral ) RPAREN ;
StopRemoteCode : LPAREN BANG BANG REMOTECODENUMBERS RPAREN ;
// CallLoop : LoopName NUMBERS? ;
// CallLoop : LPAREN ( NUMBERS | StringLiteral ) RPAREN NUMBERS? ;
CallRemoteCode : LPAREN BANG ( NUMBERS | StringLiteral ) COMMA REMOTECODENUMBERS (COMMA NUMBERS)? RPAREN ;
CallPreviousLoop : STAR NUMBERS? ;
LoadSample : LPAREN StringLiteral COMMA HexNumber RPAREN ;

AmkV1 : AMKV1 ;



fragment AMK : ('amk') ;
fragment AMM : ('amm') ;
fragment AM4 : ('am4') ;
fragment SAMPLES : ('samples') ;
fragment INSTRUMENTS : ('instruments') ;
fragment SPC : ('spc') ;
fragment AUTHOR : ('author') ;
fragment GAME : ('game') ;
fragment COMMENT : ('comment') ;
fragment TITLE : ('title') ;
fragment LENGTH : ('length') ;
fragment PAD : ('pad') ;
fragment PATH : ('path') ;
fragment HALVETEMPO : ('halvetempo') ;
fragment OPTION : ('option') ;
fragment TEMPOIMMUNITY : ('tempoimmunity') ;
fragment DIVIDETEMPO : ('dividetempo') ;
fragment SMWVTABLE : ('smwvtable') ;
fragment NSPCVTABLE : ('nspcvtable') ;
fragment NOLOOP : ('noloop') ;
fragment AMK109HOTPATCH : ('amk109hotpatch') ;
fragment LOUDER : ('louder') ;
Amk : POUND AMK ;
Amm : POUND AMM ;
Am4 : POUND AM4 ;
Samples : POUND SAMPLES ;
Instruments : POUND INSTRUMENTS ;
Spc : POUND SPC ;
SpcAuthor : POUND AUTHOR ;
SpcGame : POUND GAME ;
SpcComment : POUND COMMENT ;
SpcTitle : POUND TITLE ;
SpcLength : POUND LENGTH ;
Pad : POUND PAD ;
Path : POUND PATH ;
Halvetempo : POUND HALVETEMPO ;
Option : POUND OPTION ;
Louder : POUND LOUDER ;

Tempoimmunity : TEMPOIMMUNITY ;
Dividetempo : DIVIDETEMPO ;
Smwvtable : SMWVTABLE ;
Nspcvtable : NSPCVTABLE ;
Noloop : NOLOOP ;
Amk109hotpatch : AMK109HOTPATCH ;

Channel : POUND CHANNELS ;
SampleOptimization : POUND [a-z0-9]+ ;

fragment NF00 : ('$00') ;
fragment NF01 : ('$01') ;
fragment NF02 : ('$02') ;
fragment NF03 : ('$03') ;
fragment NF04 : ('$04') ;
fragment NF05 : ('$05') ;
fragment NF06 : ('$06') ;
fragment NF07 : ('$07') ;
fragment NF08 : ('$08') ;
fragment NF09 : ('$09') ;
fragment NF7F : ('$7F') ;
fragment NF80 : ('$80') ;
fragment NF81 : ('$81') ;

N00 : NF00 ;
N01 : NF01 ;
N02 : NF02 ;
N03 : NF03 ;
N04 : NF04 ;
N05 : NF05 ;
N06 : NF06 ;
N07 : NF07 ;
N08 : NF08 ;
N09 : NF09 ;
N7F : NF7F ;
N80 : NF80 ;
N81 : NF81 ;
NFE : NFFE ;

// N00 : DOLLAR NF00 ;
// N01 : DOLLAR NF01 ;
// N02 : DOLLAR NF02 ;
// N03 : DOLLAR NF03 ;
// N04 : DOLLAR NF04 ;
// N05 : DOLLAR NF05 ;
// N06 : DOLLAR NF06 ;
// N07 : DOLLAR NF07 ;
// N08 : DOLLAR NF08 ;
// N09 : DOLLAR NF09 ;
// N7F : DOLLAR NF7F ;
// N80 : DOLLAR NF80 ;
// N81 : DOLLAR NF81 ;
// NFE : DOLLAR NFFE ;

fragment NFDA : ('$DA') ;
fragment NFDB : ('$DB') ;
fragment NFDC : ('$DC') ;
fragment NFDD : ('$DD') ;
fragment NFDE : ('$DE') ;
fragment NFDF : ('$DF') ;
fragment NFE0 : ('$E0') ;
fragment NFE1 : ('$E1') ;
fragment NFE2 : ('$E2') ;
fragment NFE3 : ('$E3') ;
fragment NFE4 : ('$E4') ;
fragment NFE5 : ('$E5') ;
fragment NFE6 : ('$E6') ;
fragment NFE7 : ('$E7') ;
fragment NFE8 : ('$E8') ;
fragment NFE9 : ('$E9') ;
fragment NFEA : ('$EA') ;
fragment NFEB : ('$EB') ;
fragment NFEC : ('$EC') ;
fragment NFED : ('$ED') ;
fragment NFEE : ('$EE') ;
fragment NFEF : ('$EF') ;
fragment NFF0 : ('$F0') ;
fragment NFF1 : ('$F1') ;
fragment NFF2 : ('$F2') ;
fragment NFF3 : ('$F3') ;
fragment NFF4 : ('$F4') ;
fragment NFF5 : ('$F5') ;
fragment NFF6 : ('$F6') ;
fragment NFF7 : ('$F7') ;
fragment NFF8 : ('$F8') ;
fragment NFF9 : ('$F9') ;
fragment NFFA : ('$FA') ;
fragment NFFB : ('$FB') ;
fragment NFFC : ('$FC') ;
fragment NFFD : ('$FD') ;
fragment NFFE : ('$FE') ;

NDA : NFDA ;
NDB : NFDB ;
NDC : NFDC ;
NDD : NFDD ;
NDE : NFDE ;
NDF : NFDF ;
NE0 : NFE0 ;
NE1 : NFE1 ;
NE2 : NFE2 ;
NE3 : NFE3 ;
NE4 : NFE4 ;
NE5 : NFE5 ;
NE6 : NFE6 ;
NE7 : NFE7 ;
NE8 : NFE8 ;
NE9 : NFE9 ;
NEA : NFEA ;
NEB : NFEB ;
NEC : NFEC ;
NED : NFED ;
NEE : NFEE ;
NEF : NFEF ;
NF0 : NFF0 ;
NF1 : NFF1 ;
NF2 : NFF2 ;
NF3 : NFF3 ;
NF4 : NFF4 ;
NF5 : NFF5 ;
NF6 : NFF6 ;
NF7 : NFF7 ;
NF8 : NFF8 ;
NF9 : NFF9 ;
NFA : NFFA ;
NFB : NFFB ;
NFC : NFFC ;
NFD : NFFD ;

// NDA : DOLLAR NFDA ;
// NDB : DOLLAR NFDB ;
// NDC : DOLLAR NFDC ;
// NDD : DOLLAR NFDD ;
// NDE : DOLLAR NFDE ;
// NDF : DOLLAR NFDF ;
// NE0 : DOLLAR NFE0 ;
// NE1 : DOLLAR NFE1 ;
// NE2 : DOLLAR NFE2 ;
// NE3 : DOLLAR NFE3 ;
// NE4 : DOLLAR NFE4 ;
// NE5 : DOLLAR NFE5 ;
// NE6 : DOLLAR NFE6 ;
// NE7 : DOLLAR NFE7 ;
// NE8 : DOLLAR NFE6 ;
// NE9 : DOLLAR NFE9 ;
// NEA : DOLLAR NFEA ;
// NEB : DOLLAR NFEB ;
// NEC : DOLLAR NFEC ;
// NED : DOLLAR NFED ;
// NEE : DOLLAR NFEE ;
// NEF : DOLLAR NFEF ;
// NF0 : DOLLAR NFF0 ;
// NF1 : DOLLAR NFF1 ;
// NF2 : DOLLAR NFF2 ;
// NF3 : DOLLAR NFF3 ;
// NF4 : DOLLAR NFF4 ;
// NF5 : DOLLAR NFF5 ;
// NF6 : DOLLAR NFF6 ;
// NF7 : DOLLAR NFF7 ;
// NF8 : DOLLAR NFF8 ;
// NF9 : DOLLAR NFF9 ;
// NFA : DOLLAR NFFA ;
// NFB : DOLLAR NFFB ;
// NFC : DOLLAR NFFC ;
// NFD : DOLLAR NFFD ;

// F5FIRFilter : NF5 HexNumber HexNumber HexNumber HexNumber HexNumber HexNumber HexNumber HexNumber ;

// DAInstrument : DOLLAR DA ;
// DBPan : DOLLAR DB ;
// DCPanFade : DOLLAR DC ;
// DDPitchBlend : DOLLAR DD ;
// DEVibrato : DOLLAR DE ;
// DFVibratoStop : DOLLAR DF ;
// E0GlobalVolume : DOLLAR E0 ;
// E1GlobalVolumeFade : DOLLAR E1 ;
// E2Tempo : DOLLAR E2 ;
// E3TempoFade : DOLLAR E3 ;
// E4GlobalTranspose : DOLLAR E4 ;
// E5Tremolo : DOLLAR E5 ;
// E6Subloop : DOLLAR E6 ;
// E7Volume : DOLLAR E7 ;
// E8VolumeFade : DOLLAR E8 ;
// EAVibratoFade : DOLLAR EA ;
// EBPitchEnvelopeRelease : DOLLAR EB ;
// ECPitchEnvelopeAttack : DOLLAR EC ;
// EDCustomADSROrGain : DOLLAR ED ;
// EETuneChannel : DOLLAR EE ;
// EFEcho1 : DOLLAR EF ;
// F0EchoOff : DOLLAR F0 ;
// F1Echo2 : DOLLAR F1 ;
// F2EchoFade : DOLLAR F2 F2EchoDelay ;
// F2EchoDelay : DOLLAR ZERO [a-f] ;
// F3SampleLoad : DOLLAR F3 ;
// F4Items : DOLLAR F4 ;
// F5FIRFilter : DOLLAR F5 ;
// F6DSPWrite : DOLLAR F6 ;
// F8EnableNoise : DOLLAR F8 ;
// F9DataSend : DOLLAR F9 ;
// FAItems : DOLLAR FA ;
// FBItems : DOLLAR FB ;
// FCItems : DOLLAR FC ;
// FDTremoloOff : DOLLAR FD ;
// FEPitchEnvelopeOff : DOLLAR FE ;

fragment NUMBER : [0-9] ;
NUMBERS : NUMBER+ ;
UNUMBERS : [\-]?NUMBER+ ;

fragment HexDigit : [0-9a-f] ;
HexDigits : HexDigit+ ;
// HexDigits : HexDigit HexDigit+ ;
// fragment HexNumber : DOLLAR HexDigits ;
HexNumber : DOLLAR HexDigits ;


WHITESPACE : ( ' '|'\t' | '\r' | '\n' )+ -> skip ;
Comment : SEMICOLON ~[\r\n]* -> skip ;

//Error : . -> skip ;
