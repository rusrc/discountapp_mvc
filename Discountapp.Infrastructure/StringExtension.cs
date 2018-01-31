using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discountapp.Infrastructure
{
    public static class StringExtension
    {
        public static string ControllerName(this String str )
        {
            return str.Replace("Controller", string.Empty);
        }
    }
}
