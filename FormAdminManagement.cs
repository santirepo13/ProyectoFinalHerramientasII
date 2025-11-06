using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CodeQuest.Factories;
using CodeQuest.Services;
using CodeQuest.Models;
using Microsoft.VisualBasic;

namespace CodeQuest
{
    public partial class FormAdminManagement : Form
    {
        private readonly IAdministratorService administratorService;
        private DataGridView dgvAdmins;
        private Button btnCreateAdmin;
        private Button btnUpdatePassword;
        private Button btnDeleteAdmin;
        private Button btnRefreshAdmins;
        private Button btnClose;

        public FormAdminManagement()
        {
            administratorService = ServiceFactory.GetAdministratorService();
            InitializeComponent();
            LoadAdminData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Administradores - Gestión";
            this.Size = new Size(700, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(240, 248, 255);

            dgvAdmins = new DataGridView();
            dgvAdmins.Location = new Point(20, 20);
            dgvAdmins.Size = new Size(640, 360);
            dgvAdmins.ReadOnly = true;
            dgvAdmins.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAdmins.AllowUserToAddRows = false;
            dgvAdmins.AllowUserToDeleteRows = false;
            dgvAdmins.BackgroundColor = Color.White;
            this.Controls.Add(dgvAdmins);

            btnCreateAdmin = new Button();
            btnCreateAdmin.Text = "Crear Admin";
            btnCreateAdmin.Size = new Size(120, 35);
            btnCreateAdmin.Location = new Point(20, 400);
            btnCreateAdmin.Click += BtnCreateAdmin_Click;
            this.Controls.Add(btnCreateAdmin);

            btnUpdatePassword = new Button();
            btnUpdatePassword.Text = "Actualizar Contraseña";
            btnUpdatePassword.Size = new Size(160, 35);
            btnUpdatePassword.Location = new Point(150, 400);
            btnUpdatePassword.Click += BtnUpdatePassword_Click;
            this.Controls.Add(btnUpdatePassword);

            btnDeleteAdmin = new Button();
            btnDeleteAdmin.Text = "Eliminar Admin";
            btnDeleteAdmin.Size = new Size(120, 35);
            btnDeleteAdmin.Location = new Point(320, 400);
            btnDeleteAdmin.Click += BtnDeleteAdmin_Click;
            this.Controls.Add(btnDeleteAdmin);

            btnRefreshAdmins = new Button();
            btnRefreshAdmins.Text = "Refrescar";
            btnRefreshAdmins.Size = new Size(120, 35);
            btnRefreshAdmins.Location = new Point(460, 400);
            btnRefreshAdmins.Click += (s, e) => LoadAdminData();
            this.Controls.Add(btnRefreshAdmins);

            btnClose = new Button();
            btnClose.Text = "Cerrar";
            btnClose.Size = new Size(120, 35);
            btnClose.Location = new Point(460, 450);
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);

            this.ResumeLayout(false);
        }

        private void LoadAdminData()
        {
            try
            {
                var admins = administratorService.GetAllAdministrators();
                dgvAdmins.DataSource = admins;

                if (dgvAdmins.Columns["Password"] != null)
                    dgvAdmins.Columns["Password"].Visible = false;

                if (dgvAdmins.Columns["AdminID"] != null)
                    dgvAdmins.Columns["AdminID"].HeaderText = "ID";

                if (dgvAdmins.Columns["Username"] != null)
                    dgvAdmins.Columns["Username"].HeaderText = "Usuario";

                if (dgvAdmins.Columns["CreatedAt"] != null)
                {
                    dgvAdmins.Columns["CreatedAt"].HeaderText = "Creado";
                    dgvAdmins.Columns["CreatedAt"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar administradores: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCreateAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = Microsoft.VisualBasic.Interaction.InputBox("Ingrese nombre de usuario:", "Crear Administrador", "");
                if (string.IsNullOrWhiteSpace(username)) return;
                string password = Microsoft.VisualBasic.Interaction.InputBox("Ingrese contraseña:", "Crear Administrador", "");
                if (string.IsNullOrWhiteSpace(password)) return;

                int newId = administratorService.CreateAdministrator(username.Trim(), password);
                if (newId > 0)
                {
                    MessageBox.Show($"Administrador '{username}' creado (ID: {newId}).", "Creación Exitosa",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAdminData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear administrador: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdatePassword_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAdmins.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione un administrador.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var row = dgvAdmins.SelectedRows[0];
                int adminId = Convert.ToInt32(row.Cells["AdminID"].Value);
                string username = row.Cells["Username"].Value.ToString();
                string newPassword = Microsoft.VisualBasic.Interaction.InputBox($"Ingrese nueva contraseña para '{username}':", "Actualizar Contraseña", "");
                if (string.IsNullOrWhiteSpace(newPassword)) return;

                if (administratorService.UpdatePassword(adminId, newPassword))
                {
                    MessageBox.Show("Contraseña actualizada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar la contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                LoadAdminData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar contraseña: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDeleteAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAdmins.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione un administrador.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var row = dgvAdmins.SelectedRows[0];
                int adminId = Convert.ToInt32(row.Cells["AdminID"].Value);
                string username = row.Cells["Username"].Value.ToString();
                var res = MessageBox.Show($"¿Eliminar administrador '{username}' (ID: {adminId})?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    if (administratorService.DeleteAdministrator(adminId))
                    {
                        MessageBox.Show("Administrador eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAdminData();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar administrador: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}