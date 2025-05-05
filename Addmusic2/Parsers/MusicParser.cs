/*using Pidgin;
using Pidgin.Expression;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Addmusic2.Model.Constants;

using static Pidgin.Parser;

namespace Addmusic2.Parsers
{
    internal static class MusicParser
    {

        public Result<char, SongNode> Parse(string input) => Try();


        private static Parser<char, T> Token<T>(Parser<char, T> parser) => Try(parser).Before(SkipWhitespaces);
        private static Parser<char, string> Token(string token) => Token(String(token));
        private static Parser<char, char> Token(char token) => Token(Char(token));


        private static Parser<char, T> KeyWord<T>(Parser<char, T> parser) => Token(parser.Before(Lookahead(Whitespace)));
        private static Parser<char, char> KeyWord(char value) => KeyWord(Char(value));
        private static Parser<char, string> KeyWord(string value) => KeyWord(String(value));
        

        private static Parser<char, char> _pound = Token('#');
        private static Parser<char, char> _dollar = Token('$');
        private static Parser<char, char> _openParen = Token('(');
        private static Parser<char, char> _closeParen = Token(')');
        private static Parser<char, char> _openBracket = Token('[');
        private static Parser<char, char> _closeBracket = Token(']');
        private static Parser<char, char> _openBrace = Token('{');
        private static Parser<char, char> _closeBrace = Token('}');
        private static Parser<char, char> _greaterThan = Token('>');
        private static Parser<char, char> _lessThan = Token('<');
        private static Parser<char, char> _doubleQuote = Token('"');

        private static Parser<char, char> _noteChars = Letter.Or(OneOf(SongElements.ValidNoteCharacters));
        private static Parser<char, char> _channelNumbers = Letter.Or(OneOf(SongElements.ValidChannelNumbers));

        private static Parser<char, T> Parenthesised<T>(Parser<char, T> p)
            => p.Between(_openParen, _closeParen);
        private static Parser<char, T> BracketEnclosed<T>(Parser<char, T> p)
            => p.Between(_openBracket, _closeBracket);
        private static Parser<char, T> BraceEnclosed<T>(Parser<char, T> p)
            => p.Between(_openBrace, _closeBrace);


        private static readonly Parser<char, string> _specialDirective = Token(
                from first in _pound
                from rest in OneOf(Letter, Digit).ManyString()
                select first + rest
            );
        
        private static readonly Parser<char, MusicNode> _channelDirective = Token(
                from first in _pound
                from rest in _channelNumbers
                select $"{first}{rest}"
            )
            .Select(n => MusicNodeBuilder.ChannelGroup(n))
            .Labelled("channel");



        private static readonly Parser<char, MusicNode> _songParser = Rec



        private static Parser<char, char> _noteLetters = Letter.Or(OneOf(''));

        private static readonly Parser<char, string> _Directives = Letter.Assert()

        private static readonly Parser<char, string> _DirectiveStrings = Token(
                ValidSpecialDirectives
            ) //CIString(OneOf(ValidSpecialDirectives.ToArray()))
    }
}
*/