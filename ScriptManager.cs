using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class ScriptManager
    {
        private List<string> scriptLines;
        public int currentLineNumber;

        public ScriptManager()
        {
            scriptLines = new List<string>();
        }

       

        public string GetLine(int lineNumber)
        {
            currentLineNumber = lineNumber; // Update current line number whenever a line is fetched
            if (lineNumber >= 0 && lineNumber < scriptLines.Count)
            {
                return scriptLines[lineNumber];
            }
            return null;
        }

        public int GetCurrentLineNumber()
        {
            return currentLineNumber;
        }

        public int GetTotalLines()
        {
            return scriptLines.Count;
        }
    }
}
    