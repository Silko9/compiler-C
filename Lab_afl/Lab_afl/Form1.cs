using System;
using System.IO;
using System.Windows.Forms;

namespace Lab_afl
{
    public partial class Form1 : Form
    {
        LexicalAnalysis lexical;
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            richTextBox1.Text = "void main() {\r\n\tint a, b;\r\n\tswitch(a) {\r\n\t\tcase 1:\r\n\t\t\tb = b + 5;\r\n\t\tcase 2:\r\n\t\t\tb = b - 25;\r\n\t}\t}\r\n";
        }
        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Clear();
                StreamReader sr = File.OpenText(openFileDialog1.FileName);
                string line = null;
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
        private void buttonScanner_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += '\n';
            lexical = new LexicalAnalysis(richTextBox1.Text);
            string result = lexical.Scanner();
            if (result == "0")
                MessageBox.Show("Лексический анализ выполнен успешно.");
            else
                MessageBox.Show(result);
        }
        private void buttonOpenTable_Click(object sender, EventArgs e)
        {
            if (lexical != null)
            {
                FormTable form = new FormTable(lexical);
                form.Show();
            }
            else
                MessageBox.Show("Таблицы пустые. Запустите сканер.");
        }
    }
}