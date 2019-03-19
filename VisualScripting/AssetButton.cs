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
        protected Label assetLabel;

        public delegate void MyHandler(BaseAssetButton _asset);
        public event MyHandler assetPressed;

        public BaseAssetButton()
        {
            this.BackColor = Color.DimGray;
            this.Size = new Size(100, 100);

            assetLabel = new Label();
            this.Controls.Add(assetLabel);
            assetLabel.Size = new Size(80, 80);
            assetLabel.Location = new Point(10, 10);

            this.Click += AssetPressed;
            assetLabel.Click += AssetPressed;
        }

        private void AssetPressed(object sender, EventArgs e)
        {
            assetPressed(this);
        }
    }

    public class ClassAssetButton : BaseAssetButton
    {
        public VisualClass visualClass;

        public ClassAssetButton(VisualClass _visualClass) : base()
        {
            visualClass = _visualClass;
            assetLabel.Text = visualClass.ToString();
        }
    }
}
