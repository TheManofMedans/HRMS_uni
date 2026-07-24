using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) 
        { 

        }
        public NotFoundException(string entityName, Object key) : base($"{entityName} with id {key} was not found.")
        {

        }
    }
}
