using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabCrm.Service.ArchiveDataModels
{
    public class CommonFilter
    {
        public string? Cif { get; set; }

        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        public string RequestNumber { get; set; }
    }
}
