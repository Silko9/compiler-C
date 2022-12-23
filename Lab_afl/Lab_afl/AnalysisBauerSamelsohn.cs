using System.Collections.Generic;
using static Lab_afl.LexicalAnalysis;
namespace Lab_afl
{
    public class AnalysisBauerSamelsohn
    {
        private LexicalAnalysis lexicalAnalysis;
        private List<DataClassification> dataClassifications = new List<DataClassification>();
        private int index = -1;
        private string error = "пусто";
        public string Error { get { return error; } }
        private Stack<string> stackE = new Stack<string>();
        private Stack<string> stackT = new Stack<string>();
        public List<Operation> arithmeticOperatorMatrix = new List<Operation>();
        bool flag;
        bool isOperand = false;
        public AnalysisBauerSamelsohn(List<DataClassification> dataClassifications, LexicalAnalysis lexicalAnalysis)
        {
            this.dataClassifications = dataClassifications;
            this.lexicalAnalysis = lexicalAnalysis;
        }
        public struct Operation
        {
            public string name;
            public string operand1;
            public string operand2;
            public char operation;
            public Operation(string name, string operand1, string operand2, char operation)
            {
                this.name = name;
                this.operand1 = operand2;
                this.operand2 = operand1;
                this.operation = operation;
            }
        }
        public bool Scanner()
        {
            Next();
            if(stackE.Count == 0)
            {
                error = "Требуется 'lit' иди 'id'";
                return false;
            }
            return flag;
        }
        private string GetLit(int table, int element)
        {
            switch (table)
            {
                case 1: return lexicalAnalysis.keywords[element];
                case 2: return lexicalAnalysis.separator[element];
                case 3: return lexicalAnalysis.identifier[element];
                case 4: return lexicalAnalysis.literals[element];
                default: return "error";
            }
        }
        private void CreateError(string str1, string str2)
        {
            error = $"Требуется {str1}\nВстретилось {str2}";
        }
        private void InvalidCharacter(string symbol)
        {
            error = $"Символ '{symbol}' некорректен в операции";
        }
        private void Next()
        {
            index++;
            string operand;
            string operation;
            string lastOperation;
            if (index >= dataClassifications.Count || dataClassifications[index].tableType == 2)
            {
                if (index >= dataClassifications.Count)
                    operation = "$";
                else
                {
                    operation = GetLit(dataClassifications[index].tableType, dataClassifications[index].itemNumber);
                }
                if (stackT.Count != 0)
                    lastOperation = stackT.Peek();
                else
                    lastOperation = "e";
                if (operation == "(" && isOperand)
                {
                    flag = false;
                    CreateError("'+', '-', '*', '/'", "'('");
                }
                else
                {
                    isOperand = false;
                    switch (lastOperation)
                    {
                        case "e":
                            switch (operation)
                            {
                                case "$":
                                    Operation6();
                                    break;
                                case "(":
                                    Operation1(operation);
                                    break;
                                case "+":
                                    Operation1(operation);
                                    break;
                                case "-":
                                    Operation1(operation);
                                    break;
                                case "*":
                                    Operation1(operation);
                                    break;
                                case "/":
                                    Operation1(operation);
                                    break;
                                case ")":
                                    Operation5();
                                    error = "Кол-во открывающих и закрывающих скобок не совпадает";
                                    break;
                                default:
                                    InvalidCharacter(operation);
                                    break;
                            }
                            break;
                        case "(":
                            switch (operation)
                            {
                                case "$":
                                    Operation5();
                                    error = "Кол-во открывающих и закрывающих скобок не совпадает";
                                    break;
                                case "(":
                                    Operation1(operation);
                                    break;
                                case "+":
                                    Operation1(operation);
                                    break;
                                case "-":
                                    Operation1(operation);
                                    break;
                                case "*":
                                    Operation1(operation);
                                    break;
                                case "/":
                                    Operation1(operation);
                                    break;
                                case ")":
                                    Operation3();
                                    break;
                                default:
                                    InvalidCharacter(operation);
                                    break;
                            }
                            break;
                        case "+":
                            switch (operation)
                            {
                                case "$":
                                    Operation4();
                                    break;
                                case "(":
                                    Operation1(operation);
                                    break;
                                case "+":
                                    Operation2(operation);
                                    break;
                                case "-":
                                    Operation2(operation);
                                    break;
                                case "*":
                                    Operation1(operation);
                                    break;
                                case "/":
                                    Operation1(operation);
                                    break;
                                case ")":
                                    Operation4();
                                    break;
                                default:
                                    InvalidCharacter(operation);
                                    break;
                            }
                            break;
                        case "-":
                            switch (operation)
                            {
                                case "$":
                                    Operation4();
                                    break;
                                case "(":
                                    Operation1(operation);
                                    break;
                                case "+":
                                    Operation2(operation);
                                    break;
                                case "-":
                                    Operation2(operation);
                                    break;
                                case "*":
                                    Operation1(operation);
                                    break;
                                case "/":
                                    Operation1(operation);
                                    break;
                                case ")":
                                    Operation4();
                                    break;
                                default:
                                    InvalidCharacter(operation);
                                    break;
                            }
                            break;
                        case "*":
                            switch (operation)
                            {
                                case "$":
                                    Operation4();
                                    break;
                                case "(":
                                    Operation1(operation);
                                    break;
                                case "+":
                                    Operation4();
                                    break;
                                case "-":
                                    Operation4();
                                    break;
                                case "*":
                                    Operation2(operation);
                                    break;
                                case "/":
                                    Operation2(operation);
                                    break;
                                case ")":
                                    Operation4();
                                    break;
                                default:
                                    InvalidCharacter(operation);
                                    break;
                            }
                            break;
                        case "/":
                            switch (operation)
                            {
                                case "$":
                                    Operation4();
                                    break;
                                case "(":
                                    Operation1(operation);
                                    break;
                                case "+":
                                    Operation4();
                                    break;
                                case "-":
                                    Operation4();
                                    break;
                                case "*":
                                    Operation2(operation);
                                    break;
                                case "/":
                                    Operation2(operation);
                                    break;
                                case ")":
                                    Operation4();
                                    break;
                                default:
                                    InvalidCharacter(operation);
                                    break;
                            }
                            break;
                        default:
                            InvalidCharacter(lastOperation);
                            break;
                    }
                }
            }
            else
            {
                if (dataClassifications[index].tableType == 3 || dataClassifications[index].tableType == 4)
                {
                    operand = GetLit(dataClassifications[index].tableType, dataClassifications[index].itemNumber);
                    if (isOperand)
                    {
                        CreateError("'+', '-', '*', '/'", $"'{operand}'");
                        flag = false;
                    }
                    else
                        Command(operand);
                }
                else
                {
                    error = $"Ключевые слова некорректны в операции.";
                }
            }
        }
        private void Command(string id)
        {
            stackE.Push(id);
            isOperand = true;
            Next();
        }
        private bool Command(char operation)
        {
            if (stackE.Count <= 1)
            {
                flag = false;
                error = "Операндов должо быть больше чем операций";
                return false;
            }
            arithmeticOperatorMatrix.Add(new Operation($"{arithmeticOperatorMatrix.Count + 1}M", stackE.Pop(), stackE.Pop(), operation));
            stackE.Push($"{arithmeticOperatorMatrix.Count}M");
            return true;
        }
        private void Operation1(string operation)
        {
            stackT.Push(operation);
            Next();
        }
        private void Operation2(string operation)
        {
            if (!Command(stackT.Pop()[0])) return;
            stackT.Push(operation);
            Next();
        }
        private void Operation3()
        {
            stackT.Pop();
            Next();
        }
        private void Operation4()
        {
            if (!Command(stackT.Pop()[0])) return;
            index--;
            Next();
        }
        private void Operation5()
        {
            flag = false;
        }
        private void Operation6()
        {
            flag = true;
        }
    }
}