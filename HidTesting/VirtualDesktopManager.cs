using HidTesting.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsDesktop;

namespace HidTesting.VirtualDesktopManager
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
