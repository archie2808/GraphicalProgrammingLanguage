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
    public class DrawToCommand : ICommand
    {
        private readonly DrawingManager drawingManager;
        private readonly Point start;
        private readonly Point end;

        public DrawToCommand(DrawingManager drawingManager, Point start, Point end)
        {
            this.drawingManager = drawingManager;
            this.start = start;
            this.end = end;
        }

        public void Execute()
        {
            drawingManager.DrawLine(start, end);
        }
    }

}
