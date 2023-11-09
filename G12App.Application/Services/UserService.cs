using AutoMapper;
using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System.Net;

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

        public async Task<ServiceResult<UserDTO>> GetUserById(int? id)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(id);
            return new ServiceResult<UserDTO>(_mapper.Map<UserDTO>(userEntity));
        }

        public async Task<ServiceResult<IEnumerable<UserDTO>>> GetUsers(Query<UserFilter> query)
        {
            var userEntity = await _userRepository.GetUsersAsync(query);
            return new ServiceResult<IEnumerable<UserDTO>>(_mapper.Map<IEnumerable<UserDTO>>(userEntity));
        }

        public async Task<ServiceResult> Remove(int? id)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(id);
            if (userEntity == null)
                return new ServiceResult(HttpStatusCode.NotFound, "Usuário não encontrado");

            userEntity.SetupLastModified();
            await _userRepository.Remove(userEntity);
            return new ServiceResult();
        }

        public async Task<ServiceResult<UserDTO>> Update(UserDTO userDTO, int id)
        {
            var userEntity = _mapper.Map<User>(userDTO);
            userEntity.SetupLastModified();
            var user = await _userRepository.Update(userEntity);
            return new ServiceResult<UserDTO>(_mapper.Map<UserDTO>(user));
        }
    }
}
