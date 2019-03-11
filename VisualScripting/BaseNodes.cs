using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualScripting
{
    class ConstructNode : BaseNode
    {
        new public static string nodeName = "Start";
        public List<Type> inputs = new List<Type>() {};
        public List<Type> outputs = new List<Type>() {typeof(ExecutionPin)};

        public ConstructNode()
        {
            SetupAllPins(inputs, outputs);
        }

        public override string CompileToString()
        {
            return GetCodeFromOutput(0);
        }
    }

    class PrintNode : BaseNode
    {
        new public static string nodeName = "Print";
        public List<Type> inputs = new List<Type>() { typeof(ExecutionPin), typeof(string) };
        public List<Type> outputs = new List<Type>() { typeof(ExecutionPin) };

        public PrintNode()
        {
            SetupAllPins(inputs, outputs);
        }

        public override string CompileToString()
        {
            string code = "Console.out.WriteLine(" + GetCodeFromInput(1) +");" + GetCodeFromOutput(0);
            return code;
        }
    }

    class MakeString : BaseNode
    {
        new public static string nodeName = "Make string";
        public List<Type> inputs = new List<Type>() { };
        public List<Type> outputs = new List<Type>() { typeof(string) };

        TextBox thisTextBox;

        public MakeString()
        {
            SetupAllPins(inputs, outputs);
            thisTextBox = new TextBox();
            this.Controls.Add(thisTextBox);
            thisTextBox.Location = new System.Drawing.Point(10, 10);
        }

        public override string CompileToString()
        {
            string code = thisTextBox.Text;
            return code;
        }
    }

    class IfNode : BaseNode
    {
        new public static string nodeName = "If";
        public List<Type> inputs = new List<Type>() {typeof(ExecutionPin), typeof(bool)};
        public List<Type> outputs = new List<Type>() {typeof(ExecutionPin), typeof(ExecutionPin) };

        public IfNode()
        {
            SetupAllPins(inputs, outputs);
        }

        public override string CompileToString()
        {
            string code = @"if(" + GetCodeFromInput(1) + @")
                {"
                + GetCodeFromOutput(0) +
                @"}
                else
                {"
                + GetCodeFromOutput(1) +
                "}";
            return code;
        }
    }
}
