﻿using System;
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
        BaseNode firstSelectedNode;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            currentNodes = new List<BaseNode>();
            firstSelectedPin = null;
            firstSelectedNode = null;
            SpawnNode(typeof(ConstructNode), new Point(50, 50));
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
            BaseNode newNode = (BaseNode)Activator.CreateInstance(_nodeType);
            MainScriptingPanel.Controls.Add(newNode);
            newNode.Location = _position;

            currentNodes.Add(newNode);

            newNode.MouseDown += StartMovingNode;
            newNode.MouseUp += StopMovingNode;
            newNode.pinPressed += PinPressed;
            newNode.MouseMove += MainScriptingPanel_MouseMove;

            if(createNodeSearchBar != null)
            {
                createNodeSearchBar.Dispose();
            }
        }

        private void StopMovingNode(object sender, MouseEventArgs e) //Stop moving node
        {
            firstSelectedNode = null;
            MainScriptingPanel.Refresh();
        }

        private void StartMovingNode(object sender, MouseEventArgs e) //Start moving node
        {
            firstSelectedNode = (BaseNode)sender;
            MainScriptingPanel.Refresh();
        }

        private void Form1_Resize(object sender, EventArgs e) //For resizing main form window
        {
            MainScriptingPanel.Size = new Size(this.Size.Width - 40, this.Size.Height - 60);
            MainScriptingPanel.Location = new Point(10, 10);
        }

        private void MainScriptingPanel_Paint(object sender, PaintEventArgs e) //Paint connections between pins
        {
            Graphics g;
            g = MainScriptingPanel.CreateGraphics();
            Pen myPen = new Pen(Color.Black);
            myPen.Width = 2;
            foreach (BaseNode n in currentNodes) //Painting lines
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

            if(firstSelectedNode != null) //Moving node
            {
                firstSelectedNode.Location = MainScriptingPanel.PointToClient(Control.MousePosition);
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
                if (_pinPressed.pinType == firstSelectedPin.pinType && _pinPressed.pinRole != firstSelectedPin.pinRole)
                {
                    _pinPressed.otherConnectedPin = firstSelectedPin;
                    firstSelectedPin.otherConnectedPin = _pinPressed;
                    firstSelectedPin = null;
                    MainScriptingPanel.Refresh();
                }
            }
        }

        private void MainScriptingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            MainScriptingPanel.Refresh();
        }

        private void CompileButton_Click(object sender, EventArgs e)
        {
            CompileAllToString();
        }

        void CompileAllToString()
        {
            string allCode = @"
                using System; 
                using System.Collections.Generic; 
                using System.Linq;
                using System.Text;
                using System.Threading.Tasks;

                namespace NewProgram
                {
                    class Program
                    {
                        static void Main(string[] args)
                        {
                        
                        ";
            allCode += currentNodes[0].CompileToString();
            allCode += @"
                        }
                    }
                }";

            Console.Out.WriteLine(allCode);
        }
    }
}
