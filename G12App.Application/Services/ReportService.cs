using AutoMapper;
using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Extensions;
using System.Net;
using G12App.Domain.Querying.Filters;

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

        public async Task<ServiceResult<IEnumerable<ReportDTO>>> GetAllReports(Query<ReportFilter> query)
        {
            var reportEntity = await _reportRepository.GetReportsAsync(query);
            return new ServiceResult<IEnumerable<ReportDTO>>(_mapper.Map<IEnumerable<ReportDTO>>(reportEntity));
        }

        public async Task<ServiceResult<ReportDTO>> Update(ReportDTO reportDTO, int id)
        {
            var reportEntity = _mapper.Map<Report>(reportDTO);
            reportEntity.SetupLastModified();
            var report = await _reportRepository.Update(reportEntity);
            return new ServiceResult<ReportDTO>(_mapper.Map<ReportDTO>(report));

        }

        public async Task<ServiceResult<ReportDTO>> GetByReportId (int id)
        {
            var reportEntity = await _reportRepository.GetReportById(id);
            return new ServiceResult<ReportDTO>(_mapper.Map<ReportDTO>(reportEntity));
        }
    }
}
