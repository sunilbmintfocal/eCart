using System;
using System.Collections.Generic;
using System.Text;

namespace MintCart.RestClient.Enum
{
    public enum AuthScheme
    {
        Basic,
        Bearer,
        None
    }
    public enum GrantType
    {
        ClientCredential,
        AuthorizationCode
    }

    public enum HttpVerbs
    {
        Get,
        Post,
        Put,
        Delete,
    }
}
