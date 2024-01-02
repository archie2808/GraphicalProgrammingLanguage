using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    /// <summary>
    /// Responsible for creating and initialising instances of command classes.
    /// </summary>
    /// <remarks>
    /// This factory class abstacts the instantiation logic of the command objects, 
    /// therby decoupling command creation from the client code.
    /// </remarks>
    public static class CommandFactory
    {
        /// <summary>
        /// Creates a command object based on the provided command string and additional parameters
        /// </summary>
        /// <param name="commandString">The command String indicating the type of command to create</param>
        /// <param name="arguments">The arguments required for the commands execution</param>
        /// <param name="drawingManager">The drawing manager to be used bu the command for drawing</param>
        /// <param name="variableManager">The variable manager for managing command variables</param>
        /// <param name="penPosition">current position of the pen.</param>
        /// <returns>
        /// An instance of a class that implements the icommand interface, corresponding to a specified command.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown when an unknown command type is encountered.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided arguments are insufficient or invalid for the specified command.</exception>
        public static ICommand CreateCommand(string commandString,string[] arguments, DrawingManager drawingManager,
            VariableManager variableManager, Point penPosition)
        {
            var parts = commandString.Split(' ');
            var commandName = parts[0].ToLower();
            

            switch (commandName)
            {
                case "moveto":
                    return CreateMoveToCommand(arguments, variableManager);

                case "drawto":
                    return CreateDrawToCommand(arguments, drawingManager, variableManager, penPosition);

                case "rectangle":
                    return CreateRectangleCommand(arguments, drawingManager, variableManager, penPosition);

                case "circle":
                    return CreateCircleCommand(arguments, drawingManager, variableManager, penPosition);

                case "triangle":
                    return CreateTriangleCommand(arguments, drawingManager, variableManager, penPosition);

                case "colour":
                    return CreateColourCommand(arguments, drawingManager);
              
                    

                default:
                    throw new InvalidOperationException($"Unknown command: {commandName}");
            }
        }

        /// <summary>
        /// Processes the 'moveto' command, updating the pen position and drawing on the Bitmap
        /// </summary>
        /// <param name="commandParts">The parameters of the commanf, such as the co-oridinates</param>
        /// <remarks>
        /// This method updates the pen position based on the coordinates provided in the command.
        /// 
        /// </remarks>
        private static ICommand CreateMoveToCommand(string[] args, VariableManager variableManager)
        {
            if (args.Length < 2) throw new ArgumentException("Insufficient arguments for 'moveto' command.");

            int x = ResolveArgumentToInteger(args[0], variableManager);
            int y = ResolveArgumentToInteger(args[1], variableManager);

            return new MoveToCommand( new Point(x, y));
        }

        /// <summary>
        /// This method processes the draw to command, drawing a line from the current pen position to the specified co-ordinates
        /// </summary>
        /// <param name="commandParts">The parts of the command, including the drawto keyword and the x and y co-ordinates</param>
        /// <remarks>
        /// This  method interprets the 'drawto' command, extracts the destination coordinates, and draws a line from the current pen position to these coordinates.
        /// It updates the pen position to the new location after drawing the line.
        /// If the command is invalid (e.g., incorrect number of arguments or non-numeric coordinates), an error message is displayed.
        /// </remarks>
        private static ICommand CreateDrawToCommand(string[] args, DrawingManager drawingManager, VariableManager variableManager, Point penPosition)
        {
            if (args.Length < 2) throw new ArgumentException("Insufficient arguments for 'drawto' command.");

            int x = ResolveArgumentToInteger(args[0], variableManager);
            int y = ResolveArgumentToInteger(args[1], variableManager);

            return new DrawToCommand(drawingManager, penPosition, new Point(x, y));
        }

        /// <summary>
        /// Processes the circle command to draw a circle on the drawing surface.
        /// </summary>
        /// <param name="commandParts"></param>
        /// <remarks>
        /// The method interprets the circle command, extracts the radius and calls the DrawCircle method 
        /// to draw the circle on the drawing surface.
        /// </remarks>
        private static ICommand CreateCircleCommand(string[] args, DrawingManager drawingManager, VariableManager variableManager, Point penPosition)
        {
            if (args.Length < 1) throw new ArgumentException("Insufficient arguments for 'circle' command.");

            int radius = ResolveArgumentToInteger(args[0], variableManager);

            return new CircleCommand(drawingManager, penPosition, radius);
        }

        /// <summary>
        /// Processes the 'colour' command to change the pens drawing colour
        /// </summary>
      
        /// <remarks>
        /// This method interprets the 'colour' command, extracts the colour name, and changes the pen colour in the drawing manager.
        /// If the command is invalid (e.g., incorrect number of arguments or unrecognized colour name), an error message is displayed.
        /// </remarks>
        private static ICommand CreateColourCommand(string[] args, DrawingManager drawingManager)
        {
            if (args.Length < 1) throw new ArgumentException("Insufficient arguments for 'colour' command.");

            string colourName = args[0].Trim();

            return new ColourCommand(drawingManager, colourName);
        }

        /// <summary>
        /// Processes the rectangle command to draw a rectangle onto drawing surface
        /// </summary>
        
        /// <remarks>
        /// The method interprets the rectangle command, extracts the width and height parameters, 
        /// and then calls the drawing manager to a rectangle. The rectangle is drawn with its top 
        /// left corner at the current pen position
        /// </remarks>
        private static ICommand CreateRectangleCommand(string[] args, DrawingManager drawingManager, VariableManager variableManager, Point penPosition)
        {
            if (args.Length < 2) throw new ArgumentException("Insufficient arguments for 'rectangle' command.");

            int width = ResolveArgumentToInteger(args[0], variableManager);
            int height = ResolveArgumentToInteger(args[1], variableManager);

            return new RectangleCommand(drawingManager, penPosition, width, height);
        }

        /// <summary>
        /// Process the triangle command to draw a triangle on the drawing surface.
        /// </summary>
        /// <param name="commandParts"></param>
        /// <remarks>
        /// This methods interprets the 'triangle' command, extracts the base co-ordinates and length, and 
        /// instructs the drawing manager to draw a triangle. Throws excpetion if triangle command is not inputted correctly.
        /// </remarks>
        private static ICommand CreateTriangleCommand(string[] args, DrawingManager drawingManager, VariableManager variableManager, Point penPosition)
        {
            if (args.Length < 3) throw new ArgumentException("Insufficient arguments for 'triangle' command.");

            int x = ResolveArgumentToInteger(args[0], variableManager);
            int y = ResolveArgumentToInteger(args[1], variableManager);
            int sideLength = ResolveArgumentToInteger(args[2], variableManager);

            Point startVertex = new Point(x, y);
            return new TriangleCommand(drawingManager, startVertex, sideLength);
        }

        /// <summary>
        /// Resolves a command argument to an integer value
        /// </summary>
        /// <param name="arg">The command argument, which can be a variable name or a direct integer</param>
        /// <remarks>
        /// The method checks if the provided argument is a defined variable in the variable manager class, if so it retrieves the variables value.
        /// if the argument is not a variable, it attemps to parse the argument as a direct integer. If neither condition is met, an exception is thrown. 
        /// </remarks>
        public static int ResolveArgumentToInteger(string arg, VariableManager variableManager)
        {
            if (variableManager.IsVariableDefined(arg))
            {
                return variableManager.GetVariable(arg);
            }
            else if (int.TryParse(arg, out int result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException($"Invalid argument: {arg}");
            }
        }
    }
}




