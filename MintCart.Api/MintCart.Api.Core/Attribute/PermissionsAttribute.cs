using Microsoft.AspNetCore.Authorization;

namespace MintCart.Api.Core.Attributte
{
    public sealed class PermissionsAttribute : AuthorizeAttribute
    {
        public const string PermissionsGroup = "Permissions";

        private string[] _permissions;

        private bool _isDefault = true;

        public PermissionsAttribute()
        {
            _permissions = Array.Empty<string>();
        }

        public string[] Permissions
        {
            get => _permissions;
            set
            {
                BuildPolicy(ref _permissions, value, PermissionsGroup);
            }
        }

        private void BuildPolicy(ref string[] target, string[] value, string group)
        {
            target = value ?? Array.Empty<string>();

            if (_isDefault)
            {
                Policy = string.Empty;
                _isDefault = false;
            }

            Policy += $"{group}${string.Join("|", target)};";
        }
    }
}
