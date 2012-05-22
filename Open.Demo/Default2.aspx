<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default2.aspx.cs" Inherits="Open.Demo.Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        返回值access_token:<asp:Literal ID="code" runat="server"></asp:Literal><br />
        <asp:Button ID="Button1" runat="server" Text="请求提交" onclick="Button1_Click" /> <asp:Button ID="Button2" runat="server" Text="换取access_token" onclick="Button2_Click" /> <asp:Button ID="Button3" runat="server" Text="执行api" onclick="Button3_Click" />
        <asp:FileUpload ID="upload" runat="server" />
    </div>
    </form>
</body>
</html>
