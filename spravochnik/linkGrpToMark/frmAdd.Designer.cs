namespace spravochnik.linkGrpToMark
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
            this.btSave = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.lTypeMark = new System.Windows.Forms.Label();
            this.lGrp = new System.Windows.Forms.Label();
            this.lDeps = new System.Windows.Forms.Label();
            this.cmbTypeMark = new System.Windows.Forms.ComboBox();
            this.cmbTU = new System.Windows.Forms.ComboBox();
            this.cmbDeps = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Image = global::spravochnik.Properties.Resources.Save;
            this.btSave.Location = new System.Drawing.Point(254, 137);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(32, 32);
            this.btSave.TabIndex = 6;
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.Image = global::spravochnik.Properties.Resources.Exit;
            this.btClose.Location = new System.Drawing.Point(292, 137);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(32, 32);
            this.btClose.TabIndex = 7;
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // lTypeMark
            // 
            this.lTypeMark.AutoSize = true;
            this.lTypeMark.Location = new System.Drawing.Point(6, 70);
            this.lTypeMark.Name = "lTypeMark";
            this.lTypeMark.Size = new System.Drawing.Size(91, 13);
            this.lTypeMark.TabIndex = 40;
            this.lTypeMark.Text = "Тип маркировки";
            // 
            // lGrp
            // 
            this.lGrp.AutoSize = true;
            this.lGrp.Location = new System.Drawing.Point(6, 43);
            this.lGrp.Name = "lGrp";
            this.lGrp.Size = new System.Drawing.Size(22, 13);
            this.lGrp.TabIndex = 41;
            this.lGrp.Text = "ТУ";
            // 
            // lDeps
            // 
            this.lDeps.AutoSize = true;
            this.lDeps.Location = new System.Drawing.Point(6, 16);
            this.lDeps.Name = "lDeps";
            this.lDeps.Size = new System.Drawing.Size(38, 13);
            this.lDeps.TabIndex = 42;
            this.lDeps.Text = "Отдел";
            // 
            // cmbTypeMark
            // 
            this.cmbTypeMark.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTypeMark.FormattingEnabled = true;
            this.cmbTypeMark.Location = new System.Drawing.Point(103, 66);
            this.cmbTypeMark.Name = "cmbTypeMark";
            this.cmbTypeMark.Size = new System.Drawing.Size(211, 21);
            this.cmbTypeMark.TabIndex = 37;
            // 
            // cmbTU
            // 
            this.cmbTU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTU.FormattingEnabled = true;
            this.cmbTU.Location = new System.Drawing.Point(103, 39);
            this.cmbTU.Name = "cmbTU";
            this.cmbTU.Size = new System.Drawing.Size(211, 21);
            this.cmbTU.TabIndex = 38;
            // 
            // cmbDeps
            // 
            this.cmbDeps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDeps.FormattingEnabled = true;
            this.cmbDeps.Location = new System.Drawing.Point(103, 12);
            this.cmbDeps.Name = "cmbDeps";
            this.cmbDeps.Size = new System.Drawing.Size(211, 21);
            this.cmbDeps.TabIndex = 39;
            this.cmbDeps.SelectionChangeCommitted += new System.EventHandler(this.CmbDeps_SelectionChangeCommitted);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(9, 93);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox1.Size = new System.Drawing.Size(156, 17);
            this.checkBox1.TabIndex = 43;
            this.checkBox1.Text = "Без ввода ШК марировки";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // frmAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 181);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.lTypeMark);
            this.Controls.Add(this.lGrp);
            this.Controls.Add(this.lDeps);
            this.Controls.Add(this.cmbTypeMark);
            this.Controls.Add(this.cmbTU);
            this.Controls.Add(this.cmbDeps);
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
        private System.Windows.Forms.Label lTypeMark;
        private System.Windows.Forms.Label lGrp;
        private System.Windows.Forms.Label lDeps;
        private System.Windows.Forms.ComboBox cmbTypeMark;
        private System.Windows.Forms.ComboBox cmbTU;
        private System.Windows.Forms.ComboBox cmbDeps;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}