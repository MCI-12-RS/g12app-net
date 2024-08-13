using AutoMapper;
using AutoMapper.Execution;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using G12App.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G12App.Infrastructure.Data.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppContext _context;

        public ReportRepository(AppContext context)
        {
            _context = context;
        }

        public async Task<Report> Create(Report report)
        {
            _context.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }


        public async Task<Report> Update(Report report)
        {
            _context.Attach(report);
            _context.Entry(report).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return report;
        }

        private IQueryable<Report> GetReportsQueryableAsync(Query<ReportFilter> query)
        {
            return _context.Reports
                .IgnoreAutoIncludes()
                .Where(x => x.IsDeleted == false)
                .ApplyFilter(query);
        }

        public async Task<IEnumerable<Report>> GetReportsAsync(Query<ReportFilter> query)
        {
            return await GetReportsQueryableAsync(query)
                .ApplySort(query)
                .ApplyPaging(query)
                .ToListAsync();
        }

        public async Task<int> CountReportsAsync(Query<ReportFilter> query)
        {
            return await
                this.GetReportsQueryableAsync(query)
                    .CountAsync();
        }

        public async Task<Report> Remove(Report report)
        {
            if (_context.Entry(report).State == EntityState.Detached)
                _context.Attach(report);
            report.IsDeleted = true;

            _context.Entry(report).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<Report> GetReportById(int Id)
        {
            return await _context.Reports.FirstOrDefaultAsync
                (x => x.Id == Id && x.IsDeleted == false);

        }
    }
}
