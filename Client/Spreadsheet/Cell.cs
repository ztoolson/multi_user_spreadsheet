// Zach Toolson, Michael Call, Mike Fleming, Mark Mouritsen

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;
using FormulaEvaluator;

namespace SS
{
    /// <summary>
    /// This class defines the Cell type.
    /// </summary>
    class Cell
    {
        // Member which will contain the contents of a cell
        public object CellContents;
        public object CellValue;
        public Func<String, double> cellLookUp;

        /// <summary>
        /// Make a Cell object.
        /// </summary>
        /// <param name="cellContent">The contents which will be contained in the cell</param>
        public Cell(object cellContent, Func<string, double> LookUp)
        {
            CellContents = cellContent;
            cellLookUp = LookUp;
            CellEvaluate();
        }

        internal void CellEvaluate()
        {
            //if "cellcontent" is  not a formula
            if (!(CellContents is Formula))
            {//  
                CellValue = CellContents;
            }
            //Else, if "cellContent" is a formula.
            else
            {
                Formula EvaluateFormula = (Formula)CellContents;

                //  Assign evaluation of formula to "ContentsOfCell".
                ValueOfCell = EvaluateFormula.Evaluate(cellLookUp);
            }
        }

        /// <summary>
        /// Getter and setter for Cell object.
        /// </summary>
        public object ContentsOfCell
        {
            get { return CellContents; } 
            set { CellContents = value; }
        }

        public object ValueOfCell
        {
            get { return CellValue; }
            set { CellValue = value; }
        }
    }
}
