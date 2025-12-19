using System.Collections.Generic;

namespace BEZNgCore.EntityHistory;

public class EntityHistoryUiSetting
{
    public bool IsEnabled { get; set; }

    public List<string> EnabledEntities { get; set; }
}

