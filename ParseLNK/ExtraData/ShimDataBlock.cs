using System.Diagnostics;
using System.IO;
using System.Text;
using ParseLnk.Exceptions;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class ShimDataBlock : ExtraDataBase<Structs.ShimDataBlock>
    {
        public ShimDataBlock(Stream stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            if (Header.Size < 0x00000088)
                throw new ExtraDataException("Header size is less than 0x88", nameof(Header.Size));
        }

        public override void Read()
        {
            Body = new Structs.ShimDataBlock {LayerName = Encoding.Unicode.GetString(BodyBytes)};
        }
    }
}
