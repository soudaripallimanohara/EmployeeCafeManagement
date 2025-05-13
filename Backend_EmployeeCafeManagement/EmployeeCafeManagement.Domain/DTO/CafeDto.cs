using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCafeManagement.Domain.DTO
{
    public class CafeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public string Logo { get; set; }

        public string Description { get; set; }
        public string Location { get; set; }
    }
}
