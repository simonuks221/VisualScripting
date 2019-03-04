using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualScripting
{
    public partial class Form1 : Form
    {
        CreateNodeSearchBar createNodeSearchBar;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void MainScriptingPanel_MouseClick(object sender, MouseEventArgs e) //Show node search bar
        {
            if (e.Button == MouseButtons.Right)
            {
                if (createNodeSearchBar != null)
                {
                    createNodeSearchBar.Dispose();
                }
                createNodeSearchBar = new CreateNodeSearchBar(e.Location);
                MainScriptingPanel.Controls.Add(createNodeSearchBar);
                createNodeSearchBar.partPressed += SpawnNode;
            }
        }

        void SpawnNode(Type _nodeType, Point _position) //Spawn node
        {
            BaseNode newNode = new BaseNode();
            MainScriptingPanel.Controls.Add(newNode);
            newNode.Location = _position;

            newNode.MouseDown += StartMovingNode;

            if(createNodeSearchBar != null)
            {
                createNodeSearchBar.Dispose();
            }
        }

        private void StartMovingNode(object sender, MouseEventArgs e) //Start moving node
        {

            BaseNode nodeToMove = (BaseNode)sender;
            nodeToMove.Location = new Point(0, 0);
                // Console.Out.WriteLine(sender);
                //nodeToMove.Location = new Point(0, 0);
                //nodeToMove.Moves();
               
                //MainScriptingPanel.Update();
            
        }

        private void Form1_Resize(object sender, EventArgs e) //For resizing main form window
        {
            MainScriptingPanel.Size = new Size(this.Size.Width - 40, this.Size.Height - 60);
            MainScriptingPanel.Location = new Point(10, 10);
        }
    }
}
