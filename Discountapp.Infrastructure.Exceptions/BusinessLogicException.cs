﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discountapp.Infrastructure.Exceptions
{
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException()
        {
            
        }
        public BusinessLogicException(string message)
            :base(message)
        {

        }
    }
}
