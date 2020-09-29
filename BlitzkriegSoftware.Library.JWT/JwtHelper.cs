// See: https://gist.github.com/bschapendonk/80f2339e0ac6837670d7c6843455d4e2
// See: https://tools.ietf.org/html/rfc7519
// See: https://jwt.io/

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using BlitzkriegSoftware.Library.JWT.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics.CodeAnalysis;

namespace BlitzkriegSoftware.Library.JWT
{
    /// <summary>
    /// JWT Helper
    /// </summary>
    public class JwtHelper
    {
        [ExcludeFromCodeCoverage]
        private JwtHelper() { }

        private readonly byte[] _secret;
        private readonly JwtSecurityTokenHandler handler;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="secretBytes">A byte array to use for a secret (do not lose!)
        /// <para>The helper method <c>MakeRandomSecret</c> can be used to create a secret</para>
        /// </param>
        public JwtHelper(byte[] secretBytes)
        {
            this._secret = secretBytes;
            this.handler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        /// Create a JWT token of the form <c>Header.Payload.Signature</c>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>JWT</returns>
        public string Create(JwtCreateRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if ((this._secret == null) || (this._secret.Length <= 0)) throw new InvalidOperationException("A Secret must be provided in the CTOR and be an array of bytes whose length is a power of two");
            if (!request.IsValid) throw new InvalidOperationException("A request must have an issuer, Audiance, Identity, & Expiration");

            var signingKey = new SymmetricSecurityKey(this._secret);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(request.Identity, request.Claims),
                NotBefore = DateTime.Now,
                SigningCredentials = signingCredentials,
                Issuer = request.Issuer,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow + request.Expires,
                Audience = request.Audiance
            };

            SecurityToken plainToken = this.handler.CreateToken(securityTokenDescriptor);
            var signedAndEncodedToken = this.handler.WriteToken(plainToken);

            return signedAndEncodedToken;
        }

        /// <summary>
        /// Validate and Get Infomation from a Token
        /// </summary>
        /// <param name="request">JwtValidationRequest</param>
        /// <returns>JwtInformation</returns>
        public JwtInformation Validate(JwtValidationRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (!request.IsValid) throw new InvalidOperationException("A request must have a Token, Audiance, and Issuer");

            var response = new JwtInformation();

            var signingKey = new SymmetricSecurityKey(this._secret);

            var param = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.FromMinutes(1),
                ValidIssuer = request.Issuer,
                ValidAudience = request.Audiance,
                IssuerSigningKey = signingKey
            };

            var prin = handler.ValidateToken(request.Token, param, out SecurityToken validatedToken);

            if (validatedToken == null)
            {
                response.Identity = null;
            }
            else
            {
                response.Claims.AddRange(prin.Claims);
                response.Identity = prin.Identity;
            }

            return response;
        }

        /// <summary>
        /// Make a random base64 encoded N byte secret
        /// <paramref name="buffersize">Must be a power of 2 that is supported (default 256)</paramref>
        /// </summary>
        /// <returns></returns>
        public static byte[] MakeRandomSecret(int buffersize = 256)
        {
            if (buffersize <= 0) throw new ArgumentOutOfRangeException(nameof(buffersize), "Must be a positive (not) power of two.");
            if(buffersize % 2 != 0) throw new ArgumentOutOfRangeException(nameof(buffersize), "Must be a positive power of two (not).");

            var buffer = new byte[buffersize];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(buffer);
            }
            return buffer;
        }

        /// <summary>
        /// Convert to Base64
        /// </summary>
        /// <param name="plainText">Pain Text (like a JWT)</param>
        /// <returns>Base64 String</returns>
        public static string ToBase64(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) throw new ArgumentNullException(nameof(plainText));
            var b = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(b);
        }

        /// <summary>
        /// Convert From Base64
        /// </summary>
        /// <param name="base64Text">Base64 Text</param>
        /// <returns>Original Text</returns>
        public static string FromBase64(string base64Text)
        {
            if (string.IsNullOrEmpty(base64Text)) throw new ArgumentNullException(nameof(base64Text));
            var b = Convert.FromBase64String(base64Text);
            return System.Text.Encoding.UTF8.GetString(b);
        }


    }
}
