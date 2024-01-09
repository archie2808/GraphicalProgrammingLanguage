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
        private Label errorLabel;
        private MethodManager methodManager;
        private LoopManager loopManager;
        private DrawingManager drawingManager;
        private IfStatementManager ifStatementManager;
        private VariableManager variableManager;
        private Point penPosition;
        private ScriptManager scriptManager;

        private SyntaxChecker syntaxChecker;





        /// <summary>
        /// Gets the current position of the pen at the drawing surface
        /// </summary>
        /// <remarks>
        /// the property provides access to the current location of the pen, allowing external classes
        /// to query its position
        /// </remarks>
        public Point PenPosition
        {
            get { return penPosition; }

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
        public CommandParser(Label errorLabel, Bitmap surface, VariableManager vm, IfStatementManager ifStatementManager, LoopManager loopManager, ScriptManager scriptManager, SyntaxChecker syntaxChecker)
        {
            penPosition = new Point(0, 0);
            this.errorLabel = errorLabel;
            drawingSurface = surface;
            drawingManager = new DrawingManager(surface);
            variableManager = vm;
            this.scriptManager = scriptManager;
            this.methodManager = new MethodManager(variableManager, scriptManager, this);
            this.ifStatementManager = ifStatementManager;
            this.loopManager = loopManager;
            UpdatePenPositionAction = (newPosition) => { penPosition = newPosition; };
            
            
            this.syntaxChecker = syntaxChecker; 
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
                errorLabel.Text = $"error Processing command{ex.Message}";
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
                if (commandString.StartsWith("call"))
                {
                    if (methodManager.IsExecuting() && commandString.StartsWith("call"))
                    {
                        // Skip execution of 'call' command within method execution context
                        return;
                    }

                    var (methodName, arguments) = ExtractMethodCallDetails(commandString);
                    //ProcessMethodVariableAssignments(methodName);
                    methodManager.CallMethod(methodName, arguments);
                }

                // Handling different types of commands...
                else if (commandString.StartsWith("method"))
                {
                    var (methodName, parameters) = ExtractMethodNameAndParameters(commandString);
                    int startLine = scriptManager.GetCurrentLineNumber(); // Assuming this method returns the current line number
                    methodManager.DefineMethod(methodName, startLine, parameters);
                }
                else if (commandString.StartsWith("endmethod"))
                {
                    int endLine = scriptManager.GetCurrentLineNumber();
                    methodManager.EndMethodDefinition(endLine);
                }
                else if (methodManager.IsDefiningMethod)
                {
                    methodManager.AddCommand(commandString);
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
                errorLabel.Text = $"Syntax error: {ex.Message}";
            }
            catch (Exception ex)
            {
                errorLabel.Text = $"Error executing command: {ex.Message}";
            }

        }


        public (string methodName, string[] parameters) ExtractMethodNameAndParameters(string commandString)
        {
            try
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
            catch (Exception ex)
            {
                throw new SyntaxException($"Error extracting method name and parameters: {ex.Message}");
            }
        }


        private (string methodName, string[] arguments) ExtractMethodCallDetails(string commandString)
        {
            try
            {
                commandString = commandString.Substring(4).Trim(); // Remove 'call' keyword
                var parts = commandString.Split(new char[] { ' ' }, 2);
                var methodName = parts[0].Trim();

                string[] arguments = parts.Length > 1
                                     ? parts[1].Split(',').Select(arg => arg.Trim()).ToArray()
                                     : new string[0];

                return (methodName, arguments);
            }
            catch (Exception ex)
            {
                throw new SyntaxException($"Error extracting method call details: {ex.Message}");
            }
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
            catch (SyntaxException ex)
            {
                errorLabel.Text = $"Argument error: {ex.Message}";

            }
            catch (Exception ex)
            {
                errorLabel.Text = $"Error during command execution: {ex.Message}";

            }


        }

        public void ExecuteScript(string script)
        {

            try
            {
                syntaxChecker.CheckSyntax(script);


                string[] lines = script.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    ExecuteCommand(line);

                }
            }
            catch (SyntaxException ex)
            {
                errorLabel.Text = ex.Message;
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

            errorLabel.Text = "Pen position reset to top-left corner.";
        }



        public UpdatePenPositionDelegate UpdatePenPositionAction { get; set; }
    }
}
