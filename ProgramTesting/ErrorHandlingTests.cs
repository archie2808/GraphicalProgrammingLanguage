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
        private VariableManager vm;
        private CommandParser commandParser;

        [TestInitialize]
        public void Initialise()
        {
            outputTextBox = new TextBox();
            drawingSurface = new Bitmap(500, 500);
            commandParser = new CommandParser(outputTextBox, drawingSurface, vm);
        }
        /// <summary>
        /// Tests whether the 'ExecuteCommand' method throws an InvalidOperationException
        /// when an invalid command is passed. 
        /// </summary>
        /// <remarks>
        /// This test verifies the error handling capability
        /// of the command parsing logic, ensuring that it correctly identifies and rejects
        /// unrecognized commands.
        /// </remarks>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidCommandThrowsException()
        {
            //Arrange 
            string command = "movetooo 100 100";

            //Act
            commandParser.ExecuteCommand(command);
        }

        /// <summary>
        /// Tests the behavior of the ExecuteCommand method when provided with a command that has too many parameters.
        /// </summary>
        /// <remarks>
        /// An ArgumentException is expected to be thrown in this scenario.
        /// </remarks>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CommandWithTooFewParametersThrowsException()
        {
            // Arrange
            string command = "moveto 100"; 
            var commandParser = new CommandParser(outputTextBox, drawingSurface, vm);

            // Act
            commandParser.ExecuteCommand(command);

           
        }
        // <summary>
        /// Tests the behavior of the ExecuteCommand method when provided with a command that has too many parameters.
        /// </summary>
        /// <remarks>
        /// An ArgumentException is expected to be thrown in this scenario.
        /// </remarks>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CommandWithTooManyParametersThrowsException()
        {
            // Arrange
            string command = "circle 100 100 200"; 
            var commandParser = new CommandParser(outputTextBox, drawingSurface, vm);

            // Act
            commandParser.ExecuteCommand(command);

            
        }

    }
}
