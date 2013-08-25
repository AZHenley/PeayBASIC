using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PeayBasic
{
    public partial class PeayBasic : Form
    {
        public OutputText OutText;
        public OutputGraphics OutGraphics;
        public Lexer lex;
        public Parser parse;

        public PeayBasic()
        {
            InitializeComponent();

            lex = new Lexer();

            lex.AddDefinition(new TokenDefinition("WS", new Regex(@"\s+"), true));

            lex.AddDefinition(new TokenDefinition("KEY_LET", new Regex(@"let\b", RegexOptions.IgnoreCase)));
            lex.AddDefinition(new TokenDefinition("KEY_GOSUB", new Regex(@"gosub\b", RegexOptions.IgnoreCase)));
            lex.AddDefinition(new TokenDefinition("KEY_PRINT", new Regex(@"print\b", RegexOptions.IgnoreCase)));
            lex.AddDefinition(new TokenDefinition("KEY_IF", new Regex(@"if\b", RegexOptions.IgnoreCase)));
            lex.AddDefinition(new TokenDefinition("KEY_ENDIF", new Regex(@"endif\b", RegexOptions.IgnoreCase)));
            lex.AddDefinition(new TokenDefinition("KEY_THEN", new Regex(@"then\b", RegexOptions.IgnoreCase)));
            lex.AddDefinition(new TokenDefinition("KEY_FOR", new Regex(@"for\b", RegexOptions.IgnoreCase)));
            lex.AddDefinition(new TokenDefinition("KEY_END", new Regex(@"end\b", RegexOptions.IgnoreCase)));
            lex.AddDefinition(new TokenDefinition("KEY_TO", new Regex(@"to\b", RegexOptions.IgnoreCase)));
            lex.AddDefinition(new TokenDefinition("KEY_NEXT", new Regex(@"next\b", RegexOptions.IgnoreCase)));
            lex.AddDefinition(new TokenDefinition("KEY_DRAW", new Regex(@"draw\b", RegexOptions.IgnoreCase)));
            lex.AddDefinition(new TokenDefinition("KEY_COS", new Regex(@"cos\b", RegexOptions.IgnoreCase)));

            lex.AddDefinition(new TokenDefinition("STRING_LITERAL", new Regex(@"\'.*\'")));
            lex.AddDefinition(new TokenDefinition("ID", new Regex(@"[A-Za-z_$]\w*")));
            lex.AddDefinition(new TokenDefinition("NUM", new Regex(@"\d+")));

            lex.AddDefinition(new TokenDefinition("OP_LPAREN", new Regex(@"\(")));
            lex.AddDefinition(new TokenDefinition("OP_RPAREN", new Regex(@"\)")));
            lex.AddDefinition(new TokenDefinition("OP_COMMA", new Regex(@"\,")));
            lex.AddDefinition(new TokenDefinition("OP_EXP", new Regex(@"\^")));
            lex.AddDefinition(new TokenDefinition("OP_MULT", new Regex(@"\*")));
            lex.AddDefinition(new TokenDefinition("OP_DIV", new Regex(@"\/")));
            lex.AddDefinition(new TokenDefinition("OP_MOD", new Regex(@"\%")));
            lex.AddDefinition(new TokenDefinition("OP_ADD", new Regex(@"\+")));
            lex.AddDefinition(new TokenDefinition("OP_SUB", new Regex(@"\-")));
            lex.AddDefinition(new TokenDefinition("OP_EQU", new Regex(@"\=")));
            lex.AddDefinition(new TokenDefinition("OP_LT", new Regex(@"\<")));
            lex.AddDefinition(new TokenDefinition("OP_GT", new Regex(@"\>")));
            lex.AddDefinition(new TokenDefinition("OP_AND", new Regex(@"\&")));
            lex.AddDefinition(new TokenDefinition("OP_OR", new Regex(@"\|")));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (OutText != null)
                OutText.Close();
            if (OutGraphics != null)
                OutGraphics.Close();

            var tokens = lex.Tokenize(textBox1.Text);
            parse = new Parser(tokens);
            //string output = parse.run();
            var output = parse.run();

            string outputText = output.Item1;
            outputText += Environment.NewLine;

            //DEBUG
            string test = "";
            /*foreach(Token t in tokens)
            {
                test += t.ToString();
                test += Environment.NewLine;
            }*/

            //if (OutText == null || OutText.IsDisposed)
            //{
                OutText = new OutputText(outputText + test);
            //}
            //if (OutGraphics == null || OutGraphics.IsDisposed)
            //{
                OutGraphics = new OutputGraphics(output.Item2);
            //}

            OutText.Show();
            OutGraphics.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OutText.Close();
            OutGraphics.Close();
        }
    }
}
