using Ag.BusinessLogic.Interfaces;
using Ag.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ag.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly AgDbContext _context;

        public UserService(AgDbContext context)
        {
            _context = context;
        }

        public void AddPerformer(int operatorId, int performerId)
        {
            var op = _context.Users.Include(u => u.Colleague).SingleOrDefault(u => u.Id == operatorId);

            if (op == null || op.Colleague != null)
            {
                // TODO throw exception
                return;
            }

            var performer = _context.Users.Include(u => u.Colleague).SingleOrDefault(u => u.Id == performerId);

            if (performer == null || performer.Colleague != null)
            {
                // TODO throw exception
                return;
            }

            op.Colleague = performer;
            performer.Colleague = op;

            performer.Shift = op.Shift;
            performer.Sites = op.Sites;

            _context.SaveChanges();
        }
    }
}
