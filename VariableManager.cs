using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    /// <summary>
    /// The class responsible for managing the logic of variable storage and parsing. 
    /// </summary>
    public class VariableManager
    {

        private Stack<Dictionary<string, int>> scopes;

        public VariableManager()
        {
            scopes = new Stack<Dictionary<string, int>>();
            scopes.Push(new Dictionary<string, int>()); // Global scope
        }

        public void PushScope()
        {
            scopes.Push(new Dictionary<string, int>()); // New local scope
        }

        public void PopScope()
        {
            if (scopes.Count > 1)
            {
                scopes.Pop();
            }
            else
            {
                throw new InvalidOperationException("Cannot pop global scope.");
            }
        }
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
            string Name = parts[0].Trim();
            string expression = parts[1].Trim();

            int value = EvaluateVarExpression(expression);
            Console.WriteLine($"Processing variable assignment: {command}");
            SetVariable(Name, value);
        }
        /// <summary>
        /// Evaluates the expression withhin a variable
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>The value of the expression assocates with the variable</returns>
        private int EvaluateVarExpression(string expression)
        {
            // Split expression into tokens considering various operators
            var tokens = Regex.Split(expression, @"([+\-*/])").Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).ToArray();
            int result = ResolveArgumentToInteger(tokens[0]);

            for (int i = 1; i < tokens.Length; i += 2)
            {
                string operatorToken = tokens[i];
                int nextValue = ResolveArgumentToInteger(tokens[i + 1]);

                switch (operatorToken)
                {
                    case "+":
                        result += nextValue;
                        break;
                    case "-":
                        result -= nextValue;
                        break;
                    case "*":
                        result *= nextValue;
                        break;
                    case "/":
                        if (nextValue == 0)
                        {
                            throw new DivideByZeroException("Cannot divide by zero.");
                        }
                        result /= nextValue;
                        break;
                    default:
                        throw new ArgumentException($"Invalid operator: {operatorToken}");
                }
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
            bool variableSet = false;
            foreach (var scope in scopes)
            {
                if (scope.ContainsKey(name))
                {
                    scope[name] = value;
                    Console.WriteLine($"Variable '{name}' set to {value} in local scope.");
                    variableSet = true;
                    break;
                }
            }
            if (!variableSet)
            {
                scopes.Last()[name] = value;
                Console.WriteLine($"Variable '{name}' set to {value} in global scope.");
            }
            // If variable not found in any scope, create it in the global scope (or throw an error)
            
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
            foreach (var scope in scopes)
            {
                if (scope.TryGetValue(name, out int value))
                {
                    return value;
                }
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
            return scopes.Any(scope => scope.ContainsKey(name));
        }

        private int ResolveArgumentToInteger(string arg)
        {
            if (IsVariableDefined(arg))
            {
                return GetVariable(arg);
            }
            else if (int.TryParse(arg, out int result))
            {
                return result;
            }
            else
            {
                throw new InvalidOperationException($"difficulty resolving argument of type String to type Int: {arg}");
            }
        }
    }
}



        /// <summary>
        /// Responsible for assinging variable names and their values 
        /// </summary>
        /// <param name="command"></param>
        /// <remarks>
        /// The Method is called when the '=' operator is detected in a statement, the method will assign the string value to the varible name and the integer to the variables value, 
        /// variables are stored in a dictionary
        /// </remarks>
/*public void ProcessVariableAssignment(string command)
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

      throw new InvalidOperationException($"bruh {arg}");
  } 


}
}    */
