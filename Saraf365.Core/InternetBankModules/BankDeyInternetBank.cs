using HtmlAgilityPack;
using Newtonsoft.Json;
using RestSharp;
using RockCandy.Web.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;


namespace Saraf365.Core.InternetBankModules
{
    public class BankDeyInternetBank: IInternetBank
    {
        public string Username { set; get; }
        public string Password { set; get; }
        public string ApiKey { set; get; }
        public string InterBankLoginUrl { set; get; }
        public string InterBankBasePath { set; get; }
        public string InterBankLoginPostFormUrl { set; get; }
        public string HomePageAddress { set; get; }
        public string LoginCookie { set; get; }
        public string CSRFToken { set; get; }
        public string DepositNumber { set; get; }
        public string RemaningTime { set; get; }
        public string InternetPassword { set; get; }
        public string Cvv2 { set; get; }
        public string ExpireMoth { set; get; }
        public string ExpireYear { set; get; }
        public string CardNumber { set; get; }

        public string LoginError { set; get; }
        public bool CredientialIsWrong { set; get; }
        public BankDeyInternetBank(string username,string password,string depositNumber,string cardNumber,string apiKey,string internetPassword="",string cvv2="",string expireMoth="",string expireYear="")
        {
            CredientialIsWrong = false;
            LoginError = "";
            CardNumber = cardNumber;
            ExpireYear = expireYear;
            ExpireMoth = expireMoth;
            Cvv2 = cvv2;
            InternetPassword = internetPassword;
            DepositNumber = depositNumber;
            Username = username;
            Password = password;
            ApiKey = apiKey;
            InterBankLoginUrl = "https://ebank.bdi24.com/webbank/login/loginPage.action?ibReq=WEB";
            InterBankBasePath = "https://ebank.bdi24.com";
            InterBankLoginPostFormUrl = "https://ebank.bdi24.com/webbank/login/login.action?ibReq=WEB&lang=fa";
            HomePageAddress = "https://ebank.bdi24.com/webbank/home/homePage.action";
            
        }

        private void AppendParameter(StringBuilder sb, string name, string value)
        {
            string encodedValue = HttpUtility.UrlEncode(value);
            sb.AppendFormat("{0}={1}&", name, encodedValue);
        }


        private string NavigateToLoginUrl()
        {
            var request = (HttpWebRequest)WebRequest.Create(InterBankLoginUrl);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.CookieContainer = new CookieContainer();
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.5");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseText = "";
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);
            CSRFToken = doc.DocumentNode.SelectSingleNode("/html/head/meta[9]").GetAttributeValue("content", "");
            foreach (var item in response.Cookies)
            {
                if (item.ToString().Contains("JSESSIONID"))
                {
                    LoginCookie = item.ToString();
                    break;
                }
            }
            return responseText;
        }

        private Tuple<bool,string> SubmitLoginForm(string captchaText,string loginToken)
        {
            StringBuilder sb = new StringBuilder();
            AppendParameter(sb, "struts.token.name", "loginToken");
            AppendParameter(sb, "loginToken", loginToken);
            AppendParameter(sb, "otpSyncRequired", "false");
            AppendParameter(sb, "hiddenPass1", "1");
            AppendParameter(sb, "hiddenPass2", "2");
            AppendParameter(sb, "hiddenPass3", "3");
            AppendParameter(sb, "loginType", "STATIC_PASSWORD");
            if (captchaText != "")
            {
                AppendParameter(sb, "captcha", captchaText.ToLower());
            }
            AppendParameter(sb, "username", Username);
            AppendParameter(sb, "password", Password);

            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());

