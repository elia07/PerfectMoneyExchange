using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core
{
    public partial class User
    {
        public string GetSerializedData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("xIsActive", this.xIsActive.ToString());
            data.Add("xDescription", this.xDescription);
            data.Add("xIsVip", this.xIsVip.ToString());
            data.Add("xIPGRestriction", this.xIPGRestriction.ToString());
            data.Add("xIsNationalIDValidated", this.xIsNationalIDValidated.ToString());
            data.Add("xNationalIDNumber", this.xNationalIDNumber);
            data.Add("xIsEmailValidated", this.xIsEmailValidated.ToString());
            data.Add("xCellphoneActivated", this.xCellphoneActivated.ToString());
            data.Add("xNationalIDImage", this.xNationalIDImage.ToString());
            

            return JsonConvert.SerializeObject(data);
        }
    }
}
