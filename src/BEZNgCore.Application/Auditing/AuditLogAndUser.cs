using Abp.Auditing;
using BEZNgCore.Authorization.Users;

namespace BEZNgCore.Auditing;

/// <summary>
/// A helper class to store an <see cref="AuditLog"/> and a <see cref="User"/> object.
/// </summary>
public class AuditLogAndUser
{
    public AuditLog AuditLog { get; set; }

    public User User { get; set; }
}
