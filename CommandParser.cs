using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApp1
{
    /// <summary>
    /// class for parsing commands
    /// </summary>
    class CommandParser
    {
        public CommandParser()
        {

        }
        /// <summary>
        /// add parameters for null command, implement case for a draw command
        /// </summary>      
        /// <param name="command"></param>
        public void ExecuteCommand(string command)
        {
            command = command.Trim();
            if (string.IsNullOrEmpty(command))
            {
                MessageBox.Show("no command to execute");
                return;
            }

            //split the command into parts
            string[] commandParts = command.Split(' ');
            string action = commandParts[0].ToLower();

            switch (action)
            {
                case "draw":
                    DrawCommand(commandParts);
                    break;

                default:
                    MessageBox.Show($"unknown command: {action}"); 
                    break;
            }
        }
      

    }

}

   

