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
    public partial class Admin : Form
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\USER\\Documents\\Project.mdf;Integrated Security=True;Connect Timeout=30";
        private Button currentButton;
        private Random random;
        private int tempIndex;
        private Form activeForm;
        public Admin()
        {
            InitializeComponent();
            random = new Random();
            btnCloseChildForm.Visible = false;
        }
        private Color SelectThemeColor() 
        { 
            int index=random.Next(ThemeColor.ColorList.Count);
            while (tempIndex == index)
            {
                index = random.Next(ThemeColor.ColorList.Count);
            }
            tempIndex = index;
            string color= ThemeColor.ColorList[tempIndex];
            return ColorTranslator.FromHtml(color);
        }
        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentButton != (Button)btnSender)
                {
                    DisableButton();
                    Color color = SelectThemeColor();
                    currentButton = (Button)btnSender;
                    currentButton.BackColor = color;
                    currentButton.ForeColor = Color.White;
                    currentButton.Font= new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    panelTitleBar.BackColor = color;
                    panelLogo.BackColor = ThemeColor.ChangeColorBrightness(color, -0.3);
                    ThemeColor.PrimaryColor = color;
                    ThemeColor.SecondaryColor = ThemeColor.ChangeColorBrightness(color, -0.3);
                    btnCloseChildForm.Visible = true;
                }
            }
        }
        private void DisableButton()
        {
            foreach (Control previousBtn in panelMenu.Controls)
            {
                if (previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.BackColor = Color.FromArgb(51, 51, 76);
                    previousBtn.ForeColor = Color.Gainsboro;
                    previousBtn.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }
        public void openForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panelDesktopPane.Controls.Add(childForm);
            this.panelDesktopPane.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lbTitle.Text = childForm.Text;
        }
        private void OpenChildForm(Form childForm, object btnSender)
        {
            if (activeForm != null)
                activeForm.Close();
            ActivateButton(btnSender);
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panelDesktopPane.Controls.Add(childForm);
            this.panelDesktopPane.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lbTitle.Text = childForm.Text;
        }
        private void Reset()
        {
            DisableButton();
            lbTitle.Text = "Home";
            panelTitleBar.BackColor = Color.FromArgb(0, 150, 136);
            panelLogo.BackColor = Color.FromArgb(39, 39, 58);
            currentButton = null;
            btnCloseChildForm.Visible = false;
        }

        private void btnCloseChildForm_Click(object sender, EventArgs e)
        {
            if (activeForm != null)
                activeForm.Close();
            Reset();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Users(this), sender);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Employees(this), sender);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Childrens(this), sender);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenChildForm(new AdoptionRequest(), sender);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login l=new Login();
            l.Show();
        }

        private void panelDesktopPane_Paint(object sender, PaintEventArgs e)
        {
            TotalUsers();
        }
        private void TotalUsers()
        {
            string query = "SELECT COUNT(*) FROM [USER]";
            string query1 = "SELECT COUNT(*) FROM [EMPLOYEE]";
            string query2 = "SELECT COUNT(*) FROM [CHILD]";
            //string query3 = "SELECT COUNT(*) FROM [REQUEST]";

            //update count...
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        int totalUsers = (int)cmd.ExecuteScalar();
                        label5.Text = totalUsers.ToString();
                    }
                    using (SqlCommand cmd = new SqlCommand(query1, con))
                    {
                        int totalUsers = (int)cmd.ExecuteScalar();
                        label6.Text = totalUsers.ToString();
                    }
                    using (SqlCommand cmd = new SqlCommand(query2, con))
                    {
                        int totalUsers = (int)cmd.ExecuteScalar();
                        label7.Text = totalUsers.ToString();
                    }
                  /*  using (SqlCommand cmd = new SqlCommand(query3, con))
                    {
                        int totalUsers = (int)cmd.ExecuteScalar();
                        label8.Text = totalUsers.ToString();
                    }*/
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}
