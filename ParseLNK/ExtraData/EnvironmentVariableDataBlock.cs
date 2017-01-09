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
    public class EnvironmentVariableDataBlock : ExtraDataBase<Structs.EnvironmentVariableDataBlock>
    {
        public EnvironmentVariableDataBlock(StreamReader stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            Debug.Assert(Header.Size == 0x00000314);
        }
    }
}
