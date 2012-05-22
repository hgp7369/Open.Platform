using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using log4net;

namespace Open.SinaSDK
{
    public class SinaSerive: IService
    {
        ILog logger = LogManager.GetLogger(typeof(SinaSerive));
        #region 属性/构造函数
        const string SINA_URL = "http://api.t.sina.com.cn/";
        private BaseHttpRequest BaseRequest;

        public SinaSerive()
            : this(Method.GET)
        {
        }

        public SinaSerive(Method method)
        {
            BaseRequest = HttpRequestFactory.CreateHttpRequest(method);
        }

        private string _Token;
        public string Token
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    _Token = string.Empty;
                    if (HttpContext.Current.Session["oauth_token"] != null)
                    {
                        _Token = HttpContext.Current.Session["oauth_token"].ToString();
                    }
                    return _Token;
                }
                else
                {
                    return _Token;
                }
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Session["oauth_token"] = value;
                }
                else
                {
                    _Token = value;
                }
            }
        }

        private string _TokenSecret;
        public string TokenSecret
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    _TokenSecret = string.Empty;
                    if (HttpContext.Current.Session["oauth_token_secret"] != null)
                    {
                        _TokenSecret = HttpContext.Current.Session["oauth_token_secret"].ToString();
                    }
                    return _TokenSecret;
                }
                else
                {
                    return _TokenSecret;
                }
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Session["oauth_token_secret"] = value;
                }
                else
                {
                    _TokenSecret = value;
                }
            }
        }
        #endregion

        #region 获取未授权签名
        public void GetoAuth(string callbackurl)
        {
            var httpRequest = BaseRequest as HttpGet; //HttpRequestFactory.CreateHttpRequest(Method.GET) as HttpGet;
            httpRequest.GetRequestToken();            
            string url = httpRequest.GetAuthorizationUrl();
            this.Token = httpRequest.Token;
            this.TokenSecret = httpRequest.TokenSecret;
            System.Web.HttpContext.Current.Response.Redirect(url + "&oauth_callback=" + callbackurl);
        }
        #endregion

        #region 获取授权签名
        public void GetAccessToken(string oauth_verifier)
        {
            var httpRequest = BaseRequest as HttpGet; //HttpRequestFactory.CreateHttpRequest(Method.GET) as HttpGet;
            httpRequest.Token = this.Token;
            httpRequest.TokenSecret = this.TokenSecret;
            httpRequest.Verifier = oauth_verifier;
            httpRequest.GetAccessToken();
            this.Token = httpRequest.Token;
            this.TokenSecret = httpRequest.TokenSecret;            
        }
        #endregion

        #region 数据接口
        #region 获取下行数据集(timeline)接口
        #region 获取最新的公共微博消息
        /// <summary>
        /// 获取最新的公共微博消息，返回结果非完全实时，最长会缓存60秒
        /// </summary>
        /// <param name="count">每次返回的记录数,缺省值20，最大值200</param>
        /// <returns></returns>
        public IList<Status> StatusesPublicTimeline(object count)
        {
            return StatusesPublicTimeline(Format.json, count, null);
        }

        /// <summary>
        /// 获取最新的公共微博消息
        /// 返回最新的20条公共微博。返回结果非完全实时，最长会缓存60秒
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="count">每次返回的记录数,缺省值20，最大值200</param>
        /// <param name="base_app">是否仅获取当前应用发布的信息。0为所有，1为仅本应用。</param>
        /// <returns></returns>
        public IList<Status> StatusesPublicTimeline(Format format, object count, object base_app)
        {
            var url = SINA_URL + "statuses/public_timeline." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"count",count},
                {"base_app",base_app}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Status>>(format);
        }
        #endregion

        #region 获取当前登录用户及其所关注用户的最新微博消息 (别名: statuses/home_timeline)
        /// <summary>
        /// 获取当前登录用户及其所关注用户的最新微博消息。
        /// 和用户登录 http://t.sina.com.cn 后在“我的首页”中看到的内容相同。
        /// </summary>
        /// <param name="count">指定要返回的记录条数。默认值20，最大值200。</param>
        /// <param name="page">指定返回结果的页码。根据当前登录用户所关注的用户数及这些被关注用户发表的微博数，翻页功能最多能查看的总记录数会有所不同，通常最多能查看1000条左右。默认值1</param>
        /// <param name="feature">微博类型，返回指定类型的微博信息内容。</param>
        /// <returns></returns>
        public IList<Status> StatusesFriendsTimeline(object count, object page, Feature feature)
        {
            return StatusesFriendsTimeline(Format.json, null, null, count, page, null, feature);
        }

        /// <summary>
        /// 获取当前登录用户及其所关注用户的最新微博消息。
        /// 和用户登录 http://t.sina.com.cn 后在“我的首页”中看到的内容相同。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="since_id">若指定此参数，则只返回ID比since_id大的微博消息（即比since_id发表时间晚的微博消息）。</param>
        /// <param name="max_id">若指定此参数，则返回ID小于或等于max_id的微博消息</param>
        /// <param name="count">指定要返回的记录条数。默认值20，最大值200。</param>
        /// <param name="page">指定返回结果的页码。根据当前登录用户所关注的用户数及这些被关注用户发表的微博数，翻页功能最多能查看的总记录数会有所不同，通常最多能查看1000条左右。默认值1</param>
        /// <param name="base_app">是否基于当前应用来获取数据。1为限制本应用微博，0为不做限制。</param>
        /// <param name="feature">微博类型，0全部，1原创，2图片，3视频，4音乐. 返回指定类型的微博信息内容。</param>
        /// <returns></returns>
        public IList<Status> StatusesFriendsTimeline(Format format, object since_id, object max_id, object count, object page, object base_app, Feature feature)
        {
            var url = SINA_URL + "statuses/public_timeline." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"since_id",since_id},
                {"max_id",max_id},
                {"count",count},
                {"page",page},
                {"base_app",base_app},
                {"feature",(int)feature}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Status>>(format);
        }
        #endregion

        #region 获取用户发布的微博消息列表
        /// <summary>
        /// 返回用户最新发表的微博消息列表。
        /// 默认返回最近15天以内的微博信息
        /// 由于分页限制，暂时最多只能返回用户最新的200条微博信息 
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <param name="page">页码。注意：最多返回200条分页内容。默认值1</param>
        /// <param name="count">指定每页返回的记录条数。默认值20，最大值200</param>
        /// <param name="feature">微博类型，返回指定类型的微博信息内容。</param>
        /// <returns></returns>
        private IList<Status> StatusesUserTimeLine(object user_id, object page, object count, Feature feature)
        {
            return StatusesUserTimeLine(Format.json, user_id, null, null, null, page, count, null, feature);
        }

        /// <summary>
        /// 返回用户最新发表的微博消息列表。
        /// 默认返回最近15天以内的微博信息
        /// 由于分页限制，暂时最多只能返回用户最新的200条微博信息
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="user_id">用户ID，主要是用来区分用户ID跟微博昵称。当微博昵称为数字导致和用户ID产生歧义，特别是当微博昵称和用户ID一样的时候，建议使用该参数</param>
        /// <param name="screen_name">微博昵称，主要是用来区分用户UID跟微博昵称，当二者一样而产生歧义的时候，建议使用该参数</param>
        /// <param name="since_id">若指定此参数，则只返回ID比since_id大（即比since_id发表时间晚的）的微博消息。</param>
        /// <param name="max_id">若指定此参数，则返回ID小于或等于max_id的微博消息</param>
        /// <param name="page">页码。注意：最多返回200条分页内容。默认值1。</param>
        /// <param name="count">指定每页返回的记录条数。默认值20，最大值200。</param>
        /// <param name="base_app">是否基于当前应用来获取数据。1为限制本应用微博，0为不做限制。</param>
        /// <param name="feature">微博类型，0全部，1原创，2图片，3视频，4音乐. 返回指定类型的微博信息内容。</param>
        /// <returns></returns>
        private IList<Status> StatusesUserTimeLine(Format format, object user_id, object screen_name, object since_id, object max_id, object page, object count, object base_app, Feature feature)
        {
            var url = SINA_URL + "statuses/user_timeline." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"user_id",user_id},
                {"screen_name",screen_name},
                {"since_id",since_id},
                {"max_id",max_id},
                {"page",page},
                {"count",count},
                {"base_app",base_app},
                {"feature",(int)feature}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Status>>(format);
        }        
        #endregion

        #region 获取@当前用户的微博列表
        /// <summary>
        /// 返回最新n条提到登录用户的微博消息（即包含@username的微博消息）
        /// </summary>
        /// <param name="count">单页返回的记录条数。默认值20，最大值200。</param>
        /// <param name="page">返回结果的页码。注意：有分页限制。默认值1。</param>
        /// <returns></returns>
        public IList<Status> StatusesMentions(object count, object page)
        {
            return StatusesMentions(Format.json, null, null, count, page);
        }

        /// <summary>
        /// 返回最新n条提到登录用户的微博消息（即包含@username的微博消息）
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="since_id">若指定此参数，则只返回ID比since_id大的提到当前登录用户的微博消息（比since_id发表时间晚）。</param>
        /// <param name="max_id">若指定此参数，则返回ID小于或等于max_id的提到当前登录用户微博消息</param>        
        /// <param name="count">单页返回的记录条数。默认值20，最大值200。</param>
        /// <param name="page">返回结果的页码。注意：有分页限制。默认值1。</param>
        /// <returns></returns>
        public IList<Status> StatusesMentions(Format format, object since_id, object max_id, object count, object page)
        {
            var url = SINA_URL + "statuses/mentions." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"since_id",since_id},
                {"max_id",max_id},                
                {"count",count},
                {"page",page}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Status>>(format);
        }
        #endregion

        #region 获取当前用户发送及收到的评论列表
        /// <summary>
        /// 返回最新n条发送及收到的评论。
        /// </summary>
        /// <param name="count">单页返回的记录条数。默认值20，最大值200。</param>
        /// <param name="page">返回结果的页码。注意：有分页限制。默认值1。</param>
        /// <returns></returns>
        public IList<Comment> StatusesCommentsTimeline(object count, object page)
        {
            return StatusesCommentsTimeline(Format.json, null, null, count, page);
        }

        /// <summary>
        /// 返回最新n条发送及收到的评论。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="since_id">若指定此参数，则只返回ID比since_id大的评论（比since_id发表时间晚）。</param>
        /// <param name="max_id">若指定此参数，则返回ID小于或等于max_id的评论</param>
        /// <param name="count">单页返回的记录条数。默认值20，最大值200。</param>
        /// <param name="page">返回结果的页码。注意：有分页限制。默认值1。</param>
        /// <returns></returns>      
        public IList<Comment> StatusesCommentsTimeline(Format format, object since_id, object max_id, object count, object page)
        {
            var url = SINA_URL + "statuses/comments_timeline." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"since_id",since_id},
                {"max_id",max_id},                
                {"count",count},
                {"page",page}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Comment>>(format);
        }
        #endregion

        #region 获取当前用户发出的评论
        /// <summary>
        /// 获取当前用户发出的评论
        /// </summary>
        /// <param name="count">单页返回的记录条数。默认值20，最大值200。</param>
        /// <param name="page">返回结果的页码。默认值1。</param>
        /// <returns></returns>
        public IList<Comment> StatusesCommentsByMe(object count, object page)
        {
            return StatusesCommentsByMe(Format.json, null, null, count, page);
        }

        /// <summary>
        /// 获取当前用户发出的评论
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="since_id">若指定此参数，则只返回ID比since_id大的评论（比since_id发表时间晚）。</param>
        /// <param name="max_id">若指定此参数，则返回ID小于或等于max_id的评论</param>
        /// <param name="count">单页返回的记录条数。默认值20，最大值200。</param>
        /// <param name="page">返回结果的页码。默认值1。</param>        
        /// <returns></returns>
        public IList<Comment> StatusesCommentsByMe(Format format, object since_id, object max_id, object count, object page)
        {
            var url = SINA_URL + "statuses/comments_by_me." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"since_id",since_id},
                {"max_id",max_id},
                {"page",page},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Comment>>(format);
        }
        #endregion

        #region 获取当前用户收到的评论
        /// <summary>
        /// 返回当前登录用户收到的评论
        /// </summary>
        /// <param name="count">单页返回的记录条数。默认值20，最大值200。</param>
        /// <param name="page">返回结果的页码。默认值1。</param>
        /// <returns></returns>
        public IList<Comment> StatusesCommentsToMe(object count, object page)
        {
            return StatusesCommentsToMe(Format.json, null, null, count, page);
        }

        /// <summary>
        /// 返回当前登录用户收到的评论
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="since_id">若指定此参数，则只返回ID比since_id大的评论（比since_id发表时间晚）。</param>
        /// <param name="max_id">若指定此参数，则返回ID小于或等于max_id的评论</param>
        /// <param name="count">单页返回的记录条数。默认值20，最大值200。</param>
        /// <param name="page">返回结果的页码。默认值1。</param>    
        /// <returns></returns>
        public IList<Comment> StatusesCommentsToMe(Format format, object since_id, object max_id, object count, object page)
        {
            var url = SINA_URL + "statuses/comments_to_me." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"since_id",since_id},
                {"max_id",max_id},
                {"page",page},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Comment>>(format);
        }
        #endregion

        #region 根据微博消息ID返回某条微博消息的评论列表
        /// <summary>
        /// 根据微博消息ID返回某条微博消息的评论列表
        /// </summary>
        /// <param name="id">指定要获取的评论列表所属的微博消息ID</param>
        /// <param name="count">单页返回的记录条数。默认值20，最大值200。</param>
        /// <param name="page">返回结果的页码。默认值1。</param>
        /// <returns></returns>
        public IList<Comment> StatusesComments(object id, object count, object page)
        {
            return StatusesComments(Format.json, id, count, page);
        }

        /// <summary>
        /// 根据微博消息ID返回某条微博消息的评论列表
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="id">指定要获取的评论列表所属的微博消息ID</param>
        /// <param name="count">单页返回的记录条数。默认值20，最大值200。</param>
        /// <param name="page">返回结果的页码。默认值1。</param>
        /// <returns></returns>
        public IList<Comment> StatusesComments(Format format, object id, object count, object page)
        {
            var url = SINA_URL + "statuses/comments." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"id",id},
                {"page",page},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Comment>>(format);
        }
        #endregion

        #region 批量获取一组微博的评论数及转发数
        /// <summary>
        /// 批量获取n条微博消息的评论数和转发数。一次请求最多可以获取20条微博消息的评论数和转发数
        /// </summary>
        /// <param name="ids">要获取评论数和转发数的微博消息ID列表，用逗号隔开</param>
        /// <returns></returns>
        public string StatusesCounts(string ids)
        {
            return StatusesCounts(Format.json, ids);
        }

        /// <summary>
        /// 批量获取n条微博消息的评论数和转发数。一次请求最多可以获取20条微博消息的评论数和转发数
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="ids">要获取评论数和转发数的微博消息ID列表，用逗号隔开</param>
        /// <returns></returns>
        public string StatusesCounts(Format format, string ids)
        {
            var url = SINA_URL + "statuses/counts." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"ids",Uri.EscapeDataString(ids)}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion

        #region 返回一条原创微博的最新n条转发微博信息
        /// <summary>
        /// 返回一条原创微博消息的最新n条转发微博消息。本接口无法对非原创微博进行查询。
        /// </summary>
        /// <param name="id">要获取转发微博列表的原创微博ID。</param>
        /// <param name="count">单页返回的记录条数。默认值20，最大值200。</param>
        /// <param name="page">返回结果的页码。默认值1。</param>
        /// <returns></returns>
        public IList<Status> StatusesRepostTimeline(object id, object count, object page)
        {
            return StatusesRepostTimeline(Format.json, id, null, null, count, page);
        }

        /// <summary>
        /// 返回一条原创微博消息的最新n条转发微博消息。本接口无法对非原创微博进行查询。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="id">要获取转发微博列表的原创微博ID。</param>
        /// <param name="since_id">若指定此参数，则只返回ID比since_id大的记录（比since_id发表时间晚）。</param>
        /// <param name="max_id">若指定此参数，则返回ID小于或等于max_id的记录</param>
        /// <param name="count">单页返回的记录条数。默认值20，最大值200。</param>
        /// <param name="page">返回结果的页码。默认值1。</param>
        /// <returns></returns>
        public IList<Status> StatusesRepostTimeline(Format format, object id, object since_id, object max_id, object count, object page)
        {
            var url = SINA_URL + "statuses/repost_timeline." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"id",id},
                {"since_id",since_id},
                {"max_id",max_id},
                {"page",page},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Status>>(format);
        }
        #endregion

        #region 返回用户转发的最新n条微博信息
        /// <summary>
        /// 获取用户最新转发的n条微博消息
        /// </summary>
        /// <param name="id">要获取转发微博列表的用户ID。</param>
        /// <param name="count">单页返回的记录条数。默认值20，最大值200。</param>
        /// <param name="page">返回结果的页码。默认值1。</param>
        /// <returns></returns>
        public IList<Status> StatusesRepostByMe(object id, object count, object page)
        {
            return StatusesRepostByMe(Format.json, id, null, null, count, page);
        }

        /// <summary>
        /// 获取用户最新转发的n条微博消息
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="id">要获取转发微博列表的用户ID。</param>
        /// <param name="since_id">若指定此参数，则只返回ID比since_id大的记录（比since_id发表时间晚）。</param>
        /// <param name="max_id">若指定此参数，则返回ID小于或等于max_id的记录</param>
        /// <param name="count">单页返回的记录条数。默认值20，最大值200。</param>
        /// <param name="page">返回结果的页码。默认值1。</param>
        /// <returns></returns>
        public IList<Status> StatusesRepostByMe(Format format, object id, object since_id, object max_id, object count, object page)
        {
            var url = SINA_URL + "statuses/repost_by_me." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"id",id},
                {"since_id",since_id},
                {"max_id",max_id},
                {"page",page},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Status>>(format);
        }
        #endregion 
        
        #region 获取当前用户未读消息数
        /// <summary>
        /// 获取当前用户Web主站未读消息数，包括：是否有新微博消息,最新提到“我”的微博消息数,新评论数,新私信数,新粉丝数。此接口对应的清零接口为statuses/reset_count。
        /// </summary>
        /// <param name="with_new_status">1表示结果中包含new_status字段，0表示结果不包含new_status字段。new_status字段表示是否有新微博消息，1表示有，0表示没有</param>        
        /// <returns></returns>
        public string StatusesUnread(int with_new_status)
        {
            return StatusesUnread(Format.json, with_new_status, null);
        }

        /// <summary>
        /// 获取当前用户Web主站未读消息数，包括：是否有新微博消息,最新提到“我”的微博消息数,新评论数,新私信数,新粉丝数。此接口对应的清零接口为statuses/reset_count。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="with_new_status">1表示结果中包含new_status字段，0表示结果不包含new_status字段。new_status字段表示是否有新微博消息，1表示有，0表示没有</param>
        /// <param name="since_id">参数值为微博id。该参数需配合with_new_status参数使用，返回since_id之后，是否有新微博消息产生</param>
        /// <returns></returns>
        public string StatusesUnread(Format format, int with_new_status, object since_id)
        {
            var url = SINA_URL + "statuses/unread." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"with_new_status",with_new_status},
                {"since_id",since_id}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion

        #region 未读消息数清零接口(Post)
        /// <summary>
        /// 将当前登录用户的某种新消息的未读数为0。可以清零的计数类别有：1. 评论数，2. @me数，3. 私信数，4. 关注数
        /// </summary>
        /// <param name="type">需要清零的计数类别，值为下列四个之一：1. 评论数，2. @me数，3. 私信数，4. 关注数</param>
        /// <returns></returns>
        public string StatusesResetCount(int type)
        {
            return StatusesResetCount(Format.json, type);
        }

        /// <summary>
        /// 将当前登录用户的某种新消息的未读数为0。可以清零的计数类别有：1. 评论数，2. @me数，3. 私信数，4. 关注数
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="type">需要清零的计数类别，值为下列四个之一：1. 评论数，2. @me数，3. 私信数，4. 关注数</param>
        /// <returns></returns>
        public string StatusesResetCount(Format format, int type)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "statuses/reset_count." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"type",type}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST);
        }
        #endregion

        #region 表情接口，获取表情列表(无需登录)
        /// <summary>
        /// 表情接口，获取表情列表(无需登录)
        /// </summary>
        /// <param name="type">表情类别。"face":普通表情，"ani"：魔法表情，"cartoon"：动漫表情</param>
        /// <param name="language">语言类别，"cnname"简体，"twname"繁体</param>
        /// <returns></returns>
        public string StatusesEmotions(EmotionsType type, EmotionsLanguage language)
        {
            return StatusesEmotions(Format.json, type, language);
        }

        /// <summary>
        /// 表情接口，获取表情列表(无需登录)
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="type">表情类别。"face":普通表情，"ani"：魔法表情，"cartoon"：动漫表情</param>
        /// <param name="language">语言类别，"cnname"简体，"twname"繁体</param>
        /// <returns></returns>
        public string StatusesEmotions(Format format, EmotionsType type, EmotionsLanguage language)
        {
            var url = SINA_URL + "emotions." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"type",type},
                {"language",language},
                {"source",BaseRequest.AppKey}
            };
            //return Request(url, dictionary.ToQueryString());
            //url += "?source=" + BaseRequest.AppKey + "&" + dictionary.ToQueryString();
            //return HGP.Tools.Globals.CommonHelper.GetNetFile(url, Encoding.UTF8);
            return Request(url, dictionary.ToQueryString());
        }
        #endregion
        #endregion

        #region 微博访问接口
        #region 根据ID获取单条微博信息内容
        /// <summary>
        /// 根据ID获取单条微博消息，以及该微博消息的作者信息。
        /// </summary>
        /// <param name="id">要获取的微博消息ID，该参数为REST风格的参数</param>
        /// <returns></returns>
        public Status StatuesShow(object id)
        {
            return StatuesShow(Format.json, id);
        }

        /// <summary>
        /// 根据ID获取单条微博消息，以及该微博消息的作者信息。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="id">要获取的微博消息ID，该参数为REST风格的参数</param>
        /// <returns></returns>
        public Status StatuesShow(Format format,object id)
        {
            var url = SINA_URL + "statuses/show/" + id + "." + format;
            return Request(url).ToEntity<Status>(format);
        }
        #endregion

        #region 根据微博ID和用户ID跳转到单条微博页面(无需登录)
        /// <summary>
        /// 跳转到单条微博的Web地址。可以通过此url跳转到微博对应的Web网页。
        /// </summary>
        /// <param name="userid">微博消息的发布者ID</param>
        /// <param name="id">微博消息的ID</param>
        /// <returns></returns>
        public string StatuesUrl(object userid,object id)
        {
            var url = SINA_URL + userid + "/statuses/" + id + "?source=" + BaseRequest.AppKey;
            //return HGP.Tools.Globals.CommonHelper.GetNetFile(url, Encoding.UTF8);
            return Request(url);
        }
        #endregion

        #region 发布一条微博信息(Post)
        /// <summary>
        /// 发布一条微博信息。也可以同时转发某条微博。请求必须用POST方式提交。
        /// </summary>
        /// <param name="Status">要发布的微博消息文本内容</param>
        /// <returns></returns>
        public Status StatuesUpdate(object status)
        {
            return StatuesUpdate(Format.json, status);
        }

        /// <summary>
        /// 发布一条微博信息。也可以同时转发某条微博。请求必须用POST方式提交。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="Status">要发布的微博消息文本内容</param>
        /// <returns></returns>
        public Status StatuesUpdate(Format format, object status)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "statuses/update." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"status",Uri.EscapeDataString(status.ToString())}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST).ToEntity<Status>(format);
        }
        #endregion

        #region 上传图片并发布一条微博信息
        /// <summary>
        /// 发表带图片的微博。必须用POST方式提交pic参数，且Content-Type必须设置为multipart/form-data。图片大小<5M。
        /// </summary>
        /// <param name="Status">要发布的微博文本内容。</param>
        /// <param name="filePath">要上传的图片数据。仅支持JPEG、GIF、PNG格式，为空返回400错误。图片大小<5M。</param>
        /// <returns></returns>
        public Status StatuesUpload(object status, string filePath)
        {
            return StatuesUpload(Format.json, status, filePath);
        }

        /// <summary>
        /// 发表带图片的微博。必须用POST方式提交pic参数，且Content-Type必须设置为multipart/form-data。图片大小<5M。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="Status">要发布的微博文本内容。</param>
        /// <param name="filePath">要上传的图片数据。仅支持JPEG、GIF、PNG格式，为空返回400错误。图片大小<5M。</param>
        /// <returns></returns>
        public Status StatuesUpload(Format format, object status, string filePath)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "statuses/upload." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"status",Uri.EscapeDataString(status.ToString())}
            };
            return Request(url, dictionary.ToQueryString(), filePath.ToString()).ToEntity<Status>(format);
        }

        /// <summary>
        /// 发表带图片的微博。必须用POST方式提交pic参数，且Content-Type必须设置为multipart/form-data。图片大小<5M。
        /// </summary>
        /// <param name="Status">要发布的微博文本内容。</param>
        /// <param name="file">要上传的图片数据。仅支持JPEG、GIF、PNG格式，为空返回400错误。图片大小<5M。</param>
        /// <returns></returns>
        public string StatuesUpload(object status, byte[] file)
        {
            return StatuesUpload(Format.json, status, file);
        }

        /// <summary>
        /// 发表带图片的微博。必须用POST方式提交pic参数，且Content-Type必须设置为multipart/form-data。图片大小<5M。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="Status">要发布的微博文本内容。</param>
        /// <param name="file">要上传的图片数据。仅支持JPEG、GIF、PNG格式，为空返回400错误。图片大小<5M。</param>
        /// <returns></returns>
        public string StatuesUpload(Format format, object status, byte[] file)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "statuses/upload." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"Status",Uri.EscapeDataString(status.ToString())}
            };
            return Request(url, dictionary.ToQueryString(), file);
        }
        #endregion

        #region 删除一条微博信息
        /// <summary>
        /// 根据ID删除微博消息。注意：只能删除自己发布的微博消息。
        /// </summary>
        /// <param name="id">要删除的微博消息ID</param>
        /// <returns></returns>
        public Status StatuesDestroy(object id)
        {
            return StatuesDestroy(Format.json, id);
        }

        /// <summary>
        /// 根据ID删除微博消息。注意：只能删除自己发布的微博消息。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="id">要删除的微博消息ID</param>
        /// <returns></returns>
        public Status StatuesDestroy(Format format, object id)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "statuses/destroy/" + id + "." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"source",BaseRequest.AppKey}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST).ToEntity<Status>(format);
        }
        #endregion

        #region 转发一条微博信息
        /// <summary>
        /// 转发一条微博信息
        /// </summary>
        /// <param name="id">要转发的微博ID</param>
        /// <param name="Status">添加的转发文本。信息内容不超过140个汉字。如不填则默认为“转发微博”。</param>
        /// <param name="is_comment">是否在转发的同时发表评论。0表示不发表评论，1表示发表评论给当前微博，2表示发表评论给原微博，3是1、2都发表。默认为0。</param>
        /// <returns></returns>
        public Status StatuesRepost(object id, object status, CommentType is_comment)
        {
            return StatuesRepost(Format.json, id, status, is_comment);
        }

        /// <summary>
        /// 转发一条微博信息
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="id">要转发的微博ID</param>
        /// <param name="Status">添加的转发文本。信息内容不超过140个汉字。如不填则默认为“转发微博”。</param>
        /// <param name="is_comment">是否在转发的同时发表评论。0表示不发表评论，1表示发表评论给当前微博，2表示发表评论给原微博，3是1、2都发表。默认为0。</param>
        /// <returns></returns>
        public Status StatuesRepost(Format format, object id, object status, CommentType is_comment)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "statuses/repost." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"id",id},
                {"status",Uri.EscapeDataString(status.ToString())},
                {"is_comment",Convert.ToInt32(is_comment)}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST).ToEntity<Status>(format);
        }
        #endregion

        #region 对一条微博信息进行评论
        /// <summary>
        /// 对一条微博信息进行评论。
        /// </summary>
        /// <param name="id">要评论的微博消息ID</param>
        /// <param name="Comment">评论内容。必须做URLEncode,信息内容不超过140个汉字。</param>
        /// <param name="cid">要回复的评论ID。</param>
        /// <param name="without_mention">1：回复中不自动加入“回复@用户名”，0：回复中自动加入“回复@用户名”.默认为0.</param>
        /// <param name="comment_ori">当评论一条转发微博时，是否评论给原微博。0:不评论给原微博。1：评论给原微博。默认0.</param>
        /// <returns></returns>
        public Comment StatuesComment(object id, object comment, object cid, bool without_mention, bool comment_ori)
        {
            return StatuesComment(Format.json, id, comment, cid, without_mention, comment_ori);
        }

        /// <summary>
        /// 对一条微博信息进行评论。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="id">要评论的微博消息ID</param>
        /// <param name="Comment">评论内容。必须做URLEncode,信息内容不超过140个汉字。</param>
        /// <param name="cid">要回复的评论ID。</param>
        /// <param name="without_mention">1：回复中不自动加入“回复@用户名”，0：回复中自动加入“回复@用户名”.默认为0.</param>
        /// <param name="comment_ori">当评论一条转发微博时，是否评论给原微博。0:不评论给原微博。1：评论给原微博。默认0.</param>
        /// <returns></returns>
        public Comment StatuesComment(Format format, object id, object comment, object cid, bool without_mention, bool comment_ori)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "statuses/Comment." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"id",id},
                {"comment",Uri.EscapeDataString(comment.ToString())},
                {"cid",cid},
                {"without_mention",Convert.ToByte(without_mention)},
                {"comment_ori",Convert.ToByte(comment_ori)}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST).ToEntity<Comment>(format);
        }
        #endregion

        #region 删除当前用户的微博评论信息
        /// <summary>
        /// 删除评论。注意：只能删除登录用户自己发布的评论，不可以删除其他人的评论。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="id">欲删除的评论ID，该参数为一个REST风格参数。</param>
        /// <returns></returns>
        public string StatuesCommentDestroy(Format format,object id)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "statuses/comment_destroy/" + id + "." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"source",BaseRequest.AppKey}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST);
        }
        #endregion

        #region 批量删除当前用户的微博评论信息
        /// <summary>
        /// 批量删除评论。注意：只能删除登录用户自己发布的评论，不可以删除其他人的评论。
        /// </summary>
        /// <param name="ids">欲删除的一组评论ID，用半角逗号隔开，最多20个</param>
        /// <returns></returns>
        public IList<Comment> StatuesDestroyBatch(object ids)
        {
            return StatuesDestroyBatch(Format.json, ids);
        }

        /// <summary>
        /// 批量删除评论。注意：只能删除登录用户自己发布的评论，不可以删除其他人的评论。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="ids">欲删除的一组评论ID，用半角逗号隔开，最多20个</param>
        /// <returns></returns>
        public IList<Comment> StatuesDestroyBatch(Format format,object ids)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "statuses/Comment/destroy_batch." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"ids",ids}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST).ToEntity<IList<Comment>>(format);
        }
        #endregion

        #region 回复微博评论信息
        /// <summary>
        /// 回复评论
        /// </summary>
        /// <param name="cid">要回复的评论ID。</param>
        /// <param name="Comment">要回复的评论内容。必须做URLEncode,信息内容不超过140个汉字。</param>
        /// <param name="id">要评论的微博消息ID</param>
        /// <param name="without_mention">1：回复中不自动加入“回复@用户名”，0：回复中自动加入“回复@用户名”.默认为0.</param>
        /// <returns></returns>
        public Comment StatuesReply(object cid, object comment, object id, bool without_mention)
        {
            return StatuesReply(Format.json, cid, comment, id, without_mention);
        }

        /// <summary>
        /// 回复评论
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="cid">要回复的评论ID。</param>
        /// <param name="Comment">要回复的评论内容。必须做URLEncode,信息内容不超过140个汉字。</param>
        /// <param name="id">要评论的微博消息ID</param>
        /// <param name="without_mention">1：回复中不自动加入“回复@用户名”，0：回复中自动加入“回复@用户名”.默认为0.</param>
        /// <returns></returns>
        public Comment StatuesReply(Format format,object cid,object comment,object id,bool without_mention)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "statuses/reply." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"cid",cid},
                {"comment",Uri.EscapeDataString(comment.ToString())},
                {"id",id},
                {"without_mention",Convert.ToByte(without_mention)}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST).ToEntity<Comment>(format);
        }
        #endregion
        #endregion

        #region 用户接口
        #region 获取当前登录用户
        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        /// <returns></returns>
        public User UserShow()
        {
            return UserShow(Format.json);
        }

        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <returns></returns>
        public User UserShow(Format format)
        {
            //logger.Error("用户ID：" + BaseRequest.userid);
            return UserShow(format, BaseRequest.userid);
        }
        #endregion

        #region 根据用户ID获取用户资料（授权用户）
         /// <summary>
        /// 按用户ID或昵称返回用户资料以及用户的最新发布的一条微博消息。
        /// </summary>
        /// <param name="id_name">用户ID或昵称</param>
        /// <returns></returns>
        public User UserShow(object id_name)
        {
            return UserShow(Format.json, id_name);
        }

        /// <summary>
        /// 按用户ID或昵称返回用户资料以及用户的最新发布的一条微博消息。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="id_name">用户ID或昵称</param>
        /// <returns></returns>
        public User UserShow(Format format, object id_name)
        {
            var url = SINA_URL + "users/show/" + id_name + "." + format;            
            return Request(url).ToEntity<User>(format);
        }

        /// <summary>
        /// 按用户昵称返回用户资料以及用户的最新发布的一条微博消息。
        /// </summary>
        /// <param name="screen_name">用户昵称</param>
        /// <returns></returns>
        public User UserShowByScreenName(string screen_name)
        {
            return UserShowByScreenName(Format.json, screen_name);
        }

        /// <summary>
        /// 按用户昵称返回用户资料以及用户的最新发布的一条微博消息。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="screen_name">用户昵称</param>
        /// <returns></returns>
        public User UserShowByScreenName(Format format, string screen_name)
        {
            var url = SINA_URL + "users/show." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"screen_name",screen_name}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<User>(format);
        }

        /// <summary>
        /// 按用户ID返回用户资料以及用户的最新发布的一条微博消息。
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <returns></returns>
        public User UserShowByUserID(Int64 user_id)
        {
            return UserShowByUserID(Format.json, user_id);
        }

        /// <summary>
        /// 按用户ID返回用户资料以及用户的最新发布的一条微博消息。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="user_id">用户ID</param>
        /// <returns></returns>
        public User UserShowByUserID(Format format, Int64 user_id)
        {
            var url = SINA_URL + "users/show." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"user_id",user_id}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<User>(format);
        }
        #endregion

        #region 获取用户关注列表及每个关注用户最新一条微博
        /// <summary>
        /// 获取用户关注列表及每个关注用户的最新一条微博，返回结果按关注时间倒序排列，最新关注的用户排在最前面。
        /// </summary>
        /// <param name="id_name">用户ID(int64)或者昵称(string)。</param>
        /// <param name="cursor">用于分页请求，请求第1页cursor传-1，在返回的结果中会得到next_cursor字段，表示下一页的cursor。next_cursor为0表示已经到记录末尾。</param>
        /// <param name="count">每页返回的最大记录数，最大不能超过200，默认为20。</param>
        /// <returns></returns>
        public IList<User> UserFriends(object id_name, int cursor, int count)
        {
            return UserFriends(Format.json, id_name, cursor, count);
        }

        /// <summary>
        /// 获取用户关注列表及每个关注用户的最新一条微博，返回结果按关注时间倒序排列，最新关注的用户排在最前面。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="id_name">用户ID(int64)或者昵称(string)。</param>
        /// <param name="cursor">用于分页请求，请求第1页cursor传-1，在返回的结果中会得到next_cursor字段，表示下一页的cursor。next_cursor为0表示已经到记录末尾。</param>
        /// <param name="count">每页返回的最大记录数，最大不能超过200，默认为20。</param>
        /// <returns></returns>
        public IList<User> UserFriends(Format format, object id_name, int cursor, int count)
        {
            var url = SINA_URL + "statuses/friends/" + id_name + "." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"cursor",(cursor-1)*count},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<User>>(format);
        }
        #endregion

        #region 获取用户粉丝列表及及每个粉丝用户最新一条微博
        /// <summary>
        /// 获取用户粉丝列表及每个粉丝的最新一条微博，返回结果按粉丝的关注时间倒序排列，最新关注的粉丝排在最前面。每次返回20个,通过cursor参数来取得多于20的粉丝。注意目前接口最多只返回5000个粉丝。
        /// </summary>
        /// <param name="id_name">用户ID(int64)或者昵称(string)。</param>
        /// <param name="cursor">用于分页请求，请求第1页cursor传-1，在返回的结果中会得到next_cursor字段，表示下一页的cursor。next_cursor为0表示已经到记录末尾。</param>
        /// <param name="count">每页返回的最大记录数，最大不能超过200，默认为20。</param>
        /// <returns></returns>
        public IList<User> UserFollowers(object id_name, long cursor, long count)
        {
            return UserFollowers(Format.json, id_name, cursor, count);
        }

        /// <summary>
        /// 获取用户粉丝列表及每个粉丝的最新一条微博，返回结果按粉丝的关注时间倒序排列，最新关注的粉丝排在最前面。每次返回20个,通过cursor参数来取得多于20的粉丝。注意目前接口最多只返回5000个粉丝。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="id_name">用户ID(int64)或者昵称(string)。</param>
        /// <param name="cursor">用于分页请求，请求第1页cursor传-1，在返回的结果中会得到next_cursor字段，表示下一页的cursor。next_cursor为0表示已经到记录末尾。</param>
        /// <param name="count">每页返回的最大记录数，最大不能超过200，默认为20。</param>
        /// <returns></returns>
        public IList<User> UserFollowers(Format format, object id_name, long cursor, long count)
        {
            var url = SINA_URL + "statuses/followers/" + id_name + "." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"cursor",(cursor-1)*count},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<User>>(format);
        }
        #endregion

        #region 获取系统推荐用户
        /// <summary>
        /// 返回系统推荐的用户列表。
        /// </summary>
        /// <param name="category">分类，返回某一类别的推荐用户，默认为default。如果不在以下分类中，返回空列表</param>
        /// <returns></returns>
        public IList<User> UserHot(HotCategory category)
        {
            return UserHot(Format.json, category);
        }

        /// <summary>
        /// 返回系统推荐的用户列表。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="category">分类，返回某一类别的推荐用户，默认为default。如果不在以下分类中，返回空列表</param>
        /// <returns></returns>
        public IList<User> UserHot(Format format, HotCategory category)
        {
            var url = SINA_URL + "users/hot." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"category",category}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<User>>(format);
        }
        #endregion

        #region 更新当前登录用户所关注的某个好友的备注信息
        /// <summary>
        /// 更新当前登录用户所关注的某个好友的备注信息。
        /// </summary>
        /// <param name="user_id">需要修改备注信息的用户ID。</param>
        /// <param name="remark">备注信息</param>
        /// <returns></returns>
        public User UserUpdateRemark(object user_id, object remark)
        {
            return UserUpdateRemark(Format.json, user_id, remark);
        }

        /// <summary>
        /// 更新当前登录用户所关注的某个好友的备注信息。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="user_id">需要修改备注信息的用户ID。</param>
        /// <param name="remark">备注信息</param>
        /// <returns></returns>
        public User UserUpdateRemark(Format format, object user_id, object remark)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "User/friends/update_remark." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"user_id",user_id},
                {"remark",Uri.EscapeDataString(remark.ToString())}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST).ToEntity<User>(format);
        }
        #endregion

        #region 返回当前用户可能感兴趣的用户
        /// <summary>
        /// 返回当前用户可能感兴趣的用户。
        /// </summary>
        /// <param name="with_reason">是否返回推荐原因，可选值1/0。当值为1，返回结果中增加推荐原因，会大幅改变返回值格式。</param>
        /// <returns></returns>
        public User UserSuggestions(bool with_reason)
        {
            return UserSuggestions(Format.json, with_reason);
        }

        /// <summary>
        /// 返回当前用户可能感兴趣的用户。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="with_reason">是否返回推荐原因，可选值1/0。当值为1，返回结果中增加推荐原因，会大幅改变返回值格式。</param>
        /// <returns></returns>
        public User UserSuggestions(Format format,bool with_reason)
        {
            var url = SINA_URL + "users/suggestions." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"with_reason",Convert.ToByte(with_reason)}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<User>(format);
        }
        #endregion
        #endregion

        #region 关注接口
        #region 关注某用户
        /// <summary>
        /// 关注一个用户。关注成功则返回关注人的资料，目前的最多关注2000人。
        /// </summary>
        /// <param name="id_name">要关注的用户ID(int64)或者微博昵称(string)</param>
        /// <returns></returns>
        public User FriendshipsCreate(object id_name)
        {
            return FriendshipsCreate(Format.json, id_name);
        }

        /// <summary>
        /// 关注一个用户。关注成功则返回关注人的资料，目前的最多关注2000人。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="id_name">要关注的用户ID(int64)或者微博昵称(string)</param>
        /// <returns></returns>
        public User FriendshipsCreate(Format format,object id_name)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "friendships/create/" + id_name + "." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"source",BaseRequest.AppKey}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST).ToEntity<User>(format);
        }
        #endregion

        #region 取消关注
        /// <summary>
        /// 取消对某用户的关注。
        /// </summary>
        /// <param name="id_name">要关注的用户ID(int64)或者微博昵称(string)</param>
        /// <returns></returns>
        public User FriendshipsDestroy(object id_name)
        {
            return FriendshipsDestroy(Format.json, id_name);
        }

        /// <summary>
        /// 取消对某用户的关注。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="id_name">要关注的用户ID(int64)或者微博昵称(string)</param>
        /// <returns></returns>
        public User FriendshipsDestroy(Format format,object id_name)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "friendships/destroy/" + id_name + "." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"source",BaseRequest.AppKey}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST).ToEntity<User>(format);
        }
        #endregion

        #region 是否关注某用户(推荐使用friendships/show)
        /// <summary>
        /// 查看用户A是否关注了用户B。如果用户A关注了用户B，则返回true，否则返回false。
        /// </summary>
        /// <param name="user_a">用户A的用户ID</param>
        /// <param name="user_b">用户B的用户ID</param>
        /// <returns></returns>
        public string FriendshipsExists(object user_a, object user_b)
        {
            return FriendshipsExists(Format.json, user_a, user_b);
        }

        /// <summary>
        /// 查看用户A是否关注了用户B。如果用户A关注了用户B，则返回true，否则返回false。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="user_a">用户A的用户ID</param>
        /// <param name="user_b">用户B的用户ID</param>
        /// <returns></returns>
        public string FriendshipsExists(Format format, object user_a, object user_b)
        {
            var url = SINA_URL + "friendships/exists." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"user_a",user_a},
                {"user_b",user_b}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion

        #region 获取两个用户关系的详细情况
        /// <summary>
        /// 返回两个用户关注关系的详细情况
        /// </summary>
        /// <param name="source_id">源用户UID,如果不填，则默认取当前登录用户</param>
        /// <param name="target_id">要判断的目标用户ID</param>
        /// <returns></returns>
        public RelationShip FriendshipsShow(object source_id, object target_id)
        {
            var url = SINA_URL + "friendships/show." + Format.json;
            var dictionary = new Dictionary<object, object>()
            {
                {"source_id",source_id},
                {"target_id",target_id}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<RelationShip>(Format.json);
        }
        #endregion
        #endregion

        #region 话题接口
        #region 获取某人的话题
        /// <summary>
        /// 获取某用户的话题
        /// </summary>
        /// <param name="user_id">用户id</param>
        /// <param name="page">页码,默认值1</param>
        /// <param name="count">每页返回的记录数,默认值10</param>
        /// <returns></returns>
        public IList<Trend> Trends(object user_id, object page, object count)
        {
            return Trends(Format.json, user_id, page, count);
        }

        /// <summary>
        /// 获取某用户的话题
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="user_id">用户id</param>
        /// <param name="page">页码,默认值1</param>
        /// <param name="count">每页返回的记录数,默认值10</param>
        /// <returns></returns>
        public IList<Trend> Trends(Format format, object user_id, object page, object count)
        {
            var url = SINA_URL + "trends." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"user_id",user_id},
                {"page",page},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Trend>>(format);
        }
        #endregion

        #region 获取某一话题下的微博
        /// <summary>
        /// 获取某话题下的微博消息
        /// </summary>
        /// <param name="trend_name">话题关键词</param>
        /// <param name="page">页码,默认值1</param>
        /// <param name="count">每页返回的记录数,默认值20</param>
        /// <returns></returns>
        public IList<Status> TrendsStatuses(string trend_name, object page, int count)
        {
            return TrendsStatuses(Format.json, trend_name, page, count);
        }

        /// <summary>
        /// 获取某话题下的微博消息
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="trend_name">话题关键词</param>
        /// <param name="page">页码,默认值1</param>
        /// <param name="count">每页返回的记录数,默认值20</param>
        /// <returns></returns>
        public IList<Status> TrendsStatuses(Format format, string trend_name, object page,int count)
        {
            var url = SINA_URL + "trends/statuses." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"trend_name",Uri.EscapeDataString(trend_name)},
                {"page",page},
                {"count",count>50?50:count}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Status>>(format);
        }
        #endregion

        #region 关注某一个话题
        /// <summary>
        /// 关注某话题
        /// </summary>
        /// <param name="trend_name">要关注的话题关键词</param>
        /// <returns></returns>
        public string TrendsFollow(string trend_name)
        {
            return TrendsFollow(Format.json, trend_name);
        }

        /// <summary>
        /// 关注某话题
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="trend_name">要关注的话题关键词</param>
        /// <returns></returns>
        public string TrendsFollow(Format format, string trend_name)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "trends/follow." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"trend_name",trend_name}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST);
        }
        #endregion

        #region 取消关注的某一个话题(DELETE未实现)
        private string TrendsDestroy(Format format, object trend_id)
        {
            BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.DELETE);
            var url = SINA_URL + "trends/destroy." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"trend_id",trend_id}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion

        #region 按小时返回热门话题
        /// <summary>
        /// 返回最近一小时内的热门话题
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="base_app">是否基于当前应用来获取数据。1表示基于当前应用来获取数据。</param>
        /// <returns></returns>
        public string TrendsHourly(Format format, bool base_app)
        {
            var url = SINA_URL + "trends/hourly." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"base_app",Convert.ToByte(base_app)}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion

        #region 返回当日热门话题
        /// <summary>
        /// 返回最近一天内的热门话题。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="base_app">是否基于当前应用来获取数据。1表示基于当前应用来获取数据。</param>
        /// <returns></returns>
        public string TrendsDaily(Format format, bool base_app)
        {
            var url = SINA_URL + "trends/daily." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"base_app",Convert.ToByte(base_app)}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion

        #region 返回当周热门话题
        /// <summary>
        /// 返回最近一周内的热门话题。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="base_app">是否基于当前应用来获取数据。1表示基于当前应用来获取数据。</param>
        /// <returns></returns>
        public string TrendsWeekly(Format format, bool base_app)
        {
            var url = SINA_URL + "trends/weekly." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"base_app",Convert.ToByte(base_app)}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion
        #endregion

        #region Social Graph接口
        #region 获取用户关注对象uid列表
        /// <summary>
        /// 返回用户关注的一组用户的ID列表
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="id_name">用户的ID(int64)或者微博昵称(string)</param>
        /// <param name="cursor">游标。单页最多返回5000条记录。通过增加或减少cursor值来获取更多的关注列表。如果提供该参数，返回结果中将给出下一页的起始游标。</param>
        /// <param name="count">单页记录数。默认500，最大5000</param>
        /// <returns></returns>
        public string UserFriendsIDs(Format format, object id_name, long cursor, long count)
        {
            var url = SINA_URL + "friends/ids/" + id_name + "." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"cursor",(cursor-1)*count},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion

        #region 获取用户粉丝对象uid列表
        /// <summary>
        /// 返回用户的粉丝用户ID列表
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="id_name">用户的ID(int64)或者微博昵称(string)</param>
        /// <param name="cursor">游标。单页最多返回5000条记录。通过增加或减少cursor值来获取更多的粉丝列表。如果提供该参数，返回结果中将给出下一页的起始游标。</param>
        /// <param name="count">单页记录数。默认500，最大5000</param>
        /// <returns></returns>
        public string UserFollowersIDs(Format format, object id_name, long cursor, long count)
        {
            var url = SINA_URL + "followers/ids/" + id_name + "." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"cursor",(cursor-1)*count},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion
        #endregion

        #region 隐私设置接口
        #region 设置隐私信息
        /// <summary>
        /// 设置隐私信息
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="Comment">谁可以评论此账号的微薄。0：所有人，1：我关注的人。默认为0。</param>
        /// <param name="message">谁可以给此账号发私信。0：所有人，1：我关注的人。默认为1。</param>
        /// <param name="realname">是否允许别人通过真实姓名搜索到我， 0允许，1不允许，默认值1。</param>
        /// <param name="geo">发布微博，是否允许微博保存并显示所处的地理位置信息。 0允许，1不允许，默认值0。</param>
        /// <param name="badge">勋章展现状态，值—1私密状态，0公开状态，默认值0。</param>
        /// <returns></returns>
        private string AccountUpdatePrivacy(Format format, bool Comment, bool message, bool realname, bool geo, bool badge)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "account/update_privacy." + format;
            var dictionary = new Dictionary<object, object>()
            {
                //{"source",BaseRequest.AppKey},
                {"comment",Convert.ToByte(Comment)},
                {"message",Convert.ToByte(message)},
                {"realname",Convert.ToByte(realname)},
                {"geo",Convert.ToByte(geo)},
                {"badge",Convert.ToByte(badge)}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST);
        }
        #endregion

        #region 获取隐私信息
        /// <summary>
        /// 获取隐私信息设置情况
        /// </summary>
        /// <returns></returns>
        public Privacy AccountGetPrivacy()
        {
            return AccountGetPrivacy(Format.json);
        }

        /// <summary>
        /// 获取隐私信息设置情况
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <returns></returns>
        public Privacy AccountGetPrivacy(Format format)
        {
            var url = SINA_URL + "account/get_privacy." + format;
            return Request(url).ToEntity<Privacy>(format);
        }
        #endregion
        #endregion

        #region 黑名单接口
        #region 将某用户加入黑名单
        /// <summary>
        /// 将某用户加入登录用户的黑名单
        /// </summary>
        /// <param name="user_id">要加入黑名单的用户ID</param>
        /// <returns></returns>
        public User BlocksCreate(object user_id)
        {
            return BlocksCreate(Format.json, user_id);
        }

        /// <summary>
        /// 将某用户加入登录用户的黑名单
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="user_id">要加入黑名单的用户ID</param>
        /// <returns></returns>
        public User BlocksCreate(Format format, object user_id)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "blocks/create." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"user_id",user_id}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST).ToEntity<User>(format);
        }
        #endregion

        #region 将某用户移出黑名单
        /// <summary>
        /// 将某用户从当前登录用户的黑名单中移除
        /// </summary>
        /// <param name="user_id">要移出黑名单的用户ID</param>
        /// <returns></returns>
        public User BlocksDestroy(object user_id)
        {
            return BlocksDestroy(Format.json, user_id);
        }

        /// <summary>
        /// 将某用户从当前登录用户的黑名单中移除
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="user_id">要移出黑名单的用户ID</param>
        /// <returns></returns>
        public User BlocksDestroy(Format format, object user_id)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "blocks/destroy." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"user_id",user_id}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST).ToEntity<User>(format);
        }
        #endregion

        #region 检测某用户是否是黑名单用户
        /// <summary>
        /// 检测指定用户是否在登录用户的黑名单内
        /// </summary>
        /// <param name="user_id">要检测的用户ID</param>
        /// <returns></returns>
        public string BlocksExists(object user_id)
        {
            return BlocksExists(Format.json, user_id);
        }

        /// <summary>
        /// 检测指定用户是否在登录用户的黑名单内
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="user_id">要检测的用户ID</param>
        /// <returns></returns>
        public string BlocksExists(Format format, object user_id)
        {
            var url = SINA_URL + "blocks/exists." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"user_id",user_id}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion

        #region 列出黑名单用户(输出用户详细信息)
        /// <summary>
        /// 分页输出当前登录用户的黑名单列表，包括黑名单内的用户详细信息
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="count">单页记录数</param>
        /// <returns></returns>
        public IList<User> BlocksBlocking(object page, object count)
        {
            return BlocksBlocking(Format.json, page, count);
        }

        /// <summary>
        /// 分页输出当前登录用户的黑名单列表，包括黑名单内的用户详细信息
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="page">页码</param>
        /// <param name="count">单页记录数</param>
        /// <returns></returns>
        public IList<User> BlocksBlocking(Format format, object page, object count)
        {
            var url = SINA_URL + "blocks/blocking." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"page",page},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<User>>(format);
        }
        #endregion

        #region 列出分页黑名单用户（只输出id）
        /// <summary>
        /// 分页输出当前登录用户黑名单中的用户ID列表
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="page">页码</param>
        /// <param name="count">单页记录数</param>
        /// <returns></returns>
        public string BlocksBlockingIDs(Format format, object page, object count)
        {
            var url = SINA_URL + "blocks/blocking/ids." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"page",page},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion
        #endregion

        #region 用户标签接口
        #region 返回指定用户的标签列表
        /// <summary>
        /// 返回指定用户的标签列表
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="user_id">要获取的标签列表所属的用户ID</param>
        /// <param name="count">单页记录数,默认20，最大200</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public string Tags(Format format, object user_id, object count, object page)
        {
            var url = SINA_URL + "tags." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"user_id",user_id},
                {"count",count},
                {"page",page}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion

        #region 添加用户标签(中文返回400)
        /// <summary>
        /// 为当前登录用户添加新的用户标签
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="tags">要创建的一组标签，用半角逗号隔开</param>
        /// <returns></returns>
        public string TagsCreate(Format format,object tags)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "tags/create." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"tags",tags}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST);
        }
        #endregion

        #region 返回用户感兴趣的标签
        /// <summary>
        /// 获取当前登录用户感兴趣的推荐标签列表
        /// </summary>
        /// <param name="format"></param>
        /// <param name="page">页码。由于推荐标签是随机返回，故此特性暂不支持。默认1</param>
        /// <param name="count">单页记录数。默认10，最大10</param>
        /// <returns></returns>
        public string TagsSuggestions(Format format,object page, object count)
        {
            var url = SINA_URL + "tags/suggestions." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"page",page},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion

        #region 删除标签
        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="tag_id">要删除的标签ID</param>
        /// <returns></returns>
        public string TagsDestroy(Format format, object tag_id)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "tags/destroy." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"tag_id",tag_id}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST);
        }
        #endregion

        #region 批量删除标签
        /// <summary>
        /// 删除一组标签
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="ids">要删除的一组标签ID，以半角逗号隔开，一次最多提交20个ID。</param>
        /// <returns></returns>
        public string TagsDestroyBatch(Format format, object ids)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "tags/destroy_batch." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"ids",ids}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST);
        }
        #endregion
        #endregion

        #region 账号接口
        #region 验证当前用户身份是否合法
        /// <summary>
        /// 验证用户是否已经开通微博服务。如果用户的新浪通行证身份验证成功，且用户已经开通微博则返回 http状态为 200，否则返回403错误。该接口除source以外，无其他参数。
        /// </summary>
        /// <returns></returns>
        public User AccountVerifyCredentials()
        {
            return AccountVerifyCredentials(Format.json);
        }

        /// <summary>
        /// 验证用户是否已经开通微博服务。如果用户的新浪通行证身份验证成功，且用户已经开通微博则返回 http状态为 200，否则返回403错误。该接口除source以外，无其他参数。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <returns></returns>
        public User AccountVerifyCredentials(Format format)
        {
            var url = SINA_URL + "account/verify_credentials." + format;
            return Request(url).ToEntity<User>(format);
        }
        #endregion

        #region 获取当前用户API访问频率限制
        /// <summary>
        /// 获取API的访问频率限制。返回当前小时内还能访问的次数。频率限制是根据用户请求来做的限制
        /// </summary>
        /// <returns></returns>
        public RateLimitStatus AccountRateLimitStatus()
        {
            return AccountRateLimitStatus(Format.json);
        }

        /// <summary>
        /// 获取API的访问频率限制。返回当前小时内还能访问的次数。频率限制是根据用户请求来做的限制
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <returns></returns>
        public RateLimitStatus AccountRateLimitStatus(Format format)
        {
            var url = SINA_URL + "account/rate_limit_status." + format;
            return Request(url).ToEntity<RateLimitStatus>(format);
        }
        #endregion

        #region 当前用户退出登录
        /// <summary>
        /// 清除已验证用户的session，退出登录，并将cookie设为null。主要用于widget等web应用场合。
        /// </summary>
        /// <returns></returns>
        public User AccountEndSession()
        {
            return AccountEndSession(Format.json);
        }

        /// <summary>
        /// 清除已验证用户的session，退出登录，并将cookie设为null。主要用于widget等web应用场合。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <returns></returns>
        public User AccountEndSession(Format format)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "account/end_session." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"source",BaseRequest.AppKey}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST).ToEntity<User>(format);
        }
        #endregion

        #region 更改头像(未测试)
        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <param name="image">图片</param>
        /// <returns></returns>
        public User AccountUpdateProfileImage(byte[] image)
        {
            return AccountUpdateProfileImage(Format.json, image);
        }

        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="image">图片</param>
        /// <returns></returns>
        public User AccountUpdateProfileImage(Format format, byte[] image)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "account/update_profile_image." + format;
            return Request(url, string.Empty, image).ToEntity<User>(format);
        }
        #endregion

        #region 更改资料（需要申请权限weibo_app@vip.sina.com）
        /// <summary>
        /// 更新当前登录用户在新浪微博上的基本信息
        /// </summary>
        /// <param name="name">昵称，不超过20个字符</param>
        /// <param name="gender">性别， m 表示男性，f 表示女性</param>
        /// <param name="province">省份代码</param>
        /// <param name="city">城市代码</param>
        /// <param name="description">个人描述。不超过70个汉字</param>
        /// <returns></returns>
        public User AccountUpdateProfile(object name, object gender, object province, object city, object description)
        {
            return AccountUpdateProfile(Format.json, name, gender, province, city, description);
        }

        /// <summary>
        /// 更新当前登录用户在新浪微博上的基本信息
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="name">昵称，不超过20个字符</param>
        /// <param name="gender">性别， m 表示男性，f 表示女性</param>
        /// <param name="province">省份代码</param>
        /// <param name="city">城市代码</param>
        /// <param name="description">个人描述。不超过70个汉字</param>
        /// <returns></returns>
        public User AccountUpdateProfile(Format format, object name, object gender, object province, object city, object description)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "account/update_profile." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"name",name},
                {"gender",gender},
                {"province",province},
                {"city",city},
                {"description",description}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST).ToEntity<User>(format);
        }
        #endregion
        #endregion

        #region 收藏接口
        #region 获取当前用户的收藏列表
        /// <summary>
        /// 返回登录用户最近收藏的20条微博消息，和用户在主站上“我的收藏”页面看到的内容是一致的。
        /// </summary>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public IList<Status> Favorites(object page)
        {
            return Favorites(Format.json, page);
        }

        /// <summary>
        /// 返回登录用户最近收藏的20条微博消息，和用户在主站上“我的收藏”页面看到的内容是一致的。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public IList<Status> Favorites(Format format, object page)
        {
            var url = SINA_URL + "favorites." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"page",page}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Status>>(format);
        }
        #endregion

        #region 添加收藏
         /// <summary>
        /// 收藏一条微博消息
        /// </summary>
        /// <param name="id">要收藏的微博消息ID</param>
        /// <returns></returns>
        public Status FavoritesCreate(object id)
        {
            return FavoritesCreate(Format.json, id);
        }

        /// <summary>
        /// 收藏一条微博消息
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="id">要收藏的微博消息ID</param>
        /// <returns></returns>
        public Status FavoritesCreate(Format format,object id)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "favorites/create." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"id",id}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST).ToEntity<Status>(format);
        }
        #endregion

        #region 删除当前用户收藏的微博信息
        /// <summary>
        /// 删除微博收藏。注意：只能删除自己收藏的信息。
        /// </summary>
        /// <param name="id">要删除的已收藏的微博消息ID</param>
        /// <returns></returns>
        public Status FavoritesDestroy(object id)
        {
            return FavoritesDestroy(Format.json, id);
        }

        /// <summary>
        /// 删除微博收藏。注意：只能删除自己收藏的信息。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="id">要删除的已收藏的微博消息ID</param>
        /// <returns></returns>
        public Status FavoritesDestroy(Format format,object id)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "favorites/destroy/" + id + "." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"source",BaseRequest.AppKey}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST).ToEntity<Status>(format);
        }
        #endregion

        #region 批量删除收藏的微博信息
        /// <summary>
        /// 批量删除微博收藏。注意：只能删除自己收藏的信息。
        /// </summary>
        /// <param name="ids">要删除的一组已收藏的微博消息ID，用半角逗号隔开，一次提交最多提供20个ID</param>
        /// <returns></returns>
        public IList<Status> FavoritesDestroyBatch(object ids)
        {
            return FavoritesDestroyBatch(ids);
        }

        /// <summary>
        /// 批量删除微博收藏。注意：只能删除自己收藏的信息。
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="ids">要删除的一组已收藏的微博消息ID，用半角逗号隔开，一次提交最多提供20个ID</param>
        /// <returns></returns>
        public IList<Status> FavoritesDestroyBatch(Format format, object ids)
        {
            //BaseRequest = HttpRequestFactory.CreateHttpRequest(Method.POST);
            var url = SINA_URL + "favorites/destroy_batch." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"ids",ids}
            };
            return Request(url, dictionary.ToQueryString(),Method.POST).ToEntity<IList<Status>>(format);
        }
        #endregion
        #endregion

        #region 短链接口
        #region 将一个或多个长链接转换成短链接(不需要登录)
        /// <summary>
        /// 将一个或多个长链接转换成短链接
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="url_longs">需要转换的长链接，最多不超过20个。</param>
        /// <returns></returns>
        public string ShortUrlShorten(Format format,string[] url_longs)
        {
            var url = SINA_URL + "short_url/shorten." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"source",BaseRequest.AppKey},
                {"url_long",url_longs.ToQueryString("url_long")}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion

        #region 将一个或多个短链接还原成原始的长链接(不需要登录)
        /// <summary>
        /// 将一个或多个短链接还原成原始的长链接
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="url_shorts">需要还原的短链接，最多不超过20个</param>
        /// <returns></returns>
        public string ShortUrlExpand(Format format, string[] url_shorts)
        {
            var url = SINA_URL + "short_url/expand." + format;
            var dictionary = new Dictionary<object, object>()
            {
                 {"source",BaseRequest.AppKey},
                 {"url_short",url_shorts.ToQueryString("url_short")}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion

        #region 取得一个短链接在微博上的微博分享数（包含原创和转发的微博）(不需要登录)
        /// <summary>
        /// 取得一个短链接在微博上的微博分享数（包含原创和转发的微博）
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="url_shorts">需要取得分享数的短链接</param>
        /// <returns></returns>
        public string ShortUrlShareCounts(Format format, string[] url_shorts)
        {
            var url = SINA_URL + "short_url/share/counts." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"source",BaseRequest.AppKey},
                {"url_short",url_shorts.ToQueryString("url_short")}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion

        #region 取得包含指定单个短链接的最新微博内容
        /// <summary>
        /// 取得包含指定单个短链接的最新微博内容
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="url_short">需要取得关联微博内容的短链接</param>
        /// <param name="since_id">若指定此参数，则返回ID比since_id大的微博（即比since_id时间晚的微博），默认为0</param>
        /// <param name="max_id">若指定此参数，则返回ID小于或等于max_id的微博，默认为0</param>
        /// <param name="page">可选参数，返回结果的页序号，有分页限制</param>
        /// <param name="count">可选参数，每次返回的最大记录数（即页面大小），不大于200</param>
        /// <returns></returns>
        public string ShortUrlShareStatuses(Format format, string url_short, object since_id, object max_id, object page, object count)
        {
            var url = SINA_URL + "short_url/share/statuses." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"url_short",Uri.EscapeDataString(url_short)},
                {"since_id",since_id},
                {"max_id",max_id},
                {"page",page},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion

        #region 取得一个短链接在微博上的微博评论数(不需要登录)
        /// <summary>
        /// 取得一个短链接在微博上的微博评论数
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="url_shorts">需要取得评论数的短链接</param>
        /// <returns></returns>
        public string ShortUrlCommentCounts(Format format, string[] url_shorts)
        {
            var url = SINA_URL + "short_url/Comment/counts." + format;
            var dictionary = new Dictionary<object, object>()
            { 
                 {"source",BaseRequest.AppKey},
                 {"url_short",url_shorts.ToQueryString("url_short")}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion

        #region 取得包含指定单个短链接的最新微博评论内容
        /// <summary>
        /// 取得包含指定单个短链接的最新微博评论内容
        /// </summary>
        /// <param name="format">返回格式</param>
        /// <param name="url_short">需要取得关联微博评论内容的短链接</param>
        /// <param name="since_id">若指定此参数，则返回ID比since_id大的评论（即比since_id时间晚的评论），默认为0</param>
        /// <param name="max_id">若指定此参数，则返回ID小于或等于max_id的评论，默认为0</param>
        /// <param name="page">可选参数，返回结果的页序号，有分页限制</param>
        /// <param name="count">可选参数，每次返回的最大记录数（即页面大小），不大于200</param>
        /// <returns></returns>
        public string shortUrlCommentComments(Format format, string url_short, object since_id, object max_id, object page, object count)
        {
            var url = SINA_URL + "short_url/Comment/comments." + format;
            var dictionary = new Dictionary<object, object>()
            {
                {"url_short",Uri.EscapeDataString(url_short)},
                {"since_id",since_id},
                {"max_id",max_id},
                {"page",page},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion
        #endregion
        #endregion

        #region 自定义方法
        private string Request(string url)
        {
            return Request(url, string.Empty);
        }

        private string Request(string url, string postData)
        {
            return Request(url, postData, Method.GET);
        }

        private string Request(string url,  Method method)
        {
            return Request(url, string.Empty, method);
        }

        private string Request(string url, string postData,Method method)
        {
            BaseRequest = HttpRequestFactory.CreateHttpRequest(method);
            BaseRequest.Token = this.Token;
            BaseRequest.TokenSecret = this.TokenSecret;
            logger.Error("发送地址:" + url);
            return BaseRequest.Request(url, postData);
        }

        #region 带图片提交
        private string Request(string url,string postData,string filePath)
        {
            var PostRequest = HttpRequestFactory.CreateHttpRequest(Method.POST) as HttpPost;
            PostRequest.Token = this.Token;
            PostRequest.TokenSecret = this.TokenSecret;
            return PostRequest.RequestWithPicture(url, postData, filePath);
        }

        private string Request(string url, string postData, byte[] file)
        {
            var PostRequest = HttpRequestFactory.CreateHttpRequest(Method.POST) as HttpPost;
            PostRequest.Token = this.Token;
            PostRequest.TokenSecret = this.TokenSecret;
            return PostRequest.RequestWithPicture(url, postData, file);
        }
        #endregion
        #endregion
    }
}
