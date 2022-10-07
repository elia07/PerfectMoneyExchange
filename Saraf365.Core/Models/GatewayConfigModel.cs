using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.Models
{
    public class GatewayConfigModel
    {

        public string CardHolderName { set; get; }
        public string CardNumber { set; get; }
        public Dictionary<string, string> ChipsPack { set; get; }
        public IpgModel IpgConfig { set; get; }
    }

    public class IpgModel{
        public string ApiKey { set; get; }
        public string Url { set; get; }
        public string Callback { set; get; }
    }

}
