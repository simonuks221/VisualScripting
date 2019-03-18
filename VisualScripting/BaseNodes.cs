using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace VisualScripting
{
    class ConstructNode : BaseNodePanel
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

    class PrintNode : BaseNodePanel
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
            string code = "ConsoleForm.Instance.AddNewMessage(" + GetValueFromInput(1) +");" + GetCodeFromOutput(0);
            return code;
        }
    }

    class MakeStringNode : BaseNodePanel
    {
        new public static string nodeName = "Make string";
        public List<Type> inputs = new List<Type>() { };
        public List<Type> outputs = new List<Type>() { typeof(string) };

        TextBox thisTextBox;

        public MakeStringNode()
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
            outputPins[0].pinValue = "\"" + thisTextBox.Text + "\"";
            return ""; //Not gona be used
        }
    }

    class MakeIntNode : BaseNodePanel
    {
        new public static string nodeName = "Make integer";
        public List<Type> inputs = new List<Type>() { };
        public List<Type> outputs = new List<Type>() { typeof(int) };

        TextBox thisTextBox;

        public MakeIntNode()
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

    class MakeBooleanNode : BaseNodePanel
    {
        new public static string nodeName = "Make boolean";
        public List<Type> inputs = new List<Type>() { };
        public List<Type> outputs = new List<Type>() { typeof(bool) };

        CheckBox thisCheckBox;

        public MakeBooleanNode()
        {
            this.Size = new Size(50, 50);
            SetupAllPins(inputs, outputs);
            thisCheckBox = new CheckBox();
            this.Controls.Add(thisCheckBox);
            thisCheckBox.Location = new Point(10, 10);
        }

        public override string CompileToString()
        {
            return thisCheckBox.Checked.ToString();
        }
    }

    class IfNode : BaseNodePanel
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
            string code = @"if(" + GetValueFromInput(1) + @")
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

    class ForLoopNode : BaseNodePanel
    {
        new public static string nodeName = "For loop";
        public List<Type> inputs = new List<Type>() { typeof(ExecutionPin), typeof(int), typeof(int)};
        public List<Type> outputs = new List<Type>() { typeof(ExecutionPin),typeof(int), typeof(ExecutionPin) };

        public ForLoopNode()
        {
            this.Size = new Size(100, 100);
            SetupAllPins(inputs, outputs);
        }

        public override string CompileToString()
        {
            string higherSymbol = "";
            if (Int32.Parse(GetValueFromInput(2)) > Int32.Parse(GetValueFromInput(1)))
            {
                higherSymbol = "<";
            }
            else
            {
                higherSymbol = ">";
            }

            string code = @"
for(int i = " +GetValueFromInput(1) +";i "+ higherSymbol +" " +GetValueFromInput(2) +@";i++)
{
" + GetCodeFromOutput(0) + @"
}"
+ GetCodeFromOutput(2);
            return code;
        }
    }

    class WhileLoopNode : BaseNodePanel
    {
        new public static string nodeName = "While loop";
        public List<Type> inputs = new List<Type>() { typeof(ExecutionPin), typeof(bool)};
        public List<Type> outputs = new List<Type>() { typeof(ExecutionPin), typeof(ExecutionPin) };

        public WhileLoopNode()
        {
            this.Size = new Size(100, 100);
            SetupAllPins(inputs, outputs);
        }

        public override string CompileToString()
        {

            string code = @"
while("+ GetValueFromInput(1)+@")
{
"+ GetCodeFromOutput(0) +@"
}";
            return code;
        }
    }
}
