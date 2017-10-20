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
        private static string shift_comd = "";
        private static string key_oem = "";
        private static IntPtr hookID = IntPtr.Zero;
        public static Action<string> _callBackFunct;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private static readonly LowLevelKeyboardProc CallBack = HookCallback;
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int keyCode = Marshal.ReadInt32(lParam);
                KeysConverter kc = new KeysConverter();
                string mystring = kc.ConvertToString((Keys)keyCode);
                if (wParam == (IntPtr)WM_KEYDOWN) 
                {
                    if (Keys.Oemcomma == (Keys) keyCode)
                    {
                        key_oem = ",";
                        _callBackFunct(key_oem);
                    }
                    else if (Keys.OemOpenBrackets == (Keys)keyCode)
                    {
                        key_oem = "[";
                        _callBackFunct(key_oem);
                    }
                    else if (Keys.Oem6 == (Keys)keyCode)
                    {
                        key_oem = "]";
                        _callBackFunct(key_oem);
                    }
                    else if (Keys.Oem5 == (Keys)keyCode)
                    {
                        key_oem = "\\";
                        _callBackFunct(key_oem);
                    }
                    else if (Keys.Oem1 == (Keys)keyCode)
                    {
                        key_oem = ";";
                        _callBackFunct(key_oem);
                    }
                    else if (Keys.Oem7 == (Keys)keyCode)
                    {
                        key_oem = "'";
                        _callBackFunct(key_oem);
                    }
                    else if(Keys.OemQuestion == (Keys)keyCode)
                    {
                        key_oem = "/";
                        _callBackFunct(key_oem);
                    }
                    else if (Keys.OemPeriod == (Keys) keyCode)
                    {
                        key_oem = ".";
                        _callBackFunct(key_oem);
                    }
                    else
                    {
                        _callBackFunct(mystring);
                    }
                }
                if (wParam == (IntPtr) WM_KEYUP)
                {
    
                    if (Keys.Oemcomma == (Keys)keyCode && Keys.Shift == Control.ModifierKeys)
                    {
                        shift_comd = "<";
                        _callBackFunct(shift_comd);
                    }
                    if (Keys.OemPeriod == (Keys) keyCode && Keys.Shift == Control.ModifierKeys)
                    {
                        shift_comd = ">";
                        _callBackFunct(shift_comd);
                    }
                    if (Keys.LShiftKey == (Keys)keyCode && Keys.Shift == Control.ModifierKeys)
                    {
                        shift_comd = "(Отпустил)LShift";
                        _callBackFunct(shift_comd);
                    }

                }



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
