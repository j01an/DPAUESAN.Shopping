﻿using DPAUESAN.Shopping.DOMAIN.Core.DTO;
using DPAUESAN.Shopping.DOMAIN.Core.Entities;
using DPAUESAN.Shopping.DOMAIN.Core.Interfaces;

namespace DPAUESAN.Shopping.DOMAIN.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJWTFactory _jwtFactory;

        public UserService(IUserRepository userRepository, IJWTFactory jwtFactory)
        {
            _userRepository = userRepository;
            _jwtFactory = jwtFactory;
        }

        public async Task<UserAuthResponseDTO> Validate(string email, string password)
        {
            var user = await _userRepository.SignIn(email, password);
            if (user == null)
                return null;

            var token = _jwtFactory.GenerateJWToken(user);

            var userDTO = new UserAuthResponseDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Country = user.Country,
                DateOfBirth = user.DateOfBirth,
                Address = user.Address,
                Token = token
            };
            return userDTO;
        }

        public async Task<bool> Register(UserAuthRequestDTO userDTO)
        {
            //Validación para registro
            var emaiResult = await _userRepository.IsEmailRegistered(userDTO.Email);
            if (emaiResult)
                return false;

            var user = new User()
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                Country = userDTO.Country,
                Address = userDTO.Address,
                DateOfBirth = userDTO.DateOfBirth,
                Password = userDTO.Password,
                IsActive = true,
                Type = "U"
            };

            var result = await _userRepository.SignUp(user);
            return result;
        }
    }
}
