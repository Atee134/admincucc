using Ag.BusinessLogic.Interfaces;
using Ag.Domain;
using Ag.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.BusinessLogic.Services
{
    public class DatabaseSeeder : IApplicationInitializer
    {
        private readonly AgDbContext _context;
        private readonly IAuthService _authService;
        private readonly IIncomeService _incomeService;

        public DatabaseSeeder(AgDbContext context, IAuthService authService, IIncomeService incomeService)
        {
            _context = context;
            _authService = authService;
            _incomeService = incomeService;
        }

        public void Start()
        {
            bool dbExists = (_context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();

            _context.Database.Migrate();

            if (!dbExists) // TODO add check whether we're in dev or prod
            {
                SeedTestData();
            }
        }

        private void SeedTestData()
        {
            _authService.Register(new Common.Dtos.Request.UserForRegisterDto()
            {
                UserName = "TestOperator",
                Password = "1234",
                Role = Common.Enums.Role.Operator
            });

            _authService.Register(new Common.Dtos.Request.UserForRegisterDto()
            {
                UserName = "TestPerformer",
                Password = "1234",
                Role = Common.Enums.Role.Performer
            });

            _authService.Register(new Common.Dtos.Request.UserForRegisterDto()
            {
                UserName = "TestAdmin",
                Password = "1234",
                Role = Common.Enums.Role.Admin
            });

            _context.SaveChanges();
        }
    }
}
