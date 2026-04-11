using MintCart.Common;
using MintCart.RestClient.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Formatting = Newtonsoft.Json.Formatting;

namespace MintCart.RestClient
{
    public class RestService
    {
        private const string _grantType = "grant_type";
        private string _baseUrl { get; set; }
        private string _apiName { get; set; }
        private AuthScheme _authScheme { get; set; }
        private string _authSchemeValue { get; set; }
        private string _username { get; set; }
        private string _password { get; set; }
        private Dictionary<string, string> _headers { get; set; }
        private string _tokenUrl { get; set; }
        private string _tokenKey { get; set; }


        #region Contructor
        /// <summary>
        /// Initialize the new instance of Rest client with API base url
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="headers"></param>
        /// <param name="logRestResultToDB"></param>
        public RestService(string baseUrl,Dictionary<string, string> headers = null)
        {

            _baseUrl = baseUrl;
            _authScheme = AuthScheme.None;
            _authSchemeValue = string.Empty;
            _headers = headers;
        }

        /// <summary>
        /// Initialize the new instance of Rest client with API base Url, Authentication scheme and Authentication scheme value . This is usefull for Basic and Bearer authentication
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="authScheme"></param>
        /// <param name="authSchemeValue"></param>
        public RestService(string baseUrl, AuthScheme authScheme, string authSchemeValue)
        {
            _baseUrl = baseUrl;
            _authScheme = authScheme;
            _authSchemeValue = authSchemeValue;
        }

        /// <summary>
        ///  Initialize the new instance of Rest client with API base Url and Credential for Authentication
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="authScheme"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public RestService(string baseUrl, AuthScheme authScheme, string userName, string password)
        {
            if (authScheme != AuthScheme.Basic)
                throw new Exception($"Invalid Authentication scheme specified : {authScheme}");

            _baseUrl = baseUrl;
            _authScheme = authScheme;
            _username = userName;
            _password = password;
        }

        /// <summary>
        ///  Initialize the new instance of Rest client with API base Url , Credential for Authentication  and token url and key for bearer authentication 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="authScheme"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="tokenUrl"></param>
        /// <param name="tokenKey"></param>
        public RestService(string baseUrl, AuthScheme authScheme, string userName, string password, string tokenUrl, string tokenKey)
        {
            if (authScheme != AuthScheme.Bearer)
                throw new Exception($"Invalid Authentication scheme specified : {authScheme}");

            _baseUrl = baseUrl;
            _authScheme = authScheme;
            _username = userName;
            _password = password;
            _tokenUrl = tokenUrl;
            _tokenKey = tokenKey;
        }
        #endregion

        #region public method
        /// <summary>
        /// Send a GET request to the specified Url
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="headerValueCollection"></param>
        /// <returns></returns>
        public async Task<RestResult<TResult>> GetAsync<TResult>(string url, object data = null, Dictionary<string, string> headerValueCollection = null)
        {
            string queryString = data.GetQueryStringfromObject();
            if (!string.IsNullOrEmpty(queryString))
                url = $"{url}{queryString}";
            return await ExecuteService<object, TResult>(HttpVerbs.Get, url, null, headerValueCollection);
        }

        /// <summary>
        ///  Send a POST request to the specified Uri
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="headerValueCollection"></param>
        /// <returns></returns>
        public async Task<RestResult<TResult>> PostAsync<TInput, TResult>(string url, TInput postData, Dictionary<string, string> headerValueCollection = null, bool useSendAsync = false)
        {
            return await ExecuteService<TInput, TResult>(HttpVerbs.Post, url, postData, headerValueCollection, useSendAsync);
        }
        /// <summary>
        ///  Send a PUT request to the specified Uri
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="putData"></param>
        /// <param name="headerValueCollection"></param>
        /// <returns></returns>
        public async Task<RestResult<TResult>> PutAsync<TInput, TResult>(string url, TInput putData, Dictionary<string, string> headerValueCollection = null)
        {
            return await ExecuteService<TInput, TResult>(HttpVerbs.Put, url, putData, headerValueCollection);
        }
        /// <summary>
        ///  Send a DELETE request to the specified Uri
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="headerValueCollection"></param>
        /// <returns></returns>
        public async Task<RestResult<TResult>> DeleteAsync<TResult>(string url, Dictionary<string, string> headerValueCollection = null)
        {
            return await ExecuteService<object, TResult>(HttpVerbs.Delete, url, null, headerValueCollection);
        }
        /// <summary>
        /// Add Basic authentication scheme for the current request 
        /// </summary>
        /// <param name="authSchemeValue"></param>
        public void AddBasicAuthentication(string authSchemeValue)
        {
            _authScheme = AuthScheme.Basic;
            _authSchemeValue = authSchemeValue;
        }
        /// <summary>
        ///  Add Basic authentication scheme for the current request 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        public void AddBasicAuthentication(string userName, string passWord)
        {
            _authScheme = AuthScheme.Basic;
            _authSchemeValue = GetCredetialBase64String(userName, passWord);
        }
        /// <summary>
        ///  Add Bearer authentication scheme for the current request 
        /// </summary>
        /// <param name="authSchemeValue"></param>
        public void AddBearerAuthentication(string authSchemeValue)
        {
            _authScheme = AuthScheme.Bearer;
            _authSchemeValue = authSchemeValue;
        }
        /// <summary>
        ///  Add Bearer authentication scheme for the current request 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <param name="tokenUrl"></param>
        /// <param name="grantType"></param>
        /// <param name="tokenKey"></param>
        public void AddBearerAuthentication(string userName, string passWord, string tokenUrl, GrantType grantType, string tokenKey)
        {
            string grantTypeValue = string.Empty;
            AddBasicAuthentication(userName, passWord);
            var data = new Dictionary<string, string>();
            if (grantType == GrantType.ClientCredential)
                grantTypeValue = "client_credentials";
            else
                grantTypeValue = "code";

            data.Add(_grantType, grantTypeValue);

            var tokendata = PostAsync<Dictionary<string, string>, Dictionary<string, string>>(tokenUrl, data).Result.Result;
            string token = string.Empty;

            if (tokendata != null && tokendata.ContainsKey(tokenKey))
                token = tokendata[tokenKey];
            else
                throw new Exception("Invalid Token");

            _password = string.Empty;
            _username = string.Empty;
            _tokenUrl = string.Empty;
            _authSchemeValue = string.Empty;

            AddBearerAuthentication(token);
        }

