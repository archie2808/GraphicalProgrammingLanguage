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
    /// <remarks>
    /// the property provides access tothe current location of the pen, allowing external classes
    /// to query its position
    /// </remarks>
    public class CommandParser
    {
        private Point penPosition;
        private readonly Bitmap drawingSurface;
        private readonly TextBox outputTextBox;
        private readonly DrawingManager drawingManager;

        /// <summary>
        /// Initializes a new instance of the <c>CommandParser</c> class.
        /// </summary>
        /// <param name="output">The textbox control 1where output messages are displayed</param>
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
            drawingManager = new DrawingManager(surface);
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

                case "rectangle":
                    RectangleCommand(commandParts);
                    break;

                case "circle":
                    CircleCommand(commandParts);
                    break;

                case "triangle":
                    TriangleCommand(commandParts);
                    break;

                case "colour":
                    ColorCommand(commandParts);
                    break;

                case "reset":
                    ResetCommand();
                    break;

                default:
                    throw new InvalidOperationException($"unknown command: {action}");
                
            }

        }

        /// <summary>
        /// Processes the 'colour' command to change the pens drawing colour
        /// </summary>
        /// <param name="commandParts"></param>
        /// <remarks>
        /// This method interprets the 'colour' command, extracts the colour name, and changes the pen colour in the drawing manager.
        /// If the command is invalid (e.g., incorrect number of arguments or unrecognized colour name), an error message is displayed.
        /// </remarks>
        private void ColorCommand(string[] commandParts)
        {
            if (commandParts.Length == 2)
            {
                string colorName = commandParts[1];
                drawingManager.ChangePenColor(colorName);
                outputTextBox.AppendText($"Pen colour changed to {colorName}\n");
            }

            else
            {
                throw new ArgumentException("Invalid colour command, expected formal: 'colour [colourname]");
            }
        }

        /// <summary>
        /// Process the triangle command to draw a triangle on the drawing surface.
        /// </summary>
        /// <param name="commandParts"></param>
        /// <remarks>
        /// This methods interprets the 'triangle' command, extracts the base co-ordinates and length, and 
        /// instructs the drawing manager to draw a triangle. Throws excpetion if triangle command is not inputted correctly.
        /// </remarks>
        private void TriangleCommand(string[] commandParts)
        {
            if (commandParts.Length == 3 && int.TryParse(commandParts[1], out int x) && int.TryParse(commandParts[2], out int y))
            {
                Point startVertex = new Point(x, y);
                int sideLength = 100;

                drawingManager.DrawTriangle(startVertex, sideLength);
                outputTextBox.AppendText($"Triangle drawn with starting vertex at ({x} , {y}).\n");
            }

            else
            {
                throw new ArgumentException("Invalid 'triangle' command. Expected Format: triangle x y");
            }
        }

        /// <summary>
        /// Processes the circle command to draw a circle on the drawing surface.
        /// </summary>
        /// <param name="commandParts"></param>
        /// <remarks>
        /// The method interprets the circle command, extracts the radius and calls the DrawCircle method 
        /// to draw the circl on the drawing surface.
        /// </remarks>
        private void CircleCommand(string[] commandParts)
        {
            if (commandParts.Length == 2 && int.TryParse(commandParts[1], out int radius))
            {
                drawingManager.DrawCircle(penPosition, radius);
                outputTextBox.AppendText($"Circle drawn with radius {radius}.\n");
            }
            else
            {
                throw new ArgumentException("Invalid 'circle' command. Expected Format: 'circle radius'");
            }
        }

        /// <summary>
        /// Processes the rectangle command to draw a rectangle onto drawing surface
        /// </summary>
        /// <param name="commandParts"></param>
        /// <remarks>
        /// The method interprets the rectangle command, extracts the width and height parameters, 
        /// and then calls the drawing manager to a rectangle. The rectangle is drawn with its top 
        /// left corner at the current pen position
        /// </remarks>
        private void RectangleCommand(string[] commandParts)
        { 
            if (commandParts.Length == 3 && int.TryParse(commandParts[1], out int width) && int.TryParse(commandParts[2], out int height))
            {
                drawingManager.DrawRectangle(penPosition, width, height);
                outputTextBox.AppendText($"Rectangle drawn at ({penPosition.X}, {penPosition.Y}) with width {width} and height {height}");
            }
            else
            {
                throw new ArgumentException("invalid 'rectangle' command. Expected format: recatangle width height");
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
                drawingManager.DrawOnBitmap();
                outputTextBox.AppendText($"Pen moved to ({x}, {y}). \n");
            }
            else
            {
                throw new ArgumentException("Invalid 'moveto' command. Expected Format: 'moveto x y'");
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
                drawingManager.DrawLine(penPosition, newPenPosition);
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

       
      

    }

}

   

