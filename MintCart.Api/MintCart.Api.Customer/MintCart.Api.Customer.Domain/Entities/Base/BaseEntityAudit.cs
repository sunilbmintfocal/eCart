using System.ComponentModel.DataAnnotations;

namespace MintCart.Api.Customer.Domain.Entities.Base
{
    public class BaseEntityAudit<T> : AuditEntity
    {
        [Key]
        public virtual T Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}

