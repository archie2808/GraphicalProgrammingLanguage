using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class ScriptManager
    {
        private string[] scriptLines;

        public ScriptManager(string script)
        {
            scriptLines = script.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string GetCommandFromLine(int line)
        {
            if (line >= 0 && line < scriptLines.Length)
            {
                return scriptLines[line];
            }
            else
            {
                // Handle the case where the line number is invalid
                return null;
            }
        }
    }
}
