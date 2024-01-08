using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using WindowsFormsApp1;

namespace ProgramTesting
{
    [TestClass]
    public class SyntaxCheckerTests
    {
        private VariableManager variableManager;
        private SyntaxChecker syntaxChecker;

        [TestInitialize]
        public void Setup()
        {
            variableManager = new VariableManager();
            syntaxChecker = new SyntaxChecker(variableManager);
        }

        [TestMethod]
        public void TestValidSyntax()
        {
            string validScript = "moveto 100 200\n" +
                                 "drawto 150 250\n" +
                                 "while count < 10\n" +
                                 "count = count + 1\n" +
                                 "endwhile";

            syntaxChecker.CheckSyntax(validScript); // Should not throw any exception
        }

        [TestMethod]
        [ExpectedException(typeof(SyntaxException))]
        public void TestInvalidSyntax()
        {
            string invalidScript = "move 100 200"; // Incorrect command
            syntaxChecker.CheckSyntax(invalidScript);
        }
    }
}