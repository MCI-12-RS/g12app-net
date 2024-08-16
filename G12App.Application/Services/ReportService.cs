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
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        private readonly IEntityChangeTrackingService _changeTrackingService;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IMemberRepository _memberRepository;

        public ReportService(
            IReportRepository reportRepository,
            IMemberRepository memberRepository,
            IMapper mapper,
            IEntityChangeTrackingService changeTrackingService,
            IEventDispatcher eventDispatcher)
        {
            _reportRepository = reportRepository;
            _memberRepository = memberRepository;
            _mapper = mapper;
            _changeTrackingService = changeTrackingService;
            _eventDispatcher = eventDispatcher;
        }

        public async Task<ServiceResult<ReportDTO>> Add(ReportDTO reportDTO)
        {
            var reportEntity = _mapper.Map<Report>(reportDTO);

            if (reportDTO?.Frequenters != null && reportDTO.Frequenters.Any())
                foreach (var memberId in reportDTO.Frequenters)
                    reportEntity.Members.Add(
                        await _memberRepository.GetMemberByIdAsync(memberId));

            if (reportDTO.LeadershipId != default && !reportEntity.Members.Any(x => x.Id == reportDTO.LeadershipId))
                reportEntity.Members.Add(await _memberRepository.GetMemberByIdAsync(reportDTO.LeadershipId));

            if (reportDTO.TimothyId != default && !reportEntity.Members.Any(x => x.Id == reportDTO.TimothyId))
                reportEntity.Members.Add(await _memberRepository.GetMemberByIdAsync(reportDTO.TimothyId));

            if (reportDTO.HostId != default && !reportEntity.Members.Any(x => x.Id == reportDTO.HostId))
                reportEntity.Members.Add(await _memberRepository.GetMemberByIdAsync(reportDTO.HostId));

            reportEntity.NumMembers = reportEntity.Members.Count();
            reportEntity.NumVisitors = reportDTO.Visitors.Where(x => !string.IsNullOrWhiteSpace(x)).Count();

            _changeTrackingService.SetupCreationData(reportEntity);
            _changeTrackingService.SetupLastModifiedData(reportEntity);

            var report = await _reportRepository.Create(reportEntity);
            var resultReportDto = _mapper.Map<ReportDTO>(report);

            await _eventDispatcher.DispatchEvent(new ReportLogged(null, resultReportDto));

            return new ServiceResult<ReportDTO>(resultReportDto);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var reportEntity = await _reportRepository.GetReportById(id);
            if (reportEntity == null)
                return new ServiceResult(HttpStatusCode.NotFound, "Não encontrado");

            _changeTrackingService.SetupLastModifiedData(reportEntity);

            await _reportRepository.Remove(reportEntity);
            return new ServiceResult();
        }

        public async Task<ServiceResult<IEnumerable<ReportDTO>>> GetAllReports(Query<ReportFilter> query)
        {
            var reportEntity = await _reportRepository.GetReportsAsync(query);
            var totalMembers = reportEntity.Any() ? await _reportRepository.CountReportsAsync(query) : 0;
            return new ServiceResult<IEnumerable<ReportDTO>>(
                _mapper.Map<IEnumerable<ReportDTO>>(reportEntity), totalMembers);
        }

        public async Task<ServiceResult<ReportDTO>> Update(ReportDTO reportDTO, int id)
        {
            var reportEntity = _mapper.Map<Report>(reportDTO);

            _changeTrackingService.SetupLastModifiedData(reportEntity);

            var report = await _reportRepository.Update(reportEntity);

            return new ServiceResult<ReportDTO>(_mapper.Map<ReportDTO>(report));

        }

        public async Task<ServiceResult<ReportDTO>> GetById(int id)
        {
            var reportEntity = await _reportRepository.GetReportById(id);
            return new ServiceResult<ReportDTO>(_mapper.Map<ReportDTO>(reportEntity));
        }
    }
}
