namespace Lox;

public abstract class Expr
{

    public abstract T Accept<T>(IVisitor<T> visitor);

    public interface IVisitor<T>
    {
        T VisitBinaryExpr(Binary expr);
        T VisitGroupingExpr(Grouping expr);
        T VisitLiteralExpr(Literal expr);
        T VisitUnaryExpr(Unary expr);
    }

    public class Binary : Expr
    {
        private Expr left;
        private Token operat;
        private Expr right;

        public Binary(Expr left, Token operat, Expr right)
        {
            this.left = left;
            this.operat = operat;
            this.right = right;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitBinaryExpr(this);
        }
    }

    public class Grouping : Expr
    {
        private Expr expression;

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
        private Object value;

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
        private Token operat;
        private Expr right;

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
}
