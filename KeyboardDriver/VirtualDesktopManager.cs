using System;
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

            Logger.WriteDebug($"Switching to index {index} with id {Desktops[index].Id}");
            Desktops[index].Switch();
        }
    }
}
