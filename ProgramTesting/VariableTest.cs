using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using System.Drawing;

namespace ProgramTesting
{
    /// <summary>
    /// A Test class dedicated to ensuring correct functionality of variables. 
    /// </summary>
    [TestClass]
    public class VariableTest
    {
        
        private CommandParser commandParser;
        private TextBox outputTextBox;
        private Bitmap drawingSurface;
        private VariableManager vm;

        /// <summary>
        /// Setup the necassary dependancies 
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {

            outputTextBox = new TextBox();
            drawingSurface = new Bitmap(500, 500);

            vm = new VariableManager();
            commandParser = new CommandParser(outputTextBox, drawingSurface, vm);
            
        }

        /// <summary>
        /// The tests purpose is to ensure that variables and their values are being correctly assigned
        /// </summary>
        [TestMethod]
        public void VariableAssignmentAndRetrievalTest()
        {
            VariableManager vm = new VariableManager();
            vm.SetVariable("testVar", 100);
            int result = vm.GetVariable("testVar");

            Assert.AreEqual(100, result, "Variable should return the value it was set to.");

        }

        /// <summary>
        /// The Test ensures that the correct error handling is in place for calling undefined variables
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RetrievingUndefinedVariableTest()
        {
            VariableManager vm = new VariableManager();
            vm.GetVariable("undefinedVar");
        }

        /// <summary>
        /// The Test ensures that variables can be correctly assigned, called, and executed.
        /// </summary>
        [TestMethod]
        public void VariableUsageInCommands()
        {
            Point startPosition = new Point(0, 0);


            
            var expectedColor = Color.Blue.ToArgb();


    
            commandParser.ExecuteCommand("endX = 10");
            commandParser.ExecuteCommand("endY = 10");
            commandParser.ExecuteCommand("drawto endX endY");

         
            var actualColor = drawingSurface.GetPixel(5, 5).ToArgb();

          
            Assert.AreEqual(expectedColor, actualColor, "The pixel color should match the drawn line's color.");
        }
    }
}