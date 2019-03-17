using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using System.Windows.Forms;

namespace VisualScripting
{
    class BaseVariableAndFunctionPanelPart : Panel
    {
        public delegate void MyEventHandler(BaseVariableAndFunctionPanelPart _panelPressed);
        public event MyEventHandler panelPressed;

        protected Label nameLabel;
        public BaseVariableAndFunctionPanelPart()
        {
            this.BackColor = Color.White;
            this.Size = new Size(100, 20);

            nameLabel = new Label();
            this.Controls.Add(nameLabel);
            nameLabel.Size = new Size(100, 20);
            nameLabel.Location = new Point(0, 0);
            nameLabel.Text = "Something";

            this.Click += PanelPressed;
            nameLabel.Click += PanelPressed;
        }

        private void PanelPressed(object sender, EventArgs e)
        {
            panelPressed(this);
        }
    }

    class VariablePanelPart : BaseVariableAndFunctionPanelPart
    {
        public VisualVariable visualVariable;
        public VariablePanelPart(VisualVariable _visualVariable)
        {
            visualVariable = _visualVariable;
            nameLabel.Text = visualVariable.variableName;
        }
    }

    class FunctionPanelPart : BaseVariableAndFunctionPanelPart
    {
        public VisualFunction visualFunction;
        public FunctionPanelPart(VisualFunction _visualFunction)
        {
            visualFunction = _visualFunction;
            nameLabel.Text = visualFunction.name;
        }
    }
}
