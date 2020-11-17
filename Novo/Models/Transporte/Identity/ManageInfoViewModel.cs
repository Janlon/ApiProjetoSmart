namespace WEBAPI_VOPAK.Models
{
    using System.Collections.Generic;
    /// <summary>
    /// Identity transport model.
    /// </summary>
    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }
}