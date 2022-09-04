namespace spravochnik.dicTypeXposMark
{
    partial class frmAdd
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
            this.tbName = new System.Windows.Forms.TextBox();
            this.lName = new System.Windows.Forms.Label();
            this.tbDays = new System.Windows.Forms.TextBox();
            this.lCountDay = new System.Windows.Forms.Label();
            this.btSave = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(105, 12);
            this.tbName.MaxLength = 1024;
            this.tbName.Name = "tbName";
            this.tbName.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbName.Size = new System.Drawing.Size(388, 20);
            this.tbName.TabIndex = 2;
            this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
            // 
            // lName
            // 
            this.lName.AutoSize = true;
            this.lName.Location = new System.Drawing.Point(9, 16);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(83, 13);
            this.lName.TabIndex = 6;
            this.lName.Text = "Наименование";
            // 
            // tbDays
            // 
            this.tbDays.Location = new System.Drawing.Point(105, 47);
            this.tbDays.MaxLength = 7;
            this.tbDays.Name = "tbDays";
            this.tbDays.Size = new System.Drawing.Size(140, 20);
            this.tbDays.TabIndex = 8;
            this.tbDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbDays_KeyPress);
            // 
            // lCountDay
            // 
            this.lCountDay.AutoSize = true;
            this.lCountDay.Location = new System.Drawing.Point(9, 51);
            this.lCountDay.Name = "lCountDay";
            this.lCountDay.Size = new System.Drawing.Size(90, 13);
            this.lCountDay.TabIndex = 9;
            this.lCountDay.Text = "Значение Frontol";
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Location = new System.Drawing.Point(423, 40);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(32, 32);
            this.btSave.TabIndex = 6;
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.Location = new System.Drawing.Point(461, 40);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(32, 32);
            this.btClose.TabIndex = 7;
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // frmAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 84);
            this.Controls.Add(this.tbDays);
            this.Controls.Add(this.lCountDay);
            this.Controls.Add(this.lName);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.btClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAdd";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmAdd";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAdd_FormClosing);
            this.Load += new System.EventHandler(this.frmAdd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.TextBox tbDays;
        private System.Windows.Forms.Label lCountDay;
    }
}