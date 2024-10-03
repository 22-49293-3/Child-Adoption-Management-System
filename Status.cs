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
    public partial class Status : Form
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\USER\\Documents\\Project.mdf;Integrated Security=True;Connect Timeout=30";
        private Admin adminForm;
        public Status()
        {
            InitializeComponent();
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

        private void Status_Load(object sender, EventArgs e)
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
                   
                }
            }
        }
        private void LoadData()
        {
            dataGridView1.Rows.Clear();
            string query = "SELECT[ID],[Name],[RequestedBy],[Status] FROM [Child]";
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
                        // reader["Gender"].ToString(),
                        //reader["Height"].ToString(),
                        //reader["Weight"].ToString(),
                        //reader["DOB"].ToString(),
                        reader["RequestedBy"].ToString(),
                        reader["Status"].ToString()
                        );

                    }
                    reader.Close();
                    con.Close();
                    //DataTable dataTable = new DataTable();
                    //dataTable.Load(reader);
                    //bunifuDataGridView1.DataSource = dataTable;
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
            if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                if (MessageBox.Show("Are you sure you want to delete this row?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    String id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    string query = $"DELETE FROM [Child] WHERE ID =@id";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            using (SqlCommand command = new SqlCommand(query, con))
                            {
                                command.Parameters.AddWithValue("@id", id);
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
                string name = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                string requestedby = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                string status = dataGridView1.CurrentRow.Cells[3].Value.ToString();

                string query = "UPDATE [CHILD] SET [Name] =@name,[RequestedBy] = @requestedby, [Status] = @status WHERE [ID]=@id";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        try
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@name", name);
                            command.Parameters.AddWithValue("@requestedby", requestedby);
                            command.Parameters.AddWithValue("@status", status);
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
    }
}
