using Ag.BusinessLogic.Exceptions;
using Ag.BusinessLogic.Interfaces;
using Ag.BusinessLogic.Interfaces.Converters;
using Ag.Common.Dtos.Request;
using Ag.Common.Dtos.Response;
using Ag.Common.Enums;
using Ag.Domain;
using Ag.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ag.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly AgDbContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IUserConverter _userConverter;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public UserService(AgDbContext context, ILogger<UserService> logger, IUserConverter userConverter, IAuthService authService, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _userConverter = userConverter;
            _authService = authService;
            _configuration = configuration;
        }

        public UserDetailDto GetUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null) throw new AgUnfulfillableActionException($"User with ID: {userId} does not exist.");

            return _userConverter.ConvertToUserDetailDto(user);
        }

        public IEnumerable<UserForListDto> GetUsers(Role? role = null) // TODO add filters later
        {
            _logger.LogInformation("Getting user list...");

            if (role != null)
            {
                return _context.Users.Where(u => u.Role == role).Select(u => _userConverter.ConvertToUserToListDto(u));
            }
            else
            {
               return _context.Users.Select(u => _userConverter.ConvertToUserToListDto(u));    
            }
        }

        public void UpdateUser(UserForEditDto userDto)
        {
            _logger.LogInformation($"Updating user with ID: {userDto.Id}");

            var user = _context.Users.SingleOrDefault(u => u.Id == userDto.Id);

            if (user == null) throw new AgUnfulfillableActionException($"User with ID: {userDto.Id} does not exist.");

            user.Sites = String.Join(';', userDto.Sites);
            user.MinPercent = userDto.MinPercent.Value;
            user.MaxPercent = userDto.MaxPercent.Value;

            _context.SaveChanges();

            _logger.LogInformation($"Updating user with ID: {userDto.Id} was successful.");

            if (!String.IsNullOrEmpty(userDto.Password))
            {
                _authService.ChangeUserPassword(user, userDto.Password);
            }
        }

        public void AddPerformer(int operatorId, int performerId)
        {
            if (operatorId == performerId) throw new AgUnfulfillableActionException("Can not add relation to self.");

            if (_context.UserRelations.SingleOrDefault(r => (r.FromId == operatorId && r.ToId == performerId) || (r.FromId == performerId && r.ToId == operatorId)) != null) throw new AgUnfulfillableActionException("Model is already assigned to Operator");

            _logger.LogInformation($"Assigning a model to operator. Operator ID: {operatorId}, Model ID: {performerId}");

            var op = _context.Users.SingleOrDefault(u => u.Id == operatorId && u.Role == Role.Operator);

            if (op == null) throw new AgUnfulfillableActionException($"Operator with ID: {operatorId} does not exist.");

            var performer = _context.Users.SingleOrDefault(u => u.Id == performerId && u.Role == Role.Performer);

            if (performer == null) throw new AgUnfulfillableActionException($"Model with ID: {performerId} does not exist.");

            var defaultColor = _configuration.GetSection("UserColors:0").Value;

            UserRelation relation = new UserRelation
            {
                FromId = operatorId,
                ToId = performerId,
                UserFrom = op,
                UserTo = performer,
                Color = defaultColor
            };

            _context.UserRelations.Add(relation);

            // HUGE TODO HERE, consider what the hell will happen with these? performers shouldnt even be assigned sites, they are fine on their own, but shift... op must have more shifts, each assigned to a performer, so the relation should contain shift?!
            performer.Shift = op.Shift; //placeholders for now
            performer.Sites = op.Sites; //placeholders for now

            _context.SaveChanges();

            _logger.LogInformation($"Successfully assigned Model with ID: {performerId} to Operator with ID: {operatorId}.");
        }

        public void RemovePerformer(int operatorId, int performerId)
        {
            if (operatorId == performerId) throw new AgUnfulfillableActionException("Can not remove performer, both IDs are the same.");

            var userRelation = _context.UserRelations.SingleOrDefault(r => (r.FromId == operatorId && r.ToId == performerId) || (r.FromId == performerId && r.ToId == operatorId));

            if (userRelation == null) throw new AgUnfulfillableActionException("Performer is not assigned to Operator");

            _logger.LogInformation($"Removing relation between users, Operator ID: {operatorId}, Model ID: {performerId}");

            _context.UserRelations.Remove(userRelation);

            _context.SaveChanges();

            _logger.LogInformation($"Successfully removed relation between users, Operator ID: {operatorId}, Model ID: {performerId}");
        }

        public void ChangeColor(int operatorId, int performerId, string color)
        {
            if (operatorId == performerId) throw new AgUnfulfillableActionException("Can not change color. Both ID's are the same.");

            var userRelation = _context.UserRelations.SingleOrDefault(r => (r.FromId == operatorId && r.ToId == performerId) || (r.FromId == performerId && r.ToId == operatorId));

            if (userRelation == null) throw new AgUnfulfillableActionException("Performer is not assigned to Operator");

            _logger.LogInformation($"Changing color of connection between operator ID: {operatorId}, performer ID: {performerId}, old color: {userRelation.Color}, new color: {color}");

            userRelation.Color = color;

            _context.SaveChanges();
        }

        public string GetColor(int operatorId, int performerId)
        {
            if (operatorId == performerId) throw new AgUnfulfillableActionException("Can not get color. Both ID's are the same.");

            var userRelation = _context.UserRelations.SingleOrDefault(r => (r.FromId == operatorId && r.ToId == performerId) || (r.FromId == performerId && r.ToId == operatorId));

            if (userRelation == null) throw new AgUnfulfillableActionException("Performer is not assigned to Operator");

            return userRelation.Color;
        }
    }
}
