using G12App.Domain.Model;
using System;
using System.Collections.Generic;

namespace G12App.Domain.Interfaces.Services
{
    public interface IColumnFormatter
    {
        IDictionary<string, Func<CellReports, object>> GetCellReportFormatter();
    }
}
