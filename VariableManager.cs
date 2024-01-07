using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    /// <summary>
    /// The class responsible for managing the logic of variable storage and parsing. 
    /// </summary>
    public class VariableManager
    {
        
        private Dictionary<string, int> variables = new Dictionary<string, int>();

        /// <summary>
        /// Responsible for assinging variable names and their values 
        /// </summary>
        /// <param name="command"></param>
        /// <remarks>
        /// The Method is called when the '=' operator is detected in a statement, the method will assign the string value to the varible name and the integer to the variables value, 
        /// variables are stored in a dictionary
        /// </remarks>
        public void ProcessVariableAssignment(string command)
        {
            string[] parts = command.Split('=');
            string varName = parts[0].Trim();
            string expression = parts[1].Trim();

            int value = EvaluateVarExpression(expression); 
            Console.WriteLine($"Processing variable assignment: {command}");
            SetVariable(varName, value); 
        }

        private int EvaluateVarExpression(string expression)
        {
            string[] tokens = expression.Split(new char[] { ' ', '+' }, StringSplitOptions.RemoveEmptyEntries);
            int result = 0;

            foreach (string token in tokens)
            {
                result += ResolveArgumentToInteger(token);
            }

            return result;
        }


        /// <summary>
        /// Responsible for setting the new variable value 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <remarks>
        /// If the var does not exist, it will be created, if the var already exists, it will be updates with the new value
        /// </remarks>
        public void SetVariable(string name, int value)
        {
            
            
            variables[name] = value;
            Console.WriteLine($"Setting variable {name} to {value}");
        }

        /// <summary>
        /// Resposible for retriving the value of a specified variable
        /// </summary>
        /// <param name="name"></param>
        /// <returns>
        /// The integer value of the variable
        /// </returns>
        public int GetVariable(string name)
        {
            if (variables.TryGetValue(name, out int value))
            {
                return value;
            }

            throw new InvalidOperationException($"Variable '{name}' is not defined.");
        }

        /// <summary>
        /// Checks if a variable with the specified name is already defined, without retrieving its value.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>
        /// True if the var already exists
        /// </returns>
        public bool IsVariableDefined(string name)
        {
            return variables.ContainsKey(name);
        }

        private int ResolveArgumentToInteger(string arg)
        {
            if (IsVariableDefined(arg))
                return GetVariable(arg);
            else if (int.TryParse(arg, out int result))
                return result;

            throw new ArgumentException($"Invalid argument: {arg}");
        }


    }
}