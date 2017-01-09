using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using ParseLnk.ExtraData;
using ParseLnk.Interop;

namespace ParseLnk
{
    public class Parser
    {

        private StreamReader Stream { get; }

        public Structs.ShellLinkHeader ShellLinkHeader;
        public Structs.LinkTargetIDList LinkTargetIdList;
        public Structs.LinkInfo LinkInfo;
        public Structs.StringData StringData;
        public readonly Blocks ExtraData = new Blocks();

        public Parser(string filePath)
        {
            Stream = new StreamReader(filePath);
        }

        public Parser(StreamReader streamReader)
        {
            Stream = streamReader;
        }

        public void Parse()
        {
            Stream.BaseStream.Seek(0, SeekOrigin.Begin);

            ShellLinkHeader = Stream.ReadStruct<Structs.ShellLinkHeader>();

            Debug.Assert(ShellLinkHeader.HeaderSize == 0x4C, "ShellLinkHeader.HeaderSize does not equal 0x4C");
            Debug.Assert(ShellLinkHeader.LinkClsid.Equals(new Guid(Consts.LnkClsid)), "CLSID is not LNK CLSID");
            Debug.Assert(ShellLinkHeader.Reserved1 == 0, "Reserved fields must be 0");
            Debug.Assert(ShellLinkHeader.Reserved2 == 0, "Reserved fields must be 0");
            Debug.Assert(ShellLinkHeader.Reserved3 == 0, "Reserved fields must be 0");

            if (!Enum.IsDefined(typeof(Enums.ShowWindowCommands), ShellLinkHeader.ShowCommand))
                ShellLinkHeader.ShowCommand = Enums.ShowWindowCommands.Normal;
            
            Console.WriteLine("Creation Time: {0}", ShellLinkHeader.CreationTime.ToDateTime());
            Console.WriteLine("Access Time: {0}", ShellLinkHeader.AccessTime.ToDateTime());
            Console.WriteLine("Write Time: {0}", ShellLinkHeader.WriteTime.ToDateTime());
            Console.WriteLine("File Size: {0}", ShellLinkHeader.FileSize);

            if (ShellLinkHeader.LinkFlags.HasFlag(Enums.LinkFlags.HasLinkTargetIdList))
                ParseLinkTargetIdList();

            if (ShellLinkHeader.LinkFlags.HasFlag(Enums.LinkFlags.HasLinkInfo))
                ParseLinkInfo();

            ParseStringData();

            ParseExtraData();
        }
        
        private void ParseLinkTargetIdList()
        {
            LinkTargetIdList = new Structs.LinkTargetIDList
            {
                Size = Stream.ReadStruct<short>(),
                List = new Structs.IDList {ItemIDList = new List<Structs.ItemID>()}
            };
            
            var pos = 0;

            while (pos < LinkTargetIdList.Size - 2)
            {
                var itemId = new Structs.ItemID {Size = Stream.ReadStruct<ushort>()};

                itemId.Data = new byte[itemId.Size - 2];
                Stream.BaseStream.Read(itemId.Data, 0, itemId.Data.Length);

                LinkTargetIdList.List.ItemIDList.Add(itemId);

                pos += itemId.Size;
            }

            LinkTargetIdList.List.TerminalID = Stream.ReadStruct<ushort>();

            Debug.Assert(LinkTargetIdList.List.TerminalID == 0);
        }

