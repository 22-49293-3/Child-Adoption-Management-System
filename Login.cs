using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Child_Adoption
{
    public partial class Login : Form
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\USER\\Documents\\Project.mdf;Integrated Security=True;Connect Timeout=30";
        public Login()
        {
            InitializeComponent();

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;
            textBox2.PasswordChar = '●';
        }

        //Sign up button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            SignUp signup = new SignUp();
            signup.Show();
        }

        //Hide password with ●●●●
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = checkBox1.Checked ? '\0' : '●';
        }

        //Log in button click
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            string query = null;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter your username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (comboBox1.SelectedIndex == 0)
            {
                query = $"SELECT COUNT(*) FROM [USER] WHERE [Username]=@username AND [Password] = @password";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                query = $"SELECT COUNT(*) FROM EMPLOYEE WHERE [Username]=@username AND [Password] = @password";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                query = $"SELECT COUNT(*) FROM ADMIN WHERE [Username]=@username AND [Password] = @password";
            }
            if (query == null)
            {
                return;
            }
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            if (comboBox1.SelectedIndex == 1)
                            {
                                this.Hide();
                                new Employee().Show();
                            }
                            else if (comboBox1.SelectedIndex == 2)
                            {
                                this.Hide();
                                new Admin().Show();
                            }
                            else
                            {
                                this.Hide();
                                new Homepage(username).Show();
                            }

                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
