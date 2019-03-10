namespace VisualScripting
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainScriptingPanel = new System.Windows.Forms.Panel();
            this.CompileButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MainScriptingPanel
            // 
            this.MainScriptingPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.MainScriptingPanel.Location = new System.Drawing.Point(12, 31);
            this.MainScriptingPanel.Name = "MainScriptingPanel";
            this.MainScriptingPanel.Size = new System.Drawing.Size(765, 384);
            this.MainScriptingPanel.TabIndex = 0;
            this.MainScriptingPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.MainScriptingPanel_Paint);
            this.MainScriptingPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainScriptingPanel_MouseClick);
            this.MainScriptingPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainScriptingPanel_MouseMove);
            // 
            // CompileButton
            // 
            this.CompileButton.Location = new System.Drawing.Point(21, 2);
            this.CompileButton.Name = "CompileButton";
            this.CompileButton.Size = new System.Drawing.Size(75, 23);
            this.CompileButton.TabIndex = 1;
            this.CompileButton.Text = "Compile";
            this.CompileButton.UseVisualStyleBackColor = true;
            this.CompileButton.Click += new System.EventHandler(this.CompileButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CompileButton);
            this.Controls.Add(this.MainScriptingPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainScriptingPanel;
        private System.Windows.Forms.Button CompileButton;
    }
}

