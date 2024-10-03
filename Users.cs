using Final_term_project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Child_Adoption
{
    public partial class Users : Form
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\USER\\Documents\\Project.mdf;Integrated Security=True;Connect Timeout=30";
        private Admin adminForm;
        private Employee empForm;
        private bool state = true;
        public Users()
        {
            InitializeComponent();
        }

        public Users(Employee empInstance)
        {
            InitializeComponent();
            this.empForm = empInstance;
        }
        public Users(Admin adminInstance)
        {
            InitializeComponent();
            this.adminForm = adminInstance;
        }
        private void Users_Load(object sender, EventArgs e)
        {
            LoadTheme();
            LoadData();
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

        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void LoadData()
        {
            dataGridView1.Rows.Clear();
            string query = "SELECT [UserID],[FullName],[Username],[Email],[Contact],[Nid],[Password],[EmployeeId] FROM [USER]";
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
                        dataGridView1.Rows.Add(reader["UserID"].ToString(),
                            reader["FullName"].ToString(),
                            reader["Username"].ToString(),
                            reader["Email"].ToString(),
                            reader["Contact"].ToString(),
                            reader["Nid"].ToString(),
                            reader["Password"].ToString(),
                            reader["EmployeeId"].ToString()
                        );
                    }
                    reader.Close();
                    con.Close();
                }
            }
            if (!dataGridView1.Columns.Contains("Delete"))
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
            if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                dataGridView1.Columns[e.ColumnIndex].Name == "Delete" && e.RowIndex >= 0)
            {
                if (MessageBox.Show("Are you sure you want to delete this row?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    String userid = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    string query = $"DELETE FROM [USER] WHERE UserId =@userid";
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
                string userid = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                string fullName = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                string username = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                string email = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                string phone = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                string nid = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                string password = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                string empid = dataGridView1.CurrentRow.Cells[7].Value.ToString();

                string query = "UPDATE [USER] SET [FullName] =@fullname,[Username] = @username, [Email] = @email, [Contact] = @phone, [Nid] = @nid, [Password] = @password, [EmployeeId] = @empid WHERE [UserID]=@userid";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        try
                        {
                            command.Parameters.AddWithValue("@userid", userid);
                            command.Parameters.AddWithValue("@fullname", fullName);
                            command.Parameters.AddWithValue("@username", username);
                            command.Parameters.AddWithValue("@email", email);
                            command.Parameters.AddWithValue("@phone", phone);
                            command.Parameters.AddWithValue("@nid", nid);
                            command.Parameters.AddWithValue("@password", password);
                            command.Parameters.AddWithValue("@empid", empid);
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
            if (empForm != null)
            {
                empForm.openEForm(new AddUser(empForm));
            }
            else if (adminForm != null)
            {
                adminForm.openForm(new AddUser(adminForm));
            }
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            string searchText = textBox1.Text;
            string query = $"SELECT * FROM [USER] WHERE [Username] LIKE '%{searchText}%' OR FullName LIKE '%{searchText}%' OR [UserID] LIKE '%{searchText}%'";
            FillDataGridView(query);
        }
    }
}
