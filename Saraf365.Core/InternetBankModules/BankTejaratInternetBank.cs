using HtmlAgilityPack;
using Newtonsoft.Json;
using RestSharp;
using RockCandy.Web.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Saraf365.Core.InternetBankModules
{
    public class BankTejaratInternetBank : IInternetBank
    {
        public string Username { set; get; }
        public string Password { set; get; }
        public string ApiKey { set; get; }
        public string InterBankLoginUrl { set; get; }
        public string InterBankBasePath { set; get; }
        public string InterBankLoginPostFormUrl { set; get; }
        public string HomePageAddress { set; get; }
        public string LoginCookie { set; get; }
        public string CookieSession1 { set; get; }
        public string DepositNumber { set; get; }
        public string InternetPassword { set; get; }
        public string Cvv2 { set; get; }
        public string ExpireMoth { set; get; }
        public string ExpireYear { set; get; }
        public string CardNumber { set; get; }

        public string LoginError { set; get; }
        public bool CredientialIsWrong { set; get; }

        public string ViewState { set; get; }
        public string UserAgent { set; get; }
        public string Accept { set; get; }

        public string LastReferrer { set; get; }
        public string ClientID { set; get; }

        public string LastResponseString { set; get; }

        public bool IsLogin { set; get; }
        public DateTime WaitUntil { set; get; }

        public string Pin2 { set; get; }

        

        public BankTejaratInternetBank(string username, string password, string depositNumber, string cardNumber,string pin2, string apiKey, string internetPassword = "", string cvv2 = "", string expireMoth = "", string expireYear = "")
        {
            Pin2 = pin2;
            LoginCookie = "";
            CookieSession1 = "";
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
            InterBankLoginUrl = "https://ib.tejaratbank.ir/web/ns/login";
            InterBankBasePath = "https://ib.tejaratbank.ir";
            InterBankLoginPostFormUrl = "https://ib.tejaratbank.ir/web/ns/login?execution=e1s1";
            HomePageAddress = "https://ib.tejaratbank.ir/web/s/main?execution=e4s1";
            UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
            Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
            IsLogin = false;
        }

        private void AppendParameter(StringBuilder sb, string name, string value)
        {
            string encodedValue = HttpUtility.UrlEncode(value);
            sb.AppendFormat("{0}={1}&", name, encodedValue);
        }

        public string DoAction(string str)
        {

            XmlDocument document = new XmlDocument();
            document.LoadXml(str);

            string retval = "";

            if(document.SelectSingleNode("partial-response/changes/update")!=null)
            {
                foreach(XmlNode update in document.SelectNodes("partial-response/changes/update"))
                {
                    if(update.Attributes["id"] != null)
                    {
                        if (update.Attributes["id"].Value == "javax.faces.ViewState")
                        {
                            ViewState = update.ChildNodes[0].Value.Replace("/", "%2F");
                        }
                        else
                        {
                            retval = update.ChildNodes[0].Value;
                        }
                    }
                }
               
                
            }
            if (document.SelectSingleNode("partial-response/redirect") != null && document.SelectSingleNode("partial-response/redirect").Attributes["url"] != null)
            {
                retval = InterBankBasePath+ document.SelectSingleNode("partial-response/redirect").Attributes["url"].Value;

            }
            return retval;
        }

        public void Exit()
        {

            if (LoginCookie == null || LoginCookie == "")
            {
                return;
            }
            azaval:
            var request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.AllowAutoRedirect = true;

            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            //origin
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");

            request.CookieContainer = new CookieContainer();
            Uri target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie("localCode", "fa") { Domain = target.Host, Path = "/", HttpOnly = true });


            byte[] byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=j_id82786531_fa78686%3Aconfirm&javax.faces.partial.execute=%40all&j_id82786531_fa78686%3Aconfirm=j_id82786531_fa78686%3Aconfirm&_client_id={0} &j_id82786531_fa78686_SUBMIT=1&javax.faces.ViewState={1}", ClientID, ViewState));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));

            var url = DoAction(responseText);
            if (url == "" || url=="-1")
            {
                goto azaval;
            }

            request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = true;
            request.Method = "GET";
            request.CookieContainer = new CookieContainer();
            target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie("localCode", "fa") { Domain = target.Host, Path = "/", HttpOnly = true });
            request.UserAgent = UserAgent;
            request.Accept = Accept;
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.9");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = LastReferrer;
            request.Host = "ib.tejaratbank.ir";



            response = (HttpWebResponse)request.GetResponse();
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);
            LastResponseString = responseText;
            ViewState = doc.GetElementbyId("javax.faces.ViewState").GetAttributeValue("value", "");
            if (responseText.Contains("بانک تجارت - بانکداری اینترنتی :ورود"))
            {

                LastReferrer = "";
                ClientID = "";
                //CookieSession1 = "";
                LoginCookie = "";
                LastResponseString = "";
            }
            else
            {

            }
        }
        private string NavigateToLoginUrl()
        {
            Uri target = new Uri(InterBankBasePath);
            var request = (HttpWebRequest)WebRequest.Create(InterBankLoginUrl);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.CookieContainer = new CookieContainer();
            request.UserAgent = UserAgent;
            request.Accept = Accept;
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.9");
            request.AllowAutoRedirect = false;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseText = "";
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            foreach (var item in response.Cookies)
            {
                if (item.ToString().Contains("JSESSIONID"))
                {
                    LoginCookie = item.ToString();
                }
                if (item.ToString().Contains("cookiesession1"))
                {
                    CookieSession1 = item.ToString();
                }
            }
            string lastRefferer = InterBankLoginPostFormUrl;
            while (response.Headers["Location"] != null)
            {
                request = (HttpWebRequest)WebRequest.Create(response.Headers["Location"]);
                request.AllowAutoRedirect = false;
                request.Method = "GET";
                request.ContentType = response.ContentType;
                request.CookieContainer = new CookieContainer();
                target = new Uri(InterBankBasePath);
                request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
                request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });

                request.UserAgent = UserAgent;
                request.Accept = Accept;
                request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.9");
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.Referer = lastRefferer;
                response = (HttpWebResponse)request.GetResponse();
                lastRefferer = response.Headers["Location"];
            }

            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            LastResponseString = responseText;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);
            ViewState = doc.GetElementbyId("javax.faces.ViewState").GetAttributeValue("value", "");
            return responseText;
        }

        private Tuple<bool, string> SubmitLoginForm(string captchaText)
        {
            var request = (HttpWebRequest)WebRequest.Create(InterBankLoginPostFormUrl);
            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            request.Headers.Add("Origin", "https://ib.tejaratbank.ir");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = "https://ib.tejaratbank.ir/web/ns/login?execution=e1s1";
            Uri target = new Uri(InterBankBasePath);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });

            //byte[] byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=loginForm%3Aj_id888561664_2_1bf08068&javax.faces.partial.execute=%40all&javax.faces.partial.render=loginForm&loginForm%3Aj_id888561664_2_1bf08068=loginForm%3Aj_id888561664_2_1bf08068&fakeusernameremembered=&fakepasswordremembered=&loginForm%3AuserName={0}&loginForm%3Apassword={1}&loginForm%3AloginCaptcha%3AloginCaptchaInputWgt={2}&loginForm_SUBMIT=1&javax.faces.ViewState={3}",Username,Password,captchaText,ViewState));
            byte[] byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=loginForm%3Aj_id888561664_2_1bf08054&javax.faces.partial.execute=%40all&javax.faces.partial.render=loginForm&loginForm%3Aj_id888561664_2_1bf08054=loginForm%3Aj_id888561664_2_1bf08054&fakeusernameremembered=&fakepasswordremembered=&loginForm%3AuserName={0}&loginForm%3Apassword={1}&loginForm%3AloginCaptcha%3AloginCaptchaInputWgt={2}&loginForm_SUBMIT=1&javax.faces.ViewState={3}", Username, Password, captchaText, ViewState));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseText = "";
            responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));

            string redirecttoMainPage = DoAction(responseText);
            if(redirecttoMainPage == "")
            {
                HtmlAgilityPack.HtmlDocument docc = new HtmlAgilityPack.HtmlDocument();
                docc.LoadHtml(responseText);
                return new Tuple<bool, string>(false, docc.DocumentNode.Descendants("span").Where(x => x.GetAttributeValue("class", "") == "ui-messages-error-summary").FirstOrDefault().InnerText);
            }

           
  
            request = (HttpWebRequest)WebRequest.Create(redirecttoMainPage);
            request.AllowAutoRedirect = true;
            request.Method = "GET";
            request.CookieContainer = new CookieContainer();
            target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.UserAgent = UserAgent;
            request.Accept = Accept;
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.9");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = InterBankLoginPostFormUrl;
            request.Host = "ib.tejaratbank.ir";


            response = (HttpWebResponse)request.GetResponse();
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);
            ViewState = doc.GetElementbyId("javax.faces.ViewState").GetAttributeValue("value", "");
            if (doc.DocumentNode.Descendants("span").Where(x => x.GetAttributeValue("class", "") == "ui-messages-error-summary").FirstOrDefault()!=null)
            {
                var errText = doc.DocumentNode.Descendants("span").Where(x => x.GetAttributeValue("class", "") == "ui-messages-error-summary").FirstOrDefault().InnerText;
                if (errText.Contains("اطلاعات شما در سامانه بانكداری اینترنتی ثبت نشده است. لطفا جهت ثبت نام به شعبه خود مراجعه نمایید ."))
                {
                    CredientialIsWrong = true;
                }
                return new Tuple<bool, string>(false, errText);
            }
            else
            {
                if(responseText.Contains("بانک تجارت - بانکداری اینترنتی  :صفحه اصلی"))
                {

                    LastResponseString = responseText;
                    LastReferrer = response.ResponseUri.ToString();
                    ClientID = doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "_client_id").FirstOrDefault().GetAttributeValue("value", "");
                    return new Tuple<bool, string>(true, "");
                }
                else
                {
                    return new Tuple<bool, string>(false, "unknown");
                }
                
            }
            
        }

        public string GetInfo()
        {
            return Username + " | " + LoginError ;

        }
        private Int64 GetTime()
        {
            Int64 retval = 0;
            var st = new DateTime(1970, 1, 1);
            TimeSpan t = (DateTime.Now.ToUniversalTime() - st);
            retval = (Int64)(t.TotalMilliseconds + 0.5);
            return retval;
        }

        public bool Login()
        {


            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(NavigateToLoginUrl());

            string captchaText = "";
            if (doc.GetElementbyId("loginCaptcha") != null)
            {
                AntiCaptchaTaskResponseModel slvCp = null;
                slvCp = SolveCaptcha(InterBankBasePath + doc.GetElementbyId("loginCaptcha").GetAttributeValue("src","")+"?"+ GetTime());
                while (slvCp.errorId != 0)
                {
                    slvCp = SolveCaptcha(InterBankBasePath+doc.GetElementbyId("loginCaptcha").GetAttributeValue("src", "") + "?" + GetTime());
                }
                Thread.Sleep(5000);
                var cpRes = CheckAntiCaptchaStatus(slvCp.taskId);
                while (cpRes.status == "processing")
                {
                    Thread.Sleep(5000);
                    cpRes = CheckAntiCaptchaStatus(slvCp.taskId);
                }
                if (cpRes.errorId == 0)
                {
                    captchaText = cpRes.solution.text;
                }
                else
                {
                    return false;
                }


            }

            var res = SubmitLoginForm(captchaText);
            if(res.Item1)
            {
                IsLogin = true;
            }
            LoginError = res.Item2;
            return res.Item1;
        }


        public string GetCardHolderName(string cardNumber)
        {
            if (!Refresh())
            {
                return "Not Login";
            }
            Thread.Sleep(1000);
            GotoPage(TejaratMenuEnum.CartBekart);

            var request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            request.Headers.Add("Origin", "https://ib.tejaratbank.ir");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            Uri target = new Uri(InterBankBasePath);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });

            byte[] byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=addForm%3ApaymentCard&javax.faces.partial.execute=addForm%3ApaymentCard+addForm%3AaddForm_client_id&javax.faces.behavior.event=change&javax.faces.partial.event=change&_client_id={0}&addForm%3ApaymentCard_focus=&addForm%3ApaymentCard_input=4830219&fakepasswordremembered=&addForm%3AsecondCardPin=&addForm%3Acvv2=&addForm%3AexpireDateMonth=&addForm%3AexpireDateYear=&addForm%3AdestinationCardNumber=&addForm%3AamountInput_input=&addForm%3AamountInput_hinput=&addForm%3Adescription=&addForm_SUBMIT=1&javax.faces.ViewState={1}", ClientID, ViewState));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));

            DoAction(responseText);



            request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            request.Headers.Add("Origin", "https://ib.tejaratbank.ir");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            target = new Uri(InterBankBasePath);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });

            byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=addForm%3ApaymentCard&javax.faces.partial.execute=addForm%3ApaymentCard+addForm%3AaddForm_client_id&javax.faces.partial.render=addForm%3AcurrencyLabel+addForm%3Abalance&javax.faces.behavior.event=change&javax.faces.partial.event=change&_client_id={0}&addForm%3ApaymentCard_focus=&addForm%3ApaymentCard_input=4830219&fakepasswordremembered=&addForm%3AsecondCardPin=&addForm%3Acvv2=&addForm%3AexpireDateMonth=&addForm%3AexpireDateYear=&addForm%3AdestinationCardNumber=&addForm%3AamountInput_input=&addForm%3AamountInput_hinput=&addForm%3Adescription=&addForm_SUBMIT=1&javax.faces.ViewState={1}", ClientID, ViewState));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            response = (HttpWebResponse)request.GetResponse();
            responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));

            DoAction(responseText);





            request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            request.Headers.Add("Origin", "https://ib.tejaratbank.ir");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            target = new Uri(InterBankBasePath);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });

            byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=addForm%3AdestinationCardNumber&primefaces.ignoreautoupdate=true&javax.faces.partial.execute=addForm%3AdestinationCardNumber&javax.faces.partial.render=addForm%3AsendSms&javax.faces.behavior.event=change&javax.faces.partial.event=change&_client_id={0}&addForm%3ApaymentCard_focus=&addForm%3ApaymentCard_input=4830219&fakepasswordremembered=&addForm%3AsecondCardPin={1}&addForm%3Acvv2={2}&addForm%3AexpireDateMonth={3}&addForm%3AexpireDateYear={4}&addForm%3AdestinationCardNumber=&addForm%3AamountInput_input=&addForm%3AamountInput_hinput=&addForm%3Adescription=&addForm_SUBMIT=1&javax.faces.ViewState={5}", ClientID, InternetPassword, Cvv2, ExpireMoth, ExpireYear, ViewState));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            response = (HttpWebResponse)request.GetResponse();
            responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));

            DoAction(responseText);





            request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            request.Headers.Add("Origin", "https://ib.tejaratbank.ir");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            target = new Uri(InterBankBasePath);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });


            byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=addForm%3AtermsAcceptance&javax.faces.partial.execute=addForm%3AtermsAcceptance&javax.faces.partial.render=addForm%3Abuttons&javax.faces.behavior.event=change&javax.faces.partial.event=change&_client_id={0}&addForm%3ApaymentCard_focus=&addForm%3ApaymentCard_input=4830219&fakepasswordremembered=&addForm%3AsecondCardPin={1}&addForm%3Acvv2={2}&addForm%3AexpireDateMonth={3}&addForm%3AexpireDateYear={4}&addForm%3AdestinationCardNumber={5}&addForm%3AamountInput_input={6}&addForm%3AamountInput_hinput={7}&addForm%3Adescription=&addForm%3AtermsAcceptance_input=on&addForm_SUBMIT=1&javax.faces.ViewState={8}", ClientID, InternetPassword, Cvv2, ExpireMoth, ExpireYear, string.Join("-", SplitInParts(cardNumber, 4)), (100000).ToString("#,##0"), 100000, ViewState));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            response = (HttpWebResponse)request.GetResponse();
            responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));

            DoAction(responseText);


            request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            request.Headers.Add("Origin", "https://ib.tejaratbank.ir");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            target = new Uri(InterBankBasePath);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });


            byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=addForm%3Aj_id584239484_4_7e5d8e8f&javax.faces.partial.execute=%40all&javax.faces.partial.render=messagesPanel+addForm&addForm%3Aj_id584239484_4_7e5d8e8f=addForm%3Aj_id584239484_4_7e5d8e8f&_client_id={0}&addForm%3ApaymentCard_focus=&addForm%3ApaymentCard_input=4830219&fakepasswordremembered=&addForm%3AsecondCardPin={1}&addForm%3Acvv2={2}&addForm%3AexpireDateMonth={3}&addForm%3AexpireDateYear={4}&addForm%3AdestinationCardNumber={5}&addForm%3AamountInput_input={6}&addForm%3AamountInput_hinput={7}&addForm%3Adescription=&addForm%3AtermsAcceptance_input=on&addForm_SUBMIT=1&javax.faces.ViewState={8}", ClientID, InternetPassword, Cvv2, ExpireMoth, ExpireYear, string.Join("-", SplitInParts(cardNumber, 4)), (100000).ToString("#,##0"), 100000, ViewState));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            response = (HttpWebResponse)request.GetResponse();
            responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));

            if (responseText.Contains("اطلاعات کارت نادرست است"))
            {
                return "اطلاعات کارت نادرست است";
            }
            var redirectUrl = DoAction(responseText);
            if (redirectUrl == "" || !redirectUrl.Contains("/web/s/cardtocardfundtransfer"))
            {
                return "";
            }

            request = (HttpWebRequest)WebRequest.Create(redirectUrl);
            request.AllowAutoRedirect = true;
            request.Method = "GET";
            request.CookieContainer = new CookieContainer();
            target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie("localCode", "fa") { Domain = target.Host, Path = "/", HttpOnly = true });
            request.UserAgent = UserAgent;
            request.Accept = Accept;
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.9");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = LastReferrer;
            request.Host = "ib.tejaratbank.ir";



            response = (HttpWebResponse)request.GetResponse();
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);
            LastResponseString = responseText;
            ViewState = doc.GetElementbyId("javax.faces.ViewState").GetAttributeValue("value", "");
            LastReferrer = response.ResponseUri.ToString();
            ClientID = doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "_client_id").FirstOrDefault().GetAttributeValue("value", "");


            return doc.DocumentNode.Descendants("table").Where(x => x.GetAttributeValue("class", "") == "ui-panelgrid ui-widget data_attention_main_right attention_box full-width").First().ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText;
        }

        public Tuple<bool, string> CardTransfer(string toCardNumber, long amount)
        {
            if (!Refresh())
            {
                return new Tuple<bool, string>(false,"Not Login");
            }
            Thread.Sleep(1000);
            GotoPage(TejaratMenuEnum.CartBekart);

            var request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            request.Headers.Add("Origin", "https://ib.tejaratbank.ir");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            Uri target = new Uri(InterBankBasePath);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });

            byte[] byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=addForm%3ApaymentCard&javax.faces.partial.execute=addForm%3ApaymentCard+addForm%3AaddForm_client_id&javax.faces.behavior.event=change&javax.faces.partial.event=change&_client_id={0}&addForm%3ApaymentCard_focus=&addForm%3ApaymentCard_input=4830219&fakepasswordremembered=&addForm%3AsecondCardPin=&addForm%3Acvv2=&addForm%3AexpireDateMonth=&addForm%3AexpireDateYear=&addForm%3AdestinationCardNumber=&addForm%3AamountInput_input=&addForm%3AamountInput_hinput=&addForm%3Adescription=&addForm_SUBMIT=1&javax.faces.ViewState={1}", ClientID, ViewState));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));

            DoAction(responseText);



            request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            request.Headers.Add("Origin", "https://ib.tejaratbank.ir");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            target = new Uri(InterBankBasePath);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });

            byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=addForm%3ApaymentCard&javax.faces.partial.execute=addForm%3ApaymentCard+addForm%3AaddForm_client_id&javax.faces.partial.render=addForm%3AcurrencyLabel+addForm%3Abalance&javax.faces.behavior.event=change&javax.faces.partial.event=change&_client_id={0}&addForm%3ApaymentCard_focus=&addForm%3ApaymentCard_input=4830219&fakepasswordremembered=&addForm%3AsecondCardPin=&addForm%3Acvv2=&addForm%3AexpireDateMonth=&addForm%3AexpireDateYear=&addForm%3AdestinationCardNumber=&addForm%3AamountInput_input=&addForm%3AamountInput_hinput=&addForm%3Adescription=&addForm_SUBMIT=1&javax.faces.ViewState={1}", ClientID, ViewState));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            response = (HttpWebResponse)request.GetResponse();
            responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));

            DoAction(responseText);





            request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            request.Headers.Add("Origin", "https://ib.tejaratbank.ir");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            target = new Uri(InterBankBasePath);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });

            byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=addForm%3AdestinationCardNumber&primefaces.ignoreautoupdate=true&javax.faces.partial.execute=addForm%3AdestinationCardNumber&javax.faces.partial.render=addForm%3AsendSms&javax.faces.behavior.event=change&javax.faces.partial.event=change&_client_id={0}&addForm%3ApaymentCard_focus=&addForm%3ApaymentCard_input=4830219&fakepasswordremembered=&addForm%3AsecondCardPin={1}&addForm%3Acvv2={2}&addForm%3AexpireDateMonth={3}&addForm%3AexpireDateYear={4}&addForm%3AdestinationCardNumber=&addForm%3AamountInput_input=&addForm%3AamountInput_hinput=&addForm%3Adescription=&addForm_SUBMIT=1&javax.faces.ViewState={5}", ClientID,InternetPassword,Cvv2,ExpireMoth,ExpireYear, ViewState));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            response = (HttpWebResponse)request.GetResponse();
            responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));

            DoAction(responseText);





            request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            request.Headers.Add("Origin", "https://ib.tejaratbank.ir");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            target = new Uri(InterBankBasePath);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });


            byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=addForm%3AtermsAcceptance&javax.faces.partial.execute=addForm%3AtermsAcceptance&javax.faces.partial.render=addForm%3Abuttons&javax.faces.behavior.event=change&javax.faces.partial.event=change&_client_id={0}&addForm%3ApaymentCard_focus=&addForm%3ApaymentCard_input=4830219&fakepasswordremembered=&addForm%3AsecondCardPin={1}&addForm%3Acvv2={2}&addForm%3AexpireDateMonth={3}&addForm%3AexpireDateYear={4}&addForm%3AdestinationCardNumber={5}&addForm%3AamountInput_input={6}&addForm%3AamountInput_hinput={7}&addForm%3Adescription=&addForm%3AtermsAcceptance_input=on&addForm_SUBMIT=1&javax.faces.ViewState={8}", ClientID, InternetPassword,Cvv2,ExpireMoth,ExpireYear,string.Join("-",SplitInParts(toCardNumber, 4)),amount.ToString("#,##0"),amount,ViewState));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            response = (HttpWebResponse)request.GetResponse();
            responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));

            DoAction(responseText);


            request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            request.Headers.Add("Origin", "https://ib.tejaratbank.ir");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            target = new Uri(InterBankBasePath);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });


            byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=addForm%3Aj_id584239484_4_7e5d8e8f&javax.faces.partial.execute=%40all&javax.faces.partial.render=messagesPanel+addForm&addForm%3Aj_id584239484_4_7e5d8e8f=addForm%3Aj_id584239484_4_7e5d8e8f&_client_id={0}&addForm%3ApaymentCard_focus=&addForm%3ApaymentCard_input=4830219&fakepasswordremembered=&addForm%3AsecondCardPin={1}&addForm%3Acvv2={2}&addForm%3AexpireDateMonth={3}&addForm%3AexpireDateYear={4}&addForm%3AdestinationCardNumber={5}&addForm%3AamountInput_input={6}&addForm%3AamountInput_hinput={7}&addForm%3Adescription=&addForm%3AtermsAcceptance_input=on&addForm_SUBMIT=1&javax.faces.ViewState={8}", ClientID, InternetPassword, Cvv2, ExpireMoth, ExpireYear, string.Join("-", SplitInParts(toCardNumber, 4)), amount.ToString("#,##0"), amount, ViewState));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            response = (HttpWebResponse)request.GetResponse();
            responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));
            if(responseText.Contains("اطلاعات کارت نادرست است"))
            {
                return new Tuple<bool, string>(false, "اطلاعات کارت نادرست است");
            }

            var redirectUrl = DoAction(responseText);
            if (redirectUrl == "" || !redirectUrl.Contains("/web/s/cardtocardfundtransfer"))
            {
                return new Tuple<bool, string>(false, "-2");
            }

            request = (HttpWebRequest)WebRequest.Create(redirectUrl);
            request.AllowAutoRedirect = true;
            request.Method = "GET";
            request.CookieContainer = new CookieContainer();
            target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie("localCode", "fa") { Domain = target.Host, Path = "/", HttpOnly = true });
            request.UserAgent = UserAgent;
            request.Accept = Accept;
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.9");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = LastReferrer;
            request.Host = "ib.tejaratbank.ir";



            response = (HttpWebResponse)request.GetResponse();
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);
            LastResponseString = responseText;
            ViewState = doc.GetElementbyId("javax.faces.ViewState").GetAttributeValue("value", "");
            LastReferrer = response.ResponseUri.ToString();
            ClientID = doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "_client_id").FirstOrDefault().GetAttributeValue("value", "");




            request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            request.Headers.Add("Origin", "https://ib.tejaratbank.ir");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            target = new Uri(InterBankBasePath);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });


            byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=confirmForm%3Aj_id1157097414_3_50a9cec7&javax.faces.partial.execute=%40all&javax.faces.partial.render=messagesPanel+confirmForm&confirmForm%3Aj_id1157097414_3_50a9cec7=confirmForm%3Aj_id1157097414_3_50a9cec7&_client_id={0}&confirmForm_SUBMIT=1&javax.faces.ViewState={1}", ClientID, ViewState));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            response = (HttpWebResponse)request.GetResponse();
            responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));

            redirectUrl = DoAction(responseText);
            if (redirectUrl == "" || !redirectUrl.Contains("/web/s/cardtocardfundtransfer"))
            {
                return new Tuple<bool, string>(false, "-2");
            }


            request = (HttpWebRequest)WebRequest.Create(redirectUrl);
            request.AllowAutoRedirect = true;
            request.Method = "GET";
            request.CookieContainer = new CookieContainer();
            target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie("localCode", "fa") { Domain = target.Host, Path = "/", HttpOnly = true });
            request.UserAgent = UserAgent;
            request.Accept = Accept;
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.9");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = LastReferrer;
            request.Host = "ib.tejaratbank.ir";



            response = (HttpWebResponse)request.GetResponse();
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);
            LastResponseString = responseText;
            ViewState = doc.GetElementbyId("javax.faces.ViewState").GetAttributeValue("value", "");
            LastReferrer = response.ResponseUri.ToString();
            ClientID = doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "_client_id").FirstOrDefault().GetAttributeValue("value", "");

            if(doc.DocumentNode.Descendants("span").Where(x=>x.GetAttributeValue("class","")== "ui-messages-error-summary").FirstOrDefault()==null)
            {
                var tableObj = doc.DocumentNode.Descendants("table").Where(x => x.GetAttributeValue("class", "") == "ui-panelgrid ui-widget data_attention_main_right attention_box full-width").First();
                return new Tuple<bool, string>(true, tableObj.ChildNodes[0].ChildNodes[5].ChildNodes[1].InnerText + " | " + tableObj.ChildNodes[0].ChildNodes[6].ChildNodes[1].InnerText);
            }
            else
            {
                return new Tuple<bool, string>(false, doc.DocumentNode.Descendants("span").Where(x => x.GetAttributeValue("class", "") == "ui-messages-error-summary").FirstOrDefault().InnerText);
            }

            

        }

        private IEnumerable<String> SplitInParts(String s, Int32 partLength)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

        public Tuple<bool, string> NormalTransfer(string toDepositNumber, long amount, string sourceComment = "", string destinationComment = "")
        {

            if (!Refresh())
            {
                return new Tuple<bool, string>(false, "Not Login");
            }
            Thread.Sleep(1000);
            GotoPage(TejaratMenuEnum.NormalTransfer);

            var request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            request.Headers.Add("Origin", "https://ib.tejaratbank.ir");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            Uri target = new Uri(InterBankBasePath);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });

            var persianDateParst = DateConvertor.MiladiToShamsi(DateTime.Now).Split('/');
            byte[] byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=addForm%3AdestinationAccount&javax.faces.partial.execute=addForm%3AdestinationAccount&javax.faces.partial.render=addForm%3AdestinationAccount&addForm%3AdestinationAccount=addForm%3AdestinationAccount&addForm%3AdestinationAccount_query={0}&_client_id={1}&addForm%3AsourceAccountNumber_focus=&addForm%3AsourceAccountNumber_input=4830219&addForm%3AdestinationAccount_input={0}&addForm%3AdestinationAccount_hinput={0}&addForm%3AamountInput_input=&addForm%3AamountInput_hinput=&addForm%3Aj_id1140176924_3_3d4d57f9%3AsabapersianCalendar%3Aj_id1383123172_76c74d68_input={2}%2F{3}%2F{4}&addForm%3Adescription=&addForm_SUBMIT=1&javax.faces.ViewState={1}",toDepositNumber, ClientID,persianDateParst[0],persianDateParst[1],persianDateParst[2], ViewState));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));

            DoAction(responseText);



            request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            request.Headers.Add("Origin", "https://ib.tejaratbank.ir");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            target = new Uri(InterBankBasePath);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });

            byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=addForm%3Aj_id1140176924_3_3d4d41bb&javax.faces.partial.execute=%40all&javax.faces.partial.render=addForm+messagesPanel&addForm%3Aj_id1140176924_3_3d4d41bb=addForm%3Aj_id1140176924_3_3d4d41bb&_client_id={0}&addForm%3AsourceAccountNumber_focus=&addForm%3AsourceAccountNumber_input=4830219&addForm%3AdestinationAccount_input={1}&addForm%3AdestinationAccount_hinput={1}&addForm%3AamountInput_input={2}&addForm%3AamountInput_hinput={3}&addForm%3Aj_id1140176924_3_3d4d57f9%3AsabapersianCalendar%3Aj_id1383123172_76c74d68_input={4}%2F{5}%2F{6}&addForm%3Adescription={7}&addForm_SUBMIT=1&javax.faces.ViewState={8}", ClientID, toDepositNumber, amount.ToString("#,##0"),amount,persianDateParst[0],persianDateParst[1],persianDateParst[2],sourceComment,ViewState));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            response = (HttpWebResponse)request.GetResponse();
            responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));
            if(responseText.Contains("مشتری مورد نظر در سیستم وجود ندارد"))
            {
                return new Tuple<bool, string>(false, "مشتری مورد نظر در سیستم وجود ندارد");
            }

            var redirectUrl = DoAction(responseText);
            if (redirectUrl == "" || !redirectUrl.Contains("thirdpartytransfer"))
            {
                return new Tuple<bool, string>(false, "-2");
            }

            request = (HttpWebRequest)WebRequest.Create(redirectUrl);
            request.AllowAutoRedirect = true;
            request.Method = "GET";
            request.CookieContainer = new CookieContainer();
            target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie("localCode", "fa") { Domain = target.Host, Path = "/", HttpOnly = true });
            request.UserAgent = UserAgent;
            request.Accept = Accept;
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.9");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = LastReferrer;
            request.Host = "ib.tejaratbank.ir";



            response = (HttpWebResponse)request.GetResponse();
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);
            LastResponseString = responseText;
            ViewState = doc.GetElementbyId("javax.faces.ViewState").GetAttributeValue("value", "");
            LastReferrer = response.ResponseUri.ToString();
            ClientID = doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "_client_id").FirstOrDefault().GetAttributeValue("value", "");





            request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            request.Headers.Add("Origin", "https://ib.tejaratbank.ir");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            target = new Uri(InterBankBasePath);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });

            byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=thirdPartyTransferForm%3Aj_id1058297084_1_3f64473c&javax.faces.partial.execute=%40all&javax.faces.partial.render=thirdPartyTransferForm+messagesPanel&thirdPartyTransferForm%3Aj_id1058297084_1_3f64473c=thirdPartyTransferForm%3Aj_id1058297084_1_3f64473c&_client_id={0}&fakeusernameremembered={1}&fakepasswordremembered=&thirdPartyTransferForm%3Apassword={2}&thirdPartyTransferForm_SUBMIT=1&javax.faces.ViewState={3}", ClientID, Username,Pin2,ViewState));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            response = (HttpWebResponse)request.GetResponse();
            responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));

            redirectUrl = DoAction(responseText);
            if (redirectUrl == "" || !redirectUrl.Contains("thirdpartytransfer"))
            {
                return new Tuple<bool, string>(false, "-2");
            }

            request = (HttpWebRequest)WebRequest.Create(redirectUrl);
            request.AllowAutoRedirect = true;
            request.Method = "GET";
            request.CookieContainer = new CookieContainer();
            target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie("localCode", "fa") { Domain = target.Host, Path = "/", HttpOnly = true });
            request.UserAgent = UserAgent;
            request.Accept = Accept;
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.9");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = LastReferrer;
            request.Host = "ib.tejaratbank.ir";



            response = (HttpWebResponse)request.GetResponse();
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
             doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);
            LastResponseString = responseText;
            ViewState = doc.GetElementbyId("javax.faces.ViewState").GetAttributeValue("value", "");
            LastReferrer = response.ResponseUri.ToString();
            ClientID = doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "_client_id").FirstOrDefault().GetAttributeValue("value", "");


            if (doc.DocumentNode.Descendants("span").Where(x => x.GetAttributeValue("class", "") == "ui-messages-error-summary").FirstOrDefault() == null)
            {
                var tableObj = doc.DocumentNode.Descendants("table").Where(x => x.GetAttributeValue("class", "") == "ui-panelgrid ui-widget data_attention_main_right attention_box full-width").First();
                return new Tuple<bool, string>(true, tableObj.ChildNodes[0].ChildNodes[4].ChildNodes[1].InnerText + " | " + tableObj.ChildNodes[0].ChildNodes[5].ChildNodes[1].InnerText);
            }
            else
            {
                return new Tuple<bool, string>(false, doc.DocumentNode.Descendants("span").Where(x => x.GetAttributeValue("class", "") == "ui-messages-error-summary").FirstOrDefault().InnerText);
            }


            

        }
        public List<CardTransferHistoryModel> GetCartTransferHistory(DateTime fromDate, DateTime toDate)
        {
            if (!Refresh())
            {
                return new List<CardTransferHistoryModel>() ;
            }
            Thread.Sleep(1000);
            GotoPage(TejaratMenuEnum.SooratHesab);

            var request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.AllowAutoRedirect = true;

            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            //origin
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");

            request.CookieContainer = new CookieContainer();
            Uri target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie("localCode", "fa") { Domain = target.Host, Path = "/", HttpOnly = true });


            byte[] byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=pageForm%3Aj_id1684604116_3_52941591&javax.faces.partial.execute=%40all&javax.faces.partial.render=messagesPanel+pageForm%3AaccountHistpryItemsPanel&pageForm%3Aj_id1684604116_3_52941591=pageForm%3Aj_id1684604116_3_52941591&_client_id={0}&pageForm%3AhistoryType=10&pageForm%3AaccountSelect_focus=&pageForm%3AaccountSelect={1}&pageForm%3AcustomRadio=dateTime&pageForm%3AfromDate%3AsabapersianCalendar%3Aj_id1365051236_76c74d68_input={2}&pageForm%3AtoDate%3AsabapersianCalendar%3Aj_id1365051236_1_76c74d68_1_input={3}&pageForm%3AfromTime_input={4}&pageForm%3AtoTime_input={5}&pageForm%3AtransactionTypeSelect_focus=&pageForm%3AtransactionTypeSelect_input=&pageForm_SUBMIT=1&javax.faces.ViewState={6}", ClientID, DepositNumber,DateConvertor.MiladiToShamsi(fromDate).Replace(":","%2f"),DateConvertor.MiladiToShamsi(toDate).Replace("/", "%2f"),fromDate.ToString("HH:mm").Replace(":", "%3A"), toDate.ToString("HH:mm").Replace(":", "%3A"),ViewState));
            //byte[] byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=pageForm%3Aj_id1684604116_3_529415ee&javax.faces.partial.execute=%40all&javax.faces.partial.render=messagesPanel+pageForm%3AaccountHistpryItemsPanel&pageForm%3Aj_id1684604116_3_529415ee=pageForm%3Aj_id1684604116_3_529415ee&_client_id={0}&pageForm%3AhistoryType=10&pageForm%3AaccountSelect_focus=&pageForm%3AaccountSelect={1}&pageForm%3AcustomRadio=dateTime&pageForm%3AfromDate%3AsabapersianCalendar%3Aj_id1365051236_76c74d68_input={2}&pageForm%3AtoDate%3AsabapersianCalendar%3Aj_id1365051236_1_76c74d68_1_input={3}&pageForm%3AfromTime_input={4}&pageForm%3AtoTime_input={5}&pageForm%3AtransactionTypeSelect_focus=&pageForm%3AtransactionTypeSelect_input=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AaccountNumber%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AtransactionDate%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AtransactionTime%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3Abalance%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AtransactionAmountDebit%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AtransactionAmountCredit%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AfirstLangOperationDesc%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AextraInfo%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AtrackId%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AdocNumber%3Afilter=&pageForm%3AcbAccountHistoryTab_activeIndex=0&pageForm_SUBMIT=1&javax.faces.ViewState={6}",ClientID, DepositNumber,DateConvertor.MiladiToShamsi(fromDate).Replace(":","%2f"),DateConvertor.MiladiToShamsi(toDate).Replace("/", "%2f"),fromDate.ToString("HH:mm").Replace(":", "%3A"), toDate.ToString("HH:mm").Replace(":", "%3A"),ViewState));
            //byte[] byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=pageForm%3AcbAccountHistoryTab%3AdataTable&javax.faces.partial.execute=pageForm%3AcbAccountHistoryTab%3AdataTable&javax.faces.partial.render=pageForm%3AcbAccountHistoryTab%3AdataTable&javax.faces.behavior.event=page&javax.faces.partial.event=page&pageForm%3AcbAccountHistoryTab%3AdataTable_pagination=true&pageForm%3AcbAccountHistoryTab%3AdataTable_first=0&pageForm%3AcbAccountHistoryTab%3AdataTable_rows=50&pageForm%3AcbAccountHistoryTab%3AdataTable_encodeFeature=true&_client_id={0}&pageForm%3AhistoryType=10&pageForm%3AaccountSelect_focus=&pageForm%3AaccountSelect={1}&pageForm%3AcustomRadio=dateTime&pageForm%3AfromDate%3AsabapersianCalendar%3Aj_id1365051236_76c74d68_input={2}&pageForm%3AtoDate%3AsabapersianCalendar%3Aj_id1365051236_1_76c74d68_1_input={3}&pageForm%3AfromTime_input={4}&pageForm%3AtoTime_input={5}&pageForm%3AtransactionTypeSelect_focus=&pageForm%3AtransactionTypeSelect_input=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AaccountNumber%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AtransactionDate%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AtransactionTime%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3Abalance%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AtransactionAmountDebit%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AtransactionAmountCredit%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AfirstLangOperationDesc%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AextraInfo%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AtrackId%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AdocNumber%3Afilter=&pageForm%3AcbAccountHistoryTab_activeIndex=0&pageForm_SUBMIT=1&javax.faces.ViewState={6}", ClientID, DepositNumber, fromDate.ToString("yyyy:MM:dd").Replace(":", "%2f"), toDate.ToString("yyyy/MM/dd").Replace("/", "%2f"), fromDate.ToString("HH:mm").Replace(":", "%3A"), toDate.ToString("HH:mm").Replace(":", "%3A"), ViewState));
            
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream())).Replace("\"", "'");

            DoAction(responseText);

            request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.AllowAutoRedirect = true;

            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            //origin
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");

            request.CookieContainer = new CookieContainer();
            target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie("localCode", "fa") { Domain = target.Host, Path = "/", HttpOnly = true });


            
            byteArray = Encoding.UTF8.GetBytes(string.Format("javax.faces.partial.ajax=true&javax.faces.source=pageForm%3AcbAccountHistoryTab%3AdataTable&javax.faces.partial.execute=pageForm%3AcbAccountHistoryTab%3AdataTable&javax.faces.partial.render=pageForm%3AcbAccountHistoryTab%3AdataTable&javax.faces.behavior.event=page&javax.faces.partial.event=page&pageForm%3AcbAccountHistoryTab%3AdataTable_pagination=true&pageForm%3AcbAccountHistoryTab%3AdataTable_first=0&pageForm%3AcbAccountHistoryTab%3AdataTable_rows=50&pageForm%3AcbAccountHistoryTab%3AdataTable_encodeFeature=true&_client_id={0}&pageForm%3AhistoryType=10&pageForm%3AaccountSelect_focus=&pageForm%3AaccountSelect={1}&pageForm%3AcustomRadio=dateTime&pageForm%3AfromDate%3AsabapersianCalendar%3Aj_id1365051236_76c74d68_input={2}&pageForm%3AtoDate%3AsabapersianCalendar%3Aj_id1365051236_1_76c74d68_1_input={3}&pageForm%3AfromTime_input={4}&pageForm%3AtoTime_input={5}&pageForm%3AtransactionTypeSelect_focus=&pageForm%3AtransactionTypeSelect_input=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AaccountNumber%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AtransactionDate%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AtransactionTime%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3Abalance%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AtransactionAmountDebit%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AtransactionAmountCredit%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AfirstLangOperationDesc%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AextraInfo%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AtrackId%3Afilter=&pageForm%3AcbAccountHistoryTab%3AdataTable%3AdocNumber%3Afilter=&pageForm%3AcbAccountHistoryTab_activeIndex=0&pageForm_SUBMIT=1&javax.faces.ViewState={6}", ClientID, DepositNumber, DateConvertor.MiladiToShamsi(fromDate).Replace(":", "%2f"),DateConvertor.MiladiToShamsi(toDate).Replace("/", "%2f"), fromDate.ToString("HH:mm").Replace(":", "%3A"), toDate.ToString("HH:mm").Replace(":", "%3A"), ViewState));

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            response = (HttpWebResponse)request.GetResponse();

            responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream())).Replace("\"","'");
            if(responseText.Contains("pageForm:cbAccountHistoryTab:dataTable"))
            {
                responseText = responseText.Replace("<?xml version='1.0' encoding='utf-8'?><partial-response><changes><update id='pageForm:cbAccountHistoryTab:dataTable'><![CDATA[", "").Replace(string.Format("]]></update><update id='javax.faces.ViewState'><![CDATA[{0}]]></update></changes></partial-response>",ViewState), "");
            }
            else
            {
                return new List<CardTransferHistoryModel>();
            }


            List<CardTransferHistoryModel> cardTransfers = new List<CardTransferHistoryModel>();
             HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
             doc.LoadHtml(responseText);

             foreach (var tr in doc.DocumentNode.Descendants("tr"))
             {
                 try
                 {
                     if (tr.Descendants("td").Count() != 0)
                     {
                         CardTransferHistoryModel instance = new CardTransferHistoryModel();
                         instance.Date = Convert.ToDateTime(DateConvertor.ShamsiToMiladi((tr.Descendants("td").ElementAt(2).InnerText)).ToShortDateString() + " " + Convert.ToDateTime(tr.Descendants("td").ElementAt(3).InnerText).ToString("HH:mm:ss"));
                         instance.Description = tr.Descendants("td").ElementAt(7).InnerText + " | "+ tr.Descendants("td").ElementAt(8).InnerText;
                        if(tr.Descendants("td").ElementAt(6).InnerText!="")
                        {
                            instance.AmountIn = Convert.ToInt64(tr.Descendants("td").ElementAt(6).InnerText.Replace(",", ""));
                        }
                        else
                        {
                            instance.AmountIn = 0;
                        }
                        if (tr.Descendants("td").ElementAt(5).InnerText != "")
                        {
                            instance.AmountOut = Convert.ToInt64(tr.Descendants("td").ElementAt(5).InnerText.Replace(",", ""));
                        }
                        else
                        {
                            instance.AmountOut = 0;
                        }
                        
                         instance.DocumentNumber = tr.Descendants("td").ElementAt(10).InnerText+ tr.Descendants("td").ElementAt(9).InnerText;
                         instance.CodeErja = "";
                         instance.CodePeygiri = tr.Descendants("td").ElementAt(9).InnerText;
                         instance.CardNumber = "";
                        /*if (instance.Description.Contains("ازکارت"))
                        {
                           instance.CardNumber = tr.Descendants("td").ElementAt(8).InnerText.Replace("ازکارت", "").Trim().Split(' ')[0];
                        }
                       else if (instance.Description.Contains("از کارت"))
                       {
                           instance.CardNumber = tr.Descendants("td").ElementAt(8).InnerText.Replace("از کارت", "").Trim().Split(' ')[0];
                       }*/
                        var matches = Regex.Matches(tr.Descendants("td").ElementAt(8).InnerText, @"[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]", RegexOptions.IgnoreCase);
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
        public Tuple<long, long, string> GetBalance()
        {
            if (!Refresh())
            {
                return new Tuple<long, long, string>(-1,-1,"Not login");
            }
            Thread.Sleep(1000);

            var request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.AllowAutoRedirect = true;

            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            //origin
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");

            request.CookieContainer = new CookieContainer();
            Uri target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie("localCode", "fa") { Domain = target.Host, Path = "/", HttpOnly = true });


            byte[] byteArray = Encoding.UTF8.GetBytes(GetAjaxData(TejaratMenuEnum.MoshahedeHesab));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));

            string redirectUrl = DoAction(responseText);
            if (redirectUrl=="")
            {
                return new Tuple<long, long, string>(-1, -1, "");
            }




            request = (HttpWebRequest)WebRequest.Create(redirectUrl);
            request.AllowAutoRedirect = true;
            request.Method = "GET";
            request.CookieContainer = new CookieContainer();
            target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie("localCode", "fa") { Domain = target.Host, Path = "/", HttpOnly = true });
            request.UserAgent = UserAgent;
            request.Accept = Accept;
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.9");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = LastReferrer;
            request.Host = "ib.tejaratbank.ir";



            response = (HttpWebResponse)request.GetResponse();
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            LastResponseString = responseText;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);
            ViewState = doc.GetElementbyId("javax.faces.ViewState").GetAttributeValue("value", "");
            if (response.ResponseUri.ToString().Contains("accountoverview"))
            {

                LastReferrer = response.ResponseUri.ToString();
                ClientID = doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "_client_id").FirstOrDefault().GetAttributeValue("value", "");

                return new Tuple<long, long, string>(Convert.ToInt32(doc.GetElementbyId("form:dataTable_data").ChildNodes[0].ChildNodes[5].InnerText.Replace(",","")), -1, "");
            }
            else
            {
                return new Tuple<long, long, string>(-1, -1, "");
            }
        }
        private void GotoPage(TejaratMenuEnum type)
        {

            var request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.AllowAutoRedirect = true;

            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            //origin
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");

            request.CookieContainer = new CookieContainer();
            Uri target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie("localCode", "fa") { Domain = target.Host, Path = "/", HttpOnly = true });


            byte[] byteArray = Encoding.UTF8.GetBytes(GetAjaxData(type));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));

            string redirectUrl = DoAction(responseText);
            if (redirectUrl=="")
            {
                return;
            }

            request = (HttpWebRequest)WebRequest.Create(redirectUrl);
            request.AllowAutoRedirect = true;
            request.Method = "GET";
            request.CookieContainer = new CookieContainer();
            target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie("localCode", "fa") { Domain = target.Host, Path = "/", HttpOnly = true });
            request.UserAgent = UserAgent;
            request.Accept = Accept;
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.9");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = LastReferrer;
            request.Host = "ib.tejaratbank.ir";



            response = (HttpWebResponse)request.GetResponse();
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);
            LastResponseString = responseText;
            ViewState = doc.GetElementbyId("javax.faces.ViewState").GetAttributeValue("value", "");
            LastReferrer = response.ResponseUri.ToString();
            ClientID = doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "_client_id").FirstOrDefault().GetAttributeValue("value", "");

        }
        public bool Refresh()
        {

            if (LoginCookie == null || LoginCookie == "")
            {
              var res=Login();
                if(!res)
                {
                    return false;
                }
            }
            var request = (HttpWebRequest)WebRequest.Create(LastReferrer);
            request.AllowAutoRedirect = true;

            request.Method = "POST";
            request.Host = "ib.tejaratbank.ir";
            request.Accept = "application/xml, text/xml, */*; q=0.01";
            //origin
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Faces-Request", "partial/ajax");
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = LastReferrer;
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");

            request.CookieContainer = new CookieContainer();
            Uri target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie("localCode", "fa") { Domain = target.Host, Path = "/", HttpOnly = true });


            byte[] byteArray = Encoding.UTF8.GetBytes(GetAjaxData(TejaratMenuEnum.Main));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string responseText = Encoding.UTF8.GetString(ReadFullyAndDecodeGzip(response.GetResponseStream()));

            string redirectUrl = DoAction(responseText);
            if (redirectUrl=="")
            {
                return false;
            }
            
            request = (HttpWebRequest)WebRequest.Create(redirectUrl);
            request.AllowAutoRedirect = true;
            request.Method = "GET";
            request.CookieContainer = new CookieContainer();
            target = new Uri(InterBankBasePath);
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie("localCode", "fa") { Domain = target.Host, Path = "/", HttpOnly = true });
            request.UserAgent = UserAgent;
            request.Accept = Accept;
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.9");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Referer = LastReferrer;
            request.Host = "ib.tejaratbank.ir";

            

            response = (HttpWebResponse)request.GetResponse();
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseText);
            LastResponseString = responseText;
            ViewState = doc.GetElementbyId("javax.faces.ViewState").GetAttributeValue("value", "");
            if (responseText.Contains("بانک تجارت - بانکداری اینترنتی  :صفحه اصلی"))
            {

                LastReferrer = response.ResponseUri.ToString();
                ClientID = doc.DocumentNode.Descendants("input").Where(x => x.GetAttributeValue("name", "") == "_client_id").FirstOrDefault().GetAttributeValue("value", "");
            }
            else
            {
                IsLogin = false;
                var res = Login();
                if (!res)
                {
                    return false;
                }
            }
            return true;

        }
        private string GetAjaxData(TejaratMenuEnum type)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(LastResponseString);
            switch (type)
            {
                case TejaratMenuEnum.Main:
                    var id = doc.DocumentNode.Descendants("i").Where(x => x.GetAttributeValue("class", "") == "fa fa-home home-icon").First().ParentNode.ParentNode.GetAttributeValue("id", "");
                    var formID = id.Split(':')[0];
                    id = id.Replace(":", "%3A");
                    return string.Format("javax.faces.partial.ajax=true&javax.faces.source={0}&javax.faces.partial.execute=%40all&{0}={0}&_client_id={1}&{2}_SUBMIT=1&javax.faces.ViewState={3}",id, ClientID,formID, ViewState);
                case TejaratMenuEnum.CartBekart:
                    id = doc.DocumentNode.Descendants("a").Where(x => x.InnerText == "انتقال وجه كارت به كارت").First().GetAttributeValue("id", "");
                    formID = id.Split(':')[0];
                    id = id.Replace(":", "%3A");
                    return string.Format("javax.faces.partial.ajax=true&javax.faces.source={0}&javax.faces.partial.execute=%40all&{0}={0}&_client_id={1}&{2}_SUBMIT=1&javax.faces.ViewState={3}", id, ClientID, formID, ViewState);
                case TejaratMenuEnum.MoshahedeHesab:
                    id = doc.DocumentNode.Descendants("a").Where(x => x.InnerText == "مشاهده حساب ها").First().GetAttributeValue("id", "");
                    formID = id.Split(':')[0];
                    id = id.Replace(":", "%3A");
                    return string.Format("javax.faces.partial.ajax=true&javax.faces.source={0}&javax.faces.partial.execute=%40all&{0}={0}&_client_id={1}&{2}_SUBMIT=1&javax.faces.ViewState={3}", id, ClientID, formID, ViewState);
                case TejaratMenuEnum.SooratHesab:
                    id = doc.DocumentNode.Descendants("a").Where(x => x.InnerText == "صورت حساب").First().GetAttributeValue("id", "");
                    formID = id.Split(':')[0];
                    id = id.Replace(":", "%3A");
                    return string.Format("javax.faces.partial.ajax=true&javax.faces.source={0}&javax.faces.partial.execute=%40all&{0}={0}&_client_id={1}&{2}_SUBMIT=1&javax.faces.ViewState={3}", id, ClientID, formID, ViewState);
                case TejaratMenuEnum.NormalTransfer:
                    id = doc.DocumentNode.Descendants("a").Where(x => x.InnerText == "به حساب های دیگران").First().GetAttributeValue("id", "");
                    formID = id.Split(':')[0];
                    id = id.Replace(":", "%3A");
                    return string.Format("javax.faces.partial.ajax=true&javax.faces.source={0}&javax.faces.partial.execute=%40all&{0}={0}&_client_id={1}&{2}_SUBMIT=1&javax.faces.ViewState={3}", id, ClientID, formID, ViewState);

            }
            return "";

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
        private byte[] Decompress(byte[] gzip)
        {
            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip),CompressionMode.Decompress)){
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        private byte[] ReadFullyAndDecodeGzip(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                //File.WriteAllBytes("captcha.jpg", Decompress(ms.ToArray()));
                return Decompress(ms.ToArray());
            }
        }
        private byte[] ReadAsByteArray(Stream input)
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
            request.CookieContainer.Add(new Cookie(LoginCookie.Split('=')[0], LoginCookie.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });
            request.CookieContainer.Add(new Cookie(CookieSession1.Split('=')[0], CookieSession1.Split('=')[1]) { Domain = target.Host, Path = "/", HttpOnly = true });

            request.UserAgent = UserAgent;
            request.Accept = "image/webp,image/apng,image/*,*/*;q=0.8";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Host = "ib.tejaratbank.ir";
            request.Referer = "https://ib.tejaratbank.ir/web/ns/login?execution=e1s1";
            var response = ((HttpWebResponse)request.GetResponse());
            return Convert.ToBase64String(ReadFullyAndDecodeGzip(response.GetResponseStream()));

        }

        private enum TejaratMenuEnum
        {
            Main,
            MoshahedeHesab,
            SooratHesab,
            CartBekart,
            NormalTransfer
        }
    }
}