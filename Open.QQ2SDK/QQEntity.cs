using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Open.QQ2SDK
{
    #region 请求OAuth服务返回包括Access Token等消息类型。
    /// <summary>
    /// 请求OAuth服务返回包括Access Token等消息类型。
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// 要获取的Access Token。
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// Access Token的有效期，以秒为单位。
        /// </summary>
        public string expires_in { get; set; }

        /// <summary>
        /// 获取到的刷新token。
        /// </summary>
        public string refresh_token { get; set; }
    }
    #endregion

    #region OpenID信息
    public class OpenInfo
    {
        public int client_id { get; set; }
        public string openid { get; set; }
    }
    #endregion

    #region 用户信息
    public class User
    {
        /// <summary>
        /// 返回码 0: 正确返回,其它: 失败
        /// </summary>
        public int ret { get; set; }

        /// <summary>
        /// 如果ret<0，会有相应的错误信息提示，返回数据全部用UTF-8编码
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname { get; set; }

        /// <summary>
        /// 头像URL 30X30
        /// </summary>
        public string figureurl { get; set; }

        /// <summary>
        /// 头像URL 50X50
        /// </summary>
        public string figureurl_1 { get; set; }

        /// <summary>
        /// 头像URL 100X100
        /// </summary>
        public string figureurl_2 { get; set; }

        /// <summary>
        /// 是否vip会员
        /// </summary>
        public bool vip { get; set; }

        /// <summary>
        /// vip会员等级
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// 性别。如果获取不到则默认返回“男”
        /// </summary>
        public string gender { get; set; }
    }
    #endregion

    #region 分享信息
    public class Share
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public string ret { get; set; }
        /// <summary>
        /// 如果ret<0，会有相应的错误信息提示，返回数据全部用UTF-8编码
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 分享信息ID
        /// </summary>
        public long share_id { get; set; }
    }
    #endregion

    #region 微博信息
    public class Statuses
    {
        /// <summary>
        /// 微博列表
        /// </summary>
        public IList<Status> statuses { get; set; }

        /// <summary>
        /// 前游标
        /// </summary>
        public int previous_cursor { get; set; }

        /// <summary>
        /// 后游标
        /// </summary>
        public long next_cursor { get; set; }

        /// <summary>
        /// 返回数
        /// </summary>
        public int total_number { get; set; }
    }

    public class Status
    {
        /// <summary>
        /// 字符串型的微博ID
        /// </summary>
        public string idstr { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string created_at { get; set; }

        /// <summary>
        /// 微博ID
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 微博信息内容
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 微博来源
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// 是否已收藏
        /// </summary>
        public bool favorited { get; set; }

        /// <summary>
        /// 是否被截断
        /// </summary>
        public bool truncated { get; set; }

        /// <summary>
        /// 回复ID
        /// </summary>
        public string in_reply_to_status_id { get; set; }

        /// <summary>
        /// 回复人UID
        /// </summary>
        public string in_reply_to_user_id { get; set; }

        /// <summary>
        /// 回复人昵称
        /// </summary>
        public string in_reply_to_screen_name { get; set; }

        /// <summary>
        /// 微博MID
        /// </summary>
        public long mid { get; set; }

        /// <summary>
        /// 中等尺寸图片地址
        /// </summary>
        public string bmiddle_pic { get; set; }

        /// <summary>
        /// 原始图片地址
        /// </summary>
        public string original_pic { get; set; }

        /// <summary>
        /// 缩略图片地址
        /// </summary>
        public string thumbnail_pic { get; set; }

        /// <summary>
        /// 转发数
        /// </summary>
        public int reposts_count { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int comments_count { get; set; }

        /// <summary>
        /// 微博附加注释信息
        /// </summary>
        public Array annotations { get; set; }

        /// <summary>
        /// 微博作者的用户信息字段
        /// </summary>
        public User user { get; set; }
    }
    #endregion

    #region 转发信息
    public class Reposts
    {
        /// <summary>
        /// 转发列表
        /// </summary>
        public IList<Repost> reposts { get; set; }

        /// <summary>
        /// 前游标
        /// </summary>
        public int previous_cursor { get; set; }

        /// <summary>
        /// 后游标
        /// </summary>
        public long next_cursor { get; set; }

        /// <summary>
        /// 返回数
        /// </summary>
        public int total_number { get; set; }
    }

    public class Repost
    {
        /// <summary>
        /// 字符串型的微博ID
        /// </summary>
        public string idstr { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string created_at { get; set; }

        /// <summary>
        /// 微博ID
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 微博信息内容
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 微博来源
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// 是否已收藏
        /// </summary>
        public bool favorited { get; set; }

        /// <summary>
        /// 是否被截断
        /// </summary>
        public bool truncated { get; set; }

        /// <summary>
        /// 回复ID
        /// </summary>
        public string in_reply_to_status_id { get; set; }

        /// <summary>
        /// 回复人UID
        /// </summary>
        public string in_reply_to_user_id { get; set; }

        /// <summary>
        /// 回复人昵称
        /// </summary>
        public string in_reply_to_screen_name { get; set; }

        /// <summary>
        /// 微博MID
        /// </summary>
        public long mid { get; set; }

        /// <summary>
        /// 中等尺寸图片地址
        /// </summary>
        public string bmiddle_pic { get; set; }

        /// <summary>
        /// 原始图片地址
        /// </summary>
        public string original_pic { get; set; }

        /// <summary>
        /// 缩略图片地址
        /// </summary>
        public string thumbnail_pic { get; set; }

        /// <summary>
        /// 转发数
        /// </summary>
        public int reposts_count { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int comments_count { get; set; }

        /// <summary>
        /// 微博附加注释信息
        /// </summary>
        public Array annotations { get; set; }

        /// <summary>
        /// 微博作者的用户信息字段
        /// </summary>
        public User user { get; set; }

        /// <summary>
        /// 转发的微博信息字段
        /// </summary>
        public Status retweeted_status { get; set; }
    }
    #endregion

    #region 评论信息
    public class Comments
    {
        /// <summary>
        /// 评论列表
        /// </summary>
        public IList<Comment> comments { get; set; }

        /// <summary>
        /// 前游标
        /// </summary>
        public int previous_cursor { get; set; }

        /// <summary>
        /// 后游标
        /// </summary>
        public long next_cursor { get; set; }

        /// <summary>
        /// 返回数
        /// </summary>
        public int total_number { get; set; }
    }

    public class Comment
    {
        /// <summary>
        /// 评论创建时间
        /// </summary>
        public string created_at { get; set; }

        /// <summary>
        /// 评论的ID
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 评论的内容
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 评论的来源
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// 评论的MID
        /// </summary>
        public long mid { get; set; }

        /// <summary>
        /// 评论作者的用户信息字段
        /// </summary>
        public User user { get; set; }

        /// <summary>
        /// 评论的微博信息字段
        /// </summary>
        public Status status { get; set; }

        /// <summary>
        /// 回复的评论信息字段
        /// </summary>
        public Comment reply_comment { get; set; }
    }
    #endregion

    #region 表情信息
    public class Emotions
    {
        /// <summary>
        /// 分类名
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// 是否官方
        /// </summary>
        public bool common { get; set; }

        /// <summary>
        /// 是否热门
        /// </summary>
        public bool hot { get; set; }

        /// <summary>
        /// icon
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 含义
        /// </summary>
        public string phrase { get; set; }

        /// <summary>
        /// 表情id
        /// </summary>
        public string picid { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string value { get; set; }
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
        /// <summary>
        /// 启用通知
        /// </summary>
        public Boolean notifications_enabled { get; set; }
    }
    #endregion

    #region 收藏信息
    public class Favorites
    {
        /// <summary>
        /// 收藏列表
        /// </summary>
        public IList<Favorite> favorites { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int total_number { get; set; }
    }

    public class Favorite
    {
        /// <summary>
        /// 微博信息
        /// </summary>
        public Status status { get; set; }

        /// <summary>
        /// 收藏时间
        /// </summary>
        public string favorited_time { get; set; }
    }
    #endregion
}
