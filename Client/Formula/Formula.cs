using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using FormulaEvaluator;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using 
    /// double-precision floating-point syntax; variables that consist of a letter or 
    /// underscore followed by zero or more letters, underscores, or digits; parentheses; 
    /// and the four operator symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is 
    /// used to add extra restrictions on the validity of a variable (beyond the standard 
    /// requirement that it consist of a letter or underscore followed by zero or more letters,
    /// underscores, of digits.)  Their use is described in detail in the constructor and 
    /// method comments.
    /// </summary>
    public class Formula
    {
        //  The tokens of a Formula.
        private IEnumerable<String> tokens;
        //  Will represent the formula as a string.
        private String formula;

        //Delegate
        private delegate double Lookup(String v);
        //Delegate declaration.
        private FormulaEvaluator.Evaluator.Lookup lookedUpValue;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>

        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            if (ReferenceEquals(formula, null))
                throw new FormulaFormatException("Formula cannot be null.");

            //  First get tokens from "formula"
            this.tokens = GetTokens(normalize(formula));

            //  Then check to see that "formula" is of valid syntax.
            ValidSyntax(tokens);

            formula = this.tokens.ToString();

            //Check isValid Func for further validity, it check fails, throw exception.
            if (!isValid(this.tokens.ToString()))
                throw new FormulaFormatException("isValid delegate, determined formula to be invalid.");  
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked 
        /// up via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.  
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {   //  Return value.
            object result = null;

            //  This delegate setup is borrowed from PS1.
            lookedUpValue = new FormulaEvaluator.Evaluator.Lookup(lookup);
            try
            {
                result = FormulaEvaluator.Evaluator.Evaluate(this.ToString(), lookedUpValue);
                return result;
            }
            catch (ArgumentException e)
            {
                return new FormulaError();
            }
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///         
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z" 
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z". 
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            //Will return..
            List<String> putVariables = new List<String>();
            //Used in Tryparsing..
            //double outParse = 0;

            //First check for empty formula
            if ((this.formula == null) || (this.formula.Length == 0))
                //Return a new empty IEnumerable.
                return new List<String>();                      

            //since IsVariable method returns true for doubles....
            //  Manipulate this.formula
            //  Start by grabbing this.Tokens                                            
            //for each variable in grabTokens...
            foreach (string token in this.tokens.ToList())
            {
                //If "token" is an operator, continue
                if (IsOperator(token.Trim()))
                    continue;
                //Avoid duplicates
                if (putVariables.Contains(token))
                    continue;
                //  If IsVariable is true, and I can't DoubleTryParse said token, 
                //      and the receiving list does not already contain "token"..., and if token is not an operator..
                if (Regex.IsMatch(token, "^[a-z|A-Z]+[0-9]+$"))
                {
                    //add token to put variables.
                    putVariables.Add(token);
                }
            }
            return putVariables;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            String formulaToString = null;

            //Concatenate successive tokens to a string..
            foreach (string token in this.tokens.ToList())
            {
                formulaToString = formulaToString + token;
            }
            //Return space-less string.
            return formulaToString;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.                                    
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens, which are compared as doubles, and variable tokens,
        /// whose normalized forms are compared as strings.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {   //  Null check...
            try
            {
                if (obj.Equals(null))
                { }
            }
            catch (NullReferenceException)
            {
                return false;
            }
            //  Parse guys.
            double outLeft = 0;
            double outRight = 0;

            //First check of obj not being a formula
            if (!(obj is Formula))
                return false;
            //Get this's tokens
            IEnumerable<String> leftTokens = GetTokens(this.ToString());
            //Make a normalized formula out of obj.
            Formula objFormula = new Formula(obj.ToString());
            //  Get obj's Tokens.
            IEnumerable<String> rightTokens = GetTokens(objFormula.ToString());
            //Now make arrays out of the IEnnumerables.
            String[] leftTokensArray = leftTokens.ToArray();
            String[] rightTokensArray = rightTokens.ToArray();

            //  Now compare each IEnummerable token by token.
            for (int pos = 0; pos < leftTokensArray.Length; pos++)
            {
                //My IsVariable returns true for doubles.
                //call IsVariable.
                //If they are both variables
                if (IsVariable(leftTokensArray[pos]) && IsVariable(rightTokensArray[pos]))
                {
                    //  Double comparison.        //if this(left) can be parsed
                    if (Double.TryParse(leftTokensArray[pos], out outLeft))
                    {       //And obj(right) can be parsed.
                        if (Double.TryParse(rightTokensArray[pos], out outRight))
                        {       //If outs equal each other, continue
                            if (outRight == outLeft)
                                continue;
                            else     // If they can both be parsed but they don't equal each other, return false.
                                return false;
                        }
                        //  But obj can't..
                        else
                            return false;   
                    }   //Else if I cant Double parse current left position....
                    else
                    {
                        //  but I can double parse current right position...
                        if (Double.TryParse(rightTokensArray[pos], out outRight))
                        {
                            return false;   
                        }// Else if neither variables can be parsed as doubles.
                        else
                        {   //Then compare as strings, if equal.....
                            if (leftTokensArray[pos].Equals(rightTokensArray[pos]))
                                continue;
                            else  //else if both variables don't equal each other as strings.
                            {
                                return false;
                            }
                        }
                    }
                }
                //Else if they are not both variables.
                else
                {
                    //  Left is variable, right isn't
                    if (IsVariable(leftTokensArray[pos]) && !IsVariable(rightTokensArray[pos]))
                        return false;   
                    //  Left isn't variable, right is
                    if (!IsVariable(leftTokensArray[pos]) && IsVariable(rightTokensArray[pos]))
                        return false;   
                    //  Else they are both not variables, compare strings.
                    if (leftTokensArray[pos].Equals(rightTokensArray[pos]))
                        continue;
                    //else strings don't match, return false
                    else
                        return false;   
                }
            }
            return true;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {   //If both are null....
            if (ReferenceEquals(f1, f2))
                return true;
            //  If one is null and one is not...
            if (ReferenceEquals(f1, null) && !ReferenceEquals(f2, null))
            {
                return false;
            }
            if (!ReferenceEquals(f1, null) && ReferenceEquals(f2, null))    
            {
                return false;
            }
            //  If they are equal...
            if (f1.Equals(f2))
                return true;
            //  If not...
            else
                return false;
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            if (ReferenceEquals(f1, f2))
                return false;
            //  If one is null and one is not...
            if (ReferenceEquals(f1, null) && !ReferenceEquals(f2, null))
            {
                return true;
            }
            if (!ReferenceEquals(f1, null) && ReferenceEquals(f2, null))    
            {
                return true;
            }
            //If they are equal...
            if (f1.Equals(f2))
                return false;
            //  If not...
            else
                return true;
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be 
        /// extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            int hashy = 7;
            foreach (string token in GetTokens(this.ToString()))
            {
                foreach (char guy in token)
                {
                    hashy = hashy * 31 + guy;
                }
            }
            return hashy;
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left 
        /// paren; right paren; one of the four operator symbols; a string consisting of a letter 
        /// or underscore followed by zero or more letters, digits, or underscores; a double 
        /// literal; and anything that doesn't match one of those patterns.  There are no 
        /// empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }

        /// <summary>
        /// This method determines whether a given formula has the valid syntax.
        /// </summary>
        /// <param name="tokens">IEnumerable of parsed tokens</param>
        /// <returns>True if formula is valid, throws exception otherwise</returns>
        private bool ValidSyntax(IEnumerable<string> tokens) //<-- 
        {
            //1. There must be at least one token.
            if (!AtLeastOneToken(tokens))
                throw new FormulaFormatException("There are no tokens in the formula.");

            //4. The first token of an expression must be a number, a variable, or an opening parenthesis.
            if (!FirstToken(tokens))
                throw new FormulaFormatException("The first token of the formula is invalid.");

            //5. The last token of an expression must be a number, a variable, or a closing parenthesis.
            //   First check to see if the last token in the formula is an opperator.
            if (!LastToken(tokens))
                throw new FormulaFormatException("The last token in the formula is invalid.");

            string currentToken;
            string nextToken;

            // These will keep track of paren balance.
            int leftParenCount = 0;
            int rightParenCount = 0;

            //  Assign the IEnumerable to an array.
            String[] tokensToArray = tokens.ToArray<String>();

            for (int pos = 0; pos <= tokensToArray.Length - 2; pos++)
            {
                currentToken = tokensToArray[pos];
                nextToken = tokensToArray[pos + 1];

                if (currentToken.Equals("("))
                {
                    leftParenCount++;
                    if (!IsVariable(nextToken) && !nextToken.Equals("("))
                        throw new FormulaFormatException("Invlaid token folowing open paren detected.");
                }
                if (IsOperator(currentToken))
                {
                    if (!IsVariable(nextToken) && !nextToken.Equals("("))
                        throw new FormulaFormatException("Invalid token folowing operator detected.");
                }
                if (IsVariable(currentToken))
                {
                    if (!IsOperator(nextToken) && !nextToken.Equals(")"))
                        throw new FormulaFormatException("Invalid token following a variable or number.");
                }
                if (currentToken.Equals(")"))
                {
                    rightParenCount++;
                    ParenBalance(leftParenCount, rightParenCount);
                    if (!IsOperator(nextToken) && !nextToken.Equals(")"))
                        throw new FormulaFormatException("Invalid token following a closing paren.");
                }
            }
            if (tokensToArray[tokensToArray.Length - 1] == ")")
            {
                rightParenCount++;
                ParenBalance(leftParenCount, rightParenCount);
            }
            ParenTotal(leftParenCount, rightParenCount);

            return true;
        }

        /// <summary>
        /// This method determines if a string is an invalid variable.
        /// </summary>
        /// <param name="variable">A string to be checked for variable validity</param>
        /// <returns>Either true for a valid variable, or double, 
        /// or throws FormulaFormatException. Never returns false</returns>
        private bool IsVariable(string variable)
        {   //  Used in tryParsing a token.
            double outParse = 0;

            //Return false if the variable is an operator.
            if (IsOperator(variable) || IsParen(variable))
                return false;

            //First thing check TryParse for returning true.
            //  If a token can be tryparsed, return true,
            //      If a token cannot be tryparsed, continue checking....
            if (Double.TryParse(variable, out outParse))
                return true;

            //NEW FOR PS5.
            //Implement new PS5 variable definition.
            if (!Regex.IsMatch(variable, "^[a-z|A-Z]+[0-9]+$")) //Changed from 1-9 just to see what would happen.
                throw new FormulaFormatException("Invalid variable by PS5 definition detected");
            //NEW FOR PS5.

            //  Else we will assume that "variable" is valid, return true.
            return true;
        }

        /// <summary>
        /// This method determines if a token is an operator.
        /// </summary>
        /// <param name="token">A token</param>
        /// <returns>True/False if token is/is not an operator</returns>
        private bool IsOperator(String token)
        {// If the token is an operator, return true
            if (token.Equals("+") || token.Equals("-") || token.Equals("*") || token.Equals("/"))
                return true;
            //if not, return false.
            return false;
        }

        //********************************* New Helper Methods ****************************************************

        private bool AtLeastOneToken(IEnumerable<string> formula)
        {
            if (formula.Count() > 0) //We must verify that the count method is doing what we think it is doing!
                return true;
            else
                return false;
        }

        private bool FirstToken(IEnumerable<string> tokens)
        {
            //The IsVariable method will determine if a position in "tokens" is a valid variable, or a double
            if (IsVariable(tokens.ElementAt(0)))
                return true;
            if (tokens.ElementAt(0).Equals("("))
                return true;
            return false;
        }

        private bool LastToken(IEnumerable<string> tokens)
        {
            if (IsVariable(tokens.ElementAt(tokens.Count() - 1)))
                return true;
            if (tokens.ElementAt(tokens.Count() - 1).Equals(")"))
                return true;
            return false;
        }

        private bool IsParen(string token)
        {
            if (token.Equals("(") || token.Equals(")"))
                return true;
            return false;
        }
        //THIS METHOD IS NEVER ACTUALLY CALLED!!!
        private void UndefinedToken(string token)
        {// If any given token in a formula is not a paren, an operator, a variable, or a double; throw exception.
            if (!IsParen(token) && !IsOperator(token) && !IsVariable(token))    //UNCOVERED
                throw new FormulaFormatException("Undefined token detected.");  //UNCOVERED
        }

        private void ParenBalance(int open, int close)
        {
            if (close > open)
                throw new FormulaFormatException("Paren imbalance detected.");
        }

        private void ParenTotal(int open, int close)
        {
            if (open != close)
                throw new FormulaFormatException("Paren totals do not match.");
        }

        //********************************* End of new Helper Methods **********************************************
    }       //<---- End of Formula Class

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()        
        {
            Reason = reason;    
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason
        {
            get;
            private set;
        }
    }
}
