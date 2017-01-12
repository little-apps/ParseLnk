using System;

namespace ParseLnk.Exceptions
{
    public class LinkTargetIdList : ExceptionBase
    {
        public LinkTargetIdList(string message, string fieldName) : base(message)
        {
            FieldName = fieldName;
        }

        public LinkTargetIdList(string message, Exception innerException, string fieldName) : base(message, innerException)
        {
            FieldName = fieldName;
        }
    }
}
