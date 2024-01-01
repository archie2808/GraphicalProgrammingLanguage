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

            if (int.TryParse(parts[1].Trim(), out int value))
            {
                SetVariable(varName, value);
            }
            else
            {
                throw new InvalidOperationException("Invalid value for variable assignment");
            }
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

        /// <summary>
        /// Resolves a command argument to an integer value
        /// </summary>
        /// <param name="arg">The command argument, which can be a variable name or a direct integer</param>
        /// <remarks>
        /// The method checks if the provided argument is a defined variable in the variable manager class, if so it retrieves the variables value.
        /// if the argument is not a variable, it attemps to parse the argument as a direct integer. If neither condition is met, an exception is thrown. 
        /// </remarks>
        
        
    }
}