using Final_term_project;
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
    public partial class AddChildren : Form
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\USER\\Documents\\Project.mdf;Integrated Security=True;Connect Timeout=30";

        private Admin adminForm;
        private Employee empForm;
        public AddChildren(Admin adminInstance)
        {
            InitializeComponent();
            //comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.adminForm = adminInstance;
        }
        public AddChildren(Employee empInstance)
        {
            InitializeComponent();
            //comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.empForm = empInstance;
        }

        private void AddChildren_Load(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            string fullName = textBox1.Text;
            string height = textBox2.Text;
            string weight = textBox3.Text;
            DateTime dob = dateTimePicker1.Value;
            string gender = comboBox1.Text;
            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(height) ||
                string.IsNullOrWhiteSpace(weight) ||
                string.IsNullOrWhiteSpace(gender))
            {
                MessageBox.Show("Please fill all the box");
            }
            else
            {
                string query = "INSERT INTO [CHILD] ([Name],[Gender],[Height],[DOB],[Weight],[Status]) " +
                                 "VALUES (@fullname, @gender, @height, @dob, @weight,@status)";
                try
                {
                    SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Parameters.AddWithValue("@height", height);
                        cmd.Parameters.AddWithValue("@fullname", fullName);
                        cmd.Parameters.AddWithValue("@dob", dob);
                        cmd.Parameters.AddWithValue("@weight", weight);
                        cmd.Parameters.AddWithValue("@status", "Available");

                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    MessageBox.Show("Child Added");
                    adminForm.openForm(new Childrens(adminForm));

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (adminForm != null)
            {
                adminForm.openForm(new Childrens(adminForm));
            }
            else if (empForm != null)
            {
                empForm.openEForm(new Childrens(empForm));
            }
        }
    }
}
