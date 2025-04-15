using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.Auth;
using ApplicationLayer.Interfaces.Identity;

namespace Infrastructure.Identity.Services
{
    public class IdentityService(UserManager<User> userManager,
        SignInManager<User> signInManager, ILogger<IdentityService> logger) : IIdentityService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly ILogger<IdentityService> logger = logger;

        public async Task<Result<LoginResponseDto>> GetTokenAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                logger.LogWarning("No se encontró ninguna cuenta asociada al email {Email}", request.Email);
                return Result<LoginResponseDto>.BadRequest($"No se encontró ninguna cuenta asociada al email {request.Email}");
            }

            // Verificar si el usuario está bloqueado
            if (await _userManager.IsLockedOutAsync(user))
            {
                logger.LogWarning("La cuenta está bloqueada para {Email}", request.Email);
                return Result<LoginResponseDto>.BadRequest($"La cuenta está bloqueada. Por favor, contacte al administrador del sistema");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded)
            {
                await _userManager.AccessFailedAsync(user);

                if (await _userManager.IsLockedOutAsync(user))
                {
                    logger.LogWarning("La cuenta con email {Email} se ha bloqueado debido a demasiados intentos fallidos", request.Email);
                    return Result<LoginResponseDto>.BadRequest($"Demasiados intentos fallidos. Por favor, inténtelo más tarde");
                }

                logger.LogWarning("Credenciales inválidas para {Email}", request.Email);
                return Result<LoginResponseDto>.BadRequest($"Credenciales inválidas para '{request.Email}'");
            }

            await _userManager.ResetAccessFailedCountAsync(user);

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            var response = new LoginResponseDto
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                IssuedOn = jwtSecurityToken.ValidFrom.ToLocalTime(),
                ExpiresOn = jwtSecurityToken.ValidTo.ToLocalTime(),
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                IsVerified = user.EmailConfirmed,
                Roles = [.. rolesList]
            };



            logger.LogInformation("Inicio de sesión exitoso para {Email}", request.Email);
            return Result<LoginResponseDto>.Ok("Inicio de sesión exitoso", response);
        }

        private async Task<JwtSecurityToken> GenerateJWToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var tokenId = Guid.NewGuid().ToString();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, tokenId),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim("uid", user.Id),
                new Claim("first_name", user.FirstName),
                new Claim("last_name", user.LastName),
                new Claim("full_name", $"{user.FirstName} {user.LastName}"),
                new Claim("tokenId", user.SecurityStamp ?? string.Empty)
            }
            .Union(userClaims);

            user.CurrentTokenId = tokenId;
            await _userManager.UpdateAsync(user);
            return JWTGeneration(claims);
        }

        private static JwtSecurityToken JWTGeneration(IEnumerable<Claim> claims)
        {
            string key = "r9lQUmX3kXbInSlegOBGt72cqAfOqJkDIpo0ptlydduUtaIpnX$:xr.C.HA!mQgXSb=E/!;3!Au35FU=R+$swgPc6.w_bO3Uf.%bFLa9mVt1fGEK|{ia63B%DyRK?lz%v6cE01-lr?sitXMuY>kX%8`$KOi`gL";
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: "SistemaPOS.Admin",
                audience: "SistemaPOS.Customer",
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(150),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }


    }
}
