using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1;
using System;
using System.Drawing.Drawing2D;

namespace ProgramTesting
{
    [TestClass]
    public class CommandParsingTests
    {
        private TextBox outputTextBox;
        private Bitmap drawingSurface;
        private CommandParser commandParser;


        [TestInitialize]
        public void Initialise()
        {
            // Arrange
            outputTextBox = new TextBox();
            drawingSurface = new Bitmap(50, 50);
            commandParser = new CommandParser(outputTextBox, drawingSurface);

        }

        /// <summary>
        /// A test method for verifying line by line reading and execution of commands
        /// </summary>
        /// <remarks>
        /// The method expects acknowledgment and execution of a provided Command.
        /// </remarks>
        [TestMethod]
        public void ExecuteCommand_SimpleCommand_UpdatesOutputTextBox()
        {
            // Arrange
            string command = "moveto 10 10";

            // Act
            commandParser.ExecuteCommand(command);

            // Assert
            string expectedOutput = "Pen moved to (10, 10).";
            Assert.IsTrue(outputTextBox.Text.Contains(expectedOutput));
        }



    }
}
