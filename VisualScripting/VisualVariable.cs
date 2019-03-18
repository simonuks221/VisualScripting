using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace VisualScripting
{
    public class VisualVariable //: BaseSpawnableItem W.I.P
    {
        public Type variableType;
        public string variableName;
        public Object variableValue = "labas tadas";

        public VisualVariable(Type _variableType, string _variableName)
        {
            variableType = _variableType;
            variableName = _variableName;
        }
    }

    public class VisualVariableNode : BaseNodePanel
    {
        public VisualVariable visualVariable;

        new public static string nodeName = "Variable";
        public List<Type> inputs = new List<Type>() {};
        public List<Type> outputs = new List<Type>() {};

        public VisualVariableNode(VisualVariable _visualVariable)
        {
            this.Size = new Size(100, 50);
            if (_visualVariable != null)
            {
                visualVariable = _visualVariable;
            }
            else
            {
                throw new Exception("Null variable created");
            }
            outputs.Add(visualVariable.variableType);
            SetupAllPins(inputs, outputs);
            outputPins[0].pinIsVariable = true;
        }

        public override string CompileToString()
        {
            outputPins[0].pinVariable = visualVariable;
            return "";
        }
    }
}
