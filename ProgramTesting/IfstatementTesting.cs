using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApp1;
using System.Drawing;

namespace ProgramTesting
{
    [TestClass]
    public class IfstatementTesting
    {
        private VariableManager variableManager;
        private CommandParser commandParser;
        private IfStatementManager ifStatementManager;

        [TestInitialize]
        public void Setup()
        {
            
            variableManager = new VariableManager();
            commandParser = new CommandParser(null, new Bitmap(500, 500), variableManager, null, null);

            ifStatementManager = new IfStatementManager(variableManager);
            ifStatementManager.SetCommandParserIf(commandParser);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IfStatementManager_HandlesMalformedCondition_Gracefully()
        {
            
            string malformedCondition = "if x 10"; // Missing operator

            // Act: Process the malformed condition
            ifStatementManager.StartIfStatement(malformedCondition);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IfStatementManager_HandlesUndefinedVariable_Gracefully()
        {
           
            string undefinedVariableCondition = "if undefinedVar > 10";

            
            ifStatementManager.StartIfStatement(undefinedVariableCondition);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IfStatementManager_HandlesInvalidOperator_Gracefully()
        {
            
            string invalidOperatorCondition = "if x ** y"; 

            
            ifStatementManager.StartIfStatement(invalidOperatorCondition);
        }
    }
    
}