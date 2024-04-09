using Microsoft.EntityFrameworkCore;
using PersonalAccountEF.Data.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;


namespace PersonalAccountEF.Data
{
    public class PersonalAccountContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Resident> Residents { get; set; }
        public DbSet<PersonalAccount> PersonalAccounts { get; set; }


        public string DbPath { get; }

        public PersonalAccountContext()
        {
            var solutionPath = Path.Combine(Path.GetDirectoryName(typeof(PersonalAccountContext).Assembly.Location), "..", "..", "..", "..");
            DbPath = Path.Combine(solutionPath, "personalAccount.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonalAccount>().Navigation(e => e.Residents).AutoInclude();
            modelBuilder.Entity<PersonalAccount>().Navigation(e => e.Address).AutoInclude();
        }
    }
}
