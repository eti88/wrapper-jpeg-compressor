using System;
using System.Diagnostics;
using System.IO;

/// <summary>
/// Authot: Etienne Tomaselli
/// Version: 1.0
/// </summary>

namespace JpegOptimizationForWeb
{
    class Program
    {
        // Enumeration for exit codes
        enum ExitCode : int
        {
            Success = 0,
            InvalidOption = 1,
            InvalidPath = 2,
            InvalidArgumentsNumber = 3,
            ExternalExeNotIncluded = 4,
            UnknownError = 5
        }

        static void Main(string[] args)
        {
            string option = "";
            string[] jpgFiles;

            if (args.Length != 3)
            {
                ProgramCmdHelp();
                Environment.Exit((int) ExitCode.InvalidArgumentsNumber);
            }

            // Check the compression type
            if (CheckCompressionOpt(args[0]))
            {
                option = "--method " + args[0].ToLower();
            }
            else
            {
                ProgramCmdHelp();
                Environment.Exit((int)ExitCode.InvalidOption);
            }

            // Check both paths (source and destination)
            if(args.Length == 3)
            {
                if (!(FileOrDirectoryExists(args[1]) && FileOrDirectoryExists(args[2])))
                    Environment.Exit((int)ExitCode.InvalidPath);
            }
            

            string src_path = args[1];
            string out_path = args[2];

            if ((Directory.GetFiles(src_path).Length == 0) && (Directory.GetFiles(src_path, "*.jpg").Length == 0))
            {
                Environment.Exit((int) ExitCode.UnknownError);
            }
            else
            {
                jpgFiles = Directory.GetFiles(src_path, "*.jpg");
                
                foreach (var photo in jpgFiles)
                {
                    LaunchExternalExe(photo, option, out_path);
                }
                
            }
            
            
            Console.WriteLine("Done!");
            Environment.Exit((int) ExitCode.Success);
        }

        static void ProgramCmdHelp()
        {
            System.Console.WriteLine("Usage: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " [OPTION] [SOURCE DIR] [DESTINATION DIR]");
            System.Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " is a wrapper for jpeg-archive tools (https://github.com/danielgtaylor/jpeg-archive). This great tool decrase the dimension of yor image.\n\n");
            System.Console.WriteLine("Examples:");
            System.Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name +  " ssim C:\\source\\dir C:\\dest\\dir # Compress all source images whit ssim algorithm");
            System.Console.WriteLine("\nMain operation mode:");
            System.Console.WriteLine("\t [type] select the compression algorithm compression type:");
            System.Console.WriteLine("\t\tmpe \t\tMean pixel error");
            System.Console.WriteLine("\t\tssim \t\tStructural similarity");
            System.Console.WriteLine("\t\tms-ssim \tMulti-scale structural similarity (slow!)");
            System.Console.WriteLine("\t\tsmallfry \tLinear-weighted BBCQ-like");
        }

        static bool CheckCompressionOpt(string compressionType)
        {
            bool result = false;
            compressionType = compressionType.ToLower();
            switch (compressionType)
            {
                case "mpe":
                case "ssim":
                case "ms-ssim":
                case "smallfry":
                    result = true;
                    break;
                default:
                    Console.WriteLine("Specified value not valid OR invalid, proceed with default compression: ssim");
                    break;
            }
            return result;
        }

        static bool FileOrDirectoryExists(string name)
        {
            return (Directory.Exists(name) || File.Exists(name));
        }

        // Start new process with the compressor with the parameters.
        static void LaunchExternalExe(string fileName,  string opt, string out_path)
        {
            string srcPath = fileName;
            string outPath = out_path + @"\";

            if (ExternalExeExist())
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.CreateNoWindow = true;
                start.UseShellExecute = false;
                start.FileName = Directory.GetCurrentDirectory() + @"\JpegCompressor\jpeg-recompress.exe";
                start.WindowStyle = ProcessWindowStyle.Hidden;
                start.Arguments = "" + opt + " " + srcPath + " " + outPath + Path.GetFileNameWithoutExtension(srcPath) + ".jpg";

                try
                {
                    // Start the process
                    using (Process exeProc = Process.Start(start))
                    {
                        exeProc.WaitForExit();
                        Console.WriteLine(".::[CONVERTED]" + Path.GetFileNameWithoutExtension(srcPath));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(".::[ERROR]: " + e.ToString());
                    Environment.Exit((int)ExitCode.UnknownError);
                }
            }
            else
            {
                Environment.Exit((int)ExitCode.ExternalExeNotIncluded);
            }
        }

        static bool ExternalExeExist()
        {
            if (FileOrDirectoryExists(Directory.GetCurrentDirectory() + @"\JpegCompressor\") &&
                FileOrDirectoryExists(Directory.GetCurrentDirectory() + @"\JpegCompressor\jpeg-compare.exe") &&
                FileOrDirectoryExists(Directory.GetCurrentDirectory() + @"\JpegCompressor\jpeg-hash.exe") &&
                FileOrDirectoryExists(Directory.GetCurrentDirectory() + @"\JpegCompressor\jpeg-recompress.exe"))
            {
                return true;
            }
            return false;
        }
    }
}
