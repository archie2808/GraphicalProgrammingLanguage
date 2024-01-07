using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    /// <summary>
    /// Manages the Loop Logic, responsible for starting, executing, and ending the Loop
    /// </summary>
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
        /// <summary>
        /// Set the command parser to avoid circular dependancy
        /// </summary>
        /// <param name="commandParser"></param>
        public void SetCommandParserLoop(CommandParser commandParser)
        {
            this.commandParser = commandParser;
        }
        /// <summary>
        /// Gets a value indicating whether the loop is active
        /// </summary>
        public bool IsLoopActive
        {
            get { return isLoopActive; }
        }

        /// <summary>
        /// Starts a loop with the specifed condition
        /// </summary>
        /// <param name="command">The command to add to the looop</param>
        public void StartLoop(string command)
        {
            loopCondition = ExtractLoopCondition(command);
            isLoopActive = true;

            loopCommands.Clear();

        }
        /// <summary>
        /// Sets the loop flag to false once all the commands have been stored in the commandBlock, and directs the compiler to execution. 
        /// </summary>
        public void EndLoop()
        {          
            isLoopActive = false;
            ExecuteLoop();           
        }
        /// <summary>
        /// Executues the commands withink the loopblock list so long as the condition evaluates to true.
        /// </summary>
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
        /// <summary>
        /// Adds commands to the loop command list
        /// </summary>
        /// <param name="command"></param>
        public void AddCommandToLoop(string command)
        {
            if (isLoopActive)
            {
                
                loopCommands.Add(command);

            }

        }
        /// <summary>
        /// Evaluates the loop condition and returns a boolean value indicating the result. Once the condition is met
        /// the boolean will return false and cease the loop
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool EvaluateCondition()
        {
            var tokens = loopCondition.Split(' ');
            if (tokens.Length != 3) throw new ArgumentException("Invalid loop condition format.");

            int leftOperand = ResolveToInteger(tokens[0]);
            int rightOperand = ResolveToInteger(tokens[2]);
            string operatorToken = tokens[1];

            return ApplyOperator(leftOperand, rightOperand, operatorToken);
        }
        /// <summary>
        /// Resolves a token to an integer value, either by parsing it as an integer or retrieving its value if it is a variable.
        /// </summary>
        /// <param name="token">The token to resolve.</param>
        /// <returns>The integer value of the token.</returns>
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
        /// <summary>
        /// Applies the specified operator to two integer operands and returns the result.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <param name="operatorToken">The operator to apply.</param>
        /// <returns>The result of the operation.</returns>
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
        /// <summary>
        /// Extracts and returns the loop condition from a loop command string.
        /// </summary>
        /// <param name="commandString">The loop
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
   

