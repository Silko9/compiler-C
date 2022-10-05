using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_afl
{
    public partial class FormTable : Form
    {
        public FormTable(LexicalAnalysis lexical)
        {
            InitializeComponent();
            dataGridView1.ColumnCount = 2;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 40;
            dataGridView1.Columns[0].HeaderText = "Значение";
            dataGridView1.Columns[1].HeaderText = "Тип";
            dataGridView2.ColumnCount = 2;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.Columns[0].Width = 60;
            dataGridView2.Columns[1].Width = 60;
            dataGridView2.Columns[0].HeaderText = "Таблица";
            dataGridView2.Columns[1].HeaderText = "Элемент";
            dataGridView3.ColumnCount = 1;
            dataGridView3.RowHeadersVisible = false;
            dataGridView4.ColumnCount = 1;
            dataGridView4.RowHeadersVisible = false;
            dataGridView5.ColumnCount = 1;
            dataGridView5.RowHeadersVisible = false;
            dataGridView6.ColumnCount = 1;
            dataGridView6.RowHeadersVisible = false;
            foreach (var item in lexical.dataLexeme)
                dataGridView1.Rows.Add(item.str, item.index);
            foreach (var item in lexical.dataClassification)
                dataGridView2.Rows.Add(item.tableType, item.itemNumber);
            foreach (var item in lexical.keywords)
                dataGridView3.Rows.Add(item);
            foreach (var item in lexical.separator)
                dataGridView4.Rows.Add(item);
            foreach (var item in lexical.identifier)
                dataGridView5.Rows.Add(item);
            foreach (var item in lexical.literals)
                dataGridView6.Rows.Add(item);
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}