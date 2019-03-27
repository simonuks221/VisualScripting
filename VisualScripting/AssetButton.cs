using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace VisualScripting
{
    public class BaseAssetButton : Panel
    {


        public delegate void MyHandler(BaseAssetButton _asset);
        public event MyHandler assetPressed;

        public BaseAssetButton()
        {
            this.BackColor = Color.DimGray;
            this.Size = new Size(100, 100);

            this.Click += AssetPressed;
        }

        protected void AssetPressed(object sender, EventArgs e)
        {
            assetPressed(this);
        }
    }

    public class ClassAssetButton : BaseAssetButton
    {
        public VisualClass visualClass;

        public TextBox classTextBox;

        public ClassAssetButton(VisualClass _visualClass) : base()
        {
            visualClass = _visualClass;

            classTextBox = new TextBox();
            this.Controls.Add(classTextBox);
            classTextBox.Size = new Size(80, 80);
            classTextBox.Location = new Point(10, 10);
            classTextBox.Text = visualClass.className;
        }
    }
}
