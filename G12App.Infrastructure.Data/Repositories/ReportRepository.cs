using AutoMapper;
using G12App.Domain.Model;
using G12App.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace G12App.Infrastructure.Data.Repositories
{
    public class ReportRepository
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

        public async Task<Report> GetByCellId(int? cellId)
        {
            return await _context.Reports.FirstOrDefaultAsync
                (x => x.CellId == cellId && x.IsDeleted == false);
        }

        public async Task<Report> GetByLeadershipId(int? cellId)
        {
            return await _context.Reports.FirstOrDefaultAsync
                (x => x.LeadershipId == cellId && x.IsDeleted == false);
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
    }
}
