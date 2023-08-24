using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Controller
    {
        UsersService usersService = new UsersService();
        HistoryService historyService = new HistoryService();

        public bool UsersId(string id)
        {
            return usersService.GetUsersId(id);
        }

        public bool MatchUsersPass(string id, string pwd)
        {
            return usersService.GetMatchUsers(id, pwd);
        }

        public void CallLogInsertHistory(int result, string id)
        {
            historyService.LogInsertHistory(result, id);
        }

        public List<DateTime> LogTimesList(string id)
        {
            return historyService.LogAcquisition(id);
        }

    }
}
