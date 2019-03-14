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
        CreateNodeSearchBar createNodeSearchBar;

        List<BaseNode> currentNodes;
        public List<VisualVariable> visualVariables;

        BasePin firstSelectedPin;
        BaseNode firstSelectedNode;

        public List<Type> allNodesToShow = new List<Type>() { typeof(IfNode), typeof(PrintNode), typeof(MakeStringNode),typeof(MakeIntNode), typeof(ForLoopNode) };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            currentNodes = new List<BaseNode>();
            visualVariables = new List<VisualVariable>() { new VisualVariable(typeof(bool), "Naujas")};
            firstSelectedPin = null;
            firstSelectedNode = null;
            SpawnNode(new Point(50, 50), typeof(ConstructNode));
        }

        private void MainScriptingPanel_MouseClick(object sender, MouseEventArgs e) //Show node search bar
        {
            if (e.Button == MouseButtons.Right)
            {
                if (createNodeSearchBar != null)
                {
                    createNodeSearchBar.Dispose();
                }
                createNodeSearchBar = new CreateNodeSearchBar(e.Location, this);
                MainScriptingPanel.Controls.Add(createNodeSearchBar);
                createNodeSearchBar.partPressed += SpawnNode;

                firstSelectedNode = null;
                firstSelectedPin = null;
            }
            else
            {
                firstSelectedNode = null;
                firstSelectedPin = null;

                if(createNodeSearchBar != null)
                {
                    createNodeSearchBar.Dispose();
                }
                createNodeSearchBar = null;
            }
            MainScriptingPanel.Refresh();
        }

        void SpawnNode(Point _position, Type _nodeType = null, VisualVariable _visualVariable = null, VisualFunction _visualFunction = null) //Spawn node
        {
            if (_nodeType != null)
            {
                BaseNode newNode = (BaseNode)Activator.CreateInstance(_nodeType);
                MainScriptingPanel.Controls.Add(newNode);
                newNode.Location = _position;

                currentNodes.Add(newNode);

                newNode.MouseDown += StartMovingNode;
                newNode.MouseUp += StopMovingNode;
                newNode.MouseMove += MainScriptingPanel_MouseMove;

                for(int i = 0; i < newNode.inputPins.Count; i++)
                {
                    newNode.inputPins[i].pinPressed += PinPressed;
                }
                for (int i = 0; i < newNode.outputPins.Count; i++)
                {
                    newNode.outputPins[i].pinPressed += PinPressed;
                }

            }
            else if(_visualVariable != null)
            {
                VisualVariablePanel newVariable = new VisualVariablePanel(_visualVariable);
                MainScriptingPanel.Controls.Add(newVariable);
                newVariable.Location = _position;

                visualVariables.Add(_visualVariable);
                newVariable.outputPin.pinPressed += PinPressed;
            }


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
            Pen myPen = new Pen(BasePin.GetPinColor(typeof(ExecutionPin)));
            myPen.Width = 2;
            foreach (BaseNode n in currentNodes) //Painting lines
            {
                foreach (BasePin p in n.inputPins) //Paint from input
                {
                    if (p.otherConnectedPin != null)
                    {
                        myPen.Color = BasePin.GetPinColor(p.pinType);
                        Point pLoc = MainScriptingPanel.PointToClient(n.PointToScreen(p.Location));
                        Point oLoc = MainScriptingPanel.PointToClient(p.otherConnectedPin.Parent.PointToScreen(p.otherConnectedPin.Location));
                        g.DrawLine(myPen, pLoc.X, pLoc.Y, oLoc.X, oLoc.Y);
                    }
                }
            }
            if(firstSelectedPin != null) //Draw line if second pin not selected
            {
                myPen.Color = BasePin.GetPinColor(firstSelectedPin.pinType);
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
            if (firstSelectedPin != null || firstSelectedNode != null)
            {
                MainScriptingPanel.Refresh();
            }
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
                        }

                        public void Start(string args)
                        {
                        
                        ";
            allCode += currentNodes[0].CompileToString();
            allCode += @"
                        }
                    }
                }";

            VisualScriptCompiler visualCompiler = new VisualScriptCompiler(allCode);

            visualCompiler = null;
        }
    }
}
