using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace VisualScripting
{
    class ConstructNode : VisualNode
    {
        new public static string nodeName = "Start";
        new public static List<VisualNodeC> inputs = new List<VisualNodeC>() { };
        new public static List<VisualNodeC> outputs = new List<VisualNodeC>() {new VisualNodeC(typeof(ExecutionPin))};
        new public static Size nodeSize = new Size(100, 100);

        public ConstructNode()
        {
            
        }

        public override string CompileToString()
        {
            string allCode = GetCodeFromOutput(0); 
            return allCode;
        }
    }

    class PrintNode : VisualNode
    {
        new public static string nodeName = "Print";
        new public static List<VisualNodeC> inputs = new List<VisualNodeC>() { new VisualNodeC(typeof(ExecutionPin)) ,new VisualNodeC(typeof(string))};
        new public static List<VisualNodeC> outputs = new List<VisualNodeC>() { new VisualNodeC(typeof(ExecutionPin)) };
        new public static Size nodeSize = new Size(100, 100);

        public PrintNode()
        {
            nodeSize = new Size(50, 50);
        }

        public override string CompileToString()
        {
            string code = "ConsoleForm.Instance.AddNewMessage(" + GetValueFromInput(1) +");" + GetCodeFromOutput(0);
            return code;
        }
    }


    class MakeStringNode : VisualNode
    {
        new public static string nodeName = "Make string";
        new public static List<VisualNodeC> inputs = new List<VisualNodeC>();
        new public static List<VisualNodeC> outputs = new List<VisualNodeC>() {new VisualNodeC(typeof(string))};
        new public static Size nodeSize = new Size(100, 30);


        public MakeStringNode()
        {
            TextBox thisTextBox = new TextBox();
            thisTextBox.Location = new Point(10, 10);
            thisTextBox.Size = new Size(80, 27);
            specialControls.Add(thisTextBox);
        }

        public override string CompileToString()
        {
            TextBox thisTextBox = specialControls[0] as TextBox;

            visualOutputs[0].pinValue = "\"" + thisTextBox.Text + "\"";
            return ""; //Not gona be used
        }
    }

    class MakeIntNode : VisualNode
    {
        new public static string nodeName = "Make integer";
        new public static List<VisualNodeC> inputs = new List<VisualNodeC>() { };
        new public static List<VisualNodeC> outputs = new List<VisualNodeC>() {new VisualNodeC(typeof(int))};
        new public static Size nodeSize = new Size(50, 50);

        public MakeIntNode()
        {
            TextBox thisTextBox = new TextBox();
            thisTextBox.Location = new Point(10, 10);
            specialControls.Add(thisTextBox);
        }

        public override string CompileToString()
        {
            TextBox thisTextBox = specialControls[0] as TextBox;
            int integer = 0;
            if (Int32.TryParse(thisTextBox.Text, out integer))
            {

            }
            visualOutputs[0].pinValue = integer;
            return "";
        }
    }

    class MakeBooleanNode : VisualNode
    {
        new public static string nodeName = "Make boolean";
        new public static List<VisualNodeC> inputs = new List<VisualNodeC>() { };
        new public static List<VisualNodeC> outputs = new List<VisualNodeC>() {new VisualNodeC(typeof(bool))};
        new public static Size nodeSize = new Size(100, 100);

        public MakeBooleanNode()
        {
            nodeSize = new Size(50, 50);

            CheckBox thisCheckBox = new CheckBox();
            thisCheckBox.Location = new Point(10, 10);
            specialControls.Add(thisCheckBox);
        }

        public override string CompileToString()
        {
            CheckBox thisCheckBox = specialControls[0] as CheckBox;
            visualOutputs[0].pinValue = thisCheckBox.Checked.ToString().ToLower();
            return "";
        }
    }


    class ConvertIntToString : VisualNode
    {
        new public static string nodeName = "Int to string";
        new public static List<VisualNodeC> inputs = new List<VisualNodeC>() { new VisualNodeC(typeof(int)) };
        new public static List<VisualNodeC> outputs = new List<VisualNodeC>() { new VisualNodeC(typeof(string)) };
        new public static Size nodeSize = new Size(40, 40);

        public override string CompileToString()
        {
            int integer = Int32.Parse(GetValueFromInput(0));
            visualOutputs[0].pinValue = "\"" +  integer + "\"";
            return ""; //Not gona be used
        }
    }


    class IfNode : VisualNode
    {
        new public static string nodeName = "If";
        new public static List<VisualNodeC> inputs = new List<VisualNodeC>() { new VisualNodeC(typeof(ExecutionPin)), new VisualNodeC(typeof(bool))};
        new public static List<VisualNodeC> outputs = new List<VisualNodeC>() { new VisualNodeC(typeof(ExecutionPin), "true"), new VisualNodeC(typeof(ExecutionPin), "false")};
        new public static Size nodeSize = new Size(100, 100);

        public IfNode()
        {
            nodeSize = new Size(50, 50);

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

    class ForLoopNode : VisualNode
    {
        new public static string nodeName = "For loop";
        new public static List<VisualNodeC> inputs = new List<VisualNodeC>() { new VisualNodeC(typeof(ExecutionPin)), new VisualNodeC(typeof(int)), new VisualNodeC(typeof(int)) };
        new public static List<VisualNodeC> outputs = new List<VisualNodeC>() { new VisualNodeC(typeof(ExecutionPin)), new VisualNodeC(typeof(int)), new VisualNodeC(typeof(ExecutionPin)) };
        new public static Size nodeSize = new Size(100, 100);

        public ForLoopNode()
        {
            nodeSize = new Size(100, 100);
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

    class WhileLoopNode : VisualNode
    {
        new public static string nodeName = "While loop";
        new public static List<VisualNodeC> inputs = new List<VisualNodeC>() { new VisualNodeC(typeof(ExecutionPin)), new VisualNodeC(typeof(bool))};
        new public static List<VisualNodeC> outputs = new List<VisualNodeC>() { new VisualNodeC(typeof(ExecutionPin)), new VisualNodeC(typeof(ExecutionPin))};
        new public static Size nodeSize = new Size(100, 100);

        public WhileLoopNode()
        {
            nodeSize = new Size(100, 100);
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
