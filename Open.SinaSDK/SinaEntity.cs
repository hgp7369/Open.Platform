using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Open.SinaSDK
{
    #region 用户信息
    [XmlRoot("user")]
    public class User
    {
        /// <summary>
        /// 用户UID
        /// </summary>
        public Int64 id { get; set; }

        /// <summary>
        /// 微博昵称
        /// </summary>
        public String screen_name { get; set; }

        /// <summary>
        /// 友好显示名称，同微博昵称
        /// </summary>
        public String name { get; set; }

        /// <summary>
        /// 省份编码（参考省份编码表）
        /// </summary>
        public String province { get; set; }

        /// <summary>
        /// 城市编码（参考城市编码表）
        /// </summary>
        public String city { get; set; } 

        /// <summary>
        /// 地址
        /// </summary>
        public String location { get; set; }

        /// <summary>
        /// 个人描述
        /// </summary>
        public String description { get; set; }

        /// <summary>
        /// 用户博客地址
        /// </summary>
        public String url { get; set; }

        /// <summary>
        /// 自定义图像
        /// </summary>
        public String profile_image_url { get; set; }

        /// <summary>
        /// 用户个性化URL
        /// </summary>
        public String domain { get; set; }

        /// <summary>
        /// 性别,m--男，f--女,n--未知
        /// </summary>
        public String gender { get; set; }

        /// <summary>
        /// 粉丝数
        /// </summary>
        public Int32 followers_count { get; set; }

        /// <summary>
        /// 关注数
        /// </summary>
        public Int32 friends_count { get; set; }

        /// <summary>
        /// 微博数
        /// </summary>
        public Int32 statuses_count { get; set; }

        /// <summary>
        /// 收藏数
        /// </summary>
        public Int32 favourites_count { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public String created_at { get; set; }

        /// <summary>
        /// 是否已关注(此特性暂不支持)
        /// </summary>
        public String following { get; set; }

        /// <summary>
        /// 加V标示，是否微博认证用户 
        /// </summary>
        public Boolean verified { get; set; }

        /// <summary>
        /// 最新发布的一条微博消息
        /// </summary>
        [XmlElement("status")]
        public Status status { get; set; }
    }
    #endregion

    #region 微博信息
    [XmlRoot("status")]
    public class Status
    {
        /// <summary>
        /// 微博ID
        /// </summary>
        public Int64 id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public String created_at { get; set; }

        /// <summary>
        /// 微博信息内容
        /// </summary>
        public String text { get; set; }

        /// <summary>
        /// 微博来源
        /// </summary>
        public String source { get; set; }

        /// <summary>
        /// 是否已收藏
        /// </summary>
        public Boolean favorited { get; set; }

        /// <summary>
        /// 是否被截断
        /// </summary>
        public Boolean truncated { get; set; }

        /// <summary>
        /// 回复ID
        /// </summary>
        public String in_reply_to_status_id { get; set; }

        /// <summary>
        /// 回复人UID
        /// </summary>
        public String in_reply_to_user_id { get; set; }

        /// <summary>
        /// 回复人昵称
        /// </summary>
        public String in_reply_to_screen_name { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public String thumbnail_pic { get; set; }

        /// <summary>
        /// 中型图片
        /// </summary>
        public String bmiddle_pic { get; set; }

        /// <summary>
        /// 原始图片
        /// </summary>
        public String original_pic { get; set; }

        /// <summary>
        /// 作者信息
        /// </summary>
        [XmlElement("user")]
        public User user { get; set; }

        /// <summary>
        /// 转发的博文，内容为status，如果不是转发，则没有此字段
        /// </summary>
        [XmlElement("status")]
        public Status retweeted_status { get; set; }
    }
    #endregion

    #region 评论信息
    [XmlRoot("comment")]
    public class Comment
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        public Int64 id { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public String text { get; set; }

        /// <summary>
        /// 评论来源
        /// </summary>
        public String source { get; set; }

        /// <summary>
        /// 是否收藏
        /// </summary>
        public Boolean favorited { get; set; }
        /// <summary>
        /// 是否被截断
        /// </summary>
        public Boolean truncated { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public String created_at { get; set; }
        /// <summary>
        /// 评论人信息,结构参考user
        /// </summary>
        [XmlElement("user")]
        public User user { get; set; }
        /// <summary>
        /// 评论的微博,结构参考status
        /// </summary>
        [XmlElement("status")]
        public Status status { get; set; }
        /// <summary>
        /// 评论来源，数据结构跟comment一致
        /// </summary>
        [XmlElement("comment")]
        public Comment reply_comment { get; set; }
    }
    #endregion

    #region 未读信息
    [XmlRoot("count")]
    public class Count
    {
        /// <summary>
        /// 新评论数
        /// </summary>
        public int comments { get; set; }
        /// <summary>
        /// 新粉丝数
        /// </summary>
        public int followers { get; set; }
        /// <summary>
        /// 新微博消息
        /// </summary>
        public int new_status { get; set; }        
        /// <summary>
        /// 新私信数
        /// </summary>
        public int dm { get; set; }
        /// <summary>
        /// 最新提到“我”的微博消息数
        /// </summary>
        public int mentions { get; set; }
    }
    #endregion

    #region 表情信息
    [XmlRoot("emotion")]
    public class Emotion
    {
        /// <summary>
        /// 表情使用的替代文字
        /// </summary>
        public String phrase { get; set; }
        /// <summary>
        /// 表情类型，image为普通图片表情，magic为魔法表情
        /// </summary>
        public String type { get; set; }
        /// <summary>
        /// 表情图片存放的位置
        /// </summary>
        public String url { get; set; }
        /// <summary>
        /// 是否为热门表情
        /// </summary>
        public Boolean is_hot { get; set; }
        /// <summary>
        /// 该表情在系统中的排序号码
        /// </summary>
        public Int32 order_number { get; set; }
        /// <summary>
        /// 表情分类
        /// </summary>
        public String category { get; set; }
    }
    #endregion

    #region 关系信息
    public class RelationShip
    {
        public RelationInfo source { get; set; }
        public RelationInfo target { get; set; }
    }

    public class RelationInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Int64 id{get;set;}
        /// <summary>
        /// 微博昵称
        /// </summary>
        public String screen_name{get;set;}
        /// <summary>
        /// 关注
        /// </summary>
        public Boolean following{get;set;}
        /// <summary>
        /// 被关注
        /// </summary>
        public Boolean followed_by{get;set;}
        /// <summary>
        /// 启用通知
        /// </summary>
        public Boolean notifications_enabled{get;set;}
    }
    #endregion

    #region 私信信息
    public class DirectMessage
    {
        /// <summary>
        /// 私信ID
        /// </summary>
        public Int64 id { get; set; }
        /// <summary>
        /// 私信内容
        /// </summary>
        public String text { get; set; }
        /// <summary>
        /// 发送人UID
        /// </summary>
        public Int64 sender_id { get; set; }
        /// <summary>
        /// 接受人UID
        /// </summary>
        public Int64 recipient_id { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public String created_at { get; set; }
        /// <summary>
        /// 发送人昵称
        /// </summary>
        public String sender_screen_name { get; set; }
        /// <summary>
        /// 接受人昵称
        /// </summary>
        public String recipient_screen_name { get; set; }
        /// <summary>
        /// 发送人信息
        /// </summary>
        public User sender { get; set; }
        /// <summary>
        /// 接受人信息
        /// </summary>
        public User recipient { get; set; }
    }
    #endregion

    #region 话题信息
    [XmlRoot("trend")]
    public class Trend
    {
        /// <summary>
        /// 话题ID
        /// </summary>
        public string trend_id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string hotword { get; set; }

        /// <summary>
        /// 微博数量
        /// </summary>
        public string num { get; set; }
    }
    #endregion

    #region 隐私信息
    [XmlRoot("result")]
    public class Privacy
    {
        /// <summary>
        /// 谁可以评论此账号的微薄。0：所有人，1：我关注的人。
        /// </summary>
        public string comment { get; set; }

        /// <summary>
        /// 谁可以给此账号发私信。0：所有人，1：我关注的人。
        /// </summary>
        public string dm { get; set; }

        /// <summary>
        /// 是否允许别人通过真实姓名搜索到我， 0允许，1不允许。
        /// </summary>
        public string real_name { get; set; }

        /// <summary>
        /// 发布微博，是否允许微博保存并显示所处的地理位置信息。 0允许，1不允许。
        /// </summary>
        public string geo { get; set; }

        /// <summary>
        /// 勋章展现状态，值—1私密状态，0公开状态。
        /// </summary>
        public string badge { get; set; }
    }
    #endregion

    #region API的访问频率限制
    [XmlRoot("hash")]
    public class RateLimitStatus
    {
        /// <summary>
        /// 点击命中
        /// </summary>
        [XmlElement("remaining-hits")]
        public Int32 remaining_hits {get;set;}

        /// <summary>
        /// 每小时限制
        /// </summary>
        [XmlElement("hourly-limit")]
        public Int32 hourly_limit{get;set;}

        /// <summary>
        /// 重置时间（秒）
        /// </summary>
        [XmlElement("reset-time-in-seconds")]
        public Int32 reset_time_in_seconds { get; set; }

        /// <summary>
        /// 重置时间
        /// </summary>
        [XmlElement("reset-time")]
        public String reset_time { get; set; }
    }
    #endregion
}
