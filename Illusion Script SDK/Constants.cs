using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace IllusionScript.SDK
{
    public static class Constants
    {
        public const string TAB = "    ";
        public static readonly string EOL = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "\n\r" : "\n";

        public static readonly string ESCAPED_EOL =
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "\\n\\r" : "\\n";

        public static readonly char[] IGNORE_CHARACTERS =
        {
            ' ',
            '\t',
            '\r'
        };

        public static readonly Dictionary<char, char> ESCAPE_CHARACTERS = BuildEscapeCharacters();

        public static readonly char[] DIGITS =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        public static readonly char[] LETTERS =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
            'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
            'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        public static readonly char[] LETTERS_DIGITS =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
            'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
            'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        public static Dictionary<char, char> BuildEscapeCharacters()
        {
            var list = new Dictionary<char, char>();
            list['n'] = '\n';
            list['t'] = '\t';
            list['r'] = '\r';

            return list;
        }

        public static class TT
        {
            public const string INT = "INT";
            public const string FLOAT = "FLOAT";
            public const string STRING = "STRING";

            public const string IDENTIFIER = "IDENTIFIER";
            public const string KEYWORD = "KEYWORD";
            public const string HEAD_KEYWORD = "HEAD_KEYWORD";

            public const string PLUS = "PLUS";
            public const string MINUS = "MINUS";
            public const string MUL = "MUL";
            public const string DIV = "DIV";
            public const string POW = "POW";
            public const string MODULO = "MODULO";

            public const string EQUALS = "EQUALS";
            public const string DOUBLE_EQUALS = "DOUBLE_EQUALS";
            public const string NOT_EQUALS = "NOT_EQUALS";
            public const string LESS_THAN = "LESS_THAN";
            public const string GREATER_THAN = "GREATER_THAN";
            public const string LESS_EQUALS = "LESS_EQUALS";
            public const string GREATER_EQUALS = "GREATER_EQUALS";

            public const string LPAREN = "LPAREN";
            public const string RPAREN = "RPAREN";
            public const string LBRACKET = "LBRACKET";
            public const string RBRACKET = "RBRACKET";
            public const string LCURLY_BRACKET = "LCURLY_BRACKET";
            public const string RCURLY_BRACKET = "RCURLY_BRACKET";

            public const string COMMA = "COMMA";
            public const string DOT = "DOT";
            public const string ACCESS_ARROW = "ACCESS_ARROW";
            public const string ARROW = "ARROW";

            public const string NEWLINE = "NEWLINE";
            public const string EOF = "EOF";
        }

        public static class Keyword
        {
            public const string VAR = "let";
            public const string CONST = "const";
            public const string FIELD = "field";

            public const string AND = "and";
            public const string OR = "or";
            public const string NOT = "not";

            public const string IF = "if";
            public const string THEN = "then";
            public const string ELSE_IF = "elif";
            public const string ELSE = "else";

            public const string FOR = "for";
            public const string TO = "to";
            public const string STEP = "step";
            public const string WHILE = "while";

            public const string FUNCTION = "define";
            public const string METHOD = "method";
            public const string CLASS = "class";

            public const string NEW = "new";
            public const string EXTENDS = "extends";
            public const string THIS = "this";

            public const string PRIVATE = "private";
            public const string PUBLIC = "public";
            public const string STATIC = "static";

            public const string END = "end";

            public const string RETURN = "return";
            public const string CONTINUE = "continue";
            public const string BREAK = "break";

            public const string EXPORT = "export";

            public static readonly string[] KEYWORDS =
            {
                VAR,
                CONST,
                FIELD,
                AND,
                OR,
                NOT,
                IF,
                THEN,
                ELSE_IF,
                ELSE,
                FOR,
                TO,
                STEP,
                WHILE,
                FUNCTION,
                CLASS,
                METHOD,
                NEW,
                EXTENDS,
                PRIVATE,
                PUBLIC,
                STATIC,
                END,
                RETURN,
                CONTINUE,
                BREAK,
                EXPORT
            };
        }

        public static class HeadKeyword
        {
            public const string IMPORT = "@import";

            public const string END = "@end";

            public const string IF = "@if";
            public const string ELSE_IF = "@elif";
            public const string ELSE = "@else";

            public const string PACKAGE = "@package";
            public const string USE = "@use";

            public static readonly string[] KEYWORDS =
            {
                IMPORT,
                END,
                IF,
                ELSE_IF,
                ELSE,
                PACKAGE,
                USE
            };
        }

        public static class Config
        {
            public static bool FileImport = false;
            public static bool FileExport = false;
        }
    }
}