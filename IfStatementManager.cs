using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
namespace WindowsFormsApp1
{
    public class IfStatementManager
    {
        private CommandParser commandParser;
        private bool conditionResult; //Flag to store the result of the if statements condition

        //list to store the commands inside the if statements bloc
        private List<String> commandBlock = new List<string>();
        public IfStatementManager(CommandParser commandParser)
        {
            this.commandParser = commandParser;
        }
        public void StartIfStatement(string command)
        {
            //extract the condition from the if statement 
            string condition = ExtractCondition(command);
            //Evaluate the extracted condition and store the result
            conditionResult = EvaluateCondition(condition);
        }

        public void AddCommand(string command)
        {
            //If the condition is true add the command to the block for execution
            if (conditionResult)
            {
                commandBlock.Add(command);
            }
        }

        public void EndIfStatement(CommandParser commandParser)
        {
            //If the condition is true, execute all commands in the block
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

        private string ExtractCondition(string command)
        {
            //split the command by space and take the parts after the 'if' 
            var parts = command.Split(new char[] { ' ' }, 2);
            if (parts.Length < 2)
            {
                throw new ArgumentException("Invalid if statement format.");
            }
            return parts[1].Trim;

        }

        public bool EvaluateCondition(string condition, CommandParser commandParser)
        {
            //split the conditions into operands and operators
            var tokens = condition.Split(' ');
            if (tokens.Length != 3)
            {
                throw new ArgumentException("Invalid Condition Format");
            }

            //Resovle variable values or parse integers
            int leftOperand = ResolveArgumentToInteger(arg[0]);
            int rightOperand = ResolveArgumentToInteger(tokens[2]);
            string operatorToken = tokens[1];

            // Perform comparison based on the operator
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
} */
