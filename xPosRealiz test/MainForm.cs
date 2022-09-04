using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using xPosBL;
using xPosBL.Timer;
using xPosRealiz_sprav_shues;

namespace xPosRealiz
{
    public partial class MainForm : Form
    {
        private const string chbWorkCashRegisterText = @"Ручная загрузка полного справочника на кассу может занимать до 40 минут.

Чекбокс ""рабочая касса"" - включен
Для кассы которая торгует и её нельзя останавливать, для массового обновления товаров, необходимо использовать режим рабочая касса.
Он выгрузит обновленный список товаров для кассы.И удалит товары отправленные в резерв.
НО! В этом режиме кассовая программа (xPOS) может не суметь заменить одну (или несколько) из записей товаров в локальной БД xPOS
и не загрузит её (оставив старую запись). Что приведет к тому, что цена, фирма, или другие данные по товару будут неправильные.

Чекбокс ""рабочая касса"" - выключен
ВНИМАНИЕ !!!  Полностью очищает БД товаров кассы при начале загрузки.
Для кассы которую остановили для профилактики можно использовать режим полной очистки базы товаров, перед загрузкой обновлённого списка товаров.
В этом режиме в локальной БД xPOS не останется записей, и при добавлении товаров, не произойдёт конфликтов.Все товары загрузятся полностью.
Если по какому то товару будет присутствовать ошибки и касса не сможет загрузить его, то можно впоследствии добавить товар, полным справочником в режиме ""рабочая касса"".";
        private const string btnCreateText = @"Полный справочник выгружается в дочернюю для папки программы папку ""sprav"".
Для отдела ВВО(Вино и спиртные напитки) - название FULL_vvo
Для прочих отделов - название FULL_notVVO";

        private XPos _xPos;
        private FrmGoodsDT _frmGoods;

        public MainForm()
        {
            InitializeComponent();
        }

        private void xPos_ChangedTerminals(object sender, EventArgs e)
        {
            if (_frmGoods != null)
            {
                _frmGoods.ChengedTable(new[] { _xPos.DataGoodsUpdates, _xPos.DataGoodsUpdatesVvo });
                GC.Collect();
            }
        }

        public void MainForm_Load(object sender, EventArgs e)
        {
            _xPos = new XPos();
            dgvGoods.AutoGenerateColumns = false;
            _xPos.EventTimerTick += TimerTick;
            _xPos.EventNotification += Logging;
            _xPos.StartLoadData += StartLoadData;
            _xPos.StopLoadData += StopingLoadData;
            _xPos.ChangedTerminals += xPos_ChangedTerminals;
            _xPos.ChangedDataGoodsUpdates += xPos_ChangedTerminals;
            _xPos.SubEventsTimer(ButtonVisible, ButtonVisible);
            _xPos.ChangedTerminal += XPos_NewTerminals;
            _xPos.TimerStart();
            _xPos.SubCreateEndSpr(XPos_StartCreateFullSprav, XPos_EndCreateFullSprav);

            dgvGoods.DataSource = _xPos.Terminals;
            cmbTerminalType.DisplayMember = "Name";
            cmbTerminalType.ValueMember = "Id";
            cmbTerminalType.DataSource = _xPos.GetTerminalType();
            tbDelay.Text = _xPos.GetDefaultTimer().ToString();
            chbWorkCashRegister.Checked = true;

            toolTip1.SetToolTip(btSetting, "Настройки");
            toolTip1.SetToolTip(chbWorkCashRegister, chbWorkCashRegisterText);
            toolTip1.SetToolTip(btnCreate, btnCreateText);
            tbDelay.ShortcutsEnabled = true;
        }

        private void ButtonVisible(object sender, EventArgs e)
        {
            DoWork(new MethodInvoker(() =>
            {
                TimerTest tt = (TimerTest)sender;
                btStartTimer.Enabled = !tt.IsWorking;
                btStopTimer.Enabled = tt.IsWorking;
            }));
        }
        
        private void TimerTick(object sender, string mes)
        {
            DoWork(new MethodInvoker(() =>
            {
                lTimeTickUpdate.Text = mes;
            }));
        }

        private void btStopTimer_Click(object sender, EventArgs e)
        {
            _xPos.TimerStop();
        }

        private void btStartTimer_Click(object sender, EventArgs e)
        {
            _xPos.TimerResum();
        }
        
