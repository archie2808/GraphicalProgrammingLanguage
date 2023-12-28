using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class IfStatementManager
    {
        private bool conditionResult; //Flag to store the result of the if statements condition

        //list to store the commands inside the if statements bloc
        private List<String> commandBlock = new List<string>();

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

        }

        private bool EvaluateCondition(string condition)
        {

        }
    }
}
