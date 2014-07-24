using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseMessage
{
    /// <summary>
    /// This class parses and handles all messages sent from server.
    /// </summary>
    public class Parse
    {
        /// <summary>
        /// will be used in determining the diference between messsages
        /// </summary>
        static int escCount = 0;

        // throw if message from server is gobledeeguke
        static FormatException i = new FormatException("Incoming message from server is not recognized.");

        /// <summary>
        /// Parses a message from the server,
        /// and throws an appropriate exception if the message sent from the server is not in keeping with the protocol.
        /// </summary>
        /// <param name="message">The message sent from the server</param>
        public static List<string> MessageParse(string message)
        {   
            // first thing, count the number of esc chars in the message
            foreach (char letter in message)
            {
                if (letter == (char)27)
                {
                    escCount++;
                }
            }

            // FILELIST message  --------------------------------------------------------------------------------------------------------
            if(message.StartsWith("FILELIST"))
            {
                // return the list, as a list.
                return splitAndTrim(message);
            }

            // UPDATE messsage -------------------------------------------------------------------------------------------------------------------------------------------
            else if (message.StartsWith("UPDATE" + (char)27))
            {
                // various descriptive exceptions
                FormatException ue = new FormatException("UPDATE message was not correctly formated.");

                 if(escCount == 1 || escCount >= 3)
                 {
                        return splitAndTrim(message);
                 }
                    // else the update message sent from the server is invalid
                 else
                 {
                        throw ue;
                 }
            }
            // ERROR message --------------------------------------------------------------------------------------------------------------------
            else if (message.StartsWith("ERROR" + (char)27))
            {

                // return the list
                return splitAndTrim(message);
             }
            // SYNC message --------------------------------------------------------------------------------------------------------
            else if(message.StartsWith("SYNC" + (char)27))
            {
                return splitAndTrim(message);
            }

            return null;
        }

        /// <summary>
        /// Method splits a message by delimiters, removes the message label, and the ending \n.
        /// </summary>
        /// <param name="message">The message to be split</param>
        /// <returns>A list containing each token of the message</returns>
        private static List<string> splitAndTrim(string message)
        {// return value
            List<string> splitList = new List<string>();

            char[] delimiters = new char[] { (char)27, '\n' };

            // parse message by dilimiters and place into list.
            splitList = message.Split(delimiters).ToList<string>();
           
            return splitList;
        }

    }  // <-- end of  Parse class
}
