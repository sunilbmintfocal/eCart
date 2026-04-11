namespace MintCart.Validator
{
	public interface IBaseValidationModel
    {
        /// <summary>
        /// Validate the model dynamicaly using fluent validation
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="modelObject"></param>
        void Validate(object validator, IBaseValidationModel modelObject);
    }
}
