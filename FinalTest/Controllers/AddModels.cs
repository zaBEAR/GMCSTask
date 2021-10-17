using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalTest.Controllers
{
    public class AddModels
    {
        public Guid Id { get; set; }
        public string Department { get; set; }
        public string TimeSpan { get; set; }
        public bool Busy { get; set; }
    }
    public class ResultCheck
    {
        public Guid Id { get; set; }
        public string Timespan { get; set; }
    }
    public class ReturnModel1
    {
        public Guid Id { get; set; }
        public string Department { get; set; }
        public bool Busy { get; set; }
    }
    public class ReturnModel2
    {
        public Guid Id { get; set; }
        public bool Busy { get; set; }
    }
}
