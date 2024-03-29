﻿using System;
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
        /// <param name="result">ログイン判定</param>
        /// <param name="usersId">入力ID</param>
        public void InsertLogHistory(int result, int usersId, DateTime buttonClickTime)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.ConnectionStr))
            {
                MySqlCommand command = InsertLogCommand(result, usersId, connection, buttonClickTime);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 入力IDのヒストリー取得メソッド
        /// </summary>
        /// <param name="usersId">入力ID</param>
        /// <returns>直近3回で失敗したヒストリーの降順リスト</returns>
        public List<DateTime> AcquisitionLog(int usersId)
        {
            List<DateTime> loginTimesList = new List<DateTime>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.ConnectionStr))
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
        /// <param name="result">ログイン判定</param>
        /// <param name="usersId">入力ID</param>
        /// <param name="connection"></param>
        /// <returns>コマンド</returns>
        private MySqlCommand InsertLogCommand(int result, int usersId, MySqlConnection connection, DateTime buttonClickTime)
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
            command.Parameters.AddWithValue("@LogTime", buttonClickTime);
            command.Parameters.AddWithValue("@LogResult", result);
            return command;
        }

        /// <summary>
        /// 入力IDで直近3件のヒストリー取得用コマンド
        /// </summary>
        /// <param name="usersId">入力ID</param>
        /// <param name="connection"></param>
        /// <returns>コマンド</returns>
        private MySqlCommand GetLogCommand(int usersId, MySqlConnection connection)
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
