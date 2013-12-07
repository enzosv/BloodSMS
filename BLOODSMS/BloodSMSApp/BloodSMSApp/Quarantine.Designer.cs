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
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "REASON FOR QUARANTINE:";
            // 
            // quarantineReason
            // 
            this.quarantineReason.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quarantineReason.Location = new System.Drawing.Point(30, 54);
            this.quarantineReason.Name = "quarantineReason";
            this.quarantineReason.Size = new System.Drawing.Size(276, 96);
            this.quarantineReason.TabIndex = 1;
            this.quarantineReason.Text = "";
            // 
            // b_save
            // 
            this.b_save.BackColor = System.Drawing.Color.Firebrick;
            this.b_save.FlatAppearance.BorderSize = 0;
            this.b_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_save.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_save.ForeColor = System.Drawing.Color.White;
            this.b_save.Location = new System.Drawing.Point(231, 168);
            this.b_save.Name = "b_save";
            this.b_save.Size = new System.Drawing.Size(75, 36);
            this.b_save.TabIndex = 2;
            this.b_save.Text = "SAVE";
            this.b_save.UseVisualStyleBackColor = false;
            // 
            // Quarantine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(334, 227);
            this.Controls.Add(this.b_save);
            this.Controls.Add(this.quarantineReason);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
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