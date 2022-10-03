using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Foundation;

namespace KeyboardDriver
{
    internal static class WindowUtils
    {
        public static void ToggleTopMostWindow()
        {

            var hwnd = PInvoke.GetForegroundWindow();
            PInvoke.SetWindowPos(
                hwnd,
                IsTopMost(hwnd) ? HWND.HWND_NOTOPMOST : HWND.HWND_TOPMOST,
                0,
                0,
                0,
                0,
                SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOSIZE);
        }

        public static bool IsTopMost(HWND hwnd)
        {
            var hwndProps = PInvoke.GetWindowLong(hwnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);
            return ((WINDOW_EX_STYLE)hwndProps).HasFlag(WINDOW_EX_STYLE.WS_EX_TOPMOST);
        }
    }
}
