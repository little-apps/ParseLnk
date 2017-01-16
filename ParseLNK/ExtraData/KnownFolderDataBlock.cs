using System.Diagnostics;
using System.IO;
using ParseLnk.Exceptions;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class KnownFolderDataBlock : ExtraDataBase<Structs.KnownFolderDataBlock>
    {
        public KnownFolderDataBlock(Stream stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            if (Header.Size != 0x0000001C)
                throw new ExtraDataException("Header size is not 0x1C", nameof(Header.Size));
        }
    }
}
