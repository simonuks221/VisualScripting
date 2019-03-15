using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

public class ConsoleMessagePanel : Button
{
    public Label messageLabel;

    public ConsoleMessagePanel(string _message)
    {
        messageLabel = new Label();
        this.Controls.Add(messageLabel);
        messageLabel.Size = this.Size;
        messageLabel.Font = new Font("Arial", 13, FontStyle.Bold);
        messageLabel.Text = _message;
        messageLabel.BackColor = Color.DarkGray;
    }
}