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
        private CommandParser commandParser;
        private TextBox outputTextbox;
        private Bitmap drawingSurface;



        [TestInitialize]
        public void Setup()
        {
            outputTextbox = new TextBox();
            drawingSurface = new Bitmap(100, 100);
            commandParser = new CommandParser(outputTextbox, drawingSurface);

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
            var outputTextBox = new TextBox();
            var drawingSurface = new Bitmap(100, 100);
            var commandParser = new CommandParser(outputTextBox, drawingSurface);
            string command = "moveto 10 10";

            // Act
            commandParser.ExecuteCommand(command);

            // Assert
            string expectedOutput = "Pen moved to (10, 10).";
            Assert.IsTrue(outputTextBox.Text.Contains(expectedOutput));
        }



    }
}
