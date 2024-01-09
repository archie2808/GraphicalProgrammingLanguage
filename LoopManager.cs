using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class LoopManager
    {
        private string loopCondition;
        private List<string> loopCommands;
        private bool isLoopActive;
        private VariableManager variableManager;
        private CommandParser commandParser;

        public LoopManager(VariableManager variableManager)
        {
            this.variableManager = variableManager;
            this.loopCommands = new List<string>();
        }
        public void SetCommandParserLoop(CommandParser commandParser)
        {
            this.commandParser = commandParser;
        }
        public bool IsLoopActive
        {
            get { return isLoopActive; }
        }


        public void StartLoop(string command)
        {
            loopCondition = ExtractLoopCondition(command);
            isLoopActive = true;

            loopCommands.Clear();

        }

        public void EndLoop()
        {
            isLoopActive = false;
            ExecuteLoop();
        }

        public void ExecuteLoop()
        {

            while (EvaluateCondition())
            {
                var currentCommands = new List<string>(loopCommands);

                foreach (var command in currentCommands)
                {
                    commandParser.ExecuteCommand(command);
                }
            }
        }

        public void AddCommandToLoop(string command)
        {
            if (isLoopActive)
            {

                loopCommands.Add(command);

            }

        }
        private bool EvaluateCondition()
        {
            var tokens = loopCondition.Split(' ');
            if (tokens.Length != 3) throw new ArgumentException("Invalid loop condition format.");

            int leftOperand = ResolveToInteger(tokens[0]);
            int rightOperand = ResolveToInteger(tokens[2]);
            string operatorToken = tokens[1];

            return ApplyOperator(leftOperand, rightOperand, operatorToken);
        }

        private int ResolveToInteger(string token)
        {
            if (variableManager.IsVariableDefined(token))
            {
                return variableManager.GetVariable(token);
            }
            else if (int.TryParse(token, out int result))
            {
                return result;
            }
            throw new ArgumentException($"Invalid token in condition: {token}");
        }

        private bool ApplyOperator(int left, int right, string operatorToken)
        {
            switch (operatorToken)
            {
                case "<": return left < right;
                case "<=": return left <= right;
                case ">": return left > right;
                case ">=": return left >= right;
                case "==": return left == right;
                case "!=": return left != right;
                default: throw new ArgumentException($"Invalid operator: {operatorToken}");
            }
        }

        public string ExtractLoopCondition(string commandString)
        {
            const string loopKeyword = "while";
            if (!commandString.StartsWith(loopKeyword))
            {
                throw new ArgumentException("Invalid loop command format");
            }

            string condition = commandString.Substring(loopKeyword.Length).Trim();
            if (string.IsNullOrEmpty(condition))
            {
                throw new ArgumentException("No condition specified in the loop command.");
            }
            return condition;
        }
    }
}

