﻿namespace BloodSMSApp
{
    partial class PreAddItem
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
            this.aNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.fName = new System.Windows.Forms.TextBox();
            this.mInitial = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.fromDonor = new System.Windows.Forms.CheckBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ACCESSION NUMBER:";
            // 
            // aNumber
            // 
            this.aNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.aNumber.Location = new System.Drawing.Point(140, 12);
            this.aNumber.Name = "aNumber";
            this.aNumber.Size = new System.Drawing.Size(198, 20);
            this.aNumber.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Donor Last Name:";
            // 
            // lName
            // 
            this.lName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.lName.Location = new System.Drawing.Point(16, 99);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(158, 20);
            this.lName.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(175, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Donor First Name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(339, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Donor Middle Initial:";
            // 
            // fName
            // 
            this.fName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.fName.Location = new System.Drawing.Point(180, 97);
            this.fName.Name = "fName";
            this.fName.Size = new System.Drawing.Size(158, 20);
            this.fName.TabIndex = 6;
            // 
            // mInitial
            // 
            this.mInitial.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.mInitial.Location = new System.Drawing.Point(344, 99);
            this.mInitial.Name = "mInitial";
            this.mInitial.Size = new System.Drawing.Size(95, 20);
            this.mInitial.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(356, 125);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(275, 125);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // fromDonor
            // 
            this.fromDonor.AutoSize = true;
            this.fromDonor.Checked = true;
            this.fromDonor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fromDonor.Location = new System.Drawing.Point(345, 13);
            this.fromDonor.Name = "fromDonor";
            this.fromDonor.Size = new System.Drawing.Size(81, 17);
            this.fromDonor.TabIndex = 10;
            this.fromDonor.Text = "From Donor";
            this.fromDonor.UseVisualStyleBackColor = true;
            this.fromDonor.CheckedChanged += new System.EventHandler(this.fromDonor_CheckedChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(140, 39);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "DATE DONATED:";
            // 
            // PreAddItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 160);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.fromDonor);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mInitial);
            this.Controls.Add(this.fName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.aNumber);
            this.Controls.Add(this.label1);
            this.Name = "PreAddItem";
            this.Text = "PreAddItem";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox aNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox lName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox fName;
        private System.Windows.Forms.TextBox mInitial;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox fromDonor;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label5;
    }
}