using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApp1
{
    public delegate void UpdatePenPositionDelegate(Point newPosition);
    /// <summary>
    /// The <c>CommandParser</c> class is responsible for interpreting and executing user commands. 
    /// </summary>
    public class CommandParser
    {
       
        private Bitmap drawingSurface;
        private TextBox outputTextBox;
        private MethodManager methodManager;
        private LoopManager loopManager;
        private DrawingManager drawingManager;
        private IfStatementManager ifStatementManager;
        private VariableManager variableManager;
        private Point penPosition;
        private SyntaxChecker syntaxChecker;
        
        private int currentLineNumber;
        
        

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
        /// Initializes a new instance of the <c>CommandParser</c> class, setting up essential components for command parsing and execution.
        /// </summary>
        /// <param name="output">The TextBox control where output messages are displayed.</param>
        /// <param name="surface">The Bitmap surface on which drawing commands are executed.</param>
        /// <param name="vm">The variable manager for managing script variables.</param>
        /// <param name="ifStatementManager">The if statement manager for handling conditional commands.</param>
        public CommandParser(TextBox output, Bitmap surface, VariableManager vm, IfStatementManager ifStatementManager, LoopManager loopManager)
        {
            penPosition = new Point(0, 0);
            outputTextBox = output;
            drawingSurface = surface;
            drawingManager = new DrawingManager(surface);
            variableManager = vm;
            
            this.methodManager = new MethodManager(variableManager);
            this.ifStatementManager = ifStatementManager;
            this.loopManager = loopManager;
            UpdatePenPositionAction = (newPosition) => { penPosition = newPosition; };
            syntaxChecker = new SyntaxChecker(variableManager);
        }

        //Flag to indicate if we are currently processing commands inside of an if block
        private bool isInsideIfStatement = false;

        /// <summary>
        /// Executes conditional logic based on 'if' statement syntax. This method manages the evaluation of conditions and 
        /// the execution of command blocks depending on the outcome of the condition.
        /// </summary>
        /// <param name="command">The command string that may contain an 'if' statement and its associated logic.</param>
        /// <remarks>
        /// This method processes 'if', 'endif', and other commands within the scope of an if statement. It delegates to 
        /// IfStatementManager for handling the condition logic and the execution of commands within the if statement block.
        /// </remarks>
        public void IfStatementExecution(string command)
        {
            try
            { 
                
                if (command.StartsWith("if"))
                {
                    
                    isInsideIfStatement = true;
                    
                    ifStatementManager.StartIfStatement(command);
                }

                
                else if (command == "endif")
                {
                     
                    isInsideIfStatement = false;
                    
                    ifStatementManager.EndIfStatement(this);
                }

                
                else if (isInsideIfStatement)
                {
                    
                    ifStatementManager.AddCommand(command);
                }

                else
                {
                    ExecuteCommand(command);
                }
            }

            catch (SyntaxException ex)
            {
                outputTextBox.AppendText($"Error Processing command: {ex.Message}\n");
            }
        }
        /// <summary>
        /// Parses and executes a given command string. This method identifies the type of command (e.g., drawing command, variable assignment, 
        /// control flow command) and executes it accordingly.
        /// </summary>
        /// <param name="commandString">The command string to be parsed and executed.</param>
        /// <remarks>
        /// This method is the central hub for executing all types of commands. It includes logic to handle different command types 
        /// and delegates to specific methods or classes for detailed execution.
        /// </remarks>
        public void ExecuteCommand(string commandString)
        {
            try
            {
                Console.WriteLine($"executing command: {commandString}");

                commandString = commandString.Trim();
                if (string.IsNullOrEmpty(commandString))
                {
                    throw new InvalidOperationException("no command to execute");
                }
                // Increment line number for each command
                currentLineNumber++;

                // Handling different types of commands...
                if (commandString.StartsWith("method"))
                {
                    var (methodName, parameters) = ExtractMethodNameAndParameters(commandString);
                    methodManager.DefineMethod(methodName, currentLineNumber, parameters);
                }
                else if (commandString.StartsWith("endmethod"))
                {
                    var methodName = ExtractMethodName(commandString);
                    methodManager.EndMethodDefinition(methodName, currentLineNumber);
                }
                else if (commandString.StartsWith("call"))
                {
                    var (methodName, arguments) = ExtractMethodCallDetails(commandString);
                    methodManager.CallMethod(methodName, arguments);
                }

                else if (commandString.StartsWith("while"))
                {
                    loopManager.StartLoop(commandString);
                }
                else if (commandString.Trim().ToLower() == "endwhile")
                {
                    loopManager.EndLoop();
                }

                else if (loopManager.IsLoopActive)
                {
                    loopManager.AddCommandToLoop(commandString);
                }

                else
                {


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


                    if (commandString.StartsWith("if") || commandString == "endif" || isInsideIfStatement)
                    {
                        IfStatementExecution(commandString);
                        return;
                    }

                    ExecuteSingleCommand(commandString);
                }
            }
            catch (SyntaxException ex)
            {
                outputTextBox.AppendText($"Syntax error: {ex.Message}\n");
                
            }
            catch (Exception ex) 
            {
                outputTextBox.AppendText($"Error executing command: {ex.Message}\n");
                
            }
        }

        private (string methodName, string[] parameters) ExtractMethodNameAndParameters(string commandString)
        {
            
            var parts = commandString.Split(new char[] { ' ' }, 2);
            var methodName = parts[1].Split(new char[] { ' ' }, 2)[0].Trim();
            var parametersPart = parts[1].Substring(methodName.Length).Trim();
            var parameters = parametersPart.Split(',')
                                           .Select(param => param.Trim())
                                           .Where(param => !string.IsNullOrEmpty(param))
                                           .ToArray();
            return (methodName, parameters);
        }

        private string ExtractMethodName(string commandString)
        {
            
            var parts = commandString.Split(' ');
            return parts[1].Trim();
        }

        private (string methodName, string[] arguments) ExtractMethodCallDetails(string commandString)
        {
            
            var parts = commandString.Split(new char[] { ' ' }, 2);
            var methodName = parts[1].Split(new char[] { ' ' }, 2)[0].Trim();
            var argumentsPart = parts[1].Substring(methodName.Length).Trim();
            var arguments = argumentsPart.Split(',')
                                         .Select(arg => arg.Trim())
                                         .Where(arg => !string.IsNullOrEmpty(arg))
                                         .ToArray();
            return (methodName, arguments);
        }



        /// <summary>
        /// Executes a single, isolated command, typically a drawing command. This method is focused on parsing and executing commands 
        /// that involve drawing operations.
        /// </summary>
        /// <param name="commandString">The command string representing a single drawing action.</param>
        /// <remarks>
        /// This method is primarily used for executing commands related to drawing, such as 'moveto', 'drawto', etc. It relies on the 
        /// DrawingManager for the actual drawing operations.
        /// </remarks>
        public void ExecuteSingleCommand(string commandString)
        {
            try
            {
                string[] commandParts = commandString.Split(' ');
                string action = commandParts[0].ToLower();
                string[] arguments = commandParts.Skip(1).ToArray();

                if (action != "colour")
                {
                    for (int i = 1; i < commandParts.Length; i++)
                    {
                        int resolvedValue = CommandFactory.ResolveArgumentToInteger(commandParts[i], variableManager);
                        commandParts[i] = resolvedValue.ToString();  // Convert the int back to a string
                    }
                }
                ICommand command = CommandFactory.CreateCommand(action, arguments, drawingManager, variableManager, penPosition, UpdatePenPositionAction);
                command.Execute();
            }
            catch (ArgumentException ex)
            {
                outputTextBox.AppendText($"Argument error: {ex.Message}\n");
               
            }
            catch (Exception ex)
            {
                outputTextBox.AppendText($"Error during command execution: {ex.Message}\n");
                
            }


        }
  
        public void ExecuteScript(string script)
        {

            try
            {
                syntaxChecker.CheckSyntax(script); 

                
                string[] lines = script.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                currentLineNumber = 0;
                foreach (var line in lines)
                {
                    ExecuteCommand(line);
                    currentLineNumber++;
                }
            }
            catch (SyntaxException ex)
            {
                // Handle syntax error
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

        

        public UpdatePenPositionDelegate UpdatePenPositionAction { get; set; }

    }
}
    


   

