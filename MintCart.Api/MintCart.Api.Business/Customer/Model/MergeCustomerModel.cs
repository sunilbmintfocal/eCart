using System.Collections.Generic;

namespace MintCart.Api.Customer.Business.Model
{
    public class MergeCustomerModel
    {
        /// <summary>
        /// List of customer IDs to merge. The first ID is the primary/master customer
        /// whose record will be updated with the new details.
        /// </summary>
        public List<int> CustomerIds { get; set; } = new();

        /// <summary>
        /// The new customer details to apply to the primary (first) customer.
        /// </summary>
        public CustomerModel CustomerDetails { get; set; } = new();
    }
}
