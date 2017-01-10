using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseLnk.Exceptions
{
    public class ExtraDataException : Exception
    {
        public ExtraDataException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
