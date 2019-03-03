using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace VisualScripting
{
    class CreateNodePart : Panel
    {
        Type thisType;

        Label typeLabel;

        public CreateNodePart(Type _thisType)
        {
            thisType = _thisType;

            this.Size = new Size(200, 10);
            this.BackColor = Color.White;

            typeLabel = new Label();
            this.Controls.Add(typeLabel);
            typeLabel.Size = new Size(100, 10);
            typeLabel.Location = new Point();
            typeLabel.Text = thisType.ToString();
        }
    }
}
