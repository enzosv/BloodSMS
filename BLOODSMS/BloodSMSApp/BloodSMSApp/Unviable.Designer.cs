namespace BloodSMSApp
{
    partial class Unviable
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
            this.b_save = new System.Windows.Forms.Button();
            this.unviableReason = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // b_save
            // 
            this.b_save.Location = new System.Drawing.Point(312, 212);
            this.b_save.Name = "b_save";
            this.b_save.Size = new System.Drawing.Size(75, 23);
            this.b_save.TabIndex = 10;
            this.b_save.Text = "SAVE";
            this.b_save.UseVisualStyleBackColor = true;
            // 
            // unviableReason
            // 
            this.unviableReason.Location = new System.Drawing.Point(61, 98);
            this.unviableReason.Name = "unviableReason";
            this.unviableReason.Size = new System.Drawing.Size(326, 96);
            this.unviableReason.TabIndex = 9;
            this.unviableReason.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "REASON FOR REMOVAL";
            // 
            // Unviable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 261);
            this.Controls.Add(this.b_save);
            this.Controls.Add(this.unviableReason);
            this.Controls.Add(this.label1);
            this.Name = "Unviable";
            this.Text = "Unviable";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button b_save;
        private System.Windows.Forms.RichTextBox unviableReason;
        private System.Windows.Forms.Label label1;
    }
}