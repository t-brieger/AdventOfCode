using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions._2015
{
    class Year2015Day07 : Solution
    {
        private class AST
        {
            public AST left;
            public AST right;
        }

        private class ASTLiteral : AST
        {
            public ushort value;

            public ASTLiteral(ushort val)
            {
                this.value = val;
            }
        }

        private class ASTAnd : AST { }

        private class ASTOr : AST { }

        private class ASTShift : AST
        {
            public bool isLeft;
        }

        private class ASTNot : AST { }

        private AST buildAST(Dictionary<string, (byte, string, string)> wTO, string which, Dictionary<string, AST> previous = null)
        {
            if (previous == null)
                previous = new Dictionary<string, AST>();

            if (previous.ContainsKey(which))
                return previous[which];

            if (ushort.TryParse(which, out ushort x))
                return new ASTLiteral(x);

            (byte, string, string) Operation = wTO[which];

            switch (Operation.Item1)
            {
                case 0:
                    ASTLiteral a = new ASTLiteral(ushort.Parse(Operation.Item2));
                    previous.Add(which, a);
                    return a;
                case 1:
                    ASTAnd b = new ASTAnd();
                    b.left = buildAST(wTO, Operation.Item2, previous);
                    b.right = buildAST(wTO, Operation.Item3, previous);
                    previous.Add(which, b);
                    return b;
                case 2:
                    ASTOr c = new ASTOr();
                    c.left = buildAST(wTO, Operation.Item2, previous);
                    c.right = buildAST(wTO, Operation.Item3, previous);
                    previous.Add(which, c);
                    return c;
                case 3:
                    ASTShift d = new ASTShift();
                    d.isLeft = false;
                    d.left = buildAST(wTO, Operation.Item2, previous);
                    d.right = buildAST(wTO, Operation.Item3, previous);
                    previous.Add(which, d);
                    return d;
                case 4:
                    ASTShift e = new ASTShift();
                    e.isLeft = true;
                    e.left = buildAST(wTO, Operation.Item2, previous);
                    e.right = buildAST(wTO, Operation.Item3, previous);
                    previous.Add(which, e);
                    return e;
                case 5:
                    ASTNot f = new ASTNot();
                    f.left = buildAST(wTO, Operation.Item2, previous);
                    previous.Add(which, f);
                    return f;
                case 6:
                    AST g = new AST();
                    g.left = buildAST(wTO, Operation.Item2, previous);
                    previous.Add(which, g);
                    return g;
            }

            return null;
        }

        private ushort evaluateAST(AST ast)
        {
            if (ast is ASTLiteral asLiteral)
                return asLiteral.value;
            else if (ast is ASTAnd)
                return (ushort)(evaluateAST(ast.left) & evaluateAST(ast.right));
            else if (ast is ASTOr)
                return (ushort)(evaluateAST(ast.left) | evaluateAST(ast.right));
            else if (ast is ASTShift asShift)
                if (asShift.isLeft)
                    return (ushort)(evaluateAST(ast.left) << evaluateAST(ast.right));
                else
                    return (ushort)(evaluateAST(ast.left) >> evaluateAST(ast.right));
            else if (ast is ASTNot)
                return (ushort)(~evaluateAST(ast.left));
            else
                return evaluateAST(ast.left);
        }

        public override string Part1(string input)
        {
            //input = "123 -> x\n456 -> y\nx AND y -> d\nx OR y -> e\nx LSHIFT 2 -> f\ny RSHIFT 2 -> g\nNOT x -> h\nNOT y -> i\nh AND i -> z";

            string[] lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            //Literal:    0
            //AND:        1
            //OR:         2
            //RSHIFT:     3
            //LSHIFT:     4
            //NOT:        5
            //Assignment: 6
            //                  op,   val1,   val2
            Dictionary<string, (byte, string, string)> wiresToOperations = new Dictionary<string, (byte, string, string)>();

            foreach (string line in lines)
            {
                string[] inOut = line.Split(" -> ");
                byte operation = (byte)(line.Contains("AND") ? 1 : line.Contains("OR") ? 2 : line.Contains("RSHIFT") ? 3 : line.Contains("LSHIFT") ? 4 : line.Contains("NOT") ? 5 : 0);
                if (inOut[0].Any(x => x < '0' || x > '9') && operation == 0)
                    operation = 6;
                string[] inSplit = inOut[0].Split(' ');
                string left, right = null;
                if (operation == 0 || operation == 6)
                    left = inSplit[0];
                else if (operation == 5)
                    left = inSplit[1];
                else
                {
                    left = inSplit[0];
                    right = inSplit[2];
                }

                wiresToOperations.Add(inOut[1], (operation, left, right));
            }

            AST ast = buildAST(wiresToOperations, "a");

            return evaluateAST(ast).ToString();
        }

        public override string Part2(string input)
        {
            return "";
        }
    }
}
