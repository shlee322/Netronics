using System.Reflection;

namespace Netronics.Ant
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length > 0)
            {
                Loader.Load(AntConfig.Load(args[0]));
                return;
            }

            ShowHelp();
        }

        public static void ShowHelp()
        {
            System.Console.WriteLine("Netronics Ant v" + Assembly.GetExecutingAssembly().GetName().Version + " Help Docs");
            System.Console.WriteLine("Ant.exe [file]");
            System.Console.WriteLine("[file] : Config File");
        }
    }
}
