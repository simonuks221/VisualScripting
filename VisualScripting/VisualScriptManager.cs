using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace VisualScripting
{
    class VisualScriptManager
    {
        CreateNodeSearchBar createNodeSearchBar;
        List<BaseNode> currentNodes;
        public List<VisualVariable> visualVariables;

        BasePin firstSelectedPin;
        BaseNode firstSelectedNode;
        Size firstSelectedNodeOffset;

        Panel mainScriptingPanel;

        public List<Type> allNodesToShow = new List<Type>() { typeof(IfNode), typeof(PrintNode), typeof(MakeStringNode), typeof(MakeIntNode), typeof(MakeBooleanNode), typeof(ForLoopNode) };

        public VisualScriptManager(Panel _mainScriptingPanel)
        {
            mainScriptingPanel = _mainScriptingPanel;
            currentNodes = new List<BaseNode>();
            visualVariables = new List<VisualVariable>() { new VisualVariable(typeof(string), "Naujas") };
            firstSelectedPin = null;
            firstSelectedNode = null;
            firstSelectedNodeOffset = new Size(0, 0);
            SpawnNode(new Point(50, 50), typeof(ConstructNode));
        }

        public void SpawnNode(Point _position, Type _nodeType = null, VisualVariable _visualVariable = null, VisualFunction _visualFunction = null) //Spawn node
        {
            if (true)//(_nodeType != null)
            {
                BaseNode newNode;
                if (_visualVariable != null) //Spawn variable
                {
                    newNode = new VisualVariableNode(_visualVariable);
                }
                else
                {
                    newNode = (BaseNode)Activator.CreateInstance(_nodeType);
                }
                mainScriptingPanel.Controls.Add(newNode);
                newNode.Location = _position;

                currentNodes.Add(newNode);

                newNode.MouseDown += StartMovingNode;
                newNode.MouseUp += StopMovingNode;
                newNode.MouseMove += MainScriptingPanel_MouseMove;

                for (int i = 0; i < newNode.inputPins.Count; i++)
                {
                    newNode.inputPins[i].pinPressed += PinPressed;
                }
                for (int i = 0; i < newNode.outputPins.Count; i++)
                {
                    newNode.outputPins[i].pinPressed += PinPressed;
                }
            }

            if (createNodeSearchBar != null)
            {
                createNodeSearchBar.Dispose();
            }
        }

        public void StopMovingNode(object sender, MouseEventArgs e) //Stop moving node
        {
            firstSelectedNode = null;
            mainScriptingPanel.Refresh();
        }

        public void StartMovingNode(object sender, MouseEventArgs e) //Start moving node
        {
            firstSelectedNode = (BaseNode)sender;
            firstSelectedNodeOffset = new Size(e.Location.X * -1, e.Location.Y * -1);
            mainScriptingPanel.Refresh();
        }

        public void PinPressed(BasePin _pinPressed)
        {
            if (firstSelectedPin == null)
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
                    mainScriptingPanel.Refresh();
                }
            }
        }

        public void MainScriptingPanelMouseClick(object sender, EventArgs e)
        {
            MouseEventArgs r = (MouseEventArgs)e;
            if (r.Button == MouseButtons.Right)
            {
                if (createNodeSearchBar != null)
                {
                    createNodeSearchBar.Dispose();
                }
                createNodeSearchBar = new CreateNodeSearchBar(r.Location, this);
                mainScriptingPanel.Controls.Add(createNodeSearchBar);
                createNodeSearchBar.partPressed += SpawnNode;

                firstSelectedNode = null;
                firstSelectedPin = null;
            }
            else
            {
                firstSelectedNode = null;
                firstSelectedPin = null;

                if (createNodeSearchBar != null)
                {
                    createNodeSearchBar.Dispose();
                }
                createNodeSearchBar = null;
            }
            mainScriptingPanel.Refresh();
        }

        public void MainScriptingPanel_Paint(object sender, PaintEventArgs e) //Paint connections between pins
        {
            Graphics g;
            g = mainScriptingPanel.CreateGraphics();
            Pen myPen = new Pen(BasePin.GetPinColor(typeof(ExecutionPin)));
            myPen.Width = 2;
            foreach (BaseNode n in currentNodes) //Painting lines
            {
                foreach (BasePin p in n.inputPins) //Paint from input
                {
                    if (p.otherConnectedPin != null)
                    {
                        myPen.Color = BasePin.GetPinColor(p.pinType);
                        Point pLoc = mainScriptingPanel.PointToClient(n.PointToScreen(p.Location));
                        Point oLoc = mainScriptingPanel.PointToClient(p.otherConnectedPin.Parent.PointToScreen(p.otherConnectedPin.Location));
                        g.DrawLine(myPen, pLoc.X, pLoc.Y, oLoc.X, oLoc.Y);
                    }
                }
            }
            if (firstSelectedPin != null) //Draw line if second pin not selected
            {
                myPen.Color = BasePin.GetPinColor(firstSelectedPin.pinType);
                Point pLoc = mainScriptingPanel.PointToClient(firstSelectedPin.Parent.PointToScreen(firstSelectedPin.Location));
                Point mLoc = mainScriptingPanel.PointToClient(Control.MousePosition);
                g.DrawLine(myPen, pLoc.X, pLoc.Y, mLoc.X, mLoc.Y);
            }

            if (firstSelectedNode != null) //Moving node
            {
                firstSelectedNode.Location = mainScriptingPanel.PointToClient(Control.MousePosition) + firstSelectedNodeOffset;
            }

            g.Dispose();
            myPen.Dispose();
        }

        public void MainScriptingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (firstSelectedPin != null || firstSelectedNode != null)
            {
                mainScriptingPanel.Refresh();
            }
        }

        public void CompileAllToString()
        {
            string allCode = @"
                using System; 
                using System.Collections.Generic; 
                using System.Linq;
                using System.Text;
                using System.Threading.Tasks;
                using System.Windows.Forms;
                using VisualScripting;

                namespace VisualScripting
                {
                    class Program
                    {

ConsoleForm n = new ConsoleForm();
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

            if (ConsoleForm.Instance == null)
            {
                ConsoleForm c = new ConsoleForm();
                c.Show();
            }

            VisualScriptCompiler visualCompiler = new VisualScriptCompiler(allCode);
            visualCompiler = null;
        }
    }
}
