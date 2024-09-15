using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAPI.Models
{
    public class UserJobInfo
    {
        public int UserId { get; set; }
        public string JobTitle { get; set; } = "";
        public string Department { get; set; } = "";
    }
}
