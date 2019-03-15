using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace VisualScripting
{
    public partial class Form1 : Form
    {
        VisualScriptManager visualScriptManager;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            visualScriptManager = new VisualScriptManager(MainScriptingPanel);
        }

        private void MainScriptingPanel_MouseClick(object sender, EventArgs e)
        {
            visualScriptManager.MainScriptingPanelMouseClick(sender, e);
        }

        private void Form1_Resize(object sender, EventArgs e) //For resizing main form window
        {
            MainScriptingPanel.Size = new Size(this.Size.Width - 40, this.Size.Height - 60);
            MainScriptingPanel.Location = new Point(10, 10);
        }

        private void MainScriptingPanel_Paint(object sender, PaintEventArgs e) //Paint connections between pins
        {
            visualScriptManager.MainScriptingPanel_Paint(sender, e);
        }

        private void MainScriptingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            visualScriptManager.MainScriptingPanel_MouseMove(sender, e);
        }

        private void CompileButton_Click(object sender, EventArgs e)
        {
            visualScriptManager.CompileAllToString();
        }
    }
}
