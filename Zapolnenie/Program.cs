using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Zapolnenie.Models;

namespace Zapolnenie
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await GenerateDepartment();           
            await GenerateEmployee();            
        }
        static async Task GenerateDepartment()
        {
            using AplicationDbContext context = new AplicationDbContext();
            string[] names = { "management", "development", "analytics" };
            foreach (string k in names)
            {
                Departments Created = new Departments { Id = Guid.NewGuid(), Name = k };
                context.Departments.Add(Created);
                await context.SaveChangesAsync();
            }
        }
        static async Task GenerateEmployee()
        {
            Random rnd = new Random();
            using AplicationDbContext context = new AplicationDbContext();
            var saveDepartment = await context.Departments.ToListAsync();
            string[] WorkTime = { "08:00-17:00", "09:00-18:00", "10:00-19:00" };
            for (int i = 0; i < 100; i++)
            {
                Employees Created = new Employees
                {
                    Id = Guid.NewGuid(),
                    Department = saveDepartment[rnd.Next(0, 3)].Id,
                    Timespan = WorkTime[rnd.Next(0, 3)],
                    Busy = false
                };
                context.Employees.Add(Created);               
            }
            await context.SaveChangesAsync();
        }
    }
}

