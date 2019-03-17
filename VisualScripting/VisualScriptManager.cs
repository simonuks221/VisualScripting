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
        public List<VisualFunction> visualFunctions;

        BasePin firstSelectedPin;
        BaseNode firstSelectedNode;
        Size firstSelectedNodeOffset;
        VisualVariable firstSelectedVariable;
        VisualFunction firstSelectedFunction;

        Panel mainScriptingPanel;
        Panel variableAndFunctionPanel;
        Panel variableFunctionInfoPanel;

        public List<Type> allNodesToShow = new List<Type>() { typeof(IfNode), typeof(PrintNode), typeof(MakeStringNode), typeof(MakeIntNode), typeof(MakeBooleanNode), typeof(ForLoopNode) };

        public List<Type> allVariableTypesToShow = new List<Type>() {typeof(string), typeof(char), typeof(float), typeof(int) };

        public VisualScriptManager(Panel _mainScriptingPanel, Panel _variableAndFunctionPanel, Panel _variableFunctionInfoPanel)
        {
            mainScriptingPanel = _mainScriptingPanel;
            variableAndFunctionPanel = _variableAndFunctionPanel;
            variableFunctionInfoPanel = _variableFunctionInfoPanel;

            currentNodes = new List<BaseNode>();
            visualVariables = new List<VisualVariable>();
            visualFunctions = new List<VisualFunction>();
            firstSelectedPin = null;
            firstSelectedNode = null;
            firstSelectedVariable = null;
            firstSelectedFunction = null;
            firstSelectedNodeOffset = new Size(0, 0);
            SpawnNode(new Point(50, 50), new VisualNodeCreatePanelPart(typeof(ConstructNode))); //Spawns construct node

            UpdateVariableAndFunctionPanel();
        }

        public void SpawnNode(Point _position, BaseCreateNodePanelPart _panel) //Spawn node
        {
            BaseNode newNode = null;

            //Checking cast                             Needs better solution than this piece of crap, make an universal class that is arent of both:variable and node

            var CheckNode = _panel as VisualNodeCreatePanelPart;
            var CheckVariable = _panel as VisualVariableCreatePanelPart;

            if (CheckNode != null) //Node selected
            {
                VisualNodeCreatePanelPart node = (VisualNodeCreatePanelPart)_panel;
                newNode = (BaseNode)Activator.CreateInstance(node.nodeType);
            }
            else if(CheckVariable != null) //variable selected
            {
                VisualVariableCreatePanelPart variable = (VisualVariableCreatePanelPart)_panel;
                newNode = new VisualVariableNode(variable.visualVariable);
            }


            mainScriptingPanel.Controls.Add(newNode);
            newNode.Location = _position;

            currentNodes.Add(newNode);
            newNode.myMouseDown += StartMovingNode;
            newNode.myMouseUp += StopMovingNode;
            newNode.myMouseMove += MainScriptingPanel_MouseMove;

            newNode.pinPressed += PinPressed;

            if (createNodeSearchBar != null)
            {
                createNodeSearchBar.Dispose();
            }
        }

        public void StopMovingNode(BaseNode _senderNode, MouseEventArgs e) //Stop moving node
        {
            firstSelectedNode = null;
            mainScriptingPanel.Refresh();
        }

        public void StartMovingNode(BaseNode _senderNode, MouseEventArgs e) //Start moving node
        {
            firstSelectedNode = _senderNode;
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

            ClearVariableFunctionInfoPanel();
            firstSelectedVariable = null;
            firstSelectedFunction = null;

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
    {";
            for (int i = 0; i < visualVariables.Count; i++)
            {
                allCode += visualVariables[i].variableType + " " + visualVariables[i].variableName + " = ";

                if(visualVariables[i].variableType == typeof(string)) //Special cases for strings and chars
                {
                    allCode += "\"";
                }
                else if(visualVariables[i].variableType == typeof(char))
                {
                    allCode += "\'";
                }

                allCode += visualVariables[i].variableValue;

                if (visualVariables[i].variableType == typeof(string)) //Special cases for strings and chars
                {
                    allCode += "\"";
                }
                else if (visualVariables[i].variableType == typeof(char))
                {
                    allCode += "\'";
                }

                allCode += "; \n";
            }

         allCode += @"static void Main(string[] args)
         {
         }

         public void Start(string args)
         {";
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

        #region VisualVariableStuff

        public void UpdateVariableAndFunctionPanel()
        {
            ClearVariableFunctionInfoPanel();

            for (int i = 0; i < variableAndFunctionPanel.Controls.Count; i++)
            {
                variableAndFunctionPanel.Controls[i].Dispose();
            }

            for(int i = 0; i < visualVariables.Count; i++)
            {
                VariablePanelPart panel = new VariablePanelPart(visualVariables[i]);
                variableAndFunctionPanel.Controls.Add(panel);
                panel.Location = new Point(0, i * 20);
                panel.panelPressed += variableAndFunctionpanelPartPressed;
            }
            Point lastPanelLocation = Point.Empty;
            if(visualVariables.Count > 0)
            {
                lastPanelLocation = variableAndFunctionPanel.Controls[visualVariables.Count].Location;
            }
            
            for (int i = 0; i < visualFunctions.Count; i++)
            {
                FunctionPanelPart panel = new FunctionPanelPart(visualFunctions[i]);
                variableAndFunctionPanel.Controls.Add(panel);
                panel.Location = new Point(0, i * 20 + lastPanelLocation.Y);
                panel.panelPressed += variableAndFunctionpanelPartPressed;
            }


            if (firstSelectedVariable != null) //variable is selected, update panels
            {
                variableAndFunctionpanelPartPressed(new VariablePanelPart(firstSelectedVariable));
            }
            else if (firstSelectedFunction != null)
            {
                variableAndFunctionpanelPartPressed(new FunctionPanelPart(firstSelectedFunction));
            }
        }

        private void variableAndFunctionpanelPartPressed(BaseVariableAndFunctionPanelPart _panelPressed)
        {
            var CheckVariable = _panelPressed as VariablePanelPart;
            var CheckFunction = _panelPressed as FunctionPanelPart;

            ClearVariableFunctionInfoPanel();
            

            if(CheckVariable != null) //variable pressed
            {
                VisualVariable variable = CheckVariable.visualVariable;
                firstSelectedVariable = variable;

                TextBox variableNameTextBox = new TextBox();
                variableFunctionInfoPanel.Controls.Add(variableNameTextBox);
                variableNameTextBox.Location = new Point(5, 5);
                variableNameTextBox.Size = new Size(90, 13);
                variableNameTextBox.Text = variable.variableName;
                variableNameTextBox.LostFocus += ChangeVariableNameTextChanged;

                TextBox variableValueTextBox = new TextBox();
                variableFunctionInfoPanel.Controls.Add(variableValueTextBox);
                variableValueTextBox.Location = new Point(5, 30);
                variableValueTextBox.Size = new Size(90, 13);

                if(variable.variableValue == null)
                {
                    variableValueTextBox.Text = "";
                }
                else
                {
                    variableValueTextBox.Text = variable.variableValue.ToString();
                }

                variableValueTextBox.LostFocus += ChangeVariablevalueTextChanged;

                ComboBox variableTypeComboBox = new ComboBox();
                variableFunctionInfoPanel.Controls.Add(variableTypeComboBox);
                variableTypeComboBox.Location = new Point(5, 48);
                variableTypeComboBox.Size = new Size(90, 13);
                variableTypeComboBox.Items.AddRange(allVariableTypesToShow.ToArray());
                variableTypeComboBox.SelectedIndex = allVariableTypesToShow.IndexOf(firstSelectedVariable.variableType);
                
                variableTypeComboBox.SelectedValueChanged += VariableTypeComboBoxSelectedValueChanged;
            }

            if (CheckFunction != null)
            {
                VisualFunction function = CheckFunction.visualFunction;
                firstSelectedFunction = function;

                TextBox functionNameTextBox = new TextBox();
                variableFunctionInfoPanel.Controls.Add(functionNameTextBox);
                functionNameTextBox.Location = new Point(5, 5);
                functionNameTextBox.Size = new Size(90, 13);
                functionNameTextBox.Text = function.name;
                functionNameTextBox.LostFocus += ChangeFunctionNameTextChanged;
            }

            if(createNodeSearchBar != null)
            {
                createNodeSearchBar.Dispose();
            }
        }

        private void ChangeFunctionNameTextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            firstSelectedFunction.name = textBox.Text;

            UpdateVariableAndFunctionPanel();
        }

        private void VariableTypeComboBoxSelectedValueChanged(object sender, EventArgs e) //Type changed
        {
            ComboBox box = (ComboBox)sender;
            firstSelectedVariable.variableType = allVariableTypesToShow[box.SelectedIndex];
            firstSelectedVariable.variableValue = null;

            UpdateVariableAndFunctionPanel();
        }

        private void ChangeVariableNameTextChanged(object sender, EventArgs e) //Name changed
        {
            TextBox textBox = (TextBox)sender;
            firstSelectedVariable.variableName = textBox.Text;

            UpdateVariableAndFunctionPanel();
        }

        private void ChangeVariablevalueTextChanged(object sender, EventArgs e) //Value changed
        {
            TextBox textBox = (TextBox)sender;
            firstSelectedVariable.variableValue = textBox.Text;
            UpdateVariableAndFunctionPanel();
        }

        bool VariableOrFunctionNameExists(string _name)
        {
            for(int i = 0; i < visualVariables.Count; i++)
            {
                if(visualVariables[i].variableName == _name)
                {
                    return true;
                }
            }

            for(int i = 0; i < visualFunctions.Count; i++)
            {
                if(visualFunctions[i].name == _name)
                {
                    return true;
                }
            }
            return false;
        }

        void ClearVariableFunctionInfoPanel()
        {
            int amountOfChildren = variableFunctionInfoPanel.Controls.Count;
            for(int i = 0; i < amountOfChildren; i++)
            {
                variableFunctionInfoPanel.Controls[0].Dispose();
            }
        }

        public void AddNewVisualVariable()
        {
            Random r = new Random();

            string newVariableName = "a" + r.Next(0, 10000).ToString();

            while (VariableOrFunctionNameExists(newVariableName))
            {
                newVariableName = "a" + r.Next(0, 10000).ToString();
            }

            visualVariables.Add(new VisualVariable(typeof(string), newVariableName));
            UpdateVariableAndFunctionPanel();
        }

        public void AddNewVisualFunction()
        {
            Random r = new Random();

            string newFunctionName = "a" + r.Next(0, 10000).ToString();

            while (VariableOrFunctionNameExists(newFunctionName))
            {
                newFunctionName = "a" + r.Next(0, 10000).ToString();
            }

            visualFunctions.Add(new VisualFunction(newFunctionName));
            UpdateVariableAndFunctionPanel();
        }
        #endregion


    }
}
