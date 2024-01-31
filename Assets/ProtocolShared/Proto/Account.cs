using System.ComponentModel.DataAnnotations;

namespace ProtocolShared.Proto
{
    #region Dev Login
    public class DevLoginRequest
    {
        [Required]
        public string macAddress { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string userId { get; set; } = string.Empty;
        public string accessToken { get; set; } = string.Empty;
        public string refreshToken { get; set; } = string.Empty;
    }
    #endregion

    public class RefreshTokenRequest
    {
        [Required]
        public string refreshToken { set; get; } = string.Empty;
    }

    public class RefreshTokenResponse
    {
        [Required]
        public string accessToken { set; get; } = string.Empty;
    }
}
