using AutoMapper;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Model;
using G12App.Domain.Extensions;
using G12App.Domain.Querying;
using G12App.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using G12App.Domain.Querying.Filters;

namespace G12App.Infrastructure.Data.Repositories
{
    public class CellRepository : ICellRepository
    {
        private readonly AppContext _context;
        private readonly IMapper _mapper;

        public CellRepository(AppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Cell> Create(Cell cell)
        {
            _context.Add(cell);
            await _context.SaveChangesAsync();
            return cell;
        }

        public Task<Cell> GetCellByIdAsync(int? id)
        {
            return _context.Cells
                .Include(x => x.Lead)
                .Include(x => x.Host)
                .Include(x => x.Timothy)
                .Include(x => x.Place)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
        }

        public async Task<IEnumerable<Cell>> GetCellsAsync(Query<CellFilter> query)
        {
            return await _context.Cells
                .Where(x => x.IsDeleted == false)
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
            var cellEntity = await _context.Cells.FirstOrDefaultAsync(x => x.Id == cell.Id && x.IsDeleted == false);
            _mapper.Map(cell, cellEntity);
            _context.Entry(cellEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return cell;
        }
    }
}
