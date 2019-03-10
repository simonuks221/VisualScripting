using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualScripting
{
    class StartNode : BaseNode
    {
        new public static string nodeName = "Start";
        new public static List<Type> inputs = new List<Type>() {};
        new public static List<Type> outputs = new List<Type>() { typeof(int) };

        public override string CompileToString()
        {
            return "Start";
        }
    }

    class IfNode : BaseNode
    {
        new public static string nodeName = "If";
        new public static List<Type> inputs = new List<Type>() { typeof(float) };
        new public static List<Type> outputs = new List<Type>() { typeof(int) };

        public override string CompileToString()
        {
            return "IfNode";
        }
    }
}
