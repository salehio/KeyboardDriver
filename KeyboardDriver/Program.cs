using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Enumeration;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Storage;
using Windows.Storage.Streams;

namespace KeyboardDriver
{
    public static class HidTest
    {
        private static WindowManager _windowManager = default!;
        private static VirtualDesktopManager _desktopManager = default!;
        private static AudioHandler _audioHandler = default!;
        private static DriverAppContext _context = default!; 

        [STAThread]
        public static void Main(string[] args)
        {
            _audioHandler = new AudioHandler();
            _desktopManager = new VirtualDesktopManager();
            _windowManager = new WindowManager(_desktopManager);
            RegisterListener().GetAwaiter().GetResult();

            //ApplicationConfiguration.Initialize();
            //_context = new DriverAppContext();
            //Logger.TrayIcon = _context.TrayIcon;
            //Application.Run(_context);

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

                foreach (var message in messages.Trim('\0').Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    var m = message.Trim('\0');
                    Logger.WriteDebug(m);

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
                    _desktopManager.SwitchToIndex(int.Parse(cmdInfo[1]));
                    break;
                case "F":
                    // Set top most window and pin to all virtual desktops.
                    // TODO: Move to last monitor position.
                    _windowManager.ToggleFocusedWindow();
                    break;
                case "A":
                    // Set top most window and pin to all virtual desktops.
                    // TODO: Move to last monitor position.
                    switch (cmdInfo[1])
                    {
                        case "U":
                            _audioHandler.StepVolumeUp();
                            break;
                        case "D":
                            _audioHandler.StepVolumeDown();
                            break;
                        case "M":
                            _audioHandler.ToggleMute();
                            break;
                    }

                    break;
            }
        }
    }
}
