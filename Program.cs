using System;
using System.IO;
using System.Linq;
using System.Text;

namespace SigFinder
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            StreamWriter LogFile = new StreamWriter(Path.Combine(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "SigFinder.txt")));
            string DirectoryForWork = "";

            if (args.Count() < 1)
            {
                Console.WriteLine("You need to specify a signature to search for, like \"CRID\". Example: \"SigFinder.exe CRID\" or \"SigFinder.exe CRID C:\\Directory\\To\\Search\\\"\nPress any key to close the program.");
                Console.ReadKey();
                return;
            }
            else if (args.Count() == 1)
            {
                DirectoryForWork = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            else
            {
                DirectoryForWork = args[1];
            }

            string SigToSearchFor = args[0];

            var buffer = new byte[4];
            var di = Directory.GetFiles(DirectoryForWork, "*", SearchOption.AllDirectories);

            Console.WriteLine("=== SigFinder (" + DateTime.Now + ") ===");
            LogFile.WriteLine("=== SigFinder (" + DateTime.Now + ") ===");

            Console.WriteLine("Found " + di.Length + " files in " + DirectoryForWork + "...");
            LogFile.WriteLine("Found " + di.Length + " files in " + DirectoryForWork + "...");
            int FilesFound = 0;
            foreach (var filePath in di)
            {
                if (filePath.Contains("SigFinder.txt")) { continue; }
                using (var stream = File.OpenRead(filePath))
                {
                    stream.Read(buffer, 0, SigToSearchFor.Length);

                    var signature = Encoding.ASCII.GetString(buffer);
                    if (signature == SigToSearchFor)
                    {
                        Console.WriteLine("Found an " + SigToSearchFor + " file: " + filePath.Replace(DirectoryForWork, ""));
                        LogFile.WriteLine("Found an " + SigToSearchFor + " file: " + filePath.Replace(DirectoryForWork, ""));
                        FilesFound++;
                    }
                }
            }
            Console.WriteLine("Process completed - Found " + FilesFound + " " + SigToSearchFor + " file(s).  Press any key to exit.");
            LogFile.WriteLine("== Process completed (" + DateTime.Now + ") - Found " + FilesFound + " " + SigToSearchFor + " file(s).  Press any key to exit. ===");
            LogFile.Close();
            Console.ReadKey();
        }
    }
}