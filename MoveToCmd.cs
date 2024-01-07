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
    public class MoveToCommand : ICommand
    {

        private UpdatePenPositionDelegate updatePenPosition;
        private Point newPosition;



        public MoveToCommand( Point newPosition, UpdatePenPositionDelegate updatePenPositionDelegate)
        {
            this.newPosition = newPosition;
            this.updatePenPosition = updatePenPositionDelegate;
        }

        public void Execute()
        {
            updatePenPosition(newPosition);
        }
    }
}
