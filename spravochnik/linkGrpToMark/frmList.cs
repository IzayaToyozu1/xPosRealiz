using Nwuram.Framework.Logging;
using Nwuram.Framework.Settings.Connection;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using spravochnik;

namespace spravochnik.linkGrpToMark
{
    public partial class frmList : Form
    {
        private DataTable dtData, dtGrp1;

        public frmList()
        {
            InitializeComponent();

            if (Config.hCntMain == null)
                Config.hCntMain = new Procedures(ConnectionSettings.GetServer(), ConnectionSettings.GetDatabase(), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.ProgramName);

            if (Config.hCntSecond == null)
                Config.hCntSecond = new Procedures(ConnectionSettings.GetServer("3"), ConnectionSettings.GetDatabase("3"), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.ProgramName);

            dgvData.AutoGenerateColumns = false;

            ToolTip tp = new ToolTip();
            tp.SetToolTip(btAdd, "Добавить");
            tp.SetToolTip(btEdit, "Редактировать");
            tp.SetToolTip(btDelete, "Удалить");
            tp.SetToolTip(btClose, "Выход");
            tp.SetToolTip(btAlarmUpdatre, "Обновить");
            tp.SetToolTip(btSelect, "Выбрать");
            dtGrp1 = Config.hCntMain.getGrp(true).Result;
            cmbDeps.DataSource = Config.hCntMain.getDeps(true).Result;
            cmbDeps.ValueMember = "id";
            cmbDeps.DisplayMember = "cName";
            CmbDeps_SelectionChangeCommitted(null, null);
            

            cmbTypeMark.DataSource = Config.hCntMain.getTypeMarking(true).Result;
            cmbTypeMark.ValueMember = "id";
            cmbTypeMark.DisplayMember = "cName";            
        }

        private void frmList_Load(object sender, EventArgs e)
        {           
            get_data();
        }

