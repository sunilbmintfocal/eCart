using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Nodes;

namespace MintCart.Common
{
    /// <summary>
    /// TODO : REVISIT CODE
    /// </summary>
    public static class RestHelper
    {
        #region Check If Http Response is Valid JSON
        public static bool IsValidJson(this string text)
        {
            text = text.Trim();
            if (text.StartsWith("{") && text.EndsWith("}") || //For object
                text.StartsWith("[") && text.EndsWith("]") || //For array
                text.StartsWith("\"") && text.EndsWith("\"")) //For string
            {
                try
                {
                    var obj = JsonNode.Parse(text);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static bool IsNullOrEmptyJson(this string text)
        {
            try
            {
                JsonDocument doc = JsonDocument.Parse(text);
                return doc == null
                    || (doc.RootElement.ValueKind == JsonValueKind.Array && !doc.RootElement.EnumerateArray().Any())
                    || (doc.RootElement.ValueKind == JsonValueKind.Object && !doc.RootElement.EnumerateObject().Any())
                    || (doc.RootElement.ValueKind == JsonValueKind.String && doc.RootElement.GetString() == string.Empty)
                    || doc.RootElement.ValueKind == JsonValueKind.Null;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Parse Http Response
        public static (bool IsEncoded, string ParsedText) VerifyBodyContent(this string text)
        {
            try
            {
                var obj = JsonNode.Parse(text);
                return (true, obj?.ToString()??"");
            }
            catch (Exception)
            {
                return (false, text);
            }
        }
        #endregion

        #region Get JSON setting for Wrapper
        //public static JsonSerializerSettings GetJSONSettings(bool ignoreNull = true, ReferenceLoopHandling referenceLoopHandling = ReferenceLoopHandling.Ignore, bool useCamelCaseNaming = true)
        //{
        //    return new JsonSerializerSettings
        //    {
        //        Formatting = Formatting.Indented,
        //        ContractResolver = useCamelCaseNaming ? new CamelCasePropertyNamesContractResolver() : new DefaultContractResolver(),
        //        Converters = new List<JsonConverter> { new StringEnumConverter() },
        //        NullValueHandling = ignoreNull ? NullValueHandling.Ignore : NullValueHandling.Include,
        //        ReferenceLoopHandling = referenceLoopHandling
        //    };
        //}
        public static string ConvertToJSONString(object data, bool camelCase = true)
        {
            //var camelSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            //return JsonConvert.SerializeObject(data, camelSettings);
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNamingPolicy = camelCase?JsonNamingPolicy.CamelCase:null,
            };

            JsonSerializerOptions optionsCopy = new JsonSerializerOptions(options);
            return JsonSerializer.Serialize(data, optionsCopy);
        }
        #endregion
    }
}
