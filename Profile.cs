using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Child_Adoption
{
    public partial class Profile : Form
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\USER\\Documents\\Project.mdf;Integrated Security=True;Connect Timeout=30";
        private string username;

        public Profile(string username)
        {
            InitializeComponent();
            this.username = username;
            LoadUserInfo();
        }

     
        private void LoadUserInfo()
        {
          
            string query = "SELECT [FullName], [Email], [Contact], [Nid], [EmployeeId] FROM [User] WHERE [Username] = @username";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                               
                                labelFullName.Text ="Name: "+ reader["FullName"].ToString();
                                labelEmail.Text ="Email: "+ reader["Email"].ToString();
                                labelContact.Text = "Contact: " + reader["Contact"].ToString();
                                labelNid.Text = "Nid: " + reader["Nid"].ToString();
                                labelEmployeeId.Text = "EmployeeId: " + reader["EmployeeId"].ToString() != "" ? reader["EmployeeId"].ToString() : "N/A";
                            }
                            else
                            {
                                MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
