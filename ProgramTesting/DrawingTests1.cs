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
        [TestClass]
        public class DrawingTests
        {

            /// <summary>
            /// Tests the MoveToCommand method in the CommandParser class to ensure the pen position is being moved as required
            /// </summary>
            /// <remarks>
            /// This test verifies that when a moveto command is called, the pen position is being correctly moved to the approprite destination
            /// </remarks>
            [TestMethod]
            public void MoveToCommand_MovesPen()
            {
                // Arrange
                var outputTextBox = new TextBox();
                var drawingSurface = new Bitmap(50, 50);
                var commandParser = new CommandParser(outputTextBox, drawingSurface);


                // Act
                commandParser.ExecuteCommand("moveto 10 10");
                

                // Assert
                Assert.AreEqual(new Point(10, 10), commandParser.PenPosition, "The pen position should be moved to (10, 10)");

            }
            /// <summary>
            /// Tests the Clear command functionality in the form class
            /// </summary>
            /// <remarks>
            /// THe methods verifies the clear command correctly clears the drawing surface. it does this by first 
            /// drawing on the surface, executing the clear command, and checking if the surface is cleared
            /// by making sure the colour of the drawing surface is transparent. 
            /// </remarks>
            [TestMethod]
            public void TestClearCommand()
            {
                //arrange
                var outputTextBox = new TextBox();
                var drawingSurface = new Bitmap(50, 50);
                var commandParser = new CommandParser(outputTextBox, drawingSurface);

                //act
                commandParser.ExecuteCommand("drawto 50 50");
                commandParser.ExecuteCommand("clear");

                // Assert
                bool isCleared = true;
                for (int x = 0; x < drawingSurface.Width; x++)
                {
                    for (int y = 0; y < drawingSurface.Height; y++)
                    {
                        if (drawingSurface.GetPixel(x, y) != Color.FromArgb(0, 0, 0, 0))
                        {
                            isCleared = false;
                            break;
                        }
                    }
                    if (!isCleared) break;
                }

                Assert.IsTrue(isCleared, "The drawing surface was not cleared correctly.");
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
                // Arrange
                var outputTextBox = new TextBox();
                var drawingSurface = new Bitmap(50, 50);
                var commandParser = new CommandParser(outputTextBox, drawingSurface);


                // Act
                commandParser.ExecuteCommand("moveto 10 10");
                commandParser.ExecuteCommand("reset");

                // Assert
                Assert.AreEqual(new Point(0, 0), commandParser.PenPosition, "The pen position should be reset to (0, 0)");
                   
            }
        }
    }
}
