using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.Auth;

namespace ApplicationLayer.Interfaces.Identity
{
    public interface IIdentityService
    {
        Task<Result<LoginResponseDto>> GetTokenAsync(LoginRequestDto request);
    }
}
