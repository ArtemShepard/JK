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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
      //SqlConnection conn = new SqlConnection("data Source = 303-13; initial catalog = BD; integrated Security = true;");
        SqlConnection conn = new SqlConnection("data Source = HI-TECH; initial catalog = practica_2; integrated Security = true;");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn.Open();

            SqlCommand com = new SqlCommand("select [houses_in_complexes].[Название ЖК],[houses_in_complexes].[Статус строительства ЖК],(select COUNT([id]) from [practica_2].[dbo].[houses_in_complexes] where [houses_in_complexes].[id] = [houses_in_complexes].[id]),[houses_in_complexes].[Город] FROM [practica_2].[dbo].[houses_in_complexes]", conn);
            SqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
                dataGridView1.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3]);

            dr.Close();
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 newJK = new Form2();
            newJK.Show();
        }
    }
}
