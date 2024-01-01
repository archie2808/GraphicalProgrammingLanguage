using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    public class RectangleCommand : ICommand
    {
        private readonly DrawingManager drawingManager;
        private readonly Point currentPenPosition;  // Current pen position
        private readonly int width;  // Width of the rectangle
        private readonly int height;  // Height of the rectangle

        public RectangleCommand(DrawingManager drawingManager, Point currentPenPosition, int width, int height )
        {
            this.drawingManager = drawingManager;
            this.currentPenPosition = currentPenPosition;
            this.width = width;
            this.height = height;
        }

        public void Execute()
        {
            drawingManager.DrawRectangle(currentPenPosition, width, height);
        }
    }
}
