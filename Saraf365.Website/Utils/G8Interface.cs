using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;

namespace Saraf365.WebClient.Utils
{
    
    public class G8InterfaceApiRes
    {
        public int status { set; get; }
        public Dictionary<string, string> data { set; get; }

        public G8InterfaceApiRes()
        {
            data = new Dictionary<string, string>();
        }
    }
    public class G8Interface
    {
        public static string Url { set; get; }
        public static string ApiKey { set; get; }


        //must be urlencoded if addiitional parameter are exist
        //callback method must be called via post method
        public static string Callback { set; get; }

        public G8Interface(string url,string apiKey,string callback) {
            Url = url;
            ApiKey = apiKey;
            Callback = callback;
        }

        //amountToPay must be multiply of 500000 , like 500000 , 1000000 , 1500000 ,...
        public G8InterfaceApiRes CreatePaymentRequest(long amountToPay)
        {
            using (var client = new System.Net.WebClient())
            {
                var values = new NameValueCollection();
                values["api_key"] = ApiKey;
                values["amount"] = amountToPay.ToString();
                values["return_url"] = Callback;


                try
                {
                    var response = JsonConvert.DeserializeObject<Dictionary<string, string>>(Encoding.Default.GetString(client.UploadValues(Url + "invoice/request", values)));

                    G8InterfaceApiRes res = new G8InterfaceApiRes();
                    foreach (var item in response)
                    {
                        if (item.Key == "status")
                        {
                            res.status = Convert.ToInt32(item.Value);
                        }
                        else
                        {
                            res.data.Add(item.Key, item.Value);
                        }
                    }
                    return res;
                }
                catch { }
                {
                    G8InterfaceApiRes res = new G8InterfaceApiRes();
                    res.status = 0;
                    res.data.Add("errorCode", "-1");
                    res.data.Add("errorDescription", "Payment server is unreachable now please try again a few minutes later");
                    return res;
                }



            }
        }

        public string GetPaymentAddress(string invoice_Key)
        {
            return Url + "invoice/pay/" + invoice_Key;
        }

        public G8InterfaceApiRes CheckAndVerify(string invoice_Key)
        {
            using (var client = new System.Net.WebClient())
            {
                var values = new NameValueCollection();
                values["api_key"] = ApiKey;


                try
                {
                    var response = JsonConvert.DeserializeObject<Dictionary<string, string>>(Encoding.Default.GetString(client.UploadValues(Url + "invoice/check/" + invoice_Key, values)));

                    G8InterfaceApiRes res = new G8InterfaceApiRes();
                    foreach (var item in response)
                    {
                        if (item.Key == "status")
                        {
                            res.status = Convert.ToInt32(item.Value);
                        }
                        else
                        {
                            res.data.Add(item.Key, item.Value);
                        }
                    }
                    return res;
                }
                catch { }
                {
                    G8InterfaceApiRes res = new G8InterfaceApiRes();
                    res.status = 0;
                    res.data.Add("errorCode", "-1");
                    res.data.Add("errorDescription", "Payment server is unreachable now please try again a few minutes later");
                    return res;
                }



            }
        }
    }
}