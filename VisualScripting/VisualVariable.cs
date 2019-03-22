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

        new public static string nodeName = "Variable"; //Not really used
        new public static List<VisualNodeC> inputs = new List<VisualNodeC>() { };
        new public static List<VisualNodeC> outputs = new List<VisualNodeC>() { };
        new public static Size nodeSize = new Size(100, 30);

        public VisualVariable(Type _variableType, string _variableName)
        {
            variableType = _variableType;
            variableName = _variableName;

            outputs.Add(new VisualNodeC(variableType, variableName, true));
        }

        public override string CompileToString()
        {
            visualOutputs[0].pinIsVariable = true;
            visualOutputs[0].pinVariable = this;
            return "";
        }
    }

    public class VisualVariableNodePanel : BaseNodePanel
    {
        public VisualVariable visualVariable;

        public VisualVariableNodePanel(VisualNode _visualNode, VisualVariable _visualVariable) : base(_visualNode)
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
