using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories.Interfaces;
using Services;
using Services.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : Controller
    {
        private readonly IAuthService _authService;
        private readonly JwtOptions _jwtOptions;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthenticateController(IAuthService authService, IOptions<JwtOptions> jwtOptions,
            IRefreshTokenService refreshTokenService)
        {
            _authService = authService;
            _refreshTokenService = refreshTokenService;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Post(LoginDto loginDto)
        {
            var userDto = await _authService.LoginAsync(loginDto);
            var result = CreateJwtToken(userDto);

            return Ok(result);
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(string refreshToken, string accessToken)
        {
            var encryptedToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var userId = encryptedToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            var token = await _refreshTokenService.GetRefreshTokenByIdAsync(userId);

            if (refreshToken == token)
            {
                var userDto = await _authService.GetUserInfoAsync(userId);
                var result = CreateJwtToken(userDto);

                return Ok(result);
            }

            return Ok();
        }

        private AuthenticationResultDto CreateJwtToken(LoggedInUserDto userDto) 
        {
            var utcUnixTimeString = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var expUnitTime = DateTimeOffset.UtcNow.AddMinutes(1).ToUnixTimeSeconds();
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Iss, _jwtOptions.Issuer));
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, userDto.Id.ToString()));
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Aud, _jwtOptions.Audience));
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Exp, expUnitTime.ToString()));
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Nbf, utcUnixTimeString));
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Iat, utcUnixTimeString));
            //identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));  
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.UniqueName, userDto.Name));
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, userDto.Email));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, userDto.Roles.First()));


            var signingCredentials = new SigningCredentials(_jwtOptions.SigningCredentials,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(new JwtHeader(signingCredentials),
                new JwtPayload(claimsIdentity.Claims));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthenticationResultDto
            {
                AccessToken = accessToken,
                Expiration = expUnitTime,
                //RefreshToken = refreshToken
            };
        }
    }
}
