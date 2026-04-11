namespace MintCart.Validator
{
	public class ValidationError
    {
        /// <summary>
        /// Gets or sets a key to the current validation error.
        /// </summary>
        public string ValidationKey { get; set; } = string.Empty;

        // <summary>
        /// Gets or sets a message that describes the current validation error.
        /// </summary>
        public string ValidationErrorMessage { get; set; } = string.Empty ;
    }
}
