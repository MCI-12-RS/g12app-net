using G12App.Domain.DTOs;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashbordData();
    }
}
