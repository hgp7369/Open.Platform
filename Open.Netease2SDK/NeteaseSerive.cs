/*API文档更新时间: 2012-03-19*/
/*作者:http://weibo.com/u/1716169737*/
/*备注:地理信息 API未完成*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using log4net;

namespace Open.Netease2SDK
{
    public class NeteaseSerive:OAuthBase,ISerive
    {
        ILog logger = LogManager.GetLogger(typeof(NeteaseSerive));
        #region 构造函数
        public NeteaseSerive(string app_key, string app_secret, string redirect_uri)
            : base(app_key, app_secret, redirect_uri)
        { }

        public NeteaseSerive()
            : base()
        { }
        #endregion

        #region 用户
        #region 获取用户信息
        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <returns></returns>
        public string Users_Show()
        {
            return Users_Show(null);
        }
        
        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="uid">需要查询的用户ID。</param>
        /// <returns></returns>
        public string Users_Show(int? user_id)
        {
            var dictionary = new Dictionary<object, object> 
            {
                {"user_id",user_id}
            };
            return HttpGet("users/show.json", dictionary);
        }
        #endregion
        #endregion

        #region 微博
        #region 发布一条新微博
        public string Statuses_Update(string status)
        {
            var dictionary = new Dictionary<object, object> 
            {
                {"status",status}
            };
            return HttpPost("statuses/update.json", dictionary);
        }

        public string Statuses_Upload(string status, byte[] pic)
        {
            var imgUrl = HttpPost("statuses/upload.json", new Dictionary<object, object> {}, pic);
            //return Statuses_Update(status + imgUrl);
            return imgUrl;
        }
        #endregion
        #endregion

        #region Must
        #region Post
        public T HttpPost<T>(string partUrl, IDictionary<object, object> dictionary) where T : class
        {
            return HttpPost<T>(partUrl, dictionary, null);
        }

        public T HttpPost<T>(string partUrl, IDictionary<object, object> dictionary,byte[] file) where T : class
        {
            this.ToDictionary(dictionary);
            var url = base.baseUrl.ToFormat(partUrl);            
            var query = dictionary.ToQueryString();
            logger.Error(url);
            logger.Error(query);
            var json = string.Empty;
            if (file != null)
            {
                json = base.HttpMethod.HttpPost(url, dictionary, file);
            }
            else
            {
                json = base.HttpMethod.HttpPost(url, query);
            }
            
            return json.ToEntity<T>();
        }

        public string HttpPost(string partUrl, IDictionary<object, object> dictionary)
        {
            this.ToDictionary(dictionary);
            var url = base.baseUrl.ToFormat(partUrl);
            var query = dictionary.ToQueryString();
            logger.Error(url);
            logger.Error(query);
            return base.HttpMethod.HttpPost(url, query);
        }

        public string HttpPost(string partUrl, IDictionary<object, object> dictionary, byte[] file)
        {
            this.ToDictionary(dictionary);
            var url = base.baseUrl.ToFormat(partUrl);
            var query = dictionary.ToQueryString();
            logger.Error(url);
            logger.Error(query);
            var json = string.Empty;
            if (file != null)
            {
                json = base.HttpMethod.HttpPost(url, dictionary, file);
            }
            else
            {
                json = base.HttpMethod.HttpPost(url, query);
            }

            return json;
        }
        #endregion

        #region Get
        public T HttpGet<T>(string partUrl, IDictionary<object, object> dictionary) where T : class
        {
            this.ToDictionary(dictionary);
            var url = base.baseUrl.ToFormat(partUrl);
            var query = dictionary.ToQueryString();
            logger.Error(url + "?" + query);
            var json = base.HttpMethod.HttpGet(url + "?" + query);
            return json.ToEntity<T>("json");
        }

        public string HttpGet(string partUrl, IDictionary<object, object> dictionary)
        {
            this.ToDictionary(dictionary);
            var url = base.baseUrl.ToFormat(partUrl);
            var query = dictionary.ToQueryString();
            logger.Error(url + "?" + query);
            return base.HttpMethod.HttpGet(url + "?" + query);
        }
        #endregion
        #endregion
    }
}
