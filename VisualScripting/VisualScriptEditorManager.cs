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

        public virtual void AddNewVariableButtonPressed()
        {

        }

        public virtual void AddNewFunctionButtonPressed()
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

                int existingClassManagerIndex = ClassNavigationPanelExists(button.visualClass);

                if (existingClassManagerIndex == -1) //new editor
                {
                    form.projectManager.AddNewShowingEditor(button.visualClass);
                    form.projectManager.ChangeSelectedEditorIndex(form.projectManager.showingEditors.Count - 1);
                }
                else //Editor already opened
                {
                    form.projectManager.ChangeSelectedEditorIndex(existingClassManagerIndex);
                }

                
            }
        }

        int ClassNavigationPanelExists(VisualClass _visualClass)
        {
            for (int i = 0; i < form.projectManager.showingEditors.Count; i++)
            {
                var classEditor = form.projectManager.showingEditors[i] as VisualClassScriptEditorManager;

                if (classEditor != null)
                {
                    VisualClassScriptEditorManager editor = (VisualClassScriptEditorManager)classEditor;
                    if (editor.visualClass == _visualClass)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }

    public class VisualClassScriptEditorManager : BaseEditorManager
    {
        CreateNodeSearchBar createNodeSearchBar;
        List<BaseNodePanel> currentNodesPanels;


        BasePin firstSelectedPin;
        BaseNodePanel firstSelectedNode;
        Size firstSelectedNodeOffset;
        VisualVariable firstSelectedVariable;
        VisualFunction firstSelectedFunction;

        public VisualClass visualClass;

        public List<Type> allNodesToShow = new List<Type>() { typeof(IfNode), typeof(PrintNode), typeof(MakeStringNode), typeof(MakeIntNode), typeof(MakeBooleanNode), typeof(ForLoopNode), typeof(ConvertIntToString) };

        public List<Type> allVariableTypesToShow = new List<Type>() {typeof(string), typeof(char), typeof(float), typeof(int) };

        public VisualClassScriptEditorManager(Form1 _form, VisualClass _visualClass) : base(_form)
        {
            visualClass = _visualClass;

            currentNodesPanels = new List<BaseNodePanel>();
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

            for (int i = 0; i < currentNodesPanels.Count; i++)
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
            var CheckFunction = _panel as VisualFunctionCreatePanelPart;

            if (CheckNode != null) //Node selected
            {
                VisualNodeCreatePanelPart node = (VisualNodeCreatePanelPart)_panel;
                VisualNode newNode = (VisualNode)Activator.CreateInstance(node.nodeType);
                newNodePanel = new BaseNodePanel(newNode);
                newNodePanel.visualNode = newNode;
                newNode.baseNodePanel = newNodePanel;
                visualClass.currentNodes.Add(newNode);
            }
            else if(CheckVariable != null) //variable selected
            {
                VisualVariableCreatePanelPart variablePanel = (VisualVariableCreatePanelPart)_panel;
                VisualVariableNodePanel node = new VisualVariableNodePanel(new VisualNode(), variablePanel.visualVariable);
                newNodePanel = new VisualVariableNodePanel(variablePanel.visualVariable, variablePanel.visualVariable);
                variablePanel.visualVariable.baseNodePanel = newNodePanel;
            }
            else if(CheckFunction != null)
            {
                VisualFunctionCreatePanelPart functionPanel = (VisualFunctionCreatePanelPart)_panel;
                VisualFunctionNodePanel node = new VisualFunctionNodePanel(new VisualNode(), functionPanel.visualFunction);
                newNodePanel = new VisualFunctionNodePanel(functionPanel.visualFunction, functionPanel.visualFunction);
                functionPanel.visualFunction.baseNodePanel = newNodePanel;
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

            if(firstSelectedPin != null)
            {
                if(firstSelectedPin.visualPin.pinRole == PinRole.Input) //Selected input
                {
                    foreach(VisualPin p in newNodePanel.visualNode.visualOutputs)
                    {
                        if(p.pinType == firstSelectedPin.visualPin.pinType)
                        {
                            p.otherConnectedPin = firstSelectedPin.visualPin;
                            firstSelectedPin.visualPin.otherConnectedPin = p;
                            firstSelectedPin = null;
                            break;
                        }
                    }
                }
                else //Selected output
                {
                    foreach (VisualPin p in newNodePanel.visualNode.visualInputs)
                    {
                        if (p.pinType == firstSelectedPin.visualPin.pinType)
                        {
                            p.otherConnectedPin = firstSelectedPin.visualPin;
                            firstSelectedPin.visualPin.otherConnectedPin = p;
                            firstSelectedPin = null;
                            break;
                        }
                    }
                }
            }
            form.MainScriptingPanel.Refresh();
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
                if (_pinPressed.visualPin.pinType == firstSelectedPin.visualPin.pinType && _pinPressed.visualPin.pinRole != firstSelectedPin.visualPin.pinRole)
                {
                    _pinPressed.visualPin.otherConnectedPin = firstSelectedPin.visualPin;
                    firstSelectedPin.visualPin.otherConnectedPin = _pinPressed.visualPin;
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

                if(firstSelectedPin == null) //Pin not selected
                {
                    createNodeSearchBar = new CreateNodeSearchBar(r.Location, allNodesToShow, visualClass.visualVariables, visualClass.visualFunctions);
                }
                else  //Pin selected
                {
                    List<Type> newNodesToShow = new List<Type>();
                    List<VisualVariable> newVariablesToShow = new List<VisualVariable>();
                    List<VisualFunction> newFunctionsToShow = new List<VisualFunction>();

                    if (firstSelectedPin.visualPin.pinRole == PinRole.Input) //Selected Input
                    {
                        foreach (Type t in allNodesToShow)
                        {
                            List<VisualNodeC> outputs =  (List<VisualNodeC>)t.GetField("outputs").GetValue(null);
                            foreach(VisualNodeC pin in outputs)
                            {
                                if(pin.type == firstSelectedPin.visualPin.pinType)
                                {
                                    newNodesToShow.Add(t);
                                    break;
                                }
                            }
                        }
                        foreach (VisualVariable v in visualClass.visualVariables)
                        {
                            if (v.variableType == firstSelectedPin.visualPin.pinType)
                            {
                                newVariablesToShow.Add(v);
                            }
                        }
                        foreach (VisualFunction v in visualClass.visualFunctions)
                        {
                            foreach (VisualPin pin in v.visualOutputs)
                            {
                                if (pin.pinType == firstSelectedPin.visualPin.pinType)
                                {
                                    newFunctionsToShow.Add(v);
                                    break;
                                }
                            }
                        }

                    }
                    else //Selected Output
                    {
                        foreach (Type t in allNodesToShow)
                        {
                            List<VisualNodeC> inputs = (List<VisualNodeC>)t.GetField("inputs").GetValue(null);
                            foreach (VisualNodeC pin in inputs)
                            {
                                if (pin.type == firstSelectedPin.visualPin.pinType)
                                {
                                    newNodesToShow.Add(t);
                                    break;
                                }
                            }
                        }
                        foreach (VisualFunction v in visualClass.visualFunctions)
                        {
                            foreach (VisualPin pin in v.visualInputs)
                            {
                                if (pin.pinType == firstSelectedPin.visualPin.pinType)
                                {
                                    newFunctionsToShow.Add(v);
                                    break;
                                }
                            }
                        }
                    }
                    createNodeSearchBar = new CreateNodeSearchBar(r.Location, newNodesToShow, newVariablesToShow, newFunctionsToShow);
                }
                form.MainScriptingPanel.Controls.Add(createNodeSearchBar);
                createNodeSearchBar.partPressed += SpawnNode;

                firstSelectedNode = null;
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
                        if (p.visualPin.otherConnectedPin != null)
                        {
                            myPen.Color = BasePin.GetPinColor(p.visualPin.pinType);
                            Point pLoc = form.MainScriptingPanel.PointToClient(n.PointToScreen(p.Location));
                            Console.Out.WriteLine(p.visualPin.otherConnectedPin.visualNode.baseNodePanel + " aaaaaaa");
                            Point oLoc = form.MainScriptingPanel.PointToClient(p.visualPin.otherConnectedPin.visualNode.baseNodePanel.PointToScreen(p.visualPin.otherConnectedPin.basePin.Location));
                            g.DrawLine(myPen, pLoc.X, pLoc.Y, oLoc.X, oLoc.Y);
                        }
                    }
                }
            }
            if (firstSelectedPin != null) //Draw line if second pin not selected
            {
                myPen.Color = BasePin.GetPinColor(firstSelectedPin.visualPin.pinType);
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
            for(int i = 0; i < visualClass.visualVariables.Count; i++)
            {
                if(visualClass.visualVariables[i].variableName == _name)
                {
                    return true;
                }
            }

            for(int i = 0; i < visualClass.visualFunctions.Count; i++)
            {
                if(visualClass.visualFunctions[i].functionName == _name)
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

        public override void AddNewVariableButtonPressed()
        {
            Random r = new Random();

            string newVariableName = "a" + r.Next(0, 10000).ToString();

            while (VariableOrFunctionNameExists(newVariableName))
            {
                newVariableName = "a" + r.Next(0, 10000).ToString();
            }

            visualClass.visualVariables.Add(new VisualVariable(typeof(string), newVariableName));
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

            visualClass.visualFunctions.Add(new VisualFunction(newFunctionName));
            UpdateVariableAndFunctionPanel();
        }
        #endregion

        public void UpdateVariableAndFunctionPanel()
        {
            ClearVariableFunctionInfoPanel();

            for (int i = 0; i < form.VariableAndFunctionPanel.Controls.Count; i++)
            {
                form.VariableAndFunctionPanel.Controls[0].Dispose();
            }

            Point lastPanelLocation = Point.Empty;

            for (int i = 0; i < visualClass.visualVariables.Count; i++)
            {
                VariablePanelPart panel = new VariablePanelPart(visualClass.visualVariables[i]);
                form.VariableAndFunctionPanel.Controls.Add(panel);
                panel.Location = new Point(0, i * 20);
                panel.panelPressed += variableAndFunctionpanelPartPressed;
                lastPanelLocation = panel.Location;
            }

            for (int i = 0; i < visualClass.visualFunctions.Count; i++)
            {
                FunctionPanelPart panel = new FunctionPanelPart(visualClass.visualFunctions[i]);
                form.VariableAndFunctionPanel.Controls.Add(panel);
                panel.Location = new Point(0, (i + visualClass.visualVariables.Count) * 20);
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


            if (CheckVariable != null) //variable pressed
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

                if (variable.variableValue == null)
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
                functionNameTextBox.Text = function.functionName;
                functionNameTextBox.LostFocus += ChangeFunctionNameTextChanged;

                ProjectManager.Instance.AddNewShowingEditor(function);
            }

            if (createNodeSearchBar != null)
            {
                createNodeSearchBar.Dispose();
            }
        }

        #region VisualFunctionStuff

        public override void AddNewFunctionButtonPressed()
        {
            VisualFunction newVisualFunction = new VisualFunction("NaujaFunkcija");
            visualClass.visualFunctions.Add(newVisualFunction);

            UpdateVariableAndFunctionPanel();
        }

        private void ChangeFunctionNameTextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            firstSelectedFunction.functionName = textBox.Text;

            UpdateVariableAndFunctionPanel();
        }

        #endregion
    }

    public class VisualFunctionScriptEditorManager : BaseEditorManager
    {
        CreateNodeSearchBar createNodeSearchBar;
        List<BaseNodePanel> currentNodesPanels;

        BasePin firstSelectedPin;
        BaseNodePanel firstSelectedNode;
        Size firstSelectedNodeOffset;
        VisualVariable firstSelectedVariable;
        VisualFunction firstSelectedFunction;

        public VisualFunction visualFunction;

        public List<Type> allNodesToShow = new List<Type>() { typeof(IfNode), typeof(PrintNode), typeof(MakeStringNode), typeof(MakeIntNode), typeof(MakeBooleanNode), typeof(ForLoopNode), typeof(ConvertIntToString) };

        public List<Type> allVariableTypesToShow = new List<Type>() { typeof(string), typeof(char), typeof(float), typeof(int) };

        public VisualFunctionScriptEditorManager(Form1 _form, VisualFunction _visualFunction) : base(_form)
        {
            visualFunction = _visualFunction;

            currentNodesPanels = new List<BaseNodePanel>();
            firstSelectedPin = null;
            firstSelectedNode = null;
            firstSelectedVariable = null;
            firstSelectedFunction = null;
            firstSelectedNodeOffset = new Size(0, 0);
            SpawnNode(new Point(50, 50), new VisualNodeCreatePanelPart(typeof(FunctionStartNode))); 
            SpawnNode(new Point(200, 200), new VisualNodeCreatePanelPart(typeof(FunctionEndNode))); //Spawns starting nodes

            UpdateVariableAndFunctionPanel();
        }

        public override void DisplayAllOnMainPanel()
        {
            form.MainScriptingPanel.BackColor = Color.FromArgb(225, 225, 225);

            for (int i = 0; i < currentNodesPanels.Count; i++)
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
            var CheckFunction = _panel as VisualFunctionCreatePanelPart;

            if (CheckNode != null) //Node selected
            {
                VisualNodeCreatePanelPart node = (VisualNodeCreatePanelPart)_panel;
                VisualNode newNode = (VisualNode)Activator.CreateInstance(node.nodeType);
                newNodePanel = new BaseNodePanel(newNode);
                newNodePanel.visualNode = newNode;
                newNode.baseNodePanel = newNodePanel;
                visualFunction.visualNodes.Add(newNode);
            }
            else if (CheckVariable != null) //variable selected
            {
                VisualVariableCreatePanelPart variablePanel = (VisualVariableCreatePanelPart)_panel;
                VisualVariableNodePanel node = new VisualVariableNodePanel(new VisualNode(), variablePanel.visualVariable);
                newNodePanel = new VisualVariableNodePanel(variablePanel.visualVariable, variablePanel.visualVariable);
                variablePanel.visualVariable.baseNodePanel = newNodePanel;
            }
            else if (CheckFunction != null)
            {
                VisualFunctionCreatePanelPart functionPanel = (VisualFunctionCreatePanelPart)_panel;
                VisualFunctionNodePanel node = new VisualFunctionNodePanel(new VisualNode(), functionPanel.visualFunction);
                newNodePanel = new VisualFunctionNodePanel(functionPanel.visualFunction, functionPanel.visualFunction);
                functionPanel.visualFunction.baseNodePanel = newNodePanel;
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

            if (firstSelectedPin != null)
            {
                if (firstSelectedPin.visualPin.pinRole == PinRole.Input) //Selected input
                {
                    foreach (VisualPin p in newNodePanel.visualNode.visualOutputs)
                    {
                        if (p.pinType == firstSelectedPin.visualPin.pinType)
                        {
                            p.otherConnectedPin = firstSelectedPin.visualPin;
                            firstSelectedPin.visualPin.otherConnectedPin = p;
                            firstSelectedPin = null;
                            break;
                        }
                    }
                }
                else //Selected output
                {
                    foreach (VisualPin p in newNodePanel.visualNode.visualInputs)
                    {
                        if (p.pinType == firstSelectedPin.visualPin.pinType)
                        {
                            p.otherConnectedPin = firstSelectedPin.visualPin;
                            firstSelectedPin.visualPin.otherConnectedPin = p;
                            firstSelectedPin = null;
                            break;
                        }
                    }
                }
            }
            form.MainScriptingPanel.Refresh();
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
                if (_pinPressed.visualPin.pinType == firstSelectedPin.visualPin.pinType && _pinPressed.visualPin.pinRole != firstSelectedPin.visualPin.pinRole)
                {
                    _pinPressed.visualPin.otherConnectedPin = firstSelectedPin.visualPin;
                    firstSelectedPin.visualPin.otherConnectedPin = _pinPressed.visualPin;
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

                if (firstSelectedPin == null) //Pin not selected
                {
                    createNodeSearchBar = new CreateNodeSearchBar(r.Location, allNodesToShow, visualFunction.visualVariables, new List<VisualFunction>());
                }
                else  //Pin selected
                {
                    List<Type> newNodesToShow = new List<Type>();
                    List<VisualVariable> newVariablesToShow = new List<VisualVariable>();
                    List<VisualFunction> newFunctionsToShow = new List<VisualFunction>();

                    if (firstSelectedPin.visualPin.pinRole == PinRole.Input) //Selected Input
                    {
                        foreach (Type t in allNodesToShow)
                        {
                            List<VisualNodeC> outputs = (List<VisualNodeC>)t.GetField("outputs").GetValue(null);
                            foreach (VisualNodeC pin in outputs)
                            {
                                if (pin.type == firstSelectedPin.visualPin.pinType)
                                {
                                    newNodesToShow.Add(t);
                                    break;
                                }
                            }
                        }
                        foreach (VisualVariable v in visualFunction.visualVariables)
                        {
                            if (v.variableType == firstSelectedPin.visualPin.pinType)
                            {
                                newVariablesToShow.Add(v);
                            }
                        }

                    }
                    else //Selected Output
                    {
                        foreach (Type t in allNodesToShow)
                        {
                            List<VisualNodeC> inputs = (List<VisualNodeC>)t.GetField("inputs").GetValue(null);
                            foreach (VisualNodeC pin in inputs)
                            {
                                if (pin.type == firstSelectedPin.visualPin.pinType)
                                {
                                    newNodesToShow.Add(t);
                                    break;
                                }
                            }
                        }
                    }
                    createNodeSearchBar = new CreateNodeSearchBar(r.Location, newNodesToShow, newVariablesToShow, newFunctionsToShow);
                }
                form.MainScriptingPanel.Controls.Add(createNodeSearchBar);
                createNodeSearchBar.partPressed += SpawnNode;

                firstSelectedNode = null;
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
                        if (p.visualPin.otherConnectedPin != null)
                        {
                            myPen.Color = BasePin.GetPinColor(p.visualPin.pinType);
                            Point pLoc = form.MainScriptingPanel.PointToClient(n.PointToScreen(p.Location));
                            Console.Out.WriteLine(p.visualPin.otherConnectedPin.visualNode.baseNodePanel + " aaaaaaa");
                            Point oLoc = form.MainScriptingPanel.PointToClient(p.visualPin.otherConnectedPin.visualNode.baseNodePanel.PointToScreen(p.visualPin.otherConnectedPin.basePin.Location));
                            g.DrawLine(myPen, pLoc.X, pLoc.Y, oLoc.X, oLoc.Y);
                        }
                    }
                }
            }
            if (firstSelectedPin != null) //Draw line if second pin not selected
            {
                myPen.Color = BasePin.GetPinColor(firstSelectedPin.visualPin.pinType);
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
            for (int i = 0; i < visualFunction.visualVariables.Count; i++)
            {
                if (visualFunction.visualVariables[i].variableName == _name)
                {
                    return true;
                }
            }
            return false;
        }

        void ClearVariableFunctionInfoPanel()
        {
            int amountOfChildren = form.VariableFunctionInfoPanel.Controls.Count;
            for (int i = 0; i < amountOfChildren; i++)
            {
                form.VariableFunctionInfoPanel.Controls[0].Dispose();
            }
        }

        public override void AddNewVariableButtonPressed()
        {
            Random r = new Random();

            string newVariableName = "a" + r.Next(0, 10000).ToString();

            while (VariableOrFunctionNameExists(newVariableName))
            {
                newVariableName = "a" + r.Next(0, 10000).ToString();
            }

            visualFunction.visualVariables.Add(new VisualVariable(typeof(string), newVariableName));
            UpdateVariableAndFunctionPanel();
        }
        #endregion

        public void UpdateVariableAndFunctionPanel()
        {
            ClearVariableFunctionInfoPanel();

            for (int i = 0; i < form.VariableAndFunctionPanel.Controls.Count; i++)
            {
                form.VariableAndFunctionPanel.Controls[0].Dispose();
            }

            Point lastPanelLocation = Point.Empty;

            for (int i = 0; i < visualFunction.visualVariables.Count; i++)
            {
                VariablePanelPart panel = new VariablePanelPart(visualFunction.visualVariables[i]);
                form.VariableAndFunctionPanel.Controls.Add(panel);
                panel.Location = new Point(0, i * 20);
                panel.panelPressed += variableAndFunctionpanelPartPressed;
                lastPanelLocation = panel.Location;
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

            ClearVariableFunctionInfoPanel();


            if (CheckVariable != null) //variable pressed
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

                if (variable.variableValue == null)
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

            if (createNodeSearchBar != null)
            {
                createNodeSearchBar.Dispose();
            }
        }
    }
}
