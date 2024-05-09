using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lox
{
    public class Token(TokenType type, string lexeme, object literal, int line)
    {
        TokenType type = type;
        string lexeme = lexeme;
        object literal = literal;
        int line = line;

        public override string ToString()
        {
            return $"{type} {lexeme} {literal}";
        }
    }
}
