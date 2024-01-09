using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class MethodManager
    {
        private VariableManager variableManager;
        private CommandParser commandParser;
        private Dictionary<string, MethodData> methods;
        private ScriptManager scriptManager;

        private string currentDefiningMethod;
        private bool methodFlag = false;
        private List<string> currentMethodCommands;
        private bool isExecuting = false;

        public bool IsExecuting()
        {
            return isExecuting;
        }

        /// <summary>
        /// Allows exception class to check if method name already exists
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public bool IsMethodNameDefined(string methodName)
        {
            return methods.ContainsKey(methodName);
        }


        public MethodManager(VariableManager variableManager, ScriptManager scriptManager, CommandParser commandParser)
        {
            this.variableManager = variableManager;
            this.scriptManager = scriptManager;
            this.commandParser = commandParser;
            methods = new Dictionary<string, MethodData>();

        }




        public void DefineMethod(string methodName, int startLine, string[] parameters)
        {

            currentDefiningMethod = methodName;
            methodFlag = true;
            currentMethodCommands = new List<string>();
            methods[methodName] = new MethodData(startLine, -1, parameters, currentMethodCommands);
            Console.WriteLine($"Defining method: {methodName} Start Line: {startLine}");
        }

        public void EndMethodDefinition(int endLine)
        {
            if (string.IsNullOrEmpty(currentDefiningMethod))
            {
                throw new InvalidOperationException("No method is currently being defined.");
            }

            if (methods.TryGetValue(currentDefiningMethod, out var methodData))
            {
                methodData.EndLine = endLine;
                methodData.Commands = new List<string>(currentMethodCommands);
                methods[currentDefiningMethod] = methodData;
                methodFlag = false;
                Console.WriteLine($"Ending method definition: {currentDefiningMethod}");
                currentDefiningMethod = null; // Reset the current method name



            }
            else
            {
                throw new InvalidOperationException($"Method '{currentDefiningMethod}' not defined.");
            }
        }
        public bool IsDefiningMethod { get { return methodFlag; } }
        public void AddCommand(string command)
        {
            if (methodFlag)
            {
                currentMethodCommands.Add(command);
                Console.WriteLine($"Adding command to method {currentDefiningMethod}: {command}");
            }
            else
            {
                commandParser.ExecuteCommand(command);
            }
        }
        public void CallMethod(string methodName, string[] arguments)
        {
            if (methods.TryGetValue(methodName, out var method))
            {
                variableManager.PushScope(); // Enter new local scope
                MapArgumentsToParameters(arguments, method.Parameters);

                isExecuting = true;

                foreach (var command in method.Commands)
                {
                    if (!string.IsNullOrWhiteSpace(command))
                    {
                        commandParser.ExecuteCommand(command);
                    }
                }

                variableManager.PopScope(); // Exit local scope
                isExecuting = false;
            }
            else
            {
                throw new InvalidOperationException($"Method '{methodName}' not found.");
            }
        }

        private void MapArgumentsToParameters(string[] arguments, string[] parameters)
        {
            if (arguments.Length != parameters.Length)
            {
                throw new ArgumentException($"Expected {parameters.Length} arguments, but got {arguments.Length}.");
            }

            for (int i = 0; i < arguments.Length; i++)
            {
                string paramName = parameters[i];
                string argValue = arguments[i];
                if (int.TryParse(argValue, out int intValue))
                {
                    variableManager.SetVariable(paramName, intValue);
                }

                else if (variableManager.IsVariableDefined(argValue))
                {
                    int varValue = variableManager.GetVariable(argValue);
                    variableManager.SetVariable(paramName, varValue);
                }
                else
                {
                    throw new ArgumentException($"Invalid argument: {argValue} for parameter: {paramName}");
                }
            }
        }



        public struct MethodData
        {
            public int StartLine;
            public int EndLine;
            public string[] Parameters;
            public List<string> Commands; // New attribute to store the commands of the method

            public MethodData(int startLine, int endLine, string[] parameters, List<string> commands)
            {
                StartLine = startLine;
                EndLine = endLine;
                Parameters = parameters;
                Commands = commands ?? new List<string>(); // Initialize with an empty list if null
            }
        }

        public List<string> GetMethodCommands(string methodName)
        {
            if (methods.TryGetValue(methodName, out var methodData))
            {
                return methodData.Commands;
            }
            else
            {
                throw new InvalidOperationException($"Method '{methodName}' not found.");
            }
        }

    }

}