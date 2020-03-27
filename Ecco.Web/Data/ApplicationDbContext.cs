using System;
using System.Collections.Generic;
using System.Text;
using Ecco.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecco.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Connection> Connections { get; set; }
        public DbSet<Card> Cards { get; set; }
    }
}