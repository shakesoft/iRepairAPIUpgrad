using System.ComponentModel.DataAnnotations;

namespace BEZNgCore.Localization.Dto;

public class CreateOrUpdateLanguageInput
{
    [Required]
    public ApplicationLanguageEditDto Language { get; set; }
}

