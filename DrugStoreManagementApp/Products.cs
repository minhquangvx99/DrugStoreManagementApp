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
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
            DisplayProducts();
        }
        SqlConnection Con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=C:\Users\Admin\DrugStoreManagementApp\DrugStoreDb.mdf;Integrated Security = True; Connect Timeout = 30");
        private void DisplayProducts()
        {
            Con.Open();
            string Query = "Select * from ProductTable";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProductsDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void Clear()
        {
            PrNameTb.Text = "";
            PrCategoryCb.Text = "";
            PrQuantityTb.Text = "";
            PrPriceTb.Text = "";
        }
        int Key = 0;
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (PrNameTb.Text == "" || PrCategoryCb.Text == "" || PrQuantityTb.Text == "" || PrPriceTb.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into ProductTable (PrName, PrCategory, PrQuantity, PrPrice) values (@PN, @PC, @PQ, @PP)", Con);
                    cmd.Parameters.AddWithValue("@PN", PrNameTb.Text);
                    cmd.Parameters.AddWithValue("@PC", PrCategoryCb.Text);
                    cmd.Parameters.AddWithValue("@PQ", PrQuantityTb.Text);
                    cmd.Parameters.AddWithValue("@PP", PrPriceTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã thêm thông tin sản phẩm mới");
                    Con.Close();
                    DisplayProducts();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void ProductsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            PrNameTb.Text = ProductsDGV.SelectedRows[0].Cells[1].Value.ToString();
            PrCategoryCb.Text = ProductsDGV.SelectedRows[0].Cells[2].Value.ToString();
            PrQuantityTb.Text = ProductsDGV.SelectedRows[0].Cells[3].Value.ToString();
            PrPriceTb.Text = ProductsDGV.SelectedRows[0].Cells[4].Value.ToString();
            if (PrNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (PrNameTb.Text == "" || PrCategoryCb.Text == "" || PrQuantityTb.Text == "" || PrPriceTb.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update ProductTable set PrName = @PN, PrCategory = @PC, PrQuantity = @PQ, PrPrice = @PP where PrId=@PKey", Con);
                    cmd.Parameters.AddWithValue("@PN", PrNameTb.Text);
                    cmd.Parameters.AddWithValue("@PC", PrCategoryCb.Text);
                    cmd.Parameters.AddWithValue("@PQ", PrQuantityTb.Text);
                    cmd.Parameters.AddWithValue("@PP", PrPriceTb.Text);
                    cmd.Parameters.AddWithValue("@PKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã cập nhật thông tin sản phẩm");
                    Con.Close();
                    DisplayProducts();
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
                MessageBox.Show("Bạn chưa chọn sản phẩm cần xoá");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from ProductTable where PrId = @PKey", Con);
                    cmd.Parameters.AddWithValue("@PKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã xoá thông tin sản phẩm");
                    Con.Close();
                    DisplayProducts();
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
