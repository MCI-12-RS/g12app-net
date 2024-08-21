using AutoMapper;
using G12App.Application.Events;
using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Events;
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
        private readonly IEventDispatcher _eventDispatcher;

        public CellService(
            ICellRepository cellRepository,
            IMapper mapper,
            IEntityChangeTrackingService changeTrackingService,
            IEventDispatcher eventDispatcher)
        {
            _cellRepository = cellRepository;
            _mapper = mapper;
            _changeTrackingService = changeTrackingService;
            _eventDispatcher = eventDispatcher;
        }

        public async Task<ServiceResult<CellDto>> Add(CellDto cellDTO)
        {
            var cellEntity = _mapper.Map<Cell>(cellDTO);

            _changeTrackingService.SetupCreationData(cellEntity);
            _changeTrackingService.SetupLastModifiedData(cellEntity);

            var cell = await _cellRepository.Create(cellEntity);
            var resultCellDto = _mapper.Map<CellDto>(cell);

            await _eventDispatcher.DispatchEvent(new CellCreated(resultCellDto));

            return new ServiceResult<CellDto>(_mapper.Map<CellDto>(cell));
        }

        public async Task<ServiceResult<CellDto>> Update(CellDto cellDTO, int id)
        {
            var cellEntity = await _cellRepository.GetCellByIdAsync(id);
            var sourceCellDto = _mapper.Map<Cell, CellDto>(cellEntity);

            cellEntity = _mapper.Map(cellDTO, cellEntity);

            _changeTrackingService.SetupLastModifiedData(cellEntity);

            var cell = await _cellRepository.Update(cellEntity);
            var resultCellDto = _mapper.Map<CellDto>(cell);

            await _eventDispatcher.DispatchEvent(new CellUpdated(sourceCellDto, resultCellDto));

            return new ServiceResult<CellDto>(_mapper.Map<CellDto>(cell));
        }

        public async Task<ServiceResult<CellDto>> GetCellById(int? id)
        {
            var cellEntity = await _cellRepository.GetCellByIdAsync(id);
            return new ServiceResult<CellDto>(_mapper.Map<CellDto>(cellEntity));
        }

        public async Task<ServiceResult<IEnumerable<CellDto>>> GetCell(Query<CellFilter> query)
        {
            var cellEntity = await _cellRepository.GetCellsAsync(query);
            var totalMembers = cellEntity.Any() ? await _cellRepository.CountCellsAsync(query) : 0;
            return new ServiceResult<IEnumerable<CellDto>>(
                _mapper.Map<IEnumerable<CellDto>>(cellEntity), totalMembers);
        }

        public async Task<ServiceResult> Remove(int? id)
        {
            var cellEntity = await _cellRepository.GetCellByIdAsync(id);
            if (cellEntity == null)
                return new ServiceResult(HttpStatusCode.NotFound, "Não encontrado.");

            var sourceCellDto = _mapper.Map<Cell, CellDto>(cellEntity);

            _changeTrackingService.SetupLastModifiedData(cellEntity);
            await _cellRepository.Remove(cellEntity);

            await _eventDispatcher.DispatchEvent(new CellDeleted(sourceCellDto));

            return new ServiceResult();
        }
    }
}
