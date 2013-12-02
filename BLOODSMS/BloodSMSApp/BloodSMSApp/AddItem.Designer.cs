namespace BloodSMSApp
{
    partial class AddItem
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
            this.dateAddedField = new System.Windows.Forms.DateTimePicker();
            this.label20 = new System.Windows.Forms.Label();
            this.dateExpireField = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.b_add = new System.Windows.Forms.Button();
            this.takenFromField = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.t_patientName = new System.Windows.Forms.TextBox();
            this.t = new System.Windows.Forms.Label();
            this.t_patientAge = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.t_accessionNumber = new System.Windows.Forms.TextBox();
            this.baba = new System.Windows.Forms.Label();
            this.b_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dateAddedField
            // 
            this.dateAddedField.Location = new System.Drawing.Point(44, 476);
            this.dateAddedField.Name = "dateAddedField";
            this.dateAddedField.Size = new System.Drawing.Size(200, 20);
            this.dateAddedField.TabIndex = 36;
            this.dateAddedField.ValueChanged += new System.EventHandler(this.dateAddedField_ValueChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(41, 458);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(99, 16);
            this.label20.TabIndex = 33;
            this.label20.Text = "DATE ADDED:";
            // 
            // dateExpireField
            // 
            this.dateExpireField.Location = new System.Drawing.Point(44, 529);
            this.dateExpireField.Name = "dateExpireField";
            this.dateExpireField.Size = new System.Drawing.Size(200, 20);
            this.dateExpireField.TabIndex = 38;
            this.dateExpireField.ValueChanged += new System.EventHandler(this.dateExpireField_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(41, 511);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 16);
            this.label2.TabIndex = 37;
            this.label2.Text = "DATE OF EXPIRATION:";
            // 
            // b_add
            // 
            this.b_add.BackColor = System.Drawing.Color.Firebrick;
            this.b_add.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.b_add.FlatAppearance.BorderSize = 0;
            this.b_add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_add.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_add.ForeColor = System.Drawing.Color.White;
            this.b_add.Location = new System.Drawing.Point(156, 562);
            this.b_add.Name = "b_add";
            this.b_add.Size = new System.Drawing.Size(88, 33);
            this.b_add.TabIndex = 61;
            this.b_add.Text = "ADD";
            this.b_add.UseVisualStyleBackColor = false;
            this.b_add.Click += new System.EventHandler(this.button1_Click);
            // 
            // takenFromField
            // 
            this.takenFromField.Location = new System.Drawing.Point(44, 138);
            this.takenFromField.Name = "takenFromField";
            this.takenFromField.Size = new System.Drawing.Size(200, 20);
            this.takenFromField.TabIndex = 35;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(41, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 16);
            this.label1.TabIndex = 31;
            this.label1.Text = "DONOR NAME:";
            // 
            // listBox1
            // 
            this.listBox1.Enabled = false;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(44, 165);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(200, 186);
            this.listBox1.TabIndex = 62;
            // 
            // t_patientName
            // 
            this.t_patientName.Location = new System.Drawing.Point(44, 375);
            this.t_patientName.Name = "t_patientName";
            this.t_patientName.Size = new System.Drawing.Size(200, 20);
            this.t_patientName.TabIndex = 64;
            // 
            // t
            // 
            this.t.AutoSize = true;
            this.t.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.t.Location = new System.Drawing.Point(41, 356);
            this.t.Name = "t";
            this.t.Size = new System.Drawing.Size(111, 16);
            this.t.TabIndex = 63;
            this.t.Text = "PATIENT NAME:";
            // 
            // t_patientAge
            // 
            this.t_patientAge.Location = new System.Drawing.Point(44, 426);
            this.t_patientAge.Name = "t_patientAge";
            this.t_patientAge.Size = new System.Drawing.Size(200, 20);
            this.t_patientAge.TabIndex = 66;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(41, 407);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 16);
            this.label4.TabIndex = 65;
            this.label4.Text = "PATIENT AGE:";
            // 
            // t_accessionNumber
            // 
            this.t_accessionNumber.Location = new System.Drawing.Point(44, 87);
            this.t_accessionNumber.Name = "t_accessionNumber";
            this.t_accessionNumber.Size = new System.Drawing.Size(200, 20);
            this.t_accessionNumber.TabIndex = 68;
            // 
            // baba
            // 
            this.baba.AutoSize = true;
            this.baba.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baba.Location = new System.Drawing.Point(41, 68);
            this.baba.Name = "baba";
            this.baba.Size = new System.Drawing.Size(150, 16);
            this.baba.TabIndex = 67;
            this.baba.Text = "ACCESSION NUMBER:";
            // 
            // b_cancel
            // 
            this.b_cancel.BackColor = System.Drawing.Color.Firebrick;
            this.b_cancel.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.b_cancel.FlatAppearance.BorderSize = 0;
            this.b_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_cancel.ForeColor = System.Drawing.Color.White;
            this.b_cancel.Location = new System.Drawing.Point(44, 562);
            this.b_cancel.Name = "b_cancel";
            this.b_cancel.Size = new System.Drawing.Size(96, 33);
            this.b_cancel.TabIndex = 69;
            this.b_cancel.Text = "CANCEL";
            this.b_cancel.UseVisualStyleBackColor = false;
            // 
            // AddItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::BloodSMSApp.Properties.Resources.addItem_copy;
            this.ClientSize = new System.Drawing.Size(286, 593);
            this.Controls.Add(this.b_cancel);
            this.Controls.Add(this.t_accessionNumber);
            this.Controls.Add(this.baba);
            this.Controls.Add(this.t_patientAge);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.t_patientName);
            this.Controls.Add(this.t);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.b_add);
            this.Controls.Add(this.dateExpireField);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateAddedField);
            this.Controls.Add(this.takenFromField);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddItem";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateAddedField;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.DateTimePicker dateExpireField;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button b_add;
        private System.Windows.Forms.TextBox takenFromField;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox t_patientName;
        private System.Windows.Forms.Label t;
        private System.Windows.Forms.TextBox t_patientAge;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox t_accessionNumber;
        private System.Windows.Forms.Label baba;
        private System.Windows.Forms.Button b_cancel;
    }
}