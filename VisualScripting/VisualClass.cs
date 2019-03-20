using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualScripting
{
    public class VisualClass : VisualBase
    {
        public string className;

        public List<VisualNode> currentNodes;
        public List<VisualVariable> visualVariables;
        public List<VisualFunction> visualFunctions;

        public VisualClass(string _name)
        {
            className = _name;
            currentNodes = new List<VisualNode>();
            visualFunctions = new List<VisualFunction>();
            visualVariables = new List<VisualVariable>();
        }

        public override string CompileToString()
        {
            string allCode = @"public class " + className + " { \n";

            for(int i = 0; i < visualVariables.Count; i++)
            {
                allCode += visualVariables[i].variableName + " = " + visualVariables[i].variableValue + "; \n";
            }

            allCode += currentNodes[0].CompileToString(); //Construct node needs to be first

            for(int i = 0; i < visualFunctions.Count; i++)
            {
                //visualFunctions[i].c
            }

            allCode += "}";

            return allCode;
        }
    }
}
