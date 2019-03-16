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

        VisualScriptManager thisVisualScriptManager;

        //List<Type> nodesToShow = new List<Type>() {typeof(IfNode), typeof(PrintNode), typeof(MakeString)};

        public CreateNodeSearchBar(Point _panelLocation, VisualScriptManager _thisVisualScriptManager)
        {
            panelLocation = _panelLocation;
            thisVisualScriptManager = _thisVisualScriptManager;

            this.BackColor = Color.DimGray;
            this.Location = _panelLocation;
            this.Size = new Size(200, (thisVisualScriptManager.allNodesToShow.Count + thisVisualScriptManager.visualVariables.Count) * 15 + 20);

            mainTextBox = new TextBox();
            this.Controls.Add(mainTextBox);
            mainTextBox.Size = new Size(200, 20);
            mainTextBox.Location = new Point(0, 0);

            int lastPositionY = 0;

            for (int i = 0; i < thisVisualScriptManager.allNodesToShow.Count; i++)
            {
                VisualNodeCreatePanelPart newPart = new VisualNodeCreatePanelPart(thisVisualScriptManager.allNodesToShow[i]);
                this.Controls.Add(newPart);
                newPart.Location = new Point(0, 20 + i * 15);
                newPart.panelPressed += PanelPressed;
                lastPositionY = newPart.Location.Y;
            }
            lastPositionY += 15;

            for (int i = 0; i < thisVisualScriptManager.visualVariables.Count; i++)
            {
                VisualVariableCreatePanelPart newPart = new VisualVariableCreatePanelPart(thisVisualScriptManager.visualVariables[i]);
                this.Controls.Add(newPart);
                newPart.Location = new Point(0, i * 15 + lastPositionY);
                newPart.panelPressed += PanelPressed;
                lastPositionY = newPart.Location.Y;
            }
        }

        private void PanelPressed(BaseCreateNodePanelPart _panel)
        {
            MyEventHandler handler = partPressed;
            handler(panelLocation, _panel);
        }
    }
}
