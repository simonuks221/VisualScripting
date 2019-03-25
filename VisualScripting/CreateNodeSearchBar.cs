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

        //List<Type> nodesToShow = new List<Type>() {typeof(IfNode), typeof(PrintNode), typeof(MakeString)};

        public CreateNodeSearchBar(Point _panelLocation, VisualClassScriptEditorManager _thisVisualScriptManager, List<Type> nodesToShow)
        {
            panelLocation = _panelLocation;
            thisVisualScriptManager = _thisVisualScriptManager;

            this.BackColor = Color.DimGray;
            this.Location = _panelLocation;
            this.Size = new Size(200, nodesToShow.Count * 15 + 20);

            mainTextBox = new TextBox();
            this.Controls.Add(mainTextBox);
            mainTextBox.Size = new Size(200, 20);
            mainTextBox.Location = new Point(0, 0);

            for (int i = 0; i < nodesToShow.Count; i++)
            {
                BaseCreateNodePanelPart newPart = null;
                if(nodesToShow[i] == typeof(VisualVariable)) //variable
                {
                    newPart = new VisualVariableCreatePanelPart(thisVisualScriptManager.visualClass.visualVariables[i]);
                }
                else //Not variable
                {
                    newPart = new VisualNodeCreatePanelPart(nodesToShow[i]);
                }
                this.Controls.Add(newPart);
                newPart.Location = new Point(0, 20 + i * 15);
                newPart.panelPressed += PanelPressed;
            }

        }

        private void PanelPressed(BaseCreateNodePanelPart _panel)
        {
            MyEventHandler handler = partPressed;
            handler(panelLocation, _panel);
        }
    }
}
