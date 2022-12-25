using System;
using System.Windows.Forms;
namespace Lab_afl
{
    public partial class FormTable : Form
    {
        public FormTable(LexicalAnalysis lexical, SyntacticAnalysis syntacticAnalysis)
        {
            InitializeComponent();
            for (int i = 0; i < lexical.dataLexeme.Count; i++)
                dataGridView1.Rows.Add(i, lexical.dataLexeme[i].str, lexical.dataLexeme[i].index, lexical.dataClassification[i].tableType, lexical.dataClassification[i].itemNumber);

            for (int i = 0; i < lexical.keywords.Length; i++)
                dataGridView3.Rows.Add(i, lexical.keywords[i]);
            for (int i = 0; i < lexical.separator.Length; i++)
                dataGridView4.Rows.Add(i, lexical.separator[i]);
            for (int i = 0; i < lexical.identifier.Count; i++)
                dataGridView5.Rows.Add(i, lexical.identifier[i]);
            for (int i = 0; i < lexical.literals.Count; i++)
                dataGridView6.Rows.Add(i, lexical.literals[i]);
            for (int i = 0; i < syntacticAnalysis.arithmeticOperatorMatrix.Count; i++)
                dataGridView7.Rows.Add(syntacticAnalysis.arithmeticOperatorMatrix[i].id, syntacticAnalysis.arithmeticOperatorMatrix[i].operation.name, syntacticAnalysis.arithmeticOperatorMatrix[i].operation.operand1, syntacticAnalysis.arithmeticOperatorMatrix[i].operation.operand2, syntacticAnalysis.arithmeticOperatorMatrix[i].operation.operation);
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}