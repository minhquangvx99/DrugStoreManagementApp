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
    public partial class Employees : Form
    {
        public Employees()
        {
            InitializeComponent();
            DisplayEmployees();
        }
        SqlConnection Con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=C:\Users\Admin\DrugStoreManagementApp\DrugStoreDb.mdf;Integrated Security = True; Connect Timeout = 30");
        private void DisplayEmployees()
        {
            Con.Open();
            string Query = "Select * from EmployeeTable";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            EmployeesDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void Clear()
        {
            EmpNameTb.Text = "";
            EmpAddTb.Text = "";
            EmpPhoneTb.Text = "";
            EmpUsernameTb.Text = "";
            EmpPasswordTb.Text = "";
            EmpDOBTb.Text = DateTime.Now.ToString();
        }
        int Key = 0;
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if(EmpNameTb.Text == "" || EmpAddTb.Text == "" || EmpPhoneTb.Text == "" || EmpPasswordTb.Text == "" || EmpUsernameTb.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into EmployeeTable (EmpName, EmpAdd, EmpDOB, EmpPhone,EmpUser, EmpPass) values (@EN,@EA,@ED,@EP,@EU, @EPa)", Con);
                    cmd.Parameters.AddWithValue("@EN", EmpNameTb.Text);
                    cmd.Parameters.AddWithValue("@EA", EmpAddTb.Text);
                    cmd.Parameters.AddWithValue("@ED", EmpDOBTb.Value.Date);
                    cmd.Parameters.AddWithValue("@EP", EmpPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@EU", EmpUsernameTb.Text);
                    cmd.Parameters.AddWithValue("@EPa", EmpPasswordTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã thêm thông tin nhân viên mới");
                    Con.Close();
                    DisplayEmployees();
                    Clear();
                }
                catch(Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void EmployeesDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EmpNameTb.Text = EmployeesDGV.SelectedRows[0].Cells[1].Value.ToString();
            EmpAddTb.Text = EmployeesDGV.SelectedRows[0].Cells[2].Value.ToString();
            EmpDOBTb.Text = EmployeesDGV.SelectedRows[0].Cells[3].Value.ToString();
            EmpPhoneTb.Text = EmployeesDGV.SelectedRows[0].Cells[4].Value.ToString();
            EmpUsernameTb.Text = EmployeesDGV.SelectedRows[0].Cells[5].Value.ToString();
            EmpPasswordTb.Text = EmployeesDGV.SelectedRows[0].Cells[6].Value.ToString();
            if(EmpNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(EmployeesDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (EmpNameTb.Text == "" || EmpAddTb.Text == "" || EmpPhoneTb.Text == "" || EmpPasswordTb.Text == "" || EmpUsernameTb.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update EmployeeTable set EmpName = @EN, EmpAdd = @EA, EmpDOB = @ED, EmpPhone = @EP,EmpUser = @EU, EmpPass = @EPa where EmpNum=@EKey", Con);
                    cmd.Parameters.AddWithValue("@EN", EmpNameTb.Text);
                    cmd.Parameters.AddWithValue("@EA", EmpAddTb.Text);
                    cmd.Parameters.AddWithValue("@ED", EmpDOBTb.Value.Date);
                    cmd.Parameters.AddWithValue("@EP", EmpPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@EU", EmpUsernameTb.Text);
                    cmd.Parameters.AddWithValue("@EPa", EmpPasswordTb.Text);
                    cmd.Parameters.AddWithValue("@EKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã cập nhật thông tin nhân viên");
                    Con.Close();
                    DisplayEmployees();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Bạn chưa chọn nhân viên cần xoá");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from EmployeeTable where EmpNum = @EKey", Con);
                    cmd.Parameters.AddWithValue("@EKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã xoá thông tin nhân viên");
                    Con.Close();
                    DisplayEmployees();
                    Clear();
                    Key = 0;
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void HomeLabel_Click(object sender, EventArgs e)
        {
            Home Obj = new Home();
            Obj.Show();
            this.Hide();
        }
        private void ProductsLabel_Click(object sender, EventArgs e)
        {
            Products Obj = new Products();
            Obj.Show();
            this.Hide();
        }
        private void CustomersLabel_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
            Obj.Show();
            this.Hide();
        }
        private void BillingsLabel_Click(object sender, EventArgs e)
        {
            Billings Obj = new Billings();
            Obj.Show();
            this.Hide();
        }
        private void LogoutLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
