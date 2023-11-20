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
        /// Draws an equilateral triangle on the drawing surface
        /// </summary>
        /// <param name="startVertex">The starting point of the triangles base</param>
        /// <param name="sideLength">The length of each side of the triangle</param>
        /// <remarks>
        /// This method draws a triangle with a specified base starting point and side length.
        /// The triangle is always oriented such that the base is horizontal.
        /// </remarks>
        public void DrawTriangle(Point startVertex, int sideLength)
        {
            Point secondVertex = new Point(
                startVertex.X - (int)(sideLength * Math.Cos(Math.PI / 3)),
                startVertex.Y + (int)(sideLength * Math.Sin(Math.PI / 3)));

            Point thirdVertex = new Point(
                startVertex.X + (int)(sideLength * Math.Cos(Math.PI / 3)),
                secondVertex.Y);

            using (Graphics g= Graphics.FromImage(drawingSurface))
            {
                g.DrawLine(Pens.HotPink, startVertex, secondVertex);
                g.DrawLine(Pens.HotPink, secondVertex, thirdVertex);
                g.DrawLine(Pens.HotPink, thirdVertex, startVertex);
            }
        }

        /// <summary>
        /// Draws Circle on the drawing surface
        /// </summary>
        /// <param name="center">The center point of the circle</param>
        /// <param name="radius">The radius of the circles</param>
        /// <remarks>
        /// This method draws a circle based on the specified radius, it checks 
        /// if the circle is within drawing surface bounds before execution. 
        /// </remarks>
        public void DrawCircle(Point center, int radius)
        {
            if (OutOfBoundsCircle(center, radius))
            {
                throw new ArgumentException("Circle Dimensions Out of bounds. Redraw circle Within bounds of drawing surface");
            }

            using (Graphics g = Graphics.FromImage(drawingSurface))
            {
                g.DrawEllipse(Pens.Blue, center.X - radius, center.Y - radius, radius * 2, radius * 2);
            }
        }

        /// <summary>
        /// Checks if a cirlce is out of the bounds of the drawing surface.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns>
        /// This method determines whether, based on the given center and radius, 
        /// would exceed the boundaries of the drawing surface.
        /// </returns>
        private bool OutOfBoundsCircle(Point center, int radius)
        {
            return (center.X - radius < 0) ||
                   (center.Y - radius < 0) ||
                   (center.X + radius > drawingSurface.Width) ||
                   (center.Y + radius > drawingSurface.Height);
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
                throw new ArgumentException("Rectangle dimensions out of bounds. Redraw rectangle within bounds of the drawing surface.");
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
        /// The method calculates if the rectangle defined by the start position, width and height
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
