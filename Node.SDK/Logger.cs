using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.SDK
{
    public class Logger
    {

        public static void LogError(string message)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now.ToString()}: {message}");
            Console.ResetColor();
        }

        public static void LogWarning(string message)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{DateTime.Now.ToString()}: {message}");
            Console.ResetColor();
        }

        public static void LogInfo(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToString()}: {message}");
        }
    }
}
