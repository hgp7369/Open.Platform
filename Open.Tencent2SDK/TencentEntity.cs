using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Open.Tencent2SDK
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
        /// 用户帐户名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 用户统一标识，可以唯一标识一个用户
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 与openid对应的用户key，是验证openid身份的验证密钥
        /// </summary>
        public string openkey { get; set; }
    }
    #endregion

    #region 用户信息
    public class Users
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        public IList<User> users { get; set; }

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

    public class User
    {
        /// <summary>
        /// 信息
        /// </summary>
        public Data data { get; set; }

        /// <summary>
        /// 返回错误码
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 返回值，0-成功，非0-失败
        /// </summary>
        public int ret { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string msg { get; set; }
    }

    public class Data
    {
        /// <summary>
        /// 用户帐户名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 用户唯一id，与name相对应
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string nick { get; set; }

        /// <summary>
        /// 头像url
        /// </summary>
        public string head { get; set; }

        /// <summary>
        /// 是否实名认证，0-未实名认证，1-已实名认证
        /// </summary>
        public int isrealname { get; set; }

        /// <summary>
        /// 所在地
        /// </summary>
        public string location { get; set; }

        /// <summary>
        /// 是否认证用户
        /// </summary>
        public int isvip { get; set; }

        /// <summary>
        /// 是否企业机构
        /// </summary>
        public int isent { get; set; }

        /// <summary>
        /// 个人介绍
        /// </summary>
        public string introduction { get; set; }

        /// <summary>
        /// 认证信息
        /// </summary>
        public string verifyinfo { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 出生年
        /// </summary>
        public string birth_year { get; set; }

        /// <summary>
        /// 出生月
        /// </summary>
        public string birth_month { get; set; }

        /// <summary>
        /// 出生天
        /// </summary>
        public string birth_day { get; set; }

        /// <summary>
        /// 国家id
        /// </summary>
        public string country_code { get; set; }

        /// <summary>
        /// 地区id
        /// </summary>
        public string province_code { get; set; }

        /// <summary>
        /// 城市id
        /// </summary>
        public string city_code { get; set; }

        /// <summary>
        /// 用户性别，1-男，2-女，0-未填写
        /// </summary>
        public int sex { get; set; }

        /// <summary>
        /// 听众数
        /// </summary>
        public int fansnum { get; set; }

        /// <summary>
        /// 收听的人数
        /// </summary>
        public int idolnum { get; set; }

        /// <summary>
        /// 发表的微博数
        /// </summary>
        public int tweetnum { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string tag { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        public string edu { get; set; }
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
