using System;

namespace MintCart.Api.Customer.Domain.Entities.Base
{
    public class AuditEntity
    {
        private DateTime _createdDate;
        public DateTime CreatedDate
        {
            get => _createdDate;
            set => _createdDate = value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        private DateTime _modifiedDate;
        public DateTime ModifiedDate
        {
            get => _modifiedDate;
            set => _modifiedDate = value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}

