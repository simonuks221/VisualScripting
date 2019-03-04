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
        public delegate void MyEventHandler(BasePin pinPressed);
        public event MyEventHandler pinPressed;


        List<Type> inputs = new List<Type>() { typeof(int), typeof(string)};
        List<Type> outputs = new List<Type>() { typeof(int)};

        public List<BasePin> inputPins;
        public List<BasePin> outputPins;

        public BaseNode()
        {
            inputPins = new List<BasePin>();
            outputPins = new List<BasePin>();

            this.BackColor = Color.LightGray;
            this.Size = new Size(50, 50);

            for(int i = 0; i < inputs.Count; i++)
            {
                BasePin newPin = new BasePin(inputs[i], PinRole.Input);
                this.Controls.Add(newPin);
                newPin.Location = new Point(0, i * 12);
                inputPins.Add(newPin);

                newPin.Click += PinClicked;
            }
            for (int i = 0; i < outputs.Count; i++)
            {
                BasePin newPin = new BasePin(outputs[i],PinRole.Output);
                this.Controls.Add(newPin);
                newPin.Location = new Point(40, i * 12);
                outputPins.Add(newPin);

                newPin.Click += PinClicked;
            }
        }

        private void PinClicked(object sender, EventArgs e)
        {
            MyEventHandler handler = pinPressed;
            handler((BasePin)sender);
        }
    }
}
