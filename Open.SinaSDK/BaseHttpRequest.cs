using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web;

namespace Open.SinaSDK
{
    public abstract class BaseHttpRequest:oAuthBase,IHttpRequestMethod
    {
        private const string OAuthSignaturePattern = "OAuth oauth_consumer_key=\"{0}\", oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"{1}\", oauth_nonce=\"{2}\", oauth_version=\"1.0\", oauth_token=\"{3}\",oauth_signature=\"{4}\"";
        #region 属性
        private string _appKey;
        private string _appSecret;

        public string AppKey
        {
            get
            {
                if (string.IsNullOrEmpty(_appKey))
                {
                    _appKey = ConfigurationManager.AppSettings["sina_app_key"];
                }
                return _appKey;
            }
        }

        public string AppSecret
        {
            get
            {
                if (string.IsNullOrEmpty(_appSecret))
                {
                    _appSecret = ConfigurationManager.AppSettings["sina_app_secret"];
                }
                return _appSecret;
            }
        }
        
        public string Token { get; set; }

        public string TokenSecret { get; set; }

        private string _userid;
        public string userid
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    _userid = string.Empty;
                    if (HttpContext.Current.Session["userid"] != null)
                    {
                        _userid = HttpContext.Current.Session["userid"].ToString();
                    }
                    return _userid;
                }
                else
                {
                    return _userid;
                }
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Session["userid"] = value;
                }
                else
                {
                    _userid = value;
                }
            }
        }
        #endregion

        protected string AppendSignatureString(string method, string url, out string outUrl)
        {
            string querystring;
            var signature = GenerateSignature(url, method, out outUrl, out querystring);

            querystring += "&oauth_signature=" + signature;
            return querystring;
        }

        private string GenerateSignature(string url, string method, out string outUrl, out string querystring)
        {
            var uri = new Uri(url);

            var nonce = GenerateNonce();
            var timeStamp = GenerateTimeStamp();

            //Generate Signature
            var signature = GenerateSignature(uri,
                                          AppKey,
                                          AppSecret,
                                          Token,
                                          TokenSecret,
                                          method,
                                          timeStamp,
                                          nonce,
                                          out outUrl,
                                          out querystring);
            return HttpUtility.UrlEncode(signature);
        }

        protected static string GetHttpWebResponse(WebRequest webRequest)
        {
            StreamReader responseReader = null;
            string responseData;
            try
            {
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream(),System.Text.Encoding.UTF8);
                responseData = responseReader.ReadToEnd();
            }
            finally
            {
                webRequest.GetResponse().GetResponseStream().Close();
                responseReader.Close();
            }
            return responseData;
        }

        protected string GetAuthorizationHeader(string url, string method)
        {
            var timestamp = GenerateTimeStamp();
            var nounce = GenerateNonce();
            string normalizedString;
            string normalizedParameters;
            var signature = GenerateSignature(
                new Uri(url),
                AppKey,
                AppSecret,
                Token,
                TokenSecret,
                method,
                timestamp,
                nounce,
                out normalizedString,
                out normalizedParameters);
            signature = HttpUtility.UrlEncode(signature);
            return string.Format(
                CultureInfo.InvariantCulture,
                OAuthSignaturePattern,
                AppKey,
                timestamp,
                nounce,
                Token,
                signature);

        }
        public abstract string Request(string uri, string postData);
    }

    public interface IHttpRequestMethod
    {
        string Request(string uri, string postData);        
    }
}
