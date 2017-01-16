using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ParseLnk.Exceptions;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class VistaAndAboveIdListDataBlock : ExtraDataBase<Structs.VistaAndAboveIdListDataBlock>
    {
        public VistaAndAboveIdListDataBlock(Stream stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            if (Header.Size < 0x0000000A)
                throw new ExtraDataException("Header size is less than 0x0A", nameof(Header.Size));
        }

        public override void Read()
        {
            var idList = new Structs.IDList {ItemIDList = new List<Structs.ItemID>(), TerminalID = 1};
            
            var pos = 0;

            while (pos < BodyLength)
            {
                var itemId = new Structs.ItemID { Size = Stream.ReadStruct<ushort>() };

                itemId.Data = new byte[itemId.Size - 2];
                Stream.Read(itemId.Data, 0, itemId.Data.Length);

                idList.ItemIDList.Add(itemId);

                pos += itemId.Size;
            }

            idList.TerminalID = Stream.ReadStruct<ushort>();

            Body = new Structs.VistaAndAboveIdListDataBlock {IdList = idList};
            if (Body.IdList.TerminalID != 0)
                throw new ExtraDataException("Terminal ID is not 0", nameof(Body.IdList.TerminalID));
        }
    }
}
