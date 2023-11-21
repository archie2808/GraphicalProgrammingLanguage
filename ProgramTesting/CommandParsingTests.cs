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
            drawingSurface = new Bitmap(500, 500);
            commandParser = new CommandParser(outputTextBox, drawingSurface);

        }

        /// <summary>
        /// A test method for verifying line by line reading and execution of commands
        /// </summary>
        /// <remarks>
        /// The method expects acknowledgment and execution of a draw to command. verifies exectution by checking pixels of the drawing surface.
        /// </remarks>
        [TestMethod]
        public void ExecuteCommands_In_CommandLine()
        {
            var expectedColour = Color.Blue.ToArgb();

            // Act
            commandParser.ExecuteCommand("drawto 10 10");

            //Assert
            int actualColor = drawingSurface.GetPixel(10, 10).ToArgb();
            Assert.AreEqual(expectedColour, actualColor, "Pixel at (10, 10) should be colored after drawto command");
        }



    }
}
