using System;
using System.Collections.Generic;
using System.Text;

namespace MintCart.Permission.Model
{
    public class UserPermissionRoleModel
    {
        public string TenantRoleId { get; set; }
        public string PermissionId { get; set; }
        public string PermissionKey { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsPrivilage { get; set; }
    }
}
