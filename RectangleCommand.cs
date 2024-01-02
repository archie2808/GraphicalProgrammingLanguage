using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    /// <summary>
    /// Sets the necassary state and initialises required dependancies
    /// </summary>
    public class RectangleCommand : ICommand
    {
        private readonly DrawingManager drawingManager;
        private readonly Point PenPosition;  // Current pen position
        private readonly int width;  // Width of the rectangle
        private readonly int height;  // Height of the rectangle

        public RectangleCommand(DrawingManager drawingManager, Point PenPosition, int width, int height )
        {
            this.drawingManager = drawingManager;
            this.PenPosition = PenPosition;
            this.width = width;
            this.height = height;
        }

        public void Execute()
        {
            drawingManager.DrawRectangle(PenPosition, width, height);
        }
    }
}
