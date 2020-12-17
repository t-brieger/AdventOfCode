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
            public string id;
        }

        private class ASTLiteral : AST
        {
            public ushort value;
        }

        private class ASTAnd : AST
        {
        }

        private class ASTOr : AST
        {
        }

        private class ASTShift : AST
        {
            public bool isLeft;
        }

        private class ASTNot : AST
        {
        }

        private AST buildAST(Dictionary<string, (byte, string, string)> wireToOperation, string which,
            Dictionary<string, AST> previous = null)
        {
            if (previous == null)
                previous = new Dictionary<string, AST>();

            if (previous.ContainsKey(which))
                return previous[which];

            if (ushort.TryParse(which, out ushort x))
                return new ASTLiteral {value = x, id = which};

            (byte, string, string) Operation = wireToOperation[which];

            AST ast = null;

            switch (Operation.Item1)
            {
                case 0:
                    ast = new ASTLiteral {value = ushort.Parse(Operation.Item2)};
                    break;
                case 1:
                    ast = new ASTAnd();
                    ast.left = buildAST(wireToOperation, Operation.Item2, previous);
                    ast.right = buildAST(wireToOperation, Operation.Item3, previous);
                    break;
                case 2:
                    ast = new ASTOr();
                    ast.left = buildAST(wireToOperation, Operation.Item2, previous);
                    ast.right = buildAST(wireToOperation, Operation.Item3, previous);
                    break;
                case 3:
                    ast = new ASTShift();
                    ((ASTShift) ast).isLeft = false;
                    ast.left = buildAST(wireToOperation, Operation.Item2, previous);
                    ast.right = buildAST(wireToOperation, Operation.Item3, previous);
                    break;
                case 4:
                    ast = new ASTShift();
                    ((ASTShift) ast).isLeft = true;
                    ast.left = buildAST(wireToOperation, Operation.Item2, previous);
                    ast.right = buildAST(wireToOperation, Operation.Item3, previous);
                    break;
                case 5:
                    ast = new ASTNot();
                    ast.left = buildAST(wireToOperation, Operation.Item2, previous);
                    break;
                case 6:
                    ast = new AST();
                    ast.left = buildAST(wireToOperation, Operation.Item2, previous);
                    break;
            }

            previous[which] = ast;
            ast.id = which;

            return ast;
        }

        private ushort evaluateAST(AST ast, Dictionary<string, ushort> evaluated = null)
        {
            evaluated ??= new Dictionary<string, ushort>();
            if (evaluated.ContainsKey(ast.id))
                return evaluated[ast.id];

            ushort left = 0;
            if (ast.left != null)
                left = evaluateAST(ast.left, evaluated);

            ushort right = 0;
            if (ast.right != null)
                right = evaluateAST(ast.right, evaluated);

            ushort result = ast switch
            {
                ASTLiteral asLiteral => asLiteral.value,
                ASTAnd _ => (ushort) (left & right),
                ASTOr _ => (ushort) (left | right),
                ASTShift {isLeft: true} => (ushort) (left << right),
                ASTShift _ => (ushort) (left >> right),
                ASTNot _ => (ushort) ~left,
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
                new Dictionary<string, (byte, string, string)>();

            foreach (string line in lines)
            {
                string[] inOut = line.Split(" -> ");
                byte operation = (byte) (line.Contains("AND") ? 1 :
                    line.Contains("OR") ? 2 :
                    line.Contains("RSHIFT") ? 3 :
                    line.Contains("LSHIFT") ? 4 :
                    line.Contains("NOT") ? 5 : 0);
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
                new Dictionary<string, (byte, string, string)>();

            foreach (string line in lines)
            {
                string[] inOut = line.Split(" -> ");
                byte operation = (byte) (line.Contains("AND") ? 1 :
                    line.Contains("OR") ? 2 :
                    line.Contains("RSHIFT") ? 3 :
                    line.Contains("LSHIFT") ? 4 :
                    line.Contains("NOT") ? 5 : 0);
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
            ushort aVal = evaluateAST(ast);
            wiresToOperations = wiresToOperations.Where(kvp => kvp.Key != "b")
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            wiresToOperations.Add("b", (0, aVal.ToString(), null));
            
            ast = buildAST(wiresToOperations, "a");
            return evaluateAST(ast).ToString();
        }
    }
}
