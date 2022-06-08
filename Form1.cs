using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;



namespace Himus_2._0
{
    public partial class HIMUS : Form
    {

        private SqlConnection sqlConnection = null;

        public HIMUS()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)//подклюяение базы данных
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["HimusDB"].ConnectionString);
            sqlConnection.Open();

            ListViewCompletion();

            textBox_ProductName.Text = "";
        }

        private void button1_Click(object sender, EventArgs e) //метод добавления продукта (ссылка на объект, объект)
        {
            string sqlString = $"INSERT INTO [Products] (ProductName, Protein, Fat, Carbohydrates) VALUES (@ProductName, @Protein, @Fat, @Carbohydrates)";


            SqlCommand command = new SqlCommand(sqlString, sqlConnection);

            if (textBox1.Text != "")
                command.Parameters.AddWithValue("ProductName", textBox1.Text);
            else
            {
                MessageBox.Show("Нужно название продука", "Ошибка");
                return;
            }

            if (textBox2.Text != "")
            {
                command.Parameters.AddWithValue("Protein", textBox2.Text);
            }
            else
            {
                MessageBox.Show("Параметр белка неверен", "Ошибка");
                return;
            }

            if (textBox3.Text != "")
                command.Parameters.AddWithValue("Fat", textBox3.Text);
            else
            {
                MessageBox.Show("Параметр жира неверен", "Ошибка");
                return;
            }

            if (textBox4.Text != "")
                command.Parameters.AddWithValue("Carbohydrates", textBox4.Text);
            else
            {
                MessageBox.Show("Параметр углеводов неверен", "Ошибка");
                return;
            }
            command.ExecuteNonQuery();
            MessageBox.Show("Продукт добавлен в базу","Успешно");

            ListViewCompletion();
        }

        private void button2_Click(object sender, EventArgs e)//кнопка очистки в модуле добавления
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void TextAdd()//заполнение листбокса из выбранных продуктов
        {
        
            ListViewItem viewItem = (ListViewItem)listView2.SelectedItems[0].Clone();///////
            viewItem.SubItems.Add(textBox_Mass.Text);
            listView1.Items.Add(viewItem);

        }

        private void ListViewCompletion()//заполнение листвью  
        {
            listView2.Items.Clear();
            SqlDataReader datareader = null;

            try
            {
                SqlCommand ListCommand = new SqlCommand("SELECT ProductName, Protein, Fat, Carbohydrates  FROM Products", sqlConnection);
                datareader = ListCommand.ExecuteReader();

                ListViewItem viewItem = null;

                while(datareader.Read())
                {
                    viewItem = new ListViewItem(new String[] { Convert.ToString(datareader["ProductName"]),
                            Convert.ToString(datareader["Protein"]),
                            Convert.ToString(datareader["Fat"]),
                            Convert.ToString(datareader["Carbohydrates"]) });

                    listView2.Items.Add(viewItem);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if(datareader != null && !datareader.IsClosed)
                    datareader.Close();
            }   
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)//заполнение текст боксов в соответствии с выбранной позицией в листбокс
        {
            if(listView2.SelectedItems.Count > 0)
            {
                ListViewItem viewItem = listView2.SelectedItems[0];
                textBox_ProductName.Text = viewItem.SubItems[0].Text;
                textBox_Protein.Text = viewItem.SubItems[1].Text;
                textBox_Fat.Text = viewItem.SubItems[2].Text;
                textBox_Carbohydrates.Text = viewItem.SubItems[3].Text;
            }

            else
            {
                textBox_ProductName.Text = string.Empty;
                textBox_Protein.Text = string.Empty;
                textBox_Fat.Text = string.Empty;
                textBox_Carbohydrates.Text = string.Empty;
            }
        }

        private void button3_Click(object sender, EventArgs e)//добавление в листвью
        {
            if(textBox_Mass.Text != "") { }
            else
            {
                MessageBox.Show("Ошибка! Введите число.");
                return;
            }
            TextAdd();
            textBox_Mass.Clear();
        }

        private void button5_Click(object sender, EventArgs e)//очистка листвью
        {
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)//расчет
        {
            double one = 0;
            double two = 0;
            double three = 0;
            

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                double m = Convert.ToDouble(listView1.Items[i].SubItems[4].Text.Replace('.', ','));
                one += Calculation.calculationProtein(Convert.ToDouble(listView1.Items[i].SubItems[1].Text.Replace('.',',')),m);
                two += Calculation.calculationFat(Convert.ToDouble(listView1.Items[i].SubItems[2].Text.Replace('.', ',')), m);
                three += Calculation.calculationCarbohydrates(Convert.ToDouble(listView1.Items[i].SubItems[3].Text.Replace('.', ',')), m);

            }
            
            textBox6.Text = Convert.ToString(one);
            textBox7.Text = Convert.ToString(two);
            textBox8.Text = Convert.ToString(three);

        }
    }
}
