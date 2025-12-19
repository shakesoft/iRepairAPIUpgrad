using System.Threading.Tasks;

namespace BEZNgCore.Authorization.Users.Profile;

public interface IProfilePictureValidator
{
    Task ValidateProfilePictureDimensions(byte[] imageBytes);
    Task ValidateProfilePictureSize(byte[] imageBytes);
}