using System;
using System.Windows.Forms;
using xPosBL;
using xPosBL.GoodsDirectories;

namespace xPosRealiz
{
    public partial class Settings : Form
    {
        SettingXPOS _sXPOS;
        SettingsSprav _sSprav;

        public Settings(SettingXPOS sXPOS, SettingsSprav sSprav)
        {
            InitializeComponent();
            _sXPOS = sXPOS;
            _sSprav = sSprav;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            tbFileNameSprav.Text = _sXPOS.FileNameSprav;
            tbFilenameFullSprav.Text = _sXPOS.FileNameFullSprav;
            cbMKOandGST.Checked = _sSprav.MKOandGST;
            tbLimitCheckingFile.Text = _sXPOS.LimitCheckingFile.ToString();
        }

        private void tbLimitCheckingFile_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar))
            {
                return;
            }
            e.Handled = true;
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            _sXPOS.FileNameSprav = tbFileNameSprav.Text;
            _sXPOS.FileNameFullSprav = tbFilenameFullSprav.Text;
            _sSprav.MKOandGST = cbMKOandGST.Checked;
            _sXPOS.LimitCheckingFile = Convert.ToInt32(tbLimitCheckingFile.Text);
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btSaveAndClose_Click(object sender, EventArgs e)
        {
            btSave_Click(sender, e);
            Close();
        }
    }
}
