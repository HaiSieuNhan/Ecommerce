using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Ecommerce.Shared
{
    public static class Helper
    {
        public readonly static DateTime unixBeginDateTime = new DateTime(1970, 1, 1, 0, 0, 0);
        public static long ToUnixTimestamp(this DateTime d)
        {
            var epoch = d - unixBeginDateTime;

            return (long)epoch.TotalSeconds;
        }

        public static string GenerateToken(string salt = "")
        {
            using (var sha = SHA512.Create())
            {
                return System.Convert.ToBase64String(sha.ComputeHash(UTF8Encoding.UTF8.GetBytes($"{salt}.{Guid.NewGuid()}")));
            }
        }

        public static string HashPassword(string src)
        {
            using (var sha = SHA512.Create())
            {
                return Convert.ToBase64String(sha.ComputeHash(UTF8Encoding.UTF8.GetBytes(src)));
            }
        }

        public static string GetCurrentTokenFromRequest(HttpContext context)
        {
            var token = string.Empty;

            if (context == null) return token;

            if (context.Request.Headers.ContainsKey("Authorization")) token = context.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(token) && token.StartsWith(JwtBearerDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
            {
                token = token.Substring(6).Trim();
            }

            return token;
        }

    }
}
