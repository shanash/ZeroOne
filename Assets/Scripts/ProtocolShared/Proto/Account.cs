using System;
using System.ComponentModel.DataAnnotations;

namespace ProtocolShared.Proto
{
    #region Dev Login
    public class DevLoginRequest
    {
        [Required]
        public string MacAddress { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public Guid PlayerGuid { get; set; } = Guid.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
    #endregion

    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { set; get; } = string.Empty;
    }

    public class RefreshTokenResponse
    {
        [Required]
        public string AccessToken { set; get; } = string.Empty;
    }
}
