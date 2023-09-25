using AutoMapper;
using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Extensions;
using System.Net;

namespace G12App.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public ReportService(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<ReportDTO>> Add(ReportDTO reportDTO)
        {
            var reportEntity = _mapper.Map<Report>(reportDTO);
            reportEntity.SetupCreation();
            reportEntity.SetupLastModified();
            var report = await _reportRepository.Create(reportEntity);
            return new ServiceResult<ReportDTO>(_mapper.Map<ReportDTO>(report));
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var reportEntity = await _reportRepository.GetReportById(id);
            if (reportEntity == null)
                return new ServiceResult(HttpStatusCode.NotFound, "Não encontrado");

            reportEntity.SetupLastModified();
            await _reportRepository.Remove(reportEntity);
            return new ServiceResult();
        }

        public Task<ServiceResult<IEnumerable<ReportDTO>>> GetAllReports(Query query)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<ReportDTO>> GetByCellId(int id)
        {
            var reportEntity = await _reportRepository.GetReportById(id);
            return new ServiceResult<ReportDTO>(_mapper.Map<ReportDTO>(reportEntity));
        }

        public Task<ServiceResult<ReportDTO>> GetByLeadershipId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<ReportDTO>> Update(ReportDTO reportDTO, int id)
        {
            throw new NotImplementedException();
        }
    }
}
