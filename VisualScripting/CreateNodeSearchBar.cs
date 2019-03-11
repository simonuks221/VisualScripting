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
        public delegate void MyEventHandler(Type _ofType, Point _position);
        public event MyEventHandler partPressed;

        TextBox mainTextBox;

        Point panelLocation;

        List<Type> nodesToShow = new List<Type>() {typeof(IfNode)};

        public CreateNodeSearchBar(Point _panelLocation)
        {
            panelLocation = _panelLocation;

            this.BackColor = Color.DimGray;
            this.Location = _panelLocation;
            this.Size = new Size(200, 40);

            mainTextBox = new TextBox();
            this.Controls.Add(mainTextBox);
            mainTextBox.Size = new Size(200, 20);
            mainTextBox.Location = new Point(0, 0);

            for (int i = 0; i < nodesToShow.Count; i++)
            {
                CreateNodePart newPart = new CreateNodePart(nodesToShow[i]);
                this.Controls.Add(newPart);
                newPart.Location = new Point(0, 20 + i * 10);
                newPart.panelPressed += PartPressed;
            }
        }

        void PartPressed(Type _ofType) //Part of search bar pressed
        {
            MyEventHandler handler = partPressed;
            handler(_ofType, panelLocation);
        }
    }
}
