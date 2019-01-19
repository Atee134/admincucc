using Ag.BusinessLogic.Interfaces;
using Ag.Domain;
using Ag.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ag.BusinessLogic.Services
{
    public class JoinTableHelperService : IJoinTableHelperService
    {
        private readonly AgDbContext _context;

        public JoinTableHelperService(AgDbContext context)
        {
            _context = context;
        }

        public List<User> GetColleagues(int userId)
        {
            var userRelations = _context.UserRelations.Where(r => r.FromId == userId || r.ToId == userId);
            var relatedUserIds = userRelations.Select(r => r.FromId == userId ? r.ToId : r.FromId).ToList();

            return _context.Users
                .Where(i => relatedUserIds.Contains(i.Id))
                .ToList();
        }
    }
}
