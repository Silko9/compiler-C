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
        AnalysisBauerSamelsohn analysis;
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            richTextBox1.Text = "void main() {\r\na = 5+5;\r\n}\r\n";
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
        private void buttonOpenTable_Click(object sender, EventArgs e)
        {
            if (lexical != null)
            {
                FormTable form = new FormTable(lexical, syntactic);
                form.Show();
            }
            else
                MessageBox.Show("Таблицы пустые. Запустите сканер.");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            int i = richTextBox1.SelectionStart;
            richTextBox1.Text += '\n';
            lexical = new LexicalAnalysis(richTextBox1.Text);
            if (lexical.Scanner())
            {
                syntactic = new SyntacticAnalysis(lexical, lexical.dataClassification);
                if (syntactic.Scanner())
                    richTextBox2.Text = "ошибок нет";
                else
                    richTextBox2.Text = syntactic.Error;
                analysis = syntactic.analysisBauerSamelsohn;
            }
            else
            {
                richTextBox2.Text = lexical.Error;
            }
            richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.Text.Length - 1);
            richTextBox1.SelectionStart = i;
        }
    }
}