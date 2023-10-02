using G12App.Domain.DTOs;
using G12App.Domain.Messages;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Services
{
    public interface IReportService
    {
        Task<ServiceResult<IEnumerable<ReportDTO>>> GetAllReports(Query<ReportFilter> query);
        Task<ServiceResult<ReportDTO>> GetByReportId (int id);
        Task<ServiceResult<ReportDTO>> Add(ReportDTO reportDTO);
        Task <ServiceResult<ReportDTO>> Update(ReportDTO reportDTO, int id);
        Task<ServiceResult> Delete(int id);
    }
}
