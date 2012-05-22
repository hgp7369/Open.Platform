using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using log4net;

namespace Open.NeteaseSDK
{
    public class NeteaseSerive
    {
        ILog logger = LogManager.GetLogger(typeof(NeteaseSerive));
        #region 属性/构造函数
        const string NETEASE_URL = "http://api.t.163.com/";
        private BaseHttpRequest BaseRequest;

        public NeteaseSerive()
            : this(Method.GET)
        {
        }

        public NeteaseSerive(Method method)
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
        #region 微博列表(Timeline)
        #region 获取当前登录用户关注用户的最新微博列表
        /// <summary>
        /// 获取当前登录用户关注用户的最新微博列表
        /// </summary>
        /// <param name="count">数量，默认为30条，最大为200条</param>
        /// <param name="since_id">该参数需传cursor_id,返回此条索引之前发的微博列表,不包含此条</param>
        /// <param name="max_id">该参数需传cursor_id,返回此条索引之后发的微博列表,包含此条</param>
        /// <param name="trim_user">值为true时返回的user对象只包含id属性，该属性能在一定程度上减少返回的数据量</param>
        /// <returns></returns>
        public IList<Statues> StatusesHomeTimeline(object count, object since_id, object max_id, object trim_user)
        {
            var url = NETEASE_URL + "statuses/home_timeline.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"count",count},
                {"since_id",since_id},
                {"max_id",max_id},
                {"trim_user",trim_user}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Statues>>(Format.json);
        }
        #endregion

        #region 获取最新的公共微博列表(随便看看)
        /// <summary>
        /// 获取最新的公共微博列表(随便看看),返回最新的20条微博。
        /// </summary>
        /// <param name="trim_user">值为true时返回的user对象只包含id属性，该属性能在一定程度上减少返回的数据量</param>
        /// <returns></returns>
        public IList<Statues> StatusesPublicTimeline(object trim_user)
        {
            var url = NETEASE_URL + "statuses/public_timeline.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"trim_user",trim_user}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Statues>>(Format.json);
        }
        #endregion

        #region 获取@评论当前登录用户的微博列表
        /// <summary>
        /// 获取@评论当前登录用户的微博列表
        /// </summary>
        /// <param name="count">数量，默认为30条，最大为200条</param>
        /// <param name="since_id">该参数需传cursor_id,返回此条索引之前发的微博列表,不包含此条</param>
        /// <param name="max_id">该参数需传cursor_id,返回此条索引之后发的微博列表,包含此条</param>
        /// <param name="trim_user">值为true时返回的user对象只包含id属性，该属性能在一定程度上减少返回的数据量</param>
        /// <returns></returns>
        public IList<Statues> StatusesMentions(object count, object since_id, object max_id, object trim_user)
        {
            var url = NETEASE_URL + "statuses/mentions.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"count",count},
                {"since_id",since_id},
                {"max_id",max_id},
                {"trim_user",trim_user}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Statues>>(Format.json);
        }
        #endregion

        #region 获取指定用户的微博列表
        /// <summary>
        /// 获取指定用户的微博列表
        /// </summary>
        /// <param name="screen_name">可以传user_id或screen_name</param>
        /// <param name="count">数量，默认为30条，最大为200条请求示例</param>
        /// <param name="since_id">该参数需传cursor_id,返回此条索引之前发的微博列表，不包含此条</param>
        /// <param name="max_id">该参数需传cursor_id,返回此条索引之后发的微博列表，包含此条</param>
        /// <param name="trim_user">值为true时返回的user对象只包含id属性，该属性能在一定程度上减少返回的数据量</param>
        /// <returns></returns>
        public IList<Statues> StatusesUserTimeline(object screen_name, object count, object since_id, object max_id, object trim_user)
        {
            var url = NETEASE_URL + "statuses/user_timeline.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"screen_name",screen_name},
                {"count",count},
                {"since_id",since_id},
                {"max_id",max_id},
                {"trim_user",trim_user}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Statues>>(Format.json);
        }
        #endregion

        #region 获取当前登录用户所发微博被转发的列表
        /// <summary>
        /// 获取当前登录用户所发微博被转发的列表
        /// </summary>
        /// <param name="count">可选参数，数量，默认为30条，最大为200条</param>
        /// <param name="since_id">可选参数，该参数需传cursor_id,返回此条索引之前发的微博列表,不包含此条</param>
        /// <returns></returns>
        public IList<Statues> StatusesRetweetsOfMe(object count, object since_id)
        {
            var url = NETEASE_URL + "statuses/retweets_of_me.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"count",count},
                {"since_id",since_id}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Statues>>(Format.json);
        }
        #endregion

