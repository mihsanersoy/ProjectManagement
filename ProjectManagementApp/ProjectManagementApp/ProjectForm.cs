using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectManagementApp
{
    internal class ProjectForm : Form
    {
        private DataGridView dgvProjects;
        private TextBox txtProjectName;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private Button btnAddProject;

        public ProjectForm()
        {
            this.Text = "Manage Projects";
            this.Size = new System.Drawing.Size(600, 500);

            dgvProjects = new DataGridView()
            {
                Location = new System.Drawing.Point(50, 50),
                Width = 500,
                Height = 200,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            txtProjectName = new TextBox() { Location = new System.Drawing.Point(50, 270), Width = 150 };
            dtpStartDate = new DateTimePicker() { Location = new System.Drawing.Point(220, 270), Width = 150 };
            dtpEndDate = new DateTimePicker() { Location = new System.Drawing.Point(390, 270), Width = 150 };

            btnAddProject = new Button() { Text = "Add Project", Location = new System.Drawing.Point(50, 310), Width = 150 };
            btnAddProject.Click += BtnAddProject_Click;

            Button btnUpdateProject = new Button() { Text = "Update Project", Location = new System.Drawing.Point(220, 310), Width = 150 };
            btnUpdateProject.Click += BtnUpdateProject_Click;

            Button btnDeleteProject = new Button() { Text = "Delete Project", Location = new System.Drawing.Point(390, 310), Width = 150 };
            btnDeleteProject.Click += BtnDeleteProject_Click;

            this.Controls.Add(dgvProjects);
            this.Controls.Add(txtProjectName);
            this.Controls.Add(dtpStartDate);
            this.Controls.Add(dtpEndDate);
            this.Controls.Add(btnAddProject);
            this.Controls.Add(btnUpdateProject);
            this.Controls.Add(btnDeleteProject);

            LoadProjects();

            dgvProjects.SelectionChanged += DgvProjects_SelectionChanged;
        }

        private void LoadProjects()
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Projects";
                using (var command = new SqlCommand(query, connection))
                {
                    var reader = command.ExecuteReader();
                    var dt = new DataTable();
                    dt.Load(reader);
                    dgvProjects.DataSource = dt;
                }
            }
        }

        private void BtnAddProject_Click(object sender, EventArgs e)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO Projects (ProjectName, StartDate, EndDate) VALUES (@ProjectName, @StartDate, @EndDate)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProjectName", txtProjectName.Text);
                    command.Parameters.AddWithValue("@StartDate", dtpStartDate.Value);
                    command.Parameters.AddWithValue("@EndDate", dtpEndDate.Value);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Project added successfully!");
                    LoadProjects();
                }
            }
        }

        private void BtnUpdateProject_Click(object sender, EventArgs e)
        {
            if (dgvProjects.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a project to update.");
                return;
            }

            int projectId = Convert.ToInt32(dgvProjects.SelectedRows[0].Cells["ProjectId"].Value);

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "UPDATE Projects SET ProjectName = @ProjectName, StartDate = @StartDate, EndDate = @EndDate WHERE ProjectId = @ProjectId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProjectName", txtProjectName.Text);
                    command.Parameters.AddWithValue("@StartDate", dtpStartDate.Value);
                    command.Parameters.AddWithValue("@EndDate", dtpEndDate.Value);
                    command.Parameters.AddWithValue("@ProjectId", projectId);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Project updated successfully!");
                    LoadProjects();
                }
            }
        }

        private void BtnDeleteProject_Click(object sender, EventArgs e)
        {
            if (dgvProjects.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a project to delete.");
                return;
            }

            int projectId = Convert.ToInt32(dgvProjects.SelectedRows[0].Cells["ProjectId"].Value);

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM Projects WHERE ProjectId = @ProjectId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProjectId", projectId);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Project deleted successfully!");
                    LoadProjects();
                }
            }
        }

        private void DgvProjects_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProjects.SelectedRows.Count > 0)
            {
                var row = dgvProjects.SelectedRows[0];
                txtProjectName.Text = row.Cells["ProjectName"].Value.ToString();
                dtpStartDate.Value = Convert.ToDateTime(row.Cells["StartDate"].Value);
                dtpEndDate.Value = Convert.ToDateTime(row.Cells["EndDate"].Value);
            }
        }
    }
}
