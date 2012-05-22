/*API文档更新时间: 2012-05-14*/
/*作者:http://weibo.com/u/1716169737*/
/*备注:地理信息 API未完成*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using log4net;

namespace Open.Tencent2SDK
{
    public class TencentSerive:OAuthBase,ISerive
    {
        ILog logger = LogManager.GetLogger(typeof(TencentSerive));
        #region 构造函数
        public TencentSerive(string app_key, string app_secret, string redirect_uri)
            : base(app_key, app_secret, redirect_uri)
        { }

        public TencentSerive()
            : base()
        { }
        #endregion

        #region 时间线
        #region 主页时间线
        /// <summary>
        /// 主页时间线
        /// </summary>
        /// <param name="pageflag">分页标识（0：第一页，1：向下翻页，2：向上翻页）</param>
        /// <param name="pagetime">本页起始时间（第一页：填0，向上翻页：填上一次请求返回的第一条记录时间，向下翻页：填上一次请求返回的最后一条记录时间）</param>
        /// <param name="reqnum">每次请求记录的条数（1-70条）</param>
        /// <param name="type">拉取类型,0x1=原创发表,0x2=转载,0x8=回复,0x10=空回,0x20=提及,0x40=点评</param>
        /// <param name="contenttype">内容过滤。0-表示所有类型，1-带文本，2-带链接，4-带图片，8-带视频，0x10-带音频</param>
        /// <returns></returns>
        public string Statuses_Home_Timeline(int pageflag, long pagetime, int reqnum, string type, int contenttype)
        {
            var dictionary = new Dictionary<object, object> 
            {
               {"pageflag",pageflag},
               {"pagetime",pagetime},
               {"reqnum",reqnum},
               {"type",type},
               {"contenttype",contenttype}
            };
            return HttpGet("statuses/home_timeline", dictionary);
        }
        #endregion

        #region 广播大厅时间线
        /// <summary>
        /// 广播大厅时间线
        /// </summary>
        /// <param name="pos">记录的起始位置（第一次请求时填0，继续请求时填上次请求返回的pos）</param>
        /// <param name="reqnum">每次请求记录的条数（1-100条）</param>
        /// <returns></returns>
        public string Statuses_Public_Timeline(int pos, int reqnum)
        {
            var dictionary = new Dictionary<object, object> 
            {
               {"pos",pos},
               {"reqnum",reqnum}
            };
            return HttpGet("statuses/public_timeline", dictionary);
        }
        #endregion

        #region 其他用户发表时间线
        /// <summary>
        /// 其他用户发表时间线
        /// </summary>
        /// <param name="pageflag">分页标识（0：第一页，1：向下翻页，2：向上翻页）</param>
        /// <param name="pagetime">本页起始时间（第一页：填0，向上翻页：填上一次请求返回的第一条记录时间，向下翻页：填上一次请求返回的最后一条记录时间）</param>
        /// <param name="reqnum">每次请求记录的条数（1-70条）</param>
        /// <param name="lastid"> 用于翻页，和pagetime配合使用（第一页：填0，向上翻页：填上一次请求返回的第一条记录id，向下翻页：填上一次请求返回的最后一条记录id）</param>
        /// <param name="name">你需要读取的用户的用户名</param>
        /// <param name="type">拉取类型,0x1=原创发表,0x2=转载,0x8=回复,0x10=空回,0x20=提及,0x40=点评</param>
        /// <param name="contenttype">内容过滤。0-表示所有类型，1-带文本，2-带链接，4-带图片，8-带视频，0x10-带音频</param>
        /// <returns></returns>
        public string Statuses_User_Timeline(int pageflag, long pagetime, int reqnum, long lastid, string name, int type, int contenttype)
        {
            var dictionary = new Dictionary<object, object> 
            {
               {"pageflag",pageflag},
               {"pagetime",pagetime},
               {"lastid",lastid},
               {"name",name},
               {"reqnum",reqnum},
               {"type",type},
               {"contenttype",contenttype}
            };
            return HttpGet("statuses/user_timeline", dictionary);
        }
        #endregion

        #region 用户提及时间线
        public string Statuses_Mentions_Timeline()
        {
            return null;
        }
        #endregion
        #endregion

        #region 账户相关
        #region 获取用户信息
        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <returns></returns>
        public User Users_Info()
        {
            return HttpGet<User>("user/info", new Dictionary<object, object>());
        }
        #endregion
        #endregion

        #region 微博相关
        #region 发表一条微博
        /// <summary>
        /// 发表一条微博
        /// </summary>
        /// <param name="content">微博内容</param>
        /// <param name="jing">经度，为实数，如113.421234（最多支持10位有效数字，可以填空）</param>
        /// <param name="wei">纬度，为实数，如22.354231（最多支持10位有效数字，可以填空）</param>
        /// <param name="syncflag">微博同步到空间分享标记（可选，0-同步，1-不同步，默认为0）</param>
        /// <returns></returns>
        public string T_Add(string content, string jing, string wei, int? syncflag)
        {
            var dictionary = new Dictionary<object, object> 
            {
               {"content",content},
               {"jing",jing},
               {"wei",wei},
               {"syncflag",syncflag??0}
            };
            return HttpPost("t/add", dictionary);
        }
        #endregion

        #region 发表一条带图片的微博
        /// <summary>
        /// 发表一条带图片的微博
        /// </summary>
        /// <param name="content">微博内容</param>
        /// <param name="pic">文件域表单名。本字段不要放在签名的参数中，不然请求时会出现签名错误</param>
        /// <param name="jing">经度，为实数，如113.421234（最多支持10位有效数字，可以填空）</param>
        /// <param name="wei">纬度，为实数，如22.354231（最多支持10位有效数字，可以填空）</param>
        /// <param name="syncflag">微博同步到空间分享标记（可选，0-同步，1-不同步，默认为0）</param>
        /// <returns></returns>
        public string T_Add_pic(string content, byte[] pic, string jing, string wei, int? syncflag)
        {
            var dictionary = new Dictionary<object, object> 
            {
               {"content",content},
               {"jing",jing},
               {"wei",wei},
               {"syncflag",syncflag??0}
            };
            return HttpPost("t/add_pic", dictionary, pic);
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
