using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using log4net;

namespace Open.QQ2SDK
{
    public class QQSerive : OAuthBase, ISerive
    {
        ILog logger = LogManager.GetLogger(typeof(QQSerive));
        #region 构造函数
        public QQSerive(string app_key, string app_secret, string redirect_uri)
            : base(app_key, app_secret, redirect_uri)
        { }

        public QQSerive()
            : base()
        { }
        #endregion

        #region 空间
        #region 获取OpenID
        public OpenInfo GetOpenID()
        {
            var val= HttpPost("oauth2.0/me", new Dictionary<object, object>());
            val = val.Replace("callback(", string.Empty).Replace(");",string.Empty).Trim();
            return val.ToEntity<OpenInfo>();
        }
        #endregion

        #region 获取用户信息
        /// <summary>
        /// 获取用户在QQ空间的个人资料
        /// </summary>
        public User Get_User_Info()
        {
            return HttpGet<User>("user/get_user_info", new Dictionary<object, object>());
        }
        #endregion

        #region 发表一篇日志到QQ空间
        /// <summary>
        /// 发表一篇日志到QQ空间
        /// </summary>
        /// <param name="title">日志标题（纯文本，最大长度128个字节，utf-8编码）。 </param>
        /// <param name="content">文章内容（html数据，最大长度100*1024个字节，utf-8编码）</param>
        /// <returns></returns>
        public string Add_One_Blog(string title,string content)
        {
            var dictionary = new Dictionary<object, object>
            {
                {"title",title.ToURLEncode()},
                {"content",content.ToURLEncode()}
            };
            return HttpPost("blog/add_one_blog", dictionary);
        }
        #endregion

        #region 同步分享到QQ空间、朋友网、腾讯微博
        /// <summary>
        /// 同步分享到QQ空间、朋友网、腾讯微博
        /// </summary>
        /// <param name="title">feeds的标题，最长36个中文字，超出部分会被截断。</param>
        /// <param name="url">分享所在网页资源的链接，点击后跳转至第三方网页，对应上文接口说明中2的超链接。请以http://开头。</param>
        /// <param name="comment">用户评论内容，也叫发表分享时的分享理由，禁止使用系统生产的语句进行代替。最长40个中文字，超出部分会被截断。</param>
        /// <param name="summary">所分享的网页资源的摘要内容，或者是网页的概要描述，最长80个中文字，超出部分会被截断。</param>
        /// <param name="images">所分享的网页资源的代表性图片链接"，请以http://开头，长度限制255字符。多张图片以竖线（|）分隔，目前只有第一张图片有效，图片规格100*100为佳。</param>
        /// <param name="source">分享的场景。取值说明：1.通过网页 2.通过手机 3.通过软件 4.通过IPHONE 5.通过 IPAD。</param>
        /// <param name="type">分享内容的类型。true表示网页；false表示视频（type=false时，必须传入playurl）。</param>
        /// <param name="playurl">长度限制为256字节。仅在type=false的时候有效。</param>
        /// <param name="nswb">值为1时，表示分享不默认同步到微博，其他值或者不传此参数表示默认同步到微博。</param>
        /// <returns></returns>
        public Share Add_Share(string title, string url, string comment, string summary, string images, SourceType? source, bool type, string playurl, string nswb)
        {
            var dictionary = new Dictionary<object, object>
            {
                {"title",title.ToURLEncode()},
                {"url",url.ToURLEncode()},
                {"comment",comment.ToURLEncode()},
                {"summary",summary.ToURLEncode()},
                {"images",images.ToURLEncode()},
                {"source",(int)(source??SourceType.Web)},
                {"type",type?4:5},
                {"playurl",playurl.ToURLEncode()},
                {"nswb",nswb.ToURLEncode()}
            };
            return HttpPost<Share>("share/add_share", dictionary);
        }
        #endregion
        #endregion

        #region 微博
        #region 发表一条微博信息到腾讯微博
        /// <summary>
        /// 发表一条微博信息到腾讯微博
        /// </summary>
        /// <param name="content">表示要发表的微博内容。最长为140个汉字，也就是420字节。如果微博内容中有URL，后台会自动将该URL转换为短URL，每个URL折算成11个字节。</param>
        /// <param name="clientip">用户ip。必须正确填写用户侧真实ip，不能为内网ip及以127或255开头的ip，以分析用户所在地。</param>
        /// <param name="jing">用户所在地理位置的经度。为实数，最多支持10位有效数字。有效范围：-180.0到+180.0，+表示东经，默认为0.0。</param>
        /// <param name="wei">用户所在地理位置的纬度。为实数，最多支持10位有效数字。有效范围：-90.0到+90.0，+表示北纬，默认为0.0。</param>
        /// <param name="syncflag">标识是否将发布的微博同步到QQ空间（true：同步； false：不同步；），默认为true。</param>
        /// <returns></returns>
        public string Add_T(string content, string clientip, string jing, string wei, bool? syncflag)
        {
            var dictionary = new Dictionary<object, object>
             {
                 {"content",content.ToURLEncode()},
                 {"clientip",clientip},
                 {"jing",jing},
                 {"wei",wei},
                 {"syncflag",syncflag??true?0:1}//（0：同步； 1：不同步；），默认为0。
             };
            return HttpPost("t/add_t ", dictionary);
        }
        #endregion

