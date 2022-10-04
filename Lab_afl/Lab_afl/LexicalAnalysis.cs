using System.Collections.Generic;

namespace Lab_afl
{
    internal class LexicalAnalysis
    {
        public string stack = "";
        private string[] separator = new string[11] { "(", ")", "{", "}", ",", ";", ":", "=", "+", "-", "//" };
        private int index = -1;
        private char symbol;
        private string str;
        public List<Data> data = new List<Data>();
        public LexicalAnalysis(string Str)
        {
            str = Str;
        }
        public class Data
        {
            public string str;
            public char index;
            public Data(string str, char index)
            {
                this.str = str;
                this.index = index;
            }
        }
        public string Scanner()
        {
            Next();
            while (index <= str.Length)
            {
                if (char.IsLetter(symbol))
                {
                    if (!StatusI())
                        return "Идентификатор не может быть больше 8 символов.";
                    continue;
                }
                if (char.IsNumber(symbol))
                {
                    if (!StatusD())
                        return "Идентификатор не может начинаться с цифры.";
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
                        do
                        {
                            Next();
                        } while (symbol != '\n');
                    else
                    {
                        if (symbol == '*')
                        {
                            bool flag = true;
                            do
                            {
                                Next();
                                if (symbol == '*')
                                {
                                    Next();
                                    if (symbol == '/')
                                        flag = false;
                                }
                            } while (flag);
                            Next();
                        }
                        else
                            return $"Ошибка синтаксиса: символа '{stack[0]}' нету в словаре.";
                    }
                    Clear();
                    continue;
                }
                if (!StatusR())
                    return $"Ошибка синтаксиса: символа '{stack[0]}' нету в словаре.";
            }
            index = -1;
            return "0";
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
                Out('I');
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
                Out('D');
                Clear();
            }
            return true;
        }
        private bool StatusR()
        {
            Add();
            Next();
            if (char.IsSymbol(symbol))
                StatusR();
            else
            {
                if (IsSeparator(stack))
                    Out('R');
                else
                    return false;
                Clear();
            }
            return true;
        }
        private bool IsSeparator(string a)
        {
            foreach (var item in separator)
                if (item == a)
                    return true;
            return false;
        }
        private void Add()
        {
            stack += symbol;
        }
        private void Next()
        {
            index++;
            if (index < str.Length)
                symbol = str[index];
        }
        private void Out(char status)
        {
            data.Add(new Data(stack, status));
        }
        private void Clear()
        {
            stack = "";
        }
    }
}

