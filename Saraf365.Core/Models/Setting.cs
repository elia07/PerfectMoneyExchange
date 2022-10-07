using Newtonsoft.Json;
using Saraf365.Core.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Saraf365.Core.Models
{
    public class Setting
    {
        public Dictionary<string, Dictionary<string, string>> Languages = null;

        public string ApplicationName { set; get; }
        public bool RegisterEnabled { set; get; }
        public string SecurityKey { set; get; }
        public string DefaultLanguage { set; get; }
        public string InstaPage { set; get; }
        public string TelegramChannel { set; get; }
        public string TelegramSupportAccount { set; get; }



        public bool CellphoneActivation { set; get; }
        public bool EmailActivation { set; get; }

        public string SupportAccountPassword { set; get; }
        public string SupportEmailAccount { set; get; }
        public int ActivationCodeExpireInMins { set; get; }

        public bool MaintenanceMode { set; get; }
        public string DomainAddress { set; get; }
        public string ProfileAddress { set; get; }
     
        public bool IDCartVerification { set; get; }

        
        public int ActivationResendInMins { set; get; }
        public List<string> ValidFileExtentionsForIDCart { set; get; }
        public List<string> ValidImageFormatsForBackoffice { set; get; }
        
        public long MaxFileSizeForIDCard { set; get; }

        public List<string> BankTypes { set; get; }

        
        public long PerfectMoneySellPrice { set; get; }
        public long PerfectMoneyBuyPrice { set; get; }

        public Dictionary<string, List<string>> AdminNotificationSetting { set; get; }

        public int CdnImageUpdatePeriodInMins { set; get; }

        public int GalleryItemsCount { set; get; }

        public string CdnAddress { set; get; }

        public string PerfectMoneyID { set; get; }
        public string PerfectMoneyPassword { set; get; }

        public long UserLevelIncreaseEach { set; get; }

        public string MaintenanceModeMessage { set; get; }

        public string TelegramBotAccessToken { set; get; }

        public string TelegramCallBackAddress { set; get; }
        
        public int ClearTelegramSupportAfterDays { set; get; }
        public int ClearBackOfficeLogsAfterDays { set; get; }
        public int SettingUpdatePeriod { set; get; }
        
        public string PerfectMoneyAccount { set; get; }

        public Dictionary<long,Tuple<long,long>> CashWithdrawalCost { set; get; }
        public int PerfectWithdrawalCostPercent { set; get; }

        public string TelegramMessageAppendText { set; get; }
        public int UserBankAccountCountForVerification { set; get; }
        public string AntiCaptchaApiKey { set; get; }
        public int InputCardInternetBankTaskPeriod { set; get; }

        public string MailServerAddress { set; get; }
        public int MailserverPort { set; get; }
        public bool PerfectBuyAvail { set; get; }
        public bool PerfectSellAvail { set; get; }

        public long ReferralShareDefault { set; get; }

        public bool StrictHttps { set; get; }
        public Setting()
        {
            init();
        }

        public void init()
        {
            if(Languages==null)
            {
                Languages= new Dictionary<string, Dictionary<string, string>>();
                var languageFiles= Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\languages\\"));

                foreach (var item in languageFiles)
                {
                    Languages.Add(Path.GetFileNameWithoutExtension(item), JsonConvert.DeserializeObject<Dictionary<string,string>>(File.ReadAllText(item)));
                }
            }
            using (SettingRepository sr = new SettingRepository())
            {
                this.StrictHttps = Convert.ToBoolean(sr.GetByKey("StrictHttps"));
                this.ReferralShareDefault = Convert.ToInt64(sr.GetByKey("ReferralShareDefault"));
                this.PerfectBuyAvail = Convert.ToBoolean(sr.GetByKey("PerfectBuyAvail"));
                this.PerfectSellAvail = Convert.ToBoolean(sr.GetByKey("PerfectSellAvail"));
                this.InputCardInternetBankTaskPeriod = Convert.ToInt32(sr.GetByKey("InputCardInternetBankTaskPeriod"));
                this.AntiCaptchaApiKey = sr.GetByKey("AntiCaptchaApiKey");
                this.UserBankAccountCountForVerification = Convert.ToInt32(sr.GetByKey("UserBankAccountCountForVerification"));
                this.TelegramMessageAppendText = sr.GetByKey("TelegramMessageAppendText");
                this.PerfectWithdrawalCostPercent = Convert.ToInt32(sr.GetByKey("PerfectWithdrawalCostPercent"));
                this.CashWithdrawalCost = JsonConvert.DeserializeObject<Dictionary<long, Tuple<long, long>>>(sr.GetByKey("CashWithdrawalCost"));
                this.PerfectMoneyAccount = sr.GetByKey("PerfectMoneyAccount");
                this.SettingUpdatePeriod = Convert.ToInt32(sr.GetByKey("SettingUpdatePeriod"));
                this.ClearBackOfficeLogsAfterDays = Convert.ToInt32(sr.GetByKey("ClearBackOfficeLogsAfterDays")); 
                this.ClearTelegramSupportAfterDays = Convert.ToInt32(sr.GetByKey("ClearTelegramSupportAfterDays")); 
                this.TelegramCallBackAddress = sr.GetByKey("TelegramCallBackAddress");
                this.TelegramBotAccessToken = sr.GetByKey("TelegramBotAccessToken");

                this.MaintenanceModeMessage = sr.GetByKey("MaintenanceModeMessage"); 

                this.UserLevelIncreaseEach = Convert.ToInt64(sr.GetByKey("UserLevelIncreaseEach"));

                this.PerfectMoneyID = sr.GetByKey("PerfectMoneyID");
                this.PerfectMoneyPassword = sr.GetByKey("PerfectMoneyPassword");

                this.CdnAddress = sr.GetByKey("CdnAddress");
                this.CdnImageUpdatePeriodInMins = Convert.ToInt32(sr.GetByKey("CdnImageUpdatePeriodInMins"));
                this.GalleryItemsCount = Convert.ToInt32(sr.GetByKey("GalleryItemsCount"));

                this.RegisterEnabled = Convert.ToBoolean(sr.GetByKey("RegisterEnabled"));
                this.ApplicationName = sr.GetByKey("ApplicationName");
                this.SecurityKey = sr.GetByKey("SecurityKey");
                this.DefaultLanguage = sr.GetByKey("DefaultLanguage");
                this.InstaPage = sr.GetByKey("InstaPage");
                this.TelegramChannel = sr.GetByKey("TelegramChannel");
                this.TelegramSupportAccount = sr.GetByKey("TelegramSupportAccount");
                this.CellphoneActivation = Convert.ToBoolean(sr.GetByKey("CellphoneActivation"));
                this.EmailActivation = Convert.ToBoolean(sr.GetByKey("EmailActivation"));
                this.MailServerAddress = sr.GetByKey("MailServerAddress");
                this.MailserverPort = Convert.ToInt32(sr.GetByKey("MailserverPort"));
                this.SupportAccountPassword = sr.GetByKey("SupportAccountPassword");
                this.SupportEmailAccount = sr.GetByKey("SupportEmailAccount");
                this.ActivationCodeExpireInMins = Convert.ToInt32(sr.GetByKey("ActivationCodeExpireInMins"));
                this.MaintenanceMode = Convert.ToBoolean(sr.GetByKey("MaintenanceMode"));
                this.DomainAddress = sr.GetByKey("DomainAddress");
                this.ProfileAddress = sr.GetByKey("ProfileAddress");
                
                this.IDCartVerification = Convert.ToBoolean(sr.GetByKey("IDCartVerification"));
                

                

                this.ActivationResendInMins = Convert.ToInt32(sr.GetByKey("ActivationResendInMins"));


                this.ValidFileExtentionsForIDCart = JsonConvert.DeserializeObject<List<string>>(sr.GetByKey("ValidFileExtentionsForIDCart"));
                this.ValidImageFormatsForBackoffice = JsonConvert.DeserializeObject<List<string>>(sr.GetByKey("ValidImageFormatsForBackoffice"));
                this.MaxFileSizeForIDCard = Convert.ToInt64(sr.GetByKey("MaxFileSizeForIDCard"));


                this.BankTypes = JsonConvert.DeserializeObject<List<string>>(sr.GetByKey("BankTypes"));


                

                
                this.PerfectMoneySellPrice = Convert.ToInt64(sr.GetByKey("PerfectMoneySellPrice"));
                this.PerfectMoneyBuyPrice = Convert.ToInt64(sr.GetByKey("PerfectMoneyBuyPrice"));



                this.AdminNotificationSetting = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(sr.GetByKey("AdminNotificationSetting"));
    }

}

    }


}

