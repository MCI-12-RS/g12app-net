using G12App.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace G12App.Domain.Interfaces.Repositories
{
    public interface IMemberRepository
    {
        IQueryable<Member> Get();
    }
}
