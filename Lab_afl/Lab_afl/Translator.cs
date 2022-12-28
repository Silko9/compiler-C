namespace Lab_afl
{
    public class Translator
    {
        LexicalAnalysis lexical;
        public LexicalAnalysis Lexical { get { return lexical; } }
        SyntacticAnalysis syntactic;
        public SyntacticAnalysis Syntactic { get { return syntactic; } }
        string error;
        public string Error { get { return error; } }
        public Translator(){}
        public bool Process(string str)
        {
            lexical = new LexicalAnalysis(str);
            if (lexical.Scanner())
            {
                syntactic = new SyntacticAnalysis(lexical);
                if (syntactic.Scanner())
                    return true;
                error = syntactic.Error;
                return false;
            }
            error = lexical.Error;
            return false;
        }
    }
}
