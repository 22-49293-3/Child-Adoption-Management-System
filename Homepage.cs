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
    public partial class Homepage : Form
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\USER\\Documents\\Project.mdf;Integrated Security=True;Connect Timeout=30";
        private Button currentButton;
        private Random random;
        private int tempIndex;
        private Form activeForm;
        private string username;

        public Homepage(string username)
        {
            InitializeComponent();
            random = new Random();
            btnCloseChildForm.Visible = false;
            this.username = username;
        }
        private Color SelectThemeColor()
        {
            int index = random.Next(ThemeColor.ColorList.Count);
            while (tempIndex == index)
            {
                index = random.Next(ThemeColor.ColorList.Count);
            }
            tempIndex = index;
            string color = ThemeColor.ColorList[tempIndex];
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
                    currentButton.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new AvailableChildrens(this,username), sender);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenChildForm(new RequestInfo(username), sender);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenChildForm(new AdoptionStatus(username), sender);
        }

        /*private void button4_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FindFamily(), sender);
        }*/

        private void button5_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Profile(username), sender);
        }
        private void displayInformation()
        {
            string available = "SELECT COUNT(*) FROM [CHILD] WHERE [Status] = 'Available'";
            string request = "SELECT COUNT(*) FROM [CHILD] WHERE [RequestedBy] = @username AND [Status] = 'Requested'";
            string adopt = "SELECT COUNT(*) FROM [CHILD] WHERE [RequestedBy] = @username AND [Status] = 'Adopted'";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand achild = new SqlCommand(available, con))
                    {
                        int availableChild = (int)achild.ExecuteScalar();
                        label5.Text = availableChild.ToString();
                    }
                    using (SqlCommand rchild = new SqlCommand(request, con))
                    {
                        rchild.Parameters.AddWithValue("@username", username);
                        int requested = (int)rchild.ExecuteScalar();
                        label6.Text =requested.ToString();
                    }
                    using (SqlCommand adchild = new SqlCommand(adopt, con))
                    {
                        adchild.Parameters.AddWithValue("@username", username);
                        int adopted = (int)adchild.ExecuteScalar();
                        label7.Text =adopted.ToString();
                    }
                    con.Close(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void panelDesktopPane_Paint(object sender, PaintEventArgs e)
        {
            displayInformation();
        }
    }
}
