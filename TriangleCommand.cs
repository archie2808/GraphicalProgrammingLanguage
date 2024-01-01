using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    public class TriangleCommand : ICommand
    {
        private readonly DrawingManager drawingManager;
        private readonly Point startVertex;
        private readonly int sideLength;

        public TriangleCommand(DrawingManager drawingManager, Point startVertex, int sideLength)
        {
            this.drawingManager = drawingManager;
            this.startVertex = startVertex;
            this.sideLength = sideLength;
        }

        public void Execute()
        {
            drawingManager.DrawTriangle(startVertex, sideLength);
        }
    }

}
