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
        public Button closeButton;

        public int navigationPanelPartIndex;

        public BaseNavigationPanelPart(int _navigationPanelPartIndex)
        {
            this.Size = new Size(100, 20);
            this.BackColor = Color.White;

            navigationPanelPartIndex = _navigationPanelPartIndex;

            nameLabel = new Label();
            this.Controls.Add(nameLabel);
            nameLabel.Size = new Size(70, 20);
            nameLabel.Location = new Point(0, 0);

            this.Click += ThisPanelPressed;
            nameLabel.Click += ThisPanelPressed;

            closeButton = new Button();
            this.Controls.Add(closeButton);
            closeButton.Location = new Point(78, 0);
            closeButton.Size = new Size(20, 20);
            closeButton.Text = "X";
        }

        private void ThisPanelPressed(object sender, EventArgs e)
        {
            navigationPanelPressed(this);
        }
    }

    public class VisualClassNavigationPanelPart : BaseNavigationPanelPart
    {
        public VisualClass visualClass;

        public VisualClassNavigationPanelPart(int index, VisualClass _visualClass) : base(index)
        {
            visualClass = _visualClass;
            
            nameLabel.Text = visualClass.className;
        }
    }

    public class AssetsManagerNavigationPanelPart : BaseNavigationPanelPart
    {
        public VisualFunction visualFunction;

        public AssetsManagerNavigationPanelPart(int index) : base(index)
        {
            nameLabel.Text = "Assets manager";
            closeButton.Hide();
        }
    }

    public class VisualFunctionNavigationPanelPart : BaseNavigationPanelPart
    {
        public VisualFunction visualFunction;

        public VisualFunctionNavigationPanelPart(int index, VisualFunction _visualFunction) : base(index)
        {
            visualFunction = _visualFunction;
            nameLabel.Text = visualFunction.functionName;
        }
    }
}
