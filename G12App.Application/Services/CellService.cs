using AutoMapper;
using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System.Net;

namespace G12App.Application.Services
{
    public class CellService : ICellService
    {
        private readonly ICellRepository _cellRepository;
        private readonly IMapper _mapper;
        private readonly IEntityChangeTrackingService _changeTrackingService;

        public CellService(
            ICellRepository cellRepository,
            IMapper mapper,
            IEntityChangeTrackingService changeTrackingService)
        {
            _cellRepository = cellRepository;
            _mapper = mapper;
            _changeTrackingService = changeTrackingService;
        }

        public async Task<ServiceResult<CellDTO>> Add(CellDTO cellDTO)
        {
            var cellEntity = _mapper.Map<Cell>(cellDTO);
            _changeTrackingService.SetupCreationData(cellEntity);
            _changeTrackingService.SetupLastModifiedData(cellEntity);

            var cell = await _cellRepository.Create(cellEntity);
            return new ServiceResult<CellDTO>(_mapper.Map<CellDTO>(cell));
        }

        public async Task<ServiceResult<CellDTO>> GetCellById(int? id)
        {
            var cellEntity = await _cellRepository.GetCellByIdAsync(id);
            return new ServiceResult<CellDTO>(_mapper.Map<CellDTO>(cellEntity));
        }

        public async Task<ServiceResult<IEnumerable<CellDTO>>> GetCell(Query<CellFilter> query)
        {
            var cellEntity = await _cellRepository.GetCellsAsync(query);
            var totalMembers = cellEntity.Any() ? await _cellRepository.CountCellsAsync(query) : 0;
            return new ServiceResult<IEnumerable<CellDTO>>(
                _mapper.Map<IEnumerable<CellDTO>>(cellEntity), totalMembers);
        }

        public async Task<ServiceResult> Remove(int? id)
        {
            var cellEntity = await _cellRepository.GetCellByIdAsync(id);
            if (cellEntity == null)
                return new ServiceResult(HttpStatusCode.NotFound, "Não encontrado.");

            _changeTrackingService.SetupLastModifiedData(cellEntity);
            await _cellRepository.Remove(cellEntity);
            return new ServiceResult();
        }

        public async Task<ServiceResult<CellDTO>> Update(CellDTO cellDTO, int id)
        {
            var cellEntity = _mapper.Map<Cell>(cellDTO);

            _changeTrackingService.SetupLastModifiedData(cellEntity);
            var cell = await _cellRepository.Update(cellEntity);
            return new ServiceResult<CellDTO>(_mapper.Map<CellDTO>(cell));
        }
    }
}
