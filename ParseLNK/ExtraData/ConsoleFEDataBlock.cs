using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class ConsoleFeDataBlock : ExtraDataBase<Structs.ConsoleFeDataBlock>
    {
        public ConsoleFeDataBlock(StreamReader stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            Debug.Assert(Header.Size == 0x0000000C);
        }
        
    }
}
