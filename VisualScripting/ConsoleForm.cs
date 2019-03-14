using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace VisualScripting
{
    public partial class ConsoleForm : Form
    {
        public static ConsoleForm consoleForm; //Singleton

        public ConsoleForm()
        {
            InitializeComponent();
        }

        public void ConsoleForm_Load(object sender, EventArgs e)
        {
            consoleForm = this;
            this.Size = new Size(600, 300);
            Console.Out.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaa");
        }

        private void ConsoleForm_Resize(object sender, EventArgs e)
        {
            ConsoleMessagePanel.Size = new Size(this.Size.Width - 50, this.Size.Height - 75);
            ConsoleMessagePanel.Location = new Point(10, 10);
            Console.Out.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaa");
        }

        void UpdateConsoleMessages()
        {

        }
    }
}
