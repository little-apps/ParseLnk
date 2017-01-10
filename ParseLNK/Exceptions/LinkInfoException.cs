using System;

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
