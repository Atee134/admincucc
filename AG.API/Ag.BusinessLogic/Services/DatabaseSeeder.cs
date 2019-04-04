using Ag.BusinessLogic.Interfaces;
using Ag.Common.Dtos.Request;
using Ag.Common.Enums;
using Ag.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ag.BusinessLogic.Services
{
    public class DatabaseSeeder : IApplicationInitializer
    {
        private static readonly Random _random = new Random();
        private const int TEST_INCOMES_COUNT = 100;
        private readonly AgDbContext _context;
        private readonly IAuthService _authService;
        private readonly IIncomeService _incomeService;
        private readonly IUserService _userService;
        private readonly IHostingEnvironment environment;
        private readonly ILogger<DatabaseSeeder> logger;

        public DatabaseSeeder(AgDbContext context, IAuthService authService, IIncomeService incomeService, IUserService userService, IHostingEnvironment environment, ILogger<DatabaseSeeder> logger)
        {
            _context = context;
            _authService = authService;
            _incomeService = incomeService;
            _userService = userService;
            this.environment = environment;
            this.logger = logger;
        }

        public void Start()
        {
            bool dbExists = (_context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();

            logger.LogInformation("Migrating database...");
            _context.Database.Migrate();

            if (!dbExists && environment.IsDevelopment())
            {
                SeedTestData();
            }

            InsertInitialAdmin();
        }

        private void InsertInitialAdmin()
        {
            logger.LogInformation("Checking for initial admin add..");
            if (_context.Users.Count() == 0)
            {
                logger.LogInformation("No users in the database, adding a default admin...");

                _authService.Register(new UserForRegisterDto()
                {
                    UserName = "dontusethisadmin",
                    Password = "qay789edc123",
                    Role = Role.Admin
                });

                _authService.Register(new UserForRegisterDto()
                {
                    UserName = "Solo",
                    Password = "qay789edc123",
                    Role = Role.Performer
                });

                _context.SaveChanges();
            }
            else
            {
                logger.LogInformation("There is already user in the database, no action taken.");
            }
        }

        private void SeedTestData()
        {
            _authService.Register(new UserForRegisterDto()
            {
                UserName = "TestOperator",
                Password = "1234",
                Role = Role.Operator
            });

            _authService.Register(new UserForRegisterDto()
            {
                UserName = "TestPerformer",
                Password = "1234",
                Role = Role.Performer
            });

            _authService.Register(new UserForRegisterDto()
            {
                UserName = "TestAdmin",
                Password = "1234",
                Role = Role.Admin
            });

            _context.SaveChanges();

            var opi = _context.Users.Find(1);

            opi.Sites = String.Join(';', new List<string> { Site.CB.ToString(), Site.LJ.ToString(), Site.MFC.ToString() });

            _userService.AddPerformer(1, 2);

            for (int i = 0; i < TEST_INCOMES_COUNT; i++)
            {
                _incomeService.AddIncomEntry(1, CreateRandomIncomeEntry(i));
                //_incomeService.AddIncomEntry(1, CreateConstantIncomeEntry(i));
            }

            _context.SaveChanges();
        }

        private IncomeEntryAddDto CreateConstantIncomeEntry(int dayOffset)
        {
            var date = DateTime.Now.AddDays(dayOffset * (-1));

            return new IncomeEntryAddDto()
            {
                Date = date,
                PerformerId = 2,
                IncomeChunks = new List<IncomeChunkAddDto>()
            {
                new IncomeChunkAddDto { Site = Site.CB, Income = 1},
                new IncomeChunkAddDto { Site = Site.LJ, Income = 2},
                new IncomeChunkAddDto { Site = Site.MFC, Income = 3},
            }
            };
        }

        private IncomeEntryAddDto CreateRandomIncomeEntry(int dayOffset)
        {
            var date = DateTime.Now.AddDays(dayOffset * (-1));

            if (dayOffset > 10)
            {
                return new IncomeEntryAddDto()
                {
                    Date = date,
                    PerformerId = 2,
                    IncomeChunks = new List<IncomeChunkAddDto>()
                {
                    new IncomeChunkAddDto { Site = Site.CB, Income = _random.NextDouble() * 100},
                    new IncomeChunkAddDto { Site = Site.LJ, Income = _random.NextDouble() * 100},
                    new IncomeChunkAddDto { Site = Site.MFC, Income = _random.NextDouble() * 100},
                }
                };
            }
            else
            {
                return new IncomeEntryAddDto()
                {
                    Date = date,
                    PerformerId = 2,
                    IncomeChunks = new List<IncomeChunkAddDto>()
                {
                    new IncomeChunkAddDto { Site = Site.CB, Income = _random.NextDouble() * 100},
                    new IncomeChunkAddDto { Site = Site.MFC, Income = _random.NextDouble() * 100},
                }
                };
            }
          
        }
    }
}
