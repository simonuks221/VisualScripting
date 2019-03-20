using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace VisualScripting
{
    public class VisualBase
    {
        public static string name;

        public VisualBase()
        {
            
        }

        public virtual string CompileToString()
        {
            throw new Exception("Not implemented");
        }
    }

    public class VisualNode : VisualBase
    {
        public static string nodeName = "Node name";
        public static List<VisualPin> inputs = new List<VisualPin>() { };
        public static List<VisualPin> outputs = new List<VisualPin>() { };
        public static Size nodeSize = new Size(100, 100);
        public BaseNodePanel baseNodePanel;
        public List<Control> specialControls = new List<Control>();
        public Point nodeLocation = new Point(0, 0);

        protected string GetCodeFromOutput(int index)
        {
            if (outputs.Count > index)
            {
                if (outputs[index] != null)
                {
                    if (outputs[index].otherConnectedPin != null)
                    {
                        return outputs[index].otherConnectedPin.visualNode.CompileToString();
                    }
                    else //Output isnt connected, thats acceptable
                    {
                        return "";
                    }
                }
            }
            return "Error, no output pin found";
        }

        protected string GetValueFromInput(int index)
        {
            if (inputs.Count > index)
            {
                if (inputs[index] != null)
                {
                    if (inputs[index].otherConnectedPin != null)
                    {
                        if (inputs[index].otherConnectedPin.pinIsVariable) //Other connected pin is variable
                        {
                            if (inputs[index].otherConnectedPin.pinVariable == null)
                            {
                                inputs[index].otherConnectedPin.visualNode.CompileToString();
                            }
                            return inputs[index].otherConnectedPin.pinVariable.variableName;
                        }
                        else //Other connected pin isnt variable
                        {
                            if (inputs[index].otherConnectedPin.pinValue == null)
                            {
                                if (inputs[index].otherConnectedPin.visualNode != null)
                                {
                                    inputs[index].otherConnectedPin.visualNode.CompileToString();
                                }
                            }
                            return inputs[index].otherConnectedPin.pinValue.ToString();
                            /*
                            if (inputPins[index].otherConnectedPin.pinType == typeof(string)) //Special cases for string and chars, not really supported by Object saving type
                            {
                                return "\"" + inputPins[index].otherConnectedPin.pinValue + "\"";
                            }
                            else if (inputPins[index].otherConnectedPin.pinType == typeof(char))
                            {
                                return "\'" + inputPins[index].otherConnectedPin.pinValue + "\'";
                            }
                            else
                            {
                                return inputPins[index].otherConnectedPin.pinValue.ToString();
                            }
                            */
                        }
                    }
                    else //Output isnt connected, thats acceptable
                    {
                        return "";
                    }
                }
            }
            return "Error, no input pin found";
        }
    }

    public class BaseNodePanel : Panel
    {
        public delegate void MyEventHandler(BasePin pinPressed);
        public event MyEventHandler pinPressed;

        public delegate void MouseDownEventHandler(BaseNodePanel senderNode, MouseEventArgs e);
        public event MouseDownEventHandler myMouseDown;
        public event MouseDownEventHandler myMouseUp;
        public event MouseDownEventHandler myMouseMove;

        public List<BasePin> inputPins;
        public List<BasePin> outputPins;

        public Label nodeLabel;
        public VisualNode visualNode;

        public BaseNodePanel(VisualNode _visualNode)
        {
            visualNode = _visualNode;
            var newInputs = visualNode.GetType().GetField("inputs").GetValue(null);
            var newOutputs = visualNode.GetType().GetField("outputs").GetValue(null);
            var newSize = visualNode.GetType().GetField("nodeSize").GetValue(null);
            this.Size = (Size)newSize;

            SetupAllPins(newInputs as List<VisualPin>, newOutputs as List<VisualPin>);
            SetupSpecialControls();
        }

        protected void SetupSpecialControls()
        {
            for(int i = 0; i < visualNode.specialControls.Count; i++)
            {
                this.Controls.Add(visualNode.specialControls[i]);
            }
        }

        protected void SetupAllPins(List<VisualPin> _inputs, List<VisualPin> _outputs)
        {
            this.MouseDown += mouseDown;
            this.MouseUp += mouseUp;
            this.MouseMove += mouseMove;

            this.BackColor = Color.LightGray;

            nodeLabel = new Label();
            this.Controls.Add(nodeLabel);
            nodeLabel.Location = new Point(0, 0);
            nodeLabel.Size = new Size(this.Size.Width, 13);
            nodeLabel.Text = visualNode.GetType().GetField("nodeName").GetValue(null).ToString();

            nodeLabel.MouseDown += mouseDown;
            nodeLabel.MouseUp += mouseUp;
            nodeLabel.MouseMove += mouseMove;

            inputPins = new List<BasePin>();
            outputPins = new List<BasePin>();

            for (int i = 0; i < _inputs.Count; i++)
            {
                BasePin newPin = new BasePin(_inputs[i], this);
                /*
                if (_inputs[i].pinType == typeof(ExecutionPin))
                {
                    //newPin = new ExecutionPin(PinRole.Input, this);
                    newPin = new BasePin(_inputs[i], PinRole.Input, this);
                }
                else
                {
                    newPin = new BasePin(_inputs[i], PinRole.Input, this);
                }
                */

                this.Controls.Add(newPin);
                newPin.Location = new Point(0, i * 12 + 13);
                inputPins.Add(newPin);
                newPin.visualPin.visualNode = visualNode;
                _inputs[i].basePin = newPin;

                newPin.MouseMove += mouseMove;
                newPin.pinPressed += PinClicked;
            }

            for (int i = 0; i < _outputs.Count; i++)
            {
                BasePin newPin;
                /*
                if (_outputs[i] == typeof(ExecutionPin))
                {
                    newPin = new BasePin(_outputs[i], PinRole.Output, this);
                }
                else
                {
                    newPin = new BasePin(_outputs[i], PinRole.Output, this);
                }
                */
                newPin = new BasePin(_outputs[i], this);
                this.Controls.Add(newPin);
                newPin.Location = new Point(this.Size.Width - 10, i * 12 + 13);
                outputPins.Add(newPin);
                newPin.visualPin.visualNode = visualNode;
                _outputs[i].basePin = newPin;

                newPin.MouseMove += mouseMove;
                newPin.pinPressed += PinClicked;
            }
        }

        private void mouseMove(object sender, MouseEventArgs e)
        {
            myMouseMove(this, e);
        }

        private void mouseUp(object sender, MouseEventArgs e)
        {
            myMouseUp(this, e);
        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            myMouseDown(this, e);
        }

        private void PinClicked(BasePin _pinPressed)
        {
            pinPressed(_pinPressed);
        }
    }
}
