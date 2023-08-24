using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace Test
{
    class HistoryService
    {
        /// <summary>
        /// ログインヒストリーにインサートするメソッド
        /// </summary>
        /// <param name="result"></param>
        /// <param name="usersId"></param>
        public void LogInsertHistory(int result, string usersId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    MySqlCommand command = LogInsertCommand(result, usersId, connection);
                    connection.Open();
                    command.ExecuteReader();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        public List<DateTime> LogAcquisition(string usersId)
        {
            List<DateTime> loginTimesList = new List<DateTime>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    var command = GetLogCommand(usersId, connection);
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
                catch (Exception ex)
                {
                    throw;
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
        private MySqlCommand LogInsertCommand(int result, string usersId, MySqlConnection connection)
        {

            StringBuilder query = new StringBuilder();
            query.Append("INSERT INTO LoginHistory ");
            query.Append("(UsersID, ");
            query.Append("LogTime, ");
            query.Append("LogResult) ");
            query.Append("VALUES  ");
            query.Append("(@UsersID, ");
            query.Append("@LogTime, ");
            query.Append("@LogResult)");

            MySqlCommand command = new MySqlCommand(query.ToString(), connection);
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
            StringBuilder query = new StringBuilder();
            query.Append("SELECT ");
            query.Append("LogTime, ");
            query.Append("LogResult ");
            query.Append("FROM LoginHistory ");
            query.Append("WHERE UsersID = ");
            query.Append("@UsersID ");
            query.Append("ORDER BY ");
            query.Append("LogTime DESC ");
            query.Append("LIMIT 3");

            MySqlCommand command = new MySqlCommand(query.ToString(), connection);
            command.Parameters.AddWithValue("@UsersID", usersId);
            return command;
        }
    }
}
