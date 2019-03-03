using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace VisualScripting
{
    class BaseNode : Panel
    {

        List<Type> inputs = new List<Type>() { typeof(int), typeof(string)};

        public BaseNode()
        {
            this.BackColor = Color.LightGray;
            this.Size = new Size(50, 50);

            for(int i = 0; i < inputs.Count; i++)
            {
                BasePin newPin = new BasePin();
                this.Controls.Add(newPin);
                newPin.Location = new Point(0, i * 12);
            }
        }

        public void Moves()
        {
            //this.Size = new Size(50, 50);
        }
    }
}
