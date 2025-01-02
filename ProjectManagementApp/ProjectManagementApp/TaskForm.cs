using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectManagementApp
{
    internal class TaskForm : Form
    {
        private DataGridView dgvTasks;
        private TextBox txtTaskName;
        private ComboBox cmbProjects;
        private ComboBox cmbEmployees;
        private ComboBox cmbStatus;
        private DateTimePicker dtpTaskStartDate;
        private DateTimePicker dtpTaskEndDate;
        private Button btnAddTask;
        private Button btnUpdateTask;
        private Button btnDeleteTask;

        public TaskForm()
        {
            this.Text = "Task Management";
            this.Size = new System.Drawing.Size(800, 550);

            dgvTasks = new DataGridView()
            {
                Location = new System.Drawing.Point(50, 50),
                Width = 700,
                Height = 200,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            txtTaskName = new TextBox() { Location = new System.Drawing.Point(50, 270), Width = 150 };
            Label lblTaskName = new Label() { Text = "Task Name", Location = new System.Drawing.Point(50, 250) };

            cmbProjects = new ComboBox() { Location = new System.Drawing.Point(220, 270), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            Label lblProjects = new Label() { Text = "Project", Location = new System.Drawing.Point(220, 250) };

            cmbEmployees = new ComboBox() { Location = new System.Drawing.Point(390, 270), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            Label lblEmployees = new Label() { Text = "Assigned To", Location = new System.Drawing.Point(390, 250) };

            cmbStatus = new ComboBox() { Location = new System.Drawing.Point(560, 270), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStatus.Items.AddRange(new string[] { "Pending", "In Progress", "Completed" });
            Label lblStatus = new Label() { Text = "Status", Location = new System.Drawing.Point(560, 250) };

            dtpTaskStartDate = new DateTimePicker() { Location = new System.Drawing.Point(50, 320), Width = 200 };
            Label lblStartDate = new Label() { Text = "Start Date", Location = new System.Drawing.Point(50, 300) };

            dtpTaskEndDate = new DateTimePicker() { Location = new System.Drawing.Point(270, 320), Width = 200 };
            Label lblEndDate = new Label() { Text = "End Date", Location = new System.Drawing.Point(270, 300) };

            btnAddTask = new Button() { Text = "Add Task", Location = new System.Drawing.Point(50, 360), Width = 100 };
            btnAddTask.Click += btnAddTask_Click;

            btnUpdateTask = new Button() { Text = "Update Task", Location = new System.Drawing.Point(170, 360), Width = 100 };
            btnUpdateTask.Click += btnUpdateTask_Click;

            btnDeleteTask = new Button() { Text = "Delete Task", Location = new System.Drawing.Point(290, 360), Width = 100 };
            btnDeleteTask.Click += btnDeleteTask_Click;

            this.Controls.Add(dgvTasks);
            this.Controls.Add(txtTaskName);
            this.Controls.Add(lblTaskName);
            this.Controls.Add(cmbProjects);
            this.Controls.Add(lblProjects);
            this.Controls.Add(cmbEmployees);
            this.Controls.Add(lblEmployees);
            this.Controls.Add(cmbStatus);
            this.Controls.Add(lblStatus);
            this.Controls.Add(dtpTaskStartDate);
            this.Controls.Add(lblStartDate);
            this.Controls.Add(dtpTaskEndDate);
            this.Controls.Add(lblEndDate);
            this.Controls.Add(btnAddTask);
            this.Controls.Add(btnUpdateTask);
            this.Controls.Add(btnDeleteTask);

            LoadTasks();
            LoadProjects();
            LoadEmployees();

            dgvTasks.SelectionChanged += DgvTasks_SelectionChanged;
        }

        private void LoadTasks()
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Tasks";
                using (var command = new SqlCommand(query, connection))
                {
                    var reader = command.ExecuteReader();
                    var dt = new DataTable();
                    dt.Load(reader);
                    dgvTasks.DataSource = dt;
                }
            }
        }

        private void LoadProjects()
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT ProjectId, ProjectName FROM Projects";
                using (var command = new SqlCommand(query, connection))
                {
                    var reader = command.ExecuteReader();
                    var dt = new DataTable();
                    dt.Load(reader);
                    cmbProjects.DataSource = dt;
                    cmbProjects.DisplayMember = "ProjectName";
                    cmbProjects.ValueMember = "ProjectId";
                }
            }
        }

        private void LoadEmployees()
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT EmployeeId, Name FROM Employees";
                using (var command = new SqlCommand(query, connection))
                {
                    var reader = command.ExecuteReader();
                    var dt = new DataTable();
                    dt.Load(reader);
                    cmbEmployees.DataSource = dt;
                    cmbEmployees.DisplayMember = "Name";
                    cmbEmployees.ValueMember = "EmployeeId";
                }
            }
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTaskName.Text) || cmbProjects.SelectedValue == null || cmbEmployees.SelectedValue == null || cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO Tasks (TaskName, ProjectId, AssignedTo, StartDate, EndDate, Status) VALUES (@TaskName, @ProjectId, @AssignedTo, @StartDate, @EndDate, @Status)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TaskName", txtTaskName.Text);
                    command.Parameters.AddWithValue("@ProjectId", cmbProjects.SelectedValue);
                    command.Parameters.AddWithValue("@AssignedTo", cmbEmployees.SelectedValue);
                    command.Parameters.AddWithValue("@StartDate", dtpTaskStartDate.Value);
                    command.Parameters.AddWithValue("@EndDate", dtpTaskEndDate.Value);
                    command.Parameters.AddWithValue("@Status", cmbStatus.SelectedItem.ToString());

                    command.ExecuteNonQuery();
                    MessageBox.Show("Task added successfully!");
                    LoadTasks();
                }
            }
        }

        private void btnUpdateTask_Click(object sender, EventArgs e)
        {
            if (dgvTasks.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(txtTaskName.Text))
            {
                MessageBox.Show("Please select a task and fill in all fields.");
                return;
            }

            int taskId = Convert.ToInt32(dgvTasks.SelectedRows[0].Cells["TaskId"].Value);

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "UPDATE Tasks SET TaskName = @TaskName, ProjectId = @ProjectId, AssignedTo = @AssignedTo, StartDate = @StartDate, EndDate = @EndDate, Status = @Status WHERE TaskId = @TaskId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TaskName", txtTaskName.Text);
                    command.Parameters.AddWithValue("@ProjectId", cmbProjects.SelectedValue);
                    command.Parameters.AddWithValue("@AssignedTo", cmbEmployees.SelectedValue);
                    command.Parameters.AddWithValue("@StartDate", dtpTaskStartDate.Value);
                    command.Parameters.AddWithValue("@EndDate", dtpTaskEndDate.Value);
                    command.Parameters.AddWithValue("@Status", cmbStatus.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@TaskId", taskId);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Task updated successfully!");
                    LoadTasks();
                }
            }
        }

        private void btnDeleteTask_Click(object sender, EventArgs e)
        {
            if (dgvTasks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a task to delete.");
                return;
            }

            int taskId = Convert.ToInt32(dgvTasks.SelectedRows[0].Cells["TaskId"].Value);

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM Tasks WHERE TaskId = @TaskId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TaskId", taskId);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Task deleted successfully!");
                    LoadTasks();
                }
            }
        }

        private void DgvTasks_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTasks.SelectedRows.Count > 0)
            {
                var row = dgvTasks.SelectedRows[0];

                txtTaskName.Text = row.Cells["TaskName"].Value?.ToString() ?? string.Empty;
                cmbProjects.SelectedValue = row.Cells["ProjectId"].Value ?? null;
                cmbEmployees.SelectedValue = row.Cells["AssignedTo"].Value ?? null;
                cmbStatus.SelectedItem = row.Cells["Status"].Value?.ToString() ?? string.Empty;

                
                if (row.Cells["StartDate"].Value != DBNull.Value && row.Cells["StartDate"].Value != null)
                {
                    dtpTaskStartDate.Value = Convert.ToDateTime(row.Cells["StartDate"].Value);
                }

                if (row.Cells["EndDate"].Value != DBNull.Value && row.Cells["EndDate"].Value != null)
                {
                    dtpTaskEndDate.Value = Convert.ToDateTime(row.Cells["EndDate"].Value);
                }
            }
        }


    }
}