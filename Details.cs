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
    public partial class Details : Form
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\USER\\Documents\\Project.mdf;Integrated Security=True;Connect Timeout=30";

        private Homepage homeForm;
        private string username;
        private string childId;
        public Details(Homepage homeForm, string username, string childId)
        {
            InitializeComponent();
            this.homeForm = homeForm;
            this.username = username;
            this.childId = childId;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Details_Load(object sender, EventArgs e)
        {
            LoadTheme();
            LoadChildDetails();
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
        private void LoadChildDetails()
        {
            string query = "SELECT [Name], [Gender], [Height], [DOB], [Weight] FROM [Child] WHERE [ID] = @ID";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@ID", childId);
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        label6.Text = reader["Name"].ToString();
                        label7.Text = reader["Gender"].ToString();
                        label8.Text = reader["Height"].ToString();
                        label10.Text = reader["DOB"].ToString();
                        label9.Text = reader["Weight"].ToString();
                    }
                    reader.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            homeForm.openForm(new AvailableChildrens(homeForm,username));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query = "UPDATE [Child] SET [RequestedBy] = @requestedBy, [Status] = @status WHERE [ID] = @ID";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@requestedBy", username);
                    command.Parameters.AddWithValue("@status", "Requested");
                    command.Parameters.AddWithValue("@ID", childId);

                    con.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    con.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Request submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        homeForm.openForm(new AvailableChildrens(homeForm, username));
                    }
                    else
                    {
                        MessageBox.Show("Failed to submit the request.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
