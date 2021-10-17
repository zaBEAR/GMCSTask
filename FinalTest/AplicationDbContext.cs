using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinalTest.Models;

namespace FinalTest
{
    
        public class AplicationDbContext : DbContext
        {
            public DbSet<Departments> Departments { get; set; }
            public DbSet<Employees> Employees { get; set; }
            public AplicationDbContext()
            {

                Database.EnsureCreated();

            }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(@"Server=localhost;Database=final;Trusted_Connection=True");

            }
        }
    
}

