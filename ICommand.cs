using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    /// <summary>
    /// Defines a contract for command execution
    /// </summary>
    /// <remarks>
    /// The interface is implemented by all command classes, ensuring a standardised
    /// approach to executing drawing commands.
    /// </remarks>
    public interface ICommand
    {
        void Execute();
    }
}
