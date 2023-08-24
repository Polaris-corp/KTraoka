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

        public bool IsUserId(string id)
        {
            return usersService.UserIdExists(id);
        }

        public bool IsMatchUserPass(string id, string pwd)
        {
            return usersService.MatchUserExists(id, pwd);
        }

        public void CallInsertLogHistory(int result, string id)
        {
            historyService.InsertLogHistory(result, id);
        }

        public List<DateTime> LogTimesList(string id)
        {
            return historyService.AcquisitionLog(id);
        }

    }
}
