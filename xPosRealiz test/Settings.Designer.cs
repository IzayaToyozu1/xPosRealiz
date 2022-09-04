
namespace xPosRealiz
{
    partial class Settings
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.tbFileNameSprav = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tbFilenameFullSprav = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btSave = new System.Windows.Forms.Button();
            this.cbMKOandGST = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbLimitCheckingFile = new System.Windows.Forms.TextBox();
            this.btClose = new System.Windows.Forms.Button();
            this.btSaveAndClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "Наименнование файла справочника товара";
            // 
            // tbFileNameSprav
            // 
            this.tbFileNameSprav.Location = new System.Drawing.Point(148, 12);
            this.tbFileNameSprav.Name = "tbFileNameSprav";
            this.tbFileNameSprav.Size = new System.Drawing.Size(100, 20);
            this.tbFileNameSprav.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // tbFilenameFullSprav
            // 
            this.tbFilenameFullSprav.Location = new System.Drawing.Point(148, 63);
            this.tbFilenameFullSprav.Name = "tbFilenameFullSprav";
            this.tbFilenameFullSprav.Size = new System.Drawing.Size(100, 20);
            this.tbFilenameFullSprav.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 38);
            this.label2.TabIndex = 3;
            this.label2.Text = "Наименнование файла полного справочника";
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(100, 181);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 39);
            this.btSave.TabIndex = 6;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // cbMKOandGST
            // 
            this.cbMKOandGST.AutoSize = true;
            this.cbMKOandGST.Location = new System.Drawing.Point(184, 101);
            this.cbMKOandGST.Name = "cbMKOandGST";
            this.cbMKOandGST.Size = new System.Drawing.Size(15, 14);
            this.cbMKOandGST.TabIndex = 7;
            this.cbMKOandGST.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 19);
            this.label3.TabIndex = 8;
            this.label3.Text = "Только МКО и ГСТ";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 45);
            this.label4.TabIndex = 9;
            this.label4.Text = "Максимальное число проверок файла справочника";
            // 
            // tbLimitCheckingFile
            // 
            this.tbLimitCheckingFile.Location = new System.Drawing.Point(148, 130);
            this.tbLimitCheckingFile.Name = "tbLimitCheckingFile";
            this.tbLimitCheckingFile.Size = new System.Drawing.Size(100, 20);
            this.tbLimitCheckingFile.TabIndex = 10;
            this.tbLimitCheckingFile.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbLimitCheckingFile_KeyPress);
            // 
            // btClose
            // 
            this.btClose.Location = new System.Drawing.Point(186, 181);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 39);
            this.btClose.TabIndex = 11;
            this.btClose.Text = "Закрыть";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // btSaveAndClose
            // 
            this.btSaveAndClose.Location = new System.Drawing.Point(15, 181);
            this.btSaveAndClose.Name = "btSaveAndClose";
            this.btSaveAndClose.Size = new System.Drawing.Size(75, 39);
            this.btSaveAndClose.TabIndex = 12;
            this.btSaveAndClose.Text = "Сохранить и выйти";
            this.btSaveAndClose.UseVisualStyleBackColor = true;
            this.btSaveAndClose.Click += new System.EventHandler(this.btSaveAndClose_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 232);
            this.Controls.Add(this.btSaveAndClose);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbLimitCheckingFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbMKOandGST);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbFilenameFullSprav);
            this.Controls.Add(this.tbFileNameSprav);
            this.Controls.Add(this.label1);
            this.Name = "Settings";
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbFileNameSprav;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox tbFilenameFullSprav;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.CheckBox cbMKOandGST;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbLimitCheckingFile;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Button btSaveAndClose;
    }
}