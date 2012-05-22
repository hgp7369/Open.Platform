using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using log4net;

namespace Open.NeteaseSDK
{
    /// <summary>
    /// 请求方式
    /// </summary>
    public enum Method
    {
        GET,
        POST,
        DELETE
    }

    /// <summary>
    /// 返回格式
    /// </summary>
    public enum Format
    {
        xml,
        json,
    }

    /// <summary>
    /// 热门转发榜时间
    /// </summary>
    public enum TopRetweetType
    {
        /// <summary>
        /// 1小时
        /// </summary>
        oneHour,
        /// <summary>
        /// 6小时
        /// </summary>
        sixHours,
        /// <summary>
        /// 1天
        /// </summary>
        oneDay,
        /// <summary>
        /// 1周
        /// </summary>
        oneWeek
    }


    #region HttpGet
    public class HttpGet : BaseHttpRequest
    {
        private const string GET = "GET";
        private const string AccessToken = "http://api.t.163.com/oauth/access_token";
        private const string AUTHORIZE = "http://api.t.163.com/oauth/authorize";
        private const string RequestToken = "http://api.t.163.com/oauth/request_token";
        private const string OauthToken = "oauth_token";
        private const string OauthTokenSecret = "oauth_token_secret";

        public override string Request(string uri, string postData)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger("Url");

            string outUrl;
            if (postData.Length > 0)
            {
                uri += "?" + postData;
            }
            var queryString = AppendSignatureString(GET, uri, out outUrl);
            if (queryString.Length > 0)
            {
                outUrl += "?" + queryString;
            }
            logger.Error("请求地址：" + outUrl);
            return WebRequest(GET, outUrl);
        }

        public void GetAccessToken()
        {
            SetTokenAndTokenSecret(AccessToken);
        }

        private void SetTokenAndTokenSecret(string url)
        {
            var response = Request(url, string.Empty);

            if (response.Length <= 0) return;
            var queryString = HttpUtility.ParseQueryString(response);
            if (queryString[OauthToken] != null)
            {
                Token = queryString[OauthToken];
            }
            if (queryString[OauthTokenSecret] != null)
            {
                TokenSecret = queryString[OauthTokenSecret];
            }
            if (queryString["user_id"] != null)
            {
                userid = queryString["user_id"];
            }

        }

        public void GetRequestToken()
        {
            SetTokenAndTokenSecret(RequestToken);
        }

        public string GetAuthorizationUrl()
        {
            var ret = string.Format("{0}?oauth_token={1}", AUTHORIZE, Token);
            return ret;
        }


