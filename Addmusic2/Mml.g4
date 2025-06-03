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
    | globalVolume
    | tempo
    | replacements
    | noloopCommand
    | StringLiteral
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
samplesList : SampleOptimization? StringLiteral* ;

// #instruments
// {
//         "sample1.brr" $aa $bb $cc $dd $ee
//         "sample2.brr" $aa $bb $cc $dd $ee
//         @0 $aa $bb $cc $dd $ee
//         n1F $aa $bb $cc $dd $ee
// }
instruments : Instruments LBRACE instrumentsList* RBRACE ;
instrumentsList : StringLiteral HexNumber+ # NamedInstrumentListItem
    | instrument HexNumber+ # InstrumentListItem
    | noiseNote HexNumber+ # NoiseInstrumentListItem
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
pad : Pad HexNumber ;

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
    | sampleLoad
    | HexNumber
    ;
atomics: pitchslide
    | note
    | rest
    | octave
    | lowerOctave
    | raiseOctave
    | noiseNote
    | triplet
    | volume
    | tune
    | instrument
    | quantization
    | pan
    | vibrato
    | tempo
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
volume : Volume ;
//tune : Tune UNUMBERS ;
tune : Tune ;
// quantization : Quantization QUANTIZATION HexDigits
//     | Quantization QUANTIZATION volume
//     ;
quantization : Quantization ;
// pan : Pan NUMBERS COMMA ( Zero | One ) COMMA ( Zero | One )
//     | Pan NUMBERS
//     ;
pan : Pan ;
// vibrato : Vibrato NUMBERS COMMA NUMBERS COMMA NUMBERS
//     | Vibrato NUMBERS COMMA NUMBERS
//     ;
vibrato : Vibrato ;
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
globalVolume : GlobalVolume ;

// tempo : Tempo NUMBERS COMMA NUMBERS
//     | Tempo NUMBERS
//     ;
tempo : Tempo ;

instrument : Instrument ;

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
    | volume
    | tune
    // | instrument
    | quantization
    | pan
    | vibrato
    | tempo
    | HexNumber
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
sampleLoad : LoadSample ;

replacements : ReplacementText ;

hexNumber : HexNumber ;

// End Miscellaneous Commands

/*
 * Lexer Rules
 */
POUND : '#' ;
DOLLAR : '$' ;
COMMAT : '@' ;
AMPER : '&' ;
BANG : '!' ;
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

fragment NUMBER : [0-9] ;
NUMBERS : NUMBER+ ;
UNUMBERS : [\-]?NUMBER+ ;

fragment QMARKVALUES : [0-2] ;
fragment CHANNELS : [0-7] ;
fragment QUANTIZATION : [0-7] ;
fragment OCTAVES : [0-6] ;
// NOTEDURATIONS : ONE | TWO | FOUR | EIGHT | SIXTEEN | THIRTYTWO | SIXTYFOUR ;
REMOTECODENUMBERS : NEGATIVEONE | ZERO | ONE | TWO | THREE | FOUR | SEVEN | EIGHT ; 

ReplacementText : DQUOTE ~[=\n\r"]+ EQUAL ~[\n\r"]+ DQUOTE ;
StringLiteral : DQUOTE ~[\n\r"]+ DQUOTE ;
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
Noise : N HexDigits+ ;
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

LoopName : LPAREN ( NUMBERS | StringLiteral ) RPAREN ;
RemoteCodeName : LPAREN BANG ( NUMBERS | StringLiteral ) RPAREN ;
StopRemoteCode : LPAREN BANG BANG REMOTECODENUMBERS RPAREN ;
// CallLoop : LoopName NUMBERS? ;
// CallLoop : LPAREN ( NUMBERS | StringLiteral ) RPAREN NUMBERS? ;
CallRemoteCode : LPAREN BANG ( NUMBERS | StringLiteral ) COMMA REMOTECODENUMBERS (COMMA NUMBERS)? RPAREN ;
CallPreviousLoop : STAR NUMBERS? ;
LoadSample : LPAREN StringLiteral COMMA HexNumber RPAREN ;

AmkV1 : AMKV1 ;

fragment HexDigit : [0-9a-f] ;
HexDigits : HexDigit+ ;
HexNumber : DOLLAR HexDigits ;
Instrument : COMMAT [0-9]+ ;

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

Tempoimmunity : TEMPOIMMUNITY ;
Dividetempo : DIVIDETEMPO ;
Smwvtable : SMWVTABLE ;
Nspcvtable : NSPCVTABLE ;
Noloop : NOLOOP ;
Amk109hotpatch : AMK109HOTPATCH ;

Channel : POUND CHANNELS ;
SampleOptimization : POUND [a-z0-9]+ ;

WHITESPACE : ( ' '|'\t' | '\r' | '\n' )+ -> skip ;
Comment : SEMICOLON ~[\r\n]* -> skip ;

//Error : . -> skip ;
