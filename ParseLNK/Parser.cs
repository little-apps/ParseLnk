using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using ParseLnk.Exceptions;
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
        public Blocks ExtraData = new Blocks();

        public Parser(string filePath)
        {
            Stream = new StreamReader(filePath);
        }

        public Parser(StreamReader streamReader)
        {
            Stream = streamReader;
        }

        /// <summary>
        /// Parses the file as LNK format
        /// </summary>
        /// <exception cref="ShellLinkHeaderException">Thrown if the ShellLinkHeader is not formatted properly</exception>
        /// <exception cref="LinkTargetIdListException">Thrown if the LinkTargetIdList can't be parsed</exception>
        /// <exception cref="LinkInfoException">Thrown if the LinkInfo can't be parsed</exception>
        /// <exception cref="ExtraDataException">Thrown if the ExtraData can't be parsed</exception>
        /// <example>
        /// try {
        ///     parser.Parse();
        /// } catch (ParseLnk.Exceptions.ExceptionBase) {
        ///     // Couldnt be parsed
        /// }
        /// </example>
        public void Parse()
        {
            Reset();

            ParseShellLinkHeader();

            if (ShellLinkHeader.LinkFlags.HasFlag(Enums.LinkFlags.HasLinkTargetIdList))
                ParseLinkTargetIdList();

            if (ShellLinkHeader.LinkFlags.HasFlag(Enums.LinkFlags.HasLinkInfo))
                ParseLinkInfo();

            ParseStringData();

            ParseExtraData();
        }

        /// <summary>
        /// Resets the Stream position to the beginning and clears the fields
        /// </summary>
        private void Reset()
        {
            Stream.BaseStream.Seek(0, SeekOrigin.Begin);

            ShellLinkHeader = new Structs.ShellLinkHeader();
            LinkTargetIdList = new Structs.LinkTargetIDList();
            LinkInfo = new Structs.LinkInfo();
            StringData = new Structs.StringData();
            ExtraData = new Blocks();
        }

        /// <summary>
        /// Parses the ShellLinkHeader
        /// </summary>
        /// <exception cref="ShellLinkHeaderException">Thrown if ShellLinkHeader is not valid</exception>
        private void ParseShellLinkHeader()
        {
            ShellLinkHeader = Stream.ReadStruct<Structs.ShellLinkHeader>();

            if (ShellLinkHeader.HeaderSize != 0x4C)
                throw new ShellLinkHeaderException("ShellLinkHeader.HeaderSize does not equal 0x4C",
                    nameof(ShellLinkHeader.HeaderSize));

            if (!ShellLinkHeader.LinkClsid.Equals(new Guid(Consts.LnkClsid)))
                throw new ShellLinkHeaderException("CLSID is not LNK CLSID", nameof(ShellLinkHeader.LinkClsid));

            if (ShellLinkHeader.Reserved1 != 0)
                throw new ShellLinkHeaderException("Reserved fields must be 0", nameof(ShellLinkHeader.Reserved1));

            if (ShellLinkHeader.Reserved2 != 0)
                throw new ShellLinkHeaderException("Reserved fields must be 0", nameof(ShellLinkHeader.Reserved2));

            if (ShellLinkHeader.Reserved3 != 0)
                throw new ShellLinkHeaderException("Reserved fields must be 0", nameof(ShellLinkHeader.Reserved3));

            if (!Enum.IsDefined(typeof(Enums.ShowWindowCommands), ShellLinkHeader.ShowCommand))
                ShellLinkHeader.ShowCommand = Enums.ShowWindowCommands.Normal;
        }
        
        /// <summary>
        /// Parses the LinkTargetIdList
        /// </summary>
        /// <exception cref="LinkTargetIdListException">Thrown if doesn't get to terminal ID</exception>
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

            if (LinkTargetIdList.List.TerminalID != 0)
                throw new LinkTargetIdListException("LinkTargetIdList.TerminalID must be 0",
                    nameof(LinkTargetIdList.List.TerminalID));
        }

        /// <summary>
        /// Parses the LinkInfo
        /// </summary>
        /// <exception cref="LinkInfoException">Thrown if LinkInfo is not proper</exception>
        private void ParseLinkInfo()
        {
            LinkInfo = new Structs.LinkInfo {Header = Stream.ReadStruct<Structs.LinkInfoHeader>()};

            if (LinkInfo.Header.HeaderSize >= 0x24)
            {
                LinkInfo.HeaderOptional = Stream.ReadStruct<Structs.LinkInfoHeaderOptional>();
            }
            else
            {
                if (LinkInfo.Header.HeaderSize != 0x1C)
                    throw new LinkInfoException("LinkInfo.HeaderSize must be 0x1C", nameof(LinkInfo.Header.HeaderSize));
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

                if (LinkInfo.VolumeId.Header.Size <= 0x10)
                    throw new LinkInfoException("LinkInfo.VolumeId.Header.Size is not greater than 0x10",
                        nameof(LinkInfo.VolumeId.Header.Size));

                if (LinkInfo.VolumeId.Header.VolumeLabelOffset >= LinkInfo.VolumeId.Header.Size)
                    throw new LinkInfoException(
                        "LinkInfo.VolumeId.Header.VolumeLabelOffset is not less than LinkInfo.VolumeId.Header.Size",
                        nameof(LinkInfo.VolumeId.Header.VolumeLabelOffset));

                if (LinkInfo.VolumeId.VolumeLabelOffsetUnicode >= LinkInfo.VolumeId.Header.Size)
                    throw new LinkInfoException(
                        "LinkInfo.VolumeId.VolumeLabelOffsetUnicode is not less than LinkInfo.VolumeId.Header.Size",
                        nameof(LinkInfo.VolumeId.VolumeLabelOffsetUnicode));

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

                if (LinkInfo.CommonNetworkRelativeLink.Header.Size < 0x14)
                    throw new LinkInfoException("LinkInfo.CommonNetworkRelativeLink.Header.Size is less than 0x14",
                        nameof(LinkInfo.CommonNetworkRelativeLink.Header.Size));

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
                    if (LinkInfo.CommonNetworkRelativeLink.Header.DeviceNameOffset == 0)
                        throw new LinkInfoException(
                            "ValidDevice flag cannot be set when LinkInfo.CommonNetworkRelativeLink.Header.DeviceNameOffset is equal to 0",
                            nameof(LinkInfo.CommonNetworkRelativeLink.Header.DeviceNameOffset));

                    LinkInfo.CommonNetworkRelativeLink.DeviceName =
                        Marshal.PtrToStringAnsi(IntPtr.Add(pinnedBuffer.AddrOfPinnedObject(),
                            (int) (commonNetworkRelativeLinkStartOffset +
                                   LinkInfo.CommonNetworkRelativeLink.Header.DeviceNameOffset)));
                }
                else
                {
                    if (LinkInfo.CommonNetworkRelativeLink.Header.DeviceNameOffset > 0)
                        throw new LinkInfoException(
                            "LinkInfo.CommonNetworkRelativeLink.Header.DeviceNameOffset must be 0 if ValidDevice flag is not set",
                            nameof(LinkInfo.CommonNetworkRelativeLink.Header.DeviceNameOffset));
                }

                if (
                    LinkInfo.CommonNetworkRelativeLink.Header.Flags.HasFlag(
                        Enums.CommonNetworkRelativeLinkFlags.ValidNetType))
                {
                    if (
                        !Enum.IsDefined(typeof(Enums.NetworkProviderType),
                            LinkInfo.CommonNetworkRelativeLink.Header.NetProviderType))
                        throw new LinkInfoException("Valid NetProviderType must be set if ValidNetType flag is set",
                            nameof(LinkInfo.CommonNetworkRelativeLink.Header.NetProviderType));
                }
                else
                {
                    if (LinkInfo.CommonNetworkRelativeLink.Header.NetProviderType != 0)
                        throw new LinkInfoException(
                            "LinkInfo.CommonNetworkRelativeLink.Header.NetProviderType must be 0 if ValidNetType flag is set",
                            nameof(LinkInfo.CommonNetworkRelativeLink.Header.NetProviderType));
                }
            }

            pinnedBuffer.Free();
        }

        /// <summary>
        /// Parses the StringData
        /// </summary>
        private void ParseStringData()
        {
            StringData = new Structs.StringData();

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

        /// <summary>
        /// Reads string data by first getting the length (as ushort) and then getting the string using the length
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Parses the ExtraData
        /// </summary>
        private void ParseExtraData()
        {
            if (Stream.BaseStream.Position == Stream.BaseStream.Length)
                return;

            while (!AtTerminalBlock())
            {
                if (Stream.BaseStream.Position == Stream.BaseStream.Length)
                    throw new ExtraDataException("Position is at end of stream", nameof(Stream.BaseStream.Position));

                ExtraData.ParseExtraData(Stream);
            }
        }

        /// <summary>
        /// Determines if at terminal block (last 4 bytes is less than 0x4)
        /// </summary>
        /// <returns>True if at terminal block</returns>
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