            var request = (HttpWebRequest)WebRequest.Create(InterBankLoginPostFormUrl);
            request.AllowAutoRedirect = false;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();
            Uri target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/webbank", Secure = true, HttpOnly = true });
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.5");
            request.Headers.Add("CSRF_TOKEN", CSRFToken);
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = InterBankLoginUrl;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            foreach (var item in response.Cookies)
            {
                if (item.ToString().Contains("JSESSIONID"))
                {
                    LoginCookie = item.ToString();
                    break;
                }
            }
            string lastRefferer = InterBankLoginPostFormUrl;
            while (response.Headers["Location"]!=null)
            {
                request = (HttpWebRequest)WebRequest.Create(InterBankBasePath+ response.Headers["Location"]);
                request.AllowAutoRedirect = false;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = new CookieContainer();
                target = new Uri(InterBankBasePath);
                request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/webbank", Secure = true, HttpOnly = true });
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.5");
                request.Headers.Add("CSRF_TOKEN", CSRFToken);
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.Referer = lastRefferer;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(byteArray, 0, byteArray.Length);
                }
                response = (HttpWebResponse)request.GetResponse();
                foreach (var item in response.Cookies)
                {
                    if (item.ToString().Contains("JSESSIONID"))
                    {
                        LoginCookie = item.ToString();
                        break;
                    }
                }
                lastRefferer = InterBankBasePath + response.Headers["Location"];
            }

            string responseText = "";
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);
            CSRFToken = doc.DocumentNode.SelectSingleNode("/html/head/meta[2]").GetAttributeValue("content", "");
            bool loggedIn = doc.DocumentNode.SelectSingleNode("/html/head/title").InnerText.Contains("خانه");
            if (!loggedIn)
            {

                LoginError = doc.GetElementbyId("errorTitle").NextSibling.NextSibling.InnerText.Replace("\n", "").Trim();
                if (!LoginError.Contains("کد امنیتی"))
                {
                    CredientialIsWrong = true;
                }
            }
            return new Tuple<bool, string>(loggedIn, responseText);
        }

        public string GetInfo()
        {
            Refresh();
            return Username + " | " + LoginError + " | " + RemaningTime;

        }

        public bool Login()
        {





            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(NavigateToLoginUrl());

            string captchaText = "";
            if (doc.GetElementbyId("captchaBox") !=null)
            {
                AntiCaptchaTaskResponseModel slvCp =null;
                slvCp = SolveCaptcha("https://ebank.bdi24.com/webbank/login/captcha.action?isSoundCaptcha=false&r="+new Random().Next(Int32.MaxValue));
                while (slvCp.errorId != 0){
                    slvCp = SolveCaptcha(doc.GetElementbyId("captchaImage").GetAttributeValue("src",""));
                }
                Thread.Sleep(5000);
                var cpRes = CheckAntiCaptchaStatus(slvCp.taskId);
                while(cpRes.status== "processing")
                {
                    Thread.Sleep(5000);
                    cpRes = CheckAntiCaptchaStatus(slvCp.taskId);
                }
                if (cpRes.errorId == 0)
                {
                    captchaText=cpRes.solution.text;
                }
                else
                {
                    return false;
                }


            }

            var res = SubmitLoginForm(captchaText, doc.GetElementbyId("loginForm").Descendants("input").Where(node => node.GetAttributeValue("Name", "") == "loginToken").First().GetAttributeValue("Value", ""));
            return res.Item1;
        }

        public void Exit()
        {

        }
        public string GetCardHolderName(string cardNumber)
        {
            Refresh();
            if (Convert.ToInt32(RemaningTime) <= 0)
            {
                while (!Login() && !CredientialIsWrong) { }
            }
            if (CredientialIsWrong)
            {
                return "اطلاعات ورود اشتباه است";
            }
            var request = (HttpWebRequest)WebRequest.Create("https://ebank.bdi24.com/webbank/transfer/newCardTransfer.action?transferDestType=CARD");
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.CookieContainer = new CookieContainer();
            Uri target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/webbank", Secure = true, HttpOnly = true });
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
            request.Accept = "*/*";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9,fr;q=0.8");
            request.Referer = HomePageAddress;
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Host = "ebank.bdi24.com";
            request.Headers.Add("Origin", InterBankBasePath);
            request.Headers.Add("CSRF_TOKEN", CSRFToken);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseText = "";
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);

            StringBuilder sb = new StringBuilder();
            AppendParameter(sb, "transferDestType", "CARD");
            AppendParameter(sb, "struts.token.name", "cardToCardTransferToken");
            AppendParameter(sb, "cardToCardTransferToken", doc.GetElementbyId("cardTransferPayment").Descendants("input").Where(node => node.GetAttributeValue("Name", "") == "cardToCardTransferToken").First().GetAttributeValue("Value", ""));
            AppendParameter(sb, "pan", CardNumber);
            AppendParameter(sb, "panValueType", "CARD");
            AppendParameter(sb, "panPinnedDeposit", "");
            AppendParameter(sb, "panIsComboValInStore", "false");
            AppendParameter(sb, "destinationType", "CARD");
            AppendParameter(sb, "destinationValueType", "");
            AppendParameter(sb, "destinationPinnedDeposit", "");
            AppendParameter(sb, "destinationIsComboValInStore", "false");
            AppendParameter(sb, "fromCardManagement", "false");
            AppendParameter(sb, "amount", "10000");
            AppendParameter(sb, "destination", cardNumber);


            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());

            request = (HttpWebRequest)WebRequest.Create(InterBankBasePath + "/webbank/transfer/cardTransferConfirm.action");
            request.AllowAutoRedirect = false;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();
            target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/webbank", Secure = true, HttpOnly = true });
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.5");
            request.Headers.Add("CSRF_TOKEN", CSRFToken);
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = InterBankLoginUrl;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            response = (HttpWebResponse)request.GetResponse();
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            doc.LoadHtml(responseText);
            if(doc.GetElementbyId("ownerName")==null)
            {
                return "شماره کارت اشتباه است";
            }
            return doc.GetElementbyId("ownerName").GetAttributeValue("value", "");
        }

        public Tuple<bool, string> CardTransfer(string toCardNumber, long amount)
        {
            Refresh();
            if (Convert.ToInt32(RemaningTime) <= 0)
            {
                while (!Login() && !CredientialIsWrong) { }
            }
            if(CredientialIsWrong)
            {
                return new Tuple<bool, string>(false, "اطلاعات ورود اشتباه است");
            }
            var request = (HttpWebRequest)WebRequest.Create("https://ebank.bdi24.com/webbank/transfer/newCardTransfer.action?transferDestType=CARD");
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.CookieContainer = new CookieContainer();
            Uri target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/webbank", Secure = true, HttpOnly = true });
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
            request.Accept = "*/*";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9,fr;q=0.8");
            request.Referer = HomePageAddress;
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Host = "ebank.bdi24.com";
            request.Headers.Add("Origin", InterBankBasePath);
            request.Headers.Add("CSRF_TOKEN", CSRFToken);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseText = "";
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);

            StringBuilder sb = new StringBuilder();
            AppendParameter(sb, "transferDestType", "CARD");
            AppendParameter(sb, "struts.token.name", "cardToCardTransferToken");
            AppendParameter(sb, "cardToCardTransferToken", doc.GetElementbyId("cardTransferPayment").Descendants("input").Where(node => node.GetAttributeValue("Name", "") == "cardToCardTransferToken").First().GetAttributeValue("Value", ""));
            AppendParameter(sb, "pan", CardNumber);
            AppendParameter(sb, "panValueType", "CARD");
            AppendParameter(sb, "panPinnedDeposit", "");
            AppendParameter(sb, "panIsComboValInStore", "false");
            AppendParameter(sb, "destinationType", "CARD");
            AppendParameter(sb, "destinationValueType", "");
            AppendParameter(sb, "destinationPinnedDeposit", "");
            AppendParameter(sb, "destinationIsComboValInStore", "false");
            AppendParameter(sb, "fromCardManagement", "false");
            AppendParameter(sb, "amount", amount.ToString());
            AppendParameter(sb, "destination", toCardNumber);
            

            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());

            request = (HttpWebRequest)WebRequest.Create(InterBankBasePath + "/webbank/transfer/cardTransferConfirm.action");
            request.AllowAutoRedirect = false;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();
            target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/webbank", Secure = true, HttpOnly = true });
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.5");
            request.Headers.Add("CSRF_TOKEN", CSRFToken);
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = InterBankLoginUrl;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            response = (HttpWebResponse)request.GetResponse();
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            doc.LoadHtml(responseText);

            sb = new StringBuilder();
            AppendParameter(sb, "twoPhaseReferenceCode", doc.GetElementbyId("twoPhaseReferenceCode").GetAttributeValue("value",""));
            //AppendParameter(sb, "currency", "");
            AppendParameter(sb, "transferDestType", "CARD");
            AppendParameter(sb, "destinationType", "CARD");
            AppendParameter(sb, "struts.token.name", "confirmCardToCardTransferToken");
            AppendParameter(sb, "confirmCardToCardTransferToken", doc.DocumentNode.Descendants("input").Where(node => node.GetAttributeValue("Name", "") == "confirmCardToCardTransferToken").First().GetAttributeValue("Value", ""));
            AppendParameter(sb, "hiddenPass1", "1");
            AppendParameter(sb, "hiddenPass2", "2");
            AppendParameter(sb, "hiddenPass3", "3");
            AppendParameter(sb, "hiddenPass1", "1");
            AppendParameter(sb, "hiddenPass2", "2");
            AppendParameter(sb, "hiddenPass3", "3");
            AppendParameter(sb, "password", InternetPassword);
            AppendParameter(sb, "cvv2", Cvv2);
            AppendParameter(sb, "expireDate", ExpireYear+"/"+ExpireMoth);
            AppendParameter(sb, "pan", CardNumber);
            AppendParameter(sb, "amount", doc.GetElementbyId("amount").GetAttributeValue("value", ""));
            AppendParameter(sb, "destination", doc.GetElementbyId("destinationId").GetAttributeValue("value",""));
            AppendParameter(sb, "ownerName", doc.GetElementbyId("ownerName").GetAttributeValue("value", ""));
            AppendParameter(sb, "issuerTitle", doc.GetElementbyId("issuerTitle").GetAttributeValue("value", ""));

            

            byteArray = Encoding.UTF8.GetBytes(sb.ToString());

            request = (HttpWebRequest)WebRequest.Create(InterBankBasePath + "/webbank/transfer/cardTransferResult.action");
            request.AllowAutoRedirect = false;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();
            target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/webbank", Secure = true, HttpOnly = true });
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.5");
            request.Headers.Add("CSRF_TOKEN", CSRFToken);
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = InterBankLoginUrl;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            response = (HttpWebResponse)request.GetResponse();
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            doc.LoadHtml(responseText);
            if (doc.GetElementbyId("errorTitle") != null)
            {
                return new Tuple<bool, string>(false, doc.GetElementbyId("errorTitle").NextSibling.NextSibling.InnerText.Replace("\n","").Trim());
            }
            return new Tuple<bool, string>(true, doc.GetElementbyId("trackingNumber").GetAttributeValue("value", ""));

        }

        public Tuple<bool,string> NormalTransfer(string toDepositNumber,long amount,string sourceComment="",string destinationComment="")
        {
            Refresh();
            if (Convert.ToInt32(RemaningTime) <= 0)
            {
                while (!Login() && !CredientialIsWrong) { }
            }
            if (CredientialIsWrong)
            {
                return new Tuple<bool, string>(false, "اطلاعات ورود اشتباه است");
            }
            var request = (HttpWebRequest)WebRequest.Create("https://ebank.bdi24.com/webbank/transfer/newNormal.action");
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.CookieContainer = new CookieContainer();
            Uri target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/webbank", Secure = true, HttpOnly = true });
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
            request.Accept = "*/*";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9,fr;q=0.8");
            request.Referer = HomePageAddress;
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Host = "ebank.bdi24.com";
            request.Headers.Add("Origin", InterBankBasePath);
            request.Headers.Add("CSRF_TOKEN", CSRFToken);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseText = "";
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);

            StringBuilder sb = new StringBuilder();
            AppendParameter(sb, "transferType", "NORMAL");
            AppendParameter(sb, "struts.token.name", "normalTransferToken");
            AppendParameter(sb, "normalTransferToken", doc.GetElementbyId("newNormal").Descendants("input").Where(node => node.GetAttributeValue("Name", "") == "normalTransferToken").First().GetAttributeValue("Value", ""));
            AppendParameter(sb, "sourceSavingValueType", "sourceDeposit");
            AppendParameter(sb, "sourceSavingIsComboValInStore", "false");
            AppendParameter(sb, "destinationAccountIsComboValInStore", "false");
            AppendParameter(sb, "currency", "IRR");
            AppendParameter(sb, "currencyDefaultFractionDigits", "2");
            AppendParameter(sb, "amount", amount.ToString());
            AppendParameter(sb, "sourceComment", sourceComment);
            AppendParameter(sb, "destinationComment", destinationComment);
            AppendParameter(sb, "destinationAccount", toDepositNumber);
            AppendParameter(sb, "sourceSaving", DepositNumber);

            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());

            request = (HttpWebRequest)WebRequest.Create(InterBankBasePath+ "/webbank/transfer/confirmNormal.action");
            request.AllowAutoRedirect = false;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();
            target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/webbank", Secure = true, HttpOnly = true });
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.5");
            request.Headers.Add("CSRF_TOKEN", CSRFToken);
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = InterBankLoginUrl;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            response = (HttpWebResponse)request.GetResponse();
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            doc.LoadHtml(responseText);

            sb = new StringBuilder();
            AppendParameter(sb, "transferType", "NORMAL");
            AppendParameter(sb, "struts.token.name", "confirmNormalTransferToken");
            AppendParameter(sb, "confirmNormalTransferToken", doc.DocumentNode.Descendants("input").Where(node => node.GetAttributeValue("Name", "") == "confirmNormalTransferToken").First().GetAttributeValue("Value", ""));
            AppendParameter(sb, "currency", "IRR");
            AppendParameter(sb, "amount", amount.ToString());
            AppendParameter(sb, "sourceComment", sourceComment);
            AppendParameter(sb, "destinationComment", destinationComment);
            AppendParameter(sb, "destinationAccount", toDepositNumber);
            AppendParameter(sb, "sourceSaving", DepositNumber);

            byteArray = Encoding.UTF8.GetBytes(sb.ToString());

            request = (HttpWebRequest)WebRequest.Create(InterBankBasePath + "/webbank/transfer/receiptNormal.action");
            request.AllowAutoRedirect = false;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();
            target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/webbank", Secure = true, HttpOnly = true });
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.5");
            request.Headers.Add("CSRF_TOKEN", CSRFToken);
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = InterBankLoginUrl;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            response = (HttpWebResponse)request.GetResponse();
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            doc.LoadHtml(responseText);
            if (doc.GetElementbyId("errorTitle") != null)
            {
                return new Tuple<bool, string>(false, doc.GetElementbyId("errorTitle").NextSibling.NextSibling.InnerText.Replace("\n", "").Trim());
            }
            return new Tuple<bool, string>(true, doc.GetElementbyId("trackingNumber").GetAttributeValue("value", ""));

        }
        public List<CardTransferHistoryModel> GetCartTransferHistory(DateTime fromDate,DateTime toDate)
        {
            Refresh();
            if (Convert.ToInt32(RemaningTime) <= 0)
            {
                while (!Login() && !CredientialIsWrong) { }
            }
            if (CredientialIsWrong)
            {
                return new List<CardTransferHistoryModel>();
            }
            var request = (HttpWebRequest)WebRequest.Create(string.Format("https://ebank.bdi24.com/webbank/viewAcc/billHtmlReport.action?selectedDeposit={0}&fromDateTime={1}++-++{2}%3A{3}&toDateTime={4}++-++{5}%3A{6}&desc=&order=DESC&billType=0&paymentId=",DepositNumber, DateConvertor.MiladiToShamsi(fromDate).Replace("/", "%2F"),fromDate.ToString("HH"), fromDate.ToString("mm"), DateConvertor.MiladiToShamsi(toDate).Replace("/", "%2F"), toDate.ToString("HH"), toDate.ToString("mm")));
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();
            Uri target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/webbank", Secure = true, HttpOnly = true });
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
            request.Accept = "*/*";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9,fr;q=0.8");
            request.Referer = HomePageAddress;
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Host = "ebank.bdi24.com";
            request.Headers.Add("Origin", InterBankBasePath);
            request.Headers.Add("CSRF_TOKEN", CSRFToken);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
            string responseText = "";
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);
            List<CardTransferHistoryModel> cardTransfers = new List<CardTransferHistoryModel>();
            foreach (var tr in doc.DocumentNode.Descendants("tr"))
            {
                try
                {
                    if (tr.Descendants("td").Count() != 0)
                    {
                        CardTransferHistoryModel instance = new CardTransferHistoryModel();
                        instance.Date = Convert.ToDateTime(DateConvertor.ShamsiToMiladi((tr.Descendants("td").ElementAt(1).InnerText.Split(' ')[0])).ToShortDateString() + " " + Convert.ToDateTime(tr.Descendants("td").ElementAt(1).InnerText.Split(' ')[1]).ToString("HH:mm:ss"));
                        instance.Description = tr.Descendants("td").ElementAt(2).InnerText;
                        instance.AmountIn = Convert.ToInt64(tr.Descendants("td").ElementAt(3).InnerText.Replace(",", ""));
                        instance.AmountOut = Convert.ToInt64(tr.Descendants("td").ElementAt(4).InnerText.Replace(",", ""));
                        instance.DocumentNumber = tr.Descendants("td").ElementAt(7).InnerText;
                        instance.CodeErja = "";
                        instance.CodePeygiri = "";
                        instance.CardNumber = "";
                        if (instance.Description.Contains("کد رهگيري"))
                        {
                            instance.CodePeygiri = tr.Descendants("td").ElementAt(2).InnerText.Split(new string[] { "کد رهگيري" }, StringSplitOptions.None).Where(x => x.Trim() != "").Last();

                        }
                        /*if (instance.Description.Contains("انتقال وجه از") && instance.Description.Contains("به کارت"))
                        {
                            instance.CardNumber = tr.Descendants("td").ElementAt(2).InnerText.Split(new string[] { "از" }, StringSplitOptions.None)[1].Split(' ')[0];
                        }*/
                        var matches = Regex.Matches(tr.Descendants("td").ElementAt(2).InnerText, @"[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]", RegexOptions.IgnoreCase);
                        foreach (var item in matches)
                        {
                            if (item.ToString() != CardNumber)
                            {
                                instance.CardNumber = item.ToString();
                            }
                        }

                        cardTransfers.Add(instance);
                    }
                }
                catch
                {

                }
                

            }
            return cardTransfers;
        }
        public Tuple<long,long,string> GetBalance()
        {
            Refresh();
            if(Convert.ToInt32(RemaningTime)<= 0)
            {
                while (!Login() && !CredientialIsWrong) { }
            }
            if (CredientialIsWrong)
            {
                return new Tuple<long, long, string>(-2, -2, "اطلاعات ورود اشتباه است");
            }
            StringBuilder sb = new StringBuilder();
            AppendParameter(sb, "smartComboType", "DEPOSIT");
            AppendParameter(sb, "showContacts", "false");
            AppendParameter(sb, "businessType", "all");
            AppendParameter(sb, "serviceName", "");
            AppendParameter(sb, "currency", "");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());


            var request = (HttpWebRequest)WebRequest.Create("https://ebank.bdi24.com/webbank/main/getSourcesAndDestinations.action");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();
            Uri target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/webbank", Secure = true, HttpOnly = true });
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
            request.Accept = "*/*";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9,fr;q=0.8");
            request.Referer = HomePageAddress;
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Host = "ebank.bdi24.com";
            request.Headers.Add("Origin", InterBankBasePath);
            request.Headers.Add("CSRF_TOKEN", CSRFToken);
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            foreach (var item in response.Cookies)
            {
                if (item.ToString().Contains("JSESSIONID"))
                {
                    LoginCookie = item.ToString();
                    break;
                }
            }
            string responseText = "";
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            try
            {
                var resData = JsonConvert.DeserializeObject<DeyAccountAllBalanceModel>(responseText);
                return new Tuple<long, long, string>(resData.allAccounts.Where(x => x.depositNumber == DepositNumber).Single().balance.availablePrice, resData.allAccounts.Where(x => x.depositNumber == DepositNumber).Single().blockedAmount, resData.allAccounts.Where(x => x.depositNumber == DepositNumber).Single().depositStatus);
            }
            catch
            {

            }

            return new Tuple<long, long, string>(-1, -1, "") ;
        }
        public bool Refresh()
        {

            if(LoginCookie==null || LoginCookie=="")
            {
                return false ;
            }
            var request = (HttpWebRequest)WebRequest.Create(HomePageAddress);
            request.Method = "GET";
            request.CookieContainer = new CookieContainer();
            Uri target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/webbank", Secure = true, HttpOnly = true });
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9,fr;q=0.8");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = InterBankLoginUrl;
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Host = "ebank.bdi24.com";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            foreach (var item in response.Cookies)
            {
                if (item.ToString().Contains("JSESSIONID"))
                {
                    LoginCookie = item.ToString();
                    break;
                }
            }
            if (response.Headers["remainingTime"]!=null){
                RemaningTime = response.Headers["remainingTime"].ToString();
            }
            else
            {
                RemaningTime = "0";
            }

            string responseText = "";
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            /*HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);
            bool loggedIn = doc.DocumentNode.SelectSingleNode("/html/head/title").InnerText.Contains("خانه");*/
            return true;
        }

        private AntiCaptchaCheckStatusResponse CheckAntiCaptchaStatus(int taskId)
        {
            var client = new RestClient("https://api.anti-captcha.com/getTaskResult");

            var request = new RestRequest("", Method.POST);
            request.AddJsonBody(new AntiCaptchaCheckStatusRequest(ApiKey, taskId));

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<AntiCaptchaCheckStatusResponse>(response.Content);
        }

        private AntiCaptchaTaskResponseModel SolveCaptcha(string captchaImageUrl)
        {
            var client = new RestClient("https://api.anti-captcha.com/createTask");

            var request = new RestRequest("", Method.POST);
            request.AddJsonBody(new AntiCaptchaTaskModel(ApiKey, "ImageToTextTask", GetBase64CaptchaImage(captchaImageUrl), false, false, false, 0, 0, 0));
            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<AntiCaptchaTaskResponseModel>(response.Content);
        }

  
        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        private string GetBase64CaptchaImage(string captchaImageUrl)
        {
            var request = (HttpWebRequest)WebRequest.Create(captchaImageUrl);
            request.Method = "GET";
            request.CookieContainer = new CookieContainer();
            Uri target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]){ Domain = target.Host, Path = "/webbank", Secure = true, HttpOnly = true });
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9,fr;q=0.8");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = InterBankLoginUrl;
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Host = "ebank.bdi24.com";
            
            var response = ((HttpWebResponse)request.GetResponse());
            foreach (var item in response.Cookies)
            {
                if (item.ToString().Contains("JSESSIONID"))
                {
                    LoginCookie = item.ToString();
                    break;
                }
            }
            return Convert.ToBase64String(ReadFully(response.GetResponseStream()));

        }

        private class DeyAccountAllBalanceModel
        {
            public List<DeyAccountBalanceModel> allAccounts { set; get; }
        }
        private class DeyAccountBalanceModel
        {
            public DeyAccountBalanceModelBalance balance { set; get; }
            public string depositNumber { set; get; }
            public long blockedAmount { set; get; }
            public string depositStatus { set; get; }
        }
        private class DeyAccountBalanceModelBalance
        {
            public long availablePrice { set; get; }
            public long price { set; get; }
            
        }
    }
}