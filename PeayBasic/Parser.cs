using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace PeayBasic
{
    public class Parser
    {
        //private IEnumerable<Token> tokens;
        private List<Token> tokens;
        private int c = 0;
        private string output = "";
        private Dictionary<string, int> symbolTable;
        private string[] forVar;
        private int[] forTo;
        private int[] forToken;
        private int forIndex = 0;
        private List<Tuple<int, int>> cellChanges;

        public Parser(IEnumerable<Token> input)
        {
            tokens = input.ToList<Token>();
            symbolTable = new Dictionary<string,int>();
            forVar = new string[8];
            forTo = new int[8];
            forToken = new int[8];
            cellChanges = new List<Tuple<int, int>>();
        }

        public Tuple<string, List<Tuple<int, int>>> run()
        {
            while(tokens[c].Type != "$$$")
                statement();
            return new Tuple<string, List<Tuple<int, int>>>(output, cellChanges);
        }

        private void next()
        {
            c++;
        }

        private bool match(string tokenType)
        {
            if (tokens[c].Type == tokenType)
            {
                c++;
                return true;
            }
            else
            {
                throw new System.ApplicationException("Error matching " + tokens[c].Type + ". Expected " + tokenType + "!");
            }
        }

        private void statement()
        {
            switch (tokens[c].Type)
            {
                case "KEY_FOR":
                    forStatement();
                    break;

                case "KEY_PRINT":
                    printStatement();
                    break;

                case "KEY_LET":
                    letStatement();
                    break;

                case "KEY_IF":
                    ifStatement();
                    break;

                case "KEY_NEXT":
                    nextStatement();
                    break;

                case "KEY_DRAW":
                    drawStatement();
                    break;
            }
        }

        private void forStatement()
        {
            string fVar;
            int to;

            match("KEY_FOR");
            fVar = tokens[c].Value;
            match("ID");
            match("OP_EQU");
            if (symbolTable.ContainsKey(fVar))
                symbolTable[fVar] = expr();
            else
                symbolTable.Add(fVar, expr());
            match("KEY_TO");
            to = expr();

            forToken[forIndex] = c;
            forVar[forIndex] = fVar;
            forTo[forIndex] = to;

            forIndex++;
        }

        private void nextStatement()
        {
            match("KEY_NEXT");
            string varName = tokens[c].Value;
            match("ID");
            symbolTable[varName]++;
            if (symbolTable[varName] <= forTo[forIndex - 1])
                c = forToken[forIndex - 1];
            else
                forIndex--;

        }

        private void drawStatement()
        {
            match("KEY_DRAW");
            int x = expr();
            match("OP_COMMA");
            int y = expr();

            cellChanges.Add(new Tuple<int, int>(x, y));
        }

        private void returnStatement()
        {
        }

        private void gosubStatement() //maybe
        {
        }

        private void letStatement()
        {
            match("KEY_LET");
            string varName = tokens[c].Value;
            match("ID");
            match("OP_EQU");
            if (tokens[c].Type == "KEY_COS")
            {
                match("KEY_COS");
                if (symbolTable.ContainsKey(varName))
                    symbolTable[varName] = Convert.ToInt32(Math.Cos(expr()));
                else
                    symbolTable.Add(varName, Convert.ToInt32(Math.Cos(expr())));
            }

            else
            {
                if (symbolTable.ContainsKey(varName))
                    symbolTable[varName] = expr();
                else
                    symbolTable.Add(varName, expr());
            }
        }

        private void ifStatement()
        {
            match("KEY_IF");
            int r = relation();
            match("KEY_THEN");
            if (r > 0)
            {
                do
                {
                    statement();
                } while (tokens[c].Type != "KEY_ENDIF");
            }
            else
            {
                do
                {
                    next();
                } while (tokens[c].Type != "KEY_ENDIF");
            } 

            match("KEY_ENDIF");
            
        }

        private void printStatement()
        {
            match("KEY_PRINT");

            do
            {
                if (tokens[c].Type == "ID" || tokens[c].Type == "NUM")
                    output += expr();
                else if (tokens[c].Type == "STRING_LITERAL")
                {
                    string t = tokens[c].Value;
                    t = t.Substring(1, t.Length - 2);
                    output += t;
                    next();
                }
                else if (tokens[c].Type == "OP_COMMA")
                {
                    output += " ";
                    next();
                }
                else
                    break;
            } while (tokens[c].Type != "$$$");

            output += Environment.NewLine;
        }

        private int relation()
        {
            int r1, r2;
            string op;

            r1 = expr();
            op = tokens[c].Type;

            while(op == "OP_LT" || op == "OP_GT" || op == "OP_EQU")
            {
                next();
                r2 = expr();

                bool tmp = false;
                switch(op)
                {
                    case "OP_LT":
                        tmp = r1 < r2;
                        break;

                    case "OP_GT":
                        tmp = r1 > r2;
                        break;

                    case "OP_EQU":
                        tmp = r1 == r2;
                        break;
                }

                if (tmp)
                    r1 = 1;
                else
                    r1 = 0;

                op = tokens[c].Type;
            }

            return r1;
        }

        private int expr()
        {
            int t1, t2;
            string op;

            t1 = term();
            op = tokens[c].Type;

            while (op == "OP_ADD" || op == "OP_SUB")
            {
                next();
                t2 = term();

                if (op == "OP_ADD")
                    t1 = t1 + t2;
                else if (op == "OP_SUB")
                    t1 = t1 - t2;

                op = tokens[c].Type;
            }

            return t1;
        }

        private int term()
        {
            int f1, f2;
            string op;

            f1 = factor();
            op = tokens[c].Type;

            while(op == "OP_MULT" || op == "OP_DIV" || op == "OP_MOD")
            {
                next();
                f2 = factor();

                switch(op)
                {
                    case "OP_MULT":
                        f1 = f1 * f2;
                        break;

                    case "OP_DIV":
                        f1 = f1 / f2;
                        break;

                    case "OP_MOD":
                        f1 = f1 % f2;
                        break;
                }

                op = tokens[c].Type;
            }

            return f1;
        }

        private int factor()
        {
            int r = -1;

            switch(tokens[c].Type)
            {
                case "NUM":
                    r = Convert.ToInt32(tokens[c].Value);
                    match("NUM");
                    break;

                case "OP_LPAREN":
                    match("OP_LPAREN");
                    r = expr();
                    match("OP_RPAREN");
                    break;

                case "ID":
                    r = symbolTable[tokens[c].Value];
                    match("ID");
                    break;
            }

            return r;
        }
    }
}
