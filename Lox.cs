namespace Lox
{
    internal class Lox
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: clox [script]");
                return;
            }
            else if (args.Length == 1)
            {
                RunFile(args[0]);
            }
            else
            {
                RunPrompt();
            }
        }

        private static void RunFile(string pathName)
        {
            byte[] bytes = File.ReadAllBytes(Path.GetFullPath(pathName));
            // run bytes
        }

        private static void RunPrompt()
        {
            while (true)
            {
                Console.WriteLine("> ");

                var line = Console.ReadLine();

                if (line is null) { break; }

                Run(line);
            }
        }

        private static void Run(string source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.scanTokens();

            tokens.ForEach(t => Console.WriteLine(t.ToString()));
        }
    }
}
