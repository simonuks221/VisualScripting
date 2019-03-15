﻿namespace VisualScripting
{
    partial class ConsoleForm
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
            this.ConsoleMessagePanel = new System.Windows.Forms.Panel();
            this.EreaseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConsoleMessagePanel
            // 
            this.ConsoleMessagePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ConsoleMessagePanel.Location = new System.Drawing.Point(12, 44);
            this.ConsoleMessagePanel.Name = "ConsoleMessagePanel";
            this.ConsoleMessagePanel.Size = new System.Drawing.Size(851, 394);
            this.ConsoleMessagePanel.TabIndex = 0;
            // 
            // EreaseButton
            // 
            this.EreaseButton.Location = new System.Drawing.Point(12, 12);
            this.EreaseButton.Name = "EreaseButton";
            this.EreaseButton.Size = new System.Drawing.Size(75, 23);
            this.EreaseButton.TabIndex = 1;
            this.EreaseButton.Text = "Erease";
            this.EreaseButton.UseVisualStyleBackColor = true;
            this.EreaseButton.Click += new System.EventHandler(this.EreaseButton_Click);
            // 
            // ConsoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 450);
            this.Controls.Add(this.EreaseButton);
            this.Controls.Add(this.ConsoleMessagePanel);
            this.Name = "ConsoleForm";
            this.Text = "Console";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConsoleForm_FormClosing);
            this.Load += new System.EventHandler(this.ConsoleForm_Load);
            this.Resize += new System.EventHandler(this.ConsoleForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ConsoleMessagePanel;
        private System.Windows.Forms.Button EreaseButton;
    }
}

