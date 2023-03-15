using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RockSnifferBootstrapper
{
    internal class ProcessSniffer
    {
        public static void StartSniffing()
        {
            var rsExecutable = Directory.GetFiles(Directory.GetCurrentDirectory(), "RockSniffer.exe", SearchOption.AllDirectories);
            Process rsProcess = new Process();

            Thread.CurrentThread.Name = "RSBProcessSniffer";

            Task sniffProcesses = Task.Run(() =>
            {
                while (true)
                {
                    Process[] processName = Process.GetProcessesByName("Rocksmith2014");
                    Process[] rsProcessName = Process.GetProcessesByName("RockSniffer");
                    if (processName.Length > 0 && rsProcessName.Length == 0)
                    {
                        rsProcess.StartInfo = new ProcessStartInfo(rsExecutable[0]);
                        rsProcess.StartInfo.CreateNoWindow = true;
                        rsProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        rsProcess.Start();
                    }
                    else if (rsProcessName.Length > 0 && processName.Length == 0)
                    {
                        rsProcess.Kill();
                    }
                    Thread.Sleep(2000);
                }
            });
        }
    }
}
