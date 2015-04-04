using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniAuditor.Classes;
using System.Threading;


namespace MiniAuditor
{
    class Program
    {
        static void Main(string[] args)
        {
            SystemInfo si = Auditor.GetSystemInfo();

            si.Save();

            Console.WriteLine("Done!");
            Thread.Sleep(500);
        }
    }
}
