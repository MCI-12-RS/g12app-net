using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Model;

namespace G12App.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _repository;

        public MemberService(IMemberRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Member>> GetMembers()
        {
            var list = _repository.Get()
                .Where(x => x.Id >= 1 && x.Id <= 10)
                .ToList();

            return list;
        }
    }
}
