using Newtonsoft.Json;
using RockCandy.Web.Framework.Utilities.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saraf365.WebClient.Utils
{

    public class CookieUtils
    {
        public void AddCookie(string name, string value, int mins = 10, HttpContextBase httpContext=null)
        {
            HttpCookie ViewContent = new HttpCookie(MD5Encryption.Encrypt(string.Format("{0}-{1}",SectionInfo.Setting.ApplicationName, name),true,SectionInfo.Setting.SecurityKey));
            ViewContent.Value = MD5Encryption.Encrypt(value,true,SectionInfo.Setting.SecurityKey);
            ViewContent.Expires = DateTime.Now.AddMinutes(mins);
            if (httpContext == null)
            {
                HttpContext.Current.Response.SetCookie(ViewContent);
            }
            else
            {
                httpContext.Response.SetCookie(ViewContent);
            }
            
        }

        public void RemoveCookie(string name, HttpContextBase httpContext = null)
        {

            HttpCookie ViewContent = new HttpCookie(MD5Encryption.Decrypt(string.Format("{0}-{1}", SectionInfo.Setting.ApplicationName, name), true, SectionInfo.Setting.SecurityKey));
            ViewContent.Value = "";
            ViewContent.Expires = DateTime.Now.AddMinutes(-1);
            if (httpContext == null)
            {
                HttpContext.Current.Response.SetCookie(ViewContent);
            }
            else
            {
                httpContext.Response.SetCookie(ViewContent);
            }
            
        }

        public T GetCookieValue<T>(string name, HttpRequestBase request, bool JsonDeserialize = false)
        {
            try
            {
                if (JsonDeserialize)
                {
                    return JsonConvert.DeserializeObject<T>(MD5Encryption.Decrypt(request.Cookies[MD5Encryption.Encrypt(string.Format("{0}-{1}", SectionInfo.Setting.ApplicationName, name), true, SectionInfo.Setting.SecurityKey)].Value.ToString(), true, SectionInfo.Setting.SecurityKey));
                }
                else
                {
                    return TConverter.ChangeType<T>(MD5Encryption.Decrypt(request.Cookies[MD5Encryption.Encrypt(string.Format("{0}-{1}", SectionInfo.Setting.ApplicationName, name), true, SectionInfo.Setting.SecurityKey)].Value.ToString(), true, SectionInfo.Setting.SecurityKey));
                }
                
            }
            catch {
                if (typeof(T).IsValueType || typeof(T) == typeof(string))
                {
                    return default(T);
                }
                else
                {
                    return (T)Activator.CreateInstance(typeof(T));
                }
            }
            
        }
    }
}