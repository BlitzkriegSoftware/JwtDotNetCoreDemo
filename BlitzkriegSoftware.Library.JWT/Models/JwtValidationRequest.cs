// See: https://gist.github.com/bschapendonk/80f2339e0ac6837670d7c6843455d4e2
// See: https://tools.ietf.org/html/rfc7519
// See: https://jwt.io/

namespace BlitzkriegSoftware.Library.JWT.Models
{
    /// <summary>
    /// Request: JWT Validation
    /// </summary>
    public class JwtValidationRequest
    {
        /// <summary>
        /// Token (not base64 encoded)
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Audiance (required)
        /// </summary>
        public string Audiance { get; set; }

        /// <summary>
        /// Issuer (self)
        /// </summary>
        public string Issuer { get; set; } = "self";

        /// <summary>
        /// Is the request valid
        /// </summary>
        public bool IsValid
        {
            get
            {
                return 
                    !string.IsNullOrWhiteSpace(this.Issuer) &&
                    !string.IsNullOrWhiteSpace(this.Audiance) &&
                    !string.IsNullOrWhiteSpace(this.Token);
            }
        }

        /// <summary>
        /// Debug string
        /// </summary>
        /// <returns>Show fields</returns>
        public override string ToString()
        {
            return $"Issuer: {this.Issuer}, Audiance: {this.Audiance}, Token: {this.Token}";
        }

    }
}
