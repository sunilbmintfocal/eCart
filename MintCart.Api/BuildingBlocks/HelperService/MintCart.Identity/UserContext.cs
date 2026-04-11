using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MintCart.Identity
{
	public class UserContext : IUserContext
    {

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<UserContext> _logger;
        private readonly EventUserContext _eventUserContext;

        #region ctor
        public UserContext(IHttpContextAccessor contextAccessor, ILogger<UserContext> logger, 
            EventUserContext eventUserContext)
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _logger = logger;
            _eventUserContext = eventUserContext;
        }
        #endregion

        #region Public props
        /// <summary>
        ///   Returns the Authorized unique id of the user from the Identity claim
        /// </summary>
        public string UserIdentity => GetUserIdentity();

        /// <summary>
        ///  Returns the Authorized user name from the Identity claim
        /// </summary>
        public string UserName => GetUserName();

        ///  Returns the Authorized name from the Identity claim
        /// </summary>
        public string Name => GetName();

        /// <summary>
        /// Returns the Authorized user email from the Identity claim
        /// </summary>
        public string UserEmail => GetUseEmail();

        /// <summary>
        /// Returns the Authorized user primary role from the Identity claim
        /// </summary>
        public string UserPrimaryRole => GetPrimaryRole();

        /// <summary>
        /// Returns the Authorized user roles from the Identity claim
        /// </summary>
        public IEnumerable<string> UserRoles => GetUserRoles();

        public IEnumerable<string> UserRoleNames => GetUserRoleNames();

        /// <summary>
        /// Returns the Authorized user tenantId from the Identity claim
        /// </summary>
        public string TenantId => GetTenantId();

        public string TenantName => GetTenantName();

        public string SubDomainName => GetsubDomainName();

        public string UserPhoneNumber => GetUserPhoneNumber();

        /// <summary>
        /// Returns the Authorized user profile pic URL from the Identity claim
        /// </summary>
        public string Photo => GetPhotoURL();

        /// <summary>
        /// Returns the Authorized user client id from the Identity claim
        /// </summary>
        public string ClientID => GetClientID();
        public string user_station => GetUserRoleStations();  // GetUserStations();

        public string UserRoleId => GetUserRoleId();

        public string User_Id => GetUserId();

        public string User_group_role => GetUser_group_role();

        #endregion

        #region Private Methods
        private IEnumerable<Claim> GetClaims()
        {
            var identity = (ClaimsIdentity)_contextAccessor.HttpContext?.User?.Identity;
            return identity?.Claims;
        }

        private string GetUserIdentity()
        {
            var claims = GetClaims();
            return claims.Where(c => c.Type == JwtClaimTypes.Id)
                .Select(c => c.Value).FirstOrDefault();
        }

        private string GetUserName()
        {
            var claims = GetClaims();
            return claims.Where(c => c.Type == JwtClaimTypes.PreferredUserName)
                .Select(c => c.Value).FirstOrDefault();
        }

        private string GetName()
        {
            var claims = GetClaims();
            return claims.Where(c => c.Type == JwtClaimTypes.Name)
                .Select(c => c.Value).FirstOrDefault();
        }

        private string GetUseEmail()
        {
            var UserEmail = string.Empty;
            if (_eventUserContext != null && !string.IsNullOrEmpty(_eventUserContext.UserEmailId))
            {
                UserEmail = _eventUserContext.UserEmailId;
                return UserEmail;
            }
            var claims = GetClaims();
            return claims?.Where(c => c.Type == ClaimTypes.Email)
                .Select(c => c.Value).FirstOrDefault();
            //return claims?.Where(c => c.Type == ClaimTypes.Email)
            //    .Select(c => c.Value).FirstOrDefault() ?? string.Empty;
        }

        private IEnumerable<string> GetUserRoles()
        {
            var claims = GetClaims();
            var roles = claims
                   .Where(c => c.Type == "RoleID")
                   .Select(c => c.Value);
            return roles;
        }

        private IEnumerable<string> GetUserRoleNames()
        {
            var claims = GetClaims();
            var roles = claims
                   .Where(c => c.Type == ClaimTypes.Role)
                   .Select(c => c.Value);
            return roles;
        }

        private string GetPrimaryRole()
        {
            return GetUserRoles().FirstOrDefault();
        }

        private string GetTenantId()
        {
            var tenantID=string.Empty;
            if (_eventUserContext != null && !string.IsNullOrEmpty(_eventUserContext.TenantID))
            {
                tenantID = _eventUserContext.TenantID;
                return tenantID;
            }
            var claims = GetClaims();
            return claims?.FirstOrDefault(c => c.Type == "TenantID")?.Value;
        }
     

        private string GetClientID()
        {
            var clientID = string.Empty;
            if (_eventUserContext != null && !string.IsNullOrEmpty(_eventUserContext.ClientID))
            {
                clientID = _eventUserContext.ClientID;
                return clientID;
            }
            var claims = GetClaims();
            return claims?.FirstOrDefault(c => c.Type == "client_id")?.Value;
        }

        //All the stations assigned irespective of role
        private string GetUserStations()
        {
            var Stations = string.Empty;
            if (_eventUserContext != null && !string.IsNullOrEmpty(_eventUserContext.user_station))
            {
                Stations = _eventUserContext.user_station;
                return Stations;
            }

            var claims = GetClaims();
            Stations = claims?.FirstOrDefault(c => c.Type == "user_station")?.Value;
            return !string.IsNullOrEmpty(Stations)? Stations : string.Empty;
        }

        //Get Station ID based on selected role
        private string GetUserRoleStations()
        {
            var roleStationsStr = string.Empty;
            var Stations = string.Empty;
            if (_eventUserContext != null && !string.IsNullOrEmpty(_eventUserContext.user_station))
            {
                roleStationsStr = _eventUserContext.user_station;
                return roleStationsStr;
            }
            var claims = GetClaims();
            roleStationsStr = claims?.FirstOrDefault(c => c.Type == "user_station_role")?.Value?? "";

            if (!string.IsNullOrEmpty(roleStationsStr))
            {
                var roleStatios = JsonConvert.DeserializeObject<List<UserStationClaim>>(roleStationsStr);
                if (roleStatios != null && roleStatios.Any())
                {
                    Stations = string.Join(",", roleStatios.Where(x => x.RoleId == GetUserRoleId()).Select(x => x.StationId).ToList());
                }
            }
            return !string.IsNullOrEmpty(Stations) ? Stations : string.Empty;
        }

        private string GetsubDomainName()
        {
            var claims = GetClaims();
            _logger.LogDebug($"Claims - {claims} : ");
            var subDomainName = claims?.Where(c => c.Type == "subDomain").Select(c => c.Value);
            _logger.LogDebug($"SubDomain from User context - {subDomainName} : ");
            _logger.LogDebug($"SubDomain firstorDef from User context - {subDomainName.FirstOrDefault()} : ");
            return subDomainName?.FirstOrDefault();
        }

        private string GetTenantName()
        {
            var claims = GetClaims();
            var tenantName = claims?.FirstOrDefault(c => c.Type == "Tenant")?.Value;
            _logger.LogDebug($"Tenant - {tenantName} : ");
            return tenantName;
        }

        private string GetUserPhoneNumber()
        {
            var claims = GetClaims();
            return claims.Where(c => c.Type == JwtClaimTypes.PhoneNumber)
                .Select(c => c.Value).FirstOrDefault();
        }

        private string GetPhotoURL()
        {
            var claims = GetClaims();
            return claims.Where(c => c.Type == JwtClaimTypes.Picture)
                .Select(c => c.Value).FirstOrDefault();
        }

        private string GetUserRoleId()
        {
            if (_contextAccessor.HttpContext.Request.Headers.TryGetValue("RoleId"
                , out var roleHeader))
            {
                return roleHeader.ToString();
            }
            return null;
        }

        private string GetUserId()
        {
            var userId = string.Empty;
            if (_eventUserContext != null && !string.IsNullOrEmpty(_eventUserContext.User_Id))
            {
                userId = _eventUserContext.User_Id;
                return userId;
            }
            var claims = GetClaims();
            return claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).FirstOrDefault();
        }

        private string GetUser_group_role()
        {
            var userId = string.Empty;
            if (_eventUserContext != null && !string.IsNullOrEmpty(_eventUserContext.User_group_role))
            {
                userId = _eventUserContext.User_group_role;
                return userId;
            }
            var claims = GetClaims();
            return claims.Where(c => c.Type == "user_group_role")
                .Select(c => c.Value).FirstOrDefault();
        }
        #endregion
    }
    #region "private calass"
    class UserStationClaim
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string StationId { get; set; }
    }
    #endregion
}
