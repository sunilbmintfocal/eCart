using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MintCart.Common
{
    public static class JsonHelper
    {
        public static string ConvertJsonToString(string directory)
        {
            using (StreamReader r = new StreamReader(directory))
            {
                string json = r.ReadToEnd();
                return json;
            }
        }

        public static bool TryGetJsonProperty(JsonElement element, string propertyName, List<JsonValueKind> expectedKind, out JsonElement property)
        {
            property = default!;
            if (element.TryGetProperty(propertyName, out property) && expectedKind.Contains(property.ValueKind))
            {
                return true;
            }
            property = default!;
            return false;
        }

        public static bool TryGetArrayElements(JsonElement element, string arrayProperty, out IEnumerable<JsonElement> arrayElements)
        {
            arrayElements = default!;
            if (TryGetJsonProperty(element, arrayProperty, new List<JsonValueKind>() { JsonValueKind.Array }, out JsonElement arrayElement))
            {
                arrayElements = arrayElement.EnumerateArray();
                return true;
            }
            return false;
        }

        public static JsonElement FindFirstNonEmptyPropertyInArray(JsonElement root, string arrayProperty, string itemTypeProperty)
        {
            List<JsonValueKind> supportedKinds = new List<JsonValueKind>() { JsonValueKind.String, JsonValueKind.True, JsonValueKind.False };

            if (TryGetArrayElements(root, arrayProperty, out IEnumerable<JsonElement> elements))
            {
                foreach (JsonElement element in elements)
                {
                    if (TryGetJsonProperty(element, itemTypeProperty, supportedKinds, out JsonElement propertyElement))
                    {
                        return propertyElement;
                    }
                }
            }
            return default!;
        }

        public static string ParseJsonAndFindInArray(string jsonData, string idProperty, string arrayProperty, string itemTypeProperty)
        {
            using (JsonDocument document = JsonDocument.Parse(jsonData))
            {
                JsonElement root = document.RootElement;

                if (TryGetJsonProperty(root, idProperty, new List<JsonValueKind>() { JsonValueKind.String }, out JsonElement idElement))
                {
                    return FindFirstNonEmptyPropertyInArray(root, arrayProperty, itemTypeProperty).ToString() ?? default!;
                }
            }

            return default!;
        }

        public static T? Deserialize<T>(string jsonData) where T : class
        {
            var model = JsonSerializer.Deserialize<T>(jsonData);
            return model ?? default!;
        }

        public static string SerializeToJson<T>(T data)
        {
            return JsonSerializer.Serialize(data);
        }

        public static (List<string> jsonStrings, List<Exception> errors) CheckAndSplitConcatenatedJson(string jsonData)
        {
            List<string> jsonStrings = new List<string>();
            List<Exception> errors = new List<Exception>();

            if (jsonData.Contains("}{"))
            {
                var data = jsonData.Split(new[] { "}{" }, StringSplitOptions.None);
                for (int i = 0; i < data.Length; i++)
                {
                    string json = data[i].Trim();
                    if (i > 0)
                    {
                        json = "{" + json;
                    }
                    if (i < data.Length - 1)
                    {
                        json += "}";
                    }

                    var (result, exception) = ValidateJsonDocument(json);
                    if (result)
                    {
                        jsonStrings.Add(json);
                    }
                    else
                    {
                        errors.Add(exception);
                    }
                }
            }
            else
            {
                var (result, exception) = ValidateJsonDocument(jsonData);
                if (result)
                {
                    jsonStrings.Add(jsonData);
                }
                else
                {
                    errors.Add(exception);
                }
            }

            return (jsonStrings, errors);
        }

        public static (bool result, Exception exception) ValidateJsonDocument(string json)
        {
            try
            {
                using (JsonDocument.Parse(json))
                {
                    return (true, default!);
                }
            }
            catch (Exception ex)
            {
                return (false, ex);
            }
        }

        public static JsonNode ConvertToJsonNode<T>(T data)
        {
            try
            {
                string jsonString = JsonHelper.SerializeToJson(data);
                return JsonNode.Parse(jsonString) ?? default!;
            }
            catch (Exception)
            {
                return default!;
            }

        }
    }
}
