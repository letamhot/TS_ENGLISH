using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TS_Project
{
    public partial class Login : Form
    {
        SqlDataAccess sqlObj = new SqlDataAccess();
        /*SqlConnection doi1 = new SqlConnection(ConfigurationManager.ConnectionStrings["doi1"].ConnectionString);
        SqlConnection pass1 = new SqlConnection(ConfigurationManager.ConnectionStrings["pass1"].ConnectionString);
        SqlConnection doi2 = new SqlConnection(ConfigurationManager.ConnectionStrings["doi2"].ConnectionString);
        SqlConnection pass2 = new SqlConnection(ConfigurationManager.ConnectionStrings["pass2"].ConnectionString);
        SqlConnection doi3 = new SqlConnection(ConfigurationManager.ConnectionStrings["doi3"].ConnectionString);
        SqlConnection pass3 = new SqlConnection(ConfigurationManager.ConnectionStrings["pass3"].ConnectionString);
        SqlConnection doi4 = new SqlConnection(ConfigurationManager.ConnectionStrings["doi4"].ConnectionString);
        SqlConnection pass4 = new SqlConnection(ConfigurationManager.ConnectionStrings["pass4"].ConnectionString);*/
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!");
                txtUsername.Focus();
                return;
            }
            else if(txtPass.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!");
                txtPass.Focus();
                return;
            }
            DataTable dt = new DataTable();
            string sql = "SELECT * FROM ds_doi WHERE tendangnhap = '" + txtUsername.Text + "' AND matkhau = '" + txtPass.Text + "'";
            dt = sqlObj.getDataFromSql(sql, "").Tables[0];
            if (dt.Rows.Count > 0)
            {
                Hide();
                frmThiSinh thiSinh = new frmThiSinh(int.Parse(dt.Rows[0]["doiid"].ToString()));
                thiSinh.FormClosed += new FormClosedEventHandler(thiSinhForm_FormClosed);
                thiSinh.ShowDialog();
                Close();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
                txtUsername.Focus();
                return;
            }
        }

        private void thiSinhForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtUsername.Text = ConfigurationManager.AppSettings["doi"];
            txtPass.Text = ConfigurationManager.AppSettings["pass"];
            this.KeyPreview = true;
            this.KeyDown += Login_KeyDown;
            groupBox1.BackColor = Color.FromArgb(100, 0, 0, 0);
            btnLogin.BackColor = Color.FromArgb(100, 0, 0, 0);
            btnExit.BackColor = Color.FromArgb(100, 0, 0, 0);


        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
