using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Foundation;
using static KeyboardDriver.VirtualDesktopManager;

namespace KeyboardDriver
{
    internal static class WindowUtils
    {
        public static void ToggleFocusedWindow()
        {

            var hwnd = PInvoke.GetForegroundWindow();
            var makeTopMost = !IsTopMost(hwnd);
            PInvoke.SetWindowPos(
                hwnd,
                makeTopMost ? HWND.HWND_TOPMOST : HWND.HWND_NOTOPMOST,
                0,
                0,
                0,
                0,
                SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOSIZE);

            ModifyHwndPinState(hwnd, makeTopMost);
        }

        public static bool IsTopMost(HWND hwnd)
        {
            var hwndProps = PInvoke.GetWindowLong(hwnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);
            return ((WINDOW_EX_STYLE)hwndProps).HasFlag(WINDOW_EX_STYLE.WS_EX_TOPMOST);
        }
    }
}
