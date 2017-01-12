using System;

namespace ParseLnk.Exceptions
{
    public class LinkTargetIdListException : ExceptionBase
    {
        public LinkTargetIdListException(string message, string fieldName) : base(message)
        {
            FieldName = fieldName;
        }

        public LinkTargetIdListException(string message, Exception innerException, string fieldName) : base(message, innerException)
        {
            FieldName = fieldName;
        }
    }
}
