using MintCart.Permission.Model;
using System.Collections.Generic;

namespace MintCart.Permission
{
    public interface IPermissionService
    {
        /// <summary>
        /// Check whether a role have access to a particular module apis
        /// </summary>
        /// <param name="role">Role Identifier</param>
        /// <param name="api">Module Key represent a group of APIs</param>
        /// <returns>If the role have permission return true, else return false</returns>
        bool CheckRolePermission(IEnumerable<string> roles, IEnumerable<string> permissionKeys, List<UserPermissionRoleModel> userPermissionRoleList);
    }
}
