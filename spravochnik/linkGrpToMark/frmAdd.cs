using Nwuram.Framework.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using spravochnik;

namespace spravochnik.linkGrpToMark
{
    public partial class frmAdd : Form
    {
        public DataRowView row { set; private get; }

        private bool isEditData = false;
        private string oldName;
        private int id = 0, oldDays;
        public bool isSaveData = false;
        private DataTable dtGrp1;

        public frmAdd()
        {
            InitializeComponent();
            ToolTip tp = new ToolTip();
            tp.SetToolTip(btClose, "Выход");
            tp.SetToolTip(btSave, "Сохранить");
            cmbDeps.DataSource = Config.hCntMain.getDeps().Result;
            cmbDeps.ValueMember = "id";
            cmbDeps.DisplayMember = "cName";
            cmbDeps.SelectedIndex = -1;

            dtGrp1 = Config.hCntMain.getGrp().Result;

            cmbTypeMark.DataSource = Config.hCntMain.getTypeMarking().Result;
            cmbTypeMark.ValueMember = "id";
            cmbTypeMark.DisplayMember = "cName";
            cmbTypeMark.SelectedIndex = -1;
        }

        private void frmAdd_Load(object sender, EventArgs e)
        {
            if (row != null)
            {
                id = (int)row["id"];
                //oldName = tbName.Text.Trim();
                cmbDeps.SelectedValue = (int)row["id_otdel"];
                CmbDeps_SelectionChangeCommitted(null, null);
                cmbTU.SelectedValue = (int)row["id_grp1"];
                cmbTypeMark.SelectedValue = (int)row["id_TypeMarking"];
                checkBox1.Checked = (bool)row["is_CheckMarking"];
            }

            isEditData = false;
        }

        private void frmAdd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = isEditData && DialogResult.No == MessageBox.Show("На форме есть не сохранённые данные.\nЗакрыть форму без сохранения данных?\n", "Закрытие формы", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            //if (cmbDeps.SelectedIndex == -1)
            //{
            //    MessageBox.Show(Config.centralText($"Необходимо заполнить\n \"{lDeps.Text}\"\n"), "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    cmbDeps.Focus();
            //    return;
            //}

            if (cmbTU.SelectedIndex == -1)
            {
                MessageBox.Show(Config.centralText($"Необходимо заполнить\n \"{lGrp.Text}\"\n"), "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTU.Focus();
                return;
            }

            if (cmbTypeMark.SelectedIndex == -1)
            {
                MessageBox.Show(Config.centralText($"Необходимо заполнить\n \"{lTypeMark.Text}\"\n"), "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTypeMark.Focus();
                return;
            }

            Task<DataTable> task = Config.hCntMain.setGrp1VsTypeMarking(id, (int)cmbTU.SelectedValue, (int)cmbTypeMark.SelectedValue,checkBox1.Checked, 0, false);
            task.Wait();

            DataTable dtResult = task.Result;

            if (dtResult == null || dtResult.Rows.Count == 0)
            {
                MessageBox.Show("Не удалось сохранить данные", "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if ((int)dtResult.Rows[0]["id"] == -1)
            {
                //MessageBox.Show("В справочнике уже присутствует запись с таким наименованием.", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(Config.centralText($"{dtResult.Rows[0]["msg"].ToString().Replace("\\n", "\n")}"), "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if ((int)dtResult.Rows[0]["id"] == -9999)
            {
                MessageBox.Show($"{dtResult.Rows[0]["msg"]}", "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool isClose = false;
            if (id == 0)
            {
                id = (int)dtResult.Rows[0]["id"];
                Logging.StartFirstLevel((int)logEvents.Добавление_сервиса);
                Logging.Comment($"ID: {id}");
                //Logging.Comment($"{lName.Text}: {tbName.Text.Trim()}");
                //Logging.Comment($"{lCountDay.Text}: {tbDays.Text.Trim()}");
                Logging.StopFirstLevel();
                isSaveData = true;

            }
            else
            {
                Logging.StartFirstLevel((int)logEvents.Редактирование_сервиса);
                Logging.Comment($"ID: {id}");
                //Logging.VariableChange($"{lName.Text}", tbName.Text.Trim(), oldName);
                //Logging.VariableChange($"{lCountDay.Text}", tbDays.Text.Trim(), oldDays);
                Logging.StopFirstLevel();
                isClose = true;
            }

            isEditData = false;
            MessageBox.Show("Данные сохранены.", "Сохранение данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (isClose) this.DialogResult = DialogResult.OK; else ClearForm();
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            isEditData = true;
        }

        private void tbDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != '\b';
        }

        private void CmbDeps_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (dtGrp1 == null || dtGrp1.Rows.Count == 0) return;
            if (cmbDeps.SelectedIndex == -1)
            {
                dtGrp1.DefaultView.RowFilter = "id_otdel = 0";
                return;
            }
            int id_dep = (int)cmbDeps.SelectedValue;

            dtGrp1.DefaultView.RowFilter = $"id_otdel = {id_dep}";            
            if (cmbTU.DataSource == null)
            {
                cmbTU.DataSource = dtGrp1;
                cmbTU.DisplayMember = "cName";
                cmbTU.ValueMember = "id";
            }

            cmbTU.SelectedIndex = -1;
        }

        private void ClearForm()
        {
            //tbName.Text = "";
            //tbDays.Text = "";
            cmbTU.SelectedIndex = -1;
            cmbDeps.SelectedIndex = -1;
            cmbTypeMark.SelectedIndex = -1;
            CmbDeps_SelectionChangeCommitted(null, null);
            id = 0;
            isEditData = false;
        }
    }
}
