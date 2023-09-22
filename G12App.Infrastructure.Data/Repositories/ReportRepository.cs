using AutoMapper;
using AutoMapper.Execution;
using G12App.Common.Extensions;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Model;
using G12App.Domain.Querying;
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
        private readonly IMapper _mapper;

        public ReportRepository(AppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Report> Create(Report report)
        {
            _context.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<IEnumerable<Report>> GetByCellId(int? cellId)
        {
            return await _context.Reports
                .Where(x => x.IsDeleted == false && x.CellId == cellId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetReportsByLeadId(int LeadId)
        {
            return await _context.Reports
                .Where(x => x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Report> Update(Report report)
        {
            var reportEntity = await _context.Reports.FirstOrDefaultAsync
                (x => x.CellId == report.CellId && x.IsDeleted == false);
            _mapper.Map(report, reportEntity);
            _context.Entry(reportEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<IEnumerable<Report>> GetReportsAsync(Query query)
        {
            return await _context.Reports
                .Where(x => x.IsDeleted == false)
                .ApplyPaging(query)
                .ToListAsync();
        }

        public async Task<Report> Remove (Report report)
        {
            if (_context.Entry(report).State == EntityState.Detached)
                _context.Attach(report);
            report.IsDeleted = true;

            _context.Entry(report).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<IEnumerable<Report>> GetReportsById(int reportId)
        {
            return await _context.Reports
                .Where (x => x.IsDeleted == false && x.ReportId == reportId)
                .ToListAsync();
        }
    }
}
