using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Database.EntityFrameworkCore;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Servises.Interfaces.AuthenticationServices;
using Utils;
using Utils.NonStatic;

namespace AuthenticationServices.Jwt
{
    public sealed class JwtTokensService : IJwtTokensService
    {
        private readonly IUserService _userService;

        private readonly IHttpUtilsService _httpUtilsService;

        private readonly ApplicationDbContext _dbContext;

        public JwtTokensService(IUserService userService, IHttpUtilsService httpUtilsService, ApplicationDbContext dbContext)
        {
            _userService = userService;
            _httpUtilsService = httpUtilsService;
            _dbContext = dbContext;
        }

        public async Task<JwtToken> GetJwtTokenByResetTokenAsync(string resetToken)
        {
            var jwtResetToken = await _dbContext.JwtResetTokens.Include(t => t.ApplicationUser).FirstOrDefaultAsync(t => t.ResetToken == resetToken);
            if (jwtResetToken != null)
            {
                return new JwtToken()
                {
                    AccessToken = await GetAccessTokenAsync(jwtResetToken.ApplicationUser),
                    ResetToken = resetToken,
                    UserId = jwtResetToken.ApplicationUserId
                };
            }

            return null;
        }

        public async Task<JwtToken> GetJwtTokenAsync(ApplicationUser user)
        {

            var jwtAccessToken = await GetAccessTokenAsync(user);
            var jwtResetToken = GetResetToken(user);
            await AddResetTokenToDbAsync(jwtResetToken);

            return new JwtToken()
            {
                UserId = user.Id, 
                AccessToken = jwtAccessToken,
                ResetToken = jwtResetToken.ResetToken
            };

        }

        public async Task DeleteUserResetTokensAsync(ApplicationUser user)
        {
            var tokens = await _dbContext.JwtResetTokens.Where(t => t.ApplicationUserId == user.Id).ToListAsync();
            _dbContext.JwtResetTokens.RemoveRange(tokens);
            await _dbContext.SaveChangesAsync();
        }

        private async Task AddResetTokenToDbAsync(JwtResetToken jwtResetToken)
        {
            await _dbContext.JwtResetTokens.AddAsync(jwtResetToken);
            await _dbContext.SaveChangesAsync();
        }

        private JwtResetToken GetResetToken(ApplicationUser user)
        {
            return new JwtResetToken()
            {
                ApplicationUserId = user.Id,
                ClientIp = _httpUtilsService.GetClientIpAddressString(),
                CreateDateTime = DateTime.Now,
                ResetToken = user.Id + Randomizer.GetRandomString(300),
                UserAgent = _httpUtilsService.GetUserAgentString()
            };
        }

        private async Task<string> GetAccessTokenAsync(ApplicationUser user)
        {
            var userRoles = await _userService.GetRolesAsync(user);

            var identity = GetClaimsIdentity(user, userRoles);

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                AuthJwtOptions.Issuer,
                AuthJwtOptions.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthJwtOptions.Lifetime)),

                signingCredentials: new SigningCredentials(AuthJwtOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private static ClaimsIdentity GetClaimsIdentity(ApplicationUser user, IEnumerable<string> userRoles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim(ClaimsIdentity.DefaultIssuer, AuthJwtOptions.Issuer)
            };

            if (userRoles != null)
                claims.AddRange(userRoles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            var claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }


}
