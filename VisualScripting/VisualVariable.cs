using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace VisualScripting
{
    public class VisualVariable : VisualNode
    {
        public Type variableType;
        public string variableName;
        public Object variableValue = "labas tadas";

        new public static string nodeName = "Variable";
        new public List<VisualPin> inputs = new List<VisualPin>() { };
        new public List<VisualPin> outputs = new List<VisualPin>() { };

        public VisualVariable(Type _variableType, string _variableName)
        {
            variableType = _variableType;
            variableName = _variableName;

            nodeSize = new Size(100, 50);
            outputs.Add(new VisualPin(PinRole.Output,variableType, true));
        }

        public override string CompileToString()
        {
            outputs[0].pinVariable = this;
            return "";
        }
    }

    public class VisualVariableNode : BaseNodePanel
    {
        public VisualVariable visualVariable;

        public VisualVariableNode(VisualNode _visualNode, VisualVariable _visualVariable) : base(_visualNode)
        {
            
            if (_visualVariable != null)
            {
                visualVariable = _visualVariable;
            }
            else
            {
                throw new Exception("Null variable created");
            }
        }
    }
}
