using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCafeManagement.Domain.DTO
{
    public class EmployeeCafeDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string EmailAddress { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public int DaysWorked { get; set; }
        public string Cafe { get; set; } = default!;
        public string Gender { get; set; }
        public DateTime? StartDate { get; set; }
    }

}
