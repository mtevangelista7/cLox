using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lox.Expr;

namespace Lox
{
    internal class AstPrinter : IVisitor<string>
    {
        string Print(Expr expr)
        {
            return expr.Accept(this);
        }

        string ParentheSize(string name, params Expr[] exprs)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append('(').Append(name);

            foreach (Expr e in exprs)
            {
                stringBuilder.Append(' ');
                stringBuilder.Append(e.Accept(this));
            }
            stringBuilder.Append(')');

            return stringBuilder.ToString();
        }

        public string VisitBinaryExpr(Binary expr)
        {
            throw new NotImplementedException();
        }

        public string VisitGroupingExpr(Grouping expr)
        {
            throw new NotImplementedException();
        }

        public string VisitLiteralExpr(Literal expr)
        {
            throw new NotImplementedException();
        }

        public string VisitUnaryExpr(Unary expr)
        {
            throw new NotImplementedException();
        }
    }
}
