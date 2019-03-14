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
        public delegate void MyEventHandler(Point _position, Type _ofType = null, VisualVariable _ofVariable = null, VisualFunction _ofFunction = null);
        public event MyEventHandler partPressed;

        TextBox mainTextBox;

        Point panelLocation;

        Form1 thisForm;

        //List<Type> nodesToShow = new List<Type>() {typeof(IfNode), typeof(PrintNode), typeof(MakeString)};

        public CreateNodeSearchBar(Point _panelLocation, Form1 _thisForm)
        {
            panelLocation = _panelLocation;
            thisForm = _thisForm;

            this.BackColor = Color.DimGray;
            this.Location = _panelLocation;
            this.Size = new Size(200, 80);

            mainTextBox = new TextBox();
            this.Controls.Add(mainTextBox);
            mainTextBox.Size = new Size(200, 20);
            mainTextBox.Location = new Point(0, 0);

            int lastPositionY = 0;

            for (int i = 0; i < thisForm.allNodesToShow.Count; i++)
            {
                CreateNodePart newPart = new CreateNodePart(thisForm.allNodesToShow[i]);
                this.Controls.Add(newPart);
                newPart.Location = new Point(0, 20 + i * 10);
                newPart.panelPressed += PartPressed;
                lastPositionY = newPart.Location.Y;
            }


            for (int i = 0; i < thisForm.visualVariables.Count; i++)
            {
                VisualVariablePanelPart newPart = new VisualVariablePanelPart(thisForm.visualVariables[i]);
                this.Controls.Add(newPart);
                newPart.Location = new Point(0, i * 10 + lastPositionY + 10);
                newPart.panelPressed += VariablePressed;
                lastPositionY = i * 10 + lastPositionY;
            }

        }

        private void VariablePressed(VisualVariable thisVariable)
        {
            MyEventHandler handler = partPressed;
            handler(panelLocation, null, thisVariable);
        }

        void PartPressed(Type _ofType) //Part of search bar pressed
        {
            MyEventHandler handler = partPressed;
            handler(panelLocation, _ofType);
        }
    }
}
