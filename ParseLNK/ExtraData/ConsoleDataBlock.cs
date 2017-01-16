using System.Diagnostics;
using System.IO;
using ParseLnk.Exceptions;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class ConsoleDataBlock : ExtraDataBase<Structs.ConsoleDataBlock>
    {
        public ConsoleDataBlock(Stream stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            if (Header.Size != 0x000000CC)
                throw new ExtraDataException("Header size is not 0xCC", nameof(Header.Size));
        }
    }
}
