using Abp.AutoMapper;
using BEZNgCore.Authorization.Users.Dto;

namespace BEZNgCore.Maui.Models.User;

[AutoMapFrom(typeof(UserListDto))]
public class UserListModel : UserListDto
{
    public string Photo { get; set; }

    public string FullName => Name + " " + Surname;
}