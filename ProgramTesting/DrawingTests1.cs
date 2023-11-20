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
            /// Tests the MoveToCommand method in the CommandParser class.
            /// </summary>
            /// <remarks>
            /// This test verifies that executing the 'moveto' command correctly changes the color of the specified pixel on the drawing surface.
            /// The test checks if the pixel at the coordinates (50, 50) turns red after executing the command. 
            /// </remarks>
            [TestMethod]
            public void TestMoveToCommand()
            {
                // Arrange
                var outputTextBox = new TextBox();
                var drawingSurface = new Bitmap(100, 100);
                var commandParser = new CommandParser(outputTextBox, drawingSurface);
                string command = "moveto 50 50";

                // Act
                commandParser.ExecuteCommand(command); // Move pen position

                // Assert
                int expectedColor = Color.Red.ToArgb();
                int actualColor = drawingSurface.GetPixel(50, 50).ToArgb();

                Assert.AreEqual(expectedColor, actualColor);
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
            /// needs implementing correctly
            /// </summary>
            [TestMethod]
            public void TestResetCommand()
            {
                // Arrange
                var outputTextBox = new TextBox();
                var drawingSurface = new Bitmap(50, 50);
                var commandParser = new CommandParser(outputTextBox, drawingSurface);


                // Act
                commandParser.ExecuteCommand("moveto 10 10");
                commandParser.ExecuteCommand("reset");

                // Assert
                Assert.AreEqual(new Point(0, 0), commandParser.penPosition, "The pen position should be reset to (0, 0)");

            }
        }
    }
}
