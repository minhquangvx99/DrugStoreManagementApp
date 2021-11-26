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
    public partial class Billings : Form
    {
        public Billings()
        {
            InitializeComponent();
            //EmpNameLabel.Text = Login.UserNameTb;
            GetCustomers();
            DisplayProducts();
            DisplayTransactions();
        }
        SqlConnection Con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=C:\Users\Admin\DrugStoreManagementApp\DrugStoreDb.mdf;Integrated Security = True; Connect Timeout = 30");
        
        private void GetCustomers()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("Select CusNum from CustomerTable", Con);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CusNum", typeof(int));
            dt.Load(Rdr);
            CusNumCb.ValueMember = "CusNum";
            CusNumCb.DataSource = dt;
            Con.Close();
        }
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
        private void DisplayTransactions()
        {
            Con.Open();
            string Query = "Select * from BillTable";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            TransactionsDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void GetCusName()
        {
            Con.Open();
            string Query = "Select * from CustomerTable where CusNum =" + CusNumCb.SelectedValue.ToString();
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                CusNameTb.Text = dr["CusName"].ToString();
            }
            Con.Close();
        }

        private void UpdateStock()
        {
            try
            {
                int NewQuantity = Stock - Convert.ToInt32(PrQuantityTb.Text);
                Con.Open();
                SqlCommand cmd = new SqlCommand("Update ProductTable set PrQuantity=@PQ where PrId=@PKey", Con);
                cmd.Parameters.AddWithValue("@PQ", NewQuantity);
                cmd.Parameters.AddWithValue("@PKey", Key);
                cmd.ExecuteNonQuery();
                Con.Close();
                DisplayProducts();
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
        int n = 0, GrdTotal = 0;

        private void AddToBillBtn_Click(object sender, EventArgs e)
        {
            if(PrQuantityTb.Text == "" || Convert.ToInt32(PrQuantityTb.Text) > Stock)
            {
                MessageBox.Show("Không đủ sản phẩm");
            }
            else if(PrQuantityTb.Text == "" || Key == 0)
            {
                MessageBox.Show("Bạn nhập thiếu thông tin");
            }
            else
            {
                int total = Convert.ToInt32(PrQuantityTb.Text) * Convert.ToInt32(PrPriceTb.Text);
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(BillDGV);
                newRow.Cells[0].Value = n + 1;
                newRow.Cells[1].Value = PrNameTb.Text;
                newRow.Cells[2].Value = PrPriceTb.Text;
                newRow.Cells[3].Value = PrQuantityTb.Text;
                newRow.Cells[4].Value = total;
                GrdTotal = GrdTotal + total;
                BillDGV.Rows.Add(newRow);
                n++;
                TotalLabel.Text = "Tổng chi phí:" + GrdTotal;
                UpdateStock();
                Reset();
            }
        }
        private void CusNumCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetCusName();
        }

        int Key = 0, Stock = 0;

        private void Reset()
        {
            PrNameTb.Text = "";
            PrPriceTb.Text = "";
            PrQuantityTb.Text = "";
            Stock = 0;
            Key = 0;
        }
        String productName;
        int productId, productQuantity, productPrice, total, pos = 60;
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("My Drug Store", new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Red, new Point(80));
            e.Graphics.DrawString("ID PRODUCT PRICE QUANTITY TOTAL", new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Red, new Point(26, 40));
            foreach(DataGridViewRow row in BillDGV.Rows)
            {
                productId = Convert.ToInt32(row.Cells["ID"].Value);
                productName = "" + row.Cells["Product"].Value;
                productPrice = Convert.ToInt32(row.Cells["Price"].Value);
                productQuantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                total = Convert.ToInt32(row.Cells["Amount"].Value);
                e.Graphics.DrawString(productId.ToString(), new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(26, pos));
                e.Graphics.DrawString(productName.ToString(), new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(45, pos));
                e.Graphics.DrawString(productPrice.ToString(), new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(120, pos));
                e.Graphics.DrawString(productQuantity.ToString(), new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(170, pos));
                e.Graphics.DrawString(total.ToString(), new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(235, pos));
                pos = pos + 20;
            }
            e.Graphics.DrawString("Tổng chi phí: " + GrdTotal, new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Crimson, new Point(50, pos + 50));
            e.Graphics.DrawString("************DrugStore************", new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Crimson, new Point(10, pos + 85));
            BillDGV.Rows.Clear();
            BillDGV.Refresh();
            pos = 100;
            GrdTotal = 0;
            n = 0;
        }

        private void InsertBill()
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("insert into BillTable (BillDate, CusId, CusName, EmpName, Amount) values (@BD,@CI,@CN,@EN,@A)", Con);
                cmd.Parameters.AddWithValue("@BD", DateTime.Today.Date);
                cmd.Parameters.AddWithValue("@CI", CusNumCb.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@CN", CusNameTb.Text);
                cmd.Parameters.AddWithValue("@EN", EmployeesLabel.Text);
                cmd.Parameters.AddWithValue("@A", GrdTotal);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Đã lưu hoá đơn");
                Con.Close();
                DisplayTransactions();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
        private void PrintBtn_Click(object sender, EventArgs e)
        {
            InsertBill();
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 285, 600);
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void ProductsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            PrNameTb.Text = ProductsDGV.SelectedRows[0].Cells[1].Value.ToString();
            Stock = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[3].Value.ToString());
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

        private void LogoutLabel_Click(object sender, EventArgs e)
        {

        }

    }
}
