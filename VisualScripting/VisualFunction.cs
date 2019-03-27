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
        new public static List<VisualNodeC> inputs = new List<VisualNodeC>() { };
        new public static List<VisualNodeC> outputs = new List<VisualNodeC>() { };
        new public static Size nodeSize = new Size(100, 100);

        public VisualFunction(string _name)
        {
            functionName = _name;
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
}
