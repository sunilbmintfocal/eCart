using System.ComponentModel.DataAnnotations;

namespace MintCart.Api.Customer.Domain.Entities.Base
{
    public class BaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
    }
}

