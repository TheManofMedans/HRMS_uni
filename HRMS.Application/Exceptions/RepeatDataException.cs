using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Exceptions
{
    public class RepeatDataException : Exception
    {
        public RepeatDataException(string message) : base (message) 
        {

        }
        public RepeatDataException(string fieldName, Object key) : base($"{fieldName} with the content {key} already exists.")
        { 
        }
    }
}
