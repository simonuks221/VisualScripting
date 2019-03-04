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

        List<BaseNode> currentNodes;

        BasePin firstSelectedPin;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            currentNodes = new List<BaseNode>();
            firstSelectedPin = null;
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

            currentNodes.Add(newNode);

            newNode.MouseDown += StartMovingNode;
            newNode.pinPressed += PinPressed;

            if(createNodeSearchBar != null)
            {
                createNodeSearchBar.Dispose();
            }
        }

        private void StartMovingNode(object sender, MouseEventArgs e) //Start moving node
        {

            BaseNode nodeToMove = (BaseNode)sender;
            //nodeToMove.Location = new Point(0, 0);
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

        private void MainScriptingPanel_Paint(object sender, PaintEventArgs e) //Paint connections between pins
        {
            Graphics g;
            Control c = (Control)sender;
            g = c.CreateGraphics();
            Pen myPen = new Pen(Color.Black);
            myPen.Width = 2;
            foreach (BaseNode n in currentNodes)
            {
                foreach (BasePin p in n.inputPins) //Paint from input
                {
                    if (p.otherConnectedPin != null)
                    {
                        Point pLoc = MainScriptingPanel.PointToClient(n.PointToScreen(p.Location));
                        Point oLoc = MainScriptingPanel.PointToClient(p.otherConnectedPin.Parent.PointToScreen(p.otherConnectedPin.Location));
                        g.DrawLine(myPen, pLoc.X, pLoc.Y, oLoc.X, oLoc.Y);
                    }
                }
            }
            if(firstSelectedPin != null) //Draw line if second pin not selected
            {
                Point pLoc = MainScriptingPanel.PointToClient(firstSelectedPin.Parent.PointToScreen(firstSelectedPin.Location));
                Point mLoc = MainScriptingPanel.PointToClient(Control.MousePosition);
                g.DrawLine(myPen, pLoc.X, pLoc.Y, mLoc.X, mLoc.Y);
            }

            g.Dispose();
            myPen.Dispose();
        }

        void PinPressed(BasePin _pinPressed)
        {
            if(firstSelectedPin == null)
            {
                firstSelectedPin = _pinPressed;
            }
            else
            {
                _pinPressed.otherConnectedPin = firstSelectedPin;
                firstSelectedPin.otherConnectedPin = _pinPressed;
                firstSelectedPin = null;
                MainScriptingPanel.Refresh();
            }
        }

        private void MainScriptingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if(firstSelectedPin != null)
            {
                MainScriptingPanel.Refresh();
            }
        }
    }
}
