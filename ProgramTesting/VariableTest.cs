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
    /// A Test class dedicated to ensuring correct functionality of variables within the drawing application.
    /// </summary>
    [TestClass]
    public class VariableTest
    {
        private VariableManager vm;
        private DrawingManager drawingManager;
        private Bitmap drawingSurface;

        /// <summary>
        /// Sets up the necessary dependencies before each test is run.
        /// Initializes instances of VariableManager, DrawingManager, and a Bitmap for the drawing surface.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            vm = new VariableManager();
            drawingSurface = new Bitmap(500, 500);
            drawingManager = new DrawingManager(drawingSurface);
        }

        /// <summary>
        /// Tests whether variables can be correctly assigned and retrieved using the VariableManager.
        /// </summary>
        [TestMethod]
        public void VariableAssignmentAndRetrievalTest()
        {
            vm.SetVariable("testVar", 100);
            int result = vm.GetVariable("testVar");

            Assert.AreEqual(100, result, "Variable should return the value it was set to.");
        }
        /// <summary>
        /// Tests that an exception is thrown when attempting to retrieve an undefined variable.
        /// Expects an InvalidOperationException to be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RetrievingUndefinedVariableTest()
        {
            vm.GetVariable("undefinedVar");
        }

        /// <summary>
        /// Tests the usage of variables in command execution.
        /// Sets variables, creates a DrawToCommand with these variables, and executes it.
        /// Verifies that the drawing was performed at the expected location by checking the pixel color.
        /// </summary>
        [TestMethod]
        public void VariableUsageInCommands()
        {
            vm.SetVariable("endX", 10);
            vm.SetVariable("endY", 10);

            Point startPosition = new Point(0, 0);
            Point endPosition = new Point(vm.GetVariable("endX"), vm.GetVariable("endY"));
            ICommand drawToCommand = new DrawToCommand(drawingManager, startPosition, endPosition);
            drawToCommand.Execute();

            var expectedColor = Color.Blue.ToArgb();
            var actualColor = drawingSurface.GetPixel(5, 5).ToArgb();

            Assert.AreEqual(expectedColor, actualColor, "The pixel color should match the drawn line's color.");
        }
    }
}