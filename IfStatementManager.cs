using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    /// <summary>
    /// Manages the execution of conditional IF statements
    /// </summary>
    public class IfStatementManager
    {
        private CommandParser commandParser;
        private VariableManager variableManager; 
        private bool conditionResult; 

        //list to store the commands inside the if statements block 
        private List<String> commandBlock = new List<string>();
        public IfStatementManager( VariableManager variableManager)
        {
            this.variableManager = variableManager;
        }

        /// <summary>
        /// Sets the command parser instance to be used by the class
        /// </summary>
        /// <param name="commandParser"></param>
        public void SetCommandParser(CommandParser commandParser)
        {
            this.commandParser = commandParser;
        }

        /// <summary>
        /// Starts the process of handling an if statemnt
        /// </summary>
        /// <param name="command"></param>
        public void StartIfStatement(string command)
        {
           
            string condition = ExtractCondition(command);
            
            conditionResult = EvaluateCondition(condition, commandParser);
        }

        /// <summary>
        /// Adds a command to the current if statement block, to be executed if the condition is true
        /// </summary>
        /// <param name="command"></param>
        public void AddCommand(string command)
        {
            
            if (conditionResult)
            {
                commandBlock.Add(command);
            }
        }

        /// <summary>
        /// If the condition strikes true, commandblock will be executed. commandblock is cleared from memory wheter the command block is true
        /// or not.
        /// </summary>
        /// <param name="commandParser"></param>
        public void EndIfStatement(CommandParser commandParser)
        {
            
            if (conditionResult)
            {
                foreach (var cmd in commandBlock)
                {
                    commandParser.ExecuteCommand(cmd);
                }
            }
            //Clear the command block after execution or if the condition was false
            commandBlock.Clear();
        }
        /// <summary>
        /// Extracts the condition expression from the if statement command string
        /// </summary>
        /// <param name="command"></param>
        /// <returns>the condition expression</returns>
        private string ExtractCondition(string command)
        {
            //split the command by space and take the parts after the 'if' 
            var parts = command.Split(new char[] { ' ' }, 2);
            if (parts.Length < 2)
            {
                throw new ArgumentException("Invalid if statement format.");
            }
            return parts[1].Trim();

        }

        /// <summary>
        /// Evaluates a condition string within the if statement. The method parses the condition, resolves any variables, and applies
        /// the specified comparionson operator. 
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="commandParser"></param>
        /// <returns>a boolean value indicating the result of the condition evaluation</returns>
        public bool EvaluateCondition(string condition, CommandParser commandParser)
        {
            
            var tokens = condition.Split(' ');
            if (tokens.Length != 3)
            {
                throw new ArgumentException("Invalid Condition Format");
            }

        
            int leftOperand = CommandFactory.ResolveArgumentToInteger(tokens[0], variableManager);
            int rightOperand = CommandFactory.ResolveArgumentToInteger(tokens[2], variableManager);
            string operatorToken = tokens[1];

            
            switch (operatorToken)
            {
                case "<":
                    return leftOperand < rightOperand;
                case "<=":
                    return leftOperand <= rightOperand;
                case ">":
                    return leftOperand > rightOperand;
                case ">=":
                    return leftOperand >= rightOperand;
                case "==":
                    return leftOperand == rightOperand;
                case "!=":
                    return leftOperand != rightOperand;
                default:
                    throw new ArgumentException("Invalid comparison operator.");
            }

        }

        
    }
} 
