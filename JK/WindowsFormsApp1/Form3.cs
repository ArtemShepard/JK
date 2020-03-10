using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        SqlConnection conn = new SqlConnection("data Source = HI-TECH; initial catalog = practica_2; integrated Security = true;");

        public string JKname = "";

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            string status = "";
            int JKid = 0;
            conn.Open();
            SqlCommand com = new SqlCommand($"SELECT [Название ЖК], [Затраты на строительство ЖК], Город, [Добавочная стоимость ЖК], [Статус строительства ЖК], id FROM houses_in_complexes WHERE [Название ЖК] = '{JKname}'", conn);
            SqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                textBox1.Text = dr[0].ToString();
                textBox3.Text = dr[1].ToString();
                textBox2.Text = dr[2].ToString();
                textBox4.Text = dr[3].ToString();
                status = dr[4].ToString();
                JKid = Convert.ToInt16(dr[5]);
            }
            dr.Close();

            com = new SqlCommand($"SELECT [apartments_in_houses].[Номер квартиры] FROM [apartments_in_houses], [houses_in_complexes] WHERE [houses_in_complexes].[id] = '{JKid}' AND [apartments_in_houses].[ИД Дома] = [houses_in_complexes].[id] AND [apartments_in_houses].[Статус продажи] = 'sold'", conn);
            dr = com.ExecuteReader();
            if (dr.HasRows)
                foreach (RadioButton rb in groupBox1.Controls)
                {
                    if (rb.Text == status) rb.Checked = true;
                    if (rb.Text == "plan") rb.Enabled = false;
                }

            dr.Close();
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label5.Text = "";
            string name = "";
            string city = "";
            string plan = "";
            int cost = 0;
            int complex = 0;

            if (textBox1.Text.Length > 0) name = textBox1.Text;
            else label5.Text += "Укажите название ЖК\n";
            if (textBox2.Text.Length > 0) city = textBox2.Text;
            else label5.Text += "Укажите город в котором расположен ЖК\n";

            if (System.Text.RegularExpressions.Regex.IsMatch(textBox3.Text, @"[^\d]"))
                label5.Text += "Затраты на строительство должны быть указаны в виде числа\n";
            else
            {
                if (textBox3.Text.Length > 0 && Convert.ToInt32(textBox3.Text) > 0) cost = Convert.ToInt32(textBox3.Text);
                else label5.Text += "Затраты на строительство должны быть больше 0\n";
            }

            if (Regex.IsMatch(textBox4.Text, @"[^\d]"))
                label5.Text += "Коэф. добавочной стоимости должен быть указан в виде числа\n";
            else
            {
                if (textBox4.Text.Length > 0 && Convert.ToInt32(textBox4.Text) > 0) complex = Convert.ToInt32(textBox4.Text);
                else label5.Text += "Коэф. добавочной стоимости должен быть больше 0\n";
            }

            foreach (RadioButton rb in groupBox1.Controls)
                if (rb.Checked == true) plan = rb.Text;

            if (label5.Text.Length == 0)
            {
                conn.Open();
                SqlCommand com = new SqlCommand($"INSERT INTO houses_in_complexes ([Название ЖК], [Затраты на строительство ЖК], Город, [Добавочная стоимость ЖК], [Статус строительства ЖК]) VALUES ('{name}', '{cost}', '{city}', '{complex}', '{plan}')", conn);
                if (com.ExecuteNonQuery() > 0) MessageBox.Show("Запись о жилищном комплексе добавлена.");
                else MessageBox.Show("Возникла ошибка при добавлении.");

                Form1 jk = (Form1)Application.OpenForms["Form1"];
                jk.dataGridView1.Rows.Clear();
                com = new SqlCommand("select [houses_in_complexes].[Название ЖК],[houses_in_complexes].[Статус строительства ЖК],(select COUNT([id]) from [practica_2].[dbo].[houses_in_complexes] where [houses_in_complexes].[id] = [houses_in_complexes].[id]),[houses_in_complexes].[Город] FROM [practica_2].[dbo].[houses_in_complexes]", conn);
                SqlDataReader dr = com.ExecuteReader();
                jk.dataGridView1.Rows.Clear();
                while (dr.Read())
                    jk.dataGridView1.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3]);
                conn.Close();
            }
        }
    }
}
