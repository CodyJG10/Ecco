using System;
using System.Collections.Generic;
using System.Text;
using Ecco.Entities;
using Ecco.Entities.Company;
using Ecco.Web.Areas.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecco.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<EccoUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Connection> Connections { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Template> Templates { get; set; }

        #region Company System

        public DbSet<Company> Companies { get; set; }
        public DbSet<EmployeeInvitation> EmployeeInvitations { get; set; }

        #endregion
    }
}