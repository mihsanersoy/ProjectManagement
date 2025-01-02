using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace ProjectManagementApp
{
    internal class EmployeeForm : Form
    {
        private DataGridView dgvEmployees;
        private TextBox txtEmployeeName;
        private TextBox txtEmployeePosition;
        private Button btnAddEmployee;

        public EmployeeForm()
        {
            this.Text = "Employee Management";
            this.Size = new System.Drawing.Size(600, 500);

            dgvEmployees = new DataGridView()
            {
                Location = new System.Drawing.Point(50, 50),
                Width = 500,
                Height = 200,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            txtEmployeeName = new TextBox()
            {
                Location = new System.Drawing.Point(50, 270),
                Width = 150
            };

            txtEmployeePosition = new TextBox()
            {
                Location = new System.Drawing.Point(220, 270),
                Width = 150
            };

            Label lblEmployeeName = new Label()
            {
                Text = "Employee Name",
                Location = new System.Drawing.Point(50, 250)
            };

            Label lblEmployeePosition = new Label()
            {
                Text = "Position",
                Location = new System.Drawing.Point(220, 250)
            };

            btnAddEmployee = new Button()
            {
                Text = "Add Employee",
                Location = new System.Drawing.Point(50, 310),
                Width = 150
            };
            btnAddEmployee.Click += btnAddEmployee_Click;

            Button btnUpdateEmployee = new Button()
            {
                Text = "Update Employee",
                Location = new System.Drawing.Point(220, 310),
                Width = 150
            };
            btnUpdateEmployee.Click += btnUpdateEmployee_Click;

            Button btnDeleteEmployee = new Button()
            {
                Text = "Delete Employee",
                Location = new System.Drawing.Point(390, 310),
                Width = 150
            };
            btnDeleteEmployee.Click += btnDeleteEmployee_Click;

            this.Controls.Add(dgvEmployees);
            this.Controls.Add(txtEmployeeName);
            this.Controls.Add(txtEmployeePosition);
            this.Controls.Add(lblEmployeeName);
            this.Controls.Add(lblEmployeePosition);
            this.Controls.Add(btnAddEmployee);
            this.Controls.Add(btnUpdateEmployee);
            this.Controls.Add(btnDeleteEmployee);

            LoadEmployees();

            dgvEmployees.SelectionChanged += DgvEmployees_SelectionChanged;
        }

        private void LoadEmployees()
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Employees";
                using (var command = new SqlCommand(query, connection))
                {
                    var reader = command.ExecuteReader();
                    var dt = new DataTable();
                    dt.Load(reader);
                    dgvEmployees.DataSource = dt;
                }
            }
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmployeeName.Text) || string.IsNullOrWhiteSpace(txtEmployeePosition.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO Employees (Name, Position) VALUES (@Name, @Position)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", txtEmployeeName.Text);
                    command.Parameters.AddWithValue("@Position", txtEmployeePosition.Text);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Employee added successfully!");
                    LoadEmployees();
                }
            }
        }

        private void btnUpdateEmployee_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(txtEmployeeName.Text) || string.IsNullOrWhiteSpace(txtEmployeePosition.Text))
            {
                MessageBox.Show("Please select an employee and fill in all fields.");
                return;
            }

            int employeeId = Convert.ToInt32(dgvEmployees.SelectedRows[0].Cells["EmployeeId"].Value);

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "UPDATE Employees SET Name = @Name, Position = @Position WHERE EmployeeId = @EmployeeId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", txtEmployeeName.Text);
                    command.Parameters.AddWithValue("@Position", txtEmployeePosition.Text);
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Employee updated successfully!");
                    LoadEmployees();
                }
            }
        }

        private void btnDeleteEmployee_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an employee to delete.");
                return;
            }

            int employeeId = Convert.ToInt32(dgvEmployees.SelectedRows[0].Cells["EmployeeId"].Value);

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Employee deleted successfully!");
                    LoadEmployees();
                }
            }
        }

        private void DgvEmployees_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                var row = dgvEmployees.SelectedRows[0];
                txtEmployeeName.Text = row.Cells["Name"].Value.ToString();
                txtEmployeePosition.Text = row.Cells["Position"].Value.ToString();
            }
        }
    }
}
