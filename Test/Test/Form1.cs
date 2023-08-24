using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(textBox1.Text);
            form2.ShowDialog();
            //form2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string txt = textBox1.Text;
            string server = "localhost";
            string database = "test";
            string user = "root";
            string pass = "1105";
            string connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", server, database, user, pass);

            string sql = "SELECT Name FROM testtable WHERE ID = " + txt;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    bool ok = true;
                    while (reader.Read())
                    {
                        ok = false;
                        string name = reader["Name"].ToString();
                        if(!String.IsNullOrEmpty(name))
                        {
                            MessageBox.Show(name);
                        }
                        else
                        {
                            MessageBox.Show("名前がありません");
                        }
                    }
                    if (ok)
                    {
                        MessageBox.Show("データがありません");
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string txt = textBox1.Text;
            string server = "localhost";
            string database = "test";
            string user = "root";
            string pass = "1105";
            string connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", server, database, user, pass);

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append("a.Name AS abc ");
            sql.Append("FROM ");
            sql.Append("mstusers AS u ");
            sql.Append("LEFT OUTER JOIN mstauthority AS a ");
            sql.Append("ON u.Authority_id = a.ID ");
            sql.Append("WHERE u.ID = " + txt);
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(sql.ToString(), connection))
                    using (var reader = command.ExecuteReader())
                    {
                        bool ok = true;
                        while (reader.Read())
                        {
                            ok = false;
                            string name = reader["abc"].ToString();
                            if (!String.IsNullOrEmpty(name))
                            {
                                MessageBox.Show(name);
                            }
                            else
                            {
                                MessageBox.Show("データがありません");
                            }
                        }
                        if (ok)
                        {
                            MessageBox.Show("いません");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string txt = textBox1.Text;
            string server = "localhost";
            string database = "test";
            string user = "root";
            string pass = "1105";
            string connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", server, database, user, pass);

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append("a.Name AS n ");
            sql.Append("FROM ");
            sql.Append("mstauthority AS a ");
            sql.Append("WHERE a.ID = ");
            sql.Append("(SELECT u.Authority_id ");
            sql.Append("FROM mstusers AS u ");
            sql.Append("WHERE u.ID = ");
            sql.Append(txt);
            sql.Append(");");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(sql.ToString(), connection))
                    using (var reader = command.ExecuteReader())
                    {
                        bool ok = true;
                        while (reader.Read())
                        {
                            ok = false;
                            string name = reader["n"].ToString();
                            if (!String.IsNullOrEmpty(name))
                            {
                                MessageBox.Show(name);
                            }
                            else
                            {
                                MessageBox.Show("データがありません");
                            }
                        }
                        if (ok)
                        {
                            MessageBox.Show("いません");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
