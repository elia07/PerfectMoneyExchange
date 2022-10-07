using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.Models
{
    public class RefferalHistoryPerUserAndDateModel
    {
        public string Username { set; get; }
        public Dictionary<DateTime, long> Records;

        public RefferalHistoryPerUserAndDateModel(string username)
        {
            Username = username;
            Records = new Dictionary<DateTime, long>();


        }
    }
}
