namespace Lox
{
    internal class Lox
    {
        static bool hadError = false;

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
            Run(bytes.ToString()!);
            if (hadError) return;
        }

        private static void RunPrompt()
        {
            while (true)
            {
                Console.WriteLine("> ");

                var line = Console.ReadLine();

                if (line is null) { break; }

                Run(line);
                hadError = false;
            }
        }

        private static void Run(string source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.scanTokens();

            tokens.ForEach(t => Console.WriteLine(t.ToString()));
        }

        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        private static void Report(int line, string where, string message)
        {
            Console.WriteLine($"[line {line}] Error {where}: {message}");
        }


    }
}
