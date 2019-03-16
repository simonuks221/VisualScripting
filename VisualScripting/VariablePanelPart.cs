using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using System.Windows.Forms;

namespace VisualScripting
{
    public class BaseVariablePanelPart : Panel
    {
        public delegate void MyEventHandler(VisualVariable thisVariable);
        public event MyEventHandler panelPressed;

        protected Label nameLabel;

        public BaseVariablePanelPart()
        {
            this.BackColor = Color.White;
            this.Size = new Size(200, 15);

            nameLabel = new Label();
            this.Controls.Add(nameLabel);
            nameLabel.Location = new Point(0, 0);
            nameLabel.Size = new Size(190, 20);

            nameLabel.Click += BaseVariablePanelpartClicked;
            this.Click += BaseVariablePanelpartClicked;
        }

        private void BaseVariablePanelpartClicked(object sender, EventArgs e)
        {
            MyEventHandler handler = panelPressed;
            panelPressed(visualVariable);
        }
    }

    public class VisualVariablePanelPart : BaseVariablePanelPart
    {
        public VisualVariable visualVariable;

        public VisualVariablePanelPart(VisualVariable _visualVariable)
        {
            if (_visualVariable != null)
            {
                visualVariable = _visualVariable;

                nameLabel.Text = visualVariable.variableName;
            }
            else
            {
                throw new Exception("Null variable created");
            }
        }
    }
}
