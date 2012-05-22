using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using Open.Sina2SDK;
using Open.QQ2SDK;
using Open.Tencent2SDK;
using Open.Netease2SDK;
using log4net;

/*请关注 http://weibo.com/u/1716169737 如有疑问请发私信与我*/
namespace Open.Demo
{
    public partial class Default2 : System.Web.UI.Page
    {
        #region Sina
        //const long clientID = 3432766229;//app_key
        //const string responseType = "code";
        //const string redirectUri = "http://www.5sing.com/default2.aspx";//回调地址
        //const string clientSecret = "6951ccc0ff6e1b02ddc9ec5859e90c40";//app_secret

        //const long clientID = 1989070711;//app_key
        //const string responseType = "code";
        //const string redirectUri = "http://www.luantui.com/default2.aspx";//回调地址
        //const string clientSecret = "02f8dc261653a93462a3e3b12fe0b910";//app_secret

        //const long clientID = 3488955228;//app_key
        //const string responseType = "code";
        //const string redirectUri = "http://www.19online.cn/default2.aspx";//回调地址
        //const string clientSecret = "9aa89f67b1c8186dce05807eea20da97";//app_secret
        //ILog logger = LogManager.GetLogger(typeof(SinaSerive));
        
        //SinaSerive serive = new SinaSerive(clientID.ToString(),clientSecret,redirectUri);
        #endregion

        #region QQ
        //const string clientID = "200079";//app_key
        //const string responseType = "code";
        //const string redirectUri = "http://www.5sing.com/Default2.aspx";//回调地址
        //const string clientSecret = "1e2b13c2631efb5c224b499dccda40ac";//app_secret
        //QQSerive serive = new QQSerive(clientID.ToString(), clientSecret, redirectUri);
        #endregion

        #region TencentWeiBo
        const string clientID = "c15a9ac4493f4c9a8c11cba251e9838f";//app_key
        const string responseType = "code";
        const string redirectUri = "http://www.luantui.com/default2.aspx";//回调地址
        const string clientSecret = "5868c176e688d56d8070bc35cf7e4663";//app_secret

        TencentSerive serive = new TencentSerive(clientID.ToString(), clientSecret, redirectUri);
        #endregion

        #region NeteaseWeiBo
        //const string clientID = "Ow3fH7y01AOsoO5C";//app_key
        //const string responseType = "code";
        //const string redirectUri = "http://www.luantui.com/default2.aspx";//回调地址
        //const string clientSecret = "lmQobtjNzNbw6dZcKj59o7HsZNaiq8g2";//app_secret

        //NeteaseSerive serive = new NeteaseSerive(clientID.ToString(), clientSecret, redirectUri);
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetCode();
            }
        }
        
        //请求
        protected void Button1_Click(object sender, EventArgs e)
        {            
            serive.GetAuthorizationCode();
        }

        //换取access_token
        protected void Button2_Click(object sender, EventArgs e)
        {
            //serive.GetAccessTokenByAuthorizationCode(Request["code"]);
            serive.GetAccessTokenByAuthorizationCode();

            foreach (var p in serive.Token.GetType().GetProperties())
            {
                Response.Write(p.Name + ":" + p.GetValue(serive.Token, null) + "<br/>");
            }

            GetCode();
        }

        //执行API
        protected void Button3_Click(object sender, EventArgs e)
        {
            var status = serive.Statuses_User_Timeline(0, 0, 10, 0, "kaifulee", 0, 0);
            
            if (status is string)
            {
                Response.Write(status);
            }
            else
            {
                foreach (var p in status.GetType().GetProperties())
                {
                    Response.Write(p.Name + ":" + p.GetValue(status, null) + "<br/>");
                }
            }
        }

        private void GetCode()
        {
            if (serive.Token != null)
            {
                code.Text = serive.Token.access_token;
            }
            else
            {
                code.Text = "null";
            }
        }
    }
}