        private void frmList_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            frmAdd fAdd = new frmAdd() { Text = "Добавить тип" };
            fAdd.ShowDialog();
            if (fAdd.isSaveData)
            {
                get_data();
            }
            //if (DialogResult.OK == new frmAdd() { Text = "Добавить канал" }.ShowDialog())
            //{ get_data(); isUpdate = true; }
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null && dgvData.CurrentRow.Index != -1 && dtData != null && dtData.DefaultView.Count != 0)
            {
                DataRowView row = dtData.DefaultView[dgvData.CurrentRow.Index];
                if (DialogResult.OK == new frmAdd() { Text = "Редактировать тип", row = row }.ShowDialog())
                { get_data(); }
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null && dgvData.CurrentRow.Index != -1 && dtData != null && dtData.DefaultView.Count != 0)
            {
                Task<DataTable> task;
                int id = (int)dtData.DefaultView[dgvData.CurrentRow.Index]["id"];

                int id_grp1 = (int)dtData.DefaultView[dgvData.CurrentRow.Index]["id_grp1"];
                int id_TypeMarking = (int)dtData.DefaultView[dgvData.CurrentRow.Index]["id_TypeMarking"];
                bool is_CheckMarking = (bool)dtData.DefaultView[dgvData.CurrentRow.Index]["is_CheckMarking"];
                

                task = Config.hCntMain.setGrp1VsTypeMarking(id, id_grp1, id_TypeMarking, is_CheckMarking, 0, true);
                task.Wait();

                if (task.Result == null)
                {
                    MessageBox.Show(Config.centralText("При сохранение данных возникли ошибки записи.\nОбратитесь в ОЭЭС\n"), "Сохранение данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int result = (int)task.Result.Rows[0]["id"];

                if (result == -1)
                {
                    MessageBox.Show(Config.centralText("Запись уже удалена другим пользователем\n"), "Удаление записи", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    get_data();
                    return;
                }
               
                if (result == 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("Удалить выбранную запись?", "Удаление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                    {
                        setLog(id, (int)logEvents.Удаление_сервиса);
                        task = Config.hCntMain.setGrp1VsTypeMarking(id, id_grp1, id_TypeMarking, is_CheckMarking, 1, true);
                        task.Wait();
                        if (task.Result == null)
                        {
                            MessageBox.Show(Config.centralText("При сохранение данных возникли ошибки записи.\nОбратитесь в ОЭЭС\n"), "Сохранение данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        get_data();
                        return;
                    }
                }
              
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
      

        private void chbNotActive_CheckedChanged(object sender, EventArgs e)
        {
            setFilter();
        }

        private void setFilter()
        {
            if (dtData == null || dtData.Rows.Count == 0)
            {
                btEdit.Enabled = btDelete.Enabled = false;
                return;
            }

            try
            {
                string filter = "";

                if((int)cmbDeps.SelectedValue !=0)
                      filter += (filter.Length == 0 ? "" : " and ") + $"id_otdel = {cmbDeps.SelectedValue}";

                if ((int)cmbTU.SelectedValue != 0 )
                    filter += (filter.Length == 0 ? "" : " and ") + $"id_grp1 = {cmbTU.SelectedValue}";

                if ((int)cmbTypeMark.SelectedValue != 0)
                    filter += (filter.Length == 0 ? "" : " and ") + $"id_TypeMarking = {cmbTypeMark.SelectedValue}";

                dtData.DefaultView.RowFilter = filter;
            }
            catch
            {
                dtData.DefaultView.RowFilter = "id = -1";
            }
            finally
            {
                btEdit.Enabled = btDelete.Enabled =
                dtData.DefaultView.Count != 0;
                dgvData_SelectionChanged(null, null);
            }
        }

        private void dgvData_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow == null || dgvData.CurrentRow.Index == -1 || dtData == null || dtData.DefaultView.Count == 0 || dgvData.CurrentRow.Index >= dtData.DefaultView.Count)
            {
                //btDelete.Enabled = false;
                //btEdit.Enabled = false;

                tbFio.Text = tbDate.Text = "";
                return;
            }

            btDelete.Enabled = true;
           // btEdit.Enabled = (bool)dtData.DefaultView[dgvData.CurrentRow.Index]["isActive"];

           // tbFio.Text = dtData.DefaultView[dgvData.CurrentRow.Index]["FIO"].ToString();
           // tbDate.Text = dtData.DefaultView[dgvData.CurrentRow.Index]["DateEdit"].ToString();
        }

        private void dgvData_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
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

        private void dgvData_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex != -1 && dtData != null && dtData.DefaultView.Count != 0)
            {
                Color rColor = Color.White;
              //  if (!(bool)dtData.DefaultView[e.RowIndex]["isActive"])
              //      rColor = panel1.BackColor;
                dgvData.Rows[e.RowIndex].DefaultCellStyle.BackColor = rColor;
                dgvData.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = rColor;
                dgvData.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black;

            }
        }

        private void get_data()
        {
            Task.Run(() =>
            {
                Config.DoOnUIThread(() => { this.Enabled = false; }, this);

                Task<DataTable> task = Config.hCntMain.getGrp1VsTypeMarking();
                task.Wait();
                dtData = task.Result;

                Config.DoOnUIThread(() =>
                {
                    DataGridViewColumn oldCol = dgvData.SortedColumn;
                    ListSortDirection direction = ListSortDirection.Ascending;
                    if (oldCol != null)
                    {
                        if (dgvData.SortOrder == System.Windows.Forms.SortOrder.Ascending)
                        {
                            direction = ListSortDirection.Ascending;
                        }
                        else
                        {
                            direction = ListSortDirection.Descending;
                        }
                    }
                    setFilter();
                    dgvData.DataSource = dtData;


                    if (oldCol != null)
                    {
                        dgvData.Sort(oldCol, direction);
                        oldCol.HeaderCell.SortGlyphDirection =
                            direction == ListSortDirection.Ascending ?
                            System.Windows.Forms.SortOrder.Ascending : System.Windows.Forms.SortOrder.Descending;
                    }

                }, this);

                Config.DoOnUIThread(() => { this.Enabled = true; }, this);
            });
        }

        private void setLog(int id, int id_log)
        {
            return;
            Logging.StartFirstLevel(id_log);
            switch (id_log)
            {
                case (int)logEvents.Удаление_сервиса: Logging.Comment("Удаление сервиса"); break;
                case 1542: Logging.Comment("Сервис переведён в недействующие "); break;
                case 1543: Logging.Comment("Сервис переведён  в действующие"); break;
                default: break;
            }

            string cName = (string)dtData.DefaultView[dgvData.CurrentRow.Index]["cName"];
            string Days = dtData.DefaultView[dgvData.CurrentRow.Index]["typeXposCode"].ToString();

            Logging.Comment($"ID:{id}");
            Logging.Comment($"Наименование: {cName}");
            Logging.Comment($"Количество дней предупреждения: {Days}");

            Logging.StopFirstLevel();
        }
           
        private void btAlarmUpdatre_Click(object sender, EventArgs e)
        {
            get_data();
        }

        private void btSelect_Click(object sender, EventArgs e)
        {
            DataRowView row = dtData.DefaultView[dgvData.CurrentRow.Index];

            this.DialogResult = DialogResult.OK;
        }

        private void dgvData_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {            
            if (e.RowIndex == -1) return;

            //btSelect_Click(null, null);
        }

        private void CmbTU_SelectionChangeCommitted(object sender, EventArgs e)
        {
            setFilter();
        }

        private void CmbTypeMark_SelectionChangeCommitted(object sender, EventArgs e)
        {
            setFilter();
        }

        private void CmbDeps_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbDeps.SelectedIndex == -1) return;
            if (dtGrp1 == null || dtGrp1.Rows.Count == 0) return;
            int id_dep = (int)cmbDeps.SelectedValue;

            dtGrp1.DefaultView.RowFilter = $"id_otdel = {id_dep} or id = 0";
            if (cmbTU.DataSource == null)
            {
                cmbTU.DataSource = dtGrp1;
                cmbTU.DisplayMember = "cName";
                cmbTU.ValueMember = "id";
            }
            setFilter();
        }
    }
}