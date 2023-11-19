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
    public class CommandParser
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
                throw new InvalidOperationException("no command to execute");
                
            }
             

            string[] commandParts = command.Split(' ');
            string action = commandParts[0].ToLower();

            switch (action)
            {
                case "moveto":
                    MoveToCommand(commandParts);
                    break;

                case "drawto":
                    DrawToCommand(commandParts);
                    break;

                case "reset":
                    ResetCommand();
                    break;

                default:
                    throw new InvalidOperationException($"unknown command: {action}");
                
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
                throw new ArgumentException("Invlaid 'moveto' command. Expected Format: 'moveto x y'");
            }
        }

        /// <summary>
        /// This method processes the draw to command, drawing a line from the current pen position to the specified co-ordinates
        /// </summary>
        /// <param name="commandParts">The parts of the command, including the drawto keyword and the x and y co-ordinates</param>
        /// <remarks>
        /// This  method interprets the 'drawto' command, extracts the destination coordinates, and draws a line from the current pen position to these coordinates.
        /// It updates the pen position to the new location after drawing the line.
        /// If the command is invalid (e.g., incorrect number of arguments or non-numeric coordinates), an error message is displayed.
        /// </remarks>
        private void DrawToCommand(string[] commandParts)
        {
            if (commandParts.Length == 3 && int.TryParse(commandParts[1], out int x) && int.TryParse(commandParts[2], out int y))
            {
                Point newPenPosition = new Point(x, y);
                DrawLine(penPosition, newPenPosition);
                penPosition = newPenPosition;

                outputTextBox.AppendText($"Line drawn to ({x}, {y})\n");
            }
            else
            {
                throw new ArgumentException("Invlaid 'drawto' command. Expected Format: 'drawto x y'");
            }
        }

        /// <summary>
        /// Resets the pen position to the top left corner of the drawing surface.
        /// </summary>
        /// <remarks>
        /// This method sets the pen position back to the origin (0,0).
        /// it checks if the drawing surface is available before resetting the pen position, if not 
        /// and invalid operation exception is thrown
        /// </remarks>
        private void ResetCommand()
        {
            if (drawingSurface == null)
            {
                throw new InvalidOperationException("Drawing Surface not available");
                
            }

            penPosition = new Point(0, 0);
            outputTextBox.AppendText("Pen position reset to top-left corner.\n");
        }


        /// <summary>
        /// Draws a line on the drawing surface from a specified start point to an end point
        /// </summary>
        /// <param name="start">the starting point of a line</param>
        /// <param name="end">the ending point of a line</param>
        /// <remarks>
        /// This method used a graphics object from the Bitmap to draw a line. 
        /// </remarks>
        private void DrawLine(Point start, Point end)
        {
            using (Graphics g = Graphics.FromImage(drawingSurface))
            {
                g.DrawLine(Pens.Black, start, end);
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

   

