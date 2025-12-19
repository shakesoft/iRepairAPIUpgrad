using Abp.EntityHistory;
using BEZNgCore.Authorization.Users;
using System.Collections.Generic;

namespace BEZNgCore.EntityChanges;

public class EntityChangePropertyAndUser
{
    public EntityChange EntityChange { get; set; }
    public EntityChangeSet EntityChangeSet { get; set; }
    public List<EntityPropertyChange> PropertyChanges { get; set; }
    public User User { get; set; }
    public string ImpersonatorUserName { get; set; }
}
