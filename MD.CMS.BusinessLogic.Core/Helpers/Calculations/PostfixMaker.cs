using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.Helpers.Calculations
{
    public class PostfixMaker
    {
        StringBuilder postfix = new StringBuilder();
        Stack<Operator> operators = new Stack<Operator>();

        /// <summary>
        /// Transforms infix expression to postfix expression
        /// </summary>
        /// <param name="infixExpression">comma separated infix expression</param>
        /// <returns>comma separated postfix expression</returns>
        public string MakePostfixFromInfix(string infixExpression)
        {
            infixExpression = infixExpression.TrimEnd(',');
            string[] infix = infixExpression.Split(',');
            for (int i = 0, length = infix.Length; i < length; i++)
            {
                ProcessToken(infix[i]);
            }
            while (operators.Count > 0)
            {
                postfix.Append(operators.Pop().Sign);
                postfix.Append(",");
            }
            string postfixExpression = postfix.ToString().TrimEnd(',');
            return postfixExpression;
        }

        private void ProcessToken(string token)
        {
            switch (token)
            {
                case "+":
                    CompareOperators(new Operator(2, 2, "+"));
                    break;
                case "-":
                    CompareOperators(new Operator(2, 2, "-"));
                    break;
                case "*":
                    CompareOperators(new Operator(3, 3, "*"));
                    break;
                case "/":
                    CompareOperators(new Operator(3, 3, "/"));
                    break;
                case "^":
                    CompareOperators(new Operator(5, 4, "^"));
                    break;
                case "(":
                    CompareOperators(new Operator(6, 0, ""));
                    break;
                case ")":
                    CompareOperators(new Operator(1, -1, ""));
                    break;
                default:
                    postfix.Append(token);
                    postfix.Append(",");
                    break;
            }
        }

        private void CompareOperators(Operator current)
        {
            while (operators.Count > 0 && operators.Peek().Precedence >= current.IncomePrecedence)
            {
                if (!String.IsNullOrEmpty(operators.Peek().Sign))
                {
                    postfix.Append(operators.Pop().Sign);
                    postfix.Append(",");
                }
            }
            operators.Push(current);
        }
    }

    class Operator
    {
        private int incomePrecedence;
        private int precedence;
        private string sign;

        public int IncomePrecedence
        {
            get { return incomePrecedence; }
            set { incomePrecedence = value; }
        }
        public int Precedence
        {
            get { return precedence; }
            set { precedence = value; }
        }
        public string Sign
        {
            get { return sign; }
            set { sign = value; }
        }
        public Operator(int incomePrecedence, int precedence, string sign)
        {
            this.incomePrecedence = incomePrecedence;
            this.precedence = precedence;
            this.sign = sign;
        }
    }


}
