﻿using System;
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
        new public static List<Type> inputs = new List<Type>();
        new public static List<Type> outputs = new List<Type>() {typeof(ExecutionPin)};
        new public static Size nodeSize = new Size(100, 100);

        public ConstructNode()
        {
            
        }

        public override string CompileToString()
        {
            return GetCodeFromOutput(0);
        }
    }

    class PrintNode : VisualNode
    {
        new public static string nodeName = "Print";
        new public static List<Type> inputs = new List<Type>() { typeof(ExecutionPin), typeof(string) };
        new public static List<Type> outputs = new List<Type>() { typeof(ExecutionPin) };
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
        new public static List<Type> inputs = new List<Type>();
        new public static List<Type> outputs = new List<Type>() { typeof(string) };
        new public static Size nodeSize = new Size(100, 100);


        public MakeStringNode()
        {
            nodeSize = new Size(100, 30);

            TextBox thisTextBox = new TextBox();
            thisTextBox.Location = new Point(10, 10);
            thisTextBox.Size = new Size(80, 27);
            specialControls.Add(thisTextBox);
        }

        public override string CompileToString()
        {
            TextBox thisTextBox = specialControls[0] as TextBox;

            baseNodePanel.outputPins[0].pinValue = "\"" + thisTextBox.Text + "\"";
            return ""; //Not gona be used
        }
    }

    class MakeIntNode : VisualNode
    {
        new public static string nodeName = "Make integer";
        new public static List<Type> inputs = new List<Type>() { };
        new public static List<Type> outputs = new List<Type>() { typeof(int) };
        new public static Size nodeSize = new Size(100, 100);

        public MakeIntNode()
        {
            nodeSize = new Size(50, 50);

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
            return integer.ToString();
        }
    }

    class MakeBooleanNode : VisualNode
    {
        new public static string nodeName = "Make boolean";
        new public static List<Type> inputs = new List<Type>() { };
        new public static List<Type> outputs = new List<Type>() { typeof(bool) };
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
            return thisCheckBox.Checked.ToString();
        }
    }

    class IfNode : VisualNode
    {
        new public static string nodeName = "If";
        new public static List<Type> inputs = new List<Type>() {typeof(ExecutionPin), typeof(bool)};
        new public static List<Type> outputs = new List<Type>() {typeof(ExecutionPin), typeof(ExecutionPin) };
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
        new public static List<Type> inputs = new List<Type>() { typeof(ExecutionPin), typeof(int), typeof(int)};
        new public static List<Type> outputs = new List<Type>() { typeof(ExecutionPin),typeof(int), typeof(ExecutionPin) };
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
        new public static List<Type> inputs = new List<Type>() { typeof(ExecutionPin), typeof(bool)};
        new public static List<Type> outputs = new List<Type>() { typeof(ExecutionPin), typeof(ExecutionPin) };
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
