using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System.Collections.Generic;

namespace FormulaTester
{
    [TestClass]
    public class FormulaTester
    {
        //--------------------------------------------String Formula Tests------------------------------

        //      ****************************Invalid Formulas***************************

        //  This test was written before any code.
        //  Formula with no tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula01()
        {
            SpreadsheetUtilities.Formula testFormula001 = new SpreadsheetUtilities.Formula(" ");
        }

        //  This test was written to help finalize the ValidSyntax method.
        //  Formula with no tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula01a()
        {
            SpreadsheetUtilities.Formula testFormula001a = new SpreadsheetUtilities.Formula("");
        }

        //  This test was written to help finalize the ValidSyntax method.
        //  Formula with no tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula01b()
        {
            SpreadsheetUtilities.Formula testFormula001b = new SpreadsheetUtilities.Formula("                             ");
        }

        //  This test was written before any code.
        //  Unbalanced parenthesizes.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula02()
        {
            SpreadsheetUtilities.Formula testFormula002 = new SpreadsheetUtilities.Formula("((1+3) ");
        }

        //  This test was written before any code.
        //  Unbalanced parenthesizes.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula03()
        {
            SpreadsheetUtilities.Formula testFormula003 = new SpreadsheetUtilities.Formula("((1+3))) ");
        }

        //  This test was written before any code.
        //  Unbalanced parenthesizes.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula04()
        {
            SpreadsheetUtilities.Formula testFormula004 = new SpreadsheetUtilities.Formula("((1+3)) + 7/((3) ");
        }

        //  This test was written before any code.
        //  Unbalanced parenthesizes.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula05()
        {
            SpreadsheetUtilities.Formula testFormula005 = new SpreadsheetUtilities.Formula("((1+3)) + 7/(3 * (5) ");
        }

        //  This test was written before any code.
        //  Unbalanced parenthesizes.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula06()
        {
            SpreadsheetUtilities.Formula testFormula006 = new SpreadsheetUtilities.Formula("((1+3)) + 7/(3 * (5/8)+(2) ");
        }

        //  This test was written before any code.
        //  Unbalanced parenthesizes.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula07()
        {
            SpreadsheetUtilities.Formula testFormula007 = new SpreadsheetUtilities.Formula("(((1+3)) + 7/(3 * (5/8)+(2) ");
        }

        //  This test was written before any code.
        //  Negative integers.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula08()
        {
            SpreadsheetUtilities.Formula testFormula008 = new SpreadsheetUtilities.Formula("(((1+3)) + 7/(-3 * (5/8)+(2) ");
        }

        //  This test was written before any code.
        //  Negative integers.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula08a()
        {
            SpreadsheetUtilities.Formula testFormula008a = new SpreadsheetUtilities.Formula("(((1+3)) + 7/(3 * (-5/8)+(2) ");
        }

        //  This test was written before any code.
        //  Negative integers.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula09()
        {
            SpreadsheetUtilities.Formula testFormula009 = new SpreadsheetUtilities.Formula("(((-1+3)) + 7/(3 * (5/8)+(2) ");
        }

        //  This test was written before any code.
        //  Negative integers.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula10()
        {
            SpreadsheetUtilities.Formula testFormula010 = new SpreadsheetUtilities.Formula("(((1+-3)) + 7/(3 * (5/8)+(2) ");
        }

        //  This test was written before any code.
        //  Negative integers.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula11()
        {
            SpreadsheetUtilities.Formula testFormula011 = new SpreadsheetUtilities.Formula("(((1+3)) + 7/(3 * -(5/8)+(2) ");
        }

        //  This test was written before any code.
        //  Negative integers.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula12()
        {
            SpreadsheetUtilities.Formula testFormula012 = new SpreadsheetUtilities.Formula("(((1+3)) + 7/(3 * (-5/8)+(2) ");
        }

        //  This test was written before any code.
        //  Negative integers.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula13()
        {
            SpreadsheetUtilities.Formula testFormula013 = new SpreadsheetUtilities.Formula("(((1+3)) + 7/(3 * (5/-8)+(2) ");
        }

        //  This test was written before any code.
        //  Negative integers.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula14()
        {
            SpreadsheetUtilities.Formula testFormula014 = new SpreadsheetUtilities.Formula("(((1+3)) + 7/(3 * (5/8)+(-2) ");
        }

        //  This test was written before any code.
        //  The first token of an expression must be a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula15()
        {
            SpreadsheetUtilities.Formula testFormula015 = new SpreadsheetUtilities.Formula("+");
        }

        //  This test was written before any code.
        //  The first token of an expression must be a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula16()
        {
            SpreadsheetUtilities.Formula testFormula016 = new SpreadsheetUtilities.Formula("+3");
        }

        //  This test was written before any code.
        //  The first token of an expression must be a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula17()
        {
            SpreadsheetUtilities.Formula testFormula017 = new SpreadsheetUtilities.Formula("-");
        }

        //  This test was written before any code.
        //  The first token of an expression must be a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula18()
        {
            SpreadsheetUtilities.Formula testFormula018 = new SpreadsheetUtilities.Formula("-9");
        }

        //  This test was written before any code.
        //  The first token of an expression must be a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula19()
        {
            SpreadsheetUtilities.Formula testFormula019 = new SpreadsheetUtilities.Formula("*");
        }

        //  This test was written before any code.
        //  The first token of an expression must be a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula20()
        {
            SpreadsheetUtilities.Formula testFormula020 = new SpreadsheetUtilities.Formula("*0");
        }

        //  This test was written before any code.
        //  The first token of an expression must be a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula21()
        {
            SpreadsheetUtilities.Formula testFormula021 = new SpreadsheetUtilities.Formula("/");
        }

        //  This test was written before any code.
        //  The first token of an expression must be a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula22()
        {
            SpreadsheetUtilities.Formula testFormula022 = new SpreadsheetUtilities.Formula("/3");
        }

        //  This test was written before any code.
        //  The first token of an expression must be a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula23()
        {
            SpreadsheetUtilities.Formula testFormula023 = new SpreadsheetUtilities.Formula(")3+2");
        }

        //  This test was written before any code.
        //  The first token of an expression must be a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula24()
        {
            SpreadsheetUtilities.Formula testFormula024 = new SpreadsheetUtilities.Formula(")3+2/-8())+6");
        }

        //  This test was written before any code.
        //  The last token of an expression must be a number, a variable, or a closing parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula25()
        {
            SpreadsheetUtilities.Formula testFormula025 = new SpreadsheetUtilities.Formula("3+");
        }

        //  This test was written before any code.
        //  The last token of an expression must be a number, a variable, or a closing parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula26()
        {
            SpreadsheetUtilities.Formula testFormula026 = new SpreadsheetUtilities.Formula("3-");
        }

        //  This test was written before any code.
        //  The last token of an expression must be a number, a variable, or a closing parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula27()
        {
            SpreadsheetUtilities.Formula testFormula027 = new SpreadsheetUtilities.Formula("3*");
        }

        //  This test was written before any code.
        //  The last token of an expression must be a number, a variable, or a closing parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula28()
        {
            SpreadsheetUtilities.Formula testFormula028 = new SpreadsheetUtilities.Formula("3/");
        }

        //  This test was written before any code.
        //  The last token of an expression must be a number, a variable, or a closing parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula29()
        {
            SpreadsheetUtilities.Formula testFormula029 = new SpreadsheetUtilities.Formula("3-8(");
        }

        //  This test was written before any code.
        //  Any token that immediately follows an opening parenthesis or an operator must be 
        //      either a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula30()
        {   //THIS TEST IS CALLING THE SECOND CONSTRUCTOR FOR SOME REASON.
            SpreadsheetUtilities.Formula testFormula030 = new SpreadsheetUtilities.Formula("3+(*");
        }   //THIS ONE GETS CAUGHT BY BE THE EQUAL PAREN CHECK

        //  This test was written after a little code.
        //  Any token that immediately follows an opening parenthesis or an operator must be 
        //      either a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula30a()
        {
            SpreadsheetUtilities.Formula testFormula030 = new SpreadsheetUtilities.Formula("3+3+");
        }//THIS ONE GET CAUGHT BY THE CHECK FOR OPPERATOR IN LAST POSITION

        //  This test was written after a little code.
        //  Any token that immediately follows an opening parenthesis or an operator must be 
        //      either a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula30b()
        {
            SpreadsheetUtilities.Formula testFormula030 = new SpreadsheetUtilities.Formula("+3+3");
        }

        //  This test was written before any code.
        //  Any token that immediately follows an opening parenthesis or an operator must be 
        //      either a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula31()
        {
            SpreadsheetUtilities.Formula testFormula031 = new SpreadsheetUtilities.Formula("3++3");
        }

        //  This test was written after some code.
        //  Any token that immediately follows an opening parenthesis or an operator must be 
        //      either a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula31a()
        {
            SpreadsheetUtilities.Formula testFormula031a = new SpreadsheetUtilities.Formula("(3+)+3");
        }

