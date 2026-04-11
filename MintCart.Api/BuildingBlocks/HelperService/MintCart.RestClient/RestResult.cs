using System;
using System.Collections.Generic;
using System.Net;

namespace MintCart.RestClient
{
    public class RestResult<T>
    {
        public T Result { get; set; }
        public List<Error> Errors { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string HttpMethod { get; set; }
        public string Url { get; set; }
        public string RequestContent { get; set; }
        public string ResponseContent { get; set; }
        public string ResponseContentType { get; set; }
        public byte[] ResponseContentAsBytes { get; set; }
        public string ResponseObjectName { get; set; }

        public bool IsSuccess { get; set; }
    }
    public class ErrorResponse
    {
        public List<Error> Errors { get; set; }
    }

    public class Error
    {
        public int Code { get; set; }
        public string Detail { get; set; }
        public ErrorSource Source { get; set; }
    }

    public class ErrorSource
    {
        public string Parameter { get; set; }
    }
}