        private static string WebRequest(string method, string url)
        {
            var httpWebRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            httpWebRequest.Method = method;
            httpWebRequest.ServicePoint.Expect100Continue = false;
            return GetHttpWebResponse(httpWebRequest);
        }
    }
    #endregion

    #region HttpPost
    public class HttpPost : BaseHttpRequest
    {
        private const string POST = "POST";
        private const string ContentEncoding = "iso-8859-1";

        public override string Request(string uri, string postData)
        {
            var appendUrl = AppendPostDataToUrl(postData, uri);
            string outUrl;
            var querystring = AppendSignatureString(POST, appendUrl, out outUrl);
            return WebRequest(POST, outUrl, querystring);
        }

        private static string WebRequest(string method, string url, string postData)
        {
            var httpWebRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            httpWebRequest.Method = method;
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            return GetHttpWebResponse(httpWebRequest, postData);
        }

        private static string GetHttpWebResponse(WebRequest httpWebRequest, string postData)
        {
            var requestWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            try
            {
                requestWriter.Write(postData);
            }
            finally
            {
                requestWriter.Close();
            }
            return GetHttpWebResponse(httpWebRequest);
        }

        private string AppendPostDataToUrl(string postData, string url)
        {
            if (url.IndexOf("?") > 0)
            {
                url += "&";
            }
            else
            {
                url += "?";
            }
            url += ParsePostData(postData);
            return url;
        }

        private string ParsePostData(string postData)
        {
            var appendedPostData = postData + "&source=" + AppKey;
            var queryString = HttpUtility.ParseQueryString(appendedPostData);
            var resultUrl = "";
            foreach (var key in queryString.AllKeys)
            {
                if (resultUrl.Length > 0)
                {
                    resultUrl += "&";
                }
                EncodeUrl(queryString, key);
                resultUrl += (key + "=" + queryString[key]);
            }
            return resultUrl;
        }

        private void EncodeUrl(NameValueCollection queryString, string key)
        {
            if (key != "status")
            {
                queryString[key] = HttpUtility.UrlPathEncode(queryString[key]);
                queryString[key] = UrlEncode(queryString[key]);
            }
        }

        public string RequestWithPicture(string url, string postData, string filepath)
        {
            var uploadApiUrl = url;
            var status = postData.Split('=').GetValue(1).ToString();
            var appendUrl = AppendPostDataToUrl(postData, url);
            var authorizationHeader = GetAuthorizationHeader(appendUrl, POST);

            var request = (HttpWebRequest)System.Net.WebRequest.Create(uploadApiUrl);
            request.Headers.Add("Authorization", authorizationHeader);

            request.PreAuthenticate = true;
            request.AllowWriteStreamBuffering = true;
            request.Method = POST;
            request.UserAgent = "Jakarta Commons-HttpClient/3.1";

            var bytes = GetContentsBytes(request, status, filepath);

            return GetHttpWebResponse(request, bytes);
        }

        public string RequestWithPicture(string url, string postData, byte[] file)
        {
            var uploadApiUrl = url;
            var status = postData.Split('=').GetValue(1).ToString();
            var appendUrl = AppendPostDataToUrl(postData, url);
            var authorizationHeader = GetAuthorizationHeader(appendUrl, POST);

            var request = (HttpWebRequest)System.Net.WebRequest.Create(uploadApiUrl);
            request.Headers.Add("Authorization", authorizationHeader);

            request.PreAuthenticate = true;
            request.AllowWriteStreamBuffering = true;
            request.Method = POST;
            request.UserAgent = "Jakarta Commons-HttpClient/3.1";

            var bytes = GetContentsBytes(request, status, file);

            return GetHttpWebResponse(request, bytes);
        }

        private static string GetHttpWebResponse(WebRequest httpWebRequest, byte[] bytes)
        {
            var requestStream = httpWebRequest.GetRequestStream();
            try
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }
            finally
            {
                requestStream.Close();
            }
            return GetHttpWebResponse(httpWebRequest);
        }

        private byte[] GetContentsBytes(WebRequest request, string status, string filepath)
        {
            string boundary = Guid.NewGuid().ToString();
            string header = string.Format("--{0}", boundary);
            string footer = string.Format("--{0}--", boundary);

            var contents = new StringBuilder();
            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            contents.AppendLine(header);
            contents.AppendLine(String.Format("Content-Disposition: form-data; name=\"{0}\"", "status"));
            contents.AppendLine("Content-Type: text/plain; charset=US-ASCII");
            contents.AppendLine("Content-Transfer-Encoding: 8bit");
            contents.AppendLine();
            contents.AppendLine(status);

            contents.AppendLine(header);
            contents.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", "source"));
            contents.AppendLine("Content-Type: text/plain; charset=US-ASCII");
            contents.AppendLine("Content-Transfer-Encoding: 8bit");
            contents.AppendLine();
            contents.AppendLine(AppKey);


            contents.AppendLine(header);
            string fileHeader = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", "pic", filepath);
            string fileData = Encoding.GetEncoding(ContentEncoding).GetString(File.ReadAllBytes(@filepath));

            contents.AppendLine(fileHeader);
            contents.AppendLine("Content-Type: application/octet-stream; charset=UTF-8");
            contents.AppendLine("Content-Transfer-Encoding: binary");
            contents.AppendLine();
            contents.AppendLine(fileData);
            contents.AppendLine(footer);

            byte[] bytes = Encoding.GetEncoding(ContentEncoding).GetBytes(contents.ToString());
            request.ContentLength = bytes.Length;
            return bytes;
        }

        private byte[] GetContentsBytes(WebRequest request, string status, byte[] file)
        {
            string boundary = Guid.NewGuid().ToString();
            string header = string.Format("--{0}", boundary);
            string footer = string.Format("--{0}--", boundary);

            var contents = new StringBuilder();
            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            contents.AppendLine(header);
            contents.AppendLine(String.Format("Content-Disposition: form-data; name=\"{0}\"", "status"));
            contents.AppendLine("Content-Type: text/plain; charset=US-ASCII");
            contents.AppendLine("Content-Transfer-Encoding: 8bit");
            contents.AppendLine();
            contents.AppendLine(status);

            contents.AppendLine(header);
            contents.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", "source"));
            contents.AppendLine("Content-Type: text/plain; charset=US-ASCII");
            contents.AppendLine("Content-Transfer-Encoding: 8bit");
            contents.AppendLine();
            contents.AppendLine(AppKey);


            contents.AppendLine(header);
            string fileHeader = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", "pic", file);
            string fileData = Encoding.GetEncoding(ContentEncoding).GetString(file);

            contents.AppendLine(fileHeader);
            contents.AppendLine("Content-Type: application/octet-stream; charset=UTF-8");
            contents.AppendLine("Content-Transfer-Encoding: binary");
            contents.AppendLine();
            contents.AppendLine(fileData);
            contents.AppendLine(footer);

            byte[] bytes = Encoding.GetEncoding(ContentEncoding).GetBytes(contents.ToString());
            request.ContentLength = bytes.Length;
            return bytes;
        }
    }
    #endregion

    #region HttpDelete
    public class HttpDelete : BaseHttpRequest
    {
        private const string DELETE = "DELETE";
        private const string ContentEncoding = "iso-8859-1";

        public override string Request(string uri, string postData)
        {
            var appendUrl = AppendPostDataToUrl(postData, uri);
            string outUrl;
            var querystring = AppendSignatureString(DELETE, appendUrl, out outUrl);
            return WebRequest(DELETE, outUrl, querystring);
        }

        private static string WebRequest(string method, string url, string postData)
        {
            var httpWebRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            httpWebRequest.Method = method;
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            return GetHttpWebResponse(httpWebRequest, postData);
        }

        private static string GetHttpWebResponse(WebRequest httpWebRequest, string postData)
        {
            var requestWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            try
            {
                requestWriter.Write(postData);
            }
            finally
            {
                requestWriter.Close();
            }
            return GetHttpWebResponse(httpWebRequest);
        }

        private string AppendPostDataToUrl(string postData, string url)
        {
            if (url.IndexOf("?") > 0)
            {
                url += "&";
            }
            else
            {
                url += "?";
            }
            url += ParsePostData(postData);
            return url;
        }

        private string ParsePostData(string postData)
        {
            var appendedPostData = postData + "&source=" + AppKey;
            var queryString = HttpUtility.ParseQueryString(appendedPostData);
            var resultUrl = "";
            foreach (var key in queryString.AllKeys)
            {
                if (resultUrl.Length > 0)
                {
                    resultUrl += "&";
                }
                EncodeUrl(queryString, key);
                resultUrl += (key + "=" + queryString[key]);
            }
            return resultUrl;
        }

        private void EncodeUrl(NameValueCollection queryString, string key)
        {
            queryString[key] = HttpUtility.UrlPathEncode(queryString[key]);
            queryString[key] = UrlEncode(queryString[key]);
        }

        private static string GetHttpWebResponse(WebRequest httpWebRequest, byte[] bytes)
        {
            var requestStream = httpWebRequest.GetRequestStream();
            try
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }
            finally
            {
                requestStream.Close();
            }
            return GetHttpWebResponse(httpWebRequest);
        }
    }
    #endregion
}
