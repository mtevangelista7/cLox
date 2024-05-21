namespace Lox;

public abstract class Expr
{
    public abstract T Accept<T>(IVisitor<T> visitor);

    /// <summary>
    /// A interface IVisitor T possui vários métodos responsáveis
    /// por definir o que deve ser feito com uma expressão binária,
    /// uma expressão de agrupamento etc quando um visitante as visita
    /// </summary>
    public interface IVisitor<T>
    {
        T VisitBinaryExpr(Binary expr);
        T VisitGroupingExpr(Grouping expr);
        T VisitLiteralExpr(Literal expr);
        T VisitUnaryExpr(Unary expr);
    }

    #region Cada uma dessas classes representa uma expressão dentro da árvore de sintaxe abstrata AST

    /// <summary>
    /// representa uma expressão binária para a árvore de sintaxe abstrata AST
    /// </summary>
    public class Binary : Expr
    {
        public Expr left;
        public Token operat;
        public Expr right;

        public Binary(Expr left, Token operat, Expr right)
        {
            this.left = left;
            this.operat = operat;
            this.right = right;
        }

        /// <summary>
        /// Este método é uma implementação do método Accept do padrão Visitor
        /// Aqui estamos aceitando um visitante, e delegando a ele a tarefa
        /// de processar a expressão binária chamando o método VisitBinaryExpr no visitante
        /// </summary>
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitBinaryExpr(this);
        }
    }

    public class Grouping : Expr
    {
        public Expr expression;

        public Grouping(Expr expression)
        {
            this.expression = expression;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitGroupingExpr(this);
        }
    }

    public class Literal : Expr
    {
        public Object value;

        public Literal(Object value)
        {
            this.value = value;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitLiteralExpr(this);
        }
    }

    public class Unary : Expr
    {
        public Token operat;
        public Expr right;

        public Unary(Token operat, Expr right)
        {
            this.operat = operat;
            this.right = right;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }
    }
    #endregion
}
