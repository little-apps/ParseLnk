using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace ParseLnk.Interop
{
    public static class Structs
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct ShellLinkHeader
        {
            public uint HeaderSize;

            /// <summary>
            /// This value MUST be 00021401-0000-0000-C000-000000000046
            /// </summary>
            public Guid LinkClsid;

            public Enums.LinkFlags LinkFlags;
            public FileAttributes FileAttributes;
            public FILETIME CreationTime;
            public FILETIME AccessTime;
            public FILETIME WriteTime;
            public uint FileSize;
            public uint IconIndex;
            public Enums.ShowWindowCommands ShowCommand;
            public HotKeyFlags HotKey;
            public short Reserved1;
            public int Reserved2;
            public int Reserved3;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HotKeyFlags
        {
            /// <summary>
            /// 0x30 ... 0x5A = 0 ... Z
            /// 0x70 ... 0x87 = F1 ... F24
            /// 0x90 = NUM LOCK
            /// 0x91 = SCROLL LOCK
            /// </summary>
            public byte LowByte;

            public Enums.HotKeyFlagsHigh HighByte;
        }

        public struct LinkTargetIDList
        {
            public short Size;
            public IDList List;
        }

        public struct IDList
        {
            public List<ItemID> ItemIDList;
            public ushort TerminalID;
        }

        public struct ItemID
        {
            public ushort Size;
            public byte[] Data;
        }

        public struct StringData
        {
            public string NameString;
            public string RelativePath;
            public string WorkingDir;
            public string CommandLineArgs;
            public string IconLocation;
        }

        #region Link Info Structs

        public struct LinkInfo
        {
            public LinkInfoHeader Header;

            public LinkInfoHeaderOptional HeaderOptional;

            /// <summary>
            /// An optional VolumeID structure (section 2.3.1) that specifies information about the
            /// volume that the link target was on when the link was created. This field is present
            /// if the VolumeIDAndLocalBasePath flag is set.
            /// </summary>
            public LinkInfoVolumeId VolumeId;

            /// <summary>
            /// An optional, NULL–terminated string, defined by the system default code page, which
            /// is used to construct the full path to the link item or link target by appending the
            /// string in the CommonPathSuffix field. This field is present if the
            /// VolumeIDAndLocalBasePath flag is set.
            /// </summary>
            public string LocalBasePath;

            /// <summary>
            /// An optional CommonNetworkRelativeLink structure (section 2.3.2) that specifies
            /// information about the network location where the link target is stored.
            /// </summary>
            public CommonNetworkRelativeLink CommonNetworkRelativeLink;

            /// <summary>
            /// A NULL–terminated string, defined by the system default code page, which is used to
            /// construct the full path to the link item or link target by being appended to the
            /// string in the LocalBasePath field.
            /// </summary>
            public string CommonPathSuffix;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LinkInfoHeader
        {
            /// <summary>
            /// A 32-bit, unsigned integer that specifies the size, in bytes, of the LinkInfo
            /// structure. All offsets specified in this structure MUST be less than this value, and
            /// all strings contained in this structure MUST fit within the extent defined by this size.
            /// </summary>
            public uint Size;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the size, in bytes, of the LinkInfo header
            /// section, which is composed of the LinkInfoSize, LinkInfoHeaderSize, LinkInfoFlags,
            /// VolumeIDOffset, LocalBasePathOffset, CommonNetworkRelativeLinkOffset,
            /// CommonPathSuffixOffset fields, and, if included, the LocalBasePathOffsetUnicode and
            /// CommonPathSuffixOffsetUnicode fields.
            /// </summary>
            public uint HeaderSize;

            /// <summary>
            /// Flags that specify whether the VolumeID, LocalBasePath, LocalBasePathUnicode, and
            /// CommonNetworkRelativeLink fields are present in this structure.
            /// </summary>
            public Enums.LinkInfoFlags Flags;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the location of the VolumeID field. If the
            /// VolumeIDAndLocalBasePath flag is set, this value is an offset, in bytes, from the
            /// start of the LinkInfo structure; otherwise, this value MUST be zero.
            /// </summary>
            public uint VolumeIdOffset;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the location of the LocalBasePath field. If
            /// the VolumeIDAndLocalBasePath flag is set, this value is an offset, in bytes, from the
            /// start of the LinkInfo structure; otherwise, this value MUST be zero.
            /// </summary>
            public uint LocalBasePathOffset;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the location of the
            /// CommonNetworkRelativeLink field. If the CommonNetworkRelativeLinkAndPathSuffix flag
            /// is set, this value is an offset, in bytes, from the start of the LinkInfo structure;
            /// otherwise, this value MUST be zero.
            /// </summary>
            public uint CommonNetworkRelativeLinkOffset;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the location of the CommonPathSuffix field.
            /// This value is an offset, in bytes, from the start of the LinkInfo structure.
            /// </summary>
            public uint CommonPathSuffixOffset;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LinkInfoHeaderOptional
        {
            /// <summary>
            /// An optional, 32-bit, unsigned integer that specifies the location of the
            /// LocalBasePathUnicode field. If the VolumeIDAndLocalBasePath flag is set, this value
            /// is an offset, in bytes, from the start of the LinkInfo structure; otherwise, this
            /// value MUST be zero.This field can be present only if the value of the
            /// LinkInfoHeaderSize field is greater than or equal to 0x00000024.
            /// </summary>
            public uint LocalBasePathOffsetUnicode;

            /// <summary>
            /// An optional, 32-bit, unsigned integer that specifies the location of the
            /// CommonPathSuffixUnicode field. This value is an offset, in bytes, from the start of
            /// the LinkInfo structure. This field can be present only if the value of the
            /// LinkInfoHeaderSize field is greater than or equal to 0x00000024.
            /// </summary>
            public uint CommonPathSuffixOffsetUnicode;
        }

        public struct LinkInfoVolumeId
        {
            public LinkInfoVolumeIdHeader Header;

            /// <summary>
            /// An optional, 32-bit, unsigned integer that specifies the location of a string that
            /// contains the volume label of the drive that the link target is stored on. This value
            /// is an offset, in bytes, from the start of the VolumeID structure to a NULL-terminated
            /// string of Unicode characters. The volume label string is located in the Data field of
            /// this structure. If the value of the VolumeLabelOffset field is not 0x00000014, this
            /// field MUST be ignored, and the value of the VolumeLabelOffset field MUST be used to
            /// locate the volume label string.
            /// </summary>
            public uint VolumeLabelOffsetUnicode;

            /// <summary>
            /// A buffer of data that contains the volume label of the drive as a string defined by
            /// the system default code page or Unicode characters, as specified by preceding fields.
            /// </summary>
            public string Data;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LinkInfoVolumeIdHeader
        {
            /// <summary>
            /// A 32-bit, unsigned integer that specifies the size, in bytes, of this structure. This
            /// value MUST be greater than 0x00000010. All offsets specified in this structure MUST
            /// be less than this value, and all strings contained in this structure MUST fit within
            /// the extent defined by this size.
            /// </summary>
            public uint Size;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the type of drive the link target is stored
            /// on. This value must be a valid drive type
            /// </summary>
            public DriveType DriveType;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the drive serial number of the volume the
            /// link target is stored on.
            /// </summary>
            public uint SerialNumber;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the location of a string that contains the
            /// volume label of the drive that the link target is stored on. This value is an offset,
            /// in bytes, from the start of the VolumeID structure to a NULL-terminated string of
            /// characters, defined by the system default code page. The volume label string is
            /// located in the Data field of this structure. If the value of this field is
            /// 0x00000014, it MUST be ignored, and the value of the VolumeLabelOffsetUnicode field
            /// MUST be used to locate the volume label string.
            /// </summary>
            public uint VolumeLabelOffset;
        }

        public struct CommonNetworkRelativeLink
        {
            public CommonNetworkRelativeLinkHeader Header;

            public CommonNetworkRelativeLinkHeaderOptional HeaderOptional;
            
            /// <summary>
            /// A NULL–terminated string, as defined by the system default code page, which specifies
            /// a server share path.
            /// </summary>
            /// <example>"\\server\share"</example>
            public string NetName;

            /// <summary>
            /// A NULL–terminated string, as defined by the system default code page, which specifies
            /// a device;
            /// </summary>
            /// <example>"D:"</example>
            public string DeviceName;

            /// <summary>
            /// An optional, NULL–terminated, Unicode string that is the Unicode version of the
            /// NetName string. This field MUST be present if the value of the NetNameOffset field is
            /// greater than 0x00000014; otherwise, this field MUST NOT be present.
            /// </summary>
            public string NetNameUnicode;

            /// <summary>
            /// An optional, NULL–terminated, Unicode string that is the Unicode version of the
            /// DeviceName string. This field MUST be present if the value of the NetNameOffset field
            /// is greater than 0x00000014; otherwise, this field MUST NOT be present.
            /// </summary>
            public string DeviceNameUnicode;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CommonNetworkRelativeLinkHeader
        {
            /// <summary>
            /// A 32-bit, unsigned integer that specifies the size, in bytes, of the
            /// CommonNetworkRelativeLink structure. This value MUST be greater than or equal to
            /// 0x00000014. All offsets specified in this structure MUST be less than this value, and
            /// all strings contained in this structure MUST fit within the extent defined by this size.
            /// </summary>
            public uint Size;

            /// <summary>
            /// Flags that specify the contents of the DeviceNameOffset and NetProviderType fields.
            /// </summary>
            public Enums.CommonNetworkRelativeLinkFlags Flags;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the location of the NetName field. This
            /// value is an offset, in bytes, from the start of the CommonNetworkRelativeLink structure.
            /// </summary>
            public uint NetNameOffset;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the location of the DeviceName field. If
            /// the ValidDevice flag is set, this value is an offset, in bytes, from the start of the
            /// CommonNetworkRelativeLink structure; otherwise, this value MUST be zero.
            /// </summary>
            public uint DeviceNameOffset;

            /// <summary>
            /// Must be valid enum if <see cref="Enums.CommonNetworkRelativeLinkFlags"/>.ValidNetType
            /// flag is set
            /// </summary>
            public Enums.NetworkProviderType NetProviderType;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CommonNetworkRelativeLinkHeaderOptional
        {
            /// <summary>
            /// An optional, 32-bit, unsigned integer that specifies the location of the
            /// NetNameUnicode field. This value is an offset, in bytes, from the start of the
            /// CommonNetworkRelativeLink structure. This field MUST be present if the value of the
            /// NetNameOffset field is greater than 0x00000014; otherwise, this field MUST NOT be present.
            /// </summary>
            public uint NetNameOffsetUnicode;

            /// <summary>
            /// An optional, 32-bit, unsigned integer that specifies the location of the
            /// DeviceNameUnicode field. This value is an offset, in bytes, from the start of the
            /// CommonNetworkRelativeLink structure. This field MUST be present if the value of the
            /// NetNameOffset field is greater than 0x00000014; otherwise, this field MUST NOT be present.
            /// </summary>
            public uint DeviceNameOffsetUnicode;
        }

        #endregion Link Info Structs

        #region Extra Data
        [StructLayout(LayoutKind.Sequential)]
        public struct ExtraDataHeader
        {
            public uint Size;
            public uint Signature;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct ConsoleDataBlock
        {
            public Enums.FillAttributes FillAttributes;
            public Enums.FillAttributes PopupFillAttributes;
            public ushort ScreenBufferSizeX;
            public ushort ScreenBufferSizeY;
            public ushort WindowSizeX;
            public ushort WindowSizeY;
            public ushort WindowOriginX;
            public ushort WindowOriginY;
            public int Unused1;
            public int Unused2;
            public Enums.FontFamily FontFamily;
            public uint FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string FaceName;
            public uint CursorSize;
            public uint FullScreen;
            public uint QuickEdit;
            public uint InsertMode;
            public uint AutoPosition;
            public uint HistoryBufferSize;
            public uint NumberOfHistoryBuffers;
            public uint HistoryNoDup;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public uint[] ColorTable;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ConsoleFeDataBlock
        {
            public uint CodePage;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DarwinDataBlock
        {
            public DataAnsi DarwinDataAnsi;
            public DataUnicode DarwinDataUnicode;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct EnvironmentVariableDataBlock
        {
            public DataAnsi TargetAnsi;
            public DataUnicode TargetUnicode;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IconEnvironmentDataBlock
        {
            public DataAnsi TargetAnsi;
            public DataUnicode TargetUnicode;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DataAnsi
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string Value;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DataUnicode
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string Value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KnownFolderDataBlock
        {
            public Guid KnownFolderId;
            public uint Offset;
        }

        public struct PropertyStoreBlock
        {
            public byte[] PropertyStore;
        }

        public struct ShimDataBlock
        {
            public string LayerName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SpecialFolderDataBlock
        {
            public uint SpecialFolderId;
            public uint Offset;
        }
        
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct TrackerDataBlock
        {
            public uint Length;
            public uint Version;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string MachineId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.LPStruct)]
            public Guid[] Droid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.LPStruct)]
            public Guid[] DroidBirth;
        }

        public struct VistaAndAboveIdListDataBlock
        {
            public IDList IdList;
        }
        #endregion

    }
}