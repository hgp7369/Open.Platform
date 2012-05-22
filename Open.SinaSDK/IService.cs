using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.SinaSDK
{
    public interface IService
    {
        void GetoAuth(string callbackurl);
        void GetAccessToken(string oauth_verifier);
    }
}
