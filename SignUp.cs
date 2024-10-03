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
    public partial class SignUp : Form
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\USER\\Documents\\Project.mdf;Integrated Security=True;Connect Timeout=30";
        public SignUp()
        {
            InitializeComponent();
            textBox6.PasswordChar = '●';
            textBox7.PasswordChar = '●';
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox6.PasswordChar = checkBox1.Checked ? '\0' : '●';
            textBox7.PasswordChar = checkBox1.Checked ? '\0' : '●';
        }

        //Validity check
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

        //Sign up button
        private void button2_Click(object sender, EventArgs e)
        {

            string fullName = textBox1.Text;
            string username = textBox2.Text;
            string email = textBox3.Text;
            string phone = textBox4.Text;
            string nid = textBox5.Text;
            string password = textBox6.Text;
            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(nid) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill all the box");
            }
            else if (!textBox6.Text.Equals(textBox7.Text))
            {
                MessageBox.Show("Password and Confrim password must be same");
            }
            else if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address");
            }
            else if (!IsNumeric(nid) || !IsNumeric(phone))
            {
                MessageBox.Show("NID and Phone Number must be numeric");
            }
            else
            {
                string query = "INSERT INTO [USER] ([FullName],[Username],[Email],[Contact],[Nid],[Password]) " +
                                 "VALUES (@fullname, @username, @email, @phone, @nid, @password)";
                try
                {
                    SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@fullname", fullName);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@nid", nid);
                        cmd.Parameters.AddWithValue("@email", email);

                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    MessageBox.Show("Sign Up successful");
                    this.Hide();
                    Login f = new Login();
                    f.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            new Login().Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}