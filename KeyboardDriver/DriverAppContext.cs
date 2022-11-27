using KeyboardDriver.Properties;
using System;
using System.Windows.Forms;

namespace KeyboardDriver
{
    public class DriverAppContext : ApplicationContext
    {
        public NotifyIcon TrayIcon { get; private set; }

        public PrimaryForm PrimaryForm { get; set; }

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
            TrayIcon.DoubleClick += new EventHandler(OnDoubleClick);

            PrimaryForm = new PrimaryForm();

            PrimaryForm.Show();
        }

        private void OnDoubleClick(object? Sender, EventArgs e)
        {
            if (PrimaryForm.Visible)
            {
                PrimaryForm.Hide();
            }
            else
            {
                PrimaryForm.Show();
            }

            PrimaryForm.ScrollToBottom();
        }

        void Exit(object? sender, EventArgs e)
        {
            TrayIcon.Visible = false;
            Application.Exit();
        }
    }
}
