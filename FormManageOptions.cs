using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CodeQuest.Factories;
using CodeQuest.Services;
using CodeQuest.Models;

namespace CodeQuest
{
    public partial class FormManageOptions : Form
    {
        private readonly IGameService gameService;
        private readonly int questionId;
        private readonly string questionText;
        private DataGridView dgvChoices;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnClose;

        public FormManageOptions(int questionId, string questionText)
        {
            this.questionId = questionId;
            this.questionText = questionText;
            gameService = ServiceFactory.GetGameService();
            InitializeComponent();
            LoadChoices();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = $"Opciones - Q#{questionId}";
            this.Size = new Size(600, 420);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(250, 250, 250);

            dgvChoices = new DataGridView();
            dgvChoices.Location = new Point(12, 12);
            dgvChoices.Size = new Size(560, 280);
            dgvChoices.ReadOnly = false;
            dgvChoices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChoices.AllowUserToAddRows = false;
            dgvChoices.AllowUserToDeleteRows = false;
            dgvChoices.AutoGenerateColumns = true;
            dgvChoices.BackgroundColor = Color.White;
            dgvChoices.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvChoices.CellDoubleClick += DgvChoices_CellDoubleClick;
            dgvChoices.CurrentCellDirtyStateChanged += DgvChoices_CurrentCellDirtyStateChanged;
            dgvChoices.CellValueChanged += DgvChoices_CellValueChanged;
            this.Controls.Add(dgvChoices);

            btnAdd = new Button();
            btnAdd.Text = "Agregar Opción";
            btnAdd.Size = new Size(130, 34);
            btnAdd.Location = new Point(12, 305);
            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);

            btnEdit = new Button();
            btnEdit.Text = "Editar Opción";
            btnEdit.Size = new Size(130, 34);
            btnEdit.Location = new Point(160, 305);
            btnEdit.Click += BtnEdit_Click;
            this.Controls.Add(btnEdit);

            btnDelete = new Button();
            btnDelete.Text = "Eliminar Opción";
            btnDelete.Size = new Size(130, 34);
            btnDelete.Location = new Point(308, 305);
            btnDelete.Click += BtnDelete_Click;
            this.Controls.Add(btnDelete);

            btnClose = new Button();
            btnClose.Text = "Cerrar";
            btnClose.Size = new Size(100, 34);
            btnClose.Location = new Point(472, 305);
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);

            this.ResumeLayout(false);
        }

        private void LoadChoices()
        {
            try
            {
                var choices = gameService.GetChoicesForQuestion(questionId);
                dgvChoices.DataSource = null;
                dgvChoices.DataSource = choices;

                if (dgvChoices.Columns["ChoiceID"] != null)
                {
                    dgvChoices.Columns["ChoiceID"].HeaderText = "ID";
                    dgvChoices.Columns["ChoiceID"].ReadOnly = true;
                }
                if (dgvChoices.Columns["ChoiceText"] != null)
                {
                    dgvChoices.Columns["ChoiceText"].HeaderText = "Texto";
                    dgvChoices.Columns["ChoiceText"].ReadOnly = false;
                }
                if (dgvChoices.Columns["IsCorrect"] != null)
                {
                    dgvChoices.Columns["IsCorrect"].HeaderText = "Correcta";
                    dgvChoices.Columns["IsCorrect"].ReadOnly = false;
                    if (!(dgvChoices.Columns["IsCorrect"] is DataGridViewCheckBoxColumn))
                    {
                        int idx = dgvChoices.Columns["IsCorrect"].Index;
                        dgvChoices.Columns.RemoveAt(idx);
                        var chk = new DataGridViewCheckBoxColumn()
                        {
                            Name = "IsCorrect",
                            HeaderText = "Correcta",
                            DataPropertyName = "IsCorrect",
                            TrueValue = true,
                            FalseValue = false
                        };
                        dgvChoices.Columns.Insert(idx, chk);
                    }
                }

                dgvChoices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar opciones: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string ctext = Microsoft.VisualBasic.Interaction.InputBox("Ingrese texto de la nueva opción:", "Agregar Opción", "");
                if (string.IsNullOrWhiteSpace(ctext)) return;
                var isCorrect = MessageBox.Show($"¿Es esta opción la correcta?\n\n{ctext}", "Marcar correcta", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                var choice = new Choice(ctext.Trim(), isCorrect);
                int cid = gameService.CreateChoice(questionId, choice);
                MessageBox.Show("Opción agregada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadChoices();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar opción: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvChoices.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione una opción.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var row = dgvChoices.SelectedRows[0];
                int cid = Convert.ToInt32(row.Cells["ChoiceID"].Value);
                string currentText = row.Cells["ChoiceText"].Value.ToString();
                bool currentIsCorrect = Convert.ToBoolean(row.Cells["IsCorrect"].Value);

                string newText = Microsoft.VisualBasic.Interaction.InputBox("Editar texto de la opción:", "Editar Opción", currentText);
                if (string.IsNullOrWhiteSpace(newText)) return;
                var isCorrect = MessageBox.Show($"¿Es esta opción la correcta?\n\n{newText}", "Marcar correcta", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

                var choice = new Choice(newText.Trim(), isCorrect) { ChoiceID = cid };
                if (gameService.UpdateChoice(cid, choice))
                {
                    MessageBox.Show("Opción actualizada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadChoices();
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar la opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar opción: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvChoices.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione una opción.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var row = dgvChoices.SelectedRows[0];
                int cid = Convert.ToInt32(row.Cells["ChoiceID"].Value);
                string txt = row.Cells["ChoiceText"].Value.ToString();

                var res = MessageBox.Show($"¿Eliminar la opción?\n\n{txt}", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    if (gameService.DeleteChoice(cid))
                    {
                        MessageBox.Show("Opción eliminada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadChoices();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar la opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar opción: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvChoices_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnEdit_Click(sender, EventArgs.Empty);
            }
        }

        private void DgvChoices_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvChoices.IsCurrentCellDirty)
            {
                dgvChoices.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DgvChoices_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var colName = dgvChoices.Columns[e.ColumnIndex].Name;
            if (colName != "IsCorrect" && colName != "ChoiceText") return;

            try
            {
                int cid = Convert.ToInt32(dgvChoices.Rows[e.RowIndex].Cells["ChoiceID"].Value);
                string txt = dgvChoices.Rows[e.RowIndex].Cells["ChoiceText"].Value?.ToString() ?? "";
                bool isCorrect = false;
                var cellVal = dgvChoices.Rows[e.RowIndex].Cells["IsCorrect"].Value;
                if (cellVal is bool b) isCorrect = b;
                else if (cellVal != null) isCorrect = Convert.ToBoolean(cellVal);

                var choice = new Choice(txt.Trim(), isCorrect) { ChoiceID = cid };
                if (gameService.UpdateChoice(cid, choice))
                {
                    // Reload choices to reflect changes (especially when setting correct flag)
                    LoadChoices();
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar la opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadChoices();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar opción: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadChoices();
            }
        }
    }
}