using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discountapp.Infrastructure.Exceptions
{
    public class ApplicationCriticalException : Exception
    {
        public ApplicationCriticalException()
        {
            
        }

        public ApplicationCriticalException(string message)
            :base(message)
        {
            
        }
    }
}
