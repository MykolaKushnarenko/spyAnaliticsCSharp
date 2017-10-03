using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KeyboardHooks.CoreFunctionImport;
namespace KeyboardHooks
{
    class BeginHooks
    {
        public static Action<string> CallbackAction { get; set; }

        public static void GetLit(string s)
        {
            CallbackAction(s);
        }
        public void Run()
        {
            CoreFunctionImport._hookID = SetHook(CoreFunctionImport.HookCallback);
        }

        public void Exit()
        {
            CoreFunctionImport.UnhookWindowsHookEx(_hookID);
        }
        private IntPtr SetHook(CoreFunctionImport.LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return CoreFunctionImport.SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
    }
}
