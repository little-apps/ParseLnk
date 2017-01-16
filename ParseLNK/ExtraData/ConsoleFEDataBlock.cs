using System.Diagnostics;
using System.IO;
using ParseLnk.Exceptions;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class ConsoleFeDataBlock : ExtraDataBase<Structs.ConsoleFeDataBlock>
    {
        public ConsoleFeDataBlock(Stream stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            if (Header.Size != 0x0000000C)
                throw new ExtraDataException("Header size is not 0x0C", nameof(Header.Size));
        }
        
    }
}
