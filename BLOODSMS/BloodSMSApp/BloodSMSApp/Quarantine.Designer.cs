namespace BloodSMSApp
{
    partial class Quarantine
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
            this.quarantineReason = new System.Windows.Forms.RichTextBox();
            this.b_save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "REASON FOR QUARANTINE:";
            // 
            // quarantineReason
            // 
            this.quarantineReason.Location = new System.Drawing.Point(28, 112);
            this.quarantineReason.Name = "quarantineReason";
            this.quarantineReason.Size = new System.Drawing.Size(276, 96);
            this.quarantineReason.TabIndex = 1;
            this.quarantineReason.Text = "";
            // 
            // b_save
            // 
            this.b_save.Location = new System.Drawing.Point(229, 214);
            this.b_save.Name = "b_save";
            this.b_save.Size = new System.Drawing.Size(75, 23);
            this.b_save.TabIndex = 2;
            this.b_save.Text = "SAVE";
            this.b_save.UseVisualStyleBackColor = true;
            // 
            // Quarantine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 261);
            this.Controls.Add(this.b_save);
            this.Controls.Add(this.quarantineReason);
            this.Controls.Add(this.label1);
            this.Name = "Quarantine";
            this.Text = "Quarantine";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox quarantineReason;
        private System.Windows.Forms.Button b_save;
    }
}