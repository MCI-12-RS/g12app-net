using AutoMapper;
using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;

namespace G12App.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResult<UserDTO>> Add(UserDTO userDTO)
        {
            var userEntity = _mapper.Map<User>(userDTO);
            userEntity.SetupCreation();
            userEntity.SetupLastModified();
            var user = await _userRepository.Create(userEntity);
            return new ServiceResult<UserDTO>(_mapper.Map<UserDTO>(user));
        }

        public Task<ServiceResult<UserDTO>> GetUserById(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<IEnumerable<UserDTO>>> GetUsers(Query<UserFilter> query)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> Remove(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<UserDTO>> Update(UserDTO userDTO)
        {
            throw new NotImplementedException();
        }
    }
}
