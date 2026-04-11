using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MintCart.Common
{
    public static class HttpExtensions
    {
        public static string GetQueryStringfromObject(this Object data)
        {
            string queryString = string.Empty;
            if (data != null)
            {
                var properties = from p in data.GetType().GetProperties()
                                 where p.GetValue(data, null) != null
                                 select p.Name + "=" + WebUtility.UrlEncode(p.GetValue(data, null)?.ToString()??"");

                string value = String.Join("&", properties.ToArray());
                queryString = $"{queryString}?{value}";
            }
            return queryString;
        }
    }
}