        #region 获取我发出的评论列表
        /// <summary>
        /// 获取当前登录用户发出的评论列表(timeline)
        /// </summary>
        /// <param name="count">可选参数，数量，默认为30条，最大为200条</param>
        /// <param name="since_id">可选参数，该参数需传cursor_id,返回此条索引之前发的微博列表,不包含此条</param>
        /// <param name="max_id">可选参数，该参数需传cursor_id,返回此条索引之后发的微博列表，包含此条</param>
        /// <param name="trim_user">可选参数，值为true时返回的user对象只包含id属性，该属性能在一定程度上减少返回的数据量。默认为true</param>
        /// <returns></returns>
        public IList<Statues> StatusesCommentsByMe(object count, object since_id, object max_id, object trim_user)
        {
            var url = NETEASE_URL + "statuses/comments_by_me.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"count",count},
                {"since_id",since_id},
                {"max_id",max_id},
                {"trim_user",trim_user}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Statues>>(Format.json);
        }
        #endregion

        #region 获取我收到的评论列表
        /// <summary>
        /// 获取当前登录用户收到的评论列表(timeline)
        /// </summary>
        /// <param name="count">可选参数，数量，默认为30条，最大为200条</param>
        /// <param name="since_id">可选参数，该参数需传cursor_id,返回此条索引之前发的微博列表,不包含此条</param>
        /// <param name="max_id">可选参数，该参数需传cursor_id,返回此条索引之后发的微博列表，包含此条</param>
        /// <param name="trim_user">可选参数，值为true时返回的user对象只包含id属性，该属性能在一定程度上减少返回的数据量。默认为true</param>
        /// <returns></returns>
        public IList<Statues> StatusesCommentsToMe(object count, object since_id, object max_id, object trim_user)
        {
            var url = NETEASE_URL + "statuses/comments_by_me.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"count",count},
                {"since_id",since_id},
                {"max_id",max_id},
                {"trim_user",trim_user}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Statues>>(Format.json);
        }
        #endregion
        #endregion

        #region 微博
        #region 发布一条新微博
        /// <summary>
        /// 发布一条微博。微博内容过长则返回403状态;发布失败则返回500状态。
        /// 本接口不能直接传图，如果发带图片的微博，请先调用上传图片(statuses/upload)得到upload_image_url后，再调用statuses/update，并将upload_image_url作为作为status参数值
        /// </summary>
        /// <param name="status">微博内容，不得超过163个字符</param>
        /// <returns></returns>
        public Statues StatusesUpdate(object status)
        {
            var url = NETEASE_URL + "statuses/update.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"status",status}
            };
            return Request(url, dictionary.ToQueryString(), Method.POST).ToEntity<Statues>(Format.json);
        }
        #endregion

