using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    /// <summary>
    /// The main form of the application, responsible for handling user interactions and displaying the graphical interface.
    /// </summary>
    public partial class Form1 : Form
    {
        private CommandParser commandParser;
        private Bitmap drawingSurface;

        /// <summary>
        /// Initialized a new instance of the <c>Form1</c> class
        /// </summary>
        /// <remarks>
        /// This constructor intializes the componenents of the form, sets up the drawing surface,
        /// and creates an instance of the Command Parser class
        /// </remarks>
        public Form1()
        {
            InitializeComponent();
            drawingSurface = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = drawingSurface;
            commandParser = new CommandParser(textBox2, drawingSurface);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the click event of the 'run' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// This method retrieves the command from textBox1, executes it using the command parser, and then reinitialises 
        /// the command parser with the updated drawing surface
        /// </remarks>
        private void button1_Click(object sender, EventArgs e)
        {
            string command = textBox1.Text; // Get the command from textBox1
            commandParser.ExecuteCommand(command); // Parses textBox2 for output
            commandParser = new CommandParser(textBox2, drawingSurface);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        
        /// <summary>
        /// Handles the Paint event of the picture box control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// This method redraws the image on the drawing surface whenever the picture box is repainted. 
        /// </remarks>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(drawingSurface, 0, 0);
            pictureBox1.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}