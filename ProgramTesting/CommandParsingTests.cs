using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1;
using System;
using System.Drawing.Drawing2D;
using System.IO;

//namespace ProgramTesting
//{
  /*  [TestClass]
    public class CommandParsingTests
    {
        private Form1 form;
        private TextBox outputTextBox;
        private Bitmap drawingSurface;
        private VariableManager vm;
        private CommandParser commandParser;
        



        [TestInitialize]
        public void Initialise()
        {
            form = new Form1();
            outputTextBox = new TextBox();
            drawingSurface = new Bitmap(500, 500);
            commandParser = new CommandParser(outputTextBox, drawingSurface, vm);
            form.SetDrawingSurfaceForTest(drawingSurface);
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

        /// <summary>
        /// Tests whether the 'run' command correctly executes a script and reflects changes on the drawing surface.
        /// </summary>
        /// <remarks>
        /// This test method performs the following:
        /// 1. Initializes the main form (Form1) which includes the drawing surface and script input.
        /// 2. Loads a script from a specified file path and sets it in the form's script input area.
        /// 3. Executes the 'run' command to process the loaded script.
        /// 4. Refreshes the drawing surface to ensure the latest state is rendered.
        /// 5. Asserts that the drawing surface has the expected changes (e.g., a specific pixel color at a certain location).
        /// This test assumes that the script file contains valid commands that will result in visible changes on the drawing surface.
        /// </remarks>
        [TestMethod]
        public void RunCommand_ShouldExecuteProgram()
        {
            // Arrange
            var form = new Form1(); 
            string scriptFilePath = @"C:\Users\archi\OneDrive - Leeds Beckett University\YEAR 3\ASE\SCRIPTS\2.txt";
            string script = File.ReadAllText(scriptFilePath);

            string runCommand = "run";

            // Simulate setting the script in textBox2
            form.SetScriptForTest(script);

            // Act
            form.ProcessRunCommand(runCommand);
            form.RefreshDrawingSurfaceForTest();
           
            int expectedColor = Color.Blue.ToArgb(); 
            Assert.AreEqual(expectedColor, drawingSurface.GetPixel(100, 100).ToArgb());
        }

    }
}*/
