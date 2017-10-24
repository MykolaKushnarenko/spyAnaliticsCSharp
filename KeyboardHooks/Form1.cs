using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static KeyboardHooks.ProgInfo;

namespace KeyboardHooks
{
    public partial class Form1 : Form
    {
        private string language;
        private Dictionary<char, char> dictionary = new Dictionary<char, char>();
        private string NameOfApp;
        private DateTime localDate;
        private ProgInfo info;
        public Form1()
        {
            InitializeComponent();
            info = new ProgInfo();
            string En = "~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./↑←↓ ";
            string Ru = "Ё!\"№;%:?*()_+ЙЦУКЕНГШЩЗХЪ/ФЫВАПРОЛДЖЭЯЧСМИТЬБЮ,ё1234567890-=йцукенгшщзхъ\\фывапролджэячсмитьБЮ.↑←↓ ";

            char[] enen = En.ToCharArray();
            char[] ruru = Ru.ToCharArray();

            for (int i = 0; i < enen.Length; i++)
            {
                dictionary.Add(enen[i], ruru[i]);
            }
            language = GetLanguage();
            NameOfApp = info.GetNameOfApp();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            HookHandler.SetHook();
            MouseHooks.SetHook();
            HookHandler._callBackFunct = Display;
            MouseHooks._callBackFunctMouse = Display;
        }

        public void Display(string s)
        {
            localDate = DateTime.Now;
            if (NameOfApp != info.GetNameOfApp())
            {
                if (info.GetNameOfApp() == "")
                {
                    NameOfApp = info.GetNameOfApp();
                    textBox1.Text += "Проводник" + "\r\n";
                }
                else
                {
                    NameOfApp = info.GetNameOfApp();
                    textBox1.Text += NameOfApp + "\r\n";
                }
            }
            char[] masKey = s.ToCharArray();
            if (language != GetLanguage())
            {
                language = GetLanguage();
                textBox1.Text += "Язык: " + language + "\r\n";
            }
            if (masKey.Length == 1 && language == "Русский")
            {
                textBox1.Text += localDate + ": "  + dictionary[masKey[0]] + "\r\n";
            }
            else if (masKey.Length == 1)
            {
                textBox1.Text += localDate +": "+ masKey[0] + "\r\n";
            }
            else
            {
                textBox1.Text += localDate + ": " + s + "\r\n";
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            HookHandler.RemoveHook();
            MouseHooks.RemoveHook();
        }

        private string GetLanguage()
        {
            if (info.GetKeyboardLayout() == 1049)
            {
               return "Русский";
            }
            else return "English";
        }
    }
}
