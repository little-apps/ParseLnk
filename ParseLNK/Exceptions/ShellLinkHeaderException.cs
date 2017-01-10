using System;

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
