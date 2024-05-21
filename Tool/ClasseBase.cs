using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{

    // ESSE ARQUIVO É UM EXEMPLO DE COMO O PADRÃO VISITOR PODE SER UTILIZADO
    // ELE NÃO FAZ PARTE DO PROJETO LOX E NÃO TEM NENHUMA UTILIDADE ALÉM DE EXEMPLIFICAR O PADRÃO
    // DEIXEI PRESENTE AQUI PARA FINS DE ESTUDO JÁ QUE DESSA FORMA ENTENDI MELHOR O PADRÃO


    // QUAL O PROBLEMA AQUI? IMAGINA TODAS AS VEZES QUE PRECISAR ADICIONAR UM NOVO MÉTODO
    // NA CLASSE BASE, TODAS AS CLASSES DERIVADAS PRECISAM SER ALTERADAS
    // OU TODAS AS CLASSES DERIVADAS PRECISAM IMPLEMENTAR O MÉTODO DE COM UM COMPORTAMENTO DIFERENTE

    public abstract class ClasseBase
    {
        public abstract void MetodoClasseBase();
    }

    public class ClasseA : ClasseBase
    {
        // AQUI ESTAMOS SUPONDO QUE ESSE MÉTODO DA CLASSE BASE TEM UM FUNCIONAMENTO DIFERENTE NA CLASSE A
        public override void MetodoClasseBase()
        {
            throw new NotImplementedException();
        }
    }

    public class ClasseB : ClasseBase
    {
        // AQUI ESTAMOS SUPONDO QUE ESSE MÉTODO DA CLASSE BASE TEM UM FUNCIONAMENTO DIFERENTE NA CLASSE B
        public override void MetodoClasseBase()
        {
            throw new NotImplementedException();
        }
    }

    // OK ISSO É RUIM, MAS COMO POSSO RESOLVER?
    // COMO POSSO ADICIONAR NOVAS FUNCIONALIDADES SEM ALTERAR AS CLASSES DERIVADAS?
    // R: USAMOS O PADRÃO VISITOR

    // Cada operação que pode ser realizada na classe base
    // é uma nova classe que implementa essa interface abaixo
    // ela possui um método para cada classe derivada (classe ClasseAUsandoVisitor e classe ClasseBUsandoVisitor)

    public interface IClasseBaseVisitor
    {
        void VisitClasseA(ClasseAUsandoVisitor classeA);
        void VisitClasseB(ClasseBUsandoVisitor classeB);
    }

    public class ClasseAUsandoVisitor : ClasseBaseUsandoVisitor
    {
        public override void Accept(IClasseBaseVisitor visitor)
        {
            visitor.VisitClasseA(this);
        }
    }

    public class ClasseBUsandoVisitor : ClasseBaseUsandoVisitor
    {
        public override void Accept(IClasseBaseVisitor visitor)
        {
            visitor.VisitClasseB(this);
        }
    }

    // certo, agora dado uma classe base como roteamos a chamada para o método correto
    // com base em qual classe derivada estamos lidando

    // nós criamos um método Accept na classe base que recebe um visitante
    // logo a classe derivada que implementa esse método, chama o método correto do visitante

    // Ou seja, para realizar uma operação em uma classe base, chamamos o seu método Accept
    // e passamos um visitante, esse visitante é responsável por realizar a operação correta
    // como esse método é sobreescrito em cada classe derivada, ele chama o método correto do visitante

    public abstract class ClasseBaseUsandoVisitor
    {
        public abstract void Accept(IClasseBaseVisitor visitor);
    }
}
