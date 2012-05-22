using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.NeteaseSDK
{
    #region 微博信息
    public class Statues
    {
        /// <summary>
        /// 微博ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 微博信息来源
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// 微博类型，normal即原创，retweet即转发，reply即评论，deleted即删除
        /// </summary>
        public string flag { get; set; }

        /// <summary>
        /// 微博作者信息，具体字段见用户(users)
        /// </summary>
        public User user { get; set; }

        /// <summary>
        /// 微博创建时间
        /// </summary>
        public string created_at { get; set; }

        /// <summary>
        /// 微博正文
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 回复的微博id
        /// </summary>
        public string in_reply_to_status_id { get; set; }

        /// <summary>
        /// 回复的微博作者的id
        /// </summary>
        public string in_reply_to_user_id { get; set; }

        /// <summary>
        /// 回复的微博作者的个性网址
        /// </summary>
        public string in_reply_to_screen_name { get; set; }

        /// <summary>
        /// 被转发数
        /// </summary>
        public string retweet_count { get; set; }

        /// <summary>
        /// 被评论数
        /// </summary>
        public string comments_count { get; set; }

        /// <summary>
        /// 用户收藏时间（如未收藏则为null）
        /// </summary>
        public string favorited_at { get; set; }

        /// <summary>
        /// 回复的微博正文
        /// </summary>
        public string in_reply_to_status_text { get; set; }

        /// <summary>
        /// 回复微博的作者昵称
        /// </summary>
        public string in_reply_to_user_name { get; set; }

        /// <summary>
        /// 是否收藏
        /// </summary>
        public string favorited { get; set; }

        /// <summary>
        /// timeline上该微博的游标，在timeline上需传递此参数进行分页
        /// </summary>
        public string cursor_id { get; set; }

        /// <summary>
        /// 整个对话中根微博ID
        /// </summary>
        public string root_in_reply_to_status_id { get; set; }

        /// <summary>
        /// 是否被其他用户转发
        /// </summary>
        public string is_retweet_by_user { get; set; }

        /// <summary>
        /// 转发用户ID（返回用户关注者中第一个转发此微博）
        /// </summary>
        public string retweet_user_id { get; set; }

        /// <summary>
        /// 转发用户的昵称
        /// </summary>
        public string retweet_user_name { get; set; }

        /// <summary>
        /// 转发用户的个性网址
        /// </summary>
        public string retweet_user_screen_name { get; set; }

        /// <summary>
        /// 转发时间
        /// </summary>
        public string retweet_created_at { get; set; }

        /// <summary>
        /// 微博原文作者的user_id
        /// </summary>
        public string root_in_reply_to_user_id { get; set; }

        /// <summary>
        /// 微博原文作者的screen_name
        /// </summary>
        public string root_in_reply_to_screen_name { get; set; }

        /// <summary>
        /// 微博原文作者的昵称
        /// </summary>
        public string root_in_reply_to_user_name { get; set; }

        /// <summary>
        /// 微博原文内容
        /// </summary>
        public string root_in_reply_to_status_text { get; set; }
    }
    #endregion

    #region 用户信息
    public class User
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 用户地址
        /// </summary>
        public string location { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 用户描述
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 用户头像URL，最大长度为255
        /// </summary>
        public string profile_image_url { get; set; }
        /// <summary>
        /// 个性网址
        /// </summary>
        public string screen_name { get; set; }
        /// <summary>
        /// 0为保密，1为男性，2为女性
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// 个人博客地址
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 用户注册时间
        /// </summary>
        public string created_at { get; set; }
        /// <summary>
        /// 被关注数
        /// </summary>
        public string followers_count { get; set; }
        /// <summary>
        /// 关注数
        /// </summary>
        public string friends_count { get; set; }
        /// <summary>
        /// 收藏数
        /// </summary>
        public string favourites_count { get; set; }
        /// <summary>
        /// 发微博数
        /// </summary>
        public string statuses_count { get; set; }
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
        public Int64 id { get; set; }
        /// <summary>
        /// 微博昵称
        /// </summary>
        public String screen_name { get; set; }
        /// <summary>
        /// 关注
        /// </summary>
        public Boolean following { get; set; }
        /// <summary>
        /// 被关注
        /// </summary>
        public Boolean followed_by { get; set; }
    }
    #endregion

    #region 私信信息
    public class DirectMessage
    {
        /// <summary>
        /// ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 发送者信息
        /// </summary>
        public User sender { get; set; }
        /// <summary>
        /// 私信内容
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string created_at { get; set; }
        /// <summary>
        /// 发送者ID
        /// </summary>
        public string recipient_id { get; set; }
        /// <summary>
        /// 接受者ID
        /// </summary>
        public string sender_id { get; set; }
        /// <summary>
        /// 发送者昵称
        /// </summary>
        public string sender_screen_name { get; set; }
        /// <summary>
        /// 接受者昵称
        /// </summary>
        public string recipient_screen_name { get; set; }
        /// <summary>
        /// 接受者信息
        /// </summary>
        public User recipient { get; set; }
        /// <summary>
        /// 被关注
        /// </summary>
        public string followed_by { get; set; }
    }
    #endregion

    #region 未读的新消息
    public class UnReadMessage
    {
        /// <summary>
        /// 当前登录用户id
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 新@回复数量
        /// </summary>
        public string replyCount { get; set; }
        /// <summary>
        /// 新被关注数量
        /// </summary>
        public string followedCount { get; set; }
        /// <summary>
        /// 新私信数量
        /// </summary>
        public string directMessageCount { get; set; }
        /// <summary>
        /// 新home_timeline数量
        /// </summary>
        public string timelineCount { get; set; }
        /// <summary>
        /// 新评论数量
        /// </summary>
        public string commentOfMeCount { get; set; }
        /// <summary>
        /// 新转发我的数量
        /// </summary>
        public string retweetOfMeCount { get; set; }
    }
    #endregion
}
