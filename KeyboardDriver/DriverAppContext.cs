using KeyboardDriver.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyboardDriver
{
    public class DriverAppContext : ApplicationContext
    {
        public NotifyIcon TrayIcon { get; private set; }

        public DriverAppContext()
        {
            TrayIcon = new NotifyIcon()
            {
                Icon = Resources.TrayIcon,
                ContextMenuStrip = new ContextMenuStrip()
                {
                    Items = { new ToolStripMenuItem("Exit", null, Exit) }
                },
                Visible = true
            };
        }

        void Exit(object? sender, EventArgs e)
        {
            TrayIcon.Visible = false;
            Application.Exit();
        }
    }
}
