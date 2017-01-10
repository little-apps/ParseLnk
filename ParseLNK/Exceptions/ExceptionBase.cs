using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ParseLnk.Exceptions
{
    public class ExceptionBase : Exception
    {
        public string FieldName { get; protected set; }

        public ExceptionBase()
        {
        }

        protected ExceptionBase(string message) : base(message)
        {
        }

        protected ExceptionBase(string message, Exception innerException) : base(message, innerException)
        {
        }
        
    }
}
