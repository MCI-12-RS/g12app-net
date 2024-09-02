using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using G12App.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G12App.Infrastructure.Data.Repositories
{
    public class CellRepository : ICellRepository
    {
        private readonly AppDbContext _context;
        private readonly IReportRepository _reportRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CellRepository(
            AppDbContext context,
            IReportRepository reportRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _reportRepository = reportRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        private IQueryable<Cell> GetCellsQueryableAsync(Query<CellFilter> query)
        {
            return _context.Cells
                .IgnoreAutoIncludes()
                .Where(x => x.IsDeleted == false)
                .ApplyMinistryFilter(_httpContextAccessor?.HttpContext?.User)
                .ApplyFilter(query);
        }

        public async Task<Cell> Create(Cell cell)
        {
            _context.Add(cell);
            await _context.SaveChangesAsync();
            return cell;
        }

        public Task<Cell> GetCellByIdAsync(int? id)
        {
            return GetCellsQueryableAsync(null)
                .Include(x => x.Lead)
                .Include(x => x.Host)
                .Include(x => x.Timothy)
                .Include(x => x.Place)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> CountCellsAsync(Query<CellFilter> query)
        {
            return await
                this.GetCellsQueryableAsync(query)
                    .CountAsync();
        }

        public async Task<IEnumerable<Cell>> GetCellsAsync(Query<CellFilter> query)
        {
            return await GetCellsQueryableAsync(query)
                .Include(x => x.Lead)
                .Include(x => x.Lead.Leadership)
                .Include(x => x.Host)
                .Include(x => x.Timothy)
                .Include(x => x.Place)
                .ApplyFilter(query)
                .ApplyPaging(query)
                .ToListAsync();
        }

        public async Task<Cell> Remove(Cell cell)
        {
            if (_context.Entry(cell).State == EntityState.Detached)
                _context.Attach(cell);
            cell.IsDeleted = true;

            _context.Entry(cell).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return cell;
        }

        public async Task<Cell> Update(Cell cell)
        {
            _context.Attach(cell);
            _context.Entry(cell).State = EntityState.Modified;
            if (cell.Place != null && cell.Place.Id > 0)
                _context.Entry(cell.Place).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return cell;
        }

        public IQueryable<Cell> GetCellsPendingReport(Query<CellFilter> cellQuery, Query<ReportFilter> reportQuery)
        {
            var localNow = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(-3)).DateTime;

            var cells = this.GetCellsQueryableAsync(cellQuery)
                            .Select(cell => new
                            {
                                Cell = cell,
                                Report = _reportRepository.GetReportsQueryableAsync(reportQuery)
                                                        .FirstOrDefault(r => r.CellId == cell.Id)
                            })
                            .Where(cr => cr.Report == null)
                            .Select(cr => cr.Cell)
                            .Where(cell => cell.WeekDay < localNow.DayOfWeek ||
                                            (cell.WeekDay == localNow.DayOfWeek &&
                                            localNow.Hour > (Convert.ToInt32(cell.Time.Substring(0, 2)) + 1)
                                            &&
                                            localNow.Minute > Convert.ToInt32(cell.Time.Substring(3, 2))));

            return cells;
        }
    }
}
