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
    /// The <c>CommandParser</c> class is responsible for interpreting and executing user commands. 
    /// </summary>
    class CommandParser 
    {
        private Point penPosition;
        private Bitmap drawingSurface;
        private readonly TextBox outputTextBox;

        /// <summary>
        /// Initializes a new instance of the <c>CommandParser</c> class.
        /// </summary>
        /// <param name="output">The textbox control where output messages are displayed</param>
        /// <param name="surface">The bitmap surface on which drawing commands are executed</param>
        /// <remarks>
        /// The constructor sets up the initial state of the Parser, including setting the initial pen position
        /// and associating the output TextBox and drawing Bitmap
        /// </remarks>
        public CommandParser(TextBox output, Bitmap surface)
        {
            penPosition = new Point(0, 0);
            outputTextBox = output;
            drawingSurface = surface; 
        }

        /// <summary>
        /// Executes user commands based on provided inputs
        /// </summary>      
        /// <param name="command">The command string to be parsed and executed</param>
        /// <remarks>
        /// This method processes the command string, identifies the type of command. (e.g. moveto), 
        /// and executes the corresponding action. It also handles invalid commands.
        /// </remarks>
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
                case "moveto":
                    MoveToCommand(commandParts);
                    break;
                    

                default:
                    outputTextBox.AppendText($"Unknown command: {action}\n"); 
                    break;
            }
            
        }
        
        /// <summary>
        /// Processes the 'moveto' command, updating the pen position and drawing on the Bitmap
        /// </summary>
        /// <param name="commandParts">The parameters of the commanf, such as the co-oridinates</param>
        /// <remarks>
        /// This method updates the pen position based on the coordinates provided in the command.
        /// it then calls on <c>DrawOnBitmap</c> to reflect this change on the drwaing surface.
        /// </remarks>
        private void MoveToCommand(string[] commandParts)
        {
            if (commandParts.Length == 3 && int.TryParse(commandParts[1], out int x) && int.TryParse(commandParts[2], out int y)) 
            {
                penPosition = new Point(x, y);
                DrawOnBitmap();
                outputTextBox.AppendText($"Pen moved to ({x}, {y}). \n");
            }
            else
            {
                outputTextBox.AppendText("invalid move to command. \n");
            }
        }
        /// <summary>
        /// Draws on bitmaps surface at the current pen position
        /// </summary>
        /// <remarks>
        /// This method is called after updating the pen position to visually represent the pens's 
        /// new location on the bitmap. It currently draws a small red circle at the pen position.
        /// </remarks>
        private void DrawOnBitmap()
        {
            using (Graphics g = Graphics.FromImage(drawingSurface))
            {
                g.FillEllipse(Brushes.Red, penPosition.X - 2, penPosition.Y - 2, 4, 4);
            }
            
        }
      

    }

}

   

