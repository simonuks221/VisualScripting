using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace VisualScripting
{
    public class BaseNavigationPanelPart : Panel
    {
        public delegate void MyEventHandler(BaseNavigationPanelPart panelPressed);
        public event MyEventHandler navigationPanelPressed;

        protected Label nameLabel;

        public BaseNavigationPanelPart()
        {
            this.Size = new Size(100, 20);
            this.BackColor = Color.White;

            nameLabel = new Label();
            this.Controls.Add(nameLabel);
            nameLabel.Size = new Size(70, 20);
            nameLabel.Location = new Point(0, 0);

            this.Click += ThisPanelPressed;
            nameLabel.Click += ThisPanelPressed;
        }

        private void ThisPanelPressed(object sender, EventArgs e)
        {
            navigationPanelPressed(this);
        }
    }

    public class VisualClassNavigationPanelPart : BaseNavigationPanelPart
    {
        public VisualClass visualClass;

        public VisualClassNavigationPanelPart(VisualClass _visualClass)
        {
            visualClass = _visualClass;
            nameLabel.Text = visualClass.name;
        }
    }

    public class VisualFunctionnavigationPanelPart : BaseNavigationPanelPart
    {
        public VisualFunction visualFunction;

        public VisualFunctionnavigationPanelPart(VisualFunction _visualFunction)
        {
            visualFunction = _visualFunction;
            nameLabel.Text = visualFunction.name;
        }
    }
}
