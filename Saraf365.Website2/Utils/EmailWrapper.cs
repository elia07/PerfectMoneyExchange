using RockCandy.Web.Framework.Utilities.EmailUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Saraf365.Website2.Utils
{
    public class EmailWrapper
    {
        public void SendWelcomeEmail(string email,string languageCode)
        {
            EmailSenderInfo info = new EmailSenderInfo(SectionInfo.Setting.SupportEmailAccount,SectionInfo.Setting.SupportAccountPassword,SectionInfo.Setting.ApplicationName);
            EmailMessage msg = new EmailMessage(SectionInfo.Setting.Languages[languageCode]["Txt_WelcomeEmailSubject"], File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\templates\\" + languageCode + "\\WelcomeTemplate.html")));
            new EmailUtils().SendEmail(info,SectionInfo.Setting.MailServerAddress, SectionInfo.Setting.MailserverPort,false, msg, email);
        }

        public void SendActivationCode(string email,string activationCode, string languageCode)
        {
            EmailSenderInfo info = new EmailSenderInfo(SectionInfo.Setting.SupportEmailAccount, SectionInfo.Setting.SupportAccountPassword, SectionInfo.Setting.ApplicationName);
            EmailMessage msg = new EmailMessage(SectionInfo.Setting.Languages[languageCode]["Txt_WelcomeEmailSubject"], File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\templates\\" + languageCode + "\\ActivationTemplate.html")).Replace("###Code###",activationCode));
            new EmailUtils().SendEmail(info, SectionInfo.Setting.MailServerAddress, SectionInfo.Setting.MailserverPort, false, msg, email);
        }

        public void SendRecoverPasswordActivationCode(string email, string activationCode, string languageCode)
        {
            EmailSenderInfo info = new EmailSenderInfo(SectionInfo.Setting.SupportEmailAccount, SectionInfo.Setting.SupportAccountPassword, SectionInfo.Setting.ApplicationName);
            EmailMessage msg = new EmailMessage(SectionInfo.Setting.Languages[languageCode]["Txt_WelcomeEmailSubject"], File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\templates\\" + languageCode + "\\RecoverPasswordActivationCodeTemplate.html")).Replace("###Code###", activationCode));
            new EmailUtils().SendEmail(info, SectionInfo.Setting.MailServerAddress, SectionInfo.Setting.MailserverPort, false, msg, email);
        }
    }
}