using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using System.Windows.Forms;

namespace VisualScripting
{
    public class BaseCreateNodePanelPart : Panel
    {
        public delegate void MyEventHandler(BaseCreateNodePanelPart panel);
        public event MyEventHandler panelPressed;

        protected Label nameLabel;

        public BaseCreateNodePanelPart()
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

        public virtual void BaseVariablePanelpartClicked(object sender, EventArgs e)
        {
            MyEventHandler handler = panelPressed;
            panelPressed(this);
        }
    }

    public class VisualNodeCreatePanelPart : BaseCreateNodePanelPart
    {
        public Type nodeType;

        public VisualNodeCreatePanelPart(Type _nodeType)
        {
            nodeType = _nodeType;

            nameLabel.Text = nodeType.GetField("nodeName").GetValue(null).ToString();
        }
    }

    public class VisualVariableCreatePanelPart : BaseCreateNodePanelPart
    {
        public VisualVariable visualVariable;

        public VisualVariableCreatePanelPart(VisualVariable _visualVariable)
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
