using System.ComponentModel.DataAnnotations;

namespace BEZNgCore.Authorization.Users.Dto;

public class ChangeUserLanguageDto
{
    [Required]
    public string LanguageName { get; set; }
}

