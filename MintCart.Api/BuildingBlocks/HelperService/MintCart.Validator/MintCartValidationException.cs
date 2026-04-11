
using MintCart.Common;
using System;
using System.Collections.Generic;

namespace MintCart.Validator
{
	public class MintCartValidationException : Exception
    {
        public MintCartValidationException()
        {
            Errors = default!;
        }

        public MintCartValidationException(IEnumerable<ValidationError> errors) : base(ValidatorMessage.ValidationException)
        {
            Errors = errors;
        }

        public IEnumerable<ValidationError> Errors { get; private set; }

    }
}
