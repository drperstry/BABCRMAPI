using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bab.Jobs.Models
{
    public class RequestData
    {
        public DateTime dueDate { get; set; }

        public Guid Id { get; set; }
        
        public string logicalName { get; set; }
    }
}
