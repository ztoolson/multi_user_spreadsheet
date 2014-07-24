using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    /// <summary>
    /// This class provides the Evaluate method for evaluating infix expressions.
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// A method of this type is expected as the second parameter to Evaluate.
        /// Semantically, the method should map variable names to integer values,
        /// and should throw an ArgumentException is the variable doesn't have
        /// a value.
        /// </summary>
        public delegate double Lookup(String var);  //Changed return type to double***************************

        /// <summary>
        /// Given a formula written in infix notation as specified in the assignment and a
        /// method that maps variables to values, returns the value of the expression.  If
        /// the expression is malformed, or if it involves a variable that doesn't have
        /// a value (per the lookupMethod), throws an ArgumentException.
        /// </summary>
        public static double Evaluate(String formula, Lookup lookupMethod)    //Changed Return Type to double.
        {
            try
            {
                // The two stacks used by the algorithm
                Stack<double> values = new Stack<double>();     //CHANGED TO TYPE DOUBLE
                Stack<string> operators = new Stack<string>();

                // Split the string into tokens
                string[] substrings = Regex.Split(formula, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

                // Look through the tokens
                foreach (String t in substrings)
                {
                    // There may be leading or trailing spaces as an artifact of the splitting operation.  Remove it.
                    // Ignore empty tokens, which are also artifacts.
                    String token = t.Trim();
                    if (token.Length == 0) continue;

                    // The token is * or /
                    if (token == "*" || token == "/")
                    {
                        operators.Push(token);
                    }

                    // The token is + or -
                    else if (token == "+" || token == "-")
                    {
                        if (operators.OnTop("+", "-"))
                        {
                            values.PushResult(values.Pop(), operators.Pop(), values.Pop());
                        }
                        operators.Push(token);
                    }

                    // The token is (
                    else if (token == "(")
                    {
                        operators.Push(token);
                    }

                    // The token is )
                    else if (token == ")")
                    {
                        if (operators.OnTop("+", "-"))
                        {
                            values.PushResult(values.Pop(), operators.Pop(), values.Pop());
                        }
                        if (operators.Pop() != "(") throw new ArgumentException("Malformed expression");    
                        if (operators.OnTop("*", "/"))
                        {
                            values.PushResult(values.Pop(), operators.Pop(), values.Pop());
                        }
                    }

                    // The token is a variable
                    else if (IsVar(token))
                    {
                        if (operators.OnTop("*", "/"))
                        {
                            values.PushResult(lookupMethod(token), operators.Pop(), values.Pop());
                        }
                        else
                        {
                            values.Push(lookupMethod(token));
                        }
                    }

                    // The token must be an integer
                    else
                    {
                        if (operators.OnTop("*", "/"))
                        {
                            values.PushResult(Double.Parse(token), operators.Pop(), values.Pop());  //Changed from Int32
                        }
                        else
                        {
                            values.Push(Double.Parse(token));  //Changed from Int32
                        }
                    }
                }

                // We've consumed all the tokens.  Do the clean-up steps.
                if (operators.OnTop("+", "-"))
                {
                    values.PushResult(values.Pop(), operators.Pop(), values.Pop());
                }
                if (operators.Count != 0 || values.Count != 1)
                {
                    throw new ArgumentException("Malformed expression");    
                }
                return values.Pop();
            }

            // Convert any exceptions into ArgumentExceptions
            catch (DivideByZeroException)
            {
                throw new ArgumentException("Division by zero");
            }
            catch (InvalidOperationException)   
            {
                throw new ArgumentException("Malformed expression");    
            }
            catch (FormatException e)   
            {
                throw new ArgumentException("Invalid token");   
            }
        }

        /// <summary>
        /// Reports whether v is a variable.
        /// </summary>
        private static bool IsVar(String v)
        {
            return Regex.IsMatch(v, "(^[a-z]|[A-Z])+\\d+$");
        }
    }

    /// <summary>
    /// Provides some useful extension methods for the Stack class that make it easier
    /// to express the algorithm.
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// Reports whether the element is on top of the stack.
        /// </summary>
        public static bool OnTop<T>(this Stack<T> stack, T element)
        {
            return stack.Count > 0 && stack.Peek().Equals(element);     
        }

        /// <summary>
        /// Reports whether one of the elements is on top of the stack.
        /// </summary>
        public static bool OnTop<T>(this Stack<T> stack, T element1, T element2)
        {
            return stack.Count > 0 && (stack.Peek().Equals(element1) || stack.Peek().Equals(element2));
        }

        /// <summary>
        /// Performs the specified operation and pushes the result onto the stack.
        /// </summary>
        public static void PushResult(this Stack<double> stack, double secondArg, String op, double firstArg)  //CHANGED ALL INTS TO DOUBLES
        {
            switch (op)
            {
                case "+":
                    stack.Push(firstArg + secondArg);
                    break;
                case "-":
                    stack.Push(firstArg - secondArg);
                    break;
                case "*":
                    stack.Push(firstArg * secondArg);
                    break;
                case "/":
                    if (secondArg == 0)
                        throw new DivideByZeroException();
                    stack.Push(firstArg / secondArg);  // For some reason, division by zero is resulting in a value of infinity.
                    break;
                default:
                    throw new ArgumentException("Implementation error");        //UNCOVERED
            }
        }

    }
}

