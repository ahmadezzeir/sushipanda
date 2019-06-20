﻿using Microsoft.IdentityModel.Tokens;

namespace API
{
    public class JwtOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public SymmetricSecurityKey SigningCredentials { get; set; } 
    }
}
