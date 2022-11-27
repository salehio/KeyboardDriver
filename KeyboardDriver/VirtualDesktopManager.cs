using System;
using Windows.Win32.Foundation;
using WindowsDesktop;

namespace KeyboardDriver
{
    internal class VirtualDesktopManager
    {
        public VirtualDesktop[] Desktops { get; private set; }

        public VirtualDesktopManager()
        {
            VirtualDesktop.Configure();
            Desktops = VirtualDesktop.GetDesktops();
        }

        public void SwitchToIndex(int index)
        {
            if (index > Desktops.Length - 1 || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            Logger.WriteInformation($"Switching to index {index} with id {Desktops[index].Id}");
            Desktops[index].Switch();
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
