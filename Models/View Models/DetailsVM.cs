using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportManagement.Models.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            Complaint = new Complaint();
        }
        public Complaint Complaint { get; set; }
        public bool ExistsInCart { get; set; }
    }
}
