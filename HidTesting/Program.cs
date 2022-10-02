using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Windows.Devices.Enumeration;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Win32;
using WindowsDesktop;
using HidTesting.Logging;
using HidTesting.VirtualDesktopManager;
using System.Printing;
using Windows.Management.Workplace;
using System.ComponentModel;

namespace HIDdeviceTest
{
    public static class HidTest
    {
        private static VirtualDesktopManager _manager = default!;

        [STAThread]
        public static void Main(string[] args)
        {
            _manager = new VirtualDesktopManager();
            RegisterListener().GetAwaiter().GetResult();
            Thread.Sleep(-1);
        }

        private static async Task RegisterListener()
        {
            // Quefrency rev 3
            ushort vendorId = 0xCB10;
            ushort productId = 0x3257;

            // Special usagePage and usageId used by QMK via CONSOLE_ENABLE
            ushort usagePage = 0xFF31;
            ushort usageId = 0x0074;

            string selector = HidDevice.GetDeviceSelector(usagePage, usageId, vendorId, productId);

            DeviceInformation info;
            try
            {
                info = (await DeviceInformation.FindAllAsync(selector)).Single();
                Logger.WriteSuccess("Device found");
            }
            catch (Exception)
            {
                Logger.WriteError("Failed to find device.");
                throw;
            }

            // Open the target HID device.
            HidDevice device = await HidDevice.FromIdAsync(info.Id, FileAccessMode.Read);

            if (device == null)
            {
                Logger.WriteError("Failed to Get HidDevice");
                throw new Exception();
            }

            // Input reports contain data from the device.
            int i = 0;
            device.InputReportReceived += (sender, args) =>
            {
                var buffer = args.Report.Data;

                var dataReader = DataReader.FromBuffer(buffer);
                var messages = dataReader.ReadString(buffer.Length);
                Logger.WriteDebug(buffer.Length);

                foreach (var message in messages.Trim('\0').Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    var m = message.Trim('\0');
                    Logger.WriteDebug(m);
                    Logger.WriteDebug(i++);

                    var parsedCmd = ParseCommand(m);
                    if(parsedCmd != null)
                    {
                        RunCommand(parsedCmd);
                    }
                }
            };

            Logger.WriteSuccess("Event handler registered");
        }

        private static string[]? ParseCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Logger.WriteWarning("Input to ParseCommand was null or whitespace...");
                return null;
            }

            return input.Trim().Split(':');
        }

        private static void RunCommand(string[] cmdInfo)
        {
            switch (cmdInfo[0])
            {
                // Switch virtual desktops
                case "D":
                    // arg 0 is the virtual desktop to switch to.
                    _manager.SwitchToIndex(int.Parse(cmdInfo[1]));
                    break;
            }
        }
    }
}
