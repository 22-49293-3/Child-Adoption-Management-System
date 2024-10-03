using Final_term_project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Child_Adoption
{
    public partial class AddEmployee : Form
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\USER\\Documents\\Project.mdf;Integrated Security=True;Connect Timeout=30";

        private Admin adminForm;
        public AddEmployee(Admin adminInstance)
        {
            InitializeComponent();
            this.adminForm = adminInstance;
            textBox5.PasswordChar = '●';
        }
        private void AddEmployee_Load(object sender, EventArgs e)
        {
            LoadTheme();
        }
        private void LoadTheme()
        {
            foreach (Control btns in this.Controls)
            {
                if (btns.GetType() == typeof(Button))
                {
                    Button btn = (Button)btns;
                    btn.BackColor = ThemeColor.PrimaryColor;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox5.PasswordChar = checkBox1.Checked ? '\0' : '●';
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string fullName = textBox1.Text;
            string username = textBox2.Text;
            string email = textBox3.Text;
            DateTime jod = dateTimePicker1.Value;
            string sal = textBox4.Text;
            string password = textBox5.Text;
            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(sal) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill all the box");
            }
            else if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address");
            }
            else if (!IsNumeric(sal))
            {
                MessageBox.Show("Salary must be numeric");
            }
            else
            {
                string query = "INSERT INTO [EMPLOYEE] ([Name],[Username],[Password],[Email],[JoiningDate],[Salary]) " +
                                 "VALUES (@fullname, @username, @password, @email, @jod, @sal)";
                try
                {
                    SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@fullname", fullName);
                        cmd.Parameters.AddWithValue("@sal", sal);
                        cmd.Parameters.AddWithValue("@jod", jod);
                        cmd.Parameters.AddWithValue("@email", email);
                        //cmd.Parameters.AddWithValue("@empid", 1);

                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    MessageBox.Show("User Added");
                    adminForm.openForm(new Employees(adminForm));

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            adminForm.openForm(new Employees(adminForm));
        }
    }
}
