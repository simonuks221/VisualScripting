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

        public BasePin(Type _pinType, PinRole _pinRole)
        {
            otherConnectedPin = null;
            pinType = _pinType;
            pinRole = _pinRole;

            this.Size = new Size(10, 10);
            this.BackColor = Color.Gray;
        }
    }
}