        #region 评论一条微博
        /// <summary>
        /// 评论一条微博
        /// </summary>
        /// <param name="id">必选参数，值为被评论微博的ID。如果回复某条评论，则此值为该评论的id</param>
        /// <param name="status">必选参数，评论内容</param>
        /// <param name="is_retweet">可选参数，是否转发 默认不转发 1为转发</param>
        /// <param name="is_comment_to_root">是否评论给原微博 默认不评论 1为评论</param>
        /// <returns></returns>
        public Statues StatusesReply(object id, object status, object is_retweet, object is_comment_to_root)
        {
            var url = NETEASE_URL + "statuses/reply.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"id",id},
                {"status",status},
                {"is_retweet",is_retweet},
                {"is_comment_to_root",is_comment_to_root}
            };
            return Request(url, dictionary.ToQueryString(), Method.POST).ToEntity<Statues>(Format.json);
        }
        #endregion

        #region 转发一条微博
        /// <summary>
        /// 转发一条微博
        /// </summary>
        /// <param name="id">必选参数，值为被转发微博的ID</param>
        /// <param name="status">可选参数，评论内容，默认为“转发微博</param>
        /// <param name="is_comment">可选参数，是否评论 默认不评论 1为评论</param>
        /// <param name="is_comment_to_root">可选参数，是否评论给原微博 默认不评论 1为评论</param>
        /// <returns></returns>
        public Statues StatusesRetweet(object id, object status, object is_comment, object is_comment_to_root)
        {
            var url = NETEASE_URL + "statuses/retweet/" + id + ".json";
            var dictionary = new Dictionary<object, object>()
            {
                {"status",status},
                {"is_comment",is_comment},
                {"is_comment_to_root",is_comment_to_root}
            };
            return Request(url, dictionary.ToQueryString(), Method.POST).ToEntity<Statues>(Format.json);
        }
        #endregion

        #region 获取指定微博信息
        /// <summary>
        /// 获取指定微博信息
        /// </summary>
        /// <param name="id">微博的ID</param>
        /// <returns></returns>
        public Statues StatusesShow(object id)
        {
            var url = NETEASE_URL + "statuses/show/" + id + ".json";
            return Request(url).ToEntity<Statues>(Format.json);
        }
        #endregion

        #region 删除指定的微博(401未授权)
        /// <summary>
        /// 删除指定的微博
        /// </summary>
        /// <param name="id">值为删除或撤销转发微博的ID</param>
        /// <returns></returns>
        private Statues StatusesDestroy(object id)
        {
            var url = NETEASE_URL + "statuses/destroy/" + id + ".json";
            return Request(url, Method.POST).ToEntity<Statues>(Format.json);
        }
        #endregion

        #region 获取指定微博的评论列表
        /// <summary>
        /// 获取指定微博的评论列表
        /// </summary>
        /// <param name="id">微博id</param>
        /// <param name="count">数量，默认为30条，最大为200条</param>
        /// <param name="since_id">该参数需传微博id，返回此条索引之前发的微博列表，不包含此条</param>
        /// <param name="max_id">该参数需传微博id，返回此条微博之后发的微博列表，包含此条</param>
        /// <param name="trim_user">值为true时返回的user对象只包含id属性，该参数能在一定程度上减少返回的数据量</param>
        /// <returns></returns>
        public IList<Statues> StatusesComments(object id, object count, object since_id, object max_id, object trim_user)
        {
            var url = NETEASE_URL + "statuses/comments/" + id + ".json";
            var dictionary = new Dictionary<object, object>()
            {
                {"count",count},
                {"since_id",since_id},
                {"max_id",max_id},
                {"trim_user",trim_user}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Statues>>(Format.json);
        }
        #endregion

        #region 获取指定微博的转发列表
        /// <summary>
        /// 查看指定微博的所有转发信息，返回的数据与WEB上单条微博页的转发数据一致
        /// </summary>
        /// <param name="id">微博id</param>
        /// <param name="count">数量，默认为30条，最大为200条</param>
        /// <param name="since_id">该参数需传微博id，返回此条索引之前发的微博列表，不包含此条</param>
        /// <param name="max_id">该参数需传微博id，返回此条微博之后发的微博列表，包含此条</param>
        /// <param name="trim_user">值为true时返回的user对象只包含id属性，该参数能在一定程度上减少返回的数据量</param>
        /// <returns></returns>
        public IList<Statues> StatusesRetweets(object id, object count, object since_id, object max_id, object trim_user)
        {
            var url = NETEASE_URL + "statuses/retweets/" + id + ".json";
            var dictionary = new Dictionary<object, object>()
            {
                {"count",count},
                {"since_id",since_id},
                {"max_id",max_id},
                {"trim_user",trim_user}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Statues>>(Format.json);
        }
        #endregion

        #region 获取一条微博被转发的用户信息(参数格式错误: retweetedId=null)
        /// <summary>
        /// 获取一条微博被转发的用户信息
        /// </summary>
        /// <param name="id">值为被转发微博的ID</param>
        /// <param name="count">返回的用户数量，默认为100条，最大100条</param>
        /// <returns></returns>
        private IList<User> StatusesIDRetweetedBy(object id, object count)
        {
            var url = NETEASE_URL + "statuses/" + id + "/retweeted_by.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"count",count},
                {"retweetedid",id}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<User>>(Format.json);
        }
        #endregion

        #region 上传图片(未实现)
        private string StatusesUpload()
        {
            return string.Empty;
        }
        #endregion
        #endregion

        #region 用户
        #region 获取当前用户
        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public string UserShow()
        {
            var url = NETEASE_URL + "users/show.json";
            return Request(url, string.Empty);
        }
        #endregion

        #region 猜你喜欢
        /// <summary>
        /// 返回用户可能感兴趣的用户，随机返回指定数目的用户
        /// </summary>
        /// <param name="count">指定随机返回的用户数目。默认为30个，最多返回30个</param>
        /// <param name="trim_user">值为true时返回最基本的用户数据，使用此参数可以减少返回数据量，默认为false</param>
        /// <returns></returns>
        public IList<User> UsersSuggestions(object count, object trim_user)
        {
            var url = NETEASE_URL + "users/suggestions.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"count",count},
                {"trim_user",trim_user}
            };
            return JsonHelper.DeserializeToObject(Request(url, dictionary.ToQueryString()), new { users = new List<User>() }).users;
        }
        #endregion

        #region 获取推荐I达人列表
        /// <summary>
        /// 获取推荐I达人列表
        /// </summary>
        /// <param name="cursor">分页参数</param>
        /// <param name="trim_user">值为true时返回最基本的用户数据，使用此参数可以减少返回数据量，默认为false</param>
        /// <returns></returns>
        public IList<User> UsersSuggestionsIFollowers(object cursor, object trim_user)
        {
            var url = NETEASE_URL + "users/suggestions_i_followers.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"cursor",cursor},
                {"trim_user",trim_user}
            };
            return JsonHelper.DeserializeToObject(Request(url, dictionary.ToQueryString()), new { users = new List<User>() }).users;
        }
        #endregion
        #endregion

        #region 关系
        #region 关注指定用户
        /// <summary>
        /// 关注指定用户。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信息；
        /// 根据参数查找不到对应的用户则返回404的状态和错误信息
        /// </summary>
        /// <param name="id_name">用户的个性网址，也可以传user_id</param>
        /// <returns></returns>
        public User FriendshipsCreate(object id_name)
        {
            var url = NETEASE_URL + "friendships/create.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"screen_name",id_name}
            };
            return Request(url, dictionary.ToQueryString(), Method.POST).ToEntity<User>(Format.json);
        }
        #endregion

        #region 取消关注指定用户
        /// <summary>
        /// 取消关注指定用户。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信息；
        /// 根据参数查找不到对应的用户则返回404的状态和错误信息
        /// </summary>
        /// <param name="id_name">用户的个性网址，也可以传user_id</param>
        /// <returns></returns>
        public User FriendshipsDestroy(object id_name)
        {
            var url = NETEASE_URL + "friendships/destroy.json";
            var dictionary = new Dictionary<object, object>() 
            {
                {"screen_name",id_name}
            };
            return Request(url, dictionary.ToQueryString(), Method.POST).ToEntity<User>(Format.json);
        }
        #endregion

        #region 获取两个用户的相互关注关系
        /// <summary>
        /// 获取两个用户的相互关注关系。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信息；
        /// 根据参数查找不到对应的用户则返回404的状态和错误信息。
        /// </summary>
        /// <param name="source_id_name">用户的个性网址，也可以传user_id,未传时使用当前登录用户作为source用户</param>
        /// <param name="target_id_name">用户的个性网址，也可以传user_id</param>
        /// <returns></returns>
        public RelationShip FriendshipsShow(object source_id_name, object target_id_name)
        {
            var url = NETEASE_URL + "friendships/show.json";
            var dictionary = new Dictionary<object, object>() 
            {
                {"source_screen_name",source_id_name},
                {"target_screen_name",target_id_name}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<RelationShip>(Format.json);
        }
        #endregion

        #region 获取指定用户的关注列表
        /// <summary>
        /// 获取指定用户的关注用户列表。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信；
        /// 根据参数查找不到对应的用户则返回404的状态和错误信息。
        /// </summary>
        /// <param name="id_name">用户的个性网址，也可以传user_id</param>
        /// <param name="cursor">分页参数，单页只能包含30个关注列表</param>
        /// <returns></returns>
        public IList<User> StatusesFriends(object id_name, object cursor)
        {
            var url = NETEASE_URL + "statuses/friends.json";
            var dictionary = new Dictionary<object, object>() 
            {
                {"screen_name",id_name},
                {"cursor",cursor}
            };
            return JsonHelper.DeserializeToObject(Request(url, dictionary.ToQueryString()), new { users = new List<User>() }).users;
        }
        #endregion

        #region 获取指定用户的被关注列表
        /// <summary>
        /// 获取指定被关注用户列表。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信；
        /// 根据参数查找不到对应的用户则返回404的状态和错误信息。
        /// </summary>
        /// <param name="id_name">用户的个性网址，也可以传user_id</param>
        /// <param name="cursor">分页参数，单页只能包含30个关注列表</param>
        /// <returns></returns>
        public IList<User> StatusesFollowers(object id_name, object cursor)
        {
            var url = NETEASE_URL + "statuses/followers.json";
            var dictionary = new Dictionary<object, object>() 
            {
                {"screen_name",id_name},
                {"cursor",cursor}
            };
            return JsonHelper.DeserializeToObject(Request(url, dictionary.ToQueryString()), new { users = new List<User>() }).users;
        }
        #endregion

        #region 获取当前用户的关注人名字列表
        /// <summary>
        /// 获取当前用户的关注人名字列表,可用于@提示功能
        /// </summary>
        /// <returns></returns>
        public string FriendsNames()
        {
            var url = NETEASE_URL + "friends/names.json";
            return Request(url);
        }
        #endregion

        #region 获取指定用户的被关注的人名字列表，可用于发私信查找用户
        /// <summary>
        /// 返回根据输入的拼音模糊查询后符合条件的指定用户的被关注的名字列表；
        /// 如果关注指定用户的人数不大于3000，从关注指定用户的人中查找；
        /// 如果关注指定用户的人数大于3000，从与指定用户互相关注的人以及关注指定用户的人中的3000个中查找；
        /// 可通过请求参数user_id或screen_name指定用户；
        /// 如果不指定用户，默认为当前用户为指定用户。
        /// </summary>
        /// <param name="id_name">用户的个性网址，也可以传user_id</param>
        /// <param name="cursor">分页参数，返回cursor之后的结果，包括cursor，可与count参数配合使用</param>
        /// <param name="count">数量，默认为30</param>
        /// <param name="matchkey">可选参数，按照拼音或汉字模糊匹配的key，默认为空串（不做过滤）</param>
        /// <param name="show_img">为true显示头像的图片，默认不显示</param>
        /// <returns></returns>
        public string StatusesFollowersNames(object id_name, object cursor, object count, object matchkey, object show_img)
        {
            var url = NETEASE_URL + "statuses/followers/names.json";
            var dictionary = new Dictionary<object, object>() 
            {
                {"screen_name",id_name},
                {"cursor",cursor},
                {"count",count},
                {"matchkey",matchkey},
                {"show_img",show_img}
            };
            return Request(url, dictionary.ToQueryString());
        }
        #endregion
        #endregion

        #region 热榜
        #region 获取当前的热门转发榜
        /// <summary>
        /// 获取当前的热门转发榜。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信息。
        /// type非法或者其对应的排行榜无数据则发挥404的状态。
        /// </summary>
        /// <param name="type">排行榜类型</param>
        /// <param name="size">返回数量,不传则为默认值(当前为50),最多50</param>
        /// <returns></returns>
        public IList<User> StatusesTopRetweets(TopRetweetType type, object size)
        {
            var url = NETEASE_URL + "statuses/topRetweets.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"type",type},
                {"size",size}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<User>>(Format.json);
        }
        #endregion
        #endregion

        #region 话题
        #region 推荐话题
        /// <summary>
        /// 推荐话题API，其效果与“我的首页”右侧的推荐话题一致。
        /// 没有任何推荐话题返回404；
        /// 推荐话题为专题时返回url，为话题时返回query。
        /// </summary>
        /// <returns></returns>
        public string TrendsRecommended()
        {
            var url = NETEASE_URL + "trends/recommended.json";
            return Request(url);
        }
        #endregion
        #endregion

        #region 私信
        #region 获取当前用户私信列表
        /// <summary>
        /// 获取当前用户私信列表。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信息；
        /// 根据参数查找不到对应的用户则返回404的状态和错误信息。
        /// </summary>
        /// <param name="since_id">上一页最后一条私信的id</param>
        /// <param name="count">获取私信的数量</param>
        /// <returns></returns>
        public IList<DirectMessage> DirectMessages(object since_id, object count)
        {
            var url = NETEASE_URL + "direct_messages.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"since_id",since_id},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<DirectMessage>>(Format.json);
        }
        #endregion

        #region 获取当前用户发送的私信列表
        /// <summary>
        /// 获取当前用户发送的私信列表。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信息；
        /// 根据参数查找不到对应的用户则返回404的状态和错误信息。
        /// </summary>
        /// <param name="since_id">上一页最后一条私信的id</param>
        /// <param name="count">获取私信的数量</param>
        /// <returns></returns>
        public IList<DirectMessage> DirectMessagesSent(object since_id, object count)
        {
            var url = NETEASE_URL + "direct_messages/sent.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"since_id",since_id},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<DirectMessage>>(Format.json);
        }
        #endregion

        #region 发送一条私信
        /// <summary>
        /// 发送一条私信。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信息；
        /// 根据参数查找不到对应的用户则返回404的状态和错误信息。
        /// text参数为空或超过163字符或收信用户没有关注当前用户或用户发给本人则返回403的状态和错误信息。
        /// </summary>
        /// <param name="screen_name">收信用户的昵称，即name</param>
        /// <param name="text">私信内容</param>
        /// <returns></returns>
        public DirectMessage DirectMessagesNew(object screen_name, object text)
        {
            var url = NETEASE_URL + "direct_messages/new.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"user",screen_name},
                {"text",text}
            };
            return Request(url, dictionary.ToQueryString(), Method.POST).ToEntity<DirectMessage>(Format.json);
        }
        #endregion

        #region 删除一条私信
        /// <summary>
        /// 删除一条私信。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信息；
        /// 根据id参数查找不到对应的私信则返回404的状态和错误信息。
        /// </summary>
        /// <param name="id">删除的私信id</param>
        /// <returns></returns>
        public DirectMessage DirectMessagesDestroy(object id)
        {
            var url = NETEASE_URL + "direct_messages/destroy/" + id + ".json";
            return Request(url, Method.POST).ToEntity<DirectMessage>(Format.json);
        }
        #endregion

        #region 会话分组方式获取当前用户私信
        /// <summary>
        /// 获取当前用户的私信会话列表（用户-用户对话方式）。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信息；
        /// 根据参数查找不到对应的用户则返回404的状态和错误信息。
        /// </summary>
        /// <param name="since_id">上一页最后一条会话的id</param>
        /// <param name="count">获取私信的数量</param>
        /// <returns></returns>
        public IList<DirectMessage> DirectMessagesGrouped(object since_id, object count)
        {
            var url = NETEASE_URL + "direct_messages/grouped.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"since_id",since_id},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<DirectMessage>>(Format.json);
        }
        #endregion

        #region 査看会话分组下的所有私信
        /// <summary>
        /// 获取用户-用户之间的会话私信
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信息；
        /// 根据参数查找不到对应的用户则返回404的状态和错误信息。
        /// </summary>
        /// <param name="user_id">会话列表中的会话id就是 user_id</param>
        /// <param name="since_id">上一页的最后一条私信id</param>
        /// <param name="count">获取私信的数量</param>
        /// <returns></returns>
        public IList<DirectMessage> DirectMessagesSession(object user_id, object since_id, object count)
        {
            var url = NETEASE_URL + "direct_messages/session.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"user_id",user_id},
                {"since_id",since_id},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<DirectMessage>>(Format.json);
        }
        #endregion

        #region 删除一组私信会话
        /// <summary>
        /// 删除一组私信。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信息；
        /// 根据id参数查找不到对应的私信则返回404的状态和错误信息。
        /// </summary>
        /// <param name="id">删除的私信会话id (相关用户的user id)</param>
        /// <returns></returns>
        public string DirectMessagesSessionDelete(object id)
        {
            var url = NETEASE_URL + "direct_messages/session/delete/" + id + ".json";
            return Request(url, Method.POST);
        }
        #endregion
        #endregion

        #region 账号
        #region 注册用户
        //第三方可以通过API注册新用户。本API只对高级第三方合作者开放，申请此API权限请发邮件到OpenAPI@yeah.net。
        #endregion

        #region 开通微博
        /// <summary>
        /// 网易通行证开通微博，当用户有通行证账号且未开通微博时，可以通过此API开通。
        /// 此API是唯一一个在没有开通微博账号下可以访问的API，但仍需要先有通行证账号，如未登录则返回401；
        /// 没有任何参数返回400。
        /// </summary>
        /// <param name="nick_name">昵称，如果昵称不合法，则返回400</param>
        /// <param name="real_name">真实姓名，如真实姓名不合法，则返回400</param>
        /// <param name="mobile">手机号，如手机号不合法，则返回400</param>
        /// <param name="id_num">身份证号，如身份证号不合法，则返回400</param>
        /// <returns></returns>
        public string AccountActivate(object nick_name, object real_name, object mobile, object id_num)
        {
            var url = NETEASE_URL + "account/activate.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"nick_name",nick_name},
                {"real_name",real_name},
                {"mobile",mobile},
                {"id_num",id_num}
            };
            return Request(url, dictionary.ToQueryString(), Method.POST);
        }
        #endregion

        #region 修改用户个人资料(需要额外授权)
        /// <summary>
        /// 修改用户个人资料。
        /// 本API只对高级第三方合作者开放，申请此API权限请发邮件到OpenAPI@yeah.net，未授权访问则返回403；
        /// 没有任何参数返回400。
        /// </summary>
        /// <param name="nick_name">昵称，如果昵称不合法，则返回400</param>
        /// <param name="real_name">真实姓名，如真实姓名不合法，则返回400</param>
        /// <param name="description">用户描述，如超过163个字符，则返回400</param>
        /// <param name="province">用户省份，如省份不合法，则返回400</param>
        /// <param name="city">用户城市，如城市不合法，则返回400</param>
        /// <returns></returns>
        public User AccountUpdateProfile(object nick_name, object real_name, object description, object province, object city)
        {
            var url = NETEASE_URL + "account/activate.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"nick_name",nick_name},
                {"real_name",real_name},
                {"description",description},
                {"province",province},
                {"city",city}
            };
            return Request(url, dictionary.ToQueryString(), Method.POST).ToEntity<User>(Format.json);
        }
        #endregion

        #region 修改用户个人头像(未实现)
        private void AccountUpdateProfileImage()
        { }
        #endregion

        #region 判断当前用户是否验证成功并返回该用户信息
        /// <summary>
        /// 判断当前用户是否验证成功并返回该用户信息。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信息；
        /// 此方法用了判断用户身份是否合法且已经开通微博。
        /// </summary>
        /// <returns></returns>
        public User AccountVerifyCredentials()
        {
            var url = NETEASE_URL + "account/verify_credentials.json";
            return Request(url).ToEntity<User>(Format.json);
        }
        #endregion

        #region 返回当前登录用户未读的消息数量
        /// <summary>
        /// 返回当前登录用户未读的新消息数量。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信息。
        /// </summary>
        /// <returns></returns>
        public UnReadMessage RemindsMessageLatest()
        {
            var url = NETEASE_URL + "reminds/message/latest.json";
            return Request(url).ToEntity<UnReadMessage>(Format.json);
        }
        #endregion

        #region 获取当前用户API访问频率限制
        /// <summary>
        /// 返回当前小时内剩余访问次数。
        /// </summary>
        /// <returns></returns>
        public string AccountRateLimitStatus()
        {
            var url = NETEASE_URL + "account/rate_limit_status.json";
            return Request(url);
        }
        #endregion
        #endregion

        #region 收藏
        #region 获取指定用户的收藏列表
        /// <summary>
        /// 获取指定用户的收藏。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 根据参数没有找到对应用户则返回404。
        /// </summary>
        /// <param name="id">可以为该用户的个性网址(screen_name)</param>
        /// <param name="since_id">返回微博数量，默认30，最大200</param>
        /// <param name="count">分页参数，传cursor_id,返回此条微博以前发的微博列表,不包含此条</param>
        /// <returns></returns>
        public IList<Statues> Favorites(object id, object since_id, object count)
        {
            var url = NETEASE_URL + "favorites/" + id + ".json";
            var dictionary = new Dictionary<object, object>()
            {
                {"since_id",since_id},
                {"count",count}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Statues>>(Format.json);
        }
        #endregion

        #region 添加收藏(401) 未经授权
        /// <summary>
        /// 添加收藏。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 根据参数没有找到对应用户则返回404
        /// </summary>
        /// <param name="id">要收藏的微博ID</param>
        /// <returns></returns>
        public Statues FavoritesCreate(object id)
        {
            var url = NETEASE_URL + "favorites/create/" + id + ".json";
            return Request(url, Method.POST).ToEntity<Statues>(Format.json);
        }
        #endregion

        #region 删除当前用户的收藏
        /// <summary>
        /// 添加收藏。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 根据参数没有找到对应用户则返回404。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Statues FavoritesDestroy(object id)
        {
            var url = NETEASE_URL + "favorites/" + id + ".json";
            return Request(url, Method.POST).ToEntity<Statues>(Format.json);
        }
        #endregion
        #endregion

        #region 黑名单
        #region 阻止指定用户
        /// <summary>
        /// 阻止指定用户，即将该用户添加进黑名单。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信息。
        /// 根据参数查找不到对应的用户则返回404的状态和错误信息。
        /// </summary>
        /// <param name="id_name">用户的个性网址，也可以传user_id</param>
        /// <returns></returns>
        public User BlocksCreate(object id_name)
        {
            var url = NETEASE_URL + "blocks/create.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"screen_name",id_name}
            };
            return Request(url, dictionary.ToQueryString(), Method.POST).ToEntity<User>(Format.json);
        }
        #endregion

        #region 取消已阻止的用户
        /// <summary>
        /// 取消已阻止的用户，即将该用户添移除黑名单。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信息。
        /// 根据参数查找不到对应的用户或不在黑名单中则返回404的状态和错误信息。
        /// </summary>
        /// <param name="id_name">用户的个性网址，也可以传user_id</param>
        /// <returns></returns>
        public User BlocksDestroy(object id_name)
        {
            var url = NETEASE_URL + "blocks/destroy.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"screen_name",id_name}
            };
            return Request(url, dictionary.ToQueryString(), Method.POST).ToEntity<User>(Format.json);
        }
        #endregion

        #region 判断指定用户是否已被阻止
        /// <summary>
        /// 判断是否已经阻止用户,如已阻止此用户则返回用户信息，如未阻止则返回状态404，同时提示“此用户未被加入黑名单”。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// 如果不是则返回401的状态和错误信息。
        /// 根据参数查找不到对应的用户则返回404的状态和错误信息。
        /// </summary>
        /// <param name="id_name">用户的个性网址，也可以传user_id</param>
        /// <returns></returns>
        public User BlocksExists(object id_name)
        {
            var url = NETEASE_URL + "blocks/exists.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"screen_name",id_name}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<User>(Format.json);
        }
        #endregion

        #region 查看已阻止的用户列表
        /// <summary>
        /// 返回当前登录用户屏蔽的用户列表。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// </summary>
        /// <returns></returns>
        public IList<User> BlocksBlocking()
        {
            var url = NETEASE_URL + "blocks/blocking.json";
            return Request(url).ToEntity<IList<User>>(Format.json);
        }
        #endregion

        #region 查看已阻止的用户列表id数组
        /// <summary>
        /// 返回当前登录用户屏蔽的用户id数组。
        /// 如果用户通行证身份验证成功且用户已经开通微博则返回http状态为200；
        /// </summary>
        /// <returns></returns>
        public string BlocksBlockingIDs()
        {
            var url = NETEASE_URL + "blocks/blocking/ids.json";
            return Request(url);
        }
        #endregion
        #endregion

        #region 搜索
        #region 搜索微博(404)
        /// <summary>
        /// 搜索微博。
        /// 未带q参数时会返回400状态
        /// </summary>
        /// <param name="q">关键字,最大长度25,如果以#起始的关键字会作为tag搜索精确匹配</param>
        /// <param name="page">当前页数，默认为第一页</param>
        /// <param name="per_page">返回数量,最大20</param>
        /// <param name="trim_user">值为true时返回的user对象只包含id属性，该属性能在一定程度上减少返回的数据量</param>
        /// <returns></returns>
        public IList<Statues> StatusesSearch(object q, object page, object per_page, object trim_user)
        {
            var url = NETEASE_URL + "blocks/create.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"q",q},
                {"page",page},
                {"per_page",per_page},
                {"trim_user",trim_user}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<Statues>>(Format.json);
        }
        #endregion

        #region 搜索用户(404)
        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="q">关键字,最大长度25,如果以#起始的关键字会作为tag搜索精确匹配</param>
        /// <param name="page">当前页数，默认为第一页</param>
        /// <param name="per_page">返回数量,最大20</param>
        /// <returns></returns>
        public IList<User> UsersSearch(object q, object page, object per_page)
        {
            var url = NETEASE_URL + "blocks/create.json";
            var dictionary = new Dictionary<object, object>()
            {
                {"q",q},
                {"page",page},
                {"per_page",per_page}
            };
            return Request(url, dictionary.ToQueryString()).ToEntity<IList<User>>(Format.json);
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

        private string Request(string url, Method method)
        {
            return Request(url, string.Empty, method);
        }

        private string Request(string url, string postData, Method method)
        {
            BaseRequest = HttpRequestFactory.CreateHttpRequest(method);
            BaseRequest.Token = this.Token;
            BaseRequest.TokenSecret = this.TokenSecret;
            logger.Error("发送地址:" + url);
            return BaseRequest.Request(url, postData);
        }

        #region 带图片提交
        private string Request(string url, string postData, string filePath)
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
