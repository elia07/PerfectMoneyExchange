using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core
{
    public partial class Withdrawal
    {
        public string GetSerializedData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("xStatus", this.xStatus.ToString());
            data.Add("xDescription", this.xDescription);
           

            return JsonConvert.SerializeObject(data);
        }
    }
}
