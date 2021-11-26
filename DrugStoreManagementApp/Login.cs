using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DrugStoreManagementApp
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=C:\Users\Admin\DrugStoreManagementApp\DrugStoreDb.mdf;Integrated Security = True; Connect Timeout = 30");
        private int CheckUsernamePassword()
        {
            Con.Open();
            string Query = "Select * from EmployeeTable";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                if (UserNameTb.Text == dr["EmpUser"].ToString())
                {
                    if(PasswordTb.Text == dr["EmpPass"].ToString())
                    {
                        MessageBox.Show("Đăng nhập thành công");
                        Con.Close();
                        return 1;
                    }
                    else
                    {
                        MessageBox.Show("Sai mật khẩu");
                        Con.Close();
                        return 0;
                    }
                }
                else
                {
                    MessageBox.Show("Tên tài khoản không đúng");
                    Con.Close();
                    return 0;
                }
            }
            Con.Close();
            return 0;
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            if (CheckUsernamePassword() == 1) {
                Home Obj = new Home();
                Obj.Show();
                this.Hide();
            }
        }

        private void ExitPb_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
