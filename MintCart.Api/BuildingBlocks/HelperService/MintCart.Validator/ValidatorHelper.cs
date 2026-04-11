using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MintCart.Validator
{
	public class ValidatorHelper
    {
        private readonly IServiceProvider _serviceProvider;
        private List<ValidationError> _validationErrors;

        public ValidatorHelper(IServiceProvider serviceProvider, List<ValidationError> validationErrors)
        {
            _serviceProvider = serviceProvider;
            _validationErrors = validationErrors;
        }
        /// <summary>
        /// Validate model 
        /// </summary>
        /// <param name="model"></param>
        public void Validate(IBaseValidationModel model)
        {
            var modelType = model.GetType();
            var genericType = typeof(IValidator<>).MakeGenericType(modelType);
            var validator = _serviceProvider.GetService(genericType);

            if (validator != null)
                model.Validate(validator, model);
        }
        /// <summary>
        /// Validate generic data using the valiation class defined
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public ValidationResult Validate<T>(IValidator<T> instance, T data)
        {
            return instance.Validate(data);
        }
        /// <summary>
        /// Validate generic data using the valiation class defined
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="data"></param>
        public void ValidateAndThrow<T>(IValidator<T> instance, T data)
        {
            instance.ValidateAndThrow(data);
        }
        /// <summary>
        /// throw the validation error collection added to the validation error
        /// </summary>
        /// <exception cref="MintCartValidationException"></exception>
        public void ValidateAndThrow()
        {
            if (_validationErrors.Any())
            {
                // Temporarily hold onto the errors
                var errors = new List<ValidationError>(_validationErrors);

                // in the calling method, then it's cleared only if handling is performed.
                _validationErrors.Clear();

                throw new MintCartValidationException(errors);
            }
        }
        /// <summary>
        /// Add custome validation error to the colletion
        /// </summary>
        /// <param name="key"></param>
        /// <param name="validation"></param>
        public void AddValidationError(string key, string validation)
        {
            _validationErrors.Add(new ValidationError() { ValidationKey = key, ValidationErrorMessage = validation });
        }
    }
}
