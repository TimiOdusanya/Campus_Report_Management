using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportManagement.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Complaint> Complaints { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
