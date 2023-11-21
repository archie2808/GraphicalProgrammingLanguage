using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1;
using System;
using System.Drawing.Drawing2D;

namespace ProgramTesting
{
    [TestClass]
    public class DrawingTests1
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
            /// Tests the MoveToCommand method in the CommandParser class to ensure the pen position is being moved as required
            /// </summary>
            /// <remarks>
            /// This test verifies that when a moveto command is called, the pen position is being correctly moved to the approprite destination
            /// </remarks>
            [TestMethod]
            public void MoveToCommand_MovesPen()
            {
                // Act
                commandParser.ExecuteCommand("moveto 10 10");
                

                // Assert
                Assert.AreEqual(new Point(10, 10), commandParser.PenPosition, "The pen position should be moved to (10, 10)");

            }

            /// <summary>
            /// Tests the 'drawto' command of the CommandParser.
            /// </summary>
            /// <remarks>
            /// This test verifies that the 'drawto' command correctly draws a line from the current
            /// pen position to a specified point. It moves the pen to a starting position, executes
            /// the 'drawto' command to a new position, and then checks if the pixel at the new position
            /// has the expected color, indicating that the line was drawn correctly.
            /// </remarks>
            [TestMethod]
            public void DrawToCommand_DrawsCorrectly()
            {
         
                var expectedColour = Color.Blue.ToArgb();

                // Act
                commandParser.ExecuteCommand("drawto 10 10");

                //Assert
                int actualColor = drawingSurface.GetPixel(10, 10).ToArgb();
                Assert.AreEqual(expectedColour, actualColor, "Pixel at (10, 10) should be colored after drawto command");
            }
          
            /// <summary>
            /// Tests functionality of rest command
            /// </summary>
            /// <remarks>
            /// This test verifies that the Reset Command correctly resets the pen position to the origin (0, 0).
            /// The test first moves the pen to a non-origin position (10, 10) and then executes the reset command.
            /// It asserts that the pen position is reset to (0, 0) after the reset command is executed.
            /// </remarks>
            [TestMethod]
            public void ResetCommand_ResetsPen()
            {
            
                // Act
                commandParser.ExecuteCommand("moveto 10 10");
                commandParser.ExecuteCommand("reset");

                // Assert
                Assert.AreEqual(new Point(0, 0), commandParser.PenPosition, "The pen position should be reset to (0, 0)");
           
            }
        /// <summary>
        /// Tests the rectangle command of the command parser
        /// </summary>
        /// <remarks>
        /// the test moves the pen to a specified start position and then executes the rectangle command with
        /// gvien width and heigh. It asserts that the rectangle is drawn correctly by checking the colour of the pixels at the corners
        /// </remarks>
        [TestMethod]
        public void RectangleCommand_DrawsRectangle()
        {
            // Arrange
            var expectedColor = Color.Blue.ToArgb(); 
            Point startPosition = new Point(50, 50);
            int width = 10, height = 10;

            
            commandParser.ExecuteCommand($"moveto {startPosition.X} {startPosition.Y}");

            // Act
            commandParser.ExecuteCommand($"rectangle {width} {height}");

            // Assert
            Assert.AreEqual(expectedColor, drawingSurface.GetPixel(startPosition.X, startPosition.Y).ToArgb(), "Top left corner should be coloured");
            Assert.AreEqual(expectedColor, drawingSurface.GetPixel(startPosition.X + width - 1, startPosition.Y).ToArgb(), "Top right corner should be coloured");
            Assert.AreEqual(expectedColor, drawingSurface.GetPixel(startPosition.X, startPosition.Y + height - 1).ToArgb(), "Bottom left corner should be coloured");
            
        }

        /// <summary>
        /// Test circle command by verifying colour of pixels
        /// </summary>
        /// <remarks>
        /// The method moves the pen to a specified location and draws a circle, it then checks if the 
        /// pixels at the edge of the circle have the specified colour
        /// </remarks>
        [TestMethod]
        public void CircleCommand_DrawsCircle()
        {
            // Arrange
            var center = new Point(200, 200);
            int radius = 10;
            var expectedColour = Color.Blue.ToArgb();


            //Act
            commandParser.ExecuteCommand("moveto 200 200 ");
            commandParser.ExecuteCommand($"circle {radius}");
            

            // Assert
            
            Assert.AreEqual(expectedColour, drawingSurface.GetPixel(center.X + radius, center.Y).ToArgb(), "Pixel on the right edge of the circle should be coloured");
            Assert.AreEqual(expectedColour, drawingSurface.GetPixel(center.X, center.Y + radius).ToArgb(), "Pixel on the bottom edge of the circle should be coloured");
            Assert.AreEqual(expectedColour, drawingSurface.GetPixel(center.X - radius, center.Y).ToArgb(), "Pixel on the left edge of the circle should be coloured");
            Assert.AreEqual(expectedColour, drawingSurface.GetPixel(center.X, center.Y - radius).ToArgb(), "Pixel on the top edge of the circle should be coloured");

        }
    }
}





