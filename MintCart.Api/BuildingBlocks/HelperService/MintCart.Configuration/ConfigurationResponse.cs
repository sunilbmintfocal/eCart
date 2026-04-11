using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MintCart.Configuration
{
    public class ConfigurationResponse<T>
    {
        public ConfigurationResponse(T response, bool isConfigExists)
        {
            Response = response;
            IsConfigExists = isConfigExists;
        }

        public T Response { get; set; }
        public bool IsConfigExists { get; set; }
    }
}
