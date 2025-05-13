    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCafeManagement.Domain.Entities
{
    public class EmployeeCafe
    {
        [Key]
        public string EmployeeId { get; set; }
        public Guid CafeId { get; set; }
        public DateTime? StartDate { get; set; }

        public Employee Employee { get; set; }
        public Cafe Cafe { get; set; }
    }
}
