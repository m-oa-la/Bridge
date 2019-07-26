using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace BridgeMVC.Extensions
{
    public static class ClaimsExtensions
    {
        public static string Email(this ClaimsPrincipal principal)
        {
            var claim = principal.Claims.FirstOrDefault(c => c.Type.EndsWith("emailaddress"));
            return claim == null ? string.Empty : claim.Value;
        }

        public static string Name(this ClaimsPrincipal principal)
        {
            var claim = principal.Claims.FirstOrDefault(c => c.Type == "name");
            return claim == null ? string.Empty : claim.Value;
        }
    }
}