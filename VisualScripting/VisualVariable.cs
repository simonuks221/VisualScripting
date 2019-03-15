using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace VisualScripting
{
    public class VisualVariable
    {
        public Type variableType;
        public string variableName;
        public Object variableValue = "labas tadas";

        public VisualVariable(Type _variableType, string _variableName)
        {
            variableType = _variableType;
            variableName = _variableName;
        }
    }

    public class VisualVariablePanelPart: Panel
    {
        public delegate void MyEventHandler(VisualVariable thisVariable);
        public event MyEventHandler panelPressed;

        public VisualVariable visualVariable;

        Label nameLabel;

        public VisualVariablePanelPart(VisualVariable _visualVariable)
        {
            if (_visualVariable != null)
            {
                visualVariable = _visualVariable;

                this.BackColor = Color.White;
                this.Size = new Size(200, 15);

                nameLabel = new Label();
                this.Controls.Add(nameLabel);
                nameLabel.Location = new Point(0, 0);
                nameLabel.Size = new Size(190, 20);
                nameLabel.Text = visualVariable.variableName;

                nameLabel.Click += VisualVariablePanelPart_Click;
                this.Click += VisualVariablePanelPart_Click;
            }
            else
            {
                throw new Exception("Null variable created");
            }
        }

        private void VisualVariablePanelPart_Click(object sender, EventArgs e)
        {
            MyEventHandler handler = panelPressed;
            panelPressed(visualVariable);
        }
    }
    public class VisualVariablePanel : Panel
    {
        public VisualVariable visualVariable;
        Label nameLabel;
        public BasePin outputPin;

        public VisualVariablePanel(VisualVariable _visualVariable)
        {
            if (_visualVariable != null)
            {
                visualVariable = _visualVariable;

                this.BackColor = Color.White;
                this.Size = new Size(200, 10);

                nameLabel = new Label();
                this.Controls.Add(nameLabel);
                nameLabel.Location = new Point(0, 0);
                nameLabel.Size = new Size(190, 20);
                nameLabel.Text = visualVariable.variableName;

                outputPin = new BasePin(visualVariable.variableType, PinRole.Output); //Add parent here
                this.Controls.Add(outputPin);
                outputPin.Location = new Point(190, 0);
                outputPin.pinValue = visualVariable.variableValue;
                Console.Out.WriteLine(outputPin.pinValue);
            }
            else
            {
                throw new Exception("Null variable created");
            }
        }
    }
}
