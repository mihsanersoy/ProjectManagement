using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectManagementApp
{
    internal class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblMessage;

        public LoginForm()
        {
            this.Text = "Login";
            this.Size = new System.Drawing.Size(400, 300);

            var lblUsername = new Label() { Text = "Username:", Location = new System.Drawing.Point(50, 50) };
            var lblPassword = new Label() { Text = "Password:", Location = new System.Drawing.Point(50, 100) };
            lblMessage = new Label() { Location = new System.Drawing.Point(50, 200), ForeColor = System.Drawing.Color.Red };

            txtUsername = new TextBox() { Location = new System.Drawing.Point(150, 50), Width = 150 };
            txtPassword = new TextBox() { Location = new System.Drawing.Point(150, 100), Width = 150, PasswordChar = '*' };

            btnLogin = new Button() { Text = "Login", Location = new System.Drawing.Point(150, 150) };
            btnLogin.Click += btnLogin_Click;

            this.Controls.Add(lblUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtUsername);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(lblMessage);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", txtUsername.Text);
                    command.Parameters.AddWithValue("@Password", txtPassword.Text);

                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        MessageBox.Show("Login successful!");
                        MainForm mainForm = new MainForm();
                        mainForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        lblMessage.Text = "Invalid username or password.";
                    }
                }
            }
        }
    }
}
