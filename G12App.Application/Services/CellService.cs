using AutoMapper;
using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Model;
using G12App.Domain.Querying;

namespace G12App.Application.Services
{
    public class CellService : ICellService
    {
        private readonly ICellRepository _cellRepository;
        private readonly IMapper _mapper;

        public CellService(ICellRepository cellRepository, IMapper mapper )
        {
            _cellRepository = cellRepository;
            _mapper = mapper;
        }

        public async Task<CellDTO> Add(CellDTO cellDTO)
        {
            var cellEntity = _mapper.Map<Cell>(cellDTO);
            cellEntity.SetupCreation();
            cellEntity.SetupLastModified();
            var cell = await _cellRepository.Create(cellEntity);
            return _mapper.Map<CellDTO>(cell);
        }

        public async Task<CellDTO> GetCellById(int? id)
        {
            var cellEntity = await _cellRepository.GetCellByIdAsync(id);
            return _mapper.Map<CellDTO>(cellEntity);
        }

        public async Task<IEnumerable<CellDTO>> GetCell(Query query)
        {
            var cellEntity = await _cellRepository.GetCellssAsync(query);
            return _mapper.Map<CellDTO[]>(cellEntity);

            
        }

        public async Task Remove(int? id)
        {
            var cellEntity = await _cellRepository.GetCellByIdAsync(id);
            cellEntity.SetupLastModified();
            await _cellRepository.Remove(cellEntity);
        }

        public async Task<CellDTO> Update(CellDTO cellDTO, int id)
        {
            var cellEntity = _mapper.Map<Cell>(cellDTO);
            cellEntity.SetupLastModified();
            var cell = await _cellRepository.Update(cellEntity);
            return _mapper.Map<CellDTO>(cell);
        }
    }
}
