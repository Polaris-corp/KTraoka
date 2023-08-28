using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Test.Common;

namespace Test.Service
{
    public class HistoryService
    {
        /// <summary>
        /// ログインヒストリーにインサートするメソッド
        /// </summary>
        /// <param name="result"></param>
        /// <param name="usersId"></param>
        public void InsertLogHistory(int result, string usersId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.connectionString))
            {
                MySqlCommand command = InsertLogCommand(result, usersId, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        public List<DateTime> AcquisitionLog(string usersId)
        {
            List<DateTime> loginTimesList = new List<DateTime>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.connectionString))
            {
                var command = GetLogCommand(usersId, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int logResult = Convert.ToInt32(reader["LogResult"]);
                    if (logResult == 0)
                    {
                        DateTime logTime = Convert.ToDateTime(reader["LogTime"]);
                        loginTimesList.Add(logTime);
                    }
                }
                return loginTimesList;
            }
        }

        /// <summary>
        /// ログインヒストリーインサート用のコマンド
        /// </summary>
        /// <param name="result"></param>
        /// <param name="usersId"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private MySqlCommand InsertLogCommand(int result, string usersId, MySqlConnection connection)
        {

            string query = @"
                    INSERT INTO 
                        LoginHistory
                        (
                        UsersID
                        ,LogTime
                        ,LogResult
                        )
                    VALUES
                        (
                        @UsersID
                        , @LogTime
                        , @LogResult
                        );";


            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@UsersID", usersId);
            command.Parameters.AddWithValue("@LogTime", DateTime.Now);
            command.Parameters.AddWithValue("@LogResult", result);
            return command;
        }

        /// <summary>
        /// 入力IDでヒストリー直近3件の取得用コマンド
        /// </summary>
        /// <param name="usersId"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private MySqlCommand GetLogCommand(string usersId, MySqlConnection connection)
        {
            string query = @"
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
                        3;" ;

            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@UsersID", usersId);
            return command;
        }
    }
}
