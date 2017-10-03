using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static KeyboardHooks.Fimport;
namespace KeyboardHooks
{
    public static class HookHandler
    {
        private static IntPtr hookID = IntPtr.Zero;
        public static Action<string> _callBackFunct;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static readonly LowLevelKeyboardProc CallBack = HookCallback;
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                
                int keyCode = Marshal.ReadInt32(lParam);
                KeysConverter kc = new KeysConverter();
                string mystring = kc.ConvertToString((Keys)keyCode);
                _callBackFunct(mystring);
            }
            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }

        public static void SetHook()
        {

            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                hookID  = SetWindowsHookEx(WH_KEYBOARD_LL, CallBack, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public static void RemoveHook()
        {
            UnhookWindowsHookEx(hookID);
        }
    }
}
