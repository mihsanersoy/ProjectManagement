using System;
using System.Windows.Forms;

namespace ProjectManagementApp
{
    internal class MainForm : Form
    {
        private Button btnProjects;
        private Button btnTasks;
        private Button btnEmployees;
        private Button btnLogout;

        public MainForm()
        {
            this.Text = "Main Menu";
            this.Size = new System.Drawing.Size(400, 300);

            btnProjects = new Button() { Text = "Manage Projects", Location = new System.Drawing.Point(50, 50), Width = 150 };
            btnTasks = new Button() { Text = "Manage Tasks", Location = new System.Drawing.Point(50, 100), Width = 150 };
            btnEmployees = new Button() { Text = "Manage Employees", Location = new System.Drawing.Point(50, 150), Width = 150 };
            btnLogout = new Button() { Text = "Logout", Location = new System.Drawing.Point(50, 200), Width = 150 };

            btnProjects.Click += BtnProjects_Click;
            btnTasks.Click += BtnTasks_Click;
            btnEmployees.Click += BtnEmployees_Click;
            btnLogout.Click += BtnLogout_Click;

            this.Controls.Add(btnProjects);
            this.Controls.Add(btnTasks);
            this.Controls.Add(btnEmployees);
            this.Controls.Add(btnLogout);
        }

        private void BtnProjects_Click(object sender, EventArgs e)
        {
            ProjectForm projectForm = new ProjectForm();
            projectForm.Show();
        }

        private void BtnTasks_Click(object sender, EventArgs e)
        {
            TaskForm taskForm = new TaskForm();
            taskForm.Show();
        }

        private void BtnEmployees_Click(object sender, EventArgs e)
        {
            EmployeeForm employeeForm = new EmployeeForm();
            employeeForm.Show();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
