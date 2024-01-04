namespace ProtocolShared.Proto
{
    #region Dev Login
    public class DevLoginRequest
    {
        [Required]
        public string macAddress { get; set; } = string.Empty;
    }

    public class DevLoginResponse
    {
        public string userId { get; set; } = string.Empty;
        public string accessToken { get; set; } = string.Empty;
        public string refreshToken { get; set; } = string.Empty;
    }
    #endregion
}
