using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    /// <summary>
    /// The SyntaxChecker class is responsible for validating the syntax of a given script.
    /// It checks each line for correct syntax and structure, ensuring that commands, variable assignments,
    /// and control structures (loops, if statements) are correctly formatted.
    /// </summary>
    public class SyntaxChecker
    {

        private VariableManager variableManager;
        private MethodManager methodManager;

        public SyntaxChecker(VariableManager variableManager, MethodManager methodManager)
        {
            this.variableManager = variableManager;
            this.methodManager = methodManager;
        }

        /// <summary>
        /// Checks the syntax of a script
        /// </summary>
        /// <param name="script"></param>
        /// <remarks>
        /// Iterates through each line of the script, validating the syntax of commands, loops, and if statements.
        /// Throws a SyntaxException if any syntax errors are found.
        /// </remarks>
        public void CheckSyntax(string script)
        {
            string[] lines = script.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim().ToLower();
                if (line.StartsWith("while"))
                {
                    CheckLoopSyntax(lines, ref i); // i will be updated to the end of the loop
                }
                else if (line.StartsWith("if"))
                {
                    CheckIfSyntax(lines, ref i); // i will be updated to the end of the if statement
                }
                else if (line.StartsWith("method"))
                {
                    CheckMethodDeclaration(lines, ref i);
                }
                else if (line.StartsWith("endmethod"))
                {
                    CheckMethodDeclaration(lines, ref i);
                }
                else if (line.StartsWith("call"))
                {
                    ValidateMethodCallSyntax(line, i + 1);
                }
                else
                {
                    IsValidSyntax(lines[i], i + 1);
                }
            }
        }

        /// <summary>
        /// Validates the syntax of a single line of the script.
        /// </summary>
        /// <param name="line">The script line to be validated.</param>
        /// <param name="lineNumber">The line number for error reporting.</param>
        /// <returns>True if the syntax is valid.</returns>
        private bool IsValidSyntax(string line, int lineNumber)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                throw new SyntaxException($"Line {lineNumber}: The command is null, empty, or consists only of white space.");
            }

            if (line.Contains("="))
            {

                ValidateVariableAssignmentSyntax(line, lineNumber);
                return true;
            }

            string[] parts = line.Split(' ');
            string commandName = parts[0].ToLower();
            string[] arguments = parts.Skip(1).ToArray();

            switch (commandName)
            {
                case "moveto":
                    ValidateArgumentsCount(arguments, 2, lineNumber, commandName);
                    ValidateArgumentsAreIntegersOrVariables(arguments, lineNumber, variableManager);
                    break;

                case "drawto":
                    ValidateArgumentsCount(arguments, 2, lineNumber, commandName);
                    ValidateArgumentsAreIntegersOrVariables(arguments, lineNumber, variableManager);
                    break;

                case "rectangle":
                    ValidateArgumentsCount(arguments, 2, lineNumber, commandName);
                    ValidateArgumentsAreIntegersOrVariables(arguments, lineNumber, variableManager);
                    break;

                case "circle":
                    ValidateArgumentsCount(arguments, 1, lineNumber, commandName);
                    ValidateArgumentsAreIntegersOrVariables(arguments, lineNumber, variableManager);
                    break;

                case "triangle":
                    ValidateArgumentsCount(arguments, 3, lineNumber, commandName);
                    ValidateArgumentsAreIntegersOrVariables(arguments, lineNumber, variableManager);
                    break;

                case "colour":
                    ValidateArgumentsCount(arguments, 1, lineNumber, commandName);
                    break;



                default:
                    throw new SyntaxException($"Line {lineNumber}: Unknown command '{commandName}'.");
            }

            return true;
        }
        /// <summary>
        /// Checks the method starts with call and that the variable name exisits
        /// </summary>
        /// <param name="line"></param>
        /// <param name="lineNumber"></param>
        private void ValidateMethodCallSyntax(string line, int lineNumber)
        {
            var parts = line.Split(new char[] { ' ' }, 2);
            if (parts.Length != 2 || !parts[0].ToLower().Equals("call"))
            {
                throw new SyntaxException($"Line {lineNumber}: Method call must start with 'call'.");
            }

         

            
           
        }


        /// <summary>
        /// checks method declaration for method and end method keyword aswell as verifying correct method names and parmaters
        /// </summary>
        /// <param name="line"></param>
        /// <param name="lineNumber"></param>
        private void CheckMethodDeclaration(string[] lines, ref int index)
        {
            var parts = lines[index].Split(new char[] { ' ' }, 3);

            if (parts[0].ToLower().Equals("method"))
            {
                if (parts.Length < 3)
                {
                    throw new SyntaxException($"Line {index + 1}: Incomplete method declaration.");
                }

                string methodName = parts[1];
                if (!Regex.IsMatch(methodName, @"^[a-zA-Z][a-zA-Z0-9_]*$"))
                {
                    throw new SyntaxException($"Line {index + 1}: Invalid method name '{methodName}'.");
                }

                string parametersPart = parts[2];
                var parameters = parametersPart.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var param in parameters)
                {
                    if (!Regex.IsMatch(param.Trim(), @"^[a-zA-Z][a-zA-Z0-9_]*$")) // Simplified regex for parameter names
                    {
                        throw new SyntaxException($"Line {index + 1}: Invalid parameter format in '{param.Trim()}'.");
                    }
                }
            }
            else if (parts[0].ToLower().Equals("endmethod"))
            {
                if (parts.Length > 1)
                {
                    throw new SyntaxException($"Line {index + 1}: 'endmethod' should not have additional parameters.");
                }
                // Logic to handle end of method if needed
            }
            else
            {
                throw new SyntaxException($"Line {index + 1}: Expected 'method' or 'endmethod' declaration.");
            }
        }


        /// <summary>
        /// Validates the syntax of a variable assignment.
        /// </summary>
        /// <param name="line">The line containing the variable assignment.</param>
        /// <param name="lineNumber">The line number for error reporting.</param>
        private void ValidateVariableAssignmentSyntax(string line, int lineNumber)
        {
            string[] parts = line.Split('=');
            if (parts.Length != 2)
            {
                throw new SyntaxException($"Line {lineNumber}: Invalid variable assignment syntax.");
            }

            string variableName = parts[0].Trim();
            if (!IsValidVariableName(variableName))
            {
                throw new SyntaxException($"Line {lineNumber}: '{variableName}' is not a valid variable name.");
            }

            string expression = parts[1].Trim();
            ValidateExpressionSyntax(expression, lineNumber);
        }

        /// <summary>
        /// Checks the syntax of a loop construct.
        /// </summary>
        /// <param name="lines">All lines of the script.</param>
        /// <param name="index">The starting index of the loop in the script.</param>
        private void CheckLoopSyntax(string[] lines, ref int index)
        {

            string loopStartLine = lines[index];
            if (!IsValidLoopStart(loopStartLine))
            {
                throw new SyntaxException($"Line {index + 1}: Invalid loop start.");
            }

            index++;

            while (index < lines.Length && !lines[index].Trim().ToLower().Equals("endwhile"))
            {

                IsValidSyntax(lines[index], index + 1);
                index++;
            }

            if (index == lines.Length || !lines[index].Trim().ToLower().Equals("endwhile"))
            {
                throw new SyntaxException("Loop not properly closed with 'endwhile'.");
            }


        }

        /// <summary>
        /// Checks the syntax of an if statement construct.
        /// </summary>
        /// <param name="lines">All lines of the script.</param>
        /// <param name="index">The starting index of the if statement in the script.</param>
        private void CheckIfSyntax(string[] lines, ref int index)
        {

            string ifStartLine = lines[index];
            if (!IsValidIfStart(ifStartLine))
            {
                throw new SyntaxException($"Line {index + 1}: Invalid if start.");
            }

            index++;

            while (index < lines.Length && !lines[index].Trim().ToLower().Equals("endif"))
            {
                // Line 0 = 1 for user
                IsValidSyntax(lines[index], index + 1);
                index++;
            }

            if (index == lines.Length || !lines[index].Trim().ToLower().Equals("endif"))
            {
                throw new SyntaxException("If statement not properly closed with 'endif'.");
            }


        }

        /// <summary>
        /// Validates the start of a loop statement construct.
        /// </summary>
        /// <param name="line">.</param>
        /// <returns>True if the loop is valid.</returns
        private bool IsValidLoopStart(string line)
        {

            string[] parts = line.Trim().Split(new char[] { ' ' }, 2);
            if (parts.Length < 2 || !parts[0].ToLower().Equals("while"))
            {
                return false;
            }

            string condition = parts[1];


            return true;
        }

        /// <summary>
        /// Validates the start of an if statement construct.
        /// </summary>
        /// <param name="line">The line containing the if statement start.</param>
        /// <returns>True if the if start is valid.</returns
        private bool IsValidIfStart(string line)
        {

            string[] parts = line.Trim().Split(new char[] { ' ' }, 2);
            if (parts.Length < 2 || !parts[0].ToLower().Equals("if"))
            {
                return false;
            }

            string condition = parts[1];


            return true;
        }

        /// <summary>
        /// Confirms a command has the correct number of arguments
        /// </summary>
        /// <param name="args"></param>
        /// <param name="expectedCount"></param>
        /// <param name="lineNumber"></param>
        /// <param name="commandName"></param>
        private void ValidateArgumentsCount(string[] args, int expectedCount, int lineNumber, string commandName)
        {
            if (args.Length != expectedCount)
            {
                throw new SyntaxException($"Line {lineNumber}: '{commandName}' command expects {expectedCount} arguments, found {args.Length}.");
            }
        }

        /// <summary>
        /// Validates if the arguments are ints or vars
        /// </summary>
        /// <param name="args"></param>
        /// <param name="lineNumber"></param>
        /// <param name="variableManager"></param>
        private void ValidateArgumentsAreIntegersOrVariables(string[] args, int lineNumber, VariableManager variableManager)
        {
            foreach (string arg in args)
            {
                if (!int.TryParse(arg, out int result) && !IsValidVariableName(arg))
                {
                    throw new SyntaxException($"Line {lineNumber}: Argument '{arg}' is neither an integer nor a defined variable.");
                }
            }
        }

        /// <summary>
        /// Validates correct expression syntax
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="lineNumber"></param>
        private void ValidateExpressionSyntax(string expression, int lineNumber)
        {

            var tokens = Regex.Split(expression, @"([+\-*/])").Where(t => t != string.Empty).ToArray();

            foreach (var token in tokens)
            {

                var trimmedToken = token.Trim();


                if (!int.TryParse(trimmedToken, out _) &&
                    !IsValidVariableName(trimmedToken) &&
                    !IsValidOperator(trimmedToken))
                {
                    throw new SyntaxException($"Line {lineNumber}: Invalid token '{trimmedToken}' in expression '{expression}'.");
                }
            }
        }

        private bool IsValidOperator(string token)
        {
            return new[] { "+", "-", "*", "/" }.Contains(token);
        }

        private bool IsValidVariableName(string variableName)
        {
            return Regex.IsMatch(variableName, @"^[a-zA-Z_][a-zA-Z0-9_]*$");
        }


    }
}
