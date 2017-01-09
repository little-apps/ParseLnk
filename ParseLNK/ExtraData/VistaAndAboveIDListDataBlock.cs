using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class VistaAndAboveIdListDataBlock : ExtraDataBase<Structs.VistaAndAboveIdListDataBlock>
    {
        public VistaAndAboveIdListDataBlock(StreamReader stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            Debug.Assert(Header.Size >= 0x0000000A);
        }

        public override void Read()
        {
            var idList = new Structs.IDList {ItemIDList = new List<Structs.ItemID>(), TerminalID = 1};
            
            var pos = 0;

            while (pos < BodyLength)
            {
                var itemId = new Structs.ItemID { Size = Stream.ReadStruct<ushort>() };

                itemId.Data = new byte[itemId.Size - 2];
                Stream.BaseStream.Read(itemId.Data, 0, itemId.Data.Length);

                idList.ItemIDList.Add(itemId);

                pos += itemId.Size;
            }

            idList.TerminalID = Stream.ReadStruct<ushort>();

            Body = new Structs.VistaAndAboveIdListDataBlock {IdList = idList};
            Debug.Assert(Body.IdList.TerminalID == 0);
        }
    }
}
