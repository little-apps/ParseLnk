using System;

namespace ParseLnk.Exceptions
{
    public class ExtraDataException : ExceptionBase
    {
        public ExtraDataException(string message, string fieldName) : base(message)
        {
            FieldName = fieldName;
        }

        public ExtraDataException(string message, Exception innerException, string fieldName) : base(message, innerException)
        {
            FieldName = fieldName;
        }
    }
}
