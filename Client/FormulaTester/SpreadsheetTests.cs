using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System.Collections.Generic;
using System.Text;
//using System.Xml;
using System.IO;
using System.Threading;


namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        //--------------------------------------NEW PS5 TESTS--------------------------------------------------------

        //      ------------------------Abstract Spreadsheet Constructor Tests-----------------------------------

        //  NOT WORRIED ABOUT THIS TEST RIGHT NOW!
        [TestMethod]
        //This test was written after a little code.
            //Test passes if no exceptions are thrown
        public void TestAbstractSpreadsheet50()             //file path aint workin!!!!!!
        {
            AbstractSpreadsheet sheet1 = new Spreadsheet();
            AbstractSpreadsheet sheet2 = new Spreadsheet(x => true, x => x.ToUpper(), "Version2");
            AbstractSpreadsheet sheet3 = new Spreadsheet("PathToFile", x => true, x => x.ToUpper(), "Version3");
        }

        //      -----------------------End of Abstract Spreadsheet Constructor tests---------------------------



        //      -------------------------------GetCellValueTests-------------------------------------------------------

        [TestMethod]
        //This test was written after a little code.
        [ExpectedException(typeof(InvalidNameException))]
        //Null name.
        public void TestGetCellValue50()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellValue(null);
        }

        [TestMethod]
        //This test was written after a little code.
        [ExpectedException(typeof(InvalidNameException))]
        //Invalid name.
        public void TestGetCellValue51()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();  
            sheet.GetCellValue("_a3");
        }

        [TestMethod]
        //This test was written after a little code.
        [ExpectedException(typeof(InvalidNameException))]
        //Invalid name.
        public void TestGetCellValue52()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();  
            sheet.GetCellValue("");
        }

        [TestMethod]
        //This test was written after a little code.
        [ExpectedException(typeof(InvalidNameException))]
        //Invalid name.
        public void TestGetCellValue53()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();  
            sheet.GetCellValue(" ");
        }

        [TestMethod]
        //This test was written after a little code.
        [ExpectedException(typeof(InvalidNameException))]
        //Invalid name.
        public void TestGetCellValue54()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellValue("&");
        }

        [TestMethod]
        //This test was written after a little code.
        [ExpectedException(typeof(InvalidNameException))]
        //Invalid name.
        public void TestGetCellValue55()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellValue("7DV");
        }


        [TestMethod]
        //This test was written after a little code.
        [ExpectedException(typeof(InvalidNameException))]
        //Invalid name.
        public void TestGetCellValue56()    
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellValue("a7a7a7");
        }

        [TestMethod]
        //This test was written in order to debug said method.
        //Valid names with dependencies.
        public void TestGetCellValue57()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "1");
            sheet.SetContentsOfCell("B1", "=A1 + 1");
            
            Assert.AreEqual(2.0, sheet.GetCellValue("B1"));
        }

        [TestMethod]
        //This test was written in order to debug said method.
        //Valid names without dependencies.
        public void TestGetCellValue58()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B1", "=(1 + 1 * 2)/3");

            Assert.AreEqual(1.0, sheet.GetCellValue("B1"));
        }

        [TestMethod]
        //This test was written in order to debug said method.
        //Valid names with dependencies.
        public void TestGetCellValue59()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "1");
            sheet.SetContentsOfCell("B1", "=(A1 + 1 * 2)/3");       

            Assert.AreEqual(1.0, sheet.GetCellValue("B1"));
        }

        [TestMethod]
        //This test was written in order to debug said method.
        //Valid names with dependencies.
        public void TestGetCellValue60()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "1");
            sheet.SetContentsOfCell("B1", "=(A1 + A1 * 2)/3");       

            Assert.AreEqual(1.0, sheet.GetCellValue("B1"));
        }

     
        [TestMethod]
        //This test was written in order to debug said method.
        //Valid names with dependencies.
        public void TestGetCellValue61()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "1");
            sheet.SetContentsOfCell("B1", "=A1");  // = 1
            sheet.SetContentsOfCell("C1", "=A1 + B1");  //= 2
            sheet.SetContentsOfCell("D1", "=C1 + A1"); // = 3
            sheet.SetContentsOfCell("E1", "=D1 +C1+ B1 + A1"); // =7

            Assert.AreEqual(1.0, sheet.GetCellValue("B1")); //I think the -1 is coming from Evaluator -- "Last Token"
            Assert.AreEqual(2.0, sheet.GetCellValue("C1"));
        }

        [TestMethod]
        //This test was written in order to debug said method.
        //Valid names with dependencies.
        public void TestGetCellValue62()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "howdy");
          

            Assert.AreEqual("howdy", sheet.GetCellValue("A1")); //I think the -1 is coming from Evaluator -- "Last Token"

            //Assert.AreEqual(1.0, sheet.GetCellValue("B1"));
        }


        //      -----------------------------End of GetCellValue tests------------------------------------------------

        

        //      -------------------------Tests for SetContentsOfCell-------------------------------------------------

        [TestMethod]
        //This test was written after a little code.
        [ExpectedException(typeof(ArgumentNullException))]
        //  Null content.
        public void TestSetContentsOfCell50()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("AA7", null);
        }

        [TestMethod]
        //This test was written after a little code.
        [ExpectedException(typeof(InvalidNameException))]
        //  Null name.
        public void TestSetContentsOfCell51()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "7");
        }

        [TestMethod]
        //This test was written after a little code.
        [ExpectedException(typeof(InvalidNameException))]
        //  Invalid name.
        public void TestSetContentsOfCell52()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("_AA7", "face");
        }

        [TestMethod]
        //This test was written after a little code.
        [ExpectedException(typeof(InvalidNameException))]
        //  Invalid name.
        public void TestSetContentsOfCell53()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("7", "face");
        }

        [TestMethod]
        //This test was written after a little code.
        [ExpectedException(typeof(InvalidNameException))]
        //  Invalid name.
        public void TestSetContentsOfCell54()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a7a", "face");
        }

        [TestMethod]
        //This test was written after a little code.
        [ExpectedException(typeof(InvalidNameException))]
        //  Invalid name.
        public void TestSetContentsOfCell55()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("      ", "face");
        }

        [TestMethod]
        //This test was written after a little code.
        [ExpectedException(typeof(InvalidNameException))]
        //  Invalid name.
        public void TestSetContentsOfCell56()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("-", "face");
        }

        [TestMethod]
        //This test was written after a little code.
        //  Valid name, test passes if no exception is thrown.
        public void TestSetContentsOfCell57()       
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A5", "face");
        }

        
        //      ------------------------End of Tests for SetContentsOfCell-----------------------------------------

        //      ----------------------------Tests for Save Method-----------------------------------------------------

        [TestMethod]
        //This test was written after the expected completion of the Save method.
        public void TestSave00()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "1");
            sheet.SetContentsOfCell("B1", "=A1");
            sheet.SetContentsOfCell("C1", "=A1 + B1");
            sheet.SetContentsOfCell("D1", "C1");    // a string, not a formula

            sheet.Save("Save00");
        }


        //      ----------------------------End of tests for Save Method--------------------------------------------


      

        //-------------------------------END OF NEW PS5 TESTS-------------------------------------------------


        //**************** OLD PS4 TESTS THAT MAY OR MAY NOT BE STILL VALUABLE ****************

        //--------------------AbstractSpreadsheet & Spreadsheet constructor tests------------------------
        //This test was written before any code.
        //  If exception is thrown, test fails.
        [TestMethod]
        public void TestAbstractSpreadsheet01()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
        }

        //This test was written after code for Spreadsheet constructor, and Cell class.
        //  If exception is thrown, test fails.
        [TestMethod]
        public void TestAbstractSpreadsheet02()
        {
            Spreadsheet sheet = new Spreadsheet();
        }

        //This test was written after code for Spreadsheet constructor, and Cell class.
        //  Null Test.
        [TestMethod]
        public void TestAbstractSpreadsheet03()
        {
            Spreadsheet sheet = new Spreadsheet();
            Assert.IsFalse(ReferenceEquals(sheet, null));
        }

        //This test was written after code for Spreadsheet constructor, and Cell class.
        //  Null Test.
        [TestMethod]
        public void TestAbstractSpreadsheet04()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.IsFalse(ReferenceEquals(sheet, null));
        }

      
        //-------------------------End of AbstractSpreadsheet & Spreadsheet Tests----------------------------------
   

        //---------------------------Tests for SetCellContents (Double)-----------------------------------------------


        // This test is a PS4 adaptation.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //Null name.
        public void TestInvalidSetCellContentsDouble00()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            String nullString = null;
            sheet.SetContentsOfCell(nullString, "88");
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //Invalid name.
        public void TestInvalidSetCellContentsDouble01()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("7", "88");
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //Invalid name.
        public void TestInvalidSetCellContentsDouble02()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("7a", "88");
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //Invalid name.
        public void TestInvalidSetCellContentsDouble03()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("", "88");
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //Invalid name.
        public void TestInvalidSetCellContentsDouble04()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(" ", "88");
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //Invalid name.
        public void TestInvalidSetCellContentsDouble05()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("!", "88");
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //Invalid name.
        public void TestInvalidSetCellContentsDouble06()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a!", "88");
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //Invalid name.
        public void TestInvalidSetCellContentsDouble07()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("+", "88");
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //Invalid name.
        public void TestInvalidSetCellContentsDouble08()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("s+", "88");
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //Invalid name.
        public void TestInvalidSetCellContentsDouble09()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("%+", "88");
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //Invalid name.
        public void TestInvalidSetCellContentsDouble10()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a 3", "88");
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        //First addition, and adding with duplicate key but different value.
        public void TestFirstSetCellContentsDouble00()
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();      
           ISet<String> set = new HashSet<String>();

            set =sheet.SetContentsOfCell("A5", "88");
            Assert.IsTrue(set.Count == 0);
            Assert.AreEqual(88.0, sheet.GetCellContents("A5"));
            //  Duplicate key add with different cell contents.
            set = sheet.SetContentsOfCell("A5", "8");
            Assert.IsTrue(set.Count == 0);
            Assert.AreEqual(8.0, sheet.GetCellContents("A5"));
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        //First addition, and adding with duplicate key but different value.
        public void TestMoreSetCellContentsDouble00()
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();  //NOT IMPLEMENTED
            ISet<String> set = new HashSet<String>();

            set = sheet.SetContentsOfCell("A5", "88");
            Assert.IsTrue(set.Count == 0);
           
            set = sheet.SetContentsOfCell("A5", "88");
            Assert.IsTrue(set.Count == 0);
        }


        //-----------------------End of Tests for SetCellContents (Double)-----------------------------------


        //----------------------------Tests for GetCellContents--------------------------------------------------

        // This test is a PS4 adaptation.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //  Test exception throwing.
        public void TestInvalidGetCellContentsDouble00()
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents(null);
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //  Test exception throwing.
        public void TestInvalidGetCellContentsDouble01()
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("%");
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //  Test exception throwing.
        public void TestInvalidGetCellContentsDouble02()
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("");
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //  Test exception throwing.
        public void TestInvalidGetCellContentsDouble03()
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents(" ");
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //  Test exception throwing.
        public void TestInvalidGetCellContentsDouble04()
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("5a ");
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        //  Test for non-existent name.
        public void TestNoNameGetCellContentsDouble00()
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.AreEqual("", sheet.GetCellContents("a5"));
        }

        //--------------------------End of Tests for GetCellContents------------------------------------------


        //---------------------------Tests for GetNamesOfAllNonEmptyCells--------------------------------------

        // This test is a PS4 adaptation.
        [TestMethod]
        //  Test for empty IEnumerable
        public void TestGetNamesOFAllNonEmptyCellsDouble00()
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();
            List<String> nonEmpty = (List<String>)sheet.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(0, nonEmpty.Count);
        }

        // This test is a PS4 adaptation.
        [TestMethod]
        //  Test for non empty IEnumerable.
        public void TestGetNamesOFAllNonEmptyCellsDouble01()    //NOT IMPLEMENTED.
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();

            //Make additions, duplicate and non duplicate names.
            sheet.SetContentsOfCell("A1", "5");
            sheet.SetContentsOfCell("A1", "6");
            sheet.SetContentsOfCell("b1", "5");
            sheet.SetContentsOfCell("A6", "9");
            //sheet.SetContentsOfCell("_A1__78QQ", 5);      //  WRITE A TEST CASE FOR OBSOLETE VARIABLES!

            List<String> nonEmpty = (List<String>)sheet.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(3, nonEmpty.Count);
            Assert.IsTrue(nonEmpty.Contains("A1"));
            Assert.IsTrue(nonEmpty.Contains("b1"));
            Assert.IsTrue(nonEmpty.Contains("A6"));
            //Assert.IsTrue(nonEmpty.Contains("_A1__78QQ"));
            //  Check duplicate overwrite.
            Assert.AreEqual(6.0, sheet.GetCellContents("A1"));
        }
        //-----------------------End of tests for GetNamesOfAllNonEmptyCells-----------------------------


        //---------------------------Tests For SetCellContents(formula) method-----------------------------

        //  This test is an adaptation of PS4.
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        //  Null formula check.
        public void TestSetCellContentsFormula00()
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();
            String i = "=" + null;
            sheet.SetContentsOfCell("A1", i);
        }

        //  This test is an adaptation of PS4.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //  Null name check.
        public void TestSetCellContentsFormula01()
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "=A1");
        }

        //  This test is an adaptation of PS4.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        //  Invalid name check.
        public void TestSetCellContentsFormula02()
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("#", "=A1");
        }

        //  This test is an adaptation of PS4.
        [TestMethod]
        //  Formula containing no variables.
        public void TestSetCellContentsFormula03()
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=1+1");
        }

        //  This test is an adaptation of PS4.
        [TestMethod]
        // Plain.
        public void TestSetCellContentsFormula04()
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=B1");
        }

        //  This test is an adaptation of PS4.
        [TestMethod]
        // Plain.
        public void TestSetCellContentsFormula05()
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=B1 + C1");
        }

        //  This test is an adaptation of PS4.
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        // CircularException.
        public void TestSetCellContentsFormula06()
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=B1 + C1 + A1");
        }

        //----------------------End of Tests for SetCellContents(formula) method--------------------------


        //--------------------------Tests for SetCellContents(text) method------------------------------------

        //  This test was written after preliminary completion of the SCC(text) method.
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetCellContentsText00()
        {       //Null text check
            AbstractSpreadsheet sheet = new Spreadsheet();
            String nullString = null;
            sheet.SetContentsOfCell("A1", nullString);
        }

        //  This test was written after preliminary completion of the SCC(text) method.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsText01()
        {       //Null name check
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "text");
        }

        //  This test was written after preliminary completion of the SCC(text) method.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsText02()
        {       //Invalid name check
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("&", "text");
        }

        //  This test was written after expected completion of the SCC(text) method.
        [TestMethod]
        //  Coverage
        public void TestSetCellContentsText03()
        {       
            AbstractSpreadsheet sheet = new Spreadsheet();
            ISet<String> set = new HashSet<String>();                        
            Assert.IsTrue(set.Count == 0);
        }

        //------------------------End of tests for SetCellContents(text) method------------------------------

        
        
        //-----------------------Tests for multiple SCC methods--------------------------------------------------

        //  This is a test.
        [TestMethod]
        public void TestMultipleSCC00()
        {       //Test double and formula SCC
            AbstractSpreadsheet sheet = new Spreadsheet();
            ISet<String> depSet = new HashSet<String>();

            Formula B1 = new Formula("A1"); //B1 depends on A1
            Formula C1 = new Formula("B1 + A1");
           
            String[] setToArray = new String[10];

            //  Test example from spec.
             sheet.SetContentsOfCell("B1", "=A1");  //B1 = A1
             sheet.SetContentsOfCell("C1", "=B1 + A1");             //C1 = B1 + A1
             depSet = sheet.SetContentsOfCell("A1", "1");

            Assert.IsTrue(sheet.GetCellValue("A1").Equals(1.0));
            Assert.IsTrue(depSet.Contains("A1"));
            Assert.IsTrue(depSet.Contains("B1"));
            Assert.IsTrue(depSet.Contains("C1"));
            depSet.CopyTo(setToArray,0);
            Assert.IsTrue(setToArray[0].Equals("A1"));
            Assert.AreEqual(1.0, sheet.GetCellValue("B1"));
            Assert.AreEqual(2.0, sheet.GetCellValue("C1"));     //C1 showing a value of Zero here.

            depSet.Clear();
            

            depSet = sheet.SetContentsOfCell("B1", "=A1"); //B1 = A1
            Assert.IsTrue(depSet.Contains("B1"));
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(depSet.Count == 2);
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("B1"));

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("C1", "=B1 + A1"); //C1 = B1 + A1
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(depSet.Count == 1);
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("C1"));

            depSet.Clear();

            Formula A2 = new Formula("C1 + 1");
            depSet = sheet.SetContentsOfCell("A2", "=C1 + 1");//new guy    //A2 = C1 + 1
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("C1"));
            Assert.IsTrue(depSet.Count == 1);
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A2"));

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("A1", "1");
            Assert.IsTrue(depSet.Contains("A1"));
            Assert.IsTrue(depSet.Contains("B1"));
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(depSet.Count == 4);
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A1"));

            depSet.Clear();

            //  Now change B1's dependency
            depSet = sheet.SetContentsOfCell("B1", "");   
            Assert.IsTrue(depSet.Contains("B1"));
            Assert.IsTrue(depSet.Contains("C1"));   
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(depSet.Count == 3);
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("B1"));

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("A1", "1");
            Assert.IsTrue(depSet.Contains("A1"));
            Assert.IsTrue(depSet.Contains("C1")); 
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(depSet.Count == 3);
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A1"));

            depSet.Clear();

            Formula B2 = new Formula("A1 + C2");        //New guy
            depSet = sheet.SetContentsOfCell("B2", "=A1 + C2");  //B2 = A1 + C2
            Assert.IsTrue(depSet.Contains("B2"));
            Assert.IsTrue(depSet.Count == 1);
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("B2"));

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("A1", "1");
            Assert.IsTrue(depSet.Contains("A1"));
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(depSet.Contains("B2"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("C2"));
            Assert.IsTrue(depSet.Count == 4);
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A1"));

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("A2", "= C1 + 1");//A2 = C1 + 1
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("B2"));
            Assert.IsTrue(!depSet.Contains("C1"));
            Assert.IsTrue(!depSet.Contains("C2"));
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A2"));
            
            Assert.IsTrue(depSet.Count == 1);

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("B1", "");
            Assert.IsTrue(depSet.Contains("B1"));
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("B2"));
            Assert.IsTrue(!depSet.Contains("C2"));
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("B1"));
            Assert.IsTrue(depSet.Count == 3);

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("C1", "= B1 + A1"); //C1 = B1 + A1
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(depSet.Contains("A2"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("B2"));
            Assert.IsTrue(!depSet.Contains("C2"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("C1"));
            Assert.IsTrue(depSet.Count == 2);

            //Now change C1 to a double
                depSet.Clear();

            sheet.SetContentsOfCell("C1", "1");

            depSet = sheet.SetContentsOfCell("A1", "1");
            Assert.IsTrue(depSet.Contains("A1"));
            Assert.IsTrue(depSet.Contains("B2"));

            Assert.IsTrue(!depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("C1"));
            Assert.IsTrue(!depSet.Contains("C2"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A1"));
            Assert.IsTrue(depSet.Count == 2);

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("A2", "= C1 + 1"); //A2 = C1 + 1
            Assert.IsTrue(depSet.Contains("A2"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("B2"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("C1"));
            Assert.IsTrue(!depSet.Contains("C2"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A2"));
            Assert.IsTrue(depSet.Count == 1);

            Assert.IsTrue(sheet.GetCellContents("A1").Equals(1.0));
            Assert.IsTrue(sheet.GetCellContents("A2").Equals(A2));
            Assert.IsTrue(sheet.GetCellContents("B1").Equals(""));
            Assert.IsTrue(sheet.GetCellContents("B2").Equals(B2));
            Assert.IsTrue(sheet.GetCellContents("C1").Equals(1.0));
            Assert.IsTrue(sheet.GetCellContents("C2").Equals(""));

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("B1", "");
            Assert.IsTrue(depSet.Contains("B1"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("B2"));
            Assert.IsTrue(!depSet.Contains("C1"));
            Assert.IsTrue(!depSet.Contains("C2"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("B1"));
            Assert.IsTrue(depSet.Count == 1);

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("C1", "");
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(depSet.Contains("A2"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("B2"));
            Assert.IsTrue(!depSet.Contains("C2"));
            Assert.IsTrue(!depSet.Contains("B1"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("C1"));
            Assert.IsTrue(depSet.Count == 2);

          
            //New guy
            Formula C2 = new Formula("B1 - A2");
            sheet.SetContentsOfCell("C2", "=B1 - A2");


            depSet.Clear();

            depSet = sheet.SetContentsOfCell("A1", "1");
            Assert.IsTrue(depSet.Contains("A1"));
            Assert.IsTrue(depSet.Contains("B2"));

            Assert.IsTrue(!depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("C1"));
            Assert.IsTrue(!depSet.Contains("C2"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A1"));
            Assert.IsTrue(depSet.Count == 2);


            depSet.Clear();

            depSet = sheet.SetContentsOfCell("A2", "= C1 + 1"); //A2 = C1 + 1
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(depSet.Contains("C2"));
            Assert.IsTrue(depSet.Contains("B2"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("C1"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A2"));
            Assert.IsTrue(depSet.Count == 3);


            depSet.Clear();

            depSet = sheet.SetContentsOfCell("B1", "");
            Assert.IsTrue(depSet.Contains("B1"));
            Assert.IsTrue(depSet.Contains("C2"));
            Assert.IsTrue(depSet.Contains("B2"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("C1"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("B1"));
            Assert.IsTrue(depSet.Count == 3);


            depSet.Clear();

            depSet = sheet.SetContentsOfCell("B2", "= A1 + C2");     //B2 = A1 + C2
            Assert.IsTrue(depSet.Contains("B2"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("C1"));
            Assert.IsTrue(!depSet.Contains("C2"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("B2"));
            Assert.IsTrue(depSet.Count == 1);


            depSet.Clear();

            depSet = sheet.SetContentsOfCell("C1", "1");
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(depSet.Contains("C2"));
            Assert.IsTrue(depSet.Contains("B2"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("B1"));
            

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("C1"));
            Assert.IsTrue(depSet.Count == 4);


            depSet.Clear();

            depSet = sheet.SetContentsOfCell("C2", "=B1 - A2");     //C2 = B1 - A2
            Assert.IsTrue(depSet.Contains("C2"));
            Assert.IsTrue(depSet.Contains("B2"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("C1"));


            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("C2"));
            Assert.IsTrue(depSet.Count == 2);

            Assert.IsTrue(sheet.GetCellContents("A1").Equals(1.0));
            Assert.IsTrue(sheet.GetCellContents("A2").Equals(A2));
            Assert.IsTrue(sheet.GetCellContents("B1").Equals(""));
            Assert.IsTrue(sheet.GetCellContents("B2").Equals(B2));
            Assert.IsTrue(sheet.GetCellContents("C1").Equals(1.0));
            Assert.IsTrue(sheet.GetCellContents("C2").Equals(C2));


            //aye yai yai!!
        }

        //-------------------------End of tests for multiple SCC methods----------------------------------------


        //************END OF OLD PS4 TESTS THAT MAY OR MAY NOT BE STILL VALUABLE ***********


        //                                                              other tests

        /// <summary>
        ///This is a test class for SpreadsheetTest and is intended
        ///to contain all SpreadsheetTest Unit Tests
        ///</summary>
        [TestClass()]
        public class GradingTests
        {


            private TestContext testContextInstance;

            /// <summary>
            ///Gets or sets the test context which provides
            ///information about and functionality for the current test run.
            ///</summary>
            public TestContext TestContext
            {
                get
                {
                    return testContextInstance;
                }
                set
                {
                    testContextInstance = value;
                }
            }

            #region Additional test attributes
            // 
            //You can use the following additional attributes as you write your tests:
            //
            //Use ClassInitialize to run code before running the first test in the class
            //[ClassInitialize()]
            //public static void MyClassInitialize(TestContext testContext)
            //{
            //}
            //
            //Use ClassCleanup to run code after all tests in a class have run
            //[ClassCleanup()]
            //public static void MyClassCleanup()
            //{
            //}
            //
            //Use TestInitialize to run code before running each test
            //[TestInitialize()]
            //public void MyTestInitialize()
            //{
            //}
            //
            //Use TestCleanup to run code after each test has run
            //[TestCleanup()]
            //public void MyTestCleanup()
            //{
            //}
            //
            #endregion

            // Verifies cells and their values, which must alternate.
            public void VV(AbstractSpreadsheet sheet, params object[] constraints)
            {
                for (int i = 0; i < constraints.Length; i += 2)
                {
                    if (constraints[i + 1] is double)
                    {       //Invalid Cast????                                                          //THIS VALUE IS COMING BACK A FORMULA ERROR!!
                        Assert.AreEqual((double)constraints[i + 1], (double)sheet.GetCellValue((string)constraints[i]), 1e-9);
                    }
                    else
                    {
                        Assert.AreEqual(constraints[i + 1], sheet.GetCellValue((string)constraints[i]));
                    }
                }
            }


            // For setting a spreadsheet cell.
            public IEnumerable<string> Set(AbstractSpreadsheet sheet, string name, string contents)
            {
                List<string> result = new List<string>(sheet.SetContentsOfCell(name, contents));
                return result;
            }

            // Tests IsValid
            [TestMethod()]
            public void IsValidTest1()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "x");
            }

            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void IsValidTest2()
            {
                AbstractSpreadsheet ss = new Spreadsheet(s => s[0] != 'A', s => s, "");
                ss.SetContentsOfCell("A1", "x");
            }

            [TestMethod()]
            public void IsValidTest3()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("B1", "= A1 + C1");
            }

            [TestMethod()]
            [ExpectedException(typeof(FormulaFormatException))]
            public void IsValidTest4()
            {
                AbstractSpreadsheet ss = new Spreadsheet(s => s[0] != 'A', s => s, "");
                ss.SetContentsOfCell("B1", "= A1 + C1");
            }

            // Tests Normalize
            [TestMethod()]
            public void NormalizeTest1()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("B1", "hello");
                Assert.AreEqual("", s.GetCellContents("b1"));
            }

            [TestMethod()]
            public void NormalizeTest2()
            {
                AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "");
                ss.SetContentsOfCell("B1", "hello");
                Assert.AreEqual("hello", ss.GetCellContents("b1"));
            }

            [TestMethod()]
            public void NormalizeTest3()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("a1", "5");
                s.SetContentsOfCell("A1", "6");
                s.SetContentsOfCell("B1", "= a1");
                Assert.AreEqual(5.0, (double)s.GetCellValue("B1"), 1e-9);
            }

            [TestMethod()]
            public void NormalizeTest4()
            {
                AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "");
                ss.SetContentsOfCell("a1", "5");
                ss.SetContentsOfCell("A1", "6");
                ss.SetContentsOfCell("B1", "= a1");
                Assert.AreEqual(6.0, (double)ss.GetCellValue("B1"), 1e-9);
            }

            // Simple tests
            [TestMethod()]
            public void EmptySheet()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                VV(ss, "A1", "");
            }


            [TestMethod()]
            public void OneString()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                OneString(ss);
            }

            public void OneString(AbstractSpreadsheet ss)
            {
                Set(ss, "B1", "hello");
                VV(ss, "B1", "hello");
            }


            [TestMethod()]
            public void OneNumber()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                OneNumber(ss);
            }

            public void OneNumber(AbstractSpreadsheet ss)
            {
                Set(ss, "C1", "17.5");
                VV(ss, "C1", 17.5);
            }


            [TestMethod()]
            public void OneFormula()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                OneFormula(ss);
            }

            public void OneFormula(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "B1", "5.2");
                Set(ss, "C1", "= A1+B1");
                VV(ss, "A1", 4.1, "B1", 5.2, "C1", 9.3);
            }


            [TestMethod()]
            public void Changed()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                Assert.IsFalse(ss.Changed);
                Set(ss, "C1", "17.5");
                Assert.IsTrue(ss.Changed);
            }


            [TestMethod()]
            public void DivisionByZero1()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                DivisionByZero1(ss);
            }

            public void DivisionByZero1(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "B1", "0.0");
                Set(ss, "C1", "= A1 / B1");
                Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
            }

            [TestMethod()]
            public void DivisionByZero2()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                DivisionByZero2(ss);
            }

            public void DivisionByZero2(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "5.0");
                Set(ss, "A3", "= A1 / 0.0");
                Assert.IsInstanceOfType(ss.GetCellValue("A3"), typeof(FormulaError));
            }



            [TestMethod()]
            public void EmptyArgument()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                EmptyArgument(ss);
            }

            public void EmptyArgument(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "C1", "= A1 + B1");
                Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
            }


            [TestMethod()]
            public void StringArgument()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                StringArgument(ss);
            }

            public void StringArgument(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "B1", "hello");
                Set(ss, "C1", "= A1 + B1");
                Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
            }


            [TestMethod()]
            public void ErrorArgument()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                ErrorArgument(ss);
            }

            public void ErrorArgument(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "B1", "");
                Set(ss, "C1", "= A1 + B1");
                Set(ss, "D1", "= C1");
                Assert.IsInstanceOfType(ss.GetCellValue("D1"), typeof(FormulaError));
            }


            [TestMethod()]
            public void NumberFormula1()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                NumberFormula1(ss);
            }

            public void NumberFormula1(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "C1", "= A1 + 4.2");
                VV(ss, "C1", 8.3);
            }


            [TestMethod()]
            public void NumberFormula2()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                NumberFormula2(ss);
            }

            public void NumberFormula2(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "= 4.6");
                VV(ss, "A1", 4.6);
            }


            // Repeats the simple tests all together
            [TestMethod()]
            public void RepeatSimpleTests()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                Set(ss, "A1", "17.32");
                Set(ss, "B1", "This is a test");
                Set(ss, "C1", "= A1+B1");
                OneString(ss);
                OneNumber(ss);
                OneFormula(ss);
                DivisionByZero1(ss);
                DivisionByZero2(ss);
                StringArgument(ss);
                ErrorArgument(ss);
                NumberFormula1(ss);
                NumberFormula2(ss);
            }

            // Four kinds of formulas
            [TestMethod()]
            public void Formulas()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                Formulas(ss);
            }

            public void Formulas(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.4");
                Set(ss, "B1", "2.2");
                Set(ss, "C1", "= A1 + B1");
                Set(ss, "D1", "= A1 - B1");
                Set(ss, "E1", "= A1 * B1");
                Set(ss, "F1", "= A1 / B1");
                VV(ss, "C1", 6.6, "D1", 2.2, "E1", 4.4 * 2.2, "F1", 2.0);
            }

            [TestMethod()]
            public void Formulasa()
            {
                Formulas();
            }

            [TestMethod()]
            public void Formulasb()
            {
                Formulas();
            }

            // Are multiple spreadsheets supported?
            [TestMethod()]
            public void Multiple()
            {
                AbstractSpreadsheet s1 = new Spreadsheet();
                AbstractSpreadsheet s2 = new Spreadsheet();
                Set(s1, "X1", "hello");
                Set(s2, "X1", "goodbye");
                VV(s1, "X1", "hello");
                VV(s2, "X1", "goodbye");
            }

            [TestMethod()]
            public void Multiplea()
            {
                Multiple();
            }

            [TestMethod()]
            public void Multipleb()
            {
                Multiple();
            }

            [TestMethod()]
            public void Multiplec()
            {
                Multiple();
            }

            //// Reading/writing spreadsheets              //  <-- I'M THROUGH WITH YOU!!!
            //[TestMethod()]
            //[ExpectedException(typeof(SpreadsheetReadWriteException))]
            //public void SaveTest1()
            //{
            //    AbstractSpreadsheet ss = new Spreadsheet();
            //    ss.Save("q:\\missing\\save.txt");
            //}

            [TestMethod()]
            [ExpectedException(typeof(SpreadsheetReadWriteException))]
            public void SaveTest2()
            {
                AbstractSpreadsheet ss = new Spreadsheet("q:\\missing\\save.txt", s => true, s => s, "");
            }

            [TestMethod()]
            public void SaveTest3()
            {
                AbstractSpreadsheet s1 = new Spreadsheet();
                Set(s1, "A1", "hello");
                s1.Save("save1.txt");
                s1 = new Spreadsheet("save1.txt", s => true, s => s, "default");
                Assert.AreEqual("hello", s1.GetCellContents("A1"));
            }

            [TestMethod()]
            [ExpectedException(typeof(SpreadsheetReadWriteException))]
            public void SaveTest4()
            {
                using (StreamWriter writer = new StreamWriter("save2.txt"))
                {
                    writer.WriteLine("This");
                    writer.WriteLine("is");
                    writer.WriteLine("a");
                    writer.WriteLine("test!");
                }
                AbstractSpreadsheet ss = new Spreadsheet("save2.txt", s => true, s => s, "");
            }

            [TestMethod()]
            [ExpectedException(typeof(SpreadsheetReadWriteException))]
            public void SaveTest5()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                ss.Save("save3.txt");
                ss = new Spreadsheet("save3.txt", s => true, s => s, "version");
            }

            [TestMethod()]
            public void SaveTest6()
            {
                AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s, "hello");
                ss.Save("save4.txt");
                Assert.AreEqual("hello", new Spreadsheet().GetSavedVersion("save4.txt"));
            }

            //[TestMethod()]
            //public void SaveTest7()
            //{
            //    using (XmlWriter writer = XmlWriter.Create("save5.txt"))
            //    {
            //        writer.WriteStartDocument();
            //        writer.WriteStartElement("spreadsheet");
            //        writer.WriteAttributeString("version", "");

            //        writer.WriteStartElement("cell");
            //        writer.WriteElementString("name", "A1");
            //        writer.WriteElementString("contents", "hello");
            //        writer.WriteEndElement();

            //        writer.WriteStartElement("cell");
            //        writer.WriteElementString("name", "A2");
            //        writer.WriteElementString("contents", "5.0");
            //        writer.WriteEndElement();

            //        writer.WriteStartElement("cell");
            //        writer.WriteElementString("name", "A3");
            //        writer.WriteElementString("contents", "4.0");
            //        writer.WriteEndElement();

            //        writer.WriteStartElement("cell");
            //        writer.WriteElementString("name", "A4");
            //        writer.WriteElementString("contents", "= A2 + A3");
            //        writer.WriteEndElement();

            //        writer.WriteEndElement();
            //        writer.WriteEndDocument();
            //    }
            //    AbstractSpreadsheet ss = new Spreadsheet("save5.txt", s => true, s => s, "");
            //    VV(ss, "A1", "hello", "A2", 5.0, "A3", 4.0, "A4", 9.0);
            //}

            //[TestMethod()]
            //public void SaveTest8()
            //{
            //    AbstractSpreadsheet ss = new Spreadsheet();
            //    Set(ss, "A1", "hello");
            //    Set(ss, "A2", "5.0");
            //    Set(ss, "A3", "4.0");
            //    Set(ss, "A4", "= A2 + A3");
            //    ss.Save("save6.txt");
            //    using (XmlReader reader = XmlReader.Create("save6.txt"))
            //    {
            //        int spreadsheetCount = 0;
            //        int cellCount = 0;
            //        bool A1 = false;
            //        bool A2 = false;
            //        bool A3 = false;
            //        bool A4 = false;
            //        string name = null;
            //        string contents = null;

            //        while (reader.Read())
            //        {
            //            if (reader.IsStartElement())
            //            {
            //                switch (reader.Name)
            //                {
            //                    case "spreadsheet":
            //                        Assert.AreEqual("default", reader["version"]);
            //                        spreadsheetCount++;
            //                        break;

            //                    case "cell":
            //                        cellCount++;
            //                        break;

            //                    case "name":
            //                        reader.Read();
            //                        name = reader.Value;
            //                        break;

            //                    case "contents":
            //                        reader.Read();
            //                        contents = reader.Value;
            //                        break;
            //                }
            //            }
            //            else
            //            {
            //                switch (reader.Name)
            //                {
            //                    case "cell":
            //                        if (name.Equals("A1")) { Assert.AreEqual("hello", contents); A1 = true; }
            //                        else if (name.Equals("A2")) { Assert.AreEqual(5.0, Double.Parse(contents), 1e-9); A2 = true; }
            //                        else if (name.Equals("A3")) { Assert.AreEqual(4.0, Double.Parse(contents), 1e-9); A3 = true; }
            //                        else if (name.Equals("A4")) { contents = contents.Replace(" ", ""); Assert.AreEqual("=A2+A3", contents); A4 = true; }
            //                        else Assert.Fail();
            //                        break;
            //                }
            //            }
            //        }
            //        Assert.AreEqual(1, spreadsheetCount);
            //        Assert.AreEqual(4, cellCount);
            //        Assert.IsTrue(A1);
            //        Assert.IsTrue(A2);
            //        Assert.IsTrue(A3);
            //        Assert.IsTrue(A4);
            //    }
            //}


            // Fun with formulas
            [TestMethod()]
            public void Formula1()
            {
                Formula1(new Spreadsheet());
            }
            public void Formula1(AbstractSpreadsheet ss)
            {
                Set(ss, "a1", "= a2 + a3");
                Set(ss, "a2", "= b1 + b2");
                Assert.IsInstanceOfType(ss.GetCellValue("a1"), typeof(FormulaError));
                Assert.IsInstanceOfType(ss.GetCellValue("a2"), typeof(FormulaError));
                Set(ss, "a3", "5.0");
                Set(ss, "b1", "2.0");
                Set(ss, "b2", "3.0");
                VV(ss, "a1", 10.0, "a2", 5.0);  
                Set(ss, "b2", "4.0");
                VV(ss, "a1", 11.0, "a2", 6.0);
            }

            [TestMethod()]
            public void Formula2()      
            {
                Formula2(new Spreadsheet());
            }
            public void Formula2(AbstractSpreadsheet ss)
            {
                Set(ss, "a1", "= a2 + a3");
                Set(ss, "a2", "= a3");
                Set(ss, "a3", "6.0");
                VV(ss, "a1", 12.0, "a2", 6.0, "a3", 6.0);
                Set(ss, "a3", "5.0");
                VV(ss, "a1", 10.0, "a2", 5.0, "a3", 5.0);
            }

            [TestMethod()]
            public void Formula3()
            {
                Formula3(new Spreadsheet());
            }
            public void Formula3(AbstractSpreadsheet ss)    
            {
                Set(ss, "a1", "= a3 + a5");
                Set(ss, "a2", "= a5 + a4");
                Set(ss, "a3", "= a5");
                Set(ss, "a4", "= a5");
                Set(ss, "a5", "9.0");
                VV(ss, "a1", 18.0);
                VV(ss, "a2", 18.0);
                Set(ss, "a5", "8.0");
                VV(ss, "a1", 16.0);
                VV(ss, "a2", 16.0);
            }

            [TestMethod()]
            public void Formula4()      
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                Formula1(ss);
                Formula2(ss);
                Formula3(ss);
            }

            [TestMethod()]
            public void Formula4a()   
            {
                Formula4();
            }


            [TestMethod()]
            public void MediumSheet()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                MediumSheet(ss);
            }

            public void MediumSheet(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "1.0");
                Set(ss, "A2", "2.0");
                Set(ss, "A3", "3.0");
                Set(ss, "A4", "4.0");
                Set(ss, "B1", "= A1 + A2");
                Set(ss, "B2", "= A3 * A4");
                Set(ss, "C1", "= B1 + B2");
                VV(ss, "A1", 1.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 3.0, "B2", 12.0, "C1", 15.0);
                Set(ss, "A1", "2.0");
                VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 4.0, "B2", 12.0, "C1", 16.0);
                Set(ss, "B1", "= A1 / A2");
                VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
            }

            [TestMethod()]
            public void MediumSheeta()
            {
                MediumSheet();
            }


            [TestMethod()]
            public void MediumSave()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                MediumSheet(ss);
                ss.Save("save7.txt");
                ss = new Spreadsheet("save7.txt", s => true, s => s, "default");
                VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
            }

            [TestMethod()]
            public void MediumSavea()
            {
                MediumSave();
            }


            // A long chained formula.  If this doesn't finish within 60 seconds, it fails.
            [TestMethod()]
            public void LongFormulaTest()
            {
                object result = "";
                Thread t = new Thread(() => LongFormulaHelper(out result));
                t.Start();
                t.Join(60 * 1000);
                if (t.IsAlive)
                {
                    t.Abort();
                    Assert.Fail("Computation took longer than 60 seconds");
                }
                Assert.AreEqual("ok", result);
            }

            public void LongFormulaHelper(out object result)
            {
                try
                {
                    AbstractSpreadsheet s = new Spreadsheet();
                    s.SetContentsOfCell("sum1", "= a1 + a2");
                    int i;
                    int depth = 100;
                    for (i = 1; i <= depth * 2; i += 2)
                    {
                        s.SetContentsOfCell("a" + i, "= a" + (i + 2) + " + a" + (i + 3));
                        s.SetContentsOfCell("a" + (i + 1), "= a" + (i + 2) + "+ a" + (i + 3));
                    }
                    s.SetContentsOfCell("a" + i, "1");
                    s.SetContentsOfCell("a" + (i + 1), "1");
                    Assert.AreEqual(Math.Pow(2, depth + 1), (double)s.GetCellValue("sum1"), 1.0);
                    s.SetContentsOfCell("a" + i, "0");
                    Assert.AreEqual(Math.Pow(2, depth), (double)s.GetCellValue("sum1"), 1.0);
                    s.SetContentsOfCell("a" + (i + 1), "0");
                    Assert.AreEqual(0.0, (double)s.GetCellValue("sum1"), 0.1);
                    result = "ok";
                }
                catch (Exception e)
                {
                    result = e;
                }
            }

        }

