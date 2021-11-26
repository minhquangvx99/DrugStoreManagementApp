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
    public partial class Customers : Form
    {
        public Customers()
        {
            InitializeComponent();
            DisplayCustomers();
        }
        SqlConnection Con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=C:\Users\Admin\DrugStoreManagementApp\DrugStoreDb.mdf;Integrated Security = True; Connect Timeout = 30");

        private void DisplayCustomers()
        {
            Con.Open();
            string Query = "Select * from CustomerTable";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            CustomersDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void Clear()
        {
            CusNameTb.Text = "";
            CusAddTb.Text = "";
            CusPhoneTb.Text = "";
        }
        int Key = 0;

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (CusNameTb.Text == "" || CusAddTb.Text == "" || CusPhoneTb.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into CustomerTable (CusName, CusAdd, CusPhone) values (@CN, @CA, @CP)", Con);
                    cmd.Parameters.AddWithValue("@CN", CusNameTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CusAddTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CusPhoneTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã thêm thông tin khách hàng mới");
                    Con.Close();
                    DisplayCustomers();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void CustomersDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CusNameTb.Text = CustomersDGV.SelectedRows[0].Cells[1].Value.ToString();
            CusAddTb.Text = CustomersDGV.SelectedRows[0].Cells[2].Value.ToString();
            CusPhoneTb.Text = CustomersDGV.SelectedRows[0].Cells[3].Value.ToString();
            if (CusNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(CustomersDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (CusNameTb.Text == "" || CusAddTb.Text == "" || CusPhoneTb.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update CustomerTable set CusName = @CN, CusAdd = @CA, CusPhone = @CP where CusNum=@CKey", Con);
                    cmd.Parameters.AddWithValue("@CN", CusNameTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CusAddTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CusPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@CKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã cập nhật thông tin khách hàng");
                    Con.Close();
                    DisplayCustomers();
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
                MessageBox.Show("Bạn chưa chọn khách hàng cần xoá");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from CustomerTable where CusNum = @CKey", Con);
                    cmd.Parameters.AddWithValue("@CKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã xoá thông tin khách hàng");
                    Con.Close();
                    DisplayCustomers();
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
        private void EmployeesLabel_Click(object sender, EventArgs e)
        {
            Employees Obj = new Employees();
            Obj.Show();
            this.Hide();
        }
        private void ProductsLabel_Click(object sender, EventArgs e)
        {
            Products Obj = new Products();
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
