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

namespace spravochnik.dicTypeXposMark
{
    public partial class frmAdd : Form
    {
        public DataRowView row { set; private get; }

        private bool isEditData = false;
        private string oldName;
        private int id = 0, oldDays;
        public bool isSaveData = false;

        public frmAdd()
        {
            InitializeComponent();
            ToolTip tp = new ToolTip();
            tp.SetToolTip(btClose, "Выход");
            tp.SetToolTip(btSave, "Сохранить");
        }

        private void frmAdd_Load(object sender, EventArgs e)
        {
            if (row != null)
            {
                id = (int)row["id"];
                tbName.Text = (string)row["cName"];
                oldName = tbName.Text.Trim();

                oldDays = (int)row["typeXposCode"];
                tbDays.Text = oldDays.ToString();
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
            if (tbName.Text.Trim().Length == 0)
            {
                MessageBox.Show(Config.centralText($"Необходимо заполнить\n \"{lName.Text}\"\n"), "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbName.Focus();
                return;
            }

            if (tbDays.Text.Trim().Length == 0)
            {
                MessageBox.Show(Config.centralText($"Необходимо заполнить\n \"{lCountDay.Text}\"\n"), "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbDays.Focus();
                return;
            }

            int Days;
            if (!int.TryParse(tbDays.Text, out Days))
            {
                MessageBox.Show(Config.centralText($"Необходимо заполнить\n \"{lCountDay.Text}\"\n"), "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbDays.Focus();
                return;
            }


            Task<DataTable> task = Config.hCntMain.setTypeMarking(id, tbName.Text, Days, 0, false);
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
                Logging.Comment($"{lName.Text}: {tbName.Text.Trim()}");
                Logging.Comment($"{lCountDay.Text}: {tbDays.Text.Trim()}");
                Logging.StopFirstLevel();
                isSaveData = true;

            }
            else
            {
                Logging.StartFirstLevel((int)logEvents.Редактирование_сервиса);
                Logging.Comment($"ID: {id}");
                Logging.VariableChange($"{lName.Text}", tbName.Text.Trim(), oldName);
                Logging.VariableChange($"{lCountDay.Text}", tbDays.Text.Trim(), oldDays);
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

        private void ClearForm()
        {
            tbName.Text = "";
            tbDays.Text = "";
            id = 0;
            isEditData = false;
        }
    }
}
