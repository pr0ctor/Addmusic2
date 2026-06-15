grammar Preprocessor;
options { caseInsensitive=true; }


song : preprocessorElement+ EOF
    | EOF
    ;

preprocessorElement : replacements
    | amk
    | conditionalLogics
    | defines
    | arbitraryText
    ;

replacements : ReplacementText ;

amk : generalAmk # GeneralAmkVersion
    | amm # AmmVersion
    | am4 # Am4Version
    | amkV1 # AmkV1Version
    ;
generalAmk : Amk ;
amm : Amm ;
am4 : Am4 ;
amkV1 : AmkV1 ;


defines : define
    | undefine
    | error
    ;

define : DEFINE ;

undefine : UNDEF ;

error : ERROR ;

conditionalLogics : if
    | ifdef
    | ifndef
    ;

if : IF ( arbitraryText | error )+ else? ENDIF # SingleIfStatement
    | IF ( arbitraryText | error )+ elseIf+ else? ENDIF  # IfStatementWithElseIfs
    ;

elseIf : ELSEIF ( arbitraryText | error )+ ;

else : ELSE ( arbitraryText | error )+ ;

ifdef : IFDEF ( arbitraryText | error )+ ENDIF ;

ifndef : IFNDEF ( arbitraryText | error )+ ENDIF ;


arbitraryText : ArbitraryText ;

// lexer

fragment AMK : ('#amk') ;
fragment AMM : ('#amm') ;
fragment AM4 : ('#am4') ;
fragment AMKV1 : ('=1') ;

Amk : AMK ATOMIC_WHITESPACE [0-9]+ ;
Amm : AMM ;
Am4 : AM4 ;
AmkV1 : AMK AMKV1 ;

fragment EQUAL : '==' ;
fragment NOTEQUAL : '!=' ;
fragment GT : '>' ;
fragment GTEQ : '>=' ;
fragment LT : '<' ;
fragment LTEQ : '<=' ;

fragment Operators : EQUAL | NOTEQUAL | GT | GTEQ | LT | LTEQ ;

fragment ATOMIC_WHITESPACE : ( ' ' | '\t' )+ ;
fragment NOT_NEWLINE : ~[\r\n] ;
fragment NOT_COMMENT_OR_NEWLINE : ~[\r\n;] ;
fragment QUOTED_ARBITRARY_TEXT : ~( '\n' | '\r' | '"' )+ ;

DEFINE : NOT_NEWLINE? '#define' ATOMIC_WHITESPACE ArbitraryText ( ATOMIC_WHITESPACE ArbitraryText )? NOT_COMMENT_OR_NEWLINE;
IFDEF : NOT_NEWLINE? '#ifdef' ATOMIC_WHITESPACE ArbitraryText NOT_COMMENT_OR_NEWLINE;
IFNDEF : NOT_NEWLINE? '#ifndef' ATOMIC_WHITESPACE ArbitraryText NOT_COMMENT_OR_NEWLINE ;
UNDEF : NOT_NEWLINE? '#undef' ATOMIC_WHITESPACE ArbitraryText NOT_COMMENT_OR_NEWLINE ;
IF : NOT_NEWLINE? '#if' ATOMIC_WHITESPACE ArbitraryText ATOMIC_WHITESPACE? Operators ATOMIC_WHITESPACE? ArbitraryText NOT_COMMENT_OR_NEWLINE;
ELSE : NOT_NEWLINE? '#else' NOT_COMMENT_OR_NEWLINE ;
ELSEIF : NOT_NEWLINE? '#elseif' ATOMIC_WHITESPACE ArbitraryText ATOMIC_WHITESPACE? Operators ATOMIC_WHITESPACE? ArbitraryText NOT_COMMENT_OR_NEWLINE ;
ENDIF : NOT_NEWLINE? '#endif' ; // adding a NOT_COMMENT_OR_NEWLINE at the end of this item breaks token recognition
ERROR : NOT_NEWLINE? '#error' ( ATOMIC_WHITESPACE? '"' QUOTED_ARBITRARY_TEXT '"' )? NOT_COMMENT_OR_NEWLINE ;


ReplacementText : '"' ~[=\n\r"]+ '=' ~[\n\r"]+ '"' ;


WHITESPACE : ( ' ' | '\t' | '\r' | '\n' )+ -> skip ;
Comment : ';' ~[\r\n]* -> skip ;

ArbitraryText : ~( ' ' | '\t' | '\n' | ';' | '\r' )+ ;