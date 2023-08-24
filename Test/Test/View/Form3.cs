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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        string connectionString = "Server = localhost;Database = test;Uid = root;Pwd = 1105";

        Controller controller = new Controller();

        private void button1_Click(object sender, EventArgs e)
        {
            //IDとPwdを受け取る
            string id = textBox1.Text;
            string pwd = textBox2.Text;

            //IDとPwd入力チェック
            if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(pwd))
            {
                MessageBox.Show(Message.nullUserPass);
                return;
            }

            //IDの存在チェック
            if (!controller.IsUserId(id))
            {
                MessageBox.Show(Message.notUser);
                return;
            }

            //IDとPwdのひもづきデータのチェック
            if (!controller.IsMatchUserPass(id, pwd))
            {
                MessageBox.Show(Message.differentPass);
                controller.CallInsertLogHistory(0, id);
                return;
            }

            List<DateTime> loginTimesList = LogAcquisition(id);
            CheckLoginTime(loginTimesList, id);

        }

        
        /// <summary>
        /// 入力IDのヒストリー受け取りメソッド
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        private List<DateTime> LogAcquisition(string usersId)
        {
            //入力IDのヒストリー直近3件の受け取り
            string sql = @"
                SELECT
                    LogTime
                    , LogResult 
                FROM
                    LoginHistory 
                WHERE
                    UsersID = @UsersID 
                ORDER BY
                    LogTime DESC 
                LIMIT
                    3";


            List<DateTime> loginTimesList = new List<DateTime>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@UsersID", usersId);
                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            DateTime logTime = Convert.ToDateTime(reader["LogTime"]);
                            int logResult = Convert.ToInt32(reader["LogResult"]);
                            if (logResult == 0)
                            {
                                loginTimesList.Add(logTime);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return loginTimesList;
            }
        }
        /// <summary>
        /// 入力IDの取得ヒストリーのチェック
        /// </summary>
        /// <param name="loginTimesList"></param>
        /// <param name="usersId"></param>
        private void CheckLoginTime(List<DateTime> loginTimesList, string usersId)
        {
            //チェック
            int count = loginTimesList.Count;

            if (count == 3 && (loginTimesList[0] - loginTimesList[2]).TotalMinutes <= 3)
            {
                DateTime unlockTime = loginTimesList[0].AddMinutes(3);
                if (DateTime.Now < unlockTime)
                {
                    TimeSpan remainingTime = unlockTime - DateTime.Now;
                    MessageBox.Show("残りロック時間: " + remainingTime.ToString(@"mm\:ss"));
                    return;
                }
            }
            MessageBox.Show("ログイン成功");
            controller.CallInsertLogHistory(1, usersId);
        }
    }
}


