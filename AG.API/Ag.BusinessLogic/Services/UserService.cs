using Ag.BusinessLogic.Exceptions;
using Ag.BusinessLogic.Interfaces;
using Ag.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ag.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly AgDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(AgDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddPerformer(int operatorId, int performerId)
        {
            _logger.LogInformation($"Assigning a model to operator. Operatr ID: {operatorId}, Model ID: {performerId}");

            var op = _context.Users.Include(u => u.Colleague).SingleOrDefault(u => u.Id == operatorId);

            if (op == null) throw new AgUnfulfillableActionException($"Operator with ID: {operatorId} does not exist.");
            if (op.Colleague != null) throw new AgUnfulfillableActionException($"Operator with ID: {operatorId} is already assigned to a model.");

            var performer = _context.Users.Include(u => u.Colleague).SingleOrDefault(u => u.Id == performerId);

            if (performer == null) throw new AgUnfulfillableActionException($"Model with ID: {operatorId} does not exist.");
            if (performer.Colleague != null) throw new AgUnfulfillableActionException($"Model with ID: {operatorId} is already assigned to an operator.");

            op.Colleague = performer;
            performer.Colleague = op;

            performer.Shift = op.Shift;
            performer.Sites = op.Sites;

            _context.SaveChanges();

            _logger.LogInformation($"Successfully assigned model with ID: {performerId}, to operator with ID: {operatorId}");
        }
    }
}
