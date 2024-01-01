using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    public class CircleCommand : ICommand
    {
        private readonly DrawingManager drawingManager;
        private readonly Point center;
        private readonly int radius;

        public CircleCommand(DrawingManager drawingManager, Point center, int radius)
        {
            this.drawingManager = drawingManager;
            this.center = center;
            this.radius = radius;
        }

        public void Execute()
        {
            drawingManager.DrawCircle(center, radius);
        }
    }

}
