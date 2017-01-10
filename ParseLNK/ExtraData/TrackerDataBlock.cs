using System.Diagnostics;
using System.IO;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class TrackerDataBlock : ExtraDataBase<Structs.TrackerDataBlock>
    {
        public TrackerDataBlock(StreamReader stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            Debug.Assert(Header.Size == 0x00000060);
        }

        public override void Read()
        {
            base.Read();

            Debug.Assert(Body.Length >= 0x0000058);
            Debug.Assert(Body.Version == 0);
        }
    }
}
