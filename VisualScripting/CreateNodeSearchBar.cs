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

        VisualClassScriptEditorManager thisVisualScriptManager;

        List<Type> nodesToShow;

        //List<Type> nodesToShow = new List<Type>() {typeof(IfNode), typeof(PrintNode), typeof(MakeString)};

        public CreateNodeSearchBar(Point _panelLocation, VisualClassScriptEditorManager _thisVisualScriptManager, List<Type> _nodesToShow)
        {
            panelLocation = _panelLocation;
            thisVisualScriptManager = _thisVisualScriptManager;
            nodesToShow = _nodesToShow;

            this.BackColor = Color.DimGray;
            this.Location = _panelLocation;
            this.Size = new Size(200, nodesToShow.Count * 15 + 20);

            mainTextBox = new TextBox();
            this.Controls.Add(mainTextBox);
            mainTextBox.Size = new Size(200, 20);
            mainTextBox.Location = new Point(0, 0);
            mainTextBox.TextChanged += MainTextBoxTextChanged;

            for (int i = 0; i < nodesToShow.Count; i++)
            {
                DisplayNodePanelPart(i, this.Controls.Count - 1);
            }
        }

        private void DisplayNodePanelPart(int _typeIndex, int _listIndex)
        {
            BaseCreateNodePanelPart newPart = null;
            if (nodesToShow[_typeIndex] == typeof(VisualVariable)) //variable
            {
                newPart = new VisualVariableCreatePanelPart(thisVisualScriptManager.visualClass.visualVariables[_typeIndex]);
            }
            else //Not variable
            {
                newPart = new VisualNodeCreatePanelPart(nodesToShow[_typeIndex]);
            }
            this.Controls.Add(newPart);
            newPart.Location = new Point(0, 20 + _listIndex * 15);
            newPart.panelPressed += PanelPressed;
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
                    DisplayNodePanelPart(i, this.Controls.Count - 1);
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
