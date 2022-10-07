using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core
{
    public partial class Gateway
    {
        public string GetSerializedData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("xActiveforLevelsAbove", this.xActiveforLevelsAbove.ToString());
            data.Add("xAllowCustomAmountForBuyIn", this.xAllowCustomAmountForBuyIn.ToString());
            data.Add("xCommisionPercent", this.xCommisionPercent.ToString());
            data.Add("xConfig", this.xConfig);
            data.Add("xIsActive", this.xIsActive.ToString());
            data.Add("xIsPrimary", this.xIsPrimary.ToString());
            data.Add("xIsVip", this.xIsVip.ToString());
            data.Add("xMaxDailyAmount", this.xMaxDailyAmount.ToString());
            data.Add("xMinBuyIn", this.xMinBuyIn.ToString());
            data.Add("xName", this.xName);
            data.Add("xTodayTotalTransactionAmounts", this.xTodayTotalTransactionAmounts.ToString());
            data.Add("xType", this.xType.ToString());
            
            return JsonConvert.SerializeObject(data);
        }
    }
}
