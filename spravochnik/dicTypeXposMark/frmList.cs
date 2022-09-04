using Nwuram.Framework.Logging;
using Nwuram.Framework.Settings.Connection;
using Nwuram.Framework.Settings.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace spravochnik.dicTypeXposMark
{
    public partial class frmList : Form
    {
        public bool isUpdate { private set; get; }
        private DataTable dtData;
        private int id_Firm;
        private string nameFirm;
        public bool isSelect { set; private get; }

        public string getNameFirm()
        {
            return nameFirm;
        }

        public int getIdFirm()
        {
            return id_Firm;
        }
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
            //btAdd.Visible = btEdit.Visible = btDelete.Visible = new List<string> { "ИНФ", "СОП" }.Contains(UserSettings.User.StatusCode);
        }

        private void frmList_Load(object sender, EventArgs e)
        {
            if (isSelect)
            {
                btEdit.Visible = btDelete.Visible = btAdd.Visible = false;
                label1.Visible = label2.Visible = false;
                tbFio.Visible = tbDate.Visible = false;
                chbNotActive.Visible = false;
                panel1.Visible = false;
                btSelect.Visible = true;
            }
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
                isUpdate = true;
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
                { get_data(); isUpdate = true; }
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null && dgvData.CurrentRow.Index != -1 && dtData != null && dtData.DefaultView.Count != 0)
            {
                Task<DataTable> task;
                int id = (int)dtData.DefaultView[dgvData.CurrentRow.Index]["id"];

                string cName = (string)dtData.DefaultView[dgvData.CurrentRow.Index]["cName"];
                int typeXposCode = (int)dtData.DefaultView[dgvData.CurrentRow.Index]["typeXposCode"];

                task = Config.hCntMain.setTypeMarking(id, cName, typeXposCode, 0, true);
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
                    isUpdate = true;
                    get_data();
                    return;
                }

                if (result == -2)
                {
                    if (DialogResult.Yes == MessageBox.Show("Удалить выбранную запись?", "Удаление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                    {
                        task = Config.hCntMain.setTypeMarking(id, cName, typeXposCode, 1, true);
                        task.Wait();
                        if (task.Result == null)
                        {
                            MessageBox.Show(Config.centralText("При сохранение данных возникли ошибки записи.\nОбратитесь в ОЭЭС\n"), "Сохранение данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        isUpdate = true;
                        get_data();
                    }
                    return;
                }
                
                if (result == 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("Удалить выбранную запись?", "Удаление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                    {
                        setLog(id, (int)logEvents.Удаление_сервиса);
                        task = Config.hCntMain.setTypeMarking(id, cName, typeXposCode, 1, true);
                        task.Wait();
                        if (task.Result == null)
                        {
                            MessageBox.Show(Config.centralText("При сохранение данных возникли ошибки записи.\nОбратитесь в ОЭЭС\n"), "Сохранение данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        isUpdate = true;
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

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            setFilter();
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

                if (tbName.Text.Trim().Length != 0)
                    filter += (filter.Length == 0 ? "" : " and ") + $"cName like '%{tbName.Text.Trim()}%'";

                if (tbXpostType.Text.Trim().Length != 0)
                    filter += (filter.Length == 0 ? "" : " and ") + $"CONVERT(typeXposCode,'System.String') like '%{tbXpostType.Text.Trim()}%'";

                //if (!chbNotActive.Checked)
                //   filter += (filter.Length == 0 ? "" : " and ") + $"isActive = 1";

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

                Task<DataTable> task = Config.hCntMain.getTypeMarking();
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

        private void dgvData_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            int width = 0;
            foreach (DataGridViewColumn col in dgvData.Columns)
            {
                if (!col.Visible) continue;

                if (col.Name.Equals(cName.Name))
                {
                    tbName.Location = new Point(dgvData.Location.X + 1 + width, tbName.Location.Y);
                    tbName.Size = new Size(col.Width, tbName.Height);
                }

                if (col.Name.Equals(cTypeXposCode.Name))
                {
                    tbXpostType.Location = new Point(dgvData.Location.X + 1 + width, tbName.Location.Y);
                    tbXpostType.Size = new Size(col.Width, tbName.Height);
                }

                width += col.Width;

            }
        }

        private void btAlarmUpdatre_Click(object sender, EventArgs e)
        {
            get_data();
        }

        private void btSelect_Click(object sender, EventArgs e)
        {
            DataRowView row = dtData.DefaultView[dgvData.CurrentRow.Index];

            id_Firm = (int)row["id"];
            nameFirm = (string)row["cName"];
            this.DialogResult = DialogResult.OK;
        }

        private void dgvData_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!isSelect) return;
            if (e.RowIndex == -1) return;

            btSelect_Click(null, null);
        }
    }
}