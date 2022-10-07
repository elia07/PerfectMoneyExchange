using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.Utils
{
    public class PerfectMoney
    {
        public string AccountID { set; get; }
        public string PassPhrase { set; get; }
        public string Payee_Account { set; get; }

        public PerfectMoney()
        {

        }
        public PerfectMoney(string accountID, string passPhrase, string payee_Account)
        {
            AccountID = accountID;
            PassPhrase = passPhrase;
            Payee_Account = payee_Account;
        }

        public string[] GetBalance()
        {
            string[] res = new string[] { "0", "0" };
            try
            {

                var doc = new HtmlDocument();
                doc.LoadHtml(new WebClient().DownloadString(new Uri(string.Format("https://perfectmoney.is/acct/balance.asp?AccountID={0}&PassPhrase={1}", AccountID, PassPhrase))));
                var err = doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "ERROR").SingleOrDefault();
                if (err != null)
                {
                    res[0] = err.GetAttributeValue("Value", "-1");
                }
                else
                {
                    res[1] = string.Format("{0}", doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == Payee_Account).Single().GetAttributeValue("Value", ""));
                }

            }
            catch {
                res = new string[] { "connectionIssue-disabled", "0" };
            }
            return res;

        }
        public string[] Activation(string ev_number, string ev_code)
        {
            string[] res = new string[] { "", "", "" };
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(new WebClient().DownloadString(new Uri(string.Format("https://perfectmoney.is/acct/ev_activate.asp?AccountID={0}&PassPhrase={1}&Payee_Account={2}&ev_number={3}&ev_code={4}", AccountID, PassPhrase, Payee_Account, ev_number, ev_code))));
                var err = doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "ERROR").SingleOrDefault();
                if (err != null)
                {
                    res[0] = err.GetAttributeValue("Value", "-1");
                }
                else
                {
                    res[1] = string.Format("VOUCHER_NUM:{0} , VOUCHER_AMOUNT:{1} , VOUCHER_AMOUNT_CURRENCY :{2} , Payee_Account:{3} , PAYMENT_BATCH_NUM:{4}", doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "VOUCHER_NUM").Single().GetAttributeValue("Value", ""), doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "VOUCHER_AMOUNT").Single().GetAttributeValue("Value", ""), doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "VOUCHER_AMOUNT_CURRENCY").Single().GetAttributeValue("Value", ""), doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "Payee_Account").Single().GetAttributeValue("Value", ""), doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "PAYMENT_BATCH_NUM").Single().GetAttributeValue("Value", ""));
                    res[2] = doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "VOUCHER_AMOUNT").Single().GetAttributeValue("Value", "");
                }
            }
            catch {
                res = new string[] { "connectionIssue-disabled", "", "" };
            }

            return res;
        }
        public string[] Create(string amount)
        {
            string[] res = new string[] { "", "" };
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(new WebClient().DownloadString(new Uri(string.Format("https://perfectmoney.is/acct/ev_create.asp?AccountID={0}&PassPhrase={1}&Payer_Account={2}&Amount={3}", AccountID, PassPhrase, Payee_Account, amount))));
                var err = doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "ERROR").SingleOrDefault();
                if (err != null)
                {
                    res[0] = err.GetAttributeValue("Value", "-1");
                }
                else
                {
                    res[1] = string.Format("VOUCHER_NUM:{0} , VOUCHER_CODE:{1} , VOUCHER_AMOUNT :{2}", doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "VOUCHER_NUM").Single().GetAttributeValue("Value", ""), doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "VOUCHER_CODE").Single().GetAttributeValue("Value", ""), doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "VOUCHER_AMOUNT").Single().GetAttributeValue("Value", ""));
                }
            }
            catch
            {
                res = new string[] { "connectionIssue-disabled", "" };
            }

            return res;
        }

    }
}
