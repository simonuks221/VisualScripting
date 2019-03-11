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

    class BasePin : Panel
    {
        public PinRole pinRole;
        public BasePin otherConnectedPin;
        public Type pinType;

        public BaseNode parentNode;

        public BasePin(Type _pinType, PinRole _pinRole, BaseNode _parentNode)
        {
            otherConnectedPin = null;
            pinType = _pinType;
            pinRole = _pinRole;
            parentNode = _parentNode;

            this.Size = new Size(10, 10);

            Console.Out.WriteLine(pinType);

            if(pinType == typeof(ExecutionPin))
            {
                this.BackColor = Color.Black;
            }
            else if (pinType == typeof(int))
            {
                this.BackColor = Color.LawnGreen;
            }
            else if(pinType == typeof(string))
            {
                this.BackColor = Color.Purple;
            }
            else if (pinType == typeof(char))
            {
                this.BackColor = Color.Pink;
            }
            else if (pinType == typeof(float))
            {
                this.BackColor = Color.DarkGreen;
            }
            else if (pinType == typeof(bool))
            {
                this.BackColor = Color.Red;
            }
            else
            {
                this.BackColor = Color.Gray;
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
