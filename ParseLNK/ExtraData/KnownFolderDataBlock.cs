using System.Diagnostics;
using System.IO;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class KnownFolderDataBlock : ExtraDataBase<Structs.KnownFolderDataBlock>
    {
        public KnownFolderDataBlock(Stream stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            Debug.Assert(Header.Size == 0x0000001C);
        }
    }
}
