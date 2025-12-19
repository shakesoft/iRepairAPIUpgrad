using BEZNgCore.Maui.Models.NavigationMenu;

namespace BEZNgCore.Maui.Services.Navigation;

public interface IMenuProvider
{
    List<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
}