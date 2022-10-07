using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.InternetBankModules
{
    public class AntiCaptchaCheckStatusResponse
    {
        public int errorId { set; get; }
        public string status { set; get; }
        public Solution solution { set; get; }
        public string cost { set; get; }
        public string ip { set; get; }
        public string createTime { set; get; }
        public string endTime { set; get; }
        public int solveCount { set; get; }
    }
    public class Solution
    {
        public string text { set; get; }
        public string url { set; get; }
    }
}
