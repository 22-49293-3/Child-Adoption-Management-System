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
    public partial class Employees : Form
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\USER\\Documents\\Project.mdf;Integrated Security=True;Connect Timeout=30";
        private Admin adminForm;
        private bool state = true;
        public Employees()
        {
            InitializeComponent();
            state = false;
        }
        public Employees(Admin adminInstance)
        {
            InitializeComponent();
            this.adminForm = adminInstance;
        }
        private void Employees_Load(object sender, EventArgs e)
        {
            LoadTheme();
            LoadData();
            if (state)
            {
                button1.Enabled = true;
                button1.Visible = true;
                dataGridView1.ReadOnly = false;
            }
            else
            {
                button1.Enabled = false;
                button1.Visible=false;
                dataGridView1.ReadOnly=true;
            }
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
                    dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
                    dataGridView1.DefaultCellStyle.Font = new System.Drawing.Font("Cambria", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = ThemeColor.PrimaryColor;
                    dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                    dataGridView1.DefaultCellStyle.SelectionBackColor = ThemeColor.PrimaryColor;
                    dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Cambria", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void LoadData()
        {
            dataGridView1.Rows.Clear();
            string query = "SELECT [ID],[Name],[Username] ,[Password],[Email],[JoiningDate],[ResigningDate],[Salary],[UserID] FROM [Employee]";
            FillDataGridView(query);
        }
        private void FillDataGridView(string query)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        dataGridView1.Rows.Add(
                        reader["ID"].ToString(),
                        reader["Name"].ToString(),
                        reader["Username"].ToString(),
                        reader["Password"].ToString(),
                        reader["Email"].ToString(),
                        reader["JoiningDate"].ToString(),
                        reader["ResigningDate"].ToString(),
                        reader["Salary"].ToString(),
                        reader["UserID"].ToString()
                        );

                    }
                    reader.Close();
                    con.Close();
                }
            }
            if (!dataGridView1.Columns.Contains("Delete") && state)
            {
                DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();
                deleteButtonColumn.Name = "Delete";
                deleteButtonColumn.HeaderText = "Delete";
                deleteButtonColumn.Text = "Delete";
                deleteButtonColumn.UseColumnTextForButtonValue = true;
                dataGridView1.Columns.Add(deleteButtonColumn);
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!state)
            {
                return;
            }
            if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                if (MessageBox.Show("Are you sure you want to delete this row?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    String userid = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    string query = $"DELETE FROM [EMPLOYEE] WHERE ID =@userid";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            using (SqlCommand command = new SqlCommand(query, con))
                            {
                                command.Parameters.AddWithValue("@userid", userid);
                                con.Open();
                                command.ExecuteNonQuery();
                                con.Close();
                                MessageBox.Show("Record deleted successfully.");
                            }
                        }

                        dataGridView1.Rows.RemoveAt(e.RowIndex);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
            }
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                string fullName = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                string username = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                string password = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                string email = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                string jod = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                string rod = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                string sal = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                string userid = dataGridView1.CurrentRow.Cells[8].Value.ToString();

                string query = "UPDATE [EMPLOYEE] SET [Name] =@fullname,[Username] = @username, [Password] = @password, [Email] = @email, [JoiningDate] = @jod, [ResigningDate] = @rod, [Salary] = @sal, [UserId] = @userid WHERE [ID]=@id";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        try
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@fullname", fullName);
                            command.Parameters.AddWithValue("@username", username);
                            command.Parameters.AddWithValue("@password", password);
                            command.Parameters.AddWithValue("@email", email);
                            command.Parameters.AddWithValue("@jod", jod);
                            command.Parameters.AddWithValue("@rod", rod);
                            command.Parameters.AddWithValue("@sal", sal);
                            command.Parameters.AddWithValue("@Userid", userid);
                            con.Open();
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Updated");
                            }
                            else
                            {
                                MessageBox.Show("Invalid");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            adminForm.openForm(new AddEmployee(adminForm));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            string searchText = textBox1.Text;
            string query = $"SELECT * from Employee WHERE [Name] LIKE '%{searchText}%' OR [ID] LIKE '%{searchText}%' OR [Username] LIKE '%{searchText}%'";
            FillDataGridView(query);
        }
    }
}
