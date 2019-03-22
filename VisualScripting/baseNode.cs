using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace VisualScripting
{
    public struct VisualNodeC
    {
        public Type type;
        public string pinName;
        public bool isVariable;

        public VisualNodeC(Type _type, string _name = "", bool _isVariable = false)
        {
            type = _type;
            pinName = _name;
            isVariable = _isVariable;
        }
    }

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

        public static List<VisualNodeC> inputs = new List<VisualNodeC>() { };
        public static List<VisualNodeC> outputs = new List<VisualNodeC>() { };

        public List<VisualPin> visualInputs = new List<VisualPin>() { };
        public List<VisualPin> visualOutputs = new List<VisualPin>() { };
        public static Size nodeSize = new Size(100, 100);
        public BaseNodePanel baseNodePanel;
        public List<Control> specialControls = new List<Control>();
        public Point nodeLocation = new Point(0, 0);

        protected string GetCodeFromOutput(int index)
        {
            if (visualOutputs.Count > index)
            {
                if (visualOutputs[index] != null)
                {
                    if (visualOutputs[index].otherConnectedPin != null)
                    {
                        return visualOutputs[index].otherConnectedPin.visualNode.CompileToString();
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
            if (visualInputs.Count > index)
            {
                if (visualInputs[index] != null)
                {
                    if (visualInputs[index].otherConnectedPin != null)
                    {
                        if (visualInputs[index].otherConnectedPin.pinIsVariable) //Other connected pin is variable
                        {
                            if (visualInputs[index].otherConnectedPin.pinVariable == null)
                            {
                                visualInputs[index].otherConnectedPin.visualNode.CompileToString();
                            }
                            return visualInputs[index].otherConnectedPin.pinVariable.variableName;
                        }
                        else //Other connected pin isnt variable
                        {
                            visualInputs[index].otherConnectedPin.visualNode.CompileToString();
                            return visualInputs[index].otherConnectedPin.pinValue.ToString();
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

            SetupAllPins(newInputs as List<VisualNodeC>, newOutputs as List<VisualNodeC>);
            SetupSpecialControls();
        }

        protected void SetupSpecialControls()
        {
            for(int i = 0; i < visualNode.specialControls.Count; i++)
            {
                this.Controls.Add(visualNode.specialControls[i]);
            }
        }

        protected void SetupAllPins(List<VisualNodeC> _inputs, List<VisualNodeC> _outputs)
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
                VisualPin newVisualPin= new VisualPin(PinRole.Input, _inputs[i].type, _inputs[i].isVariable, _inputs[i].pinName);
                visualNode.visualInputs.Add(newVisualPin);

                BasePin newPin = new BasePin(visualNode.visualInputs[i], this);
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
                visualNode.visualInputs[i].basePin = newPin;

                newPin.MouseMove += mouseMove;
                newPin.pinPressed += PinClicked;
            }

            for (int i = 0; i < _outputs.Count; i++)
            {
                VisualPin newVisualPin = new VisualPin(PinRole.Output, _outputs[i].type, _outputs[i].isVariable, _outputs[i].pinName);
                visualNode.visualOutputs.Add(newVisualPin);

                BasePin newPin = new BasePin(visualNode.visualOutputs[i], this);
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
                this.Controls.Add(newPin);
                newPin.Location = new Point(this.Size.Width - 23, i * 12 + 13);
                outputPins.Add(newPin);
                newPin.visualPin.visualNode = visualNode;
                visualNode.visualOutputs[i].basePin = newPin;

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
