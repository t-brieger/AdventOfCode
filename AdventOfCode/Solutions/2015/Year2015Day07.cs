using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    class Year2015Day07 : Solution
    {
        private Ast BuildAst(IReadOnlyDictionary<string, (byte, string, string)> wireToOperation, string which,
            Dictionary<string, Ast> previous = null)
        {
            previous ??= new Dictionary<string, Ast>();

            if (previous.ContainsKey(which))
                return previous[which];

            if (ushort.TryParse(which, out ushort x))
                return new AstLiteral { value = x, id = which };

            (byte op, string w1, string w2) = wireToOperation[which];

            Ast ast = null;

            switch (op)
            {
                case 0:
                    ast = new AstLiteral { value = ushort.Parse(w1) };
                    break;
                case 1:
                    ast = new AstAnd();
                    ast.left = this.BuildAst(wireToOperation, w1, previous);
                    ast.right = this.BuildAst(wireToOperation, w2, previous);
                    break;
                case 2:
                    ast = new AstOr();
                    ast.left = this.BuildAst(wireToOperation, w1, previous);
                    ast.right = this.BuildAst(wireToOperation, w2, previous);
                    break;
                case 3:
                    ast = new AstShift();
                    ((AstShift)ast).isLeft = false;
                    ast.left = this.BuildAst(wireToOperation, w1, previous);
                    ast.right = this.BuildAst(wireToOperation, w2, previous);
                    break;
                case 4:
                    ast = new AstShift();
                    ((AstShift)ast).isLeft = true;
                    ast.left = this.BuildAst(wireToOperation, w1, previous);
                    ast.right = this.BuildAst(wireToOperation, w2, previous);
                    break;
                case 5:
                    ast = new AstNot();
                    ast.left = this.BuildAst(wireToOperation, w1, previous);
                    break;
                case 6:
                    ast = new Ast
                    {
                        left = this.BuildAst(wireToOperation, w1, previous)
                    };
                    break;
            }

            previous[which] = ast;
            if (ast != null)
                ast.id = which;

            return ast;
        }

        private static ushort EvaluateAst(Ast ast, Dictionary<string, ushort> evaluated = null)
        {
            evaluated ??= new Dictionary<string, ushort>();
            if (evaluated.ContainsKey(ast.id))
                return evaluated[ast.id];

            ushort left = 0;
            if (ast.left != null)
                left = EvaluateAst(ast.left, evaluated);

            ushort right = 0;
            if (ast.right != null)
                right = EvaluateAst(ast.right, evaluated);

            ushort result = ast switch
            {
                AstLiteral asLiteral => asLiteral.value,
                AstAnd => (ushort)(left & right),
                AstOr => (ushort)(left | right),
                AstShift { isLeft: true } => (ushort)(left << right),
                AstShift => (ushort)(left >> right),
                AstNot => (ushort)~left,
                _ => left
            };

            evaluated.Add(ast.id, result);
            return result;
        }

        public override string Part1(string input)
        {
            string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            //Literal:    0
            //AND:        1
            //OR:         2
            //RSHIFT:     3
            //LSHIFT:     4
            //NOT:        5
            //Assignment: 6
            //                  op,   val1,   val2
            Dictionary<string, (byte, string, string)> wiresToOperations =
                new();

            foreach (string line in lines)
            {
                string[] inOut = line.Split(" -> ");
                byte operation = (byte)(line.Contains("AND") ? 1 :
                    line.Contains("OR") ? 2 :
                    line.Contains("RSHIFT") ? 3 :
                    line.Contains("LSHIFT") ? 4 :
                    line.Contains("NOT") ? 5 : 0);
                if (inOut[0].Any(x => x is < '0' or > '9') && operation == 0)
                    operation = 6;
                string[] inSplit = inOut[0].Split(' ');
                string left, right = null;
                switch (operation)
                {
                    case 0:
                    case 6:
                        left = inSplit[0];
                        break;
                    case 5:
                        left = inSplit[1];
                        break;
                    default:
                        left = inSplit[0];
                        right = inSplit[2];
                        break;
                }

                wiresToOperations.Add(inOut[1], (operation, left, right));
            }

            Ast ast = this.BuildAst(wiresToOperations, "a");

            return EvaluateAst(ast).ToString();
        }

        public override string Part2(string input)
        {
            string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            //Literal:    0
            //AND:        1
            //OR:         2
            //RSHIFT:     3
            //LSHIFT:     4
            //NOT:        5
            //Assignment: 6
            //                  op,   val1,   val2
            Dictionary<string, (byte, string, string)> wiresToOperations =
                new();

            foreach (string line in lines)
            {
                string[] inOut = line.Split(" -> ");
                byte operation = (byte)(line.Contains("AND") ? 1 :
                    line.Contains("OR") ? 2 :
                    line.Contains("RSHIFT") ? 3 :
                    line.Contains("LSHIFT") ? 4 :
                    line.Contains("NOT") ? 5 : 0);
                if (inOut[0].Any(x => x is < '0' or > '9') && operation == 0)
                    operation = 6;
                string[] inSplit = inOut[0].Split(' ');
                string left, right = null;
                switch (operation)
                {
                    case 0:
                    case 6:
                        left = inSplit[0];
                        break;
                    case 5:
                        left = inSplit[1];
                        break;
                    default:
                        left = inSplit[0];
                        right = inSplit[2];
                        break;
                }

                wiresToOperations.Add(inOut[1], (operation, left, right));
            }

            Ast ast = this.BuildAst(wiresToOperations, "a");
            ushort aVal = EvaluateAst(ast);
            wiresToOperations = wiresToOperations.Where(kvp => kvp.Key != "b")
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            wiresToOperations.Add("b", (0, aVal.ToString(), null));

            ast = this.BuildAst(wiresToOperations, "a");
            return EvaluateAst(ast).ToString();
        }

        private class Ast
        {
            public string id;
            public Ast left;
            public Ast right;
        }

        private class AstLiteral : Ast
        {
            public ushort value;
        }

        private class AstAnd : Ast
        {
        }

        private class AstOr : Ast
        {
        }

        private class AstShift : Ast
        {
            public bool isLeft;
        }

        private class AstNot : Ast
        {
        }
    }
}