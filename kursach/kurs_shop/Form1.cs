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
using System.Drawing.Printing;

namespace kurs_shop
{
    public partial class Anime_store : Form
    {

        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\ВУЗ\3 семестр учеба\Отправка Маше\kursach\kurs_shop\Database1.mdf;Integrated Security=True";
        private string sql = "SELECT * FROM Products";
        private SqlDataAdapter adapter { get; set; }
        private DataSet dataSet { get; set; }
        private SqlCommandBuilder commandBuilder { get; set; }
        private List<string> listOfProducts { get; set; }
        private string Print { get; set; }
        int j = 0;
        public Anime_store()
        {
            InitializeComponent();
            listOfProducts = new List<string>();
            dataSet = new DataSet("productsSet");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    adapter = new SqlDataAdapter(sql, connection);     

                    dataSet.Tables.Add(new DataTable("products"));
                    adapter.Fill(dataSet.Tables["products"]);
                    dataGridView1.DataSource = dataSet.Tables["products"];

                }

               for (int i = 0; i < dataSet.Tables["products"].Rows.Count; i++)
                    listOfProducts.Add(dataSet.Tables["products"].Rows[i]["name"].ToString() );
                products.DataSource = listOfProducts;//вывод на экран
            }

            catch (Exception ex)
            {
                MessageBox.Show("No Connection." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Работа кнопки сохранения БД
        private void save_Click(object sender, EventArgs e)
        {
            products.DataSource = null;
            listOfProducts.Clear();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    adapter = new SqlDataAdapter(sql, connection);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    adapter.Update(dataSet, "products");
                    MessageBox.Show("Table saved!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                for (int i = 0; i < dataSet.Tables["products"].Rows.Count; i++)
                    listOfProducts.Add(dataSet.Tables["products"].Rows[i]["name"].ToString());
                products.DataSource = listOfProducts;
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Data base has not been saved! Error code: " + ex.Message, "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        //Работа кнопки Добавить в таблицу БД
        private void add_Click(object sender, EventArgs e)
        {
            if (dataSet.Tables["products"].Rows.Count < 15)
            {
                listOfProducts.Clear();
                DataRow row = dataSet.Tables[0].NewRow();
                dataSet.Tables["products"].Rows.Add(row);
            }
            else
            {
                MessageBox.Show("The warehouse is full!!! To continue filling in the items, free up space in the warehouse.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //Работа кнопку Удалить из таблицы БД
        private void delete_Click(object sender, EventArgs e)
        {
           listOfProducts.Clear();
            try
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    dataGridView1.Rows.Remove(row);
                save_Click(null, null);

            }

            catch(Exception ex)
            {
                MessageBox.Show("Wrong action! Code error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Работа функции добавления в корзину
        private void check_Click(object sender, EventArgs e)
        {          
            string prod = products.SelectedItem.ToString();
            float quanty = Convert.ToSingle(quantity.Text.ToString());
            foreach (DataRow row in dataSet.Tables["products"].Rows)
                if (j == 0)
                {
                
                    if (row["name"].ToString() == prod)
                    {
                        paper.Text = 
                                 "Name_ _ _ _ _ _ _ _ _ _ _ _ " + row["name"] + "\n" +
                                 "Quantity_ _ _ _ _ _ _ _ _ _ _ " + quanty.ToString() + "\n" +
                                 "Price per one_ _ _ _ _ _ _ _ " + row["price"] + "\n" +
                        "Amount_ _ _ _ _ _ _ _ _ _ _ " + quanty * Convert.ToSingle(row["price"]) + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" +
                        "The cashier: " + "Egorova Emilia Tamaska"+ "\n" +
                        "Place of payment: " + "Anime_store" + "\n" +
                        "_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _" + "\n" +
                        "Date:                                     " + DateTime.Now.ToShortDateString() + "\n" +
                        "Time:                                     " + DateTime.Now.ToLongTimeString() + "\n\n" +
                        "Thank you for your purchase!";
                        
                    }
                    
                }
            foreach (DataRow row in dataSet.Tables["products"].Rows)              
                if (j == 1)
                {               
                    if (row["name"].ToString() == prod)
                    {
                        label2.Text = "Name_ _ _ _ _ _ _ _ _ _ _ _ " + row["name"] + "\n" +
                                 "Quantity_ _ _ _ _ _ _ _ _ _ _ " + quanty.ToString() + "\n" +
                                 "Price per one_ _ _ _ _ _ _ _ " + row["price"] + "\n" +
                        "Amount_ _ _ _ _ _ _ _ _ _ _ " + quanty * Convert.ToSingle(row["price"]);                       
                        
                    }
                }
            foreach (DataRow row in dataSet.Tables["products"].Rows)
                if (j == 2)
                {
                    if (row["name"].ToString() == prod)
                    {
                        label3.Text = "Name_ _ _ _ _ _ _ _ _ _ _ _ " + row["name"] + "\n" +
                                 "Quantity_ _ _ _ _ _ _ _ _ _ _ " + quanty.ToString() + "\n" +
                                 "Price per one_ _ _ _ _ _ _ _ " + row["price"] + "\n" +
                        "Amount_ _ _ _ _ _ _ _ _ _ _ " + quanty * Convert.ToSingle(row["price"]);                       
                    }
                }
                if (j > 2)
                {
                    MessageBox.Show("The shopping cart is full!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            print.Enabled = paper.Text != string.Empty;
        }

        private void save_cart (object sender, EventArgs e)
        {
            j++;
        }

        //Проверка на вводимость пустого поля
        private void quantity_TextChanged(object sender, EventArgs e)
        {
            float quanty = Convert.ToSingle(quantity.Text.ToString());
            if (quanty < 150)
             {
                if (quantity.Text != string.Empty)
                { 
                  check.Enabled = true;
                  button1.Enabled = true;
                }
             }
             else
             {
                MessageBox.Show("There are not so many items in stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                check.Enabled = false;
            }
        }

        //Проверка ввода с клавиатуры
        private void quantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',' && new string((quantity.Text + e.KeyChar).ToCharArray())[0] == ',')
            {
                e.Handled = true;
                return;
            }
            if (Char.IsControl(e.KeyChar))
                return;
            if (e.KeyChar == '.')
                e.KeyChar = ',';
            if (e.KeyChar.ToString() == string.Empty || int.TryParse(e.KeyChar.ToString(), out _) || e.KeyChar == ',')
                return;
            e.Handled = true;
        }

        //Работа кнопки Печать чека
        private void print_Click(object sender, EventArgs e)
        {
            Print = "   Cash register receipt"+ "\n" + label2.Text+ "\n" + label3.Text+ "\n" + paper.Text;
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += PrintPageHandler;
            PrintDialog printDialog = new PrintDialog()
            {
                Document = printDocument
            };
            if (printDialog.ShowDialog() == DialogResult.OK)
                printDialog.Document.Print();
        }


        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(Print, new Font("Arial", 14), Brushes.Black, 0, 0);

        }

        //Проверка значений вводимых в поле
        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.FormattedValue.ToString() == string.Empty)
                return;
            switch (e.ColumnIndex)
            {
                case 0:
                    if (!int.TryParse(e.FormattedValue.ToString(), out _))
                    {
                        e.Cancel = true;
                        return;
                    }

                    break;
                case 2:
                    if ((e.FormattedValue.ToString()!= "piece"))
                    {
                        e.Cancel = true;
                        return;
                    }
                    break;
                case 3:
                   if (!float.TryParse(e.FormattedValue.ToString(), out _))
                    {
                        e.Cancel = true;
                        return;
                    }
                    break;
                case 4:
                    if (!float.TryParse(e.FormattedValue.ToString(), out _))
                    {
                        e.Cancel = true;
                        return;
                    }
                    break;
                case 5:
                    if (!DateTime.TryParse(e.FormattedValue.ToString(), out _))
                    {
                        e.Cancel = true;
                        return;
                    }
                    break;
            }
        }

        //Вывод ошибки на дату
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if(e.Exception != null && e.Context == DataGridViewDataErrorContexts.Commit)
                MessageBox.Show("Incorrect data! Please reenter! Error code:"+e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void products_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The app < Anime Store> is a database of products in the store." +
                            "You can add new types of goods to the warehouse, as well as change them." +
                            "\n To add a new product, click on the <Add>button. To save the change after adding it , click on the < Save>button To delete <Delete>." +
                            "You can also select the product you want to purchase and print a receipt for its payment.",                           
                            " Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void оСоздателяхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The work was done by Andrey Kalmykov and Maria Sim.", " Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void информацияОЗаполненииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If the filling of the table is paused, you should double-check what exactly you" +
                " entered in the field.", " Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
