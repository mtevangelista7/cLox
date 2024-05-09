using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lox
{
    public class Scanner(string source)
    {
        private string source = source;
        private List<Token> tokens = [];
        private int start = 0;
        private int current = 0;
        private int line = 1;

        private static readonly Dictionary<string, TokenType> keywords = new()
        {
            { "and", TokenType.AND },
            { "class", TokenType.CLASS },
            { "else", TokenType.ELSE },
            { "false", TokenType.FALSE },
            { "for", TokenType.FOR },
            { "fun", TokenType.FUN },
            { "if", TokenType.IF },
            { "nil", TokenType.NIL },
            { "or", TokenType.OR },
            { "print", TokenType.PRINT },
            { "return", TokenType.RETURN },
            { "super", TokenType.SUPER },
            { "this", TokenType.THIS },
            { "true", TokenType.TRUE },
            { "var", TokenType.VAR },
            { "while", TokenType.WHILE }
        };

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                // We are at the beginning of the next lexeme.
                start = current;
                ScanToken();
            }

            // no final adicionar o EOF e finalizamos a leitura desse token
            tokens.Add(new Token(TokenType.EOF, "", null, line));

            return tokens;
        }

        private bool IsAtEnd()
        {
            return current >= source.Length;
        }

        private void ScanToken()
        {
            char ch = Advance();
            switch (ch)
            {
                case '(':
                    AddToken(TokenType.LEFT_PAREN);
                    break;
                case ')':
                    AddToken(TokenType.RIGHT_PAREN);
                    break;
                case '{':
                    AddToken(TokenType.LEFT_BRACE);
                    break;
                case '}':
                    AddToken(TokenType.RIGHT_BRACE);
                    break;
                case ',':
                    AddToken(TokenType.COMMA);
                    break;
                case '.':
                    AddToken(TokenType.DOT);
                    break;
                case '-':
                    AddToken(TokenType.MINUS);
                    break;
                case '+':
                    AddToken(TokenType.PLUS);
                    break;
                case ';':
                    AddToken(TokenType.SEMICOLON);
                    break;
                case '*':
                    AddToken(TokenType.STAR);
                    break;
                case '!':
                    AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case '/':
                    // Caso seja um / existe a possibilidade de ser um comentário e não um sinal de divisão
                    if (Match('/'))
                    {
                        // Aqui precimos aguardar o final da linha (o que configura o final do comentário)
                        // Ou seja enquanto não for a quebra de linha e não estivermos no final, continua avançando
                        while (Peek() != '\n' && !IsAtEnd())
                        {
                            Advance();
                        }
                    }
                    // Caso o próximo char não seja um / podemos considerar que seja apenas o sinal de divisão
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    // aqui ignoramos os espaços em branco
                    break;
                case '\n':
                    // aqui caso chegue uma quebra de linha adicionamos mais uma linha
                    line++;
                    break;
                case '"':
                    // Aqui tratamos as strings -> sempre iniciam com " e terminam com "
                    String();
                    break;
                default:
                    // Caso não tenha parado em nenhuma condição acima
                    // Verificamos se o char é um número
                    if (IsDigit(ch))
                    {
                        Number();
                    }
                    // Se entrar nesse if e por que existe a possibilidade desse char fazer parte de uma
                    // palavra reservada da linguagem então é preciso analisar
                    else if (IsAlpha(ch))
                    {
                        Identifier();
                    }
                    else
                    {
                        // Caso não seja nenhum dos char esperados avisamos o usuário com um erro
                        Lox.Error(line, "Unexpected character.");
                    }
                    break;
            }
        }

        private char Advance()
        {
            // aqui pegamos o char atual e depois +1 na var atual para poder seguir para o próximo
            var ch = source[current];
            current++;
            return ch;
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object literal)
        {
            string text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }

        private bool Match(char expected)
        {
            // Se já estamos na última posição não tem o que comparar apenas retorna false
            if (IsAtEnd()) return false;

            // Aqui verificamos se o char atual é o esperado 
            if (source[current] != expected) return false;

            // Adicionamos +1 no atual para seguir para o proximo
            current++;

            return true;
        }

        private char Peek()
        {
            // Aqui verificamos se chegamos ao final do que o usuário digitou
            // Caso sim podemos retornar a quebra de linha
            // Configurando assim o final
            // usamos o '\0' (char nulo) pois dessa forma indicamos o final da string
            if (IsAtEnd())
                return '\0';

            // Agora caso não seja o último elemento, retornamos o char atual
            return source[current];
        }

        private void String()
        {
            // enquanto não char atual não for o fechamento da string e não estivermos no final do input
            while (Peek() != '"' && !IsAtEnd())
            {
                // Verificamos se antes de fechar a string o usuario quebrou uma linha
                if (Peek() == '\n')
                    line++;

                // Seguimos para o proximo char
                Advance();
            }

            // Caso tenha chegado nessá parte e for o final da linha, retornamos um erro
            // pois a string não foi finalizada
            if (IsAtEnd())
            {
                Lox.Error(line, "Unterminated string.");
                return;
            }

            Advance();

            string value = source.Substring(start + 1, current - start - 1);
            AddToken(TokenType.STRING, value);
        }

        private bool IsDigit(char ch)
        {
            // o char recebido está entre 0 e 9? caso sim é um digito
            return ch >= '0' && ch <= '9';
        }

        private void Number()
        {
            // se estamos aqui é pq o char atual é um número
            // agora verificamos se o próximo char e assim por adianta continua sendo um número
            // ou seja continuamos analisando para saber se todos os chars são apenas um valor (123456)
            while (IsDigit(Peek()))
                Advance();

            // Chegamos aqui se o próximo char não é um digito
            // porém antes de seguir precisamos lidar com a possibilidade dele ser um valor fracional (com .)
            // Aqui verifiamos se o atual é um ponto e se o próximo segue sendo um digito exemplo: (12.3)
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                // Aqui chamamos o advance para consumir o ponto e ir para o prox char (que é um digito)
                Advance();

                // Agora que já consumimos o ponto podemos seguir analisando os char para saber se os proximos
                // ainda não digitos, aqui não precisamos mais checar a possibilidade de um ponto pois seria considerado
                // inválido -> 12.3.4 -> isso não pode ser considerado um valor válido para a linguagem
                // TODO: não estamos tratando o caso do user colocar algo como (1.1.2) será gerado um erro não tratado
                while (IsDigit(Peek()))
                    Advance();
            }

            // Caso chegue aqui, finalizamos a leitura de número e podemos adicionar como token
            AddToken(TokenType.NUMBER,
                Double.Parse(source.Substring(start, current - start)));
        }

        private char PeekNext()
        {
            // Aqui verificamos se existe um proximo char 
            if (current + 1 >= source.Length)

                // Caso esteja no final do input, retornamos um char nulo 
                return '\0';

            // Caso exista o proximo char retornamos o mesmo
            return source[current + 1];
        }

        private void Identifier()
        {
            // Aqui ficamos em loop analisando os proximos chars até que seja extraido o identificador (palavra reservada da linguagem)
            while (IsAlphaNumeric(Peek()))
            {
                Advance();
            }

            // após analisar todos os char alfanúmericos juntos pegamos esse valor
            // e verificamos se ele é ou não uma palavra reservada
            string text = source.Substring(start, current - start);

            // Aqui tentamos verificar se existe uma palavra reservada
            if (!keywords.TryGetValue(text, out TokenType token))
            {
                // Caso não definimos como identificador pois pode ser o nome de uma variavel
                token = TokenType.IDENTIFIER;
            }

            AddToken(token);
        }

        private bool IsAlpha(char ch)
        {
            // Aqui verificamos o ch recibido é um alfa (letras ou um undescore)
            return
                (ch >= 'a' && ch <= 'z') ||
                (ch >= 'A' && ch <= 'Z') ||
                 ch == '_';
        }

        private bool IsAlphaNumeric(char c)
        {
            // Aqui retornamos se o char recebido é um alfa númerico (letras do alfabeto + undescore ou é um digito númerico)
            return IsAlpha(c) || IsDigit(c);
        }
    }
}