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
using static System.Windows.Forms.AxHost;

namespace Child_Adoption
{
    public partial class AvailableChildrens : Form
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\USER\\Documents\\Project.mdf;Integrated Security=True;Connect Timeout=30";
        private bool state = true;
        private Homepage homeForm;
        private string username;
        public AvailableChildrens(Homepage homeForm, string username)
        {
            InitializeComponent();
            this.homeForm = homeForm;
            this.username = username;
        }

        private void AvailableChildrens_Load(object sender, EventArgs e)
        {
            LoadTheme();
            LoadData();
            dataGridView1.ReadOnly = true;
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
            string query = "SELECT[ID],[Name],[Gender],[Height],[DOB],[Weight] FROM [Child] WHERE [Status] = 'Available'";
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
                        reader["Gender"].ToString(),
                        reader["Height"].ToString(),
                        reader["Weight"].ToString(),
                        reader["DOB"].ToString()
                        );

                    }
                    reader.Close();
                    con.Close();
                }
            }
            if (!dataGridView1.Columns.Contains("Details"))
            {
                DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();
                deleteButtonColumn.Name = "Details";
                deleteButtonColumn.HeaderText = "Details";
                deleteButtonColumn.Text = "Details";
                deleteButtonColumn.UseColumnTextForButtonValue = true;
                dataGridView1.Columns.Add(deleteButtonColumn);
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
            dataGridView1.Columns[e.ColumnIndex].Name == "Details" && e.RowIndex >= 0)
            {
                string childId = dataGridView1.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                homeForm.openForm(new Details(homeForm,username,childId));
            }
        }
    }
}
