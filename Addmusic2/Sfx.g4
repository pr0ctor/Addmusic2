grammar Sfx;
options { caseInsensitive=true; }
/*
 * Parser Rules
 */
soundEffect : e0SfxPriority? soundEffectElement+
    | EOF
    ;

soundEffectElement : specialDirective
    | atomics
    | hexCommands
    ;

// Special Directives

specialDirective : asm
    | jsr
    ;

asm : Asm JsrIdentifier AsmTextBlock ;
jsr : Jsr JsrIdentifier ;

// End Special Directives

// Atomics

atomics: pitchslide
    | note
    | rest
    | octave
    | lowerOctave
    | raiseOctave
    | volumeCommand
    | defaultLength
    | instrumentCommand
    | nakedTie
    | triplet
    ;

note : Note ;
rest : Rest ;
octave : Octave ;
lowerOctave : LT ;
raiseOctave : GT ;
pitchslide : ( Note | Rest ) ( AMPER ( Note | Rest ) )+ ;
volumeCommand : Volume # Volume
    // | ( e7Volume | e8VolumeFade ) # HexVolume
    ;
defaultLength : Length ;
// instruments are different from the song mml instruments
instrumentCommand : Instrument # Instrument
    //| daInstrument # HexInstrument
    ;

triplet : LBRACE (note | rest) (note | rest) (note | rest) RBRACE
    ;

nakedTie : Tie ;

// End Atomics

// Hex Commands

hexCommands : hexNumber ;

e0SfxPriority : NE0 HexNumber ;

hexNumber : HexNumber ;

// End Hex Commands

/*
 * Lexer Rules
 */

POUND : '#' ;
DOLLAR : '$' ;
COMMAT : '@' ;
AMPER : '&' ;
LBRACE : '{' ;
RBRACE : '}' ;
SHARP : '+' ;
FLAT : '-' ;
DOT : '.' ;
TIE : '^' ;
GT : '>' ;
LT : '<' ;
COMMA : ',' ;
EQUAL : '=' ;
SEMICOLON : ';' ;
fragment PIPE : '|' ;

fragment OCTAVES : [0-6] ;

fragment A : ('a') ;
fragment B : ('b') ;
fragment C : ('c') ;
fragment D : ('d') ;
fragment E : ('e') ;
fragment F : ('f') ;
fragment G : ('g') ;
fragment O : ('o') ;
fragment R : ('r') ;
fragment V : ('v') ;
fragment L : ('l') ;
fragment BasicNote : A | B | C | D | E | F | G ;
Note : BasicNote ( SHARP | FLAT )? EQUAL? NUMBERS? DOT* ( Tie )* ;
Rest : R EQUAL? NUMBERS? DOT* ( Tie )* ;
Octave : O OCTAVES ;
Volume : V NUMBERS ( COMMA NUMBERS )? ;
Tie : TIE EQUAL? NUMBERS DOT* ;
Instrument : COMMAT NUMBERS ( COMMA HexDigits)? ;
Length : L NUMBERS ;



fragment ASM : ('asm') ;
fragment JSR : ('jsr') ;
Asm : POUND ASM ;
Jsr : POUND JSR ;


fragment NFE0 : ('$E0') ;

NE0 : NFE0 ;


JsrIdentifier : [a-z0-9_-]+ ;
AsmTextBlock : LBRACE ( ~( '}' | '\n' )* '\n' )+ RBRACE ;

fragment NUMBER : [0-9] ;
NUMBERS : NUMBER+ ;
UNUMBERS : [\-]?NUMBER+ ;

fragment HexDigit : [0-9a-f] ;
fragment HexDigits : HexDigit+ ;
HexNumber : DOLLAR HexDigits ;

WHITESPACE : ( ' '|'\t' | '\r' | '\n' )+ -> skip ;
Comment : SEMICOLON ~[\r\n]* -> skip ;