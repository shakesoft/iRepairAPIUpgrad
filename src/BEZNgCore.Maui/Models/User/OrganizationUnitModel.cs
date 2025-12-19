using Abp.AutoMapper;
using BEZNgCore.Organizations.Dto;

namespace BEZNgCore.Maui.Models.User;

[AutoMapFrom(typeof(OrganizationUnitDto))]
public class OrganizationUnitModel : OrganizationUnitDto
{
    public bool IsAssigned { get; set; }
}