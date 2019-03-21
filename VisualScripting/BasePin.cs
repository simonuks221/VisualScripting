using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace VisualScripting
{
    public enum PinRole { Input, Output}

    public class VisualPin : VisualBase
    {
        public PinRole pinRole;
        public VisualPin otherConnectedPin;
        public Type pinType;
        public VisualNode visualNode;

        public bool pinIsVariable = false;
        public BasePin basePin;

        public Object pinValue;
        public VisualVariable pinVariable;
        public string pinName;

        public VisualPin(PinRole _pinRole, Type _type, bool _isVariable, string _pinName)
        {
            pinRole = _pinRole;
            pinType = _type;
            pinIsVariable = _isVariable;
            pinName = _pinName;
        }
    }

    public class BasePin : Panel
    {
        public delegate void MyEventHandler(BasePin pinPressed);
        public event MyEventHandler pinPressed;
        
        public BaseNodePanel parentNode;

        public VisualPin visualPin;

        public Panel actualPinPanel;
        public Label pinText;

        public BasePin(VisualPin _visualPin, BaseNodePanel _parentNode = null) //Fix this _parentNode stuff
        {
            visualPin = _visualPin;
            parentNode = _parentNode;

            this.Size = new Size(23, 10);

            actualPinPanel = new Panel();
            this.Controls.Add(actualPinPanel);
            actualPinPanel.Size = new Size(10, 10);

            switch (visualPin.pinRole)
            {
                case PinRole.Input:
                    actualPinPanel.Location = new Point(0, 0);
                    break;
                case PinRole.Output:
                    actualPinPanel.Location = new Point(this.Size.Width - 10, 0);
                    break;
            }
            actualPinPanel.BackColor = GetPinColor(visualPin.pinType);
            actualPinPanel.Click += BasePinClick;

            pinText = new Label();
            this.Controls.Add(pinText);
            pinText.Size = new Size(13, 10);

            switch (visualPin.pinRole)
            {
                case PinRole.Input:
                    pinText.Location = new Point(this.Size.Width - 13, 0);
                    break;
                case PinRole.Output:
                    pinText.Location = new Point(0, 0);
                    break;
            }
            pinText.Font = new Font("Arial", 4, FontStyle.Bold);
            pinText.Text = visualPin.pinName;
        }

        private void BasePinClick(object sender, EventArgs e)
        {
            pinPressed(this);
        }

        public static Color GetPinColor(Type _ofType)
        {
            if (_ofType == typeof(ExecutionPin))
            {
                return Color.Black;
            }
            else if (_ofType == typeof(int))
            {
                return Color.LawnGreen;
            }
            else if (_ofType == typeof(string))
            {
                return Color.Purple;
            }
            else if (_ofType == typeof(char))
            {
                return Color.Pink;
            }
            else if (_ofType == typeof(float))
            {
                return Color.DarkGreen;
            }
            else if (_ofType == typeof(bool))
            {
                return Color.Red;
            }
            else
            {
                return Color.Gray;
            }
        }
    }

    class ExecutionPin //Class for just identifying execution input/output pin
    {

    }
}
