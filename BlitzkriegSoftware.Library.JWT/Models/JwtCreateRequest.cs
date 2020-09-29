using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace BlitzkriegSoftware.Library.JWT.Models
{
    /// <summary>
    /// Request: Create JWT
    /// </summary>
    public class JwtCreateRequest
    {
        /// <summary>
        /// Identity (required)
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
        /// Expires as a timespan (required)
        /// </summary>
        public TimeSpan Expires { get; set; }


        private List<Claim> _claims;

        /// <summary>
        /// List of claims to include (optional)
        /// </summary>
        public List<Claim> Claims { 
            get
            {
                if(_claims == null)
                {
                    _claims = new List<Claim>();
                }
                return _claims;
            }
        }

        /// <summary>
        /// Is the request valid
        /// </summary>
        public bool IsValid
        {
            get
            {
                return (this.Identity != null) &&
                    !string.IsNullOrWhiteSpace(this.Issuer) &&
                    !string.IsNullOrWhiteSpace(this.Audiance) &&
                    (this.Expires.TotalMilliseconds > 0);
            }
        }

    }
}
