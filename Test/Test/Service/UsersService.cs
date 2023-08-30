using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Common;

namespace Test.Service
{
    public class UsersService
    {
        /// <summary>
        /// IDがあるかの確認メソッド
        /// </summary>
        /// <param name="id">入力ID</param>
        /// <returns>IDがある場合はtrue、ない場合はfalse</returns>
        public bool UserIdExists(string id)
        {
            int idCount = -1;
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.ConnectionStr))
            {
                var command = UserIdCountCommand(id, connection);
                connection.Open();
                idCount = Convert.ToInt32(command.ExecuteScalar());
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
        /// <param name="id">入力ID</param>
        /// <param name="pwd">入力Pwd</param>
        /// <returns>紐付きの場合はtrue、違う場合はfalse</returns>
        public bool MatchUserExists(string id, string pwd)
        {
            int count = -1;
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.ConnectionStr))
            {
                var command = MatchCountCommand(id, pwd, connection);
                connection.Open();
                count = Convert.ToInt32(command.ExecuteScalar());
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
        /// <param name="id">入力ID</param>
        /// <param name="connection"></param>
        /// <returns>コマンド</returns>
        private MySqlCommand UserIdCountCommand(string id, MySqlConnection connection)
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
        /// <param name="id">入力ID</param>
        /// <param name="pwd">入力Pwd</param>
        /// <param name="connection"></param>
        /// <returns>コマンド</returns>
        private MySqlCommand MatchCountCommand(string id, string pwd, MySqlConnection connection)
        {
            string query = @"
                    SELECT
                        COUNT(*) 
                    FROM
                        loginusers 
                    WHERE
                        ID = @ID 
                        AND Pwd = @Pwd;";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);
            command.Parameters.AddWithValue("@Pwd", pwd);
            return command;
        }
    }
}
