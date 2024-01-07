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
    /// <summary>
    /// Manages drawing operations on a SubClass
    /// </summary>
    public class DrawingManager
    {
        private Bitmap drawingSurface;
        private Pen currentPen;

        /// <summary>
        /// Initialises a new instance of the Drawing manager class
        /// </summary>
        /// <param name="surface">The Bitmap surface</param>
        /// <remarks>
        /// Sets up the initial state of the drawing manager.
        /// </remarks>
        public DrawingManager(Bitmap surface)
        {
            drawingSurface = surface;
            currentPen = new Pen(Color.Blue);
        
        }

        /// <summary>
        /// Updates the drawing surface with a new Bitmap.
        /// </summary>
        /// <param name="newSurface">The new Bitmap to be used as the drawing surface.</param>
        public void UpdateDrawingSurface(Bitmap newSurface)
        {

        
            drawingSurface = newSurface;
        }
       
        /// <summary>
        /// Changes the colour of the pen. 
        /// </summary>
        /// <param name="colorName"></param>
        /// <remarks>
        /// The method changes the colour of the pen to the specified colour name.
        /// If the colour name is not recognised, it throws an ArguementException
        /// </remarks>
        public void ChangePenColor(string colorName)
        {
            Color newColor;
            
            newColor = Color.FromName(colorName);
            if (newColor.IsKnownColor)
            {
                currentPen = new Pen(newColor);
            }
                
            else
            {
                throw new ArgumentException($"Unknown color: {colorName}");
            }
            

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
                g.DrawLine(currentPen, startVertex, secondVertex);
                g.DrawLine(currentPen, secondVertex, thirdVertex);
                g.DrawLine(currentPen, thirdVertex, startVertex);
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
            

            using (Graphics g = Graphics.FromImage(drawingSurface))
            {
                g.DrawEllipse(currentPen, center.X - radius, center.Y - radius, radius * 2, radius * 2);
            }
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
                g.DrawRectangle(currentPen, startPosition.X, startPosition.Y, width, height);
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
                g.DrawLine(currentPen, start, end);
            }
        }

      

    }
}
