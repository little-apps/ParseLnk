using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseLnk.Exceptions
{
    public class ShellLinkHeaderException : Exception
    {
        public ShellLinkHeaderException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
