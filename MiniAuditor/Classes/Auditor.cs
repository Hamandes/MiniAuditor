using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace MiniAuditor.Classes
{
    static class Auditor
    {
        static SystemInfo systemInfo = new SystemInfo();
        static bool isRunning = false;

        static string GetMachineName()
        {
            return Environment.MachineName;
        }

        public static SystemInfo GetSystemInfo()
        {
            RunCommand("systeminfo");
            return systemInfo;
        }

        static void RunCommand(string command)
        {
            try
            {
                //создаем новый процесс, который будет работать с консолью
                Process pr = new Process();
                //задаем имя запускного файла
                pr.StartInfo.FileName = command;
                //задаем аргументы для этого файла
                //pr.StartInfo.Arguments = "";
                //отключаем использование оболочки, чтобы можно было читать данные вывода
                pr.StartInfo.UseShellExecute = false;
                //перенаправляем данные вовода
                pr.StartInfo.RedirectStandardOutput = true;
                //задаем кодировку, чтобы читать кириллические символы
                pr.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(866);
                //запрещаем создавать окно для запускаемой программы               
                pr.StartInfo.CreateNoWindow = true;
                //подписываемся на событие, которые возвращает данные
                pr.OutputDataReceived += new DataReceivedEventHandler(sortOutputHandler);
                //включаем возможность определять когда происходит выход из программы, которую будем запускать
                pr.EnableRaisingEvents = true;
                //подписываемся на событие, когда процесс завершит работу
                pr.Exited += new EventHandler(whenExitProcess);
                //запускаем процесс
                pr.Start();
                isRunning = true;
                //начинаем читать стандартный вывод
                pr.BeginOutputReadLine();
                while (isRunning)
                {
                    Thread.Sleep(100);
                }

            }
            catch (Exception error)
            {
                Console.WriteLine("Ошибка при запуске!\n" + error.Message);
                isRunning = false;
            }
        }

        private static void whenExitProcess(object sender, EventArgs e)
        {
            Console.WriteLine("Command done!");
            isRunning = false;
        }

        private static void sortOutputHandler(object sender, DataReceivedEventArgs e)
        {
            if ((e != null) && (e.Data != null))
            {
                switch (e.Data.Split(':')[0].Trim())
                {
                    case "Host Name":
                        systemInfo.HostName = e.Data.Split(':')[1].Trim();
                        break;
                    case "System Manufacturer":
                        systemInfo.SystemManufacturer = e.Data.Split(':')[1].Trim();
                        break;
                    case "System Model":
                        systemInfo.SystemModel = e.Data.Split(':')[1].Trim();
                        break;
                    case "Total Physical Memory":
                        systemInfo.TotalPhysicalMemory = e.Data.Split(':')[1].Trim();
                        break;
                    case "[01]":
                        systemInfo.Processor = e.Data.Split(':')[1].Trim();
                        break;
                    case "Имя узла":
                        systemInfo.HostName = e.Data.Split(':')[1].Trim();
                        break;
                    case "Изготовитель системы":
                        systemInfo.SystemManufacturer = e.Data.Split(':')[1].Trim();
                        break;
                    case "Модель системы":
                        systemInfo.SystemModel = e.Data.Split(':')[1].Trim();
                        break;
                    case "Полный объем физической памяти":
                        systemInfo.TotalPhysicalMemory = e.Data.Split(':')[1].Trim();
                        break;

                    default:
                        break;
                }
            }
        }

        public static void Save(this SystemInfo si)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"db.txt", true))
            {
                file.WriteLine(
                          si.HostName + 
                    "|" + si.SystemManufacturer + 
                    "|" + si.SystemModel +
                    "|" + si.Processor +
                    "|" + si.TotalPhysicalMemory+
                    "|"+DateTime.Now);
            }
        }

    }
}
