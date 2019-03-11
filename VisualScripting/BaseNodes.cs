using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualScripting
{
    class ConstructNode : BaseNode
    {
        new public static string nodeName = "Start";
        new public List<Type> inputs = new List<Type>() {};
        new public List<Type> outputs = new List<Type>() {typeof(ExecutionPin)};

        public ConstructNode()
        {
            SetupAllPins(inputs, outputs);
        }

        public override string CompileToString()
        {
            return GetCodeFromOutput(0); ;
        }
    }

    class IfNode : BaseNode
    {
        new public static string nodeName = "If";
        new public List<Type> inputs = new List<Type>() {typeof(ExecutionPin), typeof(bool)};
        new public List<Type> outputs = new List<Type>() {typeof(ExecutionPin), typeof(ExecutionPin) };

        public IfNode()
        {
            SetupAllPins(inputs, outputs);
        }

        public override string CompileToString()
        {
            Console.Out.WriteLine(outputs.Count);
            return "IfNode";
        }
    }
}