        private void ParseLinkInfo()
        {
            LinkInfo = new Structs.LinkInfo {Header = Stream.ReadStruct<Structs.LinkInfoHeader>()};

            if (LinkInfo.Header.HeaderSize >= 0x24)
            {
                LinkInfo.HeaderOptional = Stream.ReadStruct<Structs.LinkInfoHeaderOptional>();
            }
            else
            {
                Debug.Assert(LinkInfo.Header.HeaderSize == 0x1C);
            }

            // Subtract all offsets that start at the beginning of LinkInfo from this
            var startOffset = LinkInfo.Header.HeaderSize;
            var linkInfoBody = new byte[LinkInfo.Header.Size - startOffset];

            Stream.BaseStream.Read(linkInfoBody, 0, linkInfoBody.Length);

            var pinnedBuffer = linkInfoBody.GetGCHandle();

            if (LinkInfo.Header.Flags.HasFlag(Enums.LinkInfoFlags.VolumeIdAndLocalBasePath))
            {
                LinkInfo.VolumeId = new Structs.LinkInfoVolumeId
                {
                    Header =
                        pinnedBuffer.ReadStruct<Structs.LinkInfoVolumeIdHeader>(LinkInfo.Header.VolumeIdOffset -
                                                                                startOffset)
                };

                Debug.Assert(LinkInfo.VolumeId.Header.Size > 0x10);
                Debug.Assert(LinkInfo.VolumeId.Header.VolumeLabelOffset < LinkInfo.VolumeId.Header.Size);
                Debug.Assert(LinkInfo.VolumeId.VolumeLabelOffsetUnicode < LinkInfo.VolumeId.Header.Size);

                if (LinkInfo.VolumeId.Header.VolumeLabelOffset == 0x14)
                {
                    LinkInfo.VolumeId.VolumeLabelOffsetUnicode =
                        pinnedBuffer.ReadStruct<uint>(
                            (uint) Marshal.OffsetOf<Structs.LinkInfoVolumeId>("VolumeLabelOffsetUnicode"));
                }

                if (LinkInfo.VolumeId.VolumeLabelOffsetUnicode > 0)
                    LinkInfo.VolumeId.Data =
                        Marshal.PtrToStringUni(IntPtr.Add(pinnedBuffer.AddrOfPinnedObject(),
                            (int) LinkInfo.VolumeId.VolumeLabelOffsetUnicode));
                else
                    LinkInfo.VolumeId.Data =
                        Marshal.PtrToStringAnsi(IntPtr.Add(pinnedBuffer.AddrOfPinnedObject(),
                            (int) LinkInfo.VolumeId.Header.VolumeLabelOffset));

                if (LinkInfo.HeaderOptional.LocalBasePathOffsetUnicode > 0)
                    LinkInfo.LocalBasePath =
                        Marshal.PtrToStringUni(IntPtr.Add(pinnedBuffer.AddrOfPinnedObject(),
                            (int) (LinkInfo.HeaderOptional.LocalBasePathOffsetUnicode - startOffset)));
                else
                    LinkInfo.LocalBasePath =
                        Marshal.PtrToStringAnsi(IntPtr.Add(pinnedBuffer.AddrOfPinnedObject(),
                            (int)(LinkInfo.Header.LocalBasePathOffset - startOffset)));

                if (LinkInfo.HeaderOptional.CommonPathSuffixOffsetUnicode > 0)
                    LinkInfo.CommonPathSuffix =
                        Marshal.PtrToStringUni(IntPtr.Add(pinnedBuffer.AddrOfPinnedObject(),
                            (int)(LinkInfo.HeaderOptional.CommonPathSuffixOffsetUnicode - startOffset)));
                else
                    LinkInfo.CommonPathSuffix =
                        Marshal.PtrToStringAnsi(IntPtr.Add(pinnedBuffer.AddrOfPinnedObject(),
                            (int)(LinkInfo.Header.CommonPathSuffixOffset - startOffset)));
            }

            if (LinkInfo.Header.Flags.HasFlag(Enums.LinkInfoFlags.CommonNetworkRelativeLinkAndPathSuffix))
            {
                var commonNetworkRelativeLinkStartOffset = LinkInfo.Header.CommonNetworkRelativeLinkOffset - startOffset;

                LinkInfo.CommonNetworkRelativeLink.Header =
                    pinnedBuffer.ReadStruct<Structs.CommonNetworkRelativeLinkHeader>(
                        commonNetworkRelativeLinkStartOffset);

                Debug.Assert(LinkInfo.CommonNetworkRelativeLink.Header.Size >= 0x14);

                LinkInfo.CommonNetworkRelativeLink.NetName =
                    Marshal.PtrToStringAnsi(IntPtr.Add(pinnedBuffer.AddrOfPinnedObject(),
                        (int) (commonNetworkRelativeLinkStartOffset +
                               LinkInfo.CommonNetworkRelativeLink.Header.NetNameOffset)));

                if (LinkInfo.CommonNetworkRelativeLink.Header.NetNameOffset > 0x14)
                {
                    LinkInfo.CommonNetworkRelativeLink.HeaderOptional =
                        pinnedBuffer.ReadStruct<Structs.CommonNetworkRelativeLinkHeaderOptional>(
                            (uint)
                            (commonNetworkRelativeLinkStartOffset +
                             Marshal.SizeOf<Structs.CommonNetworkRelativeLinkHeader>()));

                    LinkInfo.CommonNetworkRelativeLink.NetNameUnicode =
                        Marshal.PtrToStringUni(IntPtr.Add(pinnedBuffer.AddrOfPinnedObject(),
                            (int) (commonNetworkRelativeLinkStartOffset +
                                   LinkInfo.CommonNetworkRelativeLink.HeaderOptional.NetNameOffsetUnicode)));

                    if (LinkInfo.CommonNetworkRelativeLink.HeaderOptional.DeviceNameOffsetUnicode > 0)
                        LinkInfo.CommonNetworkRelativeLink.DeviceNameUnicode =
                            Marshal.PtrToStringUni(IntPtr.Add(pinnedBuffer.AddrOfPinnedObject(),
                                (int) (commonNetworkRelativeLinkStartOffset +
                                       LinkInfo.CommonNetworkRelativeLink.HeaderOptional.DeviceNameOffsetUnicode)));
                }

                if (
                    LinkInfo.CommonNetworkRelativeLink.Header.Flags.HasFlag(
                        Enums.CommonNetworkRelativeLinkFlags.ValidDevice))
                {
                    Debug.Assert(LinkInfo.CommonNetworkRelativeLink.Header.DeviceNameOffset > 0);

                    LinkInfo.CommonNetworkRelativeLink.DeviceName =
                        Marshal.PtrToStringAnsi(IntPtr.Add(pinnedBuffer.AddrOfPinnedObject(),
                            (int) (commonNetworkRelativeLinkStartOffset +
                                   LinkInfo.CommonNetworkRelativeLink.Header.DeviceNameOffset)));
                }
                else
                {
                    Debug.Assert(LinkInfo.CommonNetworkRelativeLink.Header.DeviceNameOffset == 0);
                }

                if (
                    LinkInfo.CommonNetworkRelativeLink.Header.Flags.HasFlag(
                        Enums.CommonNetworkRelativeLinkFlags.ValidNetType))
                {
                    Debug.Assert(Enum.IsDefined(typeof(Enums.NetworkProviderType),
                        LinkInfo.CommonNetworkRelativeLink.Header.NetProviderType));
                }
                else
                {
                    Debug.Assert(LinkInfo.CommonNetworkRelativeLink.Header.NetProviderType == 0);
                }
            }

            pinnedBuffer.Free();
        }

