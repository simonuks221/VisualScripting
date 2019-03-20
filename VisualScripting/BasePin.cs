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

        public VisualPin(PinRole _pinRole, Type _type, bool _isVariable)
        {
            pinRole = _pinRole;
            pinType = _type;
            pinIsVariable = _isVariable;
        }
    }

    public class BasePin : Panel
    {
        public delegate void MyEventHandler(BasePin pinPressed);
        public event MyEventHandler pinPressed;
        
        public BaseNodePanel parentNode;

        public VisualPin visualPin;

        public BasePin(VisualPin _visualPin, BaseNodePanel _parentNode = null) //Fix this _parentNode stuff
        {
            visualPin = _visualPin;
            parentNode = _parentNode;

            this.Size = new Size(10, 10);

            this.BackColor = GetPinColor(visualPin.pinType);

            this.Click += BasePinClick;
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
