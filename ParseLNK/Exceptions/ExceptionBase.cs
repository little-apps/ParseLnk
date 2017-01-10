using System;

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
