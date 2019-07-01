using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Domain.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Plus.v1;
using Google.Apis.Requests.Parameters;
using Google.Apis.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Dtos;
using Services.Interfaces;
using SystemClock = Google.Apis.Util.SystemClock;

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
            var result = await CreateJwtTokenAsync(userDto);

            return Ok(result);
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(string refreshToken, string accessToken)
        {
            var encryptedToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var userId = encryptedToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            var token = await _refreshTokenService.GetRefreshTokenByIdAsync(userId);

            if (refreshToken == token.Value)
            {
                var userDto = await _authService.GetUserInfoAsync(userId);

                if (userDto != null)
                {
                    var result = await CreateJwtTokenAsync(userDto);

                    return Ok(result);
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("signin-google")]
        public async Task<IActionResult> GoogleAuthentication(string code)
        {
            var clientSecrets = new ClientSecrets
            {
                ClientSecret = "thwksHowjddnxNhlYbGyjper",
                ClientId = "35717305987-q66eeghiuij94kvrfl29hcnp88l4du1o.apps.googleusercontent.com"
            };

            AuthorizationCodeTokenRequest request = new AuthorizationCodeTokenRequest
            {
                Code = code,
                ClientSecret = clientSecrets.ClientSecret,
                ClientId = clientSecrets.ClientId,
                GrantType = "authorization_code",
                //Scope = PlusService.Scope.PlusLogin,
                RedirectUri = "http://localhost:3000/signin"
            };


            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://www.googleapis.com/oauth2/v4/")
            };

            var clock = SystemClock.Default;

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "token")
            {
                Content = ParameterUtils.CreateFormUrlEncodedContent(request)
            };

            var response = await httpClient.SendAsync(httpRequest, CancellationToken.None).ConfigureAwait(false);
            var b = await response.Content.ReadAsStringAsync();

            //var a = await request.ExecuteAsync(httpClient, "oauth2/v4/token", CancellationToken.None, clock);


            //var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            //{
            //    ClientSecrets = clientSecrets,
            //    Scopes = new[] { PlusService.Scope.PlusLogin, PlusService.Scope.UserinfoEmail },
            //});

            //var token = flow.ExchangeCodeForTokenAsync("me", code, "postmessage", CancellationToken.None).Result;

            //var service = new Oauth2Service(new Google.Apis.Services.BaseClientService.Initializer());
            //var request = service.Tokeninfo();
            //request.AccessToken = token.AccessToken;
            //var info = request.Execute();


            //AuthorizationCodeFlow acf = new AuthorizationCodeFlow(new AuthorizationCodeFlow.Initializer());

            //AuthorizationCodeFlow.Initializer a = new AuthorizationCodeFlow.Initializer();


            //request.ExecuteAsync()

            return Ok();
        }

        private async Task<AuthenticationResultDto> CreateJwtTokenAsync(LoggedInUserDto userDto)
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

            await _refreshTokenService.DeleteRefreshTokenAsync(userDto.Id);

            var refreshTokenDto = new RefreshTokenDto
            {
                Id = userDto.Id,
                Expiration = DateTime.UtcNow.AddDays(30),
                Value = Guid.NewGuid().ToString()
            };

            await _refreshTokenService.CreateRefreshTokenAsync(refreshTokenDto);

            return new AuthenticationResultDto
            {
                AccessToken = accessToken,
                Expiration = expUnitTime,
                RefreshToken = refreshTokenDto.Value
            };
        }
    }
}
