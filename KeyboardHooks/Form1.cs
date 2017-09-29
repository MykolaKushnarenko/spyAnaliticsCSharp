using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static KeyboardHooks.CoreFunctionImport;

namespace KeyboardHooks
{
    public partial class Form1 : Form
    {

        public Dictionary<char, char> dictionary = new Dictionary<char, char>();
        public Form1()
        {
            InitializeComponent();
            string En = "~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./↑←↓ ";
            string Ru = "Ё!\"№;%:?*()_+ЙЦУКЕНГШЩЗХЪ/ФЫВАПРОЛДЖЭЯЧСМИТЬБЮ,ё1234567890-=йцукенгшщзхъ\\фывапролджэячсмитьбю.↑←↓ ";

            char[] enen = En.ToCharArray();
            char[] ruru = Ru.ToCharArray();
            for (int i = 0; i < enen.Length; i++)
            {
                dictionary.Add(enen[i], ruru[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CoreFunctionImport._hookID = SetHook(CoreFunctionImport.HookCallback);
            CoreFunctionImport.CallbackAction = Display;
        }

        public void Display(string s)
        {
            textBox2.Text = InputLanguage.CurrentInputLanguage.Culture.DisplayName;
            textBox1.Text += s + "\r\n";
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
