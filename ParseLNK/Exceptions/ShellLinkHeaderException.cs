using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseLnk.Exceptions
{
    public class ShellLinkHeaderException : ExceptionBase
    {
        public ShellLinkHeaderException(string message, Exception innerException, string fieldName) : base(message, innerException)
        {
            FieldName = fieldName;
        }
    }
}
