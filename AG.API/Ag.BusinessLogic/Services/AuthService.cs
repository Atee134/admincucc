﻿using Ag.BusinessLogic.Interfaces;
using Ag.Common.Dtos.Request;
using Ag.Common.Dtos.Response;
using Ag.Common.Enums;
using Ag.Domain;
using Ag.Domain.Models;
using System;
using System.Linq;

namespace Ag.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly AgDbContext _context;

        public AuthService(AgDbContext context)
        {
            _context = context;
        }

        public UserDetailDto Login(UserForLoginDto userDto)
        {
            userDto.UserName = userDto.UserName.ToLower();

            var user = _context.Users.SingleOrDefault(u => u.UserName == userDto.UserName);

            if (user == null) return null;

            if (!VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt)) return null;

            return new UserDetailDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Shift = user.Shift,
                Role = user.Role,
                Sites = user.Sites.Split(';').Select(s => Enum.Parse<Site>(s)).ToList(),
                MinPercent = user.MinPercent,
                MaxPercent = user.MaxPercent,
            };
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

            return new UserForListDto
            {
                UserName = user.UserName,
                Shift = user.Shift.ToString()
            };
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
                MinPercent = 0.275, // TODO add proper init values from somewhere
                MaxPercent = 0.275,
                Shift = Common.Enums.Shift.Morning
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
