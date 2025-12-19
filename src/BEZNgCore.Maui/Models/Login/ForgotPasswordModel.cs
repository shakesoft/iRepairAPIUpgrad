using System.ComponentModel.DataAnnotations;

namespace BEZNgCore.Maui.Models.Login;

public class ForgotPasswordModel
{
    [EmailAddress]
    [Required]
    public string EmailAddress { get; set; }
}