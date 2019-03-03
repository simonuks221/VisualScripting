using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace VisualScripting
{
    class BasePin : Panel
    {
        public BasePin()
        {
            this.Size = new Size(10, 10);
            this.BackColor = Color.Gray;
        }
    }
}
