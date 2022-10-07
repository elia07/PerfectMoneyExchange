using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.InternetBankModules
{
    public class InternetBankFactory
    {
        public IInternetBank GetInstance(BankCard instance,string apiKey)
        {
            switch(instance.xBankName)
            {
                case "دی":
                    return new BankDeyInternetBank(instance.xInternetUsername, instance.xInternetPassword, instance.xAccountNumber, instance.xCardNumber, apiKey, instance.xSecondPassword, instance.xCvv2, instance.xExpireMonth, instance.xExpireYear);
                case "تجارت":
                    Dictionary<string, string> specialConfigs = JsonConvert.DeserializeObject<Dictionary<string, string>>(instance.xSpecialConfigs);
                    return new BankTejaratInternetBank(instance.xInternetUsername, instance.xInternetPassword, instance.xAccountNumber, instance.xCardNumber, specialConfigs["pin2"], apiKey,instance.xSecondPassword,instance.xCvv2,instance.xExpireMonth,instance.xExpireYear);
                default:
                    return null;
            }
        }
    }
}
