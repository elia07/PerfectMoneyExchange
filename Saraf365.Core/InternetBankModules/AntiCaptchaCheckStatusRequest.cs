using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.InternetBankModules
{
    public class AntiCaptchaCheckStatusRequest
    {
        public string clientKey { set; get; }
        public int taskId { set; get; }

        public AntiCaptchaCheckStatusRequest() { }
        public AntiCaptchaCheckStatusRequest(string clientKey,int taskId) {
            this.clientKey = clientKey;
            this.taskId = taskId;
        }
    }
}
