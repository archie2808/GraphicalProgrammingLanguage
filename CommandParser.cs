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
       
        private Bitmap drawingSurface;
        private TextBox outputTextBox;
        private DrawingManager drawingManager;
        private VariableManager variableManager;
        private Point penPosition;

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
            variableManager = vm;
        }

        //private bool isInsideIfStatement = false; //Flag to indicate if we are currently processing commands inside of an if block

        /*public void IfStatementExecution(string command)
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
        }*/
        /// <summary>
        /// Executes user commands based on provided inputs
        /// </summary>      
        /// <param name="command">The command string to be parsed and executed</param>
        /// <remarks>
        /// This method processes the command string, identifies the type of command. (e.g. moveto), 
        /// and executes the corresponding action. It also handles invalid commands.
        /// </remarks>
        public void ExecuteCommand(string commandString)
        { 

            commandString = commandString.Trim();
            if (string.IsNullOrEmpty(commandString))
            {
                throw new InvalidOperationException("no command to execute");
                
            }

            if (commandString == "reset")
            {
                ResetCommand();
                return; 
            }
            
            if (commandString.Contains("="))
            {
                variableManager.ProcessVariableAssignment(commandString);
                return;
            }


            string[] commandParts = commandString.Split(' ');
            string action = commandParts[0].ToLower();

            for (int i = 1; i < commandParts.Length; i++)
            {
                int resolvedValue = CommandFactory.ResolveArgumentToInteger(commandParts[i], variableManager);
                commandParts[i] = resolvedValue.ToString();  // Convert the int back to a string
            }

            ICommand command = CommandFactory.CreateCommand( action, commandParts, drawingManager, variableManager, penPosition);
            command.Execute();

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
    


   

