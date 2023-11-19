using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class DrawingManager
    {
        private readonly Bitmap drawingSurface;
        private Point penPosition;

        public DrawingManager(Bitmap surface)
        {
            drawingSurface = surface;
        
        }
        /// <summary>
        /// Draws a Rectangle on the drawing surface
        /// </summary>
        /// <param name="startPosition">Starting point of the rectangle</param>
        /// <param name="width">Width of rectangle</param>
        /// <param name="height">Height of rectangle</param>
        /// <remarks>
        /// This method draws a rectangle based on the specified start point, width and height.
        /// it checks if the rectangle dimensions are within the bounds of the drawing surface.
        /// if the Rectangle is out of bound, an ArgumentException is thrown
        /// </remarks>
        public void DrawRectangle(Point startPosition, int width, int height)
        {
            if (OutOfBoundsRectangle(startPosition, width, height))
            {
                throw new ArgumentException("Rectangle dimensions out of bounds. Redraw rectangle within bounds of the Display Box");
            }
            
            
            using (Graphics g = Graphics.FromImage(drawingSurface))
            {
                g.DrawRectangle(Pens.Black, startPosition.X, startPosition.Y, width, height);
            }
        }
        /// <summary>
        /// Checks if the rectangle dimensions exceed the boundaries of the drawing surface
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>
        /// Returns true if the rectangle is out of bounds, otherwise fails
        /// </returns>
        /// <remarks>
        /// THe method calculates if the rectangle defined by the start position, width and height
        /// would extend beyond the edges of the drawing surface. It also checks if the start position is negative
        /// </remarks>
        private bool OutOfBoundsRectangle(Point startPosition, int width, int height)
        {
            return startPosition.X + width > drawingSurface.Width ||
                   startPosition.Y + height > drawingSurface.Height ||
                   startPosition.X < 0 || startPosition.Y < 0;
        }


        /// <summary>
        /// Draws a line on the drawing surface from a specified start point to an end point
        /// </summary>
        /// <param name="start">the starting point of a line</param>
        /// <param name="end">the ending point of a line</param>
        /// <remarks>
        /// This method used a graphics object from the Bitmap to draw a line. 
        /// </remarks>
        public void DrawLine(Point start, Point end)
        {
            using (Graphics g = Graphics.FromImage(drawingSurface))
            {
                g.DrawLine(Pens.Black, start, end);
            }
        }

        /// <summary>
        /// Draws on bitmaps surface at the current pen position
        /// </summary>
        /// <remarks>
        /// This method is called after updating the pen position to visually represent the pens's 
        /// new location on the bitmap. It currently draws a small red circle at the pen position.
        /// </remarks>
        public void DrawOnBitmap()
        {
            using (Graphics g = Graphics.FromImage(drawingSurface))
            {
                g.FillEllipse(Brushes.Red, penPosition.X - 2, penPosition.Y - 2, 4, 4);
            }

        }
    }
}
