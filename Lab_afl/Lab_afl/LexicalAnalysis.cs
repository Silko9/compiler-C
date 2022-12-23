using System.Collections.Generic;
namespace Lab_afl
{
    public class LexicalAnalysis
    {
        public string stack = "";
        public string[] keywords = new string[11] { "main", "void", "int", "double", "float", "string", "char", "switch", "case", "break", "default" };//ключевые слова(1 таблица)
        public string[] separator = new string[12] { "(", ")", "{", "}", ",", ";", ":", "=", "+", "-", "*", "/"};//разделители(2 таблица)
        public List<string> identifier = new List<string>();//индификатор(3 таблица)
        public List<string> literals = new List<string>();//литерал(4 таблица)
        private int index = -1;
        private char symbol;
        private string str;
        public List<DataLexeme> dataLexeme = new List<DataLexeme>();//таблица 1 лабы
        public List<DataClassification> dataClassification = new List<DataClassification>();//таблица 2 лабы
        private string error;
        public string Error { get { return error; } }
        private string stackError;
        public enum Index { I, D, R}
        public LexicalAnalysis(string Str)
        {
            str = Str;
        }
        public struct DataLexeme
        {
            public string str;
            public Index index;
            public DataLexeme(string str, Index index)
            {
                this.str = str;
                this.index = index;
            }
        }
        public struct DataClassification
        {
            public int tableType;
            public int itemNumber;
            public DataClassification(int tableType, int itemNumber)
            {
                this.tableType = tableType;
                this.itemNumber = itemNumber;
            }
        }
        public bool Scanner()
        {
            return LexicalAnalysis_();
        }
        private bool LexicalAnalysis_()
        {
            Next();
            while (index <= str.Length)
            {
                if (char.IsLetter(symbol))
                {
                    if (!StatusI())
                    {
                        error = "Идентификатор не может быть больше 8 символов.";
                        return false;
                    }
                    continue;
                }
                if (char.IsNumber(symbol))
                {
                    if (!StatusD())
                    {
                        error = "Идентификатор не может начинаться с цифры.";
                        return false;
                    }
                    continue;
                }
                if (symbol == ' ' || symbol == '\n' || symbol == '\t')
                {
                    Next();
                    continue;
                }
                if (symbol == '/')
                {
                    Add();
                    Next();
                    if (symbol == '/')
                    {
                        LowercaseComment();
                        Clear();
                        continue;
                    }
                    else
                    {
                        if (symbol == '*')
                        {
                            if (!MultilineComment())
                            {
                                error = "Требуется */";
                                return false;
                            }
                            Clear();
                            continue;
                        }
                        else
                        {
                            index--;
                            Clear();
                            symbol = '/';
                        }
                    }
                }
                if (!StatusR())
                {
                    error = $"Ошибка синтаксиса: символа '{stackError}' нет в словаре.";
                    return false;
                }
            }
            index = -1;
            ClassificationLexeme();
            return true;
        }
        private void ClassificationLexeme()
        {
            for (int l = 0; l < dataLexeme.Count; l++)
            {
                switch (dataLexeme[l].index)
                {
                    case Index.I:
                        ClassificationI(l);
                        break;
                    case Index.R:
                        ClassificationR(l);
                        break;
                    case Index.D:
                        ClassificationD(l);
                        break;
                    default:
                        dataClassification.Add(new DataClassification(0, 0));
                        break;
                }
            }
        }
        private bool StatusI()
        {
            Add();
            Next();
            if (char.IsLetter(symbol) || char.IsNumber(symbol))
            {
                if (StatusI())
                    return true;
                else
                    return false;
            }
            else
            {
                if (stack.Length > 8)
                    return false;
                Out(Index.I);
                Clear();
                return true;
            }
        }
        private bool StatusD()
        {
            Add();
            Next();
            if (char.IsNumber(symbol))
                StatusD();
            else
            {
                if (char.IsLetter(symbol))
                    return false;
                Out(Index.D);
                Clear();
            }
            return true;
        }
        private bool StatusR()
        {
            Add();
            Next();
            if (IsSeparator(stack))
                Out(Index.R);
            else
            {
                stackError = stack;
                Clear();
                return false;
            }
            Clear();
            return true;
        }
        private bool IsSeparator(string a)
        {
            foreach (var item in separator)
                if (item == a)
                    return true;
            return false;
        }
        private void LowercaseComment()
        {
            do
            {
                Next();
            } while (symbol != '\n');
        }
        private bool MultilineComment()
        {
            bool flag = true;
            do
            {
                if (!Next())
                    return false;
                if (symbol == '*')
                {
                    Next();
                    if (symbol == '/')
                        flag = false;
                }
            } while (flag);
            return Next();
        }
        private void Add()
        {
            stack += symbol;
        }
        private bool Next()
        {
            index++;
            if (index < str.Length)
                symbol = str[index];
            else
                return false;
            return true;
        }
        private void Out(Index status)
        {
            dataLexeme.Add(new DataLexeme(stack, status));
        }
        private void Clear()
        {
            stack = "";
        }
        private void ClassificationI(int l)
        {
            for (int k = 0; k < keywords.Length; k++)
                if (dataLexeme[l].str == keywords[k])
                {
                    dataClassification.Add(new DataClassification(1, k));
                    return;
                }
            for (int i = 0; i < identifier.Count; i++)
                if (identifier[i] == dataLexeme[l].str)
                {
                    dataClassification.Add(new DataClassification(3, i));
                    return;
                }
            dataClassification.Add(new DataClassification(3, identifier.Count));
            identifier.Add(dataLexeme[l].str);
        }
        private void ClassificationR(int l)
        {
            for (int s = 0; s < separator.Length; s++)
                if (dataLexeme[l].str == separator[s])
                {
                    dataClassification.Add(new DataClassification(2, s));
                    return;
                }
        }
        private void ClassificationD(int l)
        {
            for (int i = 0; i < literals.Count; i++)
                if (literals[i] == dataLexeme[l].str)
                {
                    dataClassification.Add(new DataClassification(4, i));
                    return;
                }
            dataClassification.Add(new DataClassification(4, literals.Count));
            literals.Add(dataLexeme[l].str);
        }
    }
}