using System;
using System.IO;
using System.Windows.Forms;
namespace Lab_afl
{
    public partial class FormMain : Form
    {
        FormTable form;
        Translator translator = new Translator();
        public FormMain()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt";
            richTextBox1.Text = "void main() {\r\na = 5+5;\r\n}\r\n";
            TranslatorProcess();
        }
        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string name = openFileDialog1.FileName;
                richTextBox1.Clear();
                StreamReader sr = File.OpenText(openFileDialog1.FileName);
                string line;
                line = sr.ReadLine();
                while (line != null)
                {
                    richTextBox1.AppendText(line);
                    richTextBox1.AppendText("\r\n");
                    line = sr.ReadLine();
                }
                sr.Close();
            }
        }
        private void buttonOpenTable_Click(object sender, EventArgs e)
        {
            if (translator.Lexical != null)
            {
                if (form != null)
                    form.Close();
                form = new FormTable(translator.Lexical, translator.Syntactic);
                form.Show();
            }
            else
                MessageBox.Show("Ошибка. Таблицы пустые.");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            TranslatorProcess();
        }
        private void TranslatorProcess()
        {
            int i = richTextBox1.SelectionStart;
            richTextBox1.Text += '\n';
            if (translator.Process(richTextBox1.Text))
                richTextBox2.Text = "Ошибок нет.";
            else
                richTextBox2.Text = translator.Error;
            richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.Text.Length - 1);
            richTextBox1.SelectionStart = i;
        }
    }
}