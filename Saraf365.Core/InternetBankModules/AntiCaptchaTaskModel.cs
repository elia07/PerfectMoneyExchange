using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.InternetBankModules
{
    public class AntiCaptchaTaskModel
    {
        public string clientKey { set; get; }
        public task task { set; get; }

        public AntiCaptchaTaskModel()
        {

        }
        public AntiCaptchaTaskModel(string clientKey,string type,string body,bool phrase,bool Case,bool numeric,int math,int minLength, int maxLength)
        {
            this.clientKey = clientKey;
            this.task = new task(type, body, phrase, Case, numeric, math, minLength, maxLength);
        }
    }

    public class task
    {

        public string type { set; get; }
        public string body { set; get; }
        public bool phrase { set; get; }
        public bool Case { set; get; }
        public bool numeric { set; get; }
        public int math { set; get; }
        public int minLength { set; get; }
        public int maxLength { set; get; }

        public task() { }
        public task(string type, string body, bool phrase, bool Case, bool numeric, int math, int minLength, int maxLength) {

            this.type = type;
            this.body = body;
            this.phrase = phrase;
            this.Case = Case;
            this.numeric = numeric;
            this.math = math;
            this.minLength = minLength;
            this.maxLength = maxLength;
        }
    }
}
