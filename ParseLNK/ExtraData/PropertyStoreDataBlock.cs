﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class PropertyStoreDataBlock : ExtraDataBase<Structs.PropertyStoreBlock>
    {
        public PropertyStoreDataBlock(StreamReader stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            Debug.Assert(Header.Size >= 0x0000000C);
        }

        public override void Read()
        {
            Body = new Structs.PropertyStoreBlock {PropertyStore = BodyBytes};
        }
    }
}