        private void tbDelay_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Modified)
            {
                tb.BackColor = Color.Yellow;
            }
        }

        private void btSetDelay_Click(object sender, EventArgs e)
        {
            int seconds = Convert.ToInt32(tbDelay.Text);
            _xPos.SetDefaultTimer(seconds);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            _xPos.CreateFullCatalogGoods(chbWorkCashRegister.Checked);
        }

        private void cmbTerminalType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _xPos.FindTerminalsType(Convert.ToInt32(cmbTerminalType.SelectedValue));
        }

        private void dgvGoods_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataTable ter = (DataTable)dgvGoods.DataSource;
            Color rColor = Color.White;
            try
            {
                if (ter != null && ter.Rows.Count > 0 && ter.DefaultView.Count > 0)
                {
                    DataRowView row = ter.DefaultView[e.RowIndex];
                    if (row["id_gu"] != DBNull.Value && row["last_id_gu"] != DBNull.Value)
                        if ((int)row["id_gu"] != (int)row["last_id_gu"] && (bool)row["isSelect"])
                            rColor = panel2.BackColor;
                        else if((int)row["id_gu"] != (int)row["last_id_gu"])
                            rColor = Color.LightGray;
                }
            }
            catch { }

            dgvGoods.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvGoods.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                dgvGoods.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = rColor;
        }

        private void dgvGoods_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            //Рисуем рамку для выделеной строки
            if (dgv.Rows[e.RowIndex].Selected)
            {
                int width = dgv.Width;
                Rectangle r = dgv.GetRowDisplayRectangle(e.RowIndex, false);
                Rectangle rect = new Rectangle(r.X, r.Y, width - 1, r.Height - 1);

                ControlPaint.DrawBorder(e.Graphics, rect,
                    SystemColors.Highlight, 2, ButtonBorderStyle.Solid,
                    SystemColors.Highlight, 2, ButtonBorderStyle.Solid,
                    SystemColors.Highlight, 2, ButtonBorderStyle.Solid,
                    SystemColors.Highlight, 2, ButtonBorderStyle.Solid);
            }
        }

        private void tbDelay_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != '\b';
        }

        private void tbDelay_Leave(object sender, EventArgs e)
        {
            if (!tbDelay.Enabled) return;
            try
            {
                int tmp;
                if (int.TryParse(tbDelay.Text, out tmp))
                {
                    if (10 <= tmp && tmp <= 600)
                        tbDelay.Text = tmp.ToString();
                    else
                        tbDelay.Text = tmp > 600 ? "600" : "10";
                }
                else
                    tbDelay.Text = "10";
            }
            catch
            {
                tbDelay.Text = "10";
            }
        }

        private void btSetting_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings(_xPos.SettingXpos, _xPos.SettingsSprav);
            settings.Show();
        }
        
        private void Logging(object sender, string message)
        {
            DoWork(() =>
            {
                string time = "[" + DateTime.Now.ToString("HH:mm:ss") + "]";
                tbResultLog.Text = time + " " + message + Environment.NewLine + tbResultLog.Text;
            });
        }

        private void cmbTerminalType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _xPos.FindTerminalsType(Convert.ToInt32(cmbTerminalType.SelectedValue));
        }

        private void StartLoadData(object s, EventArgs e)
        {
            DoWork(() =>
            {
                btStartTimer.Enabled = false;
                btStopTimer.Enabled = false;
                btTGoodsView.Enabled = false;
            });
        }

        private void StopingLoadData(object s, EventArgs e)
        {
            DoWork(() =>
            {
                btStopTimer.Enabled = true;
                btTGoodsView.Enabled = true;
            });
        }

        private void DoWork(MethodInvoker methodInvoker)
        {
            if (this.InvokeRequired) 
                BeginInvoke(methodInvoker);  
            else  
                methodInvoker(); 
        }

        private void btTGoodsView_Click(object sender, EventArgs e)
        {
            if (_frmGoods == null || _frmGoods.IsDisposed)
                _frmGoods = new FrmGoodsDT(new[] { _xPos.DataGoodsUpdates, _xPos.DataGoodsUpdatesVvo });
            _frmGoods.Show();
        }

        private void dgvGoods_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int colIndex = e.ColumnIndex;
            if(dgvGoods.Columns[colIndex].Name == "cV" && e.RowIndex != -1)
            {
                int numberTerm = Convert.ToInt32(dgvGoods.Rows[e.RowIndex].Cells["cNum"].Value);
                _xPos.ChangedChoiceTerminal(numberTerm);
            }
            else if (dgvGoods.Columns[colIndex].Name == "cV" && e.RowIndex == -1)
            {
                _xPos.ChangedAllTermChoice();
            }
        }

        private void XPos_NewTerminals(object sender, EventArgs e)
        {
            DoWork(() =>
            {
                DataTable terms = (DataTable)sender;
                dgvGoods.DataSource = terms;
            });
        }

        private void XPos_StartCreateFullSprav(object sender, EventArgs e)
        {
            DoWork(() => { btnCreate.Enabled = false; });
        }

        private void XPos_EndCreateFullSprav(object sender, EventArgs e)
        {
            DoWork(() => btnCreate.Enabled = true);
        }
    }
}
