using System.Collections.Generic;

namespace MintCart.Identity
{
	public interface IUserContext
    {
        string UserIdentity { get; }
        string UserName { get; }
        string Name { get; }
        string UserEmail { get; }
        string UserPrimaryRole { get; }
        IEnumerable<string> UserRoles { get; }
        string TenantId { get; }
        IEnumerable<string> UserRoleNames { get; }
        string TenantName { get; }
        string SubDomainName { get; }
        string UserPhoneNumber { get; }
        string Photo { get; }
        string ClientID { get; }
        string user_station { get; }
        string UserRoleId { get; }
        string User_Id { get; }
        string User_group_role { get; }
    }
}
