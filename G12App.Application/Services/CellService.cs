using AutoMapper;
using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Messages;
using System.Net;
using AutoMapper.Execution;

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

        public async Task<ServiceResult<CellDTO>> Add(CellDTO cellDTO)
        {
            var cellEntity = _mapper.Map<Cell>(cellDTO);
            cellEntity.SetupCreation();
            cellEntity.SetupLastModified();
            var cell = await _cellRepository.Create(cellEntity);
            return new ServiceResult<CellDTO>(_mapper.Map<CellDTO>(cell));
        }

        public async Task<ServiceResult<CellDTO>> GetCellById(int? id)
        {
            var cellEntity = await _cellRepository.GetCellByIdAsync(id);
            return new ServiceResult<CellDTO>(_mapper.Map<CellDTO>(cellEntity));
        }

        public async Task<ServiceResult<IEnumerable<CellDTO>>> GetCell(Query query)
        {
            var cellEntity = await _cellRepository.GetCellssAsync(query);
            return new ServiceResult<IEnumerable<CellDTO>>(_mapper.Map<IEnumerable<CellDTO>>(cellEntity));


        }

        public async Task<ServiceResult> Remove(int? id)
        {
            var cellEntity = await _cellRepository.GetCellByIdAsync(id);
            if (cellEntity == null)
                return new ServiceResult(HttpStatusCode.NotFound, "Não encontrado.");

            cellEntity.SetupLastModified();
            await _cellRepository.Remove(cellEntity);
            return new ServiceResult();
        }

        public async Task<ServiceResult<CellDTO>> Update(CellDTO cellDTO, int id)
        {
            var cellEntity = _mapper.Map<Cell>(cellDTO);
            cellEntity.SetupLastModified();
            var cell = await _cellRepository.Update(cellEntity);
            return new ServiceResult<CellDTO>(_mapper.Map<CellDTO>(cell));
        }
    }
}
