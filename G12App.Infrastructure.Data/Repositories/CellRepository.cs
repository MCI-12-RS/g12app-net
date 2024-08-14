using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using G12App.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G12App.Infrastructure.Data.Repositories
{
    public class CellRepository : ICellRepository
    {
        private readonly AppContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CellRepository(
            AppContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
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
    }
}
