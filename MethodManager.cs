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
        private ScriptManager scriptManager;
        private Dictionary<string, MethodData> methods;
        private List<string> scriptLines;
        private int currentLine;
        private Stack<int> returnStack;
        private bool methodFlag = false;

        public MethodManager(VariableManager variableManager)
        {
            this.variableManager = new VariableManager();
            
            methods = new Dictionary<string, MethodData>();
            returnStack = new Stack<int>();
        }
        public void UpdateScript(string newScript)
        {
            this.scriptLines = new List<string>(newScript.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries));
        }

        public void SetCommandParserLoop(CommandParser commandParser)
        {
            this.commandParser = commandParser;
        }

        public void DefineMethod(string methodName, int startLine, string[] parameters)
        {
            methods[methodName] = new MethodData(startLine, -1, parameters);
        }

        public void EndMethodDefinition(string methodName, int endLine)
        {
            if (methods.TryGetValue(methodName, out var methodData))
            {
                methodData.EndLine = endLine;
                methods[methodName] = methodData;
            }
            else
            {
                throw new InvalidOperationException($"Method '{methodName}' not defined.");
            }
        }

        public void CallMethod(string methodName, string[] arguments)
        {
            if (methods.TryGetValue(methodName, out var method))
            {

                MapArgumentsToParameters(arguments, method.Parameters);
                returnStack.Push(currentLine);
                currentLine = method.StartLine;

                for (int i = method.StartLine; i <= method.EndLine; i++)
                {
                    string command = GetCommandFromLine(i);
                    if (!string.IsNullOrEmpty(command))
                    {
                        commandParser.ExecuteCommand(command);
                    }
                }

                currentLine = returnStack.Pop();
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

        public string GetCommandFromLine(int line)
        {
            
            if (line >= 0 && line < scriptLines.Count)
            {
                return scriptLines[line];
            }
            else
            {
                // Handle the case where the line number is invalid
                return null;
            }
        }

        public struct MethodData
        {
            public int StartLine;
            public int EndLine;
            public string[] Parameters;

            public MethodData(int startLine, int endLine, string[] parameters)
            {
                StartLine = startLine;
                EndLine = endLine;
                Parameters = parameters;
            }
        }
    }
}