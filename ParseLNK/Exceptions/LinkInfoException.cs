using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseLnk.Exceptions
{
    public class LinkInfoException : Exception
    {
        public LinkInfoException() : base("LinkInfo could not be parsed")
        {
            
        }
    }
}
