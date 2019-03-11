using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace VisualScripting
{
    class ConstructNode : BaseNode
    {
        new public static string nodeName = "Start";
        public List<Type> inputs = new List<Type>() {};
        public List<Type> outputs = new List<Type>() {typeof(ExecutionPin)};

        public ConstructNode()
        {
            this.Size = new Size(50, 50);
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
            this.Size = new Size(50, 50);
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
            this.Size = new Size(100, 30);
            SetupAllPins(inputs, outputs);
            thisTextBox = new TextBox();
            this.Controls.Add(thisTextBox);
            thisTextBox.Location = new Point(10, 10);
            thisTextBox.Size = new Size(80, 27);
        }

        public override string CompileToString()
        {
            string code = thisTextBox.Text;
            return code;
        }
    }

    class MakeInt : BaseNode
    {
        new public static string nodeName = "Make integer";
        public List<Type> inputs = new List<Type>() { };
        public List<Type> outputs = new List<Type>() { typeof(int) };

        TextBox thisTextBox;

        public MakeInt()
        {
            this.Size = new Size(50, 50);
            SetupAllPins(inputs, outputs);
            thisTextBox = new TextBox();
            this.Controls.Add(thisTextBox);
            thisTextBox.Location = new System.Drawing.Point(10, 10);
        }

        public override string CompileToString()
        {
            int integer = 0;
            if (Int32.TryParse(thisTextBox.Text, out integer))
            {

            }
            return integer.ToString();
        }
    }

    class IfNode : BaseNode
    {
        new public static string nodeName = "If";
        public List<Type> inputs = new List<Type>() {typeof(ExecutionPin), typeof(bool)};
        public List<Type> outputs = new List<Type>() {typeof(ExecutionPin), typeof(ExecutionPin) };

        public IfNode()
        {
            this.Size = new Size(50, 50);
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
