namespace MintCart.Permission.Model.TenantRolePermissionModel
{
    public class TenantRolePermissionModel
    {
        public string? Id { get; set; }
        public string TenantId { get; set; }
        public string TenantRoleId { get; set; }
        public string ModuleId { get; set; }
        public string PermissionId { get; set; }
    }
}
