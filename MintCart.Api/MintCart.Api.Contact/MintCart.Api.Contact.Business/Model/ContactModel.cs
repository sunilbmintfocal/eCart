using System;

namespace MintCart.Api.Contact.Business.Model
{
    public class ContactModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Mobile { get; set; }
    }
}
