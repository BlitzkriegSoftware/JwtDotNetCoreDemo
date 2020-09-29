using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using System.Text;

namespace BlitzkriegSoftware.Library.JWT.Models
{
    /// <summary>
    /// JWT Indentity
    /// </summary>
    public class JwtIdentity : IIdentity
    {
        [ExcludeFromCodeCoverage]
        private JwtIdentity() { }
     
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="name">Username</param>
        public JwtIdentity(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Hardcoded to <c>JWT</c>
        /// </summary>
        public string AuthenticationType { get; } = "JWT";
        
        /// <summary>
        /// Hardcoded to <c>true</c>
        /// </summary>
        public bool IsAuthenticated { get; } = true;
        
        /// <summary>
        /// Username
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Debug string
        /// </summary>
        /// <returns>Show fields</returns>
        public override string ToString()
        {
            return $"Typ: {this.AuthenticationType}; IsAuth: {this.IsAuthenticated}; Name: {this.Name}";
        }
    }
}
