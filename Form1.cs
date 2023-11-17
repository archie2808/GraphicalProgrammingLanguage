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
            try
            {
                string command = textBox1.Text; // Get the command from textBox1
                commandParser.ExecuteCommand(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 

            }
               

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

        /// <summary>
        /// Handles the text changed event of textBox1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>This method checks if the text entered in textBox1 is the 'run' command
        /// If so, it retrieves the program commands from textBox2 and executes them using CommandParser</remarks>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string command = textBox1.Text;
                if (command.Trim().ToLower() == "run")
                {
                    string program = textBox2.Text;
                    commandParser.ExecuteProgram(program);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the key down event of textBox1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// This method triggers when the enter key is pressed, it checks the command entered is 'run' and will exectue any program within textBox2.
        /// Otherwise it will treat the string value as an individual command to be executed. 
        /// </remarks>
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    string command = textBox1.Text.Trim().ToLower();
                    if (command == "run")
                    {
                        // Execute the program written in textBox2
                        string program = textBox2.Text;
                        commandParser.ExecuteProgram(program);
                    }
                    else
                    {
                        // Execute individual command
                        commandParser.ExecuteCommand(command);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}