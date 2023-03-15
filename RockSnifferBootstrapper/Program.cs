using RockSnifferBootstrapper.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RockSnifferBootstrapper
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RSBApplicationContext());
        }
    }

    public class RSBApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;

        public RSBApplicationContext()
        {
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.AppIcon,
                ContextMenu = new ContextMenu(new MenuItem[]
                {
                    new MenuItem("Exit", Exit)
                }),
                Visible = true
            };

            Updater.Update();

            ProcessSniffer.StartSniffing();
        }

        void Exit(object sender, EventArgs e) 
        {
            trayIcon.Visible = false;
            Application.Exit();
        }
    }
}
