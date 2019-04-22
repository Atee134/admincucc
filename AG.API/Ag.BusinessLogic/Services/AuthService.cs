using Ag.BusinessLogic.Interfaces;
using Ag.BusinessLogic.Interfaces.Converters;
using Ag.Common.Dtos.Request;
using Ag.Common.Dtos.Response;
using Ag.Domain;
using Ag.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Ag.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly AgDbContext _context;
        private readonly ILogger<AuthService> _logger;
        private readonly IJoinTableHelperService _joinTableHelperService;
        private readonly IUserConverter _userConverter;
        private readonly IConfiguration _configuration;

        public AuthService(AgDbContext context, ILogger<AuthService> logger, IJoinTableHelperService joinTableHelperService, IUserConverter userConverter, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _joinTableHelperService = joinTableHelperService;
            _userConverter = userConverter;
            _configuration = configuration;
        }

        public UserDetailDto Login(UserForLoginDto userDto)
        {
            userDto.UserName = userDto.UserName.ToLower();

            _logger.LogInformation($"A login attemp is made with username: {userDto.UserName}");

            var user = _context.Users.SingleOrDefault(u => u.UserName == userDto.UserName);

            if (user == null) return null;

            if (!VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt)) return null;

            _logger.LogInformation($"User: {userDto.UserName} logged in successfully.");

            return _userConverter.ConvertToUserDetailDto(user);
        }

        public UserForListDto Register(UserForRegisterDto userDto)
        {
            userDto.UserName = userDto.UserName.ToLower();

            byte[] passwordHash;
            byte[] passwordSalt;
            CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

            var user = CreateUserFromDto(userDto);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

            _logger.LogInformation($"User: {userDto.UserName} successfully registrated");

            return _userConverter.ConvertToUserToListDto(user);
        }

        public void ChangeUserPassword(User user, string newPassword)
        {
            byte[] passwordHash;
            byte[] passwordSalt;
            CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.SaveChanges();

            _logger.LogInformation($"Changing password of user with ID: {user.Id} was successful.");
        }

        public bool UserExists(string userName)
        {
            userName = userName.ToLower();

            if (_context.Users.Any(u => u.UserName == userName))
                return true;

            return false;
        }

        private User CreateUserFromDto(UserForRegisterDto userDto)
        {
            return new User
            {
                UserName = userDto.UserName,
                Role = userDto.Role,
                MinPercent = 0.25,
                MaxPercent = 0.3,
                Shift = Common.Enums.Shift.Morning,
                Sites = String.Empty,
            };
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }

                return true;
            }
        }
    }
}
