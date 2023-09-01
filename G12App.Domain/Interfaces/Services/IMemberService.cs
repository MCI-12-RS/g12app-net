using G12App.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Services
{
    public interface IMemberService
    {
        Task<List<Member>> GetMembers();
    }
}