        #region 上传图片并发表消息到腾讯微博
        /// <summary>
        /// 上传图片并发表消息到腾讯微博
        /// </summary>
        /// <param name="content">表示要发表的微博内容。最长为140个汉字，也就是420字节。如果微博内容中有URL，后台会自动将该URL转换为短URL，每个URL折算成11个字节。</param>
        /// <param name="clientip">用户ip。必须正确填写用户侧真实ip，不能为内网ip及以127或255开头的ip，以分析用户所在地。</param>
        /// <param name="jing">用户所在地理位置的经度。为实数，最多支持10位有效数字。有效范围：-180.0到+180.0，+表示东经，默认为0.0。</param>
        /// <param name="wei">用户所在地理位置的纬度。为实数，最多支持10位有效数字。有效范围：-90.0到+90.0，+表示北纬，默认为0.0。</param>
        /// <param name="pic">要上传的图片的文件名以及图片的内容,图片仅支持JPEG、GIF、PNG格式（所有图片都会重新压缩，gif被重新压缩后不会再有有动画效果），图片size小于2M。</param>
        /// <param name="syncflag">标识是否将发布的微博同步到QQ空间（true：同步； false：不同步；），默认为true。</param>
        /// <returns></returns>
        public string Add_Pic_T(string content, string clientip, string jing, string wei, byte[] pic, bool? syncflag)
        {
            var dictionary = new Dictionary<object, object>
             {
                 {"content",content.ToURLEncode()},
                 {"clientip",clientip},
                 {"jing",jing},
                 {"wei",wei},
                 {"syncflag",syncflag??true?0:1}//（0：同步； 1：不同步；），默认为0。
             };
            return HttpPost("t/add_pic_t", dictionary, pic);
        }
        #endregion

        #region 获取登录用户自己的详细信息
        /// <summary>
        /// 获取登录用户自己的详细信息
        /// </summary>
        /// <param name="id">微博消息的ID，用来唯一标识一条微博消息。</param>
        /// <returns></returns>
        public string Del_T(string id)
        {
            var dictionary = new Dictionary<object, object>
            {
                {"id",id}
            };
            return HttpPost("user/get_info ", dictionary);
        }
        #endregion

        #region 获取登录用户自己的详细信息
        public string Get_Info()
        {
            return HttpPost("user/get_info ", new Dictionary<object, object>());
        }
        #endregion
        #endregion

        #region Must
        #region Post
        public T HttpPost<T>(string partUrl, IDictionary<object, object> dictionary) where T : class
        {
            return HttpPost<T>(partUrl, dictionary, null);
        }

        public T HttpPost<T>(string partUrl, IDictionary<object, object> dictionary, byte[] file) where T : class
        {
            dictionary.Add("access_token", base.Token.access_token);
            dictionary.Add("oauth_consumer_key", base.App_Key);
            dictionary.Add("openid", base.App_Secret);

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
            dictionary.Add("access_token", base.Token.access_token);
            dictionary.Add("oauth_consumer_key", base.App_Key);
            dictionary.Add("openid", base.App_Secret);

            var url = base.baseUrl.ToFormat(partUrl);
            var query = dictionary.ToQueryString();
            logger.Error(url);
            logger.Error(query);
            return base.HttpMethod.HttpPost(url, query);
        }

        public string HttpPost(string partUrl, IDictionary<object, object> dictionary, byte[] file)
        {
            dictionary.Add("access_token", base.Token.access_token);
            dictionary.Add("oauth_consumer_key", base.App_Key);
            dictionary.Add("openid", base.App_Secret);

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
            dictionary.Add("access_token", base.Token.access_token);
            dictionary.Add("oauth_consumer_key", base.App_Key);
            dictionary.Add("openid", base.App_Secret);

            var url = base.baseUrl.ToFormat(partUrl);
            var query = dictionary.ToQueryString();
            logger.Error(url + "?" + query);
            var json = base.HttpMethod.HttpGet(url + "?" + query);
            return json.ToEntity<T>("json");
        }

        public string HttpGet(string partUrl, IDictionary<object, object> dictionary)
        {
            dictionary.Add("access_token", base.Token.access_token);
            dictionary.Add("oauth_consumer_key", base.App_Key);
            dictionary.Add("openid", base.App_Secret);

            var url = base.baseUrl.ToFormat(partUrl);
            var query = dictionary.ToQueryString();
            logger.Error(url + "?" + query);
            return base.HttpMethod.HttpGet(url + "?" + query);
        }
        #endregion
        #endregion
    }
}
