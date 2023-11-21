using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1;
using System;
using System.Drawing.Drawing2D;
using System.IO;

namespace ProgramTesting
{
    /// <summary>
    /// Test class for Verifying functionality of Error Handling. 
    /// </summary>
    [TestClass]
    public class ErrorHandlingTests
    {
       
        private TextBox outputTextBox;
        private Bitmap drawingSurface;
        private CommandParser commandParser;

        [TestInitialize]
        public void Initialise()
        {
            outputTextBox = new TextBox();
            drawingSurface = new Bitmap(500, 500);
            commandParser = new CommandParser(outputTextBox, drawingSurface);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidCommandThrowsException()
        {
            //Arrange 
            string command = "movetooo 100 100";

            //Act
            commandParser.ExecuteCommand(command);
        }
    }
}
