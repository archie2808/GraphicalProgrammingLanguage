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
        private TextBox outputTextBox;
        private DrawingManager drawingManager;
        private VariableManager variableManager;
        private IfStatementManager ifStatementManager;

        /// <summary>
        /// Gets the current position of the pen at the drawing surface
        /// </summary>
        /// <remarks>
        /// the property provides access to the current location of the pen, allowing external classes
        /// to query its position
        /// </remarks>
        public Point PenPosition
        {
            get {return penPosition; }
            
        }

        /// <summary>
        /// Updates the drawing surface with a new Bitmap. 
        /// </summary>
        /// <param name="newSurface">The new Bitmap to be set as drawing surface</param>
        /// <remarks>
        /// This method replaces the current drawing surface with the provided bitmap image. It is paticuarly
        /// important in maintaining properfunctionality of the clear command. The Method delegates the
        /// Update operation to the drawing manager class
        /// </remarks>
        public void UpdateDrawingSurface(Bitmap newSurface)
        {
            drawingManager.UpdateDrawingSurface(newSurface);
        }
     
        /// <summary>
        /// Initializes a new instance of the <c>CommandParser</c> class.
        /// </summary>
        /// <param name="output">The textbox control 1where output messages are displayed</param>
        /// <param name="surface">The bitmap surface on which drawing commands are executed</param>
        /// <remarks>
        /// The constructor sets up the initial state of the Parser, including setting the initial pen position
        /// and associating the output TextBox and drawing Bitmap
        /// </remarks>
        public CommandParser(TextBox output, Bitmap surface, VariableManager vm)
        {
            penPosition = new Point(0, 0);
            outputTextBox = output;
            drawingSurface = surface;
            drawingManager = new DrawingManager(surface);
            variableManager = new VariableManager();
            ifStatementManager = new IfStatementManager();
        }

        private bool isInsideIfStatement = false; //Flag to indicate if we are currently processing commands inside of an if block

        public void IfStatementExecution(string command)
        {
            //Check if the command is the start of an if statement
            if (command.StartsWith("if"))
            {
                //Set the flag to true upon entering if statement
                isInsideIfStatement = true;
                //Delegation of logic to IfStatementManager
                ifStatementManager.StartIfStatement(command);
            }

            //check if the command signifies the end of the if statement
            else if (command == "endif")
            {
                //set flag to false
                isInsideIfStatement = false;
                //delegation of logic to IfStatementManager
                ifStatementManager.EndIfStatement(this);
            }

            //check if we are inside if statement block
            else if (isInsideIfStatement)
            {
                //Add the command to the current if statement block in IfStatementManager
                ifStatementManager.AddCommand(command);
            }

            else
            {
                ExecuteCommand(command);
            }
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

            
            if (command.Contains("="))
            {
                variableManager.ProcessVariableAssignment(command);
                return;
            }


            string[] commandParts = command.Split(' ');
            string action = commandParts[0].ToLower();

            for (int i = 1; i < commandParts.Length; i++)
            {
                int resolvedValue = ResolveArgumentToInteger(commandParts[i]);
                commandParts[i] = resolvedValue.ToString();  // Convert the int back to a string
            }

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
                    ColourCommand(commandParts);
                    break;

                case "reset":
                    ResetCommand();
                    break;

               

                default:
                    throw new InvalidOperationException($"unknown command: {action}");
                
            }

        }
        /// <summary>
        /// Resolves a command argument to an integer value
        /// </summary>
        /// <param name="arg">The command argument, which can be a variable name or a direct integer</param>
        /// <remarks>
        /// The method checks if the provided argument is a defined variable in the variable manager class, if so it retrieves the variables value.
        /// if the argument is not a variable, it attemps to parse the argument as a direct integer. If neither condition is met, an exception is thrown. 
        /// </remarks>
        public int ResolveArgumentToInteger(string arg)
        {
            if (variableManager.IsVariableDefined(arg))
            {
                return variableManager.GetVariable(arg);
            }
            else if (int.TryParse(arg, out int value))
            {
                return value; 
            }
            else
            {
                throw new ArgumentException($"Invalid argument {arg} for command");
            }
        }

        /// <summary>
        /// Executes a series of commands provided in a script format
        /// </summary>
        /// <param name="script">The script containing the commands to be executed</param>
        /// <remarks>
        /// The method processes each line of the script as an individual command.
        /// It trims any whitespace from each command before execution. 
        /// If a command causes an exception, a Messagebox is displayed with error details. 
        /// </remarks>
        public void ExecuteScript(string script)
        {
            string[] commands = script.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string command in commands)
            {
                try
                {
                    
                    string trimmedCommand = command.Trim();

                    
                    if (!string.IsNullOrEmpty(trimmedCommand))
                    {
                        ExecuteCommand(trimmedCommand);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"error {ex}");
                    break;
                }
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
        private void ColourCommand(string[] commandParts)
        {
            if (commandParts.Length == 2)
            {
                string colourName = commandParts[1];
                drawingManager.ChangePenColor(colourName);
                outputTextBox.AppendText($"Pen colour changed to {colourName}\n");
            }

            else
            {
                throw new ArgumentException("Invalid colour command, expected format: 'colour [colourname]");
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
            if (commandParts.Length < 3)
            {
                throw new ArgumentException("Invalid 'triangle' command. Expected Format: triangle x y");
            }

            int x = ResolveArgumentToInteger(commandParts[1]);
            int y = ResolveArgumentToInteger(commandParts[2]);
            int sideLength = ResolveArgumentToInteger(commandParts[3]);

            Point startVertex = new Point(x, y);
           

            drawingManager.DrawTriangle(startVertex, sideLength);
            outputTextBox.AppendText($"Triangle drawn with starting vertex at ({x}, {y}).\n");
        }

        /// <summary>
        /// Processes the circle command to draw a circle on the drawing surface.
        /// </summary>
        /// <param name="commandParts"></param>
        /// <remarks>
        /// The method interprets the circle command, extracts the radius and calls the DrawCircle method 
        /// to draw the circle on the drawing surface.
        /// </remarks>
        private void CircleCommand(string[] commandParts)
        {
            if (commandParts.Length < 2)
            {
                throw new ArgumentException("Insufficient arguments for 'circle' command.");
            }

            int radius = ResolveArgumentToInteger(commandParts[1]);

            drawingManager.DrawCircle(penPosition, radius);
            outputTextBox.AppendText($"Circle drawn with radius {radius}.\n");
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
            if (commandParts.Length < 3)
            {
                throw new ArgumentException("Insufficient arguments for 'rectangle' command.");
            }

            int width = ResolveArgumentToInteger(commandParts[1]);
            int height = ResolveArgumentToInteger(commandParts[2]);

            drawingManager.DrawRectangle(penPosition, width, height);
            outputTextBox.AppendText($"Rectangle drawn at ({penPosition.X}, {penPosition.Y}) with width {width} and height {height}\n");
        }
       
        /// <summary>
        /// Processes the 'moveto' command, updating the pen position and drawing on the Bitmap
        /// </summary>
        /// <param name="commandParts">The parameters of the commanf, such as the co-oridinates</param>
        /// <remarks>
        /// This method updates the pen position based on the coordinates provided in the command.
        /// 
        /// </remarks>
        private void MoveToCommand(string[] commandParts)
        {
            if (commandParts.Length < 3)
            {
                throw new ArgumentException("Insufficient arguments for 'moveto' command.");
            }

            int x = ResolveArgumentToInteger(commandParts[1]);
            int y = ResolveArgumentToInteger(commandParts[2]);

            penPosition = new Point(x, y);
            outputTextBox.AppendText($"Pen moved to ({x}, {y}). \n");
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
            if (commandParts.Length < 3)
            {
                throw new ArgumentException("Insufficient arguments for 'drawto' command.");
            }

            int x = ResolveArgumentToInteger(commandParts[1]);
            int y = ResolveArgumentToInteger(commandParts[2]);

            
            Point newPenPosition = new Point(x, y);
            drawingManager.DrawLine(penPosition, newPenPosition);
            penPosition = newPenPosition; 

            outputTextBox.AppendText($"Line drawn to ({x}, {y}).\n");
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
    


   

