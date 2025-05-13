using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCafeManagement.Application.Middleware.GlobalExceptions
{
    public class IdAlreadyExistException : Exception
    {
        public IdAlreadyExistException(string message) : base(message)
        {
        }
    }
}