        /// <summary>
        /// Add Bearer authentication scheme for the current request 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tokenUrl"></param>
        /// <param name="headerValueCollection"></param>
        /// <returns></returns>
        public async Task<T> AddBearerAuthentication<T>(string tokenUrl, Dictionary<string, string> headerValueCollection = null)
        {
            var authResult = await PostAsync<Dictionary<string, string>, T>(tokenUrl, null, headerValueCollection);

            var tokenData = authResult.Result;
            _password = string.Empty;
            _username = string.Empty;
            _tokenUrl = string.Empty;
            _authSchemeValue = string.Empty;

            return tokenData;
        }
        #endregion

        #region Private methods
        private async Task<RestResult<TResult>> ExecuteService<TInput, TResult>(HttpVerbs method, string url, TInput data, Dictionary<string, string> headerValueCollection = null, bool useSendAsync = false)
        {
            HttpResponseMessage response = null;


            RestResult<TResult> result = new RestResult<TResult>();

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(_baseUrl);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    AddHeaderElements(client, headerValueCollection);

                    AddAuthorizationheader(client);

                    result.HttpMethod = method.ToString();
                    result.Url = $"{_baseUrl}/{url}";
                    result.RequestContent = JsonConvert.SerializeObject(data);
                    result.StartTime = DateTime.Now;

                    if (useSendAsync)
                    {

                        var json = data == null ?
                     string.Empty :
                     JsonConvert.SerializeObject(data, Formatting.Indented,
                         new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        string atradiusUrl = $"{_baseUrl}/{url}";
                        var request = new HttpRequestMessage(HttpMethod.Post, atradiusUrl) { Content = content };
                        request.Headers.TryAddWithoutValidation("authorization", "Bearer " + _authSchemeValue);
                        request.Headers.TryAddWithoutValidation("atradius-app-key", _headers.FirstOrDefault().Value);
                        response = await client.SendAsync(request);
                    }
                    else
                    {
                        switch (method)
                        {
                            case HttpVerbs.Get:
                                response = await client.GetAsync(url);
                                break;
                            case HttpVerbs.Post:
                                response = await client.PostAsJsonAsync<TInput>(url, data);
                                break;
                            case HttpVerbs.Put:
                                response = await client.PutAsJsonAsync<TInput>(url, data);
                                break;
                            case HttpVerbs.Delete:
                                response = await client.DeleteAsync(url);
                                break;
                        }
                    }

                    var responseContent = await response.Content.ReadAsStringAsync();

                    var apiResponse = JsonConvert.DeserializeObject<TResult>(responseContent);

                    result.Result = apiResponse;
                    result.StatusCode = response.StatusCode;
                    result.IsSuccess = (int)response.StatusCode >= 200 && (int)response.StatusCode <= 299;
                    result.ResponseContent = responseContent;
                    result.ResponseContentType = response.Content.Headers.ContentType.MediaType;

                    result.EndTime = DateTime.Now;
                }
                catch (Exception ex)
                {
                    result.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    result.IsSuccess = false;
                    result.HttpMethod = method.ToString();
                    result.Url = $"{_baseUrl}/{url}";
                    result.ResponseContent = string.Empty;
                    result.ResponseContentType = string.Empty;
                    result.EndTime = DateTime.Now;
                    result.Errors = GetErrors(ex);
                }

            }

            return result;
        }

        private void AddHeaderElements(HttpClient client, Dictionary<string, string> headerValueCollection)
        {
            if (_headers != null)
            {
                if (headerValueCollection == null)
                    headerValueCollection = _headers;
                else
                {
                    foreach (var item in _headers)
                    {
                        if (!headerValueCollection.ContainsKey(item.Key))
                            headerValueCollection.Add(item.Key, item.Value);
                    }
                }

            }
            if (headerValueCollection != null)
            {
                foreach (var item in headerValueCollection)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                }
            }

        }

        private void AddAuthorizationheader(HttpClient client)
        {
            if ((_authScheme == AuthScheme.Basic || _authScheme == AuthScheme.Bearer) && !string.IsNullOrEmpty(_authSchemeValue))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_authScheme.ToString(), _authSchemeValue);
            else if (_authScheme == AuthScheme.Basic && !string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
            {
                _authSchemeValue = GetCredetialBase64String(_username, _password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_authScheme.ToString(), _authSchemeValue);
            }
        }

        private static string GetCredetialBase64String(string username, string password)
        {
            string plainText = $"{username}:{password}";
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private List<Error> GetErrors(Exception ex)
        {
            var errors = new List<Error>();
            errors.Add(new Error()
            {
                Detail = $"{ex.Message} {ex.StackTrace}",
            });
            return errors;
        }

        #endregion
    }
}

