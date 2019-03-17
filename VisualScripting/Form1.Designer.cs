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
            this.VariableAndFunctionPanel = new System.Windows.Forms.Panel();
            this.NewVariableButton = new System.Windows.Forms.Button();
            this.NewFunctionButton = new System.Windows.Forms.Button();
            this.VariableFunctionInfoPanel = new System.Windows.Forms.Panel();
            this.NavigationPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // MainScriptingPanel
            // 
            this.MainScriptingPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.MainScriptingPanel.Location = new System.Drawing.Point(126, 35);
            this.MainScriptingPanel.Name = "MainScriptingPanel";
            this.MainScriptingPanel.Size = new System.Drawing.Size(651, 403);
            this.MainScriptingPanel.TabIndex = 0;
            this.MainScriptingPanel.Click += new System.EventHandler(this.MainScriptingPanel_MouseClick);
            this.MainScriptingPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.MainScriptingPanel_Paint);
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
            // VariableAndFunctionPanel
            // 
            this.VariableAndFunctionPanel.BackColor = System.Drawing.Color.Silver;
            this.VariableAndFunctionPanel.Location = new System.Drawing.Point(11, 93);
            this.VariableAndFunctionPanel.Name = "VariableAndFunctionPanel";
            this.VariableAndFunctionPanel.Size = new System.Drawing.Size(100, 181);
            this.VariableAndFunctionPanel.TabIndex = 2;
            // 
            // NewVariableButton
            // 
            this.NewVariableButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.NewVariableButton.Location = new System.Drawing.Point(21, 35);
            this.NewVariableButton.Name = "NewVariableButton";
            this.NewVariableButton.Size = new System.Drawing.Size(75, 23);
            this.NewVariableButton.TabIndex = 3;
            this.NewVariableButton.Text = "Variable";
            this.NewVariableButton.UseVisualStyleBackColor = true;
            this.NewVariableButton.Click += new System.EventHandler(this.NewVariableButton_Click);
            // 
            // NewFunctionButton
            // 
            this.NewFunctionButton.Location = new System.Drawing.Point(21, 64);
            this.NewFunctionButton.Name = "NewFunctionButton";
            this.NewFunctionButton.Size = new System.Drawing.Size(75, 23);
            this.NewFunctionButton.TabIndex = 4;
            this.NewFunctionButton.Text = "Function";
            this.NewFunctionButton.UseVisualStyleBackColor = true;
            this.NewFunctionButton.Click += new System.EventHandler(this.NewFunctionButton_Click);
            // 
            // VariableFunctionInfoPanel
            // 
            this.VariableFunctionInfoPanel.BackColor = System.Drawing.Color.Silver;
            this.VariableFunctionInfoPanel.Location = new System.Drawing.Point(11, 280);
            this.VariableFunctionInfoPanel.Name = "VariableFunctionInfoPanel";
            this.VariableFunctionInfoPanel.Size = new System.Drawing.Size(100, 158);
            this.VariableFunctionInfoPanel.TabIndex = 3;
            // 
            // NavigationPanel
            // 
            this.NavigationPanel.BackColor = System.Drawing.Color.Silver;
            this.NavigationPanel.Location = new System.Drawing.Point(126, 12);
            this.NavigationPanel.Name = "NavigationPanel";
            this.NavigationPanel.Size = new System.Drawing.Size(651, 24);
            this.NavigationPanel.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.NavigationPanel);
            this.Controls.Add(this.VariableFunctionInfoPanel);
            this.Controls.Add(this.NewFunctionButton);
            this.Controls.Add(this.NewVariableButton);
            this.Controls.Add(this.VariableAndFunctionPanel);
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
        private System.Windows.Forms.Panel VariableAndFunctionPanel;
        private System.Windows.Forms.Button NewVariableButton;
        private System.Windows.Forms.Button NewFunctionButton;
        private System.Windows.Forms.Panel VariableFunctionInfoPanel;
        private System.Windows.Forms.Panel NavigationPanel;
    }
}