        //  This test was written before any code.
        //  Any token that immediately follows an opening parenthesis or an operator must be 
        //      either a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula32()
        {
            SpreadsheetUtilities.Formula testFormula032 = new SpreadsheetUtilities.Formula("3*+3");
        }

        //  This test was written before any code.
        //  Any token that immediately follows an opening parenthesis or an operator must be 
        //      either a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula33()
        {
            SpreadsheetUtilities.Formula testFormula033 = new SpreadsheetUtilities.Formula("3*/3");
        }

        //  This test was written before any code.
        //  Any token that immediately follows an opening parenthesis or an operator must be 
        //      either a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula34()
        {
            SpreadsheetUtilities.Formula testFormula034 = new SpreadsheetUtilities.Formula("3//3");
        }

        //  This test was written before any code.
        //  Any token that immediately follows an opening parenthesis or an operator must be 
        //      either a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula33a()
        {
            SpreadsheetUtilities.Formula testFormula033a = new SpreadsheetUtilities.Formula("3*-3");
        }

        //  This test was written before any code.
        //  Any token that immediately follows a number, a variable, or a closing parenthesis must 
        //  be either an operator or a closing parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula34a()       //FAILS, THIS SHOULD BE THROWING AN EXCEPTION.
        {
            SpreadsheetUtilities.Formula testFormula034a = new SpreadsheetUtilities.Formula("3 3");
        }

        //  This test was written before any code.
        //  Any token that immediately follows a number, a variable, or a closing parenthesis must 
        //  be either an operator or a closing parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula35()
        {
            SpreadsheetUtilities.Formula testFormula035 = new SpreadsheetUtilities.Formula("(3+ 3)*");
        }

        //  This test was written before any code.
        //  Any token that immediately follows a number, a variable, or a closing parenthesis must 
        //  be either an operator or a closing parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula36()
        {
            SpreadsheetUtilities.Formula testFormula036 = new SpreadsheetUtilities.Formula("(3+ 3)-");
        }

        //  This test was written before any code.
        //  Any token that immediately follows a number, a variable, or a closing parenthesis must 
        //  be either an operator or a closing parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula37()
        {
            SpreadsheetUtilities.Formula testFormula037 = new SpreadsheetUtilities.Formula("(3+ 3)+");
        }

        //  This test was written before any code.
        //  Any token that immediately follows a number, a variable, or a closing parenthesis must 
        //  be either an operator or a closing parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula38()
        {
            SpreadsheetUtilities.Formula testFormula038 = new SpreadsheetUtilities.Formula("(3+ 3)/");
        }

        //  This test was written before any code.
        //  Any token that immediately follows a number, a variable, or a closing parenthesis must 
        //  be either an operator or a closing parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidFormula39()
        {
            SpreadsheetUtilities.Formula testFormula039 = new SpreadsheetUtilities.Formula("(3+ 3)8");
        }
        //      **************************End of Invalid Formulas**********************

        //                          %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

