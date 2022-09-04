using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace xPosRealiz
{
    public partial class FrmGoodsDT : Form
    {
        public FrmGoodsDT(DataTable[] dt)
        {
            if (dt == null || dt.Length < 2)
                this.Close();
            InitializeComponent();
            DgvTGoods.DataSource = dt[0];
            DgvGoodsVVO.DataSource = dt[1];
            this.MinimumSize = new Size(1024, 768);
        }

        public void ChengedTable(DataTable[] dt)
        {
            DoWork(() =>
            {
                DgvTGoods.DataSource = dt[0];
                DgvGoodsVVO.DataSource = dt[1];
                GC.Collect();
            });
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DoWork(MethodInvoker methodInvoker)
        {
            if (this.InvokeRequired)
                BeginInvoke(methodInvoker);
            else
                methodInvoker();
        }

        private void FrmGoodsDT_Resize(object sender, EventArgs e)
        {

        }

        private void DataTable_Changed(object sender, DataRowChangeEventArgs e) 
        {
            Update();
        }
    }
}
