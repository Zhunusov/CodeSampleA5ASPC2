using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationServices.Jwt
{
    public static class AuthJwtOptions
    {
        public static string Issuer { get; private set; }
        public static string Audience { get; private set; }
        public static string Key { get; private set; }
        public static int Lifetime { get; private set; }
        public static SymmetricSecurityKey SymmetricSecurityKey { get; private set; }

        public static void SetAuthOptions(string issuer, string audience, string key, int lifetime)
        {
            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(key) || lifetime < 0)
            {
                throw new Exception("Configuration error");
            }

            Issuer = issuer;
            Audience = audience;
            Key = key;
            Lifetime = lifetime;
            SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
