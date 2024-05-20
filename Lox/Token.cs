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
        //  um lexeme é um agrupamento de caracteres que forma uma unidade significativa na linguagem de programação
        // exemplo: total = price * quantity + tax -> "total", "=", "price", "*", "quantity", "+", "tax"
        string lexeme = lexeme;
        object literal = literal;
        int line = line;

        public override string ToString()
        {
            return $"type: {type} lexeme: {lexeme} literal: {literal}";
        }
    }
}
