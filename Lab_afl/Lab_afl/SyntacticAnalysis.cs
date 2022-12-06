using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab_afl.LexicalAnalysis;

namespace Lab_afl
{
    internal class SyntacticAnalysis
    {
        LexicalAnalysis lexicalAnalysis;
        List<DataClassification> dataClassification;
        string Lexem;
        int index = -1;
        public string error = "пусто";
        public SyntacticAnalysis(LexicalAnalysis lexicalAnalysis, List<DataClassification> dataClassification)
        {
            this.lexicalAnalysis = lexicalAnalysis;
            this.dataClassification = dataClassification;
        }
        public bool Scanner()
        {
            if (!Next("void")) return false;
            return Procedure_программа();
        }
        private bool CheakLexem(string expectedLexem)
        {
            if (expectedLexem != Lexem)
            {
                CreateError(expectedLexem);
                return false;
            }
            return true;
        }
        private void CreateError(string expectedLexem)
        {
            error = $"Ожидалось \"{expectedLexem}\"\nВстретилось \"{Lexem}\"";
        }
        private bool Next(string expectedLexem)
        {
            try
            {
                index++;
                Lexem = GetLit(dataClassification[index].tableType, dataClassification[index].itemNumber);
                return true;
            }
            catch
            {
                error = $"Ожидалось \"{expectedLexem}\"";
                return false;
            }
        }
        private bool Procedure_программа()
        {
            if (!CheakLexem("void") || !Next("main")) return false;
            if (!CheakLexem("main") || !Next("(")) return false;
            if (!CheakLexem("(") || !Next(")")) return false;
            if (!CheakLexem(")") || !Next("{")) return false;
            if (!CheakLexem("{") || !Next("оператор")) return false;
            if (!Procedure_список_операторов()) return false;
            if (!CheakLexem("}")) return false;
            if (Next("конец программы")) return false;
            return true;
        }
        private bool Procedure_список_операторов()
        {
            if (!Procedure_оператор()) return false;
            if (!Procedure_список_операторов_X()) return false;
            return true;
        }
        private bool Procedure_список_операторов_X()
        {
            if (Lexem == "}")
                return true;
            if (Lexem == "int" || Lexem == "double" || Lexem == "float" || Lexem == "string" || Lexem == "char" || Lexem == "switch" || Lexem == "id")
            {
                if (!Procedure_список_операторов()) return false;
                return true;
            }
            CreateError("оператор");
            return false;
        }
        
