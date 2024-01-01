using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    public class MoveToCommand : ICommand
    {
        
        private  Point newPosition;
        private Action<Point> updatePenPositionAction;

        public MoveToCommand(Action<Point> updatePenPositionAction, Point newPosition)
        {
            this.updatePenPositionAction = updatePenPositionAction;
            this.newPosition = newPosition;
        }

        public void Execute()
        {
            updatePenPositionAction(newPosition);
        }
    }
}
