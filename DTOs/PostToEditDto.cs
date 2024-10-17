using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAPI.DTOs
{
    public class PostToEditDto
    {
        public int PostId { get; set; }
        public string PostTitle { get; set; } = "";
        public string PostContent { get; set; } = "";
    }
}
