using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    public static class CommandFactory
    {
        private Point currentPenPosition
        public static ICommand CreateCommand(string commandString, DrawingManager drawingManager, VariableManager variableManager, Point currentPenPosition)
        {
            var parts = commandString.Split(' ');
            var commandName = parts[0].ToLower();
            var arguments = parts.Skip(1).ToArray();

            switch (commandName)
            {
                case "moveto":
                    return CreateMoveToCommand(arguments, variableManager, ref currentPenPosition);

                case "drawto":
                    return CreateDrawToCommand(arguments, drawingManager, variableManager, ref currentPenPosition);

                case "rectangle":
                    return CreateRectangleCommand(arguments, drawingManager, variableManager, currentPenPosition);

                case "circle":
                    return CreateCircleCommand(arguments, drawingManager, variableManager, currentPenPosition);

                case "triangle":
                    return CreateTriangleCommand(arguments, drawingManager, variableManager, currentPenPosition);

                case "colour":
                    return CreateColourCommand(arguments, drawingManager);

                default:
                    throw new InvalidOperationException($"Unknown command: {commandName}");
            }
        }

        private static ICommand CreateMoveToCommand(string[] args, VariableManager variableManager, ref Point currentPenPosition)
        {
            if (args.Length < 2) throw new ArgumentException("Insufficient arguments for 'moveto' command.");

            int x = ResolveArgumentToInteger(args[0], variableManager);
            int y = ResolveArgumentToInteger(args[1], variableManager);

            return new MoveToCommand(ref currentPenPosition, new Point(x, y));
        }

        private static ICommand CreateDrawToCommand(string[] args, DrawingManager drawingManager, VariableManager variableManager, Point currentPenPosition)
        {
            if (args.Length < 2) throw new ArgumentException("Insufficient arguments for 'drawto' command.");

            int x = ResolveArgumentToInteger(args[0], variableManager);
            int y = ResolveArgumentToInteger(args[1], variableManager);

            return new DrawToCommand(drawingManager, currentPenPosition, new Point(x, y));
        }

        private static ICommand CreateCircleCommand(string[] args, DrawingManager drawingManager, VariableManager variableManager, Point currentPenPosition)
        {
            if (args.Length < 1) throw new ArgumentException("Insufficient arguments for 'circle' command.");

            int radius = ResolveArgumentToInteger(args[0], variableManager);

            return new CircleCommand(drawingManager, currentPenPosition, radius);
        }

        private static ICommand CreateColourCommand(string[] args, DrawingManager drawingManager)
        {
            if (args.Length < 1) throw new ArgumentException("Insufficient arguments for 'colour' command.");

            string colourName = args[0].Trim();

            return new ColourCommand(drawingManager, colourName);
        }

        private static ICommand CreateRectangleCommand(string[] args, DrawingManager drawingManager, VariableManager variableManager, Point currentPenPosition)
        {
            if (args.Length < 2) throw new ArgumentException("Insufficient arguments for 'rectangle' command.");

            int width = ResolveArgumentToInteger(args[0], variableManager);
            int height = ResolveArgumentToInteger(args[1], variableManager);

            return new RectangleCommand(drawingManager, currentPenPosition, width, height);
        }

        private static ICommand CreateTriangleCommand(string[] args, DrawingManager drawingManager, VariableManager variableManager, Point currentPenPosition)
        {
            if (args.Length < 3) throw new ArgumentException("Insufficient arguments for 'triangle' command.");

            int x = ResolveArgumentToInteger(args[0], variableManager);
            int y = ResolveArgumentToInteger(args[1], variableManager);
            int sideLength = ResolveArgumentToInteger(args[2], variableManager);

            Point startVertex = new Point(x, y);
            return new TriangleCommand(drawingManager, startVertex, sideLength);
        }

        private static int ResolveArgumentToInteger(string arg, VariableManager variableManager)
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




