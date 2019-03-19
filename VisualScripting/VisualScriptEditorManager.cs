using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace VisualScripting
{
    public class BaseEditorManager
    {
        public Form1 form;

        public BaseEditorManager(Form1 _form)
        {
            form = _form;
        }

        public virtual void MainScriptingPanel_MouseMove(object sender, MouseEventArgs e)
        {

        }

        public virtual void MainScriptingPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        public virtual void MainScriptingPanelMouseClick(object sender, EventArgs e)
        {

        }

        public virtual void DisplayAllOnMainPanel()
        {

        }
    }

    public class AssetsEditorManager : BaseEditorManager
    {
        public AssetsEditorManager(Form1 _form) : base(_form)
        {
            
        }

        public override void DisplayAllOnMainPanel()
        {
            form.MainScriptingPanel.BackColor = Color.Silver;

            VisualProject visualProject = form.projectManager.visualProject;

            int amountOfPanelsInRow = (int)Math.Floor((double)form.MainScriptingPanel.Size.Width / (double)100);
            int amountOfRows = (int)Math.Ceiling((double)visualProject.visualClasses.Count / (double)amountOfPanelsInRow);

            for (int y = 0; y < amountOfRows; y++)
            {
                for (int x = 0; x < amountOfPanelsInRow; x++)
                {
                    if (!(visualProject.visualClasses.Count <= y * amountOfPanelsInRow + x))
                    {
                        ClassAssetButton newPanel = new ClassAssetButton(visualProject.visualClasses[y * amountOfPanelsInRow + x]);
                        form.MainScriptingPanel.Controls.Add(newPanel);
                        newPanel.Location = new Point(x * 100, y * 100);
                        newPanel.assetPressed += AssetPressed;
                    }
                }
            }
        }

        private void AssetPressed(BaseAssetButton _asset)
        {
            var CheckVisualClass = _asset as ClassAssetButton;

            if (CheckVisualClass != null) //Class selected
            {
                ClassAssetButton button = (ClassAssetButton)CheckVisualClass;
                form.projectManager.AddNewShowingEditor(button.visualClass);

                form.projectManager.ChangeSelectedEditorIndex(form.projectManager.showingEditors.Count - 1);
            }
        }
    }

    public class VisualScriptEditorManager : BaseEditorManager
    {
        CreateNodeSearchBar createNodeSearchBar;
        List<BaseNodePanel> currentNodesPanels;

        public List<VisualNode> currentNodes;
        public List<VisualVariable> visualVariables;
        public List<VisualFunction> visualFunctions;

        BasePin firstSelectedPin;
        BaseNodePanel firstSelectedNode;
        Size firstSelectedNodeOffset;
        VisualVariable firstSelectedVariable;
        VisualFunction firstSelectedFunction;

        public VisualBase visualBase;

        public List<Type> allNodesToShow = new List<Type>() { typeof(IfNode), typeof(PrintNode), typeof(MakeStringNode), typeof(MakeIntNode), typeof(MakeBooleanNode), typeof(ForLoopNode) };

        public List<Type> allVariableTypesToShow = new List<Type>() {typeof(string), typeof(char), typeof(float), typeof(int) };

        public VisualScriptEditorManager(Form1 _form, VisualBase _visualBase) : base(_form)
        {
            visualBase = _visualBase;

            currentNodesPanels = new List<BaseNodePanel>();
            currentNodes = new List<VisualNode>();
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

        public override void DisplayAllOnMainPanel()
        {
            form.MainScriptingPanel.BackColor = Color.FromArgb(225, 225, 225);

            for (int i = 0; i < currentNodes.Count; i++)
            {
                currentNodesPanels[i].Visible = true;
            }
            form.MainScriptingPanel.Refresh();
        }

        public void SpawnNode(Point _position, BaseCreateNodePanelPart _panel) //Spawn node
        {
            BaseNodePanel newNodePanel = null;

            var CheckNode = _panel as VisualNodeCreatePanelPart;
            var CheckVariable = _panel as VisualVariableCreatePanelPart;

            if (CheckNode != null) //Node selected
            {
                VisualNodeCreatePanelPart node = (VisualNodeCreatePanelPart)_panel;
                VisualNode newNode = (VisualNode)Activator.CreateInstance(node.nodeType);
                newNodePanel = new BaseNodePanel(newNode);
                newNodePanel.visualNode = newNode;
                newNode.baseNodePanel = newNodePanel;
                currentNodes.Add(newNode);
            }
            else if(CheckVariable != null) //variable selected
            {
                /*
                VisualVariableCreatePanelPart variable = (VisualVariableCreatePanelPart)_panel;
                VisualVariableNode node = new VisualVariableNode(new VisualNode(), variable.visualVariable);
                newNodePanel = new VisualVariableNode(node, variable.visualVariable);
                */
            }

            form.MainScriptingPanel.Controls.Add(newNodePanel);
            newNodePanel.Location = _position;

            currentNodesPanels.Add(newNodePanel);
            newNodePanel.myMouseDown += StartMovingNode;
            newNodePanel.myMouseUp += StopMovingNode;
            newNodePanel.myMouseMove += MainScriptingPanel_MouseMove;

            newNodePanel.pinPressed += PinPressed;

            if (createNodeSearchBar != null)
            {
                createNodeSearchBar.Dispose();
            }
        }

        public void StopMovingNode(BaseNodePanel _senderNode, MouseEventArgs e) //Stop moving node
        {
            firstSelectedNode = null;
            form.MainScriptingPanel.Refresh();
        }

        public void StartMovingNode(BaseNodePanel _senderNode, MouseEventArgs e) //Start moving node
        {
            firstSelectedNode = _senderNode;
            firstSelectedNodeOffset = new Size(e.Location.X * -1, e.Location.Y * -1);
            form.MainScriptingPanel.Refresh();
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
                    form.MainScriptingPanel.Refresh();
                }
            }
        }

        public override void MainScriptingPanelMouseClick(object sender, EventArgs e)
        {
            MouseEventArgs r = (MouseEventArgs)e;
            if (r.Button == MouseButtons.Right)
            {
                if (createNodeSearchBar != null)
                {
                    createNodeSearchBar.Dispose();
                }
                createNodeSearchBar = new CreateNodeSearchBar(r.Location, this);
                form.MainScriptingPanel.Controls.Add(createNodeSearchBar);
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

            form.MainScriptingPanel.Refresh();
        }

        public override void MainScriptingPanel_Paint(object sender, PaintEventArgs e) //Paint connections between pins
        {
            Graphics g;
            g = form.MainScriptingPanel.CreateGraphics();
            Pen myPen = new Pen(BasePin.GetPinColor(typeof(ExecutionPin)));
            myPen.Width = 2;
            foreach (BaseNodePanel n in currentNodesPanels) //Painting lines
            {
                if (n.inputPins != null)
                {
                    foreach (BasePin p in n.inputPins) //Paint from input
                    {
                        if (p.otherConnectedPin != null)
                        {
                            myPen.Color = BasePin.GetPinColor(p.pinType);
                            Point pLoc = form.MainScriptingPanel.PointToClient(n.PointToScreen(p.Location));
                            Point oLoc = form.MainScriptingPanel.PointToClient(p.otherConnectedPin.Parent.PointToScreen(p.otherConnectedPin.Location));
                            g.DrawLine(myPen, pLoc.X, pLoc.Y, oLoc.X, oLoc.Y);
                        }
                    }
                }
            }
            if (firstSelectedPin != null) //Draw line if second pin not selected
            {
                myPen.Color = BasePin.GetPinColor(firstSelectedPin.pinType);
                Point pLoc = form.MainScriptingPanel.PointToClient(firstSelectedPin.Parent.PointToScreen(firstSelectedPin.Location));
                Point mLoc = form.MainScriptingPanel.PointToClient(Control.MousePosition);
                g.DrawLine(myPen, pLoc.X, pLoc.Y, mLoc.X, mLoc.Y);
            }

            if (firstSelectedNode != null) //Moving node
            {
                firstSelectedNode.Location = form.MainScriptingPanel.PointToClient(Control.MousePosition) + firstSelectedNodeOffset;
            }

            g.Dispose();
            myPen.Dispose();
        }

        public override void MainScriptingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (firstSelectedPin != null || firstSelectedNode != null)
            {
                form.MainScriptingPanel.Refresh();
            }
        }

        #region VisualVariableStuff

        public void UpdateVariableAndFunctionPanel()
        {
            ClearVariableFunctionInfoPanel();

            for (int i = 0; i < form.VariableAndFunctionPanel.Controls.Count; i++)
            {
                form.VariableAndFunctionPanel.Controls[i].Dispose();
            }

            for(int i = 0; i < visualVariables.Count; i++)
            {
                VariablePanelPart panel = new VariablePanelPart(visualVariables[i]);
                form.VariableAndFunctionPanel.Controls.Add(panel);
                panel.Location = new Point(0, i * 20);
                panel.panelPressed += variableAndFunctionpanelPartPressed;
            }
            Point lastPanelLocation = Point.Empty;
            if(visualVariables.Count > 0)
            {
                lastPanelLocation = form.VariableAndFunctionPanel.Controls[visualVariables.Count].Location;
            }
            
            for (int i = 0; i < visualFunctions.Count; i++)
            {
                FunctionPanelPart panel = new FunctionPanelPart(visualFunctions[i]);
                form.VariableAndFunctionPanel.Controls.Add(panel);
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
                form.VariableFunctionInfoPanel.Controls.Add(variableNameTextBox);
                variableNameTextBox.Location = new Point(5, 5);
                variableNameTextBox.Size = new Size(90, 13);
                variableNameTextBox.Text = variable.variableName;
                variableNameTextBox.LostFocus += ChangeVariableNameTextChanged;

                TextBox variableValueTextBox = new TextBox();
                form.VariableFunctionInfoPanel.Controls.Add(variableValueTextBox);
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
                form.VariableFunctionInfoPanel.Controls.Add(variableTypeComboBox);
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
                form.VariableFunctionInfoPanel.Controls.Add(functionNameTextBox);
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
            int amountOfChildren = form.VariableFunctionInfoPanel.Controls.Count;
            for(int i = 0; i < amountOfChildren; i++)
            {
                form.VariableFunctionInfoPanel.Controls[0].Dispose();
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
