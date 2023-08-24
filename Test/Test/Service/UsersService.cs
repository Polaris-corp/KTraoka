using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class UsersService
    {
        /// <summary>
        /// IDがあるかの確認メソッド
        /// </summary>
        /// <param name="id">入力ID</param>
        /// <returns>IDがある場合はtrue、違う場合はfalse</returns>
        public bool UserIdExists(string id)
        {
            int idCount = -1;
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    var command = UserCountCommand(id, connection);
                    connection.Open();
                    idCount = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            if (idCount == 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// パスワードとIDが紐づいているか確認メソッド
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pwd"></param>
        /// <returns>紐付きの場合はtrue、違う場合はfalse</returns>
        public bool MatchUserExists(string id, string pwd)
        {
            int count = -1;
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    var command = MatchCountCommand(id, pwd, connection);
                    connection.Open();
                    count = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            if (count == 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// IDカウント用のコマンド
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private MySqlCommand UserCountCommand(string id, MySqlConnection connection)
        {
            string query = @"
                    SELECT
                        COUNT(*) 
                    FROM
                        loginusers 
                    WHERE
                        ID = @ID;";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);
            return command;
        }

        /// <summary>
        /// パスワードがIDと紐づいているかカウント用のコマンド
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pwd"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private MySqlCommand MatchCountCommand(string id, string pwd, MySqlConnection connection)
        {
            string query = @"
                    SELECT
                        COUNT(*) 
                    FROM
                        loginusers 
                    WHERE
                        ID = @ID 
                        AND Pwd = @pwd;";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);
            command.Parameters.AddWithValue("@Pwd", pwd);
            return command;
        }
    }
}
