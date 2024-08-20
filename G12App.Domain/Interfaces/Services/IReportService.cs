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
        Task<ServiceResult<IEnumerable<ReportDto>>> GetAllReports(Query<ReportFilter> query);
        Task<ServiceResult<ReportDto>> GetById (int id);
        Task<ServiceResult<ReportDto>> Add(ReportDto reportDTO);
        Task <ServiceResult<ReportDto>> Update(ReportDto reportDTO, int id);
        Task<ServiceResult> Delete(int id);
    }
}
