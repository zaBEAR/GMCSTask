using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FinalTest.Models;
using FinalTest;

namespace FinalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentControllers: ControllerBase
    {
        private readonly ILogger<DepartmentControllers> _logger;
        private AplicationDbContext daf;
        public DepartmentControllers(AplicationDbContext arg,ILogger<DepartmentControllers> logger)
        {
            daf = arg;
            _logger = logger;
        }
        [HttpPost]
        [Route("Assign")]
        public async Task<IActionResult> assign(DateTime questtime,string NameDep)
        {
            _logger.LogInformation($"Request Content:questtime={questtime},NameDep={NameDep}");
            try
            {
                if (NameDep == null)
                {
                    _logger.LogError($"500|");
                    return StatusCode(500);
                }
                var model = daf.Employees.Join(daf.Departments, k => k.Department, g => g.Id, (k, g) => new AddModels
                {
                    Id = k.Id,
                    Department = g.Name,
                    TimeSpan = k.Timespan,
                    Busy = k.Busy,
                }).Where(d => d.Department == NameDep && d.Busy == false);
                var result = new List<ResultCheck>();
                foreach (var k in model)
                {
                    var splitedtime = k.TimeSpan.Split('-');
                    var parsedDate1 = TimeSpan.Parse(splitedtime[0]);
                    var parsedDate2 = TimeSpan.Parse(splitedtime[1]);
                    if ((questtime.TimeOfDay > parsedDate1) && (questtime.TimeOfDay < parsedDate2))
                    {
                        result.Add(new ResultCheck() { Id = k.Id, Timespan = k.TimeSpan });
                    }
                }
                if (result == null)
                {
                    _logger.LogError("404|подходящий сотрудник не найден");
                    return NotFound("подходящий сотрудник не найден");
                }
                var availableEmployee = result.OrderBy(r => r.Timespan).First();
                var foundedEmployee = daf.Employees.First(m => m.Id == availableEmployee.Id);
                foundedEmployee.Busy = true;
                await daf.SaveChangesAsync();
                _logger.LogInformation("Response content");
                return Ok();
            }
            catch(Exception e)
            {
                _logger.LogError($"500|{e.Message}");
                return StatusCode(500, new { message = e.Message, stackTrace = e.StackTrace });
            }
        }
        
    }
    
}   
            

