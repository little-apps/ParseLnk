﻿using System.Diagnostics;
using System.IO;
using System.Text;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class ShimDataBlock : ExtraDataBase<Structs.ShimDataBlock>
    {
        public ShimDataBlock(Stream stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            Debug.Assert(Header.Size >= 0x00000088);
        }

        public override void Read()
        {
            Body = new Structs.ShimDataBlock {LayerName = Encoding.Unicode.GetString(BodyBytes)};
        }
    }
}