        private bool Procedure_оператор()
        {
            if (Lexem == "int" || Lexem == "double" || Lexem == "float" || Lexem == "string" || Lexem == "char")
            {
                if (!Procedure_объявление_переменной()) return false;
                return true;
            }
            if (Lexem == "switch")
            {
                if (!Procedure_условный_оператор()) return false;
                return true;
            }
            //иф лексема равна id елсе ошиька ожидался оператор
            //проверить аналогичные ситуации
            if (!Procedure_присваивание()) return false;
            return true;
        }
        private bool Procedure_объявление_переменной()
        {
            if (!Procedure_тип()) return false;
            if (!Procedure_список_переменных()) return false;
            if (!CheakLexem(";") || !Next("оператор или '}'")) return false;
            return true;
        }
        private bool Procedure_тип()
        {
            if (!Next("id")) return false;
            return true;
        }
        private bool Procedure_список_переменных()
        {
            if (!CheakLexem("id") || !Next("',' или ';'")) return false;
            if (!Procedure_список_переменных_X()) return false;
            return true;
        }
        private bool Procedure_список_переменных_X()
        {
            if (Lexem == ";")
                return true;
            if (Lexem == ",")
            {
                if (!Procedure_список_переменных_U()) return false;
                return true;
            }
            CreateError("',' или ';'");
            return false;
        }
        private bool Procedure_список_переменных_U()
        {
            if (!Next("id")) return false;
            if (!CheakLexem("id") || !Next("',' или ';'")) return false;
            if (!Procedure_список_переменных_X()) return false;
            return true;
        }
        private bool Procedure_условный_оператор()
        {
            if (!Next("'('")) return false;
            if (!CheakLexem("(") || !Next("'id'")) return false;
            if (!CheakLexem("id") || !Next("')'")) return false;
            if (!CheakLexem(")") || !Next("'{'")) return false;
            if (!CheakLexem("{") || !Next("'case'")) return false;
            if (!Procedure_список_блоков_операторов()) return false;
            if (!Procedure_условный_оператор_U()) return false;
            return true;
        }
        private bool Procedure_условный_оператор_U()
        {
            if (Lexem == "}")
            {
                if (!Next("'}' или оператор")) return false;
                return true;
            }
            if (Lexem == "default")
            {
                if (!Next("':'")) return false;
                if (!CheakLexem(":") || !Next("'оператор'")) return false;
                if (!Procedure_список_операторов_case()) return false;
                if (!Procedure_условный_оператор_X()) return false;
                return true;
            }
            CreateError("'}' или 'default'");
            return false;
        }
        private bool Procedure_условный_оператор_X()
        {
            if (Lexem == "}")
                return true;
            if (Lexem == "break")
            {
                if (!Next("';'")) return false;
                if (!CheakLexem(";") || !Next("'}'")) return false;
                if (!CheakLexem("}") || !Next("'оператор' или '}'")) return false;
                return true;
            }
            CreateError("'}' или 'break'");
            return false;
        }
        private bool Procedure_список_блоков_операторов()
        {
            if (!Procedure_блок_операторов()) return false;
            if (!Procedure_список_блоков_операторов_X()) return false;
            return true;
        }
        private bool Procedure_список_блоков_операторов_X()
        {
            if (Lexem == "}" || Lexem == "default")
                return true;
            if (Lexem == "case")
            {
                if (!Procedure_список_блоков_операторов()) return false;
                return true;
            }
            CreateError("'}' или 'case' или 'default'");
            return false;
        }
        private bool Procedure_блок_операторов()
        {
            if (!CheakLexem("case") || !Next("'операнд'")) return false;
            if (!Procedure_операнд()) return false;
            if (!CheakLexem(":") || !Next("'оператор'")) return false;
            if (!Procedure_список_операторов_case()) return false;
            if (!Procedure_блок_операторов_U()) return false;
            return true;
        }
        private bool Procedure_блок_операторов_U()
        {
            if (Lexem == "}" || Lexem == "default" || Lexem == "case")
                return true;
            if (Lexem == "break")
            {
                if (!Next("';'")) return false;
                if (!CheakLexem(";") || !Next("'}' или 'case' или 'default'")) return false;
                return true;
            }
            CreateError("'}' или 'case' или 'default' или 'break'");
            return false;
        }
        private bool Procedure_список_операторов_case()
        {
            if (!Procedure_оператор()) return false;
            if (!Procedure_список_операторов_case_X()) return false;
            return true;
        }
        private bool Procedure_список_операторов_case_X()
        {
            if (Lexem == "}" || Lexem == "default" || Lexem == "case" || Lexem == "break")
                return true;
            if (Lexem == "int" || Lexem == "double" || Lexem == "float" || Lexem == "string" || Lexem == "char" || Lexem == "switch" || Lexem == "id")
            {
                if (!Procedure_список_операторов_case()) return false;
                return true;
            }
            CreateError("'}' или 'case' или 'default' или 'break' или 'оператор'");
            return false;
        }
        private bool Procedure_присваивание()
        {
            if (!CheakLexem("id") || !Next("'=")) return false;
            if (!CheakLexem("=") || !Next("'expr'")) return false;
            Expr();
            if (!CheakLexem(";") || !Next("'}' или оператор")) return false;
            return true;
        }
        private bool Procedure_операнд()
        {
            if (Lexem == "id" || Lexem == "lit")
            {
                if (!Next("':'")) return false;
                return true;
            }
            CreateError("операнд");
            return false;
        }
        private string GetLit(int table, int element)
        {
                switch (table)
                {
                    case 1: return lexicalAnalysis.keywords[element];
                    case 2: return lexicalAnalysis.separator[element];
                    case 3: return "id";
                    case 4: return "lit";
                    default: return "error";
                }

        }
        private void Expr()
        {
            index += 2;
            Next("ошибка expr");
        }
    }
}