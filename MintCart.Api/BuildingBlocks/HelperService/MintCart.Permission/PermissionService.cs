using MintCart.Permission.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MintCart.Permission
{
	public class PermissionService : IPermissionService
	{

		public PermissionService()
		{

		}

		/// <summary>
		/// Check whether a role have access to a particular module apis
		/// </summary>
		/// <param name="role">Role Identifier</param>
		/// <param name="api">Module Key represent a group of APIs</param>
		/// <returns>If the role have permission return true, else return false</returns>
		public bool CheckRolePermission(IEnumerable<string> roles, IEnumerable<string> permissionKeys, List<UserPermissionRoleModel> userPermissionRoleList)
		{
			bool isValid = false;
			foreach (var key in permissionKeys)
			{
				foreach (var userPermissionRole in userPermissionRoleList)
				{
					if (userPermissionRole.PermissionKey == key)
					{
						if (roles.Contains(userPermissionRole.TenantRoleId))
						{
							isValid = true;
							if (userPermissionRole.StartDate.HasValue && userPermissionRole.EndDate.HasValue)
							{
								DateTime currentDate = DateTime.UtcNow.Date;
								if (!(userPermissionRole.StartDate.Value <= currentDate && userPermissionRole.EndDate.Value >= currentDate))
									return false;
							}
						}
						else
							return false;
					}
				}
			}
			return isValid;
		}
	}
}
