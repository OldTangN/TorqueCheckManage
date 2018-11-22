using QDDL.Comm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.DAL.Service
{
    public class HttpWebResponseUtility
    {
        public static string getValueByKeyName(string dictString, string keyName)
        {
            try
            {
                List<Dictionary<string, string>> ttt = (List<Dictionary<string, string>>)JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(dictString);
                if (ttt != null && ttt.Count > 0)
                {
                    List<string> list = ttt[0].Keys.ToList();
                    if (list[0] == "success")
                    {
                        return "success";//
                    }
                    else
                    {
                        return ttt[0]["fail"];
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(null, "getValueByKeyName(string dictString, string keyName)" + ex.Message);
                return "";
            }
        }

        public static string getValueByKeyName_return_id(string dictString, string keyName)
        {
            try
            {
                List<Dictionary<string, string>> ttt = (List<Dictionary<string, string>>)JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(dictString);
                if (ttt != null && ttt.Count > 0)
                {
                    List<string> list = ttt[0].Keys.ToList();
                    if (list[0] == "success")
                    {
                        return ttt[0]["success"];
                    }
                    else
                    {
                        return "fail";
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(null, "getValueByKeyName_return_id" + ex.Message);
                return "";
            }
        }

        public static string getValueByKeyName_not_return_guid(string dictString, string keyName)
        {
            try
            {
                List<Dictionary<string, string>> ttt = (List<Dictionary<string, string>>)JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(dictString);
                if (ttt != null && ttt.Count > 0)
                {
                    List<string> list = ttt[0].Keys.ToList();
                    if (list[0] == "success")
                    {
                        return "success";
                    }
                    else
                    {
                        return "fail";
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(null, "getValueByKeyName_not_return_guid" + ex.Message);
                return "";
            }
        }

        public static string getValueByKeyName(string dictString, out bool isSuccess)
        {
            isSuccess = false;
            try
            {
                List<Dictionary<string, object>> ttt = (List<Dictionary<string, object>>)JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(dictString);
                if (ttt != null && ttt.Count > 0)
                {
                    if (ttt[0].Keys.First().Equals("success"))
                    {
                        isSuccess = true;
                        string returnStr = ttt[0]["success"].ToString();
                        return returnStr;

                    }
                    else if (ttt[0].Keys.First().Equals("fail"))
                    {
                        return ttt[0]["fail"].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(null, "getValueByKeyName(string dictString, out bool isSuccess)" + ex.Message);
                return "";
            }
        }

        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";


        public static string PostWebResponse(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentNullException("url");
                }
                if (requestEncoding == null)
                {
                    throw new ArgumentNullException("requestEncoding");
                }
                HttpWebRequest request = null;
                //如果是发送HTTPS请求  
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    request = WebRequest.Create(url) as HttpWebRequest;
                    request.ProtocolVersion = HttpVersion.Version10;
                }
                else
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                }
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded;charset=utf8";

                if (!string.IsNullOrEmpty(userAgent))
                {
                    request.UserAgent = userAgent;
                }
                else
                {
                    request.UserAgent = DefaultUserAgent;
                }

                if (timeout.HasValue)
                {
                    request.Timeout = timeout.Value;
                }
                if (cookies != null)
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookies);
                }
                //如果需要POST数据  
                if (!(parameters == null || parameters.Count == 0))
                {
                    StringBuilder buffer = new StringBuilder();
                    int i = 0;
                    foreach (string key in parameters.Keys)
                    {
                        if (i > 0)
                        {
                            buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}",key , parameters[key]);
                        }
                        i++;
                    }
                    byte[] data = requestEncoding.GetBytes(buffer.ToString());
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string responseStr = reader.ReadToEnd();
                response.Close();
                return responseStr;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(null, "PostWebResponse" + ex.Message);
                return "";
            }
        }

        public static string GetWebRequest(string postUrl, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "GET";
                webReq.ContentType = "application/x-www-form-urlencoded";

                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));// Encoding.Default
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(null, "GetWebRequest" + ex.Message);
            }
            return ret;
        }
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
    }
}
