
namespace xPosRealiz
{
    partial class FrmGoodsDT
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
            this.DgvTGoods = new System.Windows.Forms.DataGridView();
            this.btClose = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.DgvGoodsVVO = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.DgvTGoods)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvGoodsVVO)).BeginInit();
            this.SuspendLayout();
            // 
            // DgvTGoods
            // 
            this.DgvTGoods.AllowUserToAddRows = false;
            this.DgvTGoods.AllowUserToDeleteRows = false;
            this.DgvTGoods.AllowUserToResizeRows = false;
            this.DgvTGoods.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvTGoods.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DgvTGoods.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.DgvTGoods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvTGoods.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.DgvTGoods.Location = new System.Drawing.Point(0, 0);
            this.DgvTGoods.MultiSelect = false;
            this.DgvTGoods.Name = "DgvTGoods";
            this.DgvTGoods.ReadOnly = true;
            this.DgvTGoods.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.DgvTGoods.RowHeadersVisible = false;
            this.DgvTGoods.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvTGoods.Size = new System.Drawing.Size(976, 645);
            this.DgvTGoods.TabIndex = 0;
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.Location = new System.Drawing.Point(921, 689);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 23);
            this.btClose.TabIndex = 1;
            this.btClose.Text = "Закрыть";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(984, 671);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.DgvTGoods);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(976, 645);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Продукты";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.DgvGoodsVVO);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(515, 357);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Прдукты ВВО";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // DgvGoodsVVO
            // 
            this.DgvGoodsVVO.AllowUserToAddRows = false;
            this.DgvGoodsVVO.AllowUserToDeleteRows = false;
            this.DgvGoodsVVO.AllowUserToResizeRows = false;
            this.DgvGoodsVVO.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvGoodsVVO.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.DgvGoodsVVO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvGoodsVVO.GridColor = System.Drawing.SystemColors.ScrollBar;
            this.DgvGoodsVVO.Location = new System.Drawing.Point(0, 0);
            this.DgvGoodsVVO.MultiSelect = false;
            this.DgvGoodsVVO.Name = "DgvGoodsVVO";
            this.DgvGoodsVVO.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvGoodsVVO.Size = new System.Drawing.Size(515, 357);
            this.DgvGoodsVVO.TabIndex = 1;
            // 
            // FrmGoodsDT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btClose);
            this.Name = "FrmGoodsDT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmGoodsDT";
            this.Resize += new System.EventHandler(this.FrmGoodsDT_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.DgvTGoods)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvGoodsVVO)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DgvTGoods;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView DgvGoodsVVO;
    }
}