        private void ParseStringData()
        {
            if (ShellLinkHeader.LinkFlags.HasFlag(Enums.LinkFlags.HasName))
                StringData.NameString = ReadStringData();

            if (ShellLinkHeader.LinkFlags.HasFlag(Enums.LinkFlags.HasRelativePath))
                StringData.RelativePath = ReadStringData();

            if (ShellLinkHeader.LinkFlags.HasFlag(Enums.LinkFlags.HasWorkingDir))
                StringData.WorkingDir = ReadStringData();

            if (ShellLinkHeader.LinkFlags.HasFlag(Enums.LinkFlags.HasArguments))
                StringData.CommandLineArgs = ReadStringData();

            if (ShellLinkHeader.LinkFlags.HasFlag(Enums.LinkFlags.HasIconLocation))
                StringData.IconLocation = ReadStringData();
        }

        private string ReadStringData()
        {
            var sizeBuffer = new byte[2];
            Stream.BaseStream.Read(sizeBuffer, 0, 2);

            var pinnedBuffer = sizeBuffer.GetGCHandle();

            var size = (ushort) Marshal.ReadInt16(pinnedBuffer.AddrOfPinnedObject());

            pinnedBuffer.Free();

            if (size == 0)
                return string.Empty;

            if (ShellLinkHeader.LinkFlags.HasFlag(Enums.LinkFlags.IsUnicode))
            {
                var buffer = new byte[size * 2];
                Stream.BaseStream.Read(buffer, 0, size * 2);

                return Encoding.Unicode.GetString(buffer);
            }
            else
            {
                var buffer = new byte[size];
                Stream.BaseStream.Read(buffer, 0, size);

                return Encoding.Default.GetString(buffer);
            }
            
        }

        private void ParseExtraData()
        {
            if (Stream.BaseStream.Position == Stream.BaseStream.Length)
                return;

            while (!AtTerminalBlock())
            {
                Debug.Assert(Stream.BaseStream.Position != Stream.BaseStream.Length);

                ExtraData.ParseExtraData(Stream);
            }
        }

        private bool AtTerminalBlock()
        {
            if (Stream.BaseStream.Length - Stream.BaseStream.Position != 4)
                return false;

            var buffer = new byte[4];

            Stream.BaseStream.Read(buffer, 0, 4);

            if (BitConverter.ToUInt32(buffer, 0) < 0x00000004)
                return true;

            Stream.BaseStream.Seek(-4, SeekOrigin.Current);

            return false;
        }
    }
}
