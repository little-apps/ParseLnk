using System.Diagnostics;
using System.IO;
using ParseLnk.Exceptions;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class SpecialFolderDataBlock : ExtraDataBase<Structs.SpecialFolderDataBlock>
    {
        public SpecialFolderDataBlock(Stream stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            if (Header.Size != 0x00000010)
                throw new ExtraDataException("Header size is not 0x10", nameof(Header.Size));
        }
        
    }
}
