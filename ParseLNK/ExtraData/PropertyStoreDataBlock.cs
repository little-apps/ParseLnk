using System.Diagnostics;
using System.IO;
using ParseLnk.Exceptions;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class PropertyStoreDataBlock : ExtraDataBase<Structs.PropertyStoreBlock>
    {
        public PropertyStoreDataBlock(Stream stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            if (Header.Size < 0x0000000C)
                throw new ExtraDataException("Header size is less than 0x0C", nameof(Header.Size));
        }

        public override void Read()
        {
            Body = new Structs.PropertyStoreBlock {PropertyStore = BodyBytes};
        }
    }
}
