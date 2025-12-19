using System.Collections.Generic;

namespace BEZNgCore.DynamicEntityProperties;

public interface IDynamicEntityPropertyDefinitionAppService
{
    List<string> GetAllAllowedInputTypeNames();

    List<string> GetAllEntities();
}

