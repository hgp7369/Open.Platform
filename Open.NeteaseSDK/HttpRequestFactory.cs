using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.NeteaseSDK
{
    public class HttpRequestFactory
    {
        private HttpRequestFactory() { }

        public static BaseHttpRequest CreateHttpRequest(Method method)
        {
            if (method == Method.GET)
            {
                return new HttpGet();
            }
            else if (method == Method.POST)
            {
                return new HttpPost();
            }
            return new HttpDelete();
        }
    }
}
