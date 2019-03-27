using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace VisualScripting
{
    class CreateNodeSearchBar : Panel
    {
        public delegate void MyEventHandler(Point _position, BaseCreateNodePanelPart _panel);
        public event MyEventHandler partPressed;

        TextBox mainTextBox;

        Point panelLocation;

        List<Type> nodesToShow;
        List<VisualVariable> variablesToShow;
        List<VisualFunction> functionsToShow;

        //List<Type> nodesToShow = new List<Type>() {typeof(IfNode), typeof(PrintNode), typeof(MakeString)};

        public CreateNodeSearchBar(Point _panelLocation, List<Type> _nodesToShow, List<VisualVariable> _variablesToShow, List<VisualFunction> _functionsToShow)
        {
            panelLocation = _panelLocation;
            nodesToShow = _nodesToShow;
            variablesToShow = _variablesToShow;
            functionsToShow = _functionsToShow;

            this.BackColor = Color.DimGray;
            this.Location = _panelLocation;

            mainTextBox = new TextBox();
            this.Controls.Add(mainTextBox);
            mainTextBox.Size = new Size(200, 20);
            mainTextBox.Location = new Point(0, 0);
            mainTextBox.TextChanged += MainTextBoxTextChanged;

            for (int i = 0; i < nodesToShow.Count; i++)
            {
                DisplayNodePanelPart(nodesToShow[i], i);
            }

            for(int i = 0; i < variablesToShow.Count; i++)
            {
                DisplayNodePanelPart(typeof(VisualVariable), i);
            }

            for (int i = 0; i < functionsToShow.Count; i++)
            {
                DisplayNodePanelPart(typeof(VisualFunction), i);
            }

            //mainTextBox.Visible = true;
            // mainTextBox.Enabled = true; //Not working focus on search bar
            //thisVisualScriptManager.form.ActiveControl = mainTextBox;
        }

        private void DisplayNodePanelPart(Type _type, int _typeIndex)
        {
            BaseCreateNodePanelPart newPart = null;
            if (_type == typeof(VisualVariable)) //variable
            {
                newPart = new VisualVariableCreatePanelPart(variablesToShow[_typeIndex]);
            }
            else if(_type == typeof(VisualFunction)) //Function
            {
                newPart = new VisualFunctionCreatePanelPart(functionsToShow[_typeIndex]);
            }
            else //Basic node
            {
                newPart = new VisualNodeCreatePanelPart(nodesToShow[_typeIndex]);
            }
            this.Controls.Add(newPart);
            newPart.Location = new Point(0, 20 + (this.Controls.Count - 2) * 15);
            newPart.panelPressed += PanelPressed;

            this.Size = new Size(200, (this.Controls.Count - 1) * 15 + 20); //Keep size dynamic
        }

        private void MainTextBoxTextChanged(object sender, EventArgs e)
        {
            int amountOfChildren = this.Controls.Count - 1; //Clear children, dont deletete the text box
            for (int i = 0; i < amountOfChildren; i++)
            {
                this.Controls[1].Dispose();
            }
            
            for(int i = 0; i < nodesToShow.Count; i++)
            {
                string typeName = nodesToShow[i].GetField("nodeName").GetValue(null).ToString().ToLower();
                if (typeName.Contains(mainTextBox.Text.ToLower()))
                {
                    DisplayNodePanelPart(nodesToShow[i], i);
                }
            }
            for (int i = 0; i < variablesToShow.Count; i++)
            {
                string typeName = variablesToShow[i].variableName.ToLower();
                if (typeName.Contains(mainTextBox.Text.ToLower()))
                {
                    DisplayNodePanelPart(typeof(VisualVariable), i);
                }
            }
        }

        private void PanelPressed(BaseCreateNodePanelPart _panel)
        {
            MyEventHandler handler = partPressed;
            handler(panelLocation, _panel);
        }
    }
}
