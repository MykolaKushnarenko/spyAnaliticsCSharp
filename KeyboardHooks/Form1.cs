using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyboardHooks
{
    public partial class Form1 : Form
    {

        public string language;
        public Dictionary<char, char> dictionary = new Dictionary<char, char>();
        public string NameOfApp;
        private DateTime localDate;
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
            language = InputLanguage.CurrentInputLanguage.Culture.DisplayName;


        }


        private void button1_Click(object sender, EventArgs e)
        {
            HookHandler.SetHook();
            HookHandler._callBackFunct = Display;
        }

        public void Display(string s)
        {
            localDate = DateTime.Now;
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();

            char[] masKey = s.ToCharArray();
            if (language != InputLanguage.CurrentInputLanguage.Culture.DisplayName)
            {
                language = InputLanguage.CurrentInputLanguage.Culture.DisplayName;
                textBox1.Text += "Язык: " + language + "\r\n";
            }
            if ((language == "Русский (Россия)") && (masKey.Length == 1))
            {
                textBox1.Text += localDate.ToString() + ": "  + dictionary[masKey[0]] + "\r\n";
            }
            else if (masKey.Length == 1)
            {
                textBox1.Text += localDate.ToString() +": "+ masKey[0] + "\r\n";
            }
            else
            {
                textBox1.Text += localDate.ToString() + ": " + s + "\r\n";
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
        }
    }
}
