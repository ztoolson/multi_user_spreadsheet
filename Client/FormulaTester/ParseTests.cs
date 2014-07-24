using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParseMessage;
using System.Collections.Generic;

namespace TestParse
{
    [TestClass]
    public class ParseTests
    {
        //--------------------------------------------------  UPDATE messsage checks -------------------------------------------------------
        [TestMethod]
        public void TestUpdate00()
        {
            string message = "UPDATE" + (char)27 + "cell_name" + (char)27 + "cell_content\n";
            List<string> l = Parse.MessageParse(message);

            Assert.AreEqual("cell_name", l[0]);
            Assert.AreEqual("cell_content", l[1]);
        }

        [TestMethod]
        public void TestUpdate01()
        {

            string message = "UPDATE" + (char)27 + "1" + (char)27 + "a" + (char)27 + "b\n";
            List<string> l = Parse.MessageParse(message);

            Assert.AreEqual("1", l[0]);
            Assert.AreEqual("a", l[1]);
            Assert.AreEqual("b", l[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestUpdate03()
        {
            string message = "UPDATE" + (char)27 + "1" + (char)27 +  "a" + (char)27 + "b\n\n";
            Parse.MessageParse(message);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestUpdate04()
        {
            string message = "UPDATE" + (char)27 + (char)27 + "1" + (char)27 + "a" + (char)27 + "b\n";
            Parse.MessageParse(message);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestUpdate05()
        {
            string message = "UPDATE" + (char)27 + (char)27 + "1" + (char)27 + "a" + (char)27 + "b\n\n";
            Parse.MessageParse(message);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestUpdate06()
        {
            string message = "UPDATE" + (char)27 + (char)27 + "a" + (char)27 + "b";
            Parse.MessageParse(message);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestUpdate07()
        {
            string message = "UPDATE"  + "1" + (char)27 + "a" + (char)27 + "b\n";
            Parse.MessageParse(message);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestUpdate08()
        {
            string message = "UPDATE" + (char)27 + (char)27 + "b\n";
            Parse.MessageParse(message);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestUpdate09()
        {
            string message = "UPDATE" + (char)27+ "a" + (char)27 + "\n";
            Parse.MessageParse(message);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestUpdate10()
        {
            string message = "UPDATE" + (char)27 + "a" + (char)27 + "\n";
            Parse.MessageParse(message);
        }
        //----------------------------------------------- end of UPDATE messsage tests -------------------------------------------------


        //---------------------------------------------- INVALID message tests -----------------------------------------------------------

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void TestInvalid01()
    {
        string message = "INVALID\n\n";
        Parse.MessageParse(message);
    }
        //----------------------------------------- end of INVALID messsage tests ------------------------------------------------------
    


        //------------------------------------------ ERROR message tests ----------------------------------------------------------------

    [TestMethod]
        public void TestError00()
    {
        string message = "ERROR" + (char)27 + "a\n";
        List<string> l = Parse.MessageParse(message);

        Assert.AreEqual("a", l[0]);
    }
        
        // -------------------------------------------End of ERROR message tests -------------------------------------------------------
    
        

        // --------------------------------------- FILELIST tests -------------------------------------------------------------------------------

    [TestMethod]
    public void TestFileList00()
    {
        string message = "FILELIST" + (char)27 + "a" + (char)27 + "b" + (char)27 + "c\n";
        List<string> l = Parse.MessageParse(message);

        Assert.AreEqual(l.Count, 3);

        Assert.AreEqual(l[0], "a");
        Assert.AreEqual(l[1], "b");
        Assert.AreEqual(l[2], "c");
    }

    [TestMethod]
    public void TestFileList01()
    {
        string message = "FILELIST" + (char)27 + (char)27 + (char)27 + "a" + (char)27 + "b" + (char)27 + "c\n";
        List<string> l = Parse.MessageParse(message);

        Assert.AreEqual(l.Count, 3);

        Assert.AreEqual(l[0], "a");
        Assert.AreEqual(l[1], "b");
        Assert.AreEqual(l[2], "c");
    }
        // ----------------------------------------- end of FILELIST tests --------------------------------------------------------------------

    }
}
