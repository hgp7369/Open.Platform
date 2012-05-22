using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Open.SinaSDK;
//using Open.NeteaseSDK;

namespace Open.Demo
{
    public partial class _Default : System.Web.UI.Page
    {
        //网易微博 调试 http://localhost:42808/Default.aspx?oauth_verifier=授权码&oauth_token=XXXXX
        //NeteaseSerive net = new NeteaseSerive();
        SinaSerive net = new SinaSerive();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["oauth_verifier"] != null)
            {
                net.GetAccessToken(Request["oauth_verifier"]);
                Response.Redirect("/Default.aspx");
            }

            if (Session["oauth_token"] != null)
            {
                Response.Write("连接成功<br />");
            }
        }

        protected void btn2_click(object sender, EventArgs e)
        {
            net.GetoAuth("http://localhost:42808/Default.aspx");
        }

        protected void btn1_click(object sender, EventArgs e)
        {
            var obj = net.StatuesComment(3370502825947042, "!!", null, false, false);
            Response.Write(obj.text);
        }
    }
}
