using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    public class ColourCommand : ICommand
    {
        private readonly DrawingManager drawingManager;
        private readonly string colourName;

        public ColourCommand(DrawingManager drawingManager, string colourName)
        {
            this.drawingManager = drawingManager;
            this.colourName = colourName;
        }

        public void Execute()
        {
            drawingManager.ChangePenColor(colourName);
        }
    }

}
