using System;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab_afl
{
    public partial class Form1 : Form
    {
        LexicalAnalysis lexical;
        SyntacticAnalysis syntactic;
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
        private void buttonScannerLexical_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += '\n';
            lexical = new LexicalAnalysis(richTextBox1.Text);
            string result = lexical.Scanner();
            if (result == "0")
            {
                MessageBox.Show("Лексический анализ выполнен успешно.");
                EnabledButton(true);
            }
            else
            {
                MessageBox.Show(result);
                EnabledButton(false);
            }
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

        private void buttonScannerSyntactic_Click(object sender, EventArgs e)
        {
            syntactic = new SyntacticAnalysis(lexical, lexical.dataClassification);
            if (syntactic.Scanner())
                MessageBox.Show("успешно");
            else
                MessageBox.Show(syntactic.error);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            int i = richTextBox1.SelectionStart;
            richTextBox1.Text += '\n';
            lexical = new LexicalAnalysis(richTextBox1.Text);
            string result = lexical.Scanner();
            if (result == "0")
            {
                syntactic = new SyntacticAnalysis(lexical, lexical.dataClassification);
                if (syntactic.Scanner())
                    richTextBox2.Text = "ошибок нет";
                else
                    richTextBox2.Text = syntactic.error;
            }
            else
            {
                richTextBox2.Text = result;
            }
            richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.Text.Length - 1);
            richTextBox1.SelectionStart = i;
        }
        private void EnabledButton(bool flag)
        {
            buttonOpenTable.Enabled = flag;
            buttonScannerSyntactic.Enabled = flag;
        }
    }
}