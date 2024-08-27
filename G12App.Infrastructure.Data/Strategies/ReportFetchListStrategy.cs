using G12App.Domain.Interfaces.Strategies;
using G12App.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace G12App.Infrastructure.Data.Strategies
{
    public class ReportFetchListStrategy : IReportFetchListStrategy
    {
        public IQueryable<Report> Setup(IQueryable<Report> query)
        {
            return query
                    .Include(x => x.Members)
                    .Include(x => x.Lead)
                    .Include(x => x.Cell)
                    .Include(x => x.Cell.Place);
        }
    }
}
