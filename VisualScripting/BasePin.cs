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

    public class BasePin : Panel
    {
        public delegate void MyEventHandler(BasePin pinPressed);
        public event MyEventHandler pinPressed;

        public PinRole pinRole;
        public BasePin otherConnectedPin;
        public Type pinType;

        public Object pinValue;

        public BaseNode parentNode;

        public BasePin(Type _pinType, PinRole _pinRole, BaseNode _parentNode = null) //Fix this _parentNode stuff
        {
            otherConnectedPin = null;
            pinType = _pinType;
            pinRole = _pinRole;
            parentNode = _parentNode;

            this.Size = new Size(10, 10);

            this.BackColor = GetPinColor(pinType);

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

    class ExecutionPin// : BasePin
    {
        //public ExecutionPin(PinRole _pinRole, BaseNode _parentNode) : base(typeof(ExecutionPin), _pinRole, _parentNode)
        //{
        //    this.BackColor = Color.Black;
        //}
    }
}
