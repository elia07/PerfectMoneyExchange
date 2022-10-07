using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saraf365.WebClient.Utils
{
    public class SessionUtils
    {
        public void AddSession(HttpSessionStateBase session,string key,object value)
        {
            var existSession = GetSessionValue<long>(session, key);
            if (existSession == 0)
            {
                RemoveSession(session, key);
            }
            session.Add(key, value);
        }

        public void RemoveSession(HttpSessionStateBase session, string key)
        {
            session.Remove(key);
        }

        public T GetSessionValue<T>(HttpSessionStateBase session, string key, bool JsonDeserialize = false)
        {
            try
            {
                if (JsonDeserialize)
                {
                    return JsonConvert.DeserializeObject<T>(session[key].ToString());
                }
                else
                {
                    return TConverter.ChangeType<T>(session[key].ToString());
                }

            }
            catch
            {
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