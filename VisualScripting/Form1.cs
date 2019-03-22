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
        public ProjectManager projectManager;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            projectManager = new ProjectManager(this);
            Form1_Resize(sender, e);
        }

        private void MainScriptingPanel_MouseClick(object sender, EventArgs e)
        {
            projectManager.showingEditors[projectManager.currentEditorIndex].MainScriptingPanelMouseClick(sender, e);
        }

        private void Form1_Resize(object sender, EventArgs e) //For resizing main form window
        {
            MainScriptingPanel.Size = new Size(this.Size.Width - 40 - 126, this.Size.Height - 80);
            MainScriptingPanel.Location = new Point(126, 35);

            VariableAndFunctionPanel.Size = new Size(100, this.Size.Height - 60 - 93 - 100);
            VariableAndFunctionPanel.Location = new Point(11, 93);

            VariableFunctionInfoPanel.Location = new Point(11, VariableAndFunctionPanel.Location.Y + VariableAndFunctionPanel.Size.Height + 10);
            VariableFunctionInfoPanel.Size = new Size(100, MainScriptingPanel.Location.Y + MainScriptingPanel.Size.Height - VariableFunctionInfoPanel.Location.Y);

            NavigationPanel.Size = new Size(MainScriptingPanel.Size.Width, 24);
        }

        private void MainScriptingPanel_Paint(object sender, PaintEventArgs e) //Paint connections between pins
        {
            projectManager.showingEditors[projectManager.currentEditorIndex].MainScriptingPanel_Paint(sender, e);
        }

        private void MainScriptingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            projectManager.showingEditors[projectManager.currentEditorIndex].MainScriptingPanel_MouseMove(sender, e);
        }

        private void CompileButton_Click(object sender, EventArgs e)
        {
            projectManager.CompileAllToString();
        }

        private void NewVariableButton_Click(object sender, EventArgs e)
        {
            projectManager.showingEditors[projectManager.currentEditorIndex].AddNewVariableButtonPressed();
        }

        private void NewFunctionButton_Click(object sender, EventArgs e)
        {
            //projectManager.showingEditors[projectManager.currentEditorIndex].AddNewVisualFunction();
        }

        private void AddNewClassButton_Click(object sender, EventArgs e)
        {
            projectManager.AddNewClass();
        }
    }
}
