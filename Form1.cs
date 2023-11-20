using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    /// <summary>
    /// The main form of the application, responsible for handling user interactions and displaying the graphical interface.
    /// </summary>
    public partial class Form1 : Form
    {
        private CommandParser commandParser;
       
        private Bitmap drawingSurface;
        private string defaultDirectory;

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
            defaultDirectory = @"C:\Users\archi\OneDrive - Leeds Beckett University\YEAR 3\ASE\SCRIPTS"; 
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
        /// This method retrieves the command from textBox1. If the command is 'clear', it clears the drawing surface.
        /// Otherwise, it executes the command using the command parser. This method also handles any exceptions that
        /// might occur during command execution and reports them to a user in a label UI element.
        /// </remarks>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string command = textBox1.Text.Trim().ToLower(); // Get the command from textBox1

                if (command == "clear")
                {
                    ClearDrawingSurface();
                }
                else
                {
                    commandParser.ExecuteCommand(command);
                }
            }
            catch (InvalidOperationException ex)
            {
                errorLabel.Text = ex.Message;
            }

        }

        /// <summary>
        /// Clears and resets drawing surface
        /// </summary>
        /// <remarks>
        /// This method disposes of the current drawing surface and creates a new Bitmap with the same dimensions.
        /// It then updates the PictureBox to reflect the cleared drawing surface. This method is called when the
        /// 'clear' command is executed.
        /// </remarks>
        private void ClearDrawingSurface()
        {
            drawingSurface.Dispose();
            drawingSurface = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = drawingSurface;
            pictureBox1.Refresh();
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
            Refresh();
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
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Processes a given command string. If the command is 'run', it executes a script from textbox2
        /// otherwise it executes the command using the commandParser
        /// </summary>
        /// <param name="command"></param>
        /// <remarks>
        /// This method abstracts the command processing logic away from the UI event, making it more testable
        /// </remarks>
        public void ProcessRunCommand(string command)
        {
            if (command == "run")
            {

                string script = textBox2.Text;


                commandParser.ExecuteScript(script);
            }
            else
            {

                try
                {
                    commandParser.ExecuteCommand(command);
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error executing command '{command}': {ex.Message}";

                }
            }
        }
        /// <summary>
        /// Handles the key down event of textBox1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// This event handler captures the Enter key press in textBox1 and initiates the processing of the command
        /// entered by the user. The actual command processing logic is delegated to the ProcessCommand method
        /// </remarks>
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string command = textBox1.Text.Trim().ToLower();
                    ProcessRunCommand(command);
                
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the click event of a save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// The method opens a Save File dialog allowing the user to save the contents of textBox2 to a .txt file
        /// </remarks>
        private void saveScript_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.AddExtension = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, textBox2.Text);
            }
        }

        /// <summary>
        /// Handles the click event of the load button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// The method opens a Open File dialog allowing the user to load a file from their file directory
        /// </remarks>
        private void loadScript_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = defaultDirectory;
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }
    }
}