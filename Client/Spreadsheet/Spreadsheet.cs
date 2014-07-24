// Zach Toolson, Michael Call, Mike Fleming, Mark Mouritsen

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Threading;

namespace SS
{
    /// <summary>
    /// A spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string is a cell name if and only if it consists of one or more letters,
    /// followed by one or more digits AND it satisfies the predicate IsValid.
    /// For example, "A15", "a15", "XY032", and "BC7" are cell names so long as they
    /// satisfy IsValid.  On the other hand, "Z", "X_", and "hello" are not cell names,
    /// regardless of IsValid.
    /// 
    /// Any valid incoming cell name, whether passed as a parameter or embedded in a formula,
    /// must be normalized with the Normalize method before it is used by or saved in 
    /// this spreadsheet.  For example, if Normalize is s => s.ToUpper(), then
    /// the Formula "x3+a5" should be converted to "X3+A5" before use.
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  
    /// In addition to a name, each cell has a contents and a value.  The distinction is
    /// important.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In a new spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError,
    /// as reported by the Evaluate method of the Formula class.  The value of a Formula,
    /// of course, can depend on the values of variables.  The value of a variable is the 
    /// value of the spreadsheet cell it names (if that cell's value is a double) or 
    /// is undefined (otherwise).
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        // Graph to contain cell dependencies. used for checking for circular references
        private DependencyGraph dependencyGraph;

        // Spreadsheet is represented using a dictionary
        private Dictionary<String, Cell> spreadsheet;
        
        private string filePath;
        private bool isChanged;
        private int saveCount = 0;    

        /// <summary>
        /// Constructs a spreadsheet by recording its variable validity test,
        /// its normalization method, and its version information.  The variable validity
        /// test is used throughout to determine whether a string that consists of one or
        /// more letters followed by one or more digits is a valid cell name.  The variable
        /// equality test should be used throughout to determine whether two variables are
        /// equal.
        /// </summary>
        /// <param name="isValid">Additional user defined Cell name restriction</param>
        /// <param name="normalize">User defined formating restriction of Cell names</param>
        /// <param name="version">Version name of "this"</param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) :
            base(isValid, normalize, version)
        {
            //  IsValid is a user defined additional restriction,
            //      it has nothing to do with the base restriction
            this.IsValid = isValid;
            this.Normalize = normalize;

            //  Initialize components of "this".
            dependencyGraph = new DependencyGraph();
            spreadsheet = new Dictionary<string, Cell>();

            this.Version = version;
            isChanged = false;
        }

        /// <summary>
        /// Constructor which makes a Spreadsheet Object.
        /// </summary>
        public Spreadsheet() :
            base(s => true, s => s, "default")
        {
            //  Initialize components of "this".
            dependencyGraph = new DependencyGraph();
            spreadsheet = new Dictionary<string, Cell>();

            this.Version = "default";
        }

