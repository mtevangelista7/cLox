namespace Lox
{
    internal class Lox
    {
        static bool hadError = false;

        static void Main(string[] args)
        {
            switch (args.Length)
            {
                case > 1:
                    Console.WriteLine("Usage: clox [script]");
                    return;
                case 1:
                    RunFile(args[0]);
                    break;
                default:
                    RunPrompt();
                    break;
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
                Console.Write("> ");

                var line = Console.ReadLine();

                if (line is null) { break; }

                Run(line);
                hadError = false;
            }
        }

        private static void Run(string source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();

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
