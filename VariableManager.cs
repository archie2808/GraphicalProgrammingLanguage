using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class VariableManager
    {
        
        private Dictionary<string, int> variables = new Dictionary<string, int>();
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

        public void SetVariable(string name, int value)
        {
            variables[name] = value;
        }

        public int GetVariable(string name)
        {
            if (variables.TryGetValue(name, out int value))
            {
                return value;
            }

            throw new InvalidOperationException($"Variable '{name}' is not defined.");
        }

        public bool IsVariableDefined(string name)
        {
            return variables.ContainsKey(name);
        }
    }
}