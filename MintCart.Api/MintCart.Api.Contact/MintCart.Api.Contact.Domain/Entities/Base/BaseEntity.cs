using System.ComponentModel.DataAnnotations;

namespace MintCart.Api.Contact.Domain.Entities.Base
{
    public class BaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
    }
}
