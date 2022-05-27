namespace Project5_starter
{
    partial class UserInterface
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
            this.label1 = new System.Windows.Forms.Label();
            this.uxWord = new System.Windows.Forms.TextBox();
            this.uxCheckButton = new System.Windows.Forms.Button();
            this.uxResultingText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(56, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter a word:";
            // 
            // uxWord
            // 
            this.uxWord.Location = new System.Drawing.Point(232, 54);
            this.uxWord.Multiline = true;
            this.uxWord.Name = "uxWord";
            this.uxWord.Size = new System.Drawing.Size(452, 35);
            this.uxWord.TabIndex = 1;
            // 
            // uxCheckButton
            // 
            this.uxCheckButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.uxCheckButton.Location = new System.Drawing.Point(328, 129);
            this.uxCheckButton.Name = "uxCheckButton";
            this.uxCheckButton.Size = new System.Drawing.Size(139, 39);
            this.uxCheckButton.TabIndex = 2;
            this.uxCheckButton.Text = "Spell Check";
            this.uxCheckButton.UseVisualStyleBackColor = true;
            this.uxCheckButton.Click += new System.EventHandler(this.uxCheckButton_Click);
            // 
            // uxResultingText
            // 
            this.uxResultingText.Location = new System.Drawing.Point(61, 213);
            this.uxResultingText.Multiline = true;
            this.uxResultingText.Name = "uxResultingText";
            this.uxResultingText.Size = new System.Drawing.Size(623, 34);
            this.uxResultingText.TabIndex = 3;
            // 
            // UserInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 293);
            this.Controls.Add(this.uxResultingText);
            this.Controls.Add(this.uxCheckButton);
            this.Controls.Add(this.uxWord);
            this.Controls.Add(this.label1);
            this.Name = "UserInterface";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox uxWord;
        private System.Windows.Forms.Button uxCheckButton;
        private System.Windows.Forms.TextBox uxResultingText;
    }
}

