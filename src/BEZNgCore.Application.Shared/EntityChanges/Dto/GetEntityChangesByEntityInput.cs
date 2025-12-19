using BEZNgCore.Dto;
using System;

namespace BEZNgCore.EntityChanges.Dto;

public class GetEntityChangesByEntityInput
{
    public string EntityTypeFullName { get; set; }
    public string EntityId { get; set; }
}

