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
    public class Blocks
    {
        private static readonly Dictionary<uint, Type> SignatureBlocks = new Dictionary<uint, Type>
        {
            {0xA0000002, typeof(ConsoleDataBlock)},
            {0xA0000004, typeof(ConsoleFeDataBlock) },
            {0xA0000006, typeof(DarwinDataBlock) },
            {0xA0000001, typeof(EnvironmentVariableDataBlock) },
            {0xA0000007, typeof(IconEnvironmentDataBlock) },
            {0xA000000B, typeof(KnownFolderDataBlock) },
            {0xA0000009, typeof(PropertyStoreDataBlock) },
            {0xA0000008, typeof(ShimDataBlock) },
            {0xA0000005, typeof(SpecialFolderDataBlock) },
            {0xA0000003, typeof(TrackerDataBlock) },
            {0xA000000C, typeof(VistaAndAboveIdListDataBlock) }
        };

        public List<ConsoleDataBlock> ConsoleDataBlocks { get; } = new List<ConsoleDataBlock>();
        public List<ConsoleFeDataBlock> ConsoleFeDataBlocks { get; } = new List<ConsoleFeDataBlock>();
        public List<DarwinDataBlock> DarwinDataBlocks { get; } = new List<DarwinDataBlock>();
        public List<EnvironmentVariableDataBlock> EnvironmentVariableDataBlocks { get; } = new List<EnvironmentVariableDataBlock>();
        public List<IconEnvironmentDataBlock> IconEnvironmentDataBlocks { get; } = new List<IconEnvironmentDataBlock>();
        public List<KnownFolderDataBlock> KnownFolderDataBlocks { get; } = new List<KnownFolderDataBlock>();
        public List<PropertyStoreDataBlock> PropertyStoreDataBlocks { get; } = new List<PropertyStoreDataBlock>();
        public List<ShimDataBlock> ShimDataBlocks { get; } = new List<ShimDataBlock>();
        public List<SpecialFolderDataBlock> SpecialFolderDataBlocks { get; } = new List<SpecialFolderDataBlock>();
        public List<TrackerDataBlock> TrackerDataBlocks { get; } = new List<TrackerDataBlock>();
        public List<VistaAndAboveIdListDataBlock> VistaAndAboveIdListDataBlocks { get; } = new List<VistaAndAboveIdListDataBlock>();

        public Blocks()
        {
            
        }

        public void ParseExtraData(StreamReader stream)
        {
            var header = stream.ReadStruct<Structs.ExtraDataHeader>();

            Debug.Assert(SignatureBlocks.ContainsKey(header.Signature));
            Debug.Assert(SignatureBlocks[header.Signature].BaseType != null);
            Debug.Assert(SignatureBlocks[header.Signature].BaseType.GUID.Equals(typeof(ExtraDataBase<>).GUID));

            var type = SignatureBlocks[header.Signature];
            var extraData = Activator.CreateInstance(type, stream, header) as dynamic;
            
            extraData.Read();

            if (extraData is ConsoleDataBlock)
            {
                ConsoleDataBlocks.Add(extraData);
            }
            else if (extraData is ConsoleFeDataBlock)
            {
                ConsoleFeDataBlocks.Add(extraData);
            }
            else if (extraData is DarwinDataBlock)
            {
                DarwinDataBlocks.Add(extraData);
            }
            else if (extraData is EnvironmentVariableDataBlock)
            {
                EnvironmentVariableDataBlocks.Add(extraData);
            }
            else if (extraData is IconEnvironmentDataBlock)
            {
                IconEnvironmentDataBlocks.Add(extraData);
            }
            else if (extraData is KnownFolderDataBlock)
            {
                KnownFolderDataBlocks.Add(extraData);
            }
            else if (extraData is PropertyStoreDataBlock)
            {
                PropertyStoreDataBlocks.Add(extraData);
            }
            else if (extraData is ShimDataBlock)
            {
                ShimDataBlocks.Add(extraData);
            }
            else if (extraData is SpecialFolderDataBlock)
            {
                SpecialFolderDataBlocks.Add(extraData);
            }
            else if (extraData is TrackerDataBlock)
            {
                TrackerDataBlocks.Add(extraData);
            }
            else if (extraData is VistaAndAboveIdListDataBlock)
            {
                VistaAndAboveIdListDataBlocks.Add(extraData);
            }
        }
    }
}
