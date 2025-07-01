using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class LogDto
    {
        public DateTime CreateDate { get; set; }
        public string Method { get; set; }
        public string Exception { get; set; }
        public string Trace { get; set; }
        public int StatusCode { get; set; }
    }
}
