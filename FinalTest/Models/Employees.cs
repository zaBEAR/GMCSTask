using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalTest.Models
{
    public class Employees
    {
        public Guid Id { get; set; }
        public Guid Department { get; set; }
        public string Timespan { get; set; }
        public bool Busy { get; set; }
    }
}
