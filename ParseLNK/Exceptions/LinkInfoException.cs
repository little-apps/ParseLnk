using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseLnk.Exceptions
{
    public class LinkInfoException : ExceptionBase
    {
        public LinkInfoException() : base("LinkInfo could not be parsed")
        {
            
        }

        public LinkInfoException(string message, Exception innerException, string fieldName) : base(message, innerException)
        {
            FieldName = fieldName;
        }
    }
}
