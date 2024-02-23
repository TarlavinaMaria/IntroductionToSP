using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Threads
{
    internal class Program
    {
        static bool finish = false;
        static bool suspend_plus = false;
        static bool suspend_minus = false;
        static void Main(string[] args)
        {
            //Plus();
            //Minus();
            Thread plus_thread = new Thread(Plus);
            Thread minus_thread = new Thread(Minus);
            plus_thread.Start();
            minus_thread.Start();
            Console.WriteLine($"{Thread.CurrentThread.GetHashCode()}");
            Console.WriteLine("Press any key to stop");
            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key; 
                switch(key) 
                { 
                    case ConsoleKey.Escape: finish = true; break;
                    case ConsoleKey.OemPlus: plus_thread.Thr plus_thread.Suspend(); break;
                }
            } while (key != ConsoleKey.Escape);
        }
        static void Plus()
        {
            while (!finish)
            {
                Console.Write($"+ {Thread.CurrentThread.GetHashCode()}\t");
                Thread.Sleep(500);
            }

        }
        static void Minus()
        {
            while (!finish)
            {
                
                Console.Write($"- {Thread.CurrentThread.GetHashCode()}\t");
                Thread.Sleep(500);
            }

        }
    }
}