        //      ***************************Formulas with invalid tokens****************

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken00()
        {
            SpreadsheetUtilities.Formula testFormula00 = new SpreadsheetUtilities.Formula("(3+ 3)*#");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken01()
        {
            SpreadsheetUtilities.Formula testFormula01 = new SpreadsheetUtilities.Formula("(3+ 3)*=");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken02()
        {
            SpreadsheetUtilities.Formula testFormula02 = new SpreadsheetUtilities.Formula("(3+ 3-!)");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken03()
        {
            SpreadsheetUtilities.Formula testFormula03 = new SpreadsheetUtilities.Formula("(3+ 3+%)");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken04()
        {
            SpreadsheetUtilities.Formula testFormula04 = new SpreadsheetUtilities.Formula("(3+ 3+^)");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken05()
        {
            SpreadsheetUtilities.Formula testFormula05 = new SpreadsheetUtilities.Formula("(3+ 3+$)");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken06()
        {
            SpreadsheetUtilities.Formula testFormula06 = new SpreadsheetUtilities.Formula("(3+ 3+@)");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken07()
        {
            SpreadsheetUtilities.Formula testFormula07 = new SpreadsheetUtilities.Formula("(3+ 3+`)");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken08()
        {
            SpreadsheetUtilities.Formula testFormula08 = new SpreadsheetUtilities.Formula("(3+ 3+~)");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken09()
        {
            SpreadsheetUtilities.Formula testFormula09 = new SpreadsheetUtilities.Formula("(3+ 3+,)");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken10()
        {
            SpreadsheetUtilities.Formula testFormula10 = new SpreadsheetUtilities.Formula("(3+ 3+<)");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken11()
        {
            SpreadsheetUtilities.Formula testFormula11 = new SpreadsheetUtilities.Formula("(3+ 3+.)");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken12()
        {
            SpreadsheetUtilities.Formula testFormula12 = new SpreadsheetUtilities.Formula("(3+ 3+>)");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken13()
        {
            SpreadsheetUtilities.Formula testFormula13 = new SpreadsheetUtilities.Formula("(3+ 3+?)");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken14()
        {
            SpreadsheetUtilities.Formula testFormula14 = new SpreadsheetUtilities.Formula("(3+ 3+;)");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken15()
        {
            SpreadsheetUtilities.Formula testFormula15 = new SpreadsheetUtilities.Formula("(3+ 3+:)");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken16()
        {
            SpreadsheetUtilities.Formula testFormula16 = new SpreadsheetUtilities.Formula("(3+ 3+')");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken17()
        {
            SpreadsheetUtilities.Formula testFormula17 = new SpreadsheetUtilities.Formula("(3+ 3+[)");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken18()
        {
            SpreadsheetUtilities.Formula testFormula18 = new SpreadsheetUtilities.Formula("(3+ 3+{)");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken19()
        {
            SpreadsheetUtilities.Formula testFormula19 = new SpreadsheetUtilities.Formula("(3+ 3+])");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken20()
        {
            SpreadsheetUtilities.Formula testFormula20 = new SpreadsheetUtilities.Formula("(3+ 3+})");
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken21()
        {
            SpreadsheetUtilities.Formula testFormula21 = new SpreadsheetUtilities.Formula("(3+ 3+|)");
        }

        //  This test was written after most code.       
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodInvalidToken22()
        {
            SpreadsheetUtilities.Formula testFormula22 = new SpreadsheetUtilities.Formula(null);
        }

        //      ***********************End of formulas with invalid tokens*****************************

        //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

        //      ************************Valid Formula tests******************************************

        //  This test was written before any code.
        //  Valid formulas, with no variables, test will only fail if exception is thrown by first constructor.
        [TestMethod]
        public void TestMethodValidFormulaFirst00()
        {
            SpreadsheetUtilities.Formula testValidFormulaFirst00 = new SpreadsheetUtilities.Formula("(3+ 3)");
        }

        //  This test was written before any code.
        //  Valid formulas, with no variables, test will only fail if exception is thrown by first constructor.
        [TestMethod]
        public void TestMethodValidFormulaFirst01()
        {
            SpreadsheetUtilities.Formula testValidFormulaFirst01 = new SpreadsheetUtilities.Formula("(0)");
        }

        //  This test was written before any code.
        //  Valid formulas, with no variables, test will only fail if exception is thrown by first constructor.
        [TestMethod]
        public void TestMethodValidFormulaFirst01a()
        {
            SpreadsheetUtilities.Formula testValidFormulaFirst01a = new SpreadsheetUtilities.Formula("1");
        }

        //  This test was written before any code.
        //  Valid formulas, with no variables, test will only fail if exception is thrown by first constructor.
        [TestMethod]
        public void TestMethodValidFormulaFirst02()
        {
            SpreadsheetUtilities.Formula testValidFormulaFirst02 = new SpreadsheetUtilities.Formula("1+(9)");
        }

        //  This test was written before any code.
        //  Valid formulas, with no variables, test will only fail if exception is thrown by first constructor.
        [TestMethod]
        public void TestMethodValidFormulaFirst03()
        {
            SpreadsheetUtilities.Formula testValidFormulaFirst03 = new SpreadsheetUtilities.Formula("1+(9/8)");
        }

        //  This test was written before any code.
        //  Valid formulas, with no variables, test will only fail if exception is thrown by first constructor.
        [TestMethod]
        public void TestMethodValidFormulaFirst04()
        {
            SpreadsheetUtilities.Formula testValidFormulaFirst04 = new SpreadsheetUtilities.Formula("(1+(9/8)) * (10 - 0)");
        }

        //*************************With variables****************************

        //  This test was written before any code.
        //  Valid formulas, variables, test will only fail if exception is thrown by first constructor.
        [TestMethod]
        public void TestMethodValidVariableFormulaFirst00()
        {
            SpreadsheetUtilities.Formula testValidFormulaFirst00 = new SpreadsheetUtilities.Formula("A4 + 1");
        }

        //  This test was written before any code.
        //  Valid formulas, variables, test will only fail if exception is thrown by first constructor.
        [TestMethod]
        public void TestMethodValidVariableFormulaFirst01()
        {
            SpreadsheetUtilities.Formula testValidFormulaFirst01 = new SpreadsheetUtilities.Formula("A4- 1");
        }

        //  This test was written before any code.
        //  Valid formulas, variables, test will only fail if exception is thrown by first constructor.
        [TestMethod]
        public void TestMethodValidVariableFormulaFirst02()
        {
            SpreadsheetUtilities.Formula testValidFormulaFirst02 = new SpreadsheetUtilities.Formula("a1/ 1");
        }

        //  This test was written before any code.
        //  Valid formulas, variables, test will only fail if exception is thrown by first constructor.
        [TestMethod]
        public void TestMethodValidVariableFormulaFirst03()
        {
            SpreadsheetUtilities.Formula testValidFormulaFirst03 = new SpreadsheetUtilities.Formula("a2/ 1");
        }

        //  This test was written before any code.
        //  Valid formulas, variables, test will only fail if exception is thrown by first constructor.
        [TestMethod]
        public void TestMethodValidVariableFormulaFirst04()
        {
            SpreadsheetUtilities.Formula testValidFormulaFirst04 = new SpreadsheetUtilities.Formula("a4 * 1");
        }

        //  ***********************End of Valid Formula Tests***************************************

        //---------------------------------------End of String Formula Tests---------------------


        //-------------------------------Func Formula Tests----------------------------------

        //  This method will be used as the second parameter.
        public String upperCase(String variable)
        {
            return variable.ToUpper();
        }

        //  This method will be used as the third parameter.
        public bool validator(String variable)
        {
            return true;    //Just return true for all tokens for testing purposes.
        }

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //%%%%%%%%%%%% BOTH CONSTRUCTORS WIL EMPLOY THE SAME SYTAX AND VARIABLE CHECKING
        //  HELPER METHODS, SO I WILL NOT EXHASTIVELY TEST THE SECOND CONSTRUCTOR%%%%%%%%%
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        //              ***************************Invalid Formulas**********************************

        //  This test was written before any code.
        //  Unbalanced parenthesizes.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodSecondInvalidFormula00()
        {
            SpreadsheetUtilities.Formula testSecondFormula00 = new SpreadsheetUtilities.Formula("((1+3)) + 7/((3) ", upperCase, validator);
        }

        //  This test was written before any code.
        //  Negative integers.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodSecondInvalidFormula01()
        {
            SpreadsheetUtilities.Formula testSecondFormula01 = new SpreadsheetUtilities.Formula("(((1+-3)) + 7/(3 * (5/8)+(2) ", upperCase, validator);
        }

        //  This test was written before any code.
        //  The first token of an expression must be a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodSecondInvalidFormula02()
        {
            SpreadsheetUtilities.Formula testSecondFormula02 = new SpreadsheetUtilities.Formula(")3+2/-8())+6", upperCase, validator);
        }

        //  This test was written before any code.
        //  Any token that immediately follows an opening parenthesis or an operator must be 
        //      either a number, a variable, or an opening parenthesis.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodSecondInvalidFormula03()
        {
            SpreadsheetUtilities.Formula testFormula03 = new SpreadsheetUtilities.Formula("3*-3", upperCase, validator);
        }

        //  This test was written before any code.
        //  Invalid tokens.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void TestMethodSecondInvalidToken00()
        {
            SpreadsheetUtilities.Formula testSecondInvalidTokenFormula00 = new SpreadsheetUtilities.Formula("(3+ 3+%)", upperCase, validator);
        }

        //          **************************End of Invalid Formulas****************************

        //                                      %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

        //          ***************************Valid formula tests*********************************

        //  This test was written before any code.
        //  Valid formulas, with no variables, test will only fail if exception is thrown by second constructor.
        [TestMethod]
        public void TestMethodSecondValidFormula00()
        {
            SpreadsheetUtilities.Formula testValidFormulaSecond00 = new SpreadsheetUtilities.Formula("(1+(9/8)) * (10 - 0)", upperCase, validator);
        }

        //  This test was written before any code.
        //  Valid formulas, with variables, test will only fail if exception is thrown by second constructor.
        [TestMethod]
        public void TestMethodValidVariableFormulaSecond00()
        {
            SpreadsheetUtilities.Formula testValidFormulaSecond00 = new SpreadsheetUtilities.Formula("a4 * 1", upperCase, validator);
        }

        //          ****************************End of valid formula Tests*************************

        //---------------------------End of Func Formula Tests-----------------------------------------------------------------

        //--------------------------Evaluate() Tests---------------------------------------------------------------------------------

        //  Delegate/Func for testing that returns 0 regardless.
        public double lookUpEvaluateTest00(String variable)
        {
            return 0;
        }

        public double lookUpEvaluateTestArgumentException(String variable)
        {
            throw new ArgumentException();
        }

        public double lookUpEvaluateTest01(String variable)
        {
            if (variable.Equals("A1"))
                return 1;
            if (variable.Equals("B1"))
                return 2;

            return 3;
        }

        //          ************************Tests for invalid formulas*********************************

        //              *********************First Constructor Tests**************************

        //  This test was written before any code.
        //  Division by zero, no variables.
        [TestMethod]
        public void TestMethodEvaluateDivideByZeroFirst00()
        {
            Formula testEvaluateDivideByZeroFirst00 = new Formula("3/0");
            Assert.IsInstanceOfType(testEvaluateDivideByZeroFirst00.Evaluate(s => 0), typeof(FormulaError));
        }

        //  This test was written before any code.
        //  Division by zero, no variables.
        [TestMethod]
        public void TestMethodEvaluateDivideByZeroFirst01()
        {
            Formula testEvaluateDivideByZeroFirst01 = new Formula("(3-6+5*3)/0");
            Assert.IsInstanceOfType(testEvaluateDivideByZeroFirst01.Evaluate(s => 0), typeof(FormulaError));
        }

        //  This test was written before any code.
        //  Variables resulting in ArgumentException.
        [TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        public void TestMethodNoValueVariableFirst00()
        {
            Formula testEvaluateNoValueVariableFirst00 = new Formula("a4");
            testEvaluateNoValueVariableFirst00.Evaluate(lookUpEvaluateTestArgumentException);
        }

        //              ******************End of First constructor tests***********************

        //                                  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

        //              *********************Tests for second constructor********************

        //  This test was written before any code.      
        //  Division by zero, no variables.
        [TestMethod]
        public void TestMethodEvaluateDivideByZeroSecond00()
        {
            Formula testEvaluateDivideByZeroSecond00 = new Formula("3/0", upperCase, validator);
            Assert.IsInstanceOfType(testEvaluateDivideByZeroSecond00.Evaluate(s => 0), typeof(FormulaError));
        }

        //  This test was written before any code.
        //  Division by zero, no variables.
        [TestMethod]
        public void TestMethodEvaluateDivideByZeroSecond01()
        {
            Formula testEvaluateDivideByZeroSecond01 = new Formula("(3-6+5*3)/0", upperCase, validator);
            Assert.IsInstanceOfType(testEvaluateDivideByZeroSecond01.Evaluate(s => 0), typeof(FormulaError));
        }

        //  This test was written before any code.
        //  Variables resulting in ArgumentException.
        [TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        public void TestMethodNoValueVariableSecond00()
        {
            Formula testEvaluateNoValueVariableSecond00 = new Formula("a4", upperCase, validator);
            testEvaluateNoValueVariableSecond00.Evaluate(lookUpEvaluateTestArgumentException);
        }

        //              *******************End of Tests for second Constructors**************

        //          ***********************End of Tests for invalid formulas****************************

        //                              %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

        //          **********************Tests for valid formulas***********************************

        //              *********************First Constructor Tests**************************

        //  This test was written before any code.
        //  Valid formula, no variables.
        [TestMethod]
        public void TestMethodEvaluateValidFormulaFirst00()
        {
            Formula testEvaluateValidFormulaFirst00 = new Formula("0");
            Assert.AreEqual(0.0, testEvaluateValidFormulaFirst00.Evaluate(lookUpEvaluateTest00));
        }

        //  This test was written before any code.
        //  Valid formula, no variables.
        [TestMethod]
        public void TestMethodEvaluateValidFormulaFirst01()
        {
            Formula testEvaluateValidFormulaFirst01 = new Formula("0+1");
            Assert.AreEqual(1.0, testEvaluateValidFormulaFirst01.Evaluate(lookUpEvaluateTest00));
        }

        //  This test was written before any code.
        //  Valid formula, no variables.
        [TestMethod]
        public void TestMethodEvaluateValidFormulaFirst02()
        {
            Formula testEvaluateValidFormulaFirst02 = new Formula("0+(1*3)");
            Assert.AreEqual(3.0, testEvaluateValidFormulaFirst02.Evaluate(lookUpEvaluateTest00));
        }

        //  This test was written before any code.
        //  Valid formula, no variables.
        [TestMethod]
        public void TestMethodEvaluateValidFormulaFirst03()
        {
            Formula testEvaluateValidFormulaFirst03 = new Formula("0+(1*3)/2");
            Assert.AreEqual(1.5, testEvaluateValidFormulaFirst03.Evaluate(lookUpEvaluateTest00));
        }

        //  This test was written before any code.
        //  Valid formula, no variables.
        [TestMethod]
        public void TestMethodEvaluateValidFormulaFirst04()
        {
            Formula testEvaluateValidFormulaFirst04 = new Formula("(0+(1*3)/2)*3");
            Assert.AreEqual(4.5, testEvaluateValidFormulaFirst04.Evaluate(lookUpEvaluateTest00));
        }

        //  This test was written before any code.
        //  Valid formula, no variables.
        [TestMethod]
        public void TestMethodEvaluateValidFormulaFirst05()
        {
            Formula testEvaluateValidFormulaFirst05 = new Formula("(0+(1*3)/2)*3-1");
            Assert.AreEqual(3.5, testEvaluateValidFormulaFirst05.Evaluate(lookUpEvaluateTest00));
        }

        //  This test was written before any code.
        //  Valid formula, no variables.
        [TestMethod]
        public void TestMethodEvaluateValidFormulaFirst06()
        {
            Formula testEvaluateValidFormulaFirst06 = new Formula("(0+(1*3)/2)*3-1/2");
            Assert.AreEqual(4.0, testEvaluateValidFormulaFirst06.Evaluate(lookUpEvaluateTest00));
        }

        //  This test was written before any code.
        //  Valid formula, with variables.
        [TestMethod]
        public void TestMethodEvaluateValidVariableFormulaFirst00()
        {
            Formula testEvaluateValidVariableFormulaFirst00 = new Formula("A1 + B1+ C1");
            Assert.AreEqual(6.0, testEvaluateValidVariableFormulaFirst00.Evaluate(lookUpEvaluateTest01));
        }

        //  This test was written before any code.
        //  Valid formula, with variables.
        [TestMethod]
        public void TestMethodEvaluateValidVariableFormulaFirst01()
        {
            Formula testEvaluateValidVariableFormulaFirst01 = new Formula("D1");
            Assert.AreEqual(3.0, testEvaluateValidVariableFormulaFirst01.Evaluate(lookUpEvaluateTest01));
        }

        //  This test was written before any code.
        //  Valid formula, with variables.
        [TestMethod]
        public void TestMethodEvaluateValidVariableFormulaFirst02()
        {
            Formula testEvaluateValidVariableFormulaFirst02 = new Formula("A1/B1");
            Assert.AreEqual(0.5, testEvaluateValidVariableFormulaFirst02.Evaluate(lookUpEvaluateTest01));
        }

        //  This test was written before any code.
        //  Valid formula, with variables.
        [TestMethod]
        public void TestMethodEvaluateValidVariableFormulaFirst03()
        {
            Formula testEvaluateValidVariableFormulaFirst03 = new Formula("A1/B1 + C1");
            Assert.AreEqual(3.5, testEvaluateValidVariableFormulaFirst03.Evaluate(lookUpEvaluateTest01));
        }

        //  This test was written before any code.
        //  Valid formula, with variables.
        [TestMethod]
        public void TestMethodEvaluateValidVariableFormulaFirst04()
        {
            Formula testEvaluateValidVariableFormulaFirst04 = new Formula("A1/B1 * C1");
            Assert.AreEqual(1.5, testEvaluateValidVariableFormulaFirst04.Evaluate(lookUpEvaluateTest01));
        }

        //  This test was written before any code.
        //  Valid formula, with variables.
        [TestMethod]
        public void TestMethodEvaluateValidVariableFormulaFirst05()
        {
            Formula testEvaluateValidVariableFormulaFirst05 = new Formula("A1/B1 * C1 + 1");
            Assert.AreEqual(2.5, testEvaluateValidVariableFormulaFirst05.Evaluate(lookUpEvaluateTest01));
        }

        //  This test was written before any code.
        //  Valid formula, with variables.
        [TestMethod]
        public void TestMethodEvaluateValidVariableFormulaFirst06()
        {
            Formula testEvaluateValidVariableFormulaFirst06 = new Formula("A1/B1 - C1 + 1");
            Assert.AreEqual(-1.5, testEvaluateValidVariableFormulaFirst06.Evaluate(lookUpEvaluateTest01));
        }

        //  This test was written after most code.
        //  Valid formula, no variables.
        [TestMethod]
        public void TestMethodEvaluateValidVariableFormulaFirst07()
        {
            Formula testEvaluateValidVariableFormulaFirst07 = new Formula("3.000 +  4.000");
            Assert.AreEqual(7.0, testEvaluateValidVariableFormulaFirst07.Evaluate(lookUpEvaluateTest01));
        }

        //  This test was written after most code.
        //  Valid formula, no variables.
        [TestMethod]
        public void TestMethodEvaluateValidVariableFormulaFirst08()
        {
            Formula testEvaluateValidVariableFormulaFirst08 = new Formula("3.0001000 +  4.000");
            Assert.AreEqual(7.0001, testEvaluateValidVariableFormulaFirst08.Evaluate(lookUpEvaluateTest01));
        }

        //  This test was written during the final stages of my PS3.
        //  Valid formula, no variables.
        [TestMethod]
        public void TestMethodEvaluateValidVariableFormulaFirst09()
        {
            Formula testEvaluateValidVariableFormulaFirst09 = new Formula("3.0001000");
            Assert.AreEqual(3.0001, testEvaluateValidVariableFormulaFirst09.Evaluate(lookUpEvaluateTest01));
        }

        //  This test was written during the final stages of my PS3.
        //  Valid formula, with variables.
        [TestMethod]
        public void TestMethodEvaluateValidVariableFormulaFirst10()
        {
            Formula testEvaluateValidVariableFormulaFirst10 = new Formula("3.0001000 + x1", upperCase, validator);
            Assert.AreEqual(6.0001, testEvaluateValidVariableFormulaFirst10.Evaluate(lookUpEvaluateTest01));
        }

        //  This test was written during the final stages of my PS3.
        //  Valid formula, with variables.
        [TestMethod]
        public void TestMethodEvaluateValidVariableFormulaFirst11()
        {
            Formula testEvaluateValidVariableFormulaFirst11 = new Formula("3.0001000 + x1", upperCase, validator);
            Assert.AreEqual(6.00010000000, testEvaluateValidVariableFormulaFirst11.Evaluate(lookUpEvaluateTest01));
        }

        //              ******************End of First constructor tests***********************

        //                                  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

        //              *********************Tests for second constructor********************

        //  This test was written before any code.
        //  Valid formula, no variables.
        [TestMethod]
        public void TestMethodEvaluateValidFormulaSecond00()
        {
            Formula testEvaluateValidFormulaSecond00 = new Formula("(0+(1*3)/2)*3-1/2", upperCase, validator);
            Assert.AreEqual(4.0, testEvaluateValidFormulaSecond00.Evaluate(lookUpEvaluateTest00));
        }

        //  This test was written before any code.
        //  Valid formula, with variables.
        [TestMethod]
        public void TestMethodEvaluateValidVariableFormulaSecond01()
        {
            Formula testEvaluateValidVariableFormulaSecond01 = new Formula("a1/B1 - C1 + 1", upperCase, validator);
            Assert.AreEqual(-1.5, testEvaluateValidVariableFormulaSecond01.Evaluate(lookUpEvaluateTest01));
        }
        //              *******************End of Tests for second Constructors**************

        //          *********************End of tests for valid formulas*****************************

        //-------------------------------End of Evaluate() Tests-------------------------------------------------------------------

        //------------------------------GetVariables tests-----------------------------------------------------------------------

        //          **********************First constructor*************************

        //  This test was written before any code.
        //  Formula with no variables.
        [TestMethod]
        public void TestMethodGetVariablesFirst00()
        {
            Formula testGetVariablesFirst00 = new Formula("1 + 1");
            //  I would like to take this opportunity to state how it pisses me off that you can't call contains(),
            //      or Count on an IEnumerable.
            IEnumerable<String> GetVariablesEnum00 = testGetVariablesFirst00.GetVariables();

            List<String> GetEnumStrings00 = new List<String>();

            foreach (String variable in GetVariablesEnum00)
                GetEnumStrings00.Add(variable);

            Assert.AreEqual(0, GetEnumStrings00.Count);
        }

        //  This test was written before any code.
        //  Formula with no variables. No Nulls!
        [TestMethod]
        public void TestMethodGetVariablesFirst00a()
        {
            Formula testGetVariablesFirst00 = new Formula("1 + 1");
            //  I would like to take this opportunity to state how it pisses me off that you can't call contains(),
            //      or count on an IEnumerable.
            IEnumerable<String> GetVariablesEnum00 = testGetVariablesFirst00.GetVariables();

            List<String> GetEnumStrings00 = new List<String>();

            foreach (String variable in GetVariablesEnum00)
                GetEnumStrings00.Add(variable);

            Assert.AreEqual(0, GetEnumStrings00.Count);
        }

        //  This test was written before any code.
        //  Formula with a variety of variables.
        [TestMethod]
        public void TestMethodGetVariablesFirst01()
        {
            Formula testGetVariablesFirst01 = new Formula("A1 + a1 / ab1 * Ab1   + a43 + 44 + AAAA333");
            //  I would like to take this opportunity to state how it pisses me off that you can't call contains(),
            //      or count on an IEnumerable.
            IEnumerable<String> GetVariablesEnum01 = testGetVariablesFirst01.GetVariables();

            List<String> GetEnumStrings01 = new List<String>();

            foreach (String variable in GetVariablesEnum01)
                GetEnumStrings01.Add(variable);

            Assert.AreEqual(6, GetEnumStrings01.Count);

            Assert.IsTrue(GetEnumStrings01.Contains("A1"));
            Assert.IsTrue(GetEnumStrings01.Contains("a1"));
            Assert.IsTrue(GetEnumStrings01.Contains("ab1"));
            Assert.IsTrue(GetEnumStrings01.Contains("Ab1"));
            Assert.IsTrue(GetEnumStrings01.Contains("a43"));
            Assert.IsTrue(GetEnumStrings01.Contains("AAAA333"));
        }

        //          ********************End of first constructor tests****************

        //                                      %%%%%%%%%%%%%%%%%%%%%%%%%%

        //          *******************Second Constructor Tests********************

        //  This test was written before any code.
        //  Formula with no variables. No Nulls!
        [TestMethod]
        public void TestMethodGetVariablesSecond00()
        {
            Formula testGetVariablesSecond00 = new Formula("1 + 1", upperCase, validator);

            IEnumerable<String> GetVariablesEnum00 = testGetVariablesSecond00.GetVariables();

            List<String> GetEnumStrings00 = new List<String>();

            foreach (String variable in GetVariablesEnum00)
                GetEnumStrings00.Add(variable);

            Assert.AreEqual(0, GetEnumStrings00.Count);
        }

        //  This test was written before any code.
        //  Formula with a variety of variables.
        [TestMethod]
        public void TestMethodGetVariableSecond01()
        {
            Formula testGetVariablesSecond01 = new Formula("A1 + a1 / ab1 * Ab1  + a43 + 44 + AAAA333 ", upperCase, validator);

            IEnumerable<String> GetVariablesEnum01 = testGetVariablesSecond01.GetVariables();

            List<String> GetEnumStrings01 = new List<String>();

            foreach (String variable in GetVariablesEnum01)
                GetEnumStrings01.Add(variable);

            Assert.AreEqual(4, GetEnumStrings01.Count);

            Assert.IsTrue(GetEnumStrings01.Contains("A1"));
            Assert.IsTrue(GetEnumStrings01.Contains("AB1"));
            Assert.IsTrue(GetEnumStrings01.Contains("A43"));
            Assert.IsTrue(GetEnumStrings01.Contains("AAAA333"));
        }

        //          *****************End of Second Constructor Tests***************

        //--------------------------------End of GetVariables Tests-----------------------------------------------------------

        //-----------------------------------ToString() Tests----------------------------------------------------------------------

        //          *************************First Constructor Tests******************

        //  This test was written before any code.
        //  Simple formula.
        [TestMethod]
        public void TestMethodToStringFirst00()
        {
            Formula testToStringFirst00 = new Formula("1 ");
            Assert.IsTrue(testToStringFirst00.ToString().Equals("1"));
        }

        //  This test was written before any code.
        //  Simple formula.
        [TestMethod]
        public void TestMethodToStringFirst01()
        {
            Formula testToStringFirst01 = new Formula("1 + 3 * 4 / 5 ");
            Assert.IsTrue(testToStringFirst01.ToString().Equals("1+3*4/5"));
        }

        //  This test was written before any code.
        //  Simple formula, with variables.
        [TestMethod]
        public void TestMethodToStringFirst02()
        {
            Formula testToStringFirst02 = new Formula("1 + 3 * 4 / 5 +a2 -   ba3 ");
            Assert.IsTrue(testToStringFirst02.ToString().Equals("1+3*4/5+a2-ba3"));
        }

        //          **********************End of First Constructor Tests**************

        //                                      %%%%%%%%%%%%%%%%%%%%%%%%%

        //          ***********************Second Constructor Tests*****************

        //  This test was written before any code.
        //  Simple formula, with variables.
        [TestMethod]
        public void TestMethodToStringSecond00()
        {
            Formula testToStringSecond00 = new Formula("1 + 3 * 4 / 5 + a2 -   ba3 ", upperCase, validator);
            Assert.IsTrue(testToStringSecond00.ToString().Equals("1+3*4/5+A2-BA3"));
        }

        //          ********************End of Second Constructor Tests*************

        //------------------------------End of ToString() Tests------------------------------------------------------------------

        //---------------------------------Equals Tests----------------------------------------------------------------------------

        //          *****************************First constructor Tests***************

        //  This test was written before any code.
        //  False Condition for non-Formula parameter.
        [TestMethod]
        public void TestMethodEqualsFirst00()
        {
            Formula TestEqualsFormulaFirst00 = new Formula("0");
            Assert.IsFalse(TestEqualsFormulaFirst00.Equals("0"));
        }

        //  This test was written before any code.
        //  False Condition for null parameter.
        [TestMethod]
        public void TestMethodEqualsFirst01()
        {
            Formula TestEqualsFormulaFirst01 = new Formula("0");
            Assert.IsFalse(TestEqualsFormulaFirst01.Equals(null));
        }

        //  This test was written before any code.
        //  False Condition for non-Formula parameter.
        [TestMethod]
        public void TestMethodEqualsFirst02()
        {
            Formula TestEqualsFormulaFirst02 = new Formula("0");
            Assert.IsFalse(TestEqualsFormulaFirst02.Equals(0));
        }

        //  This test was written before any code.
        //  Regular comparison. True.
        [TestMethod]
        public void TestMethodEqualsFirst03()
        {
            Formula TestEqualsFormulaFirst03 = new Formula("0    + a4 *7-a3  /(5    -   t4)");
            Formula TestEqualsFormulaFirst03a = new Formula("0  +  a4 *  7-a3 /(  5-   t4)   ");

            Assert.IsTrue(TestEqualsFormulaFirst03.Equals(TestEqualsFormulaFirst03a));
        }

        //  This test was written after most code.
        //  Regular comparison. False.
        [TestMethod]
        public void TestMethodEqualsFirst03a()
        {
            Formula TestEqualsFormulaFirst03a = new Formula("0  + a1");
            Formula TestEqualsFormulaFirst03aa = new Formula("1+ a1 ");
            Assert.IsFalse(TestEqualsFormulaFirst03a.Equals(TestEqualsFormulaFirst03aa));
        }

        //  This test was written after most code.
        //  Regular comparison. False.
        [TestMethod]
        public void TestMethodEqualsFirst03b()
        {
            Formula TestEqualsFormulaFirst03b = new Formula("0  + a1");
            Formula TestEqualsFormulaFirst03bb = new Formula("0+ b1 ");
            Assert.IsFalse(TestEqualsFormulaFirst03b.Equals(TestEqualsFormulaFirst03bb));
        }

        //  This test was written after most code.
        //  Regular comparison. False.
        [TestMethod]
        public void TestMethodEqualsFirst03c()
        {
            Formula TestEqualsFormulaFirst03c = new Formula("0  + a1 /  65");
            Formula TestEqualsFormulaFirst03cc = new Formula("0+ a1 / 45");
            Assert.IsFalse(TestEqualsFormulaFirst03c.Equals(TestEqualsFormulaFirst03cc));
        }

        //  This test was written before any code.
        //  Regular comparison. False.
        [TestMethod]
        public void TestMethodEqualsFirst04()
        {
            Formula TestEqualsFormulaFirst04 = new Formula("0    + a4 *7-a3  /(5    -   t4)");
            Formula TestEqualsFormulaFirst04a = new Formula("0  +  a4 *  7-a3 /(  5-   t47)   ");
            Assert.IsFalse(TestEqualsFormulaFirst04.Equals(TestEqualsFormulaFirst04a));
        }

        //          **************************End of First Constructor Tests***********

        //                              %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

        //          ***********************Second Constructor Tests******************

        //  This test was written before any code.
        //  False Condition for non-Formula parameter.
        [TestMethod]
        public void TestMethodEqualsSecond00()
        {
            Formula TestEqualsFormulaSecond00 = new Formula("0", upperCase, validator);
            Assert.IsFalse(TestEqualsFormulaSecond00.Equals("0"));
        }

        //  This test was written before any code.
        //  False Condition for null parameter.
        [TestMethod]
        public void TestMethodEqualsSecondt01()
        {
            Formula TestEqualsFormulaSecond01 = new Formula("0", upperCase, validator);
            Assert.IsFalse(TestEqualsFormulaSecond01.Equals(null));
        }

        //  This test was written before any code.
        //  False Condition for non-Formula parameter.
        [TestMethod]
        public void TestMethodEqualsSecond02()
        {
            Formula TestEqualsFormulaSecond02 = new Formula("0", upperCase, validator);
            Assert.IsFalse(TestEqualsFormulaSecond02.Equals(0));
        }

        //  This test was written before any code.
        //  Regular comparison. True.
        [TestMethod]
        public void TestMethodEqualsSecond03()
        {
            Formula TestEqualsFormulaSecond03 = new Formula("0    + a4 *7-A3  /(5    -   t4)", upperCase, validator);
            Formula TestEqualsFormulaSecond03a = new Formula("0  +  A4 *  7-a3 /(  5-   t4)   ", upperCase, validator);
            Assert.IsTrue(TestEqualsFormulaSecond03.Equals(TestEqualsFormulaSecond03a));
        }

        //  This test was written before any code.
        //  Regular comparison. False.
        [TestMethod]
        public void TestMethodEqualsSecond04()
        {
            Formula TestEqualsFormulaSecond04 = new Formula("0    + a4 *7-A3  /(5    -   t4)", upperCase, validator);
            Formula TestEqualsFormulaSecond04a = new Formula("0  +  A4 *  7-a3 /(  5-   t44)   ", upperCase, validator);
            Assert.IsFalse(TestEqualsFormulaSecond04.Equals(TestEqualsFormulaSecond04a));
        }
        //          *********************End Of Second Constructor Tests*************

        //------------------------------End of Equals Tests----------------------------------------------------------------------

        //----------------------------------- == Tests-------------------------------------------------------------------------------

        //          *****************************First constructor Tests***************

        //  This test was written before any code.
        //  Simple Comparison True.
        [TestMethod]
        public void TestMethodEqualsOperatorFirst00()
        {
            Formula TestEqualsFormulaOperatorFirst00 = new Formula("0");
            Formula TestEqualsFormulaOperatorFirst00a = new Formula("  0");
            Assert.IsTrue(TestEqualsFormulaOperatorFirst00 == TestEqualsFormulaOperatorFirst00a);
        }

        //  This test was written before any code.
        //  False Condition for null comparison.
        [TestMethod]
        public void TestMethodEqualsOperatorFirst01()
        {
            Formula TestEqualsFormulaOperatorFirst01 = new Formula("0");
            Formula TestEqualsFormulaOperatorFirst01a = null;
            Assert.IsFalse(TestEqualsFormulaOperatorFirst01 == TestEqualsFormulaOperatorFirst01a);
        }

        //  This test was written before any code.
        //  False Condition for null comparison.
        [TestMethod]
        public void TestMethodEqualsOperatorFirst01a()
        {
            Formula TestEqualsFormulaOperatorFirst01a = null;
            Formula TestEqualsFormulaOperatorFirst01aa = new Formula("0");
            Assert.IsFalse(TestEqualsFormulaOperatorFirst01a == TestEqualsFormulaOperatorFirst01aa);
        }

        //  This test was written before any code.
        //  True Condition for null comparison.
        [TestMethod]
        public void TestMethodEqualsOperatorFirst02()
        {
            Formula TestEqualsFormulaOperatorFirst02 = null;
            Formula TestEqualsFormulaOperatorFirst02a = null;
            Assert.IsTrue(TestEqualsFormulaOperatorFirst02 == TestEqualsFormulaOperatorFirst02a);
        }

        //  This test was written before any code.
        //  Regular comparison. True.
        [TestMethod]
        public void TestMethodEqualsOperatorFirst03()
        {
            Formula TestEqualsOperatorFormulaFirst03 = new Formula("0    + a4 *7-a3  /(5    -   t4)");
            Formula TestEqualsOperatorFormulaFirst03a = new Formula("0  +  a4 *  7-a3 /(  5-   t4)   ");
            Assert.IsTrue(TestEqualsOperatorFormulaFirst03 == TestEqualsOperatorFormulaFirst03a);
        }

        //  This test was written before any code.
        //  Regular comparison. False.
        [TestMethod]
        public void TestMethodEqualsOperatorFirst04()
        {
            Formula TestEqualsOperatorFormulaFirst04 = new Formula("0    + a4 *7-a3  /(5    -   t4)");
            Formula TestEqualsOperatorFormulaFirst04a = new Formula("0  +  a4 *  7-a3 /(  5-   t47)   ");
            Assert.IsFalse(TestEqualsOperatorFormulaFirst04 == TestEqualsOperatorFormulaFirst04a);
        }

        //          **************************End of First Constructor Tests***********

        //                              %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

        //          ***********************Second Constructor Tests******************

        //  This test was written before any code.
        //  True Condition for null Comparison.
        [TestMethod]
        public void TestMethodEqualsOperatorSecond00()
        {
            Formula TestEqualsOperatorFormulaSecond00 = null;
            Formula TestEqualsOperatorFormulaSecond00a = null;
            Assert.IsTrue(TestEqualsOperatorFormulaSecond00 == TestEqualsOperatorFormulaSecond00a);
        }

        //  This test was written before any code.
        //  False Condition for null parameter.
        [TestMethod]
        public void TestMethodEqualsOperatorSecondt01()
        {
            Formula TestEqualsOperatorFormulaSecond01 = new Formula("0", upperCase, validator);
            Formula TestEqualsOperatorFormulaSecond01a = null;
            Assert.IsFalse(TestEqualsOperatorFormulaSecond01 == TestEqualsOperatorFormulaSecond01a);
        }

        //  This test was written before any code.
        //  False Condition for null parameter.
        [TestMethod]
        public void TestMethodEqualsOperatorSecondt01a()
        {
            Formula TestEqualsOperatorFormulaSecond01a = new Formula("0", upperCase, validator);
            Assert.IsFalse(TestEqualsOperatorFormulaSecond01a == null);
        }

        //  This test was written before any code.
        //  False Condition for null comparison.
        [TestMethod]
        public void TestMethodEqualsOperatorSecond02()
        {
            Formula TestEqualsOperatorFormulaSecond02 = null;
            Formula TestEqualsOperatorFormulaSecond02a = new Formula("0", upperCase, validator);
            Assert.IsFalse(TestEqualsOperatorFormulaSecond02 == TestEqualsOperatorFormulaSecond02a);
        }

        //  This test was written before any code.
        //  Regular comparison. True.
        [TestMethod]
        public void TestMethodEqualsOperatorSecond03()
        {
            Formula TestEqualsOperatorFormulaSecond03 = new Formula("0    + a4 *7-A3  /(5    -   t4)", upperCase, validator);
            Formula TestEqualsOperatorFormulaSecond03a = new Formula("0  +  A4 *  7-a3 /(  5-   t4)   ", upperCase, validator);
            Assert.IsTrue(TestEqualsOperatorFormulaSecond03 == TestEqualsOperatorFormulaSecond03a);
        }

        //  This test was written before any code.
        //  Regular comparison. False.
        [TestMethod]
        public void TestMethodOperatorEqualsSecond04()
        {
            Formula TestEqualsOperatorFormulaSecond04 = new Formula("0    + a4 *7-A3  /(5    -   t4)", upperCase, validator);
            Formula TestEqualsOperatorFormulaSecond04a = new Formula("0  +  A4 *  7-a3 /(  5-   t44)   ", upperCase, validator);
            Assert.IsFalse(TestEqualsOperatorFormulaSecond04 == TestEqualsOperatorFormulaSecond04a);
        }
        //          ********************End of Second Constructor Tests*******************************

        //---------------------------------End of == Tests-------------------------------------------------------------------------

        //------------------------------ != Tests-------------------------------------------------------------------------------------

        //          *****************************First constructor Tests*************************

        //  This test was written before any code.
        //  Simple Comparison False.
        [TestMethod]
        public void TestMethodNotEqualsOperatorFirst00()
        {
            Formula TestNotEqualsFormulaOperatorFirst00 = new Formula("0");
            Formula TestNotEqualsFormulaOperatorFirst00a = new Formula("  0");
            Assert.IsFalse(TestNotEqualsFormulaOperatorFirst00 != TestNotEqualsFormulaOperatorFirst00a);
        }

        //  This test was written before any code.
        //  True Condition for null comparison.
        [TestMethod]
        public void TestMethodNotEqualsOperatorFirst01()
        {
            Formula TestNotEqualsFormulaOperatorFirst01 = new Formula("0");
            Formula TestNotEqualsFormulaOperatorFirst01a = null;
            Assert.IsTrue(TestNotEqualsFormulaOperatorFirst01 != TestNotEqualsFormulaOperatorFirst01a);
        }

        //  This test was written before any code.
        //  False Condition for null comparison.
        [TestMethod]
        public void TestMethodNotEqualsOperatorFirst01a()
        {
            Formula TestNotEqualsFormulaOperatorFirst01a = null;
            Formula TestNotEqualsFormulaOperatorFirst01aa = new Formula("0");
            Assert.IsTrue(TestNotEqualsFormulaOperatorFirst01a != TestNotEqualsFormulaOperatorFirst01aa);
        }

        //  This test was written before any code.
        //  False Condition for null comparison.
        [TestMethod]
        public void TestMethodNotEqualsOperatorFirst02()
        {
            Formula TestNotEqualsFormulaOperatorFirst02 = null;
            Formula TestNotEqualsFormulaOperatorFirst02a = null;
            Assert.IsFalse(TestNotEqualsFormulaOperatorFirst02 != TestNotEqualsFormulaOperatorFirst02a);
        }

        //  This test was written before any code.
        //  Regular comparison. False.
        [TestMethod]
        public void TestMethodNotEqualsOperatorFirst03()
        {
            Formula TestNotEqualsOperatorFormulaFirst03 = new Formula("0    + a4 *7-a3  /(5    -   t4)");
            Formula TestNotEqualsOperatorFormulaFirst03a = new Formula("0  +  a4 *  7-a3 /(  5-   t4)   ");
            Assert.IsFalse(TestNotEqualsOperatorFormulaFirst03 != TestNotEqualsOperatorFormulaFirst03a);
        }

        //  This test was written before any code.
        //  Regular comparison. True.
        [TestMethod]
        public void TestMethodNotEqualsOperatorFirst04()
        {
            Formula TestNotEqualsOperatorFormulaFirst04 = new Formula("0    + a4 *7-a3  /(5    -   t4)");
            Formula TestNotEqualsOperatorFormulaFirst04a = new Formula("0  +  a4 *  7-a3 /(  5-   t47)   ");
            Assert.IsTrue(TestNotEqualsOperatorFormulaFirst04 != TestNotEqualsOperatorFormulaFirst04a);
        }

        //          **************************End of First Constructor Tests***********

        //                              %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

        //          ***********************Second Constructor Tests******************

        //  This test was written before any code.
        //  False Condition for null Comparison.
        [TestMethod]
        public void TestMethodNotEqualsOperatorSecond00()
        {
            Formula TestNotEqualsOperatorFormulaSecond00 = null;
            Formula TestNotEqualsOperatorFormulaSecond00a = null;
            Assert.IsFalse(TestNotEqualsOperatorFormulaSecond00 != TestNotEqualsOperatorFormulaSecond00a);
        }

        //  This test was written before any code.
        //  True Condition for null parameter.
        [TestMethod]
        public void TestMethodNotEqualsOperatorSecondt01()
        {
            Formula TestNotEqualsOperatorFormulaSecond01 = new Formula("0", upperCase, validator);
            Formula TestNotEqualsOperatorFormulaSecond01a = null;
            Assert.IsTrue(TestNotEqualsOperatorFormulaSecond01 != TestNotEqualsOperatorFormulaSecond01a);
        }

        //  This test was written before any code.
        //  True Condition for null parameter.
        [TestMethod]
        public void TestMethodNotEqualsOperatorSecondt01a()
        {
            Formula TestNotEqualsOperatorFormulaSecond01a = new Formula("0", upperCase, validator);
            Assert.IsTrue(TestNotEqualsOperatorFormulaSecond01a != null);
        }

        //  This test was written before any code.
        //  True Condition for null comparison.
        [TestMethod]
        public void TestMethodNotEqualsOperatorSecond02()
        {
            Formula TestNotEqualsOperatorFormulaSecond02 = null;
            Formula TestNotEqualsOperatorFormulaSecond02a = new Formula("0", upperCase, validator);
            Assert.IsTrue(TestNotEqualsOperatorFormulaSecond02 != TestNotEqualsOperatorFormulaSecond02a);
        }

        //  This test was written before any code.
        //  Regular comparison. False.
        [TestMethod]
        public void TestMethodNotEqualsOperatorSecond03()
        {
            Formula TestNotEqualsOperatorFormulaSecond03 = new Formula("0    + a4 *7-A3  /(5    -   t4)", upperCase, validator);
            Formula TestNotEqualsOperatorFormulaSecond03a = new Formula("0  +  A4 *  7-a3 /(  5-   t4)   ", upperCase, validator);
            Assert.IsFalse(TestNotEqualsOperatorFormulaSecond03 != TestNotEqualsOperatorFormulaSecond03a);
        }

        //  This test was written before any code.
        //  Regular comparison. True.
        [TestMethod]
        public void TestMethodOperatorNotEqualsSecond04()
        {
            Formula TestNotEqualsOperatorFormulaSecond04 = new Formula("0    + a4 *7-A3  /(5    -   t4)", upperCase, validator);
            Formula TestNotEqualsOperatorFormulaSecond04a = new Formula("0  +  A4 *  7-a3 /(  5-   t44)   ", upperCase, validator);
            Assert.IsTrue(TestNotEqualsOperatorFormulaSecond04 != TestNotEqualsOperatorFormulaSecond04a);
        }

        //          *******************End of Second Constructor Tests***********************************

        //------------------------------End of != Tests-----------------------------------------------------------------------------


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //%%%%%%%%%%%%% I'm going to start coding now %%%%%%%%%%%%%%%%%%%%%%
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        //------------------------------Get HashCode Tests---------------------------------------------------------------------
        //Get HashCode
        //  This test was written after most code.
        [TestMethod]
        public void TestGetHash00()
        {
            Formula TestGetHash00 = new Formula("1 + 1");
            Formula TestGetHash00a = new Formula("1 + 1");
            Assert.AreEqual(TestGetHash00.GetHashCode(), TestGetHash00a.GetHashCode());
        }

        //Get HashCode
        //  This test was written after most code.
        [TestMethod]
        public void TestGetHash01()
        {
            Formula TestGetHash01 = new Formula("1 +           1");
            Formula TestGetHash01a = new Formula("1 + 1");
            Assert.AreEqual(TestGetHash01.GetHashCode(), TestGetHash01a.GetHashCode());
        }

        //Get HashCode
        //  This test was written after most code.
        [TestMethod]
        public void TestGetHash02()
        {
            Formula TestGetHash02 = new Formula("1 + 2");
            Formula TestGetHash02a = new Formula("1 + 1");
            Assert.AreNotEqual(TestGetHash02.GetHashCode(), TestGetHash02a.GetHashCode());
        }

        //Get HashCode
        //  This test was written after most code.
        [TestMethod]
        public void TestGetHash03()
        {
            Formula TestGetHash03 = new Formula("1 +   ab1 * gh67/        1");
            Formula TestGetHash03a = new Formula("1 +ab1       * gh67/  1");
            Assert.AreEqual(TestGetHash03.GetHashCode(), TestGetHash03a.GetHashCode());
        }

        //Get HashCode
        //  This test was written after most code.
        [TestMethod]
        public void TestGetHash04()
        {
            Formula TestGetHash04 = new Formula("1 +   ab1 * gh67/    8-    1");
            Formula TestGetHash04a = new Formula("1 +ab1     * gh67/  1");
            Assert.AreNotEqual(TestGetHash04.GetHashCode(), TestGetHash04a.GetHashCode());
        }

        //Get HashCode
        //  This test was written in the final stages of PS3.
        [TestMethod]
        public void TestGetHash05()
        {
            Formula TestGetHash05 = new Formula("0");
            Formula TestGetHash05a = new Formula("1");
            Assert.AreNotEqual(TestGetHash05.GetHashCode(), TestGetHash05a.GetHashCode());
        }

        //Get HashCode
        //  This test was written in the final stages of PS3.
        [TestMethod]
        public void TestGetHash06()
        {
            Formula TestGetHash06 = new Formula("0*0");
            Formula TestGetHash06a = new Formula("1");
            Assert.AreNotEqual(TestGetHash06.GetHashCode(), TestGetHash06a.GetHashCode());
        }

        //Get HashCode
        //  This test was written in the final stages of PS3.
        [TestMethod]
        public void TestGetHash07()
        {
            Formula TestGetHash07 = new Formula("0000000000000000000000000");
            Formula TestGetHash07a = new Formula("0000000000000000000000");
            Assert.AreNotEqual(TestGetHash07.GetHashCode(), TestGetHash07a.GetHashCode());
        }

        //Get HashCode
        //  This test was written in the final stages of PS3.
        [TestMethod]
        public void TestGetHash08()
        {
            Formula TestGetHash08 = new Formula(".0000000000000000000000000");
            Formula TestGetHash08a = new Formula("0000000000000000000000");
            Assert.AreNotEqual(TestGetHash08.GetHashCode(), TestGetHash08a.GetHashCode());
        }

        //Get HashCode
        //  This test was written in the final stages of PS3.
        [TestMethod]
        public void TestGetHash09()
        {
            Formula TestGetHash09 = new Formula(".000");
            Formula TestGetHash09a = new Formula(".0000");
            Assert.AreNotEqual(TestGetHash09.GetHashCode(), TestGetHash09a.GetHashCode());
        }

        //Get HashCode
        //  This test was written in the final stages of PS3.
        [TestMethod]
        public void TestGetHash10()
        {
            Formula TestGetHash10 = new Formula("0.000");
            Formula TestGetHash10a = new Formula(".0000");
            Assert.AreNotEqual(TestGetHash10.GetHashCode(), TestGetHash10a.GetHashCode());
        }

        //Get HashCode
        //  This test was written in the final stages of PS3.
        [TestMethod]
        public void TestGetHash11()
        {
            Formula TestGetHash11 = new Formula("aaa1");
            Formula TestGetHash11a = new Formula("aaaa1");
            Assert.AreNotEqual(TestGetHash11.GetHashCode(), TestGetHash11a.GetHashCode());
        }

        //Get HashCode
        //  This test was written in the final stages of PS3.
        [TestMethod]
        public void TestGetHash12()
        {
            Formula TestGetHash12 = new Formula("aaa1");
            Formula TestGetHash12a = new Formula("AAA1");
            Assert.AreNotEqual(TestGetHash12.GetHashCode(), TestGetHash12a.GetHashCode());
        }

        //Get HashCode
        //  This test was written in the final stages of PS3.
        [TestMethod]
        public void TestGetHash13()
        {
            Formula TestGetHash13 = new Formula("aaa1", upperCase, validator);
            Formula TestGetHash13a = new Formula("AAA1", upperCase, validator);
            Assert.AreEqual(TestGetHash13.GetHashCode(), TestGetHash13a.GetHashCode());
        }
        //----------------------------End of GetHashCode Tests---------------------------------------------------------------




        //-------------------------------All Purpose Tests-----------------------------------------------------------------------

        //Buh Bye!
        [TestMethod]
        public void TestAllPurpose00()
        {
            Formula allPurposeTest00 = new Formula("1   +A1  *2-  0");
            Formula allPurposeTest00a = new Formula("1+A1  *2- 0");
            //string aptString = 
            Assert.AreEqual(1.0, allPurposeTest00.Evaluate(lookUpEvaluateTest00));

            List<String> AllVariables00 = (List<String>)allPurposeTest00.GetVariables();

            Assert.IsTrue(AllVariables00.Contains("A1"));
            Assert.AreEqual("1+A1*2-0", allPurposeTest00.ToString());
            Assert.IsTrue(allPurposeTest00.Equals(allPurposeTest00a));
            Assert.IsTrue(allPurposeTest00 == allPurposeTest00a);
            Assert.IsFalse(allPurposeTest00 != allPurposeTest00a);
            Assert.AreEqual(allPurposeTest00.GetHashCode(), allPurposeTest00a.GetHashCode());
        }

        //------------------------------End of All Purpose Tests--------------------------------------------------------


        //------------------------------ Provided Tests -------------------------------------------------------------------

        [TestMethod()]
        public void Test17()
        {
            Formula f = new Formula("5/0");  // 0.0 is getting returned here, we will assume it is the Formual class's fault.
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
        }

        [TestMethod()]
        public void Test18()
        {
            Formula f = new Formula("(5 + X1) / (X1 - 3)");  //come back!!
            Assert.IsInstanceOfType(f.Evaluate(s => 3), typeof(FormulaError));
        }

        // Tests of syntax errors detected by the constructor
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test19()
        {
            Formula f = new Formula("+");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test20()
        {
            Formula f = new Formula("2+5+");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test21()
        {
            Formula f = new Formula("2+5*7)");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test22()
        {
            Formula f = new Formula("((3+5*7)");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test23()
        {
            Formula f = new Formula("xx");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test24()
        {
            Formula f = new Formula("5+xx");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test25()
        {
            Formula f = new Formula("5+7+(5)8");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test26()
        {
            Formula f = new Formula("5 5");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test27()
        {
            Formula f = new Formula("5 + + 3");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test28()
        {
            Formula f = new Formula("");
        }

        // Some more complicated formula evaluations
        [TestMethod()]
        public void Test29()
        {
            Formula f = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");   //ps5 variable deffinition?
            Assert.AreEqual(5.14285714285714, (double)f.Evaluate(s => (s == "x7") ? 1 : 4), 1e-9);
        }

        [TestMethod()]
        public void Test30()
        {
            Formula f = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
            Assert.AreEqual(6, (double)f.Evaluate(s => 1), 1e-9);
        }

        [TestMethod()]
        public void Test31()
        {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            Assert.AreEqual(12, (double)f.Evaluate(s => 2), 1e-9);
        }

        [TestMethod()]
        public void Test32()
        {
            Formula f = new Formula("a4-a4*a4/a4");
            Assert.AreEqual(0, (double)f.Evaluate(s => 3), 1e-9);
        }

        // Test of the Equals method
        [TestMethod()]
        public void Test33()
        {
            Formula f1 = new Formula("X1+X2");
            Formula f2 = new Formula("X1+X2");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod()]
        public void Test34()
        {
            Formula f1 = new Formula("X1+X2");
            Formula f2 = new Formula(" X1  +  X2   ");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod()]
        public void Test35()
        {
            Formula f1 = new Formula("2+X1*3.00");
            Formula f2 = new Formula("2.00+X1*3.0");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod()]
        public void Test36()
        {
            Formula f1 = new Formula("1e-2 + X5 + 17.00 * 19 ");
            Formula f2 = new Formula("   0.0100  +     X5+ 17 * 19.00000 ");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod()]
        public void Test37()
        {
            Formula f = new Formula("2");
            Assert.IsFalse(f.Equals(null));
            Assert.IsFalse(f.Equals(""));
        }


        // Tests of == operator
        [TestMethod()]
        public void Test38()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("2");
            Assert.IsTrue(f1 == f2);
        }

        [TestMethod()]
        public void Test39()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("5");
            Assert.IsFalse(f1 == f2);
        }

        [TestMethod()]
        public void Test40()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("2");
            Assert.IsFalse(null == f1);
            Assert.IsFalse(f1 == null);
            Assert.IsTrue(f1 == f2);
        }

        // Tests of != operator
        [TestMethod()]
        public void Test41()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("2");
            Assert.IsFalse(f1 != f2);
        }

        [TestMethod()]
        public void Test42()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("5");
            Assert.IsTrue(f1 != f2);
        }


        // Test of ToString method
        [TestMethod()]
        public void Test43()
        {
            Formula f = new Formula("2*5");
            Assert.IsTrue(f.Equals(new Formula(f.ToString())));
        }


        // Tests of GetHashCode method
        [TestMethod()]
        public void Test44()
        {
            Formula f1 = new Formula("2*5");
            Formula f2 = new Formula("2*5");
            Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
        }

        [TestMethod()]
        public void Test45()
        {
            Formula f1 = new Formula("2*5");
            Formula f2 = new Formula("3/8*2+(7)");
            Assert.IsTrue(f1.GetHashCode() != f2.GetHashCode());
        }

        // Tests of GetVariables method
        [TestMethod()]
        public void Test46()
        {
            Formula f = new Formula("2*5");
            Assert.IsFalse(f.GetVariables().GetEnumerator().MoveNext());
        }

        [TestMethod()]
        public void Test47()
        {
            Formula f = new Formula("2*X2");
            List<string> actual = new List<string>(f.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "X2" };
            Assert.AreEqual(actual.Count, 1);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod()]
        public void Test48()
        {
            Formula f = new Formula("2*X2+Y3");
            List<string> actual = new List<string>(f.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "Y3", "X2" };
            Assert.AreEqual(actual.Count, 2);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod()]
        public void Test49()
        {
            Formula f = new Formula("2*X2+X2");
            List<string> actual = new List<string>(f.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "X2" };
            Assert.AreEqual(actual.Count, 1);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod()]
        public void Test50()
        {
            Formula f = new Formula("X1+Y2*X3*Y2+Z7+X1/Z8");
            List<string> actual = new List<string>(f.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "X1", "Y2", "X3", "Z7", "Z8" };
            Assert.AreEqual(actual.Count, 5);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        // Tests to make sure there can be more than one formula at a time
        [TestMethod()]
        public void Test51a()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("3");
            Assert.IsTrue(f1.ToString().IndexOf("2") >= 0);
            Assert.IsTrue(f2.ToString().IndexOf("3") >= 0);
        }

        [TestMethod()]
        public void Test51b()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("3");
            Assert.IsTrue(f1.ToString().IndexOf("2") >= 0);
            Assert.IsTrue(f2.ToString().IndexOf("3") >= 0);
        }

        [TestMethod()]
        public void Test51c()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("3");
            Assert.IsTrue(f1.ToString().IndexOf("2") >= 0);
            Assert.IsTrue(f2.ToString().IndexOf("3") >= 0);
        }

        [TestMethod()]
        public void Test51d()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("3");
            Assert.IsTrue(f1.ToString().IndexOf("2") >= 0);
            Assert.IsTrue(f2.ToString().IndexOf("3") >= 0);
        }

        [TestMethod()]
        public void Test51e()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("3");
            Assert.IsTrue(f1.ToString().IndexOf("2") >= 0);
            Assert.IsTrue(f2.ToString().IndexOf("3") >= 0);
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test52a()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test52b()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test52c()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test52d()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test52e()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }
        //-------------------------------- End of Provided Tests -----------------------------------------------------------
    }
}