//-------------------------------------------- ALL TESTS FROM PS5 PASTED HERE-------------------------------------

        //--------------------------------------NEW PS5 TESTS--------------------------------------------------------

        //      ------------------------Abstract Spreadsheet Constructor Tests-----------------------------------

       

        //      -----------------------End of Abstract Spreadsheet Constructor tests---------------------------



        //      -------------------------------GetCellValueTests-------------------------------------------------------

       

        [TestMethod]
        //This test was written for the resubmission of this assignment.
        public void TestSaveAndLoad01()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "1");
            sheet.SetContentsOfCell("B1", "=A1");
            sheet.SetContentsOfCell("C1", "=A1 + B1");
            sheet.SetContentsOfCell("D1", "C1");    // a string, not a formula
            sheet.SetContentsOfCell("A1", "2");

            Formula B1Formula = new Formula("A1");
            Formula C1Formula = new Formula("A1+B1");

            sheet.Save("Save00");

            AbstractSpreadsheet sheetLoad = new Spreadsheet("Save00", s => true, s => s, "default");

            List<String> cells = (List<String>)sheetLoad.GetNamesOfAllNonemptyCells();

            Assert.IsTrue(cells.Contains("A1"));
            Assert.IsTrue(cells.Contains("B1"));
            Assert.IsTrue(cells.Contains("C1"));
            Assert.IsTrue(cells.Contains("D1"));

            Assert.IsTrue(cells.Count == 4);

            Assert.AreEqual(2.0, sheetLoad.GetCellContents("A1"));
            Assert.AreEqual(2.0, sheetLoad.GetCellValue("A1"));

            Assert.AreEqual(B1Formula, sheetLoad.GetCellContents("B1"));
            Assert.AreEqual(2.0, sheetLoad.GetCellValue("B1"));

            Assert.AreEqual(C1Formula, sheetLoad.GetCellContents("C1"));
            Assert.AreEqual(4.0, sheetLoad.GetCellValue("C1"));

            Assert.AreEqual("C1", sheetLoad.GetCellContents("D1"));
            Assert.AreEqual("C1", sheetLoad.GetCellValue("D1"));
        }

        //      ----------------------------End of tests for Save Method--------------------------------------------




        //-------------------------------END OF NEW PS5 TESTS-------------------------------------------------


        //**************** OLD PS4 TESTS THAT MAY OR MAY NOT BE STILL VALUABLE ****************

        //--------------------AbstractSpreadsheet & Spreadsheet constructor tests------------------------
      

       

        // Resubmission test.
        [TestMethod]
        public void TestMultipleSCC01()
        {       //Test double and formula SCC
            AbstractSpreadsheet sheet = new Spreadsheet();
            ISet<String> depSet = new HashSet<String>();

            Formula B1 = new Formula("A1"); //B1 depends on A1
            Formula C1 = new Formula("B1 + A1");

            String[] setToArray = new String[10];

            //  Test example from spec.
            sheet.SetContentsOfCell("B1", "=A1");  //B1 = A1
            sheet.SetContentsOfCell("C1", "=B1 + A1");             //C1 = B1 + A1
            depSet = sheet.SetContentsOfCell("A1", "1");

            Assert.IsTrue(sheet.GetCellValue("A1").Equals(1.0));
            Assert.IsTrue(depSet.Contains("A1"));
            Assert.IsTrue(depSet.Contains("B1"));
            Assert.IsTrue(depSet.Contains("C1"));
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A1"));
            Assert.AreEqual(1.0, sheet.GetCellValue("B1"));
            Assert.AreEqual(2.0, sheet.GetCellValue("C1"));     //C1 showing a value of Zero here.

            depSet.Clear();


            depSet = sheet.SetContentsOfCell("B1", "=A1"); //B1 = A1
            Assert.IsTrue(depSet.Contains("B1"));
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(depSet.Count == 2);
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("B1"));

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("C1", "=B1 + A1"); //C1 = B1 + A1
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(depSet.Count == 1);
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("C1"));

            depSet.Clear();

            Formula A2 = new Formula("C1 + 1");
            depSet = sheet.SetContentsOfCell("A2", "=C1 + 1");//new guy    //A2 = C1 + 1
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("C1"));
            Assert.IsTrue(depSet.Count == 1);
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A2"));

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("A1", "1");
            Assert.IsTrue(depSet.Contains("A1"));
            Assert.IsTrue(depSet.Contains("B1"));
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(depSet.Count == 4);
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A1"));

            depSet.Clear();

            //  Now change B1's dependency
            depSet = sheet.SetContentsOfCell("B1", "");
            Assert.IsTrue(depSet.Contains("B1"));
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(depSet.Count == 3);
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("B1"));

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("A1", "1");
            Assert.IsTrue(depSet.Contains("A1"));
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(depSet.Count == 3);
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A1"));

            depSet.Clear();

            Formula B2 = new Formula("A1 + C2");        //New guy
            depSet = sheet.SetContentsOfCell("B2", "=A1 + C2");  //B2 = A1 + C2
            Assert.IsTrue(depSet.Contains("B2"));
            Assert.IsTrue(depSet.Count == 1);
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("B2"));

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("A1", "1");
            Assert.IsTrue(depSet.Contains("A1"));
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(depSet.Contains("B2"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("C2"));
            Assert.IsTrue(depSet.Count == 4);
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A1"));

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("A2", "= C1 + 1");//A2 = C1 + 1
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("B2"));
            Assert.IsTrue(!depSet.Contains("C1"));
            Assert.IsTrue(!depSet.Contains("C2"));
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A2"));

            Assert.IsTrue(depSet.Count == 1);

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("B1", "");
            Assert.IsTrue(depSet.Contains("B1"));
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("B2"));
            Assert.IsTrue(!depSet.Contains("C2"));
            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("B1"));
            Assert.IsTrue(depSet.Count == 3);

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("C1", "= B1 + A1"); //C1 = B1 + A1
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(depSet.Contains("A2"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("B2"));
            Assert.IsTrue(!depSet.Contains("C2"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("C1"));
            Assert.IsTrue(depSet.Count == 2);

            //Now change C1 to a double
            depSet.Clear();

            sheet.SetContentsOfCell("C1", "1");

            depSet = sheet.SetContentsOfCell("A1", "1");
            Assert.IsTrue(depSet.Contains("A1"));
            Assert.IsTrue(depSet.Contains("B2"));

            Assert.IsTrue(!depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("C1"));
            Assert.IsTrue(!depSet.Contains("C2"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A1"));
            Assert.IsTrue(depSet.Count == 2);

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("A2", "= C1 + 1"); //A2 = C1 + 1
            Assert.IsTrue(depSet.Contains("A2"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("B2"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("C1"));
            Assert.IsTrue(!depSet.Contains("C2"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A2"));
            Assert.IsTrue(depSet.Count == 1);

            Assert.IsTrue(sheet.GetCellContents("A1").Equals(1.0));
            Assert.IsTrue(sheet.GetCellContents("A2").Equals(A2));
            Assert.IsTrue(sheet.GetCellContents("B1").Equals(""));
            Assert.IsTrue(sheet.GetCellContents("B2").Equals(B2));
            Assert.IsTrue(sheet.GetCellContents("C1").Equals(1.0));
            Assert.IsTrue(sheet.GetCellContents("C2").Equals(""));

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("B1", "");
            Assert.IsTrue(depSet.Contains("B1"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("B2"));
            Assert.IsTrue(!depSet.Contains("C1"));
            Assert.IsTrue(!depSet.Contains("C2"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("B1"));
            Assert.IsTrue(depSet.Count == 1);

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("C1", "");
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(depSet.Contains("A2"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("B2"));
            Assert.IsTrue(!depSet.Contains("C2"));
            Assert.IsTrue(!depSet.Contains("B1"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("C1"));
            Assert.IsTrue(depSet.Count == 2);


            //New guy
            Formula C2 = new Formula("B1 - A2");
            sheet.SetContentsOfCell("C2", "=B1 - A2");

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("A1", "1");
            Assert.IsTrue(depSet.Contains("A1"));
            Assert.IsTrue(depSet.Contains("B2"));

            Assert.IsTrue(!depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("C1"));
            Assert.IsTrue(!depSet.Contains("C2"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A1"));
            Assert.IsTrue(depSet.Count == 2);


            depSet.Clear();

            depSet = sheet.SetContentsOfCell("A2", "= C1 + 1"); //A2 = C1 + 1
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(depSet.Contains("C2"));
            Assert.IsTrue(depSet.Contains("B2"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("C1"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("A2"));
            Assert.IsTrue(depSet.Count == 3);

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("B1", "");
            Assert.IsTrue(depSet.Contains("B1"));
            Assert.IsTrue(depSet.Contains("C2"));
            Assert.IsTrue(depSet.Contains("B2"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("C1"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("B1"));
            Assert.IsTrue(depSet.Count == 3);

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("B2", "= A1 + C2");     //B2 = A1 + C2
            Assert.IsTrue(depSet.Contains("B2"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("C1"));
            Assert.IsTrue(!depSet.Contains("C2"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("B2"));
            Assert.IsTrue(depSet.Count == 1);

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("C1", "1");
            Assert.IsTrue(depSet.Contains("C1"));
            Assert.IsTrue(depSet.Contains("A2"));
            Assert.IsTrue(depSet.Contains("C2"));
            Assert.IsTrue(depSet.Contains("B2"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("B1"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("C1"));
            Assert.IsTrue(depSet.Count == 4);

            depSet.Clear();

            depSet = sheet.SetContentsOfCell("C2", "=B1 - A2");     //C2 = B1 - A2
            Assert.IsTrue(depSet.Contains("C2"));
            Assert.IsTrue(depSet.Contains("B2"));

            Assert.IsTrue(!depSet.Contains("A1"));
            Assert.IsTrue(!depSet.Contains("A2"));
            Assert.IsTrue(!depSet.Contains("B1"));
            Assert.IsTrue(!depSet.Contains("C1"));

            depSet.CopyTo(setToArray, 0);
            Assert.IsTrue(setToArray[0].Equals("C2"));
            Assert.IsTrue(depSet.Count == 2);

            Assert.IsTrue(sheet.GetCellContents("A1").Equals(1.0));
            Assert.IsTrue(sheet.GetCellContents("A2").Equals(A2));
            Assert.IsTrue(sheet.GetCellContents("B1").Equals(""));
            Assert.IsTrue(sheet.GetCellContents("B2").Equals(B2));
            Assert.IsTrue(sheet.GetCellContents("C1").Equals(1.0));
            Assert.IsTrue(sheet.GetCellContents("C2").Equals(C2));

            //save
            sheet.Save("SaveSheet");

            Spreadsheet loadSheet = new Spreadsheet("SaveSheet", s => true, s => s, "default");

            List<String> loadSheetCells = (List<String>)loadSheet.GetNamesOfAllNonemptyCells();

            Assert.IsTrue(loadSheetCells.Contains("A1"));
            Assert.AreEqual(1.0, loadSheet.GetCellContents("A1"));
            Assert.AreEqual(1.0, loadSheet.GetCellValue("A1"));


            //Assert.IsTrue(loadSheetCells.Contains("A2"));
            //Assert.AreEqual(new Formula("C1 + 1") , loadSheet.GetCellContents("A2"));
            //Assert.IsTrue(loadSheet.GetCellValue("A2") is FormulaError);

            int i;
        }

        //-------------------------End of tests for multiple SCC methods----------------------------------------

        //  -------------------------------------------- Circular exception tests- ----------------------------

        //Resubmission Test.
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestCircular00()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A1", "1");
            sheet.SetContentsOfCell("B1", "=A1");
            sheet.SetContentsOfCell("A1", "=B1");
        }

        //Resubmission Test.
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestCircular01()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A1", "1");
            sheet.SetContentsOfCell("B1", "=A1");
            sheet.SetContentsOfCell("C1", "=B1");
            sheet.SetContentsOfCell("D1", "=C1");
            sheet.SetContentsOfCell("E1", "=D1");
            sheet.SetContentsOfCell("A1", "=B1");
        }


        //  --------------------------------------End of circular exception tests -----------------------------

        //************END OF OLD PS4 TESTS THAT MAY OR MAY NOT BE STILL VALUABLE ***********


        //                                                              other tests

   
            // Verifies cells and their values, which must alternate.
            public void VV(AbstractSpreadsheet sheet, params object[] constraints)
            {
                for (int i = 0; i < constraints.Length; i += 2)
                {
                    if (constraints[i + 1] is double)
                    {       //Invalid Cast????                                                          //THIS VALUE IS COMING BACK A FORMULA ERROR!!
                        Assert.AreEqual((double)constraints[i + 1], (double)sheet.GetCellValue((string)constraints[i]), 1e-9);
                    }
                    else
                    {
                        Assert.AreEqual(constraints[i + 1], sheet.GetCellValue((string)constraints[i]));
                    }
                }
            }


            // For setting a spreadsheet cell.
            public IEnumerable<string> Set(AbstractSpreadsheet sheet, string name, string contents)
            {
                List<string> result = new List<string>(sheet.SetContentsOfCell(name, contents));
                return result;
            }

            // Tests IsValid
            [TestMethod()]
            public void IsValidTest1()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "x");
            }

            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void IsValidTest2()
            {
                AbstractSpreadsheet ss = new Spreadsheet(s => s[0] != 'A', s => s, "");
                ss.SetContentsOfCell("A1", "x");
            }

            [TestMethod()]
            public void IsValidTest3()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("B1", "= A1 + C1");
            }

            [TestMethod()]
            [ExpectedException(typeof(FormulaFormatException))]
            public void IsValidTest4()
            {
                AbstractSpreadsheet ss = new Spreadsheet(s => s[0] != 'A', s => s, "");
                ss.SetContentsOfCell("B1", "= A1 + C1");
            }

            // Tests Normalize
            [TestMethod()]
            public void NormalizeTest1()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("B1", "hello");
                Assert.AreEqual("", s.GetCellContents("b1"));
            }

            [TestMethod()]
            public void NormalizeTest2()
            {
                AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "");
                ss.SetContentsOfCell("B1", "hello");
                Assert.AreEqual("hello", ss.GetCellContents("b1"));
            }

            [TestMethod()]
            public void NormalizeTest3()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("a1", "5");
                s.SetContentsOfCell("A1", "6");
                s.SetContentsOfCell("B1", "= a1");
                Assert.AreEqual(5.0, (double)s.GetCellValue("B1"), 1e-9);
            }

            [TestMethod()]
            public void NormalizeTest4()
            {
                AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "");
                ss.SetContentsOfCell("a1", "5");
                ss.SetContentsOfCell("A1", "6");
                ss.SetContentsOfCell("B1", "= a1");
                Assert.AreEqual(6.0, (double)ss.GetCellValue("B1"), 1e-9);
            }

            // Simple tests
            [TestMethod()]
            public void EmptySheet()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                VV(ss, "A1", "");
            }


            [TestMethod()]
            public void OneString()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                OneString(ss);
            }

            public void OneString(AbstractSpreadsheet ss)
            {
                Set(ss, "B1", "hello");
                VV(ss, "B1", "hello");
            }


            [TestMethod()]
            public void OneNumber()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                OneNumber(ss);
            }

            public void OneNumber(AbstractSpreadsheet ss)
            {
                Set(ss, "C1", "17.5");
                VV(ss, "C1", 17.5);
            }


            [TestMethod()]
            public void OneFormula()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                OneFormula(ss);
            }

            public void OneFormula(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "B1", "5.2");
                Set(ss, "C1", "= A1+B1");
                VV(ss, "A1", 4.1, "B1", 5.2, "C1", 9.3);
            }

            [TestMethod()]
            public void DivisionByZero1()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                DivisionByZero1(ss);
            }

            public void DivisionByZero1(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "B1", "0.0");
                Set(ss, "C1", "= A1 / B1");
                Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
            }

            public void DivisionByZero2(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "5.0");
                Set(ss, "A3", "= A1 / 0.0");
                Assert.IsInstanceOfType(ss.GetCellValue("A3"), typeof(FormulaError));
            }

            [TestMethod()]
            public void EmptyArgument()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                EmptyArgument(ss);
            }

            public void EmptyArgument(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "C1", "= A1 + B1");
                Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
            }

            [TestMethod()]
            public void StringArgument()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                StringArgument(ss);
            }

            public void StringArgument(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "B1", "hello");
                Set(ss, "C1", "= A1 + B1");
                Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
            }


            [TestMethod()]
            public void ErrorArgument()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                ErrorArgument(ss);
            }

            public void ErrorArgument(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "B1", "");
                Set(ss, "C1", "= A1 + B1");
                Set(ss, "D1", "= C1");
                Assert.IsInstanceOfType(ss.GetCellValue("D1"), typeof(FormulaError));
            }


            [TestMethod()]
            public void NumberFormula1()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                NumberFormula1(ss);
            }

            public void NumberFormula1(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "C1", "= A1 + 4.2");
                VV(ss, "C1", 8.3);
            }


            [TestMethod()]
            public void NumberFormula2()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                NumberFormula2(ss);
            }

            public void NumberFormula2(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "= 4.6");
                VV(ss, "A1", 4.6);
            }


            // Repeats the simple tests all together
            [TestMethod()]
            public void RepeatSimpleTests()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                Set(ss, "A1", "17.32");
                Set(ss, "B1", "This is a test");
                Set(ss, "C1", "= A1+B1");
                OneString(ss);
                OneNumber(ss);
                OneFormula(ss);
                DivisionByZero1(ss);
                DivisionByZero2(ss);
                StringArgument(ss);
                ErrorArgument(ss);
                NumberFormula1(ss);
                NumberFormula2(ss);
            }

            // Four kinds of formulas
            [TestMethod()]
            public void Formulas()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                Formulas(ss);
            }

            public void Formulas(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.4");
                Set(ss, "B1", "2.2");
                Set(ss, "C1", "= A1 + B1");
                Set(ss, "D1", "= A1 - B1");
                Set(ss, "E1", "= A1 * B1");
                Set(ss, "F1", "= A1 / B1");
                VV(ss, "C1", 6.6, "D1", 2.2, "E1", 4.4 * 2.2, "F1", 2.0);
            }

            [TestMethod()]
            public void Formulasa()
            {
                Formulas();
            }

            [TestMethod()]
            public void Formulasb()
            {
                Formulas();
            }


            // Are multiple spreadsheets supported?
            [TestMethod()]
            public void Multiple()
            {
                AbstractSpreadsheet s1 = new Spreadsheet();
                AbstractSpreadsheet s2 = new Spreadsheet();
                Set(s1, "X1", "hello");
                Set(s2, "X1", "goodbye");
                VV(s1, "X1", "hello");
                VV(s2, "X1", "goodbye");
            }

            [TestMethod()]
            public void Multiplea()
            {
                Multiple();
            }

            [TestMethod()]
            public void Multipleb()
            {
                Multiple();
            }

            [TestMethod()]
            public void Multiplec()
            {
                Multiple();
            }

            //// Reading/writing spreadsheets
            //[TestMethod()]
            //[ExpectedException(typeof(SpreadsheetReadWriteException))]//YOU DOCTORED THIS EXCEPTION!!!
            //public void SaveTest1()
            //{
            //    AbstractSpreadsheet ss = new Spreadsheet();
            //    ss.Save("q:\\missing\\save.txt");
            //}

            [TestMethod()]
            [ExpectedException(typeof(SpreadsheetReadWriteException))]
            public void SaveTest2()
            {
                AbstractSpreadsheet ss = new Spreadsheet("q:\\missing\\save.txt", s => true, s => s, "");
            }

            [TestMethod()]
            public void SaveTest3()
            {
                AbstractSpreadsheet s1 = new Spreadsheet();
                Set(s1, "A1", "hello");
                s1.Save("save1.txt");
                s1 = new Spreadsheet("save1.txt", s => true, s => s, "default");
                Assert.AreEqual("hello", s1.GetCellContents("A1"));
            }

            [TestMethod()]
            [ExpectedException(typeof(SpreadsheetReadWriteException))]
            public void SaveTest4()
            {
                using (StreamWriter writer = new StreamWriter("save2.txt"))
                {
                    writer.WriteLine("This");
                    writer.WriteLine("is");
                    writer.WriteLine("a");
                    writer.WriteLine("test!");
                }
                AbstractSpreadsheet ss = new Spreadsheet("save2.txt", s => true, s => s, "");
            }

            [TestMethod()]
            [ExpectedException(typeof(SpreadsheetReadWriteException))]
            public void SaveTest5()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                ss.Save("save3.txt");
                ss = new Spreadsheet("save3.txt", s => true, s => s, "version");
            }

            [TestMethod()]
            public void SaveTest6()
            {
                AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s, "hello");
                ss.Save("save4.txt");
                Assert.AreEqual("hello", new Spreadsheet().GetSavedVersion("save4.txt"));
            }

            //[TestMethod()]  //It appears that nothing is being entered into the Spreadsheet via the file constructed below
            //public void SaveTest7()
            //{
            //    using (XmlWriter writer = XmlWriter.Create("save5.txt"))//this file does not contain any newlines.
            //    {
            //        writer.WriteStartDocument();
            //        writer.WriteStartElement("spreadsheet");
            //        writer.WriteAttributeString("version", "");

            //        writer.WriteStartElement("cell");
            //        writer.WriteElementString("name", "A1");
            //        writer.WriteElementString("contents", "hello");
            //        writer.WriteEndElement();

            //        writer.WriteStartElement("cell");
            //        writer.WriteElementString("name", "A2");
            //        writer.WriteElementString("contents", "5.0");
            //        writer.WriteEndElement();

            //        writer.WriteStartElement("cell");
            //        writer.WriteElementString("name", "A3");
            //        writer.WriteElementString("contents", "4.0");
            //        writer.WriteEndElement();

            //        writer.WriteStartElement("cell");
            //        writer.WriteElementString("name", "A4");
            //        writer.WriteElementString("contents", "= A2 + A3");
            //        writer.WriteEndElement();

            //        writer.WriteEndElement();
            //        writer.WriteEndDocument();
            //    }// THE NAME OF THIS TEST IS MISLEADING, THIS TEST DOES NOT TEST THE SAVE() METHOD, BUT THE LOAD METHOD.
            //    AbstractSpreadsheet ss = new Spreadsheet("save5.txt", s => true, s => s, "");
            //    VV(ss, "A1", "hello", "A2", 5.0, "A3", 4.0, "A4", 9.0);
            //}

            //[TestMethod()]
            //public void SaveTest8()
            //{
            //    AbstractSpreadsheet ss = new Spreadsheet();
            //    Set(ss, "A1", "hello");
            //    Set(ss, "A2", "5.0");
            //    Set(ss, "A3", "4.0");
            //    Set(ss, "A4", "= A2 + A3");
            //    ss.Save("save6.txt");
            //    using (XmlReader reader = XmlReader.Create("save6.txt"))
            //    {
            //        int spreadsheetCount = 0;
            //        int cellCount = 0;
            //        bool A1 = false;
            //        bool A2 = false;
            //        bool A3 = false;
            //        bool A4 = false;
            //        string name = null;
            //        string contents = null;

            //        while (reader.Read())
            //        {
            //            if (reader.IsStartElement())
            //            {
            //                switch (reader.Name)
            //                {
            //                    case "spreadsheet":
            //                        Assert.AreEqual("default", reader["version"]);
            //                        spreadsheetCount++;
            //                        break;

            //                    case "cell":
            //                        cellCount++;
            //                        break;

            //                    case "name":
            //                        reader.Read();
            //                        name = reader.Value;
            //                        break;

            //                    case "contents":
            //                        reader.Read();
            //                        contents = reader.Value;
            //                        break;
            //                }
            //            }
            //            else
            //            {
            //                switch (reader.Name)
            //                {
            //                    case "cell":
            //                        if (name.Equals("A1")) { Assert.AreEqual("hello", contents); A1 = true; }
            //                        else if (name.Equals("A2")) { Assert.AreEqual(5.0, Double.Parse(contents), 1e-9); A2 = true; }
            //                        else if (name.Equals("A3")) { Assert.AreEqual(4.0, Double.Parse(contents), 1e-9); A3 = true; }
            //                        else if (name.Equals("A4")) { contents = contents.Replace(" ", ""); Assert.AreEqual("=A2+A3", contents); A4 = true; }
            //                        else Assert.Fail();
            //                        break;
            //                }
            //            }
            //        }
            //        Assert.AreEqual(1, spreadsheetCount);
            //        Assert.AreEqual(4, cellCount);
            //        Assert.IsTrue(A1);
            //        Assert.IsTrue(A2);
            //        Assert.IsTrue(A3);
            //        Assert.IsTrue(A4);
            //    }
            //}


            // Fun with formulas
            [TestMethod()]
            public void Formula1()
            {
                Formula1(new Spreadsheet());
            }
            public void Formula1(AbstractSpreadsheet ss)
            {
                Set(ss, "a1", "= a2 + a3");
                Set(ss, "a2", "= b1 + b2");
                Assert.IsInstanceOfType(ss.GetCellValue("a1"), typeof(FormulaError));
                Assert.IsInstanceOfType(ss.GetCellValue("a2"), typeof(FormulaError));
                Set(ss, "a3", "5.0");
                Set(ss, "b1", "2.0");
                Set(ss, "b2", "3.0");
                VV(ss, "a1", 10.0, "a2", 5.0);  //Invalid cast exception!!!?!?!???!?
                Set(ss, "b2", "4.0");
                VV(ss, "a1", 11.0, "a2", 6.0);
            }

            [TestMethod()]
            public void Formula2()      //Invalid cast??????
            {
                Formula2(new Spreadsheet());
            }
            public void Formula2(AbstractSpreadsheet ss)
            {
                Set(ss, "a1", "= a2 + a3");
                Set(ss, "a2", "= a3");
                Set(ss, "a3", "6.0");
                VV(ss, "a1", 12.0, "a2", 6.0, "a3", 6.0);
                Set(ss, "a3", "5.0");
                VV(ss, "a1", 10.0, "a2", 5.0, "a3", 5.0);
            }

            [TestMethod()]
            public void Formula3()
            {
                Formula3(new Spreadsheet());
            }
            public void Formula3(AbstractSpreadsheet ss)    //Invalid cast????
            {
                Set(ss, "a1", "= a3 + a5");
                Set(ss, "a2", "= a5 + a4");
                Set(ss, "a3", "= a5");
                Set(ss, "a4", "= a5");
                Set(ss, "a5", "9.0");
                VV(ss, "a1", 18.0);
                VV(ss, "a2", 18.0);
                Set(ss, "a5", "8.0");
                VV(ss, "a1", 16.0);
                VV(ss, "a2", 16.0);
            }

            [TestMethod()]
            public void Formula4()      //Invalid cast???????
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                Formula1(ss);
                Formula2(ss);
                Formula3(ss);
            }

            [TestMethod()]
            public void Formula4a()     //Invalid cast??????
            {
                Formula4();
            }


            [TestMethod()]
            public void MediumSheet()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                MediumSheet(ss);
            }

            public void MediumSheet(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "1.0");
                Set(ss, "A2", "2.0");
                Set(ss, "A3", "3.0");
                Set(ss, "A4", "4.0");
                Set(ss, "B1", "= A1 + A2");
                Set(ss, "B2", "= A3 * A4");
                Set(ss, "C1", "= B1 + B2");
                VV(ss, "A1", 1.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 3.0, "B2", 12.0, "C1", 15.0);
                Set(ss, "A1", "2.0");
                VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 4.0, "B2", 12.0, "C1", 16.0);
                Set(ss, "B1", "= A1 / A2");
                VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
            }

            [TestMethod()]
            public void MediumSheeta()
            {
                MediumSheet();
            }


            [TestMethod()]
            public void MediumSave()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                MediumSheet(ss);
                ss.Save("save7.txt");// I THINK THE PROBLEM IS THAT MEDIUMSAVEA IS TRYING TO SAVE TO THE FILE
                // WHILE MEDIUMSAVE IS TRYING TO LOAD THE FILE....
                ss = new Spreadsheet("save7.txt", s => true, s => s, "default");
                VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
            }

            [TestMethod()]
            public void MediumSavea()
            {
                //System.Threading.Thread.Sleep(3000);
                MediumSave();
            }

            // A long chained formula.  If this doesn't finish within 60 seconds, it fails.
            [TestMethod()]
            public void LongFormulaTest()   //PS5 variable definition!!
            {
                object result = "";
                Thread t = new Thread(() => LongFormulaHelper(out result));
                t.Start();
                t.Join(60 * 1000);  //Originally  (60 * 1000)
                if (t.IsAlive)
                {
                    t.Abort();
                    Assert.Fail("Computation took longer than 60 seconds");
                }
                Assert.AreEqual("ok", result);
            }

            public void LongFormulaHelper(out object result)
            {
                try
                {
                    AbstractSpreadsheet s = new Spreadsheet();
                    s.SetContentsOfCell("sum1", "= a1 + a2");   
                    int i;
                    int depth = 100;        //Was originally 100
                    for (i = 1; i <= depth * 2; i += 2)
                    {
                        s.SetContentsOfCell("a" + i, "= a" + (i + 2) + " + a" + (i + 3));
                        s.SetContentsOfCell("a" + (i + 1), "= a" + (i + 2) + "+ a" + (i + 3));
                    }
                    s.SetContentsOfCell("a" + i, "1");
                    s.SetContentsOfCell("a" + (i + 1), "1");
                    Assert.AreEqual(Math.Pow(2, depth + 1), (double)s.GetCellValue("sum1"), 1.0);  
                    //2^51 = 2251799813685248
                    s.SetContentsOfCell("a" + i, "0");
                    Assert.AreEqual(Math.Pow(2, depth), (double)s.GetCellValue("sum1"), 1.0);
                    s.SetContentsOfCell("a" + (i + 1), "0");
                    Assert.AreEqual(0.0, (double)s.GetCellValue("sum1"), 0.1);
                    result = "ok";
                }
                catch (Exception e)
                {
                    result = e;
                }
            }

        //**************************************** Post semester tests ******************************************

        // Self explanitory
        [TestMethod]
        public void NoCellNameFormula()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("a1", "=1+1");
                Assert.IsTrue(s.GetCellValue("a1").Equals(2.0));
            }

        //THIS TEST IS PASSING,
        //  WHICH SUGGESTS THAT THE PROBLEM WITH THE PANEL NOT UPDATING CELLS WITH BAD DEPENDENCIES TO ERROR IS PROBLEM WITH PS6.
        [TestMethod]
        public void ChangeToFormulaError00()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1");
            s.SetContentsOfCell("B1", "=A1");
            s.SetContentsOfCell("A1", "f");
            Assert.IsNotInstanceOfType(s.GetCellValue("B1"), typeof(FormulaError));//  ToString()!!!
        }

        [TestMethod]
        //[ExpectedException(typeof(CircularException))]
        public void TestCircular02()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A1", "1");
            sheet.SetContentsOfCell("B1", "=A1");
            sheet.SetContentsOfCell("C1", "=B1");
            sheet.SetContentsOfCell("D1", "=C1");
            sheet.SetContentsOfCell("E1", "=D1");
            try
            { sheet.SetContentsOfCell("A1", "=E1"); }
            catch (CircularException e){ }
            finally
            {
                Assert.AreEqual(1.0, sheet.GetCellContents("A1"));  
                Assert.AreEqual(1.0, sheet.GetCellValue("A1"));
                Assert.AreEqual("A1", sheet.GetCellContents("B1").ToString());
                Assert.AreEqual(1.0, sheet.GetCellValue("B1"));
                Assert.AreEqual("B1", sheet.GetCellContents("C1").ToString());
                Assert.AreEqual(1.0, sheet.GetCellValue("C1"));
                Assert.AreEqual("C1", sheet.GetCellContents("D1").ToString());
                Assert.AreEqual(1.0, sheet.GetCellValue("D1"));
                Assert.AreEqual("D1", sheet.GetCellContents("E1").ToString());
                Assert.AreEqual(1.0, sheet.GetCellValue("E1"));
            }
          }

        //  This test is an adaptation of PS4, this is a weak, so we are going to improve upon it.
        [TestMethod]
        // CircularException, and proper reversion.
        public void TestCircular03()
        {       //test members.
            AbstractSpreadsheet sheet = new Spreadsheet();
            try
            {   //  This will cause a circular exception because of the circular dependency implied by such an assignment,
                //      which means reversion must take place.
                //          which means that "A1"s contents and value must be either set back to an empty string,
                //              or it could mean that "A1" will be set back to a non-existent cell....?!?!?
                sheet.SetContentsOfCell("A1", "=B1 + C1 + A1");
                //  At the very least,
                //      calling get cell contents as this point should return and empty string.
            }
            catch (CircularException e) {/*do nothing */}
            finally
            {
                Assert.AreEqual("", sheet.GetCellContents("A1"));
            }
        }
    
    
    }//<-- End of class..



}
