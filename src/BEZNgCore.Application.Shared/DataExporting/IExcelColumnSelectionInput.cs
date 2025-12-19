using System.Collections.Generic;

namespace BEZNgCore.DataExporting;

public interface IExcelColumnSelectionInput
{
    List<string> SelectedColumns { get; set; }
}

