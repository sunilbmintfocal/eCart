using System.Text.Json.Serialization;

namespace MintCart.Api.Core.Response
{
    public class ApiException
    {
        /// <summary>
        /// Gets or sets a message that describes the current exception
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// Gets or sets a coded numerical value that is assigned to a specific exception
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Code { get; set; }
        /// <summary>
        /// Gets or sets string representation of the immediate frames on the call stack
        /// </summary>
        public string StackTrace { get; set; } = "";
        /// <summary>
        /// Gets or sets System.Exception instance that caused the current exception
        /// </summary>
        public string InnerExcpetion { get; set; } = "";
    }
}
