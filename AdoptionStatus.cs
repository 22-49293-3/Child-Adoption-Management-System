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
    public partial class AdoptionStatus : Form
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\USER\\Documents\\Project.mdf;Integrated Security=True;Connect Timeout=30";

        private Homepage homeForm;
        private string username;
        private string childId;
        public AdoptionStatus(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void AdoptionStatus_Load(object sender, EventArgs e)
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
        private void LoadData()
        {
            dataGridView1.Rows.Clear();
            string query = "SELECT [ID], [Name], [Gender], [Height], [Weight],[DOB], [Status] FROM [Child] WHERE [RequestedBy] = @username AND [Status] = 'Adopted'";
            FillDataGridView(query);
        }
        private void FillDataGridView(string query)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@username", username);
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        dataGridView1.Rows.Add(
                        reader["ID"].ToString(),
                        reader["Name"].ToString(),
                        reader["Gender"].ToString(),
                        reader["Height"].ToString(),
                        reader["Weight"].ToString(),
                        reader["DOB"].ToString(),
                        reader["Status"].ToString()
                        );

                    }
                    reader.Close();
                    con.Close();
                }
            }/*
            if (!dataGridView1.Columns.Contains("Cancel"))
            {
                DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();
                deleteButtonColumn.Name = "Cancel";
                deleteButtonColumn.HeaderText = "Cancel";
                deleteButtonColumn.Text = "Cancel";
                deleteButtonColumn.UseColumnTextForButtonValue = true;
                dataGridView1.Columns.Add(deleteButtonColumn);
            }*/
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
            dataGridView1.Columns[e.ColumnIndex].Name == "Cancel" && e.RowIndex >= 0)
            {
                string childId = dataGridView1.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                string query = "UPDATE [Child] SET [Status] = 'Available', [RequestedBy] = NULL WHERE [ID] = @childId";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@childId", childId);
                        con.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        con.Close();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Request canceled successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to cancel the request.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}
