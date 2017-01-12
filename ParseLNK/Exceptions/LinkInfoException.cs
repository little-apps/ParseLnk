using System;

namespace ParseLnk.Exceptions
{
    public class LinkInfoException : ExceptionBase
    {
        public LinkInfoException(string message, string fieldName) : base(message)
        {
            FieldName = fieldName;
        }

        public LinkInfoException(string message, Exception innerException, string fieldName) : base(message, innerException)
        {
            FieldName = fieldName;
        }
    }
}