        /// <summary>
        /// TODO: THIS CONSTRUCTOR IS NOW OBSOLETE, WE ARE NOW LONGER SAVING OR LOADING SPREADSHEET TO/FROM DISK.
        /// Same implementation as three argument constructor,
        /// but with an additional String "FilePath" argument 
        /// used in saving and loading Spreadsheets.
        /// </summary>
        /// <param name="FilePath">A string representing the saved file location of "this"</param>
        /// <param name="isValid">Additional user defined Cell name restriction</param>
        /// <param name="normalize">User defined formating restriction of Cell names</param>
        /// <param name="version">Version name of "this"</param>
        public Spreadsheet(String FilePath, Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {
            //  IsValid is a user defined additional restriction,
            //      it has nothing to do with the current base restriction.
            this.IsValid = isValid;
            this.Normalize = normalize;
            this.Version = version;

            filePath = FilePath;

            //  Initialize components of "this".
            dependencyGraph = new DependencyGraph();  
            spreadsheet = new Dictionary<string, Cell>();    

            isChanged = false;  

            LoadSpreadSheetFromFile(filePath);
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        /// <returns>An IEnumberable containing all the names of non-empty Cells</returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            // Make return local.
            List<String> nonEmptyList = new List<String>();
            //  Try Get.
            Cell outCell;

            //If the spreadsheet is empty, return an empty IEnumerble.
            if (this.spreadsheet.Count == 0)
                return nonEmptyList;
            //  Else if the Spreadsheet is not empty...
            else
            {   //  Walk through every added name in "TheeSpreadsheet".
                foreach (String name in this.spreadsheet.Keys)
                {   //  Get cell corresponding to name.
                    this.spreadsheet.TryGetValue(name, out outCell);
                    //   If the key's value/Cell's contents does not equal an empty string...
                    if (!outCell.CellContents.Equals(""))
                        //   Add it to the list.
                        nonEmptyList.Add(name.Trim());
                }
            }// Return list with names.
            return nonEmptyList;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        /// <param name="name">The name of the queried Cell</param>
        public override object GetCellContents(string name)
        {   //  Try Get.
            Cell outCell;
            //  Null name Check
            if (ReferenceEquals(name, null))
                throw new InvalidNameException();
            //  Validate "name".
            if (!Regex.IsMatch(name, "^[a-z|A-Z]+[0-9]+$"))
                throw new InvalidNameException();

            //  Otherwise, try  get the contents of the  normalized named cell.
            if (this.spreadsheet.TryGetValue(this.Normalize(name), out outCell))
                return outCell.CellContents;

            //  Otherwise the name does not yet exist in the Spreadsheet,
            //          return an empty string.
            return "";
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name"> The name of the Cell whose contents will be set as double</param>
        /// <param name="number">The double that will be set as the contents of the named Cell
        /// </param>
        /// <returns>See above</returns>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            //  Make an ISet out of a HashSet.
            ISet<String> returnSet = new HashSet<String>();
            ISet<String> emptySet = new HashSet<String>();

            IEnumerable<String> getDependents = new HashSet<String>();

            // For valid input:
            //      "name" will go into the key portion of the dictionary
            //      cell comprised of "number" will go into value portion of dictionary.
            Cell doubleCell = new Cell(number, LookUpMethod);

            // overwrite in case of duplicate Key value
            this.spreadsheet[name] = doubleCell;

            //  Since we just added a contents that cannot be dependee,
            //      We will remove all of the dependents of the cell we just updated.
            this.dependencyGraph.ReplaceDependents(name, emptySet);

            //  If the dictionary only has one pair in it, return "emptySet". 
            if (this.spreadsheet.Count < 2)
                return emptySet;
            //  Else if there is more than one pair in "TheeDictionary"....
            else
            {   //  For every name contained in the return value for GetDirectDependents()....
                foreach (String cellName in GetCellsToRecalculate(name.Trim()))
                    //  Put "cellName" in "returnSet".
                    returnSet.Add(cellName.Trim());

                return returnSet;
            }
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name"> The name of the Cell whose contents will be set as a String</param>
        /// <param name="text">The String that will be set as the contents of the named Cell
        /// </param>
        /// <returns>See summary</returns>
        protected override ISet<string> SetCellContents(string name, string text)
        {
            //  Check for null "text".
            if (ReferenceEquals(text, null))
                throw new ArgumentNullException();  

            //  If name does not match new PS5 definition....
            if (!Regex.IsMatch(name, "^[a-z|A-Z]+[0-9]+$"))
                throw new InvalidNameException();   

            //  Local sets.
            ISet<String> returnSet = new HashSet<String>();
            ISet<String> emptySet = new HashSet<String>();
            //  Assigned dependents.
            IEnumerable<String> getDependents = new HashSet<String>();

            //******** NO CHANGE TO TESTS PASSED WITH THIS COMENTED OUT ****************
            //  If cell is set to an empty string.
            if (text == "")
            {
                this.spreadsheet.Remove(name);      
                this.dependencyGraph.RemoveDependency(name, text);
            }

            //  Cell member.
            Cell valueCell = new Cell(text, LookUpMethod);
            //  Add the pair to the dictionary, overwriting if necessary.
            this.spreadsheet[name] = valueCell;

            //******** NO CHANGE TO TESTS PASSED WITH THIS COMENTED OUT ****************
            //  Get dependents so we can make all the appropriate removals from the DG.
            //getDependents = GetDirectDependents(name);      //<-----------GDD

            //  Remove all of "name"s dependees.
            this.dependencyGraph.ReplaceDependents(name, emptySet);

            //  Return empty set if only one pair exists in dictionary member.
            if (this.spreadsheet.Count < 2)
                return emptySet;
            else
            {   //  Add all cell names dependent upon "name".
                foreach (String cellName in GetCellsToRecalculate(name.Trim()))
                    returnSet.Add(cellName.Trim());

                return returnSet;
            }
        }

        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name"> The name of the Cell whose contents will be set as a Formula</param>
        /// <param name="formula">The Formula that will be set as the contents of the named Cell
        /// </param>
        /// <returns>See summary</returns>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            //***********NO CHANGE TO NUMBER OF TESTS PASSED IF THESE VAR'S ARE COMMENTED OUT ****************
            //  Result of a formula.
            //double result = 0.0;
            //double outResult = 0.0;

            //Local set.
            IEnumerable<String> getReturnList = new List<String>();

            //  Save the old dependents
            IEnumerable<String> oldDependents = new HashSet<String>();
            // Assigned to GetDirectDependents.
            IEnumerable<String> getDependents = new HashSet<String>();

            ISet<String> theSetToBeReturned = new HashSet<String>();
            ISet<String> emptySet = new HashSet<String>();

            //  List will contain all the variables in "formula".
            List<String> variables = new List<String>();

            //  Cell that will be added to the dictionary.
            Cell formulaCell = new Cell(formula, LookUpMethod);

            //  Value of the cell that will be attained by the evaluate method.
            object cellValue = 0;

            //  Get a list of variables from "formula".
            variables = (List<String>)formula.GetVariables();

            foreach (String cellName in variables)
            {
                //  Add Dependency.
                this.dependencyGraph.AddDependency(name, cellName);

                //******** NO CHANGE TO TESTS PASSED WITH THIS COMENTED OUT ****************
                try
                {   //  If GetCellsToRecalculate throws CircularException
                    GetCellsToRecalculate(name);
                }
                catch (CircularException e)
                {
                    //Put the old dependents back.
                    this.dependencyGraph.ReplaceDependents(name, oldDependents);
                    //  Rethrow Exception
                    throw new CircularException();
                }
            }
            //  If no CircularException is thrown add the cell to the dictionary.
            this.spreadsheet[name] = formulaCell;

            //  Now get the set to return.
            getReturnList = GetCellsToRecalculate(name);
            //   Transfer.
            foreach (String changling in getReturnList)
                theSetToBeReturned.Add(changling);

            return theSetToBeReturned;
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and  C1
        /// </summary>
        /// <param name="name">The name of the queried Cell</param>
        /// <returns>See summary</returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            //  IEnumerable member.
            IEnumerable<String> returnIEnumerable = new List<String>();

            //Call DG's GetDependents here.
            returnIEnumerable = this.dependencyGraph.GetDependees(name);

            return returnIEnumerable;
        }

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get
            {
                return this.isChanged;
            }
            protected set
            {
                this.isChanged = value; 
            }
        }

        /// <summary>
        /// ********************* OBSOLETE ************************************************************************
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename">Filename representing th queried Spreadsheet</param>
        /// <returns>See summary</returns>
        public override string GetSavedVersion(string filename)
        {       //  Initialize Xml reader.
            System.Xml.XmlTextReader versionReader = new System.Xml.XmlTextReader(filename);

            //Will be assigned version from file.
            String fileVersion = "";

            try
            {
                while (versionReader.Read())
                {
                    if (versionReader.Name != "spreadsheet")
                        versionReader.Skip();
                    //  Attempt to locate version in file.
                    if (versionReader.MoveToAttribute("version"))
                    {

                        // versionReader.MoveToNextAttribute();

                        //     Assign file's version.
                        fileVersion = versionReader.GetAttribute("version");
                        break;
                    }
                    //  If no version information is found...
                    else
                    {
                        throw new SpreadsheetReadWriteException("Could not locate version information in file."); //UNCOVERED
                    }
                }
                return fileVersion;
            }// Catch any unexpected exceptions.
            catch (Exception e) //For now!!!
            {   // LINE BELOW UNCOVERED
                throw new SpreadsheetReadWriteException("An unexpected error occurred while attempting to retrieve file version information.");
            }
        }

        /// <summary>
        /// ************************************ OBSOLETE ***************************************************8
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>
        /// cell name goes here
        /// </name>
        /// <contents>
        /// cell contents goes here
        /// </contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename">The file name of the queried Spreadsheet</param>
        public override void Save(string filename)
        {
            double outParseDouble = 0;

            //  Use a specific set of settings to make file writing easier.
            XmlWriterSettings saveWriterSettings = new XmlWriterSettings();

            // This will omit information that is not supposed to be in the file
            saveWriterSettings.OmitXmlDeclaration = true;  
            saveWriterSettings.CloseOutput = true;
            //Use try block to handle any unexpected exceptions being thrown.
            try
            {
                //System.Threading.Thread.Sleep(100);
                //     Make a writer that uses the above specified settings and will save to a file named "filename".
                using (XmlWriter saveWriter = XmlWriter.Create(filename, saveWriterSettings))
                {
                    //  Begin the writing to file process.
                    saveWriter.WriteStartDocument();
                    //  Left half of opening spreadsheet tag.
                    saveWriter.WriteStartElement("spreadsheet");

                    //  Right half of opening tag with current version information.
                    saveWriter.WriteAttributeString("version", this.Version);

                    //saveWriter.WriteString("\n\n");

                    //      For every cell in the spreadsheet...
                    foreach (String cellName in this.spreadsheet.Keys)
                    {
                        //      Write opening cell tag.
                        saveWriter.WriteStartElement("cell");
                        //saveWriter.WriteString("\n");

                        //          write opening name tag.
                        saveWriter.WriteStartElement("name");
                        //saveWriter.WriteString("\n");

                        //              write current cell name
                        saveWriter.WriteString(cellName);
                        //saveWriter.WriteString("\n");

                        //          write closing name tag.
                        saveWriter.WriteEndElement();
                        //saveWriter.WriteString("\n");
                        //  Get the contents of the current named cell.
                        Cell currentContents = this.spreadsheet[cellName];

                        //          write opening contents tag.
                        saveWriter.WriteStartElement("contents");
                        //saveWriter.WriteString("\n");

                        //              write current cell contents.
                        //  if "currentContents" is a Formula...
                        if (currentContents.CellContents is Formula)
                        {
                            saveWriter.WriteString("=" + currentContents.CellContents.ToString());
                            //saveWriter.WriteString("\n");
                        }
                        // if current contents is a double..
                        //else if(currentContents is double)
                        else if (Double.TryParse(currentContents.CellContents.ToString(), out outParseDouble))
                        {                                                                               //Suspect
                            saveWriter.WriteString(currentContents.CellContents.ToString());
                            //saveWriter.WriteString("\n");
                        }
                        //  Else if current contents is a string
                        else
                        {
                            saveWriter.WriteString(currentContents.CellContents.ToString());//Suspect
                            //saveWriter.WriteString("\n");// Commenting out this and all the above 
                            //newlines was all that was required to get savetest8() to pass....
                        }
                        //          write closing contents tag.
                        saveWriter.WriteEndElement();
                        //saveWriter.WriteString("\n");

                        //      write closing cell tag.
                        saveWriter.WriteEndElement();
                        //saveWriter.WriteString("\n\n");
                    }
                    //  write spreadsheet closing tag.
                    saveWriter.WriteEndElement();
                    //saveWriter.WriteString("\n");

                    // End writing
                    saveWriter.WriteEndDocument();


                    saveWriter.Dispose();
                    saveWriter.Close();
                    //System.Threading.Thread.Sleep(100);

                }       //<---using

                //Spreadsheet has been saved, set changed member to false                
                this.isChanged = false;
            }
            catch (Exception e)
            {
                saveCount++;
                
                Save(filename + saveCount.ToString());

                //throw new SpreadsheetReadWriteException("An Error Occurred while trying to write to file.");
                //throw new SpreadsheetReadWriteException(e.Message);
                //return;
            }
            
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        /// <param name="name">The name of the queried Cell</param>
        /// <returns>See summary</returns>
        public override object GetCellValue(string name)
        {
            // If name is null or invalid.
            if (ReferenceEquals(name, null) || !Regex.IsMatch(name, "^[a-z|A-Z]+[0-9]+$"))
                throw new InvalidNameException();

            Cell outCell;

            // Check for a key corresponding to "name" in this dictionary.
            //      If the key does exist......
            if (this.spreadhsheet.TryGetValue(name, out outCell))
                return outCell.ValueOfCell;
            //if the key does not exist..
            else
                return "";
        }

        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name">The name of the Cell who's contents will be set as "content"</param>
        /// <param name="content">The contents that will be assigned to the named Cell</param>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            //  Exception checks.
            if (ReferenceEquals(content, null))
                throw new ArgumentNullException();
            if (ReferenceEquals(name, null) || !Regex.IsMatch(name, "^[a-z|A-Z]+[0-9]+$")) //changed from 1-9!
                throw new InvalidNameException();

            //  Make sure input passes IsValid check.
            if (!this.IsValid(name))
                throw new InvalidNameException();

            double outDouble = 0;
            //  Return value.
            ISet<String> returnSet = new HashSet<String>();

            //Attempt to parse "content" to a double.  
            if (Double.TryParse(content, out outDouble))
            {
                //  Call SCC(Double) on normalized "name" & "content" and return ISet.
                returnSet = this.SetCellContents(this.Normalize(name), outDouble);  

                // Set change member to true.                
                this.isChanged = true;

                Re_Evaluate(returnSet);

                return returnSet;
            }
            //  If "content" begins with "="
            if (content.StartsWith("="))
            {//     Attempt to make formula from content minus first character.
                try
                {//     Make a formula from contents, excluding the leading "=".
                    Formula contentFormula = new Formula(this.Normalize(content.Substring(1, content.Length - 1).Trim()));

                    //make sure "contentFormula" passes IsValid restriction
                    if (!this.IsValid(contentFormula.ToString()))//MIGHT WANT TO TRIM HERE???
                        throw new FormulaFormatException("Formula does not pass IsValid restriction.");

                    //  Check for CircularException.
                    try
                    {   //***** THERE IS NO CHANGE IN THE AMOUNT OF TESTS PASSED IF WE COMMENT THIS OUT *******
                        //GetCellsToRecalculate(this.SetCellContents(name, contentFormula));

                        // Set change member to true.                        
                        this.isChanged = true;
                        //  Call and return SCC on normalized "name" & "contentFormula".
                        returnSet = this.SetCellContents(this.Normalize(name), contentFormula);

                        //  Update.
                        Re_Evaluate(returnSet);

                        return returnSet;
                    }
                    catch (CircularException e)// CATCHING THIS IN BOTH THIS METHOD AND THE OTHER SCC METHODS MAY CAUSE PROBLEMS
                    {                                                               // WITH REVERSIONS
                        throw new CircularException();
                    }
                }
                catch (SpreadsheetUtilities.FormulaFormatException e)
                {
                    throw new SpreadsheetUtilities.FormulaFormatException(e.Message);
                }
            }
            //  Else, "content" is a plain old String, call and return SCC(text) on "name" and "content".
            else
            {
                // Set change member to true.              
                this.isChanged = true;

                returnSet = this.SetCellContents(this.Normalize(name), content);

                // ************************ NEW
                Re_Evaluate(returnSet);   

                return returnSet;
            }
        }

        /// <summary>
        /// Helper that re-evaluates a collection of cell's values once a cell's 
        /// value has changed within the spreadsheet.
        /// </summary>
        /// <param name="EvaluateCells"></param>
        private void Re_Evaluate(ISet<String> EvaluateCells)
        {
            foreach (string cellName in EvaluateCells)
            {
                Cell sccCell = new Cell(cellName, LookUpMethod);
                if (this.spreadsheet.TryGetValue(cellName, out sccCell))
                {
                    sccCell.CellEvaluate();
                }
            }
        }

        /// <summary>
        /// Method helps with looking variables in formulas.
        /// </summary>
        /// <param name="cellName"></param>
        public double LookUpMethod(String cellName)
        {
            object cellContents = GetCellValue(cellName);   

            //double outParse;
            if (cellContents is double)
                return (double)cellContents;

            else
                throw new ArgumentException();
        }

        // ******************************** OBSOLETE ****************************************************
        private void LoadSpreadSheetFromFile(string fileName)
        {
            // Will hold the current string being read.
            String currentString;
            //  Will hold a current Cell name.
            String currentCellName = "";
            //  Will hold the current Cell contents.
            String currentCellContents;
            //  Will hold the value of a parsed double.
            Double outDouble = 0;

            //  Check to see if the file exists.
            if (!File.Exists(this.filePath))
            {
                throw new SpreadsheetReadWriteException("File to be loaded could not be found.");
            }

            //System.Threading.Thread.Sleep(300);
            //  If file exists, read the file.
            using (XmlReader FileToBeLoaded = XmlReader.Create(fileName))
            {
                //  Catch any exceptions
                try
                {
                    //  While we can read a string in the file..
                    while (FileToBeLoaded.Read())
                    {//     If current string is an Xml start element...
                        if (FileToBeLoaded.IsStartElement())
                        {
                            //  We will switch between different cases depending on the current read string.
                            switch (FileToBeLoaded.Name)
                            {
                                //LINE BELOW UNCOVERED
                                case "version":
                                    //**************** ABSTRACT MEMBER SWAPP ***********************
                                    if (!this.Version.Equals(FileToBeLoaded.Value))
                                    {//     Throw exception of versions do not match.
                                        throw new SpreadsheetReadWriteException("Incorrect version detected."); //UNCOVERED
                                    }
                                    break;

                                //  The next tag in the file should be "cell".  
                                case "cell":
                                    //  Extract tag.
                                    currentString = FileToBeLoaded.Name; // this ensures that the encoutered tag, is the expected tag <cell>
                                    if (!currentString.Equals("cell"))  //LINE BELOW UNCOVERD
                                        throw new SpreadsheetReadWriteException("'cell' tag is missing or misplaced in Spreadsheet file.");
                                    break;

                                //  The next tag in the file should be "name".  
                                case "name":
                                    currentString = FileToBeLoaded.Name;
                                    if (!currentString.Equals("name"))           // LINE BELOW UNCOVERD
                                        throw new SpreadsheetReadWriteException("'name' tag is missing or misplaced in Spreadsheet file.");
                                    //  Extract Cell name from file.
                                    currentCellName = FileToBeLoaded.ReadElementString();   
                                    try
                                    {   //  Check to see if the extracted cell name is valid.
                                        //      Attempt to make a formula out of the extracted cell name.
                                        Formula extractedCellNameFormula = new Formula(currentCellName);
                                    }// If making of formula throws a FormulaFormatException...
                                    catch (FormulaFormatException e)    //UNCOVERED
                                    {
                                        throw new SpreadsheetReadWriteException("Invalid cell name detected in File."); //UNCOVERED
                                    }
                                    if (FileToBeLoaded.Name == "contents")  //The rest of this portion of the case was implemented to pass SaveTest7(), there is duplicate code here.
                                    {
                                        currentCellContents = FileToBeLoaded.ReadElementString();
                                        if (Double.TryParse(currentCellContents, out outDouble))
                                            //  Add it to the spreadsheet.
                                            this.SetContentsOfCell(currentCellName.Trim(), outDouble.ToString());
                                        //  If the extracted contents starts with "="...
                                        else if (currentCellContents.StartsWith("="))
                                        {
                                            //  Attempt to make a formula out of the extracted contents.
                                            try
                                            {//     Attempt to make a formula out of the contents.  //LINE BELOW UNCOVERED
                                                Formula currentContentsFormula = new Formula(currentCellContents.Substring(1, currentCellContents.Length - 1));
                                                //  Add it to the spreadsheet.
                                                this.SetContentsOfCell(currentCellName, currentCellContents);   //UNCOVERED                                                    
                                            }// If an invalid formula is detected...
                                            catch (FormulaFormatException e)    //UNCOVERED
                                            {// Throw exception with message.   //LINE BELOW UNCOVERED.
                                                throw new SpreadsheetReadWriteException("An invalid Formula was detected in the file.");
                                            }
                                        }
                                        //  Else the contents must be a string...
                                        else
                                        {
                                            //      Make sure the contents is not an empty string.
                                            if (!currentCellContents.Equals(String.Empty))
                                            {//     Add the cell to the spreadsheet, with unexpected newlines trimmed.
                                                this.SetContentsOfCell(currentCellName.Trim(), currentCellContents.Trim());  //Newlines are at front and back of contents here!!!
                                            }
                                        }
                                        break;
                                    }
                                    break;
                                //    The next tag in the file should "contents". 
                                case "contents":
                                    currentString = FileToBeLoaded.Name;
                                    if (!currentString.Equals("contents"))       //LINE BELOW UNCOVERED
                                        throw new SpreadsheetReadWriteException("'contents' tag is missing or misplaced in Spreadsheet file.");
                                    currentCellContents = FileToBeLoaded.ReadElementString();
                                    //  If the extracted contents can be parsed to a double...
                                    if (Double.TryParse(currentCellContents, out outDouble))
                                        //  Add it to the spreadsheet.
                                        this.SetContentsOfCell(currentCellName.Trim(), outDouble.ToString()); 
                                    //  If the extracted contents starts with "="...
                                    else if (currentCellContents.StartsWith("="))
                                    {
                                        //  Attempt to make a formula out of the extracted contents.
                                        try
                                        {//     Attempt to make a formula out of the contents.  //LINE BELOW UNCOVERED
                                            Formula currentContentsFormula = new Formula(currentCellContents.Substring(1, currentCellContents.Length - 1));
                                            //  Add it to the spreadsheet.
                                            this.SetContentsOfCell(currentCellName, currentCellContents);   //UNCOVERED
                                        }// If an invalid formula is detected...
                                        catch (FormulaFormatException e)    //UNCOVERED
                                        {// Throw exception with message.   //LINE BELOW UNCOVERED.
                                            throw new SpreadsheetReadWriteException("An invalid Formula was detected in the file.");
                                        }
                                    }
                                    //  Else the contents must be a string...
                                    else
                                    {
                                        //      Make sure the contents is not an empty string.
                                        if (!currentCellContents.Equals(String.Empty))
                                        {//     Add the cell to the spreadsheet, with unexpected newlines trimmed.
                                            this.SetContentsOfCell(currentCellName.Trim(), currentCellContents.Trim());  //Newlines are at front and back of contents here!!!
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }// Catch any other exceptions caused by reading the file.
                catch (Exception e)
                {
                    //throw new SpreadsheetReadWriteException("An unexpected error occurred while trying to load the file.");
                    throw new SpreadsheetReadWriteException(e.Message);
                }
                //  If versions don't match.
                if (this.Version != this.GetSavedVersion(filePath))  //Suspect.
                {
                    throw new SpreadsheetReadWriteException("Loaded file did not have the correct version.");
                }
                //FileToBeLoaded.Dispose();
                // FileToBeLoaded.Close();
            }  //<-- using

        }
        //****************************************** New for cs3505 ***************************************************************
        /// <summary>
        /// Return the current version of the Spreadsheet.
        /// </summary>
        /// <returns></returns>
        public string getVersion()
        {
            return this.Version;
        }

        //****************************************** New for cs3505 ***************************************************************
        /// <summary>
        /// Set the current version of the Spreadsheet.
        /// </summary>
        /// <param name="updatedVersion"></param>
        public void setVersion(string updatedVersion)
        {
            this.Version = updatedVersion;
        }

    }   //<--   end of SpreadSheet Class.
}
