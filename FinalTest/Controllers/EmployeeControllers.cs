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
    public class EmployeeControllers:ControllerBase
    {
        private readonly ILogger<EmployeeControllers> _logger;
        private AplicationDbContext daf;
        public EmployeeControllers(AplicationDbContext arg, ILogger<EmployeeControllers> logger)
        {
            daf = arg;
            _logger = logger;
        }
        [HttpGet]
        [Route("List")]
        public async Task<List<ReturnModel1>> List(string namedep,bool ?isbusy)
        {
            _logger.LogInformation($"Request Content:namedep={namedep},isbusy={isbusy}");
            var model= daf.Employees.Join(daf.Departments, k => k.Department, g => g.Id, (k, g) => new ReturnModel1
            {
                Id = k.Id,
                Department = g.Name,
                Busy = k.Busy,
            });
            if (namedep!=null)
                model=model.Where(m => m.Department == namedep);
            if(isbusy.HasValue)
                model = model.Where(n => n.Busy == isbusy);
            var result= await model.ToListAsync();
            _logger.LogInformation($"Response content:result={result}");
            return result;
        }
        [HttpGet]
        [Route("IsBusy")]
        public async Task<IActionResult> IsBusy(Guid workerid)
        {
            _logger.LogInformation($"Request Content:workerid={workerid}");
            Employees zan = await daf.Employees.FirstOrDefaultAsync(x => x.Id == workerid);
            if (zan != null)
            {
                _logger.LogInformation($"Response content:{zan.Busy}");
                return Ok(zan.Busy);
            }
            else
            {
                _logger.LogError($"404| сотрудник с id={workerid} не найден");
                return NotFound($"сотрудник с id={workerid} не найден");
            }
        }
        [HttpPost]
        [Route("Assign")]
        public async Task<IActionResult> assign(DateTime questtime,Guid workerid)
        {
            _logger.LogInformation($"Request Content:questtime={questtime},workerid={workerid}");
            var data = daf.Employees.ToList();
            Employees zan = daf.Employees.FirstOrDefault(x => x.Id == workerid);
            if (zan!=null)
            {
                if (zan.Busy== false)
                {
                    var splitedtime = zan.Timespan.Split('-');
                    var parsedDate1 = TimeSpan.Parse(splitedtime[0]);
                    var parsedDate2 = TimeSpan.Parse(splitedtime[1]);
                    if ((questtime.TimeOfDay >= parsedDate1) && (questtime.TimeOfDay <= parsedDate2))
                    {
                        zan.Busy = true;
                        await daf.SaveChangesAsync();
                        _logger.LogInformation($"Response content");
                        return Ok();
                    }
                    else
                    {
                        _logger.LogError("500|Не рабочее время сотрудника");
                        return StatusCode(500, "Не рабочее время сотрудника");
                    }
                    
                }
                else
                {
                    _logger.LogError("500| В выбранное время сотрудник занят");
                    return StatusCode(500, "В выбранное время сотрудник занят");
                }
            }
            else
            {
                _logger.LogError("404|Данный сотрудник не найден");
                return NotFound("Данный сотрудник не найден");
            }
        }
    }
}
