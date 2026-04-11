namespace MintCart.Api.Core.Response
{
    public class ApiErrors
    {
        /// <summary>
        /// Gets or sets a key to the current validation error.
        /// </summary>
        public string ValidationKey { get; set; } = "";

        // <summary>
        /// Gets or sets a message that describes the current validation error.
        /// </summary>
        public string ValidationErrorMessage { get; set; } = "";
    }
}
