using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace VisualScripting
{
    public class VisualFunction : VisualNode
    {
        public string functionName;

        new public static string nodeName = "Function"; //Not really used
        new public static List<VisualNodeC> inputs = new List<VisualNodeC>() { new VisualNodeC(typeof(ExecutionPin))};
        new public static List<VisualNodeC> outputs = new List<VisualNodeC>() { new VisualNodeC(typeof(ExecutionPin))};
        new public static Size nodeSize = new Size(100, 100);

        public List<VisualNode> visualNodes;
        public List<VisualVariable> visualVariables;

        public VisualFunction(string _name)
        {
            functionName = _name;
            visualNodes = new List<VisualNode>();
            visualVariables = new List<VisualVariable>();
        }

        public override string CompileToString()
        {
            return "";
        }
    }

    public class VisualFunctionNodePanel : BaseNodePanel
    {
        public VisualFunction visualFunction;

        public VisualFunctionNodePanel(VisualNode _visualNode, VisualFunction _visualFunction) : base(_visualNode)
        {
            if (_visualFunction != null)
            {
                visualFunction = _visualFunction;
            }
            else
            {
                throw new Exception("Null function created");
            }
        }
    }

    class FunctionStartNode : VisualNode
    {
        new public static string nodeName = "Function end";
        new public static List<VisualNodeC> inputs = new List<VisualNodeC>() { };
        new public static List<VisualNodeC> outputs = new List<VisualNodeC>() { new VisualNodeC(typeof(ExecutionPin)) };
        new public static Size nodeSize = new Size(100, 100);

        public FunctionStartNode()
        {

        }

        public override string CompileToString()
        {
            string allCode = GetCodeFromOutput(0);
            return allCode;
        }
    }

    class FunctionEndNode : VisualNode
    {
        new public static string nodeName = "Function start";
        new public static List<VisualNodeC> inputs = new List<VisualNodeC>() { new VisualNodeC(typeof(ExecutionPin)) };
        new public static List<VisualNodeC> outputs = new List<VisualNodeC>() {  };
        new public static Size nodeSize = new Size(100, 100);

        public FunctionEndNode()
        {

        }

        public override string CompileToString()
        {
            return "";
        }
    }
}
