using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Service;

namespace Test.Controller
{
    public class Controller
    {
        UsersService usersService = new UsersService();
        HistoryService historyService = new HistoryService();

        public bool IsUserId(string id)
        {
            return usersService.UserIdExists(id);
        }

        public bool IsMatchUserPass(string id, string pwd)
        {
            return usersService.MatchUserExists(id, pwd);
        }

        public void UseInsertLogHistory(int result, string id)
        {
            historyService.InsertLogHistory(result, id);
        }

        public List<DateTime> LogTimesList(string id)
        {
            return historyService.AcquisitionLog(id);
        }

    }
}
