namespace Tool
{
    public class GenerateAst
    {
        static void Main(string[] args)
        {
            // caso não tenha argumentos, a printa a mensagem
            if (args.Length != 1)
            {
                System.Console.Error.WriteLine("Usage: generate_ast <output directory>");
                return;
            }

            // pega o primeiro arg que deve ser um path
            string outputDir = args[0];

            // cria uma lista com todos os tipos de expressões
            List<string> types =
            [
                "Binary   : Expr left, Token operat, Expr right",
                "Grouping : Expr expression",
                "Literal  : Object value",
                "Unary    : Token operat, Expr right",
            ];

            // chama o método para definir a ast
            DefineAst(outputDir, "Expr", types);
        }

        /// <summary>
        /// Cria a classe Expressão onde definimos todas as expressões do interpretador (assim podemos transformar em json serializando)
        /// </summary>
        static void DefineAst(string outputDir, string baseName, List<string> types)
        {
            // monta o path para gerar
            string path = outputDir + "/" + baseName + ".cs";

            // abre um stream write e escreve no arquivo
            using StreamWriter streamWriter = new(path);
            streamWriter.WriteLine("namespace Lox;");
            streamWriter.WriteLine();
            streamWriter.WriteLine($"public abstract class {baseName} {"{"}");

            // aqui definimos o método que será sobreescrito
            streamWriter.WriteLine();
            streamWriter.WriteLine("    public abstract T Accept<T>(IVisitor<T> visitor);");
            streamWriter.WriteLine();

            // Define a interface visitor 
            DefineVisitor(streamWriter, baseName, types);

            // passa por cada um dos tipos para criar uma classe do mesmo que herda da classe base expr
            foreach (string type in types)
            {
                string className = type.Split(":")[0].Trim();
                string fields = type.Split(":")[1].Trim();

                // Passar o writer para continuar escrevendo no arquivo
                // o nome da classe base Expr
                // o nome da classe (um dos tipos)
                // o nome dos campos (propriedades) dessa classe
                DefineType(streamWriter, baseName, className, fields);
            }

            // fecha a classe
            streamWriter.WriteLine("}");
        }

        // implemetação do padrão visitante, criando sua interface
        // https://refactoring.guru/design-patterns/visitor
        static void DefineVisitor(StreamWriter streamWriter, string baseName, List<string> types)
        {
            // assinatura da interface
            streamWriter.WriteLine("    public interface IVisitor<T> {");

            // para cada tipo listado, criamos um método visit
            foreach (string type in types)
            {
                string typeName = type.Split(":")[0].Trim();

                streamWriter.WriteLine($"       T Visit{typeName}{baseName}({typeName} {baseName.ToLower()});");
            }

            // fecha a interface
            streamWriter.WriteLine("    }");
        }

        // TODO: Deixar em forma de construtor primário C#
        static void DefineType(StreamWriter streamWriter, string baseName, string className, string fieldList)
        {
            streamWriter.WriteLine();

            // assinatura da classe herdando da classe base Expr
            streamWriter.WriteLine($"   public class {className} : {baseName} {"{"}");

            // quebra todos os tipos + propriedades em um array
            string[] fields = fieldList.Split(",");

            // campos
            foreach (string field in fields)
            {
                streamWriter.WriteLine($"       private {field};");
            }

            streamWriter.WriteLine();
            // construtor com os parametros
            streamWriter.WriteLine($"    public {className}({fieldList}) {"{"}");

            // armazenando os parametros nos campos da classe
            foreach (string field in fieldList.Trim().Split(","))
            {
                string name = field.Trim().Split(" ")[1];
                streamWriter.WriteLine($"       this.{name} = {name};");
            }

            streamWriter.WriteLine("    }");

            // padrão visitante 
            // aqui implementamos o método abstrato da classe base
            streamWriter.WriteLine();
            streamWriter.WriteLine($"   public override T Accept<T>(IVisitor<T> visitor) {"{"}");
            streamWriter.WriteLine($"       return visitor.Visit{className}{baseName}(this);");
            streamWriter.WriteLine("    }");

            streamWriter.WriteLine("}");
        }
    }
}
