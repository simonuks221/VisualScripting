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

        //public List<Type> inputs = new List<Type>() { typeof(ExecutionPin)};
        //public List<Type> outputs = new List<Type>() { typeof(ExecutionPin) };

        public List<BasePin> inputPins;
        public List<BasePin> outputPins;

        public static string nodeName = "Node name here";

        public BaseNode()
        {
            
        }

        protected void SetupAllPins(List<Type> _inputs, List<Type> _outputs)
        {
            inputPins = new List<BasePin>();
            outputPins = new List<BasePin>();

            this.BackColor = Color.LightGray;
            this.Size = new Size(50, 50);

            for (int i = 0; i < _inputs.Count; i++)
            {
                BasePin newPin;
                if (_inputs[i] == typeof(ExecutionPin))
                {
                    //newPin = new ExecutionPin(PinRole.Input, this);
                    newPin = new BasePin(_inputs[i], PinRole.Input, this);
                }
                else
                {
                    newPin = new BasePin(_inputs[i], PinRole.Input, this);
                }
                this.Controls.Add(newPin);
                newPin.Location = new Point(0, i * 12);
                inputPins.Add(newPin);

                newPin.Click += PinClicked;
            }

            for (int i = 0; i < _outputs.Count; i++)
            {
                BasePin newPin;
                if (_outputs[i] == typeof(ExecutionPin))
                {
                    //newPin = new ExecutionPin(PinRole.Output, this);
                    newPin = new BasePin(_outputs[i], PinRole.Output, this);
                }
                else
                {
                    newPin = new BasePin(_outputs[i], PinRole.Output, this);
                }
                this.Controls.Add(newPin);
                newPin.Location = new Point(40, i * 12);
                outputPins.Add(newPin);

                newPin.Click += PinClicked;
            }
        }

        protected string GetCodeFromOutput(int index)
        {
            if(outputPins.Count > index)
            {
                if (outputPins[index] != null)
                {
                    if (outputPins[index].otherConnectedPin != null)
                    {
                        return outputPins[index].otherConnectedPin.parentNode.CompileToString();
                    }
                    else //Output isnt connected, thats acceptable
                    {
                        return "";
                    }
                }
            }
            return "Error, no output pin found";
        }

        public virtual string CompileToString()
        {
            return "Not implemented node compilation";
        }

        private void PinClicked(object sender, EventArgs e)
        {
            MyEventHandler handler = pinPressed;
            handler((BasePin)sender);
        }
    }
}
