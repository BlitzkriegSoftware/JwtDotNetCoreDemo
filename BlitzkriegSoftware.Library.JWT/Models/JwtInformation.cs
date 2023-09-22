using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace BlitzkriegSoftware.Library.JWT.Models
{

    /// <summary>
    /// JWT Information
    /// </summary>
    public class JwtInformation
    {

        /// <summary>
        /// Default: Expiration Minutes
        /// </summary>
        public const int DefaultJwtExpirationMinutes = 15;

        /// <summary>
        /// IIdentity
        /// </summary>
        public IIdentity Identity { get; set; }

        /// <summary>
        /// Audiance (required)
        /// </summary>
        public string Audiance { get; set; }

        /// <summary>
        /// Issuer (self)
        /// </summary>
        public string Issuer { get; set; } = "self";

        /// <summary>
        /// Is a valid token
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Identity?.Name);
            }
        }

        private List<Claim> _claims;

        /// <summary>
        /// List of claims to include (optional)
        /// </summary>
        public List<Claim> Claims
        {
            get
            {
                _claims ??= new List<Claim>();
                return _claims;
            }
        }

        /// <summary>
        /// Expires At (UTC), Default <c>DefaultJwtExpirationMinutes</c>
        /// </summary>
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(DefaultJwtExpirationMinutes);

        /// <summary>
        /// Debug string
        /// </summary>
        /// <returns>Show fields</returns>
        public override string ToString()
        {
            return $"Id: {this.Identity}; Aud: {this.Audiance}; Iss: {this.Issuer}; Expires: {this.ExpiresAt}; Claims: {string.Join(",", this.Claims)}";
        }

    }
}
