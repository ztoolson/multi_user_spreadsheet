using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetClient;

namespace MessageReceivedTests
{
    [TestClass]
    public class MessageReceivedTests
    {
        // Select a SS to load.
        [TestMethod]
        public void TestMessageReceived00()
        {
            Control c = new Control();

            string list = "FILELIST" + (char)27 + "a" + (char)27 + "b" + (char)27 + "c\n";
            c.MessageReceived(list, null, null);
        }

        // Load a SS
        [TestMethod]
        public void TestMessageReceived01()  
        {
            Control c = new Control();

            string list = "UPDATE" + (char)27 + "0" + (char)27 + "A1" + (char)27 + "=1 + 1" + (char)27 + "C1" + (char)27 + "d\n";
            c.MessageReceived(list, null, null);
        }

        // Open a new SS
        [TestMethod]
        public void TestMessageReceived02()
        {
            Control c = new Control();

            string list = "UPDATE" + (char)27 + "0\n";
            c.MessageReceived(list, null, null);
        }

        // Load a SS
        [TestMethod]
        public void TestMessageReceived03()
        {
            Control c = new Control();
            char esc = (char)27;

            string list = "UPDATE" + esc + "0" + esc + "A1" + esc + "1" + esc + "B1" + esc + "=A1" + esc + "C1" + esc + "=A1 + B1\n";
            c.MessageReceived(list, null, null);
        }

        // Load a SS
        [TestMethod]
        public void TestMessageReceived04()
        {
            Control c = new Control();
            char esc = (char)27;

            string list = "UPDATE" + esc + "0" + esc + "B1" + esc + "1" + esc + "C1" + esc + "=B1" + esc + "D1" + esc + "=C1 + B1\n";
           // c.MessageReceived(list, null, null);
        }

        // Load a SS
        [TestMethod]
        public void TestMessageReceived05()
        {
            Control c = new Control();
            char esc = (char)27;

            string list = "UPDATE" + esc + "0" + esc + "A1" + esc + "=Y1" + esc + "B1" + esc + "=A1" + esc + "C1" + esc + "=A1 + B1\n";
           // c.MessageReceived(list, null, null);
        }

        // Load a SS
        [TestMethod]
        public void TestMessageReceived06()
        {
            Control c = new Control();
            char esc = (char)27;

            string list = "UPDATE" + esc + "0" + esc + "A1" + esc + "=Y1" + esc + "B1" + esc + "=A1" + esc + "C1" + esc + "=A1 + B1" + esc + "A1" + esc + "1\n";
           // c.MessageReceived(list, null, null);
        }

        // Update a SS
        [TestMethod]
        public void TestMessageReceived07()
        {
            Control c = new Control();
            char esc = (char)27;

            string list = "UPDATE" + esc + "0" + esc + "A1" + esc + "1\n";
            c.MessageReceived(list, null, null);
        }

     

    }
}
