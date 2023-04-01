using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReportManagement.Models
{
    public class Complaint
    {
            [Key]
            public int Id { get; set; }
            [Required]
            public string Name { get; set; }

            public string ShortDesc { get; set; }
            public string Description { get; set; }
            
            public string Date { get; set; }
            public string Time { get; set; }
            public string Location { get; set; }
            public string Image { get; set; }

            [Display(Name = "Category Type")]
            public int CategoryId { get; set; }
            [ForeignKey("CategoryId")]
            public virtual Category Category { get; set; }

       
    }
}
