namespace xPosRealiz_sprav_shues.dicGoods
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btSave = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.lGrp = new System.Windows.Forms.Label();
            this.lDeps = new System.Windows.Forms.Label();
            this.cmbTU = new System.Windows.Forms.ComboBox();
            this.cmbDeps = new System.Windows.Forms.ComboBox();
            this.lTypeMark = new System.Windows.Forms.Label();
            this.cmbTypeMark = new System.Windows.Forms.ComboBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tbEAN = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.cV = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cEan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Image = global::xPosRealiz.Properties.Resources.Save;
            this.btSave.Location = new System.Drawing.Point(341, 625);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(32, 32);
            this.btSave.TabIndex = 8;
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.BtSave_Click);
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.Image = global::xPosRealiz.Properties.Resources.exit;
            this.btClose.Location = new System.Drawing.Point(379, 625);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(32, 32);
            this.btClose.TabIndex = 9;
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.BtClose_Click);
            // 
            // lGrp
            // 
            this.lGrp.AutoSize = true;
            this.lGrp.Location = new System.Drawing.Point(10, 43);
            this.lGrp.Name = "lGrp";
            this.lGrp.Size = new System.Drawing.Size(22, 13);
            this.lGrp.TabIndex = 45;
            this.lGrp.Text = "ТУ";
            // 
            // lDeps
            // 
            this.lDeps.AutoSize = true;
            this.lDeps.Location = new System.Drawing.Point(10, 16);
            this.lDeps.Name = "lDeps";
            this.lDeps.Size = new System.Drawing.Size(38, 13);
            this.lDeps.TabIndex = 46;
            this.lDeps.Text = "Отдел";
            // 
            // cmbTU
            // 
            this.cmbTU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTU.FormattingEnabled = true;
            this.cmbTU.Location = new System.Drawing.Point(107, 39);
            this.cmbTU.Name = "cmbTU";
            this.cmbTU.Size = new System.Drawing.Size(304, 21);
            this.cmbTU.TabIndex = 43;
            this.cmbTU.SelectionChangeCommitted += new System.EventHandler(this.CmbTU_SelectionChangeCommitted);
            // 
            // cmbDeps
            // 
            this.cmbDeps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDeps.FormattingEnabled = true;
            this.cmbDeps.Location = new System.Drawing.Point(107, 12);
            this.cmbDeps.Name = "cmbDeps";
            this.cmbDeps.Size = new System.Drawing.Size(304, 21);
            this.cmbDeps.TabIndex = 44;
            this.cmbDeps.SelectionChangeCommitted += new System.EventHandler(this.CmbDeps_SelectionChangeCommitted);
            // 
            // lTypeMark
            // 
            this.lTypeMark.AutoSize = true;
            this.lTypeMark.Location = new System.Drawing.Point(10, 70);
            this.lTypeMark.Name = "lTypeMark";
            this.lTypeMark.Size = new System.Drawing.Size(91, 13);
            this.lTypeMark.TabIndex = 48;
            this.lTypeMark.Text = "Тип маркировки";
            // 
            // cmbTypeMark
            // 
            this.cmbTypeMark.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTypeMark.FormattingEnabled = true;
            this.cmbTypeMark.Location = new System.Drawing.Point(107, 66);
            this.cmbTypeMark.Name = "cmbTypeMark";
            this.cmbTypeMark.Size = new System.Drawing.Size(304, 21);
            this.cmbTypeMark.TabIndex = 47;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(16, 93);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(106, 17);
            this.radioButton1.TabIndex = 49;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Включать товар";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(298, 93);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(113, 17);
            this.radioButton2.TabIndex = 49;
            this.radioButton2.Text = "Исключать товар";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cV,
            this.cEan,
            this.cName});
            this.dataGridView1.Location = new System.Drawing.Point(12, 138);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(404, 481);
            this.dataGridView1.TabIndex = 50;
            // 
            // tbEAN
            // 
            this.tbEAN.Location = new System.Drawing.Point(42, 116);
            this.tbEAN.MaxLength = 13;
            this.tbEAN.Name = "tbEAN";
            this.tbEAN.Size = new System.Drawing.Size(119, 20);
            this.tbEAN.TabIndex = 51;
            this.tbEAN.TextChanged += new System.EventHandler(this.TbEAN_TextChanged);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(167, 116);
            this.tbName.MaxLength = 1024;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(229, 20);
            this.tbName.TabIndex = 51;
            this.tbName.TextChanged += new System.EventHandler(this.TbEAN_TextChanged);
            // 
            // cV
            // 
            this.cV.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.cV.DataPropertyName = "isSelect";
            this.cV.HeaderText = "V";
            this.cV.MinimumWidth = 30;
            this.cV.Name = "cV";
            this.cV.Width = 30;
            // 
            // cEan
            // 
            this.cEan.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.cEan.DataPropertyName = "ean";
            this.cEan.HeaderText = "EAN";
            this.cEan.MinimumWidth = 100;
            this.cEan.Name = "cEan";
            this.cEan.ReadOnly = true;
            this.cEan.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cEan.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cName
            // 
            this.cName.DataPropertyName = "nameGood";
            this.cName.HeaderText = "Наименование";
            this.cName.Name = "cName";
            this.cName.ReadOnly = true;
            // 
            // frmAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 669);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.tbEAN);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.lTypeMark);
            this.Controls.Add(this.cmbTypeMark);
            this.Controls.Add(this.lGrp);
            this.Controls.Add(this.lDeps);
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
            this.Text = "Товары для формирования марки";
            this.Load += new System.EventHandler(this.FrmAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Label lGrp;
        private System.Windows.Forms.Label lDeps;
        private System.Windows.Forms.ComboBox cmbTU;
        private System.Windows.Forms.ComboBox cmbDeps;
        private System.Windows.Forms.Label lTypeMark;
        private System.Windows.Forms.ComboBox cmbTypeMark;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox tbEAN;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cV;
        private System.Windows.Forms.DataGridViewTextBoxColumn cEan;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
    }
}