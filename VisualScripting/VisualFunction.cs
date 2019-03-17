﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualScripting
{
    public class VisualFunction
    {
        public string name;
        public List<Type> inputs;
        public List<Type> outputs;

        public VisualFunction(string _name)
        {
            name = _name;
        }
    }
}
