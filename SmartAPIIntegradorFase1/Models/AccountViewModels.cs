using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartAPIIntegradorFase1.Models
{
    // Modelos retornados por ações AccountController.

    public class LoginViewModel
    {
        [Key(), ScaffoldColumn(false), Editable(false)]
        public string UserId { get; set; }

        [Display(Name ="Conta")]
        [DataType(DataType.Text)]
        [StringLength(80, ErrorMessage ="{0} deve ter entre {2} e {1} caracteres.")]
        public string Account { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Text)]
        [StringLength(80, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "{0} e {1} devem ser idênticas")]
        [DataType(DataType.Password)]
        [StringLength(80, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres.")]
        public string PasswordConfirmation { get; set; }

        [Display(Name ="Manter conectado?")]
        public bool KeepAlive { get; set; }
    }

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}
