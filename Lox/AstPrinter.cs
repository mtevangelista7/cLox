using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lox.Expr;

namespace Lox
{
    /// <summary>
    /// AstPrinter é uma classe que implementa o padrão Visitor, ela é responsável por 
    /// imprimir a árvore de sintaxe abstrata (AST) em forma de string
    /// e isso é uma das operações que podem ser realizadas na classe base
    /// caso novas operações sejam necessárias, basta criar uma nova classe que implemente a interface IVisitor
    /// se não estiviéssemos usando o padrão Visitor, seria necessário alterar todas as classes derivadas
    /// literealmente entrar em cada classe deverivada e adicionar a sua propriá implementação de impressão da AST
    /// </summary>
    internal class AstPrinter : IVisitor<string>
    {
        /// <summary>
        /// Dentro do método, está sendo utilizado o padrão Visitor para percorrer a árvore de expressão.
        /// O método Accept é chamado no objeto expr, que é uma instância de uma classe derivada da classe base Expr.
        /// Este método Accept é responsável por executar a lógica de visitação apropriada com base no tipo real da expressão.
        ///
        /// Passa o this (a instância atual da classe AstPrinter) como o visitante para o método Accept.
        /// Isso permite que a expressão aceite o visitante e execute as ações necessárias com base em seu próprio tipo.
        /// </summary>
        private string Print(Expr expr)
        {
            // aqui é realizado o processo de aceitar um visitante
            // Expr é uma classe base, logo quando entrar nesse método
            // Já sabemos que é uma das classes derivadas
            // ou seja uma das expressões definidas no Expr.cs
            // Como já sabemos que é uma expressão, podemos chamar o método Accept
            // Esse Accept será executado já pela sua clase derivada (Expr é apenas a base)
            // Aqui passamos a classe AstPrinter como visitante
            return expr.Accept(this);
        }

        /// <summary>
        /// Cria um StringBuilder para construir uma representação em string de uma expressão entre parênteses.
        /// </summary>
        private string ParentheSize(string name, params Expr[] exprs)
        {
            StringBuilder stringBuilder = new();

            stringBuilder.Append('(').Append(name);

            foreach (Expr expression in exprs)
            {
                stringBuilder.Append(' ');

                // Método Accept é chamado em cada expressão,
                // passando a instância atual da classe AstPrinter como o visitante.
                // Isso permite que a expressão execute suas próprias ações com base em seu tipo
                stringBuilder.Append(expression.Accept(this));
            }

            stringBuilder.Append(')');

            return stringBuilder.ToString();
        }

        #region Visitantes (Visitors) -> métodos visitantes que executam a lógica de impressão da AST

        public string VisitBinaryExpr(Binary expr)
        {
            // expressão esquerda operador expressão direita
            return ParentheSize(expr.operat.lexeme, expr.left, expr.right);
        }

        public string VisitGroupingExpr(Grouping expr)
        {
            // (expression)
            return ParentheSize("group", expr.expression);
        }

        public string VisitLiteralExpr(Literal expr)
        {
            // nil -> null ou a expressão literal direto
            // aqui não é precisa usar o ParentheSize, ou o retorno
            // é um literal ou é nil
            // NUMBER | STRING | "true" | "false" | "nil"
            return expr.value is null ? "nil" : expr.value.ToString()!;
        }

        public string VisitUnaryExpr(Unary expr)
        {
            // por enquanto - ou ! para (! negação) e (- negativo)
            return ParentheSize(expr.operat.lexeme, expr.right);
        }

        #endregion

        // public static void Main(String[] args)
        // {
        //     //Expr expression =
        //     //    new Binary(
        //     //        new Unary(
        //     //            new Token(TokenType.MINUS, "-", null, 1),
        //     //            new Literal(123)),
        //     //        new Token(TokenType.STAR, "*", null, 1),
        //     //        new Grouping(
        //     //            new Literal(45.67)));
        //
        //     Expr expression = new Grouping(new Literal(123));
        //
        //     Console.WriteLine(new AstPrinter().Print(expression));
        // }
    }
}