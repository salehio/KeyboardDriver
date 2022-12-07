using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using WindowsDesktop;

namespace KeyboardDriver
{
    internal class VirtualDesktopManager
    {
        public VirtualDesktop[] Desktops { get; private set; }
        public HWND[] Hwnds { get; private set; }

        public VirtualDesktopManager()
        {
            VirtualDesktop.Configure();
            Desktops = VirtualDesktop.GetDesktops();
            Hwnds = new HWND[Desktops.Length];
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowText",
        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        public void SwitchToIndex(int index)
        {
            if (index > Desktops.Length - 1 || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            Logger.WriteInformation($"Switching to index {index} with id {Desktops[index].Id}");

            // Save current hwnd
            var hwnd = PInvoke.GetForegroundWindow();
            Logger.WriteDebug($"Saving foreground Window {hwnd.Value} {GetWindowTitle(hwnd)}");
            var oldIndex = Array.IndexOf(Desktops, VirtualDesktop.Current);

            if (oldIndex < 0)
            {
                Logger.WriteError("Could not find virtual desktop in array.");
            }
            else
            {
                Hwnds[oldIndex] = hwnd;
                Logger.WriteWarning($"Saved hwnd {hwnd.Value} [{GetWindowTitle(hwnd)}] into index {oldIndex}.");
            }

            Desktops[index].Switch();

            // Restore
            var windowToRestore = Hwnds[index];
            Logger.WriteDebug($"Restoring foreground Window {windowToRestore.Value} {GetWindowTitle(windowToRestore)}");
            if (windowToRestore == default)
            {
                Logger.WriteInformation("Not restoring foreground window, since it didn't exist yet for the virtual desktop");
            }
            else
            {
                Thread.Sleep(500);
                var fwres = PInvoke.SetForegroundWindow(windowToRestore);
                if (fwres)
                {
                    Logger.WriteSuccess($"Successfully restored foreground window to [{GetWindowTitle(windowToRestore)}].");
                }
                else
                {
                    Logger.WriteError($"Failed to restore foreground window.");
                }
            }
        }

        private string GetWindowTitle(HWND hwnd)
        {
            var bld = new StringBuilder();
            GetWindowText(hwnd, bld, int.MaxValue);
            return bld.ToString();
        }

        public void MoveWindowToMain(HWND hwnd)
        {
            VirtualDesktop.MoveToDesktop(hwnd, Desktops[0]);
        }

        public static void ModifyHwndPinState(HWND hwnd, bool shouldBePinned = true)
        {
            if (shouldBePinned)
            {
                VirtualDesktop.PinWindow(hwnd.Value);
            }
            else
            {
                VirtualDesktop.UnpinWindow(hwnd.Value);
            }
        }
    }
}
