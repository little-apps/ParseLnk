using System;

namespace ParseLnk.Exceptions
{
    public class ExtraDataException : ExceptionBase
    {
        public ExtraDataException(string message, Exception innerException, string fieldName) : base(message, innerException)
        {
            FieldName = fieldName;
        }
    }
}
