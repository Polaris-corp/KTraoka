﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Service;
using Test.Common;

namespace Test.Controller
{
    public class Controller
    {
        UsersService usersService = new UsersService();
        HistoryService historyService = new HistoryService();

        public bool IsUserId(int id)
        {
            return usersService.UserIdExists(id);
        }

        public bool IsMatchUserPass(int id, string pwd)
        {
            return usersService.MatchUserExists(id, pwd);
        }

        public void UseInsertLogHistory(int result, int id, DateTime buttonClickTime)
        {
            historyService.InsertLogHistory(result, id, buttonClickTime);
        }

        public List<DateTime> LogTimesList(int id)
        {
            return historyService.AcquisitionLog(id);
        }

        /// <summary>
        /// 取得したリストが3回連続ミス、3分以内かのチェックメソッド
        /// </summary>
        /// <param name="loginTimesList">ログインヒストリーの降順リスト</param>
        public bool CheckLoginTime(List<DateTime> loginTimesList)
        {
            int count = loginTimesList.Count;

            if (count == ConstNums.FailureCount && (loginTimesList[0] - loginTimesList[2]).TotalMinutes <= ConstNums.JudgeTime)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 最後のミスから3分以内かのチェックメソッド
        /// </summary>
        /// <param name="loginTimesList">ログインヒストリーの降順リスト</param>
        /// <returns>3分以内の場合true、3分経過の場合はfalse</returns>
        public bool CheckThreeMinutes(List<DateTime> loginTimesList, DateTime buttonClickTime)
        {
            DateTime unlockTime = loginTimesList[0].AddMinutes(ConstNums.AddTime);
            return buttonClickTime < unlockTime;
        }
        /// <summary>
        /// ログインロックの残り時間計算メソッド
        /// </summary>
        /// <param name="loginTimesList">ログインヒストリーの降順リスト</param>
        /// <returns>残りロック時間</returns>
        public TimeSpan GetLockTime(List<DateTime> loginTimesList, DateTime buttonClickTime)
        {
            DateTime unlockTime = loginTimesList[0].AddMinutes(ConstNums.AddTime);
            TimeSpan remainingTime = unlockTime - buttonClickTime;
            return remainingTime;
        }

    }
}
