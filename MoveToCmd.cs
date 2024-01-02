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
        
        private  Point newPosition;
        public Point NewPenPosition { get; private set; }

        public MoveToCommand( Point newPosition)
        {
          
            this.newPosition = newPosition;
        }

        public void Execute()
        {
            NewPenPosition = newPosition;
        }
    }
}
