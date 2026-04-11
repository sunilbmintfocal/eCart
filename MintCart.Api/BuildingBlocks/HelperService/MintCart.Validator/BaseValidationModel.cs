using FluentValidation;

namespace MintCart.Validator
{
	public class BaseValidationModel<T> : IBaseValidationModel
    {
        public void Validate(object validator, IBaseValidationModel modelObject)
        {
            var instance = (IValidator<T>)validator;
            instance.ValidateAndThrow((T)modelObject);
        }
    }
}
