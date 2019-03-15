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
        public static ConsoleForm Instance; //Singleton

        List<ConsoleMessagePanel> messageControls = new List<ConsoleMessagePanel>();

        public ConsoleForm()
        {
            InitializeComponent();
        }

        public void ConsoleForm_Load(object sender, EventArgs e)
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Instance.Close();
                Instance = this;
            }
            this.Size = new Size(600, 300);

            UpdateConsoleMessages();
        }

        private void ConsoleForm_Resize(object sender, EventArgs e)
        {
            ConsoleMessagePanel.Size = new Size(this.Size.Width - 50, this.Size.Height - 75);
            ConsoleMessagePanel.Location = new Point(12, 44);
            UpdateConsoleMessages();
        }

        public void AddNewMessage(string _message)
        {
            ConsoleMessagePanel newMessage = new ConsoleMessagePanel(_message);
            ConsoleMessagePanel.Controls.Add(newMessage);
            newMessage.Size = new Size(ConsoleMessagePanel.Size.Width, 20);
            newMessage.Location = new Point(0, (ConsoleMessagePanel.Controls.Count - 1) * 21);
            messageControls.Add(newMessage);
            UpdateConsoleMessages();
        }

        void UpdateConsoleMessages()
        {
            for (int i = 0; i < ConsoleMessagePanel.Controls.Count; i++)
            {
                ConsoleMessagePanel panel = (ConsoleMessagePanel)ConsoleMessagePanel.Controls[i];
                panel.Size = new Size(ConsoleMessagePanel.Size.Width, 20);
                panel.messageLabel.Size = panel.Size;
            }
            ConsoleMessagePanel.Refresh();
        }

        private void EreaseButton_Click(object sender, EventArgs e)
        {
            if (ConsoleMessagePanel.Controls.Count > 0)
            {
                foreach (Control c in messageControls)
                {
                    c.Dispose();
                }
                messageControls.Clear();
                ConsoleMessagePanel.Refresh();
            }
        }

        private void ConsoleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Instance = null;
        }
    }
}
