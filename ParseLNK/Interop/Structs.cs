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
            /// <summary>
            /// The size, in bytes, of this structure. This value MUST be 0x0000004C.
            /// </summary>
            public uint HeaderSize;

            /// <summary>
            /// A class identifier (CLSID). This value MUST be 00021401-0000-0000-C000-000000000046.
            /// </summary>
            public Guid LinkClsid;

            /// <summary>
            /// A <see cref="Enums.LinkFlags"/> structure that specifies information about the shell link and the
            /// presence of optional portions of the structure.
            /// </summary>
            public Enums.LinkFlags LinkFlags;

            /// <summary>
            /// A <see cref="System.IO.FileAttributes"/> structure that specifies information about the link target.
            /// </summary>
            public FileAttributes FileAttributes;

            /// <summary>
            /// A <see cref="FILETIME"/> structure that specifies the creation time of the link
            /// target in UTC (Coordinated Universal Time). If the value is zero, there is no
            /// creation time set on the link target.
            /// </summary>
            public FILETIME CreationTime;

            /// <summary>
            /// A <see cref="FILETIME"/> structure that specifies the access time of the link target
            /// in UTC (Coordinated Universal Time). If the value is zero, there is no access time
            /// set on the link target.
            /// </summary>
            public FILETIME AccessTime;

            /// <summary>
            /// A <see cref="FILETIME"/> structure that specifies the write time of the link target
            /// in UTC (Coordinated Universal Time). If the value is zero, there is no write time set
            /// on the link target.
            /// </summary>
            public FILETIME WriteTime;

            /// <summary>
            /// A 32-bit unsigned integer that specifies the size, in bytes, of the link target. If
            /// the link target file is larger than 0xFFFFFFFF, this value specifies the least
            /// significant 32 bits of the link target file size.
            /// </summary>
            public uint FileSize;

            /// <summary>
            /// A 32-bit signed integer that specifies the index of an icon within a given icon location.
            /// </summary>
            public int IconIndex;

            /// <summary>
            /// A 32-bit unsigned integer that specifies the expected window state of an application
            /// launched by the link. This value should be one of <see cref="Enums.ShowWindowCommands"/>
            /// </summary>
            public Enums.ShowWindowCommands ShowCommand;

            /// <summary>
            /// A <see cref="HotKeyFlags"/> structure that specifies the keystrokes used to launch
            /// the application referenced by the shortcut key. This value is assigned to the
            /// application after it is launched, so that pressing the key activates that application.
            /// </summary>
            public HotKeyFlags HotKey;

            /// <summary>
            /// A value that MUST be zero.
            /// </summary>
            public short Reserved1;

            /// <summary>
            /// A value that MUST be zero.
            /// </summary>
            public int Reserved2;

            /// <summary>
            /// A value that MUST be zero.
            /// </summary>
            public int Reserved3;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HotKeyFlags
        {
            /// <summary>
            /// 0x30 ... 0x5A = 0 ... Z 0x70 ... 0x87 = F1 ... F24 0x90 = NUM LOCK 0x91 = SCROLL LOCK
            /// </summary>
            public byte LowByte;

            public Enums.HotKeyFlagsHigh HighByte;
        }

        /// <summary>
        /// The LinkTargetIDList structure specifies the target of the link. The presence of this
        /// optional structure is specified by the HasLinkTargetIDList bit in <see
        /// cref="Enums.LinkFlags"/> in the <see cref="ShellLinkHeader"/>.
        /// </summary>
        public struct LinkTargetIDList
        {
            /// <summary>
            /// The size, in bytes, of the IDList field.
            /// </summary>
            public short Size;

            /// <summary>
            /// A stored <see cref="IDList"/> structure, which contains the item ID list.
            /// </summary>
            public IDList List;
        }

        /// <summary>
        /// The stored IDList structure specifies the format of a persisted item ID list.
        /// </summary>
        public struct IDList
        {
            /// <summary>
            /// An array of zero or more <see cref="ItemID"/> structures
            /// </summary>
            public List<ItemID> ItemIDList;

            /// <summary>
            /// A 16-bit, unsigned integer that indicates the end of the item IDs. This value MUST be zero.
            /// </summary>
            public ushort TerminalID;
        }

        /// <summary>
        /// An ItemID is an element in an <see cref="IDList"/> structure. The data stored in a given
        /// ItemID is defined by the source that corresponds to the location in the target namespace
        /// of the preceding ItemIDs. This data uniquely identifies the items in that part of the namespace.
        /// </summary>
        public struct ItemID
        {
            /// <summary>
            /// A 16-bit, unsigned integer that specifies the size, in bytes, of the ItemID
            /// structure, including the ItemIDSize field.
            /// </summary>
            public ushort Size;

            /// <summary>
            /// The shell data source-defined data that specifies an item.
            /// </summary>
            public byte[] Data;
        }

        /// <summary>
        /// StringData refers to a set of structures that convey user interface and path
        /// identification information. The presence of these optional structures is controlled by
        /// <see cref="Enums.LinkFlags"/> in the <see cref="ShellLinkHeader"/>.
        /// </summary>
        public struct StringData
        {
            /// <summary>
            /// An optional structure that specifies a description of the shortcut that is displayed
            /// to end users to identify the purpose of the shell link. This structure MUST be
            /// present if the HasName flag is set.
            /// </summary>
            public string NameString;

            /// <summary>
            /// An optional structure that specifies the location of the link target relative to the
            /// file that contains the shell link. When specified, this string SHOULD be used when
            /// resolving the link. This structure MUST be present if the HasRelativePath flag is set.
            /// </summary>
            public string RelativePath;

            /// <summary>
            /// An optional structure that specifies the file system path of the working directory to
            /// be used when activating the link target. This structure MUST be present if the
            /// HasWorkingDir flag is set.
            /// </summary>
            public string WorkingDir;

            /// <summary>
            /// An optional structure that stores the command-line arguments that are specified when
            /// activating the link target. This structure MUST be present if the HasArguments flag
            /// is set.
            /// </summary>
            public string CommandLineArgs;

            /// <summary>
            /// An optional structure that specifies the location of the icon to be used when
            /// displaying a shell link item in an icon view. This structure MUST be present if the
            /// HasIconLocation flag is set.
            /// </summary>
            public string IconLocation;
        }

        #region Link Info Structs

        /// <summary>
        /// The LinkInfo structure specifies information necessary to resolve a link target if it is
        /// not found in its original location. This includes information about the volume that the
        /// target was stored on, the mapped drive letter, and a Universal Naming Convention (UNC)
        /// form of the path if one existed when the link was created.
        /// </summary>
        public struct LinkInfo
        {
            /// <summary>
            /// See <seealso cref="LinkInfoHeader"/> for more information
            /// </summary>
            public LinkInfoHeader Header;

            /// <summary>
            /// See <seealso cref="LinkInfoHeaderOptional"/> for more informaton
            /// </summary>
            public LinkInfoHeaderOptional HeaderOptional;

            /// <summary>
            /// An optional <see cref="LinkInfoVolumeId"/> structure that specifies information about
            /// the volume that the link target was on when the link was created. This field is
            /// present if the VolumeIDAndLocalBasePath flag is set in <see cref="Enums.LinkInfoFlags"/>.
            /// </summary>
            public LinkInfoVolumeId VolumeId;

            /// <summary>
            /// An optional, NULL–terminated string, defined by the system default code page, which
            /// is used to construct the full path to the link item or link target by appending the
            /// string in the CommonPathSuffix field. This field is present if the
            /// VolumeIDAndLocalBasePath flag is set in <see cref="Enums.LinkInfoFlags"/>.
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

        /// <summary>
        /// ExtraData refers to a set of structures that convey additional information about a link
        /// target. These optional structures can be present in an extra data section that is
        /// appended to the basic Shell Link Binary File Format. These are the first two fields for
        /// each extra data block.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ExtraDataHeader
        {
            /// <summary>
            /// A 32-bit, unsigned integer that specifies the size of the block.
            /// </summary>
            public uint Size;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the signature of the each extra data
            /// section. See <seealso cref="ParseLnk.ExtraData.Blocks"/> for the signatures for each
            /// extra data section.
            /// </summary>
            public uint Signature;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct ConsoleDataBlock
        {
            /// <summary>
            /// A 16-bit, unsigned integer that specifies the <see cref="FillAttributes"/> attributes
            /// that control the foreground and background text colors in the console window.
            /// </summary>
            public Enums.FillAttributes FillAttributes;

            /// <summary>
            /// A 16-bit, unsigned integer that specifies the <see cref="FillAttributes"/> that
            /// control the foreground and background text color in the console window popup. The
            /// values are the same as for the FillAttributes field.
            /// </summary>
            public Enums.FillAttributes PopupFillAttributes;

            /// <summary>
            /// A 16-bit, signed integer that specifies the horizontal size (X axis), in characters,
            /// of the console window buffer.
            /// </summary>
            public ushort ScreenBufferSizeX;

            /// <summary>
            /// A 16-bit, signed integer that specifies the vertical size (Y axis), in characters, of
            /// the console window buffer.
            /// </summary>
            public ushort ScreenBufferSizeY;

            /// <summary>
            /// A 16-bit, signed integer that specifies the horizontal size (X axis), in characters,
            /// of the console window.
            /// </summary>
            public ushort WindowSizeX;

            /// <summary>
            /// A 16-bit, signed integer that specifies the vertical size (Y axis), in characters, of
            /// the console window.
            /// </summary>
            public ushort WindowSizeY;

            /// <summary>
            /// A 16-bit, signed integer that specifies the horizontal coordinate (X axis), in
            /// pixels, of the console window origin.
            /// </summary>
            public ushort WindowOriginX;

            /// <summary>
            /// A 16-bit, signed integer that specifies the vertical coordinate (Y axis), in pixels,
            /// of the console window origin.
            /// </summary>
            public ushort WindowOriginY;

            /// <summary>
            /// A value that is undefined and MUST be ignored.
            /// </summary>
            public int Unused1;

            /// <summary>
            /// A value that is undefined and MUST be ignored.
            /// </summary>
            public int Unused2;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the size, in pixels, of the font used in
            /// the console window.
            /// </summary>
            public uint FontSize;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the family of the font used in the console
            /// window. This value MUST be one of the following in <see cref="Enums.FontFamily"/>
            /// </summary>
            public Enums.FontFamily FontFamily;

            /// <summary>
            /// A 16-bit, unsigned integer that specifies the stroke weight of the font used in the
            /// console window. If the value is equal to or greater than 700, it is bold. Anything
            /// less than 700,
            /// </summary>
            public uint FontWeight;

            /// <summary>
            /// A 32-character Unicode string that specifies the face name of the font used in the
            /// console window.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string FaceName;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the size of the cursor, in pixels, used in
            /// the console window. A value less than or equal represents a small cursor. A value
            /// between 26 and 50 represents a medium cursor. A value between 51 and 100 represents a
            /// large cursor.
            /// </summary>
            public uint CursorSize;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies whether to open the console window in
            /// full-screen mode.
            /// </summary>
            [MarshalAs(UnmanagedType.Bool)]
            public bool FullScreen;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies whether to open the console window in
            /// QuikEdit mode. In QuickEdit mode, the mouse can be used to cut, copy, and paste text
            /// in the console window.
            /// </summary>
            [MarshalAs(UnmanagedType.Bool)]
            public bool QuickEdit;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies insert mode in the console window.
            /// </summary>
            [MarshalAs(UnmanagedType.Bool)]
            public bool InsertMode;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies auto-position mode of the console window.
            /// If false, the values of the WindowOriginX and WindowOriginY fields are used to
            /// position the console window.
            /// </summary>
            [MarshalAs(UnmanagedType.Bool)]
            public bool AutoPosition;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the size, in characters, of the buffer that
            /// is used to store a history of user input into the console window.
            /// </summary>
            public uint HistoryBufferSize;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the number of history buffers to use.
            /// </summary>
            public uint NumberOfHistoryBuffers;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies whether to remove duplicates in the history buffer.
            /// </summary>
            [MarshalAs(UnmanagedType.Bool)]
            public bool HistoryNoDup;

            /// <summary>
            /// A table of 16 32-bit, unsigned integers specifying the RGB colors that are used for
            /// text in the console window. The values of the fill attribute fields FillAttributes
            /// and PopupFillAttributes are used as indexes into this table to specify the final
            /// foreground and background color for a character.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public uint[] ColorTable;
        }

        /// <summary>
        /// The ConsoleFEDataBlock structure specifies the code page to use for displaying text when
        /// a link target specifies an application that is run in a console window.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ConsoleFeDataBlock
        {
            /// <summary>
            /// A 32-bit, unsigned integer that specifies a code page language code identifier.
            /// </summary>
            public uint CodePage;
        }

        /// <summary>
        /// The DarwinDataBlock structure specifies an application identifier that can be used
        /// instead of a link target IDList to install an application when a shell link is activated.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct DarwinDataBlock
        {
            /// <summary>
            /// A NULL–terminated string, defined by the system default code page, which specifies an
            /// application identifier. This field SHOULD be ignored.
            /// </summary>
            public DataAnsi DarwinDataAnsi;

            /// <summary>
            /// An optional, NULL–terminated, Unicode string that specifies an application identifier.
            /// </summary>
            public DataUnicode DarwinDataUnicode;
        }

        /// <summary>
        /// The EnvironmentVariableDataBlock structure specifies a path to environment variable
        /// information when the link target refers to a location that has a corresponding
        /// environment variable.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct EnvironmentVariableDataBlock
        {
            /// <summary>
            /// A NULL-terminated string, defined by the system default code page, which specifies a
            /// path to environment variable information.
            /// </summary>
            public DataAnsi TargetAnsi;

            /// <summary>
            /// An optional, NULL-terminated, Unicode string that specifies a path to environment
            /// variable information.
            /// </summary>
            public DataUnicode TargetUnicode;
        }

        /// <summary>
        /// The IconEnvironmentDataBlock structure specifies the path to an icon. The path is encoded
        /// using environment variables, which makes it possible to find the icon across machines
        /// where the locations vary but are expressed using environment variables.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct IconEnvironmentDataBlock
        {
            /// <summary>
            /// A NULL-terminated string, defined by the system default code page, which specifies a
            /// path that is constructed with environment variables.
            /// </summary>
            public DataAnsi TargetAnsi;

            /// <summary>
            /// An optional, NULL-terminated, Unicode string that specifies a path that is
            /// constructed with environment variables.
            /// </summary>
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

        /// <summary>
        /// The KnownFolderDataBlock structure specifies the location of a known folder. This data
        /// can be used when a link target is a known folder to keep track of the folder so that the
        /// link target IDList can be translated when the link is loaded.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct KnownFolderDataBlock
        {
            /// <summary>
            /// A value in GUID packet representation that specifies the folder GUID ID.
            /// </summary>
            public Guid KnownFolderId;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the location of the <see cref="ItemID"/> of
            /// the first child segment of the <see cref="IDList"/> specified by KnownFolderID. This
            /// value is the offset, in bytes, into the link target IDList.
            /// </summary>
            public uint Offset;
        }

        /// <summary>
        /// A PropertyStoreDataBlock structure specifies a set of properties that can be used by
        /// applications to store extra data in the shell link.
        /// </summary>
        public struct PropertyStoreBlock
        {
            /// <summary>
            /// A serialized property storage structure
            /// </summary>
            public byte[] PropertyStore;
        }

        /// <summary>
        /// The ShimDataBlock structure specifies the name of a shim that can be applied when
        /// activating a link target.
        /// </summary>
        public struct ShimDataBlock
        {
            /// <summary>
            /// A Unicode string that specifies the name of a shim layer to apply to a link target
            /// when it is being activated.
            /// </summary>
            public string LayerName;
        }

        /// <summary>
        /// The SpecialFolderDataBlock structure specifies the location of a special folder. This
        /// data can be used when a link target is a special folder to keep track of the folder, so
        /// that the link target <see cref="IDList"/> can be translated when the link is loaded.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SpecialFolderDataBlock
        {
            /// <summary>
            /// A 32-bit, unsigned integer that specifies the folder integer ID.
            /// </summary>
            public uint SpecialFolderId;

            /// <summary>
            /// A 32-bit, unsigned integer that specifies the location of the <see cref="ItemID"/> of
            /// the first child segment of the <see cref="IDList"/> specified by SpecialFolderID.
            /// This value is the offset, in bytes, into the link target IDList.
            /// </summary>
            public uint Offset;
        }

        /// <summary>
        /// The TrackerDataBlock structure specifies data that can be used to resolve a link target
        /// if it is not found in its original location when the link is resolved. This data is
        /// passed to the Link Tracking service to find the link target.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct TrackerDataBlock
        {
            /// <summary>
            /// A 32-bit, unsigned integer. This value MUST be greater than or equal to 0x0000058.
            /// </summary>
            public uint Length;

            /// <summary>
            /// A 32-bit, unsigned integer. This value MUST be 0x00000000.
            /// </summary>
            public uint Version;

            /// <summary>
            /// A character string, as defined by the system default code page, which specifies the
            /// NetBIOS name of the machine where the link target was last known to reside.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string MachineId;

            /// <summary>
            /// Two values in GUID packet representation that are used to find the link target with
            /// the Link Tracking service
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.LPStruct)]
            public Guid[] Droid;

            /// <summary>
            /// Two values in GUID packet representation that are used to find the link target with
            /// the Link Tracking service
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.LPStruct)]
            public Guid[] DroidBirth;
        }

        /// <summary>
        /// The VistaAndAboveIDListDataBlock structure specifies an alternate IDList that can be used
        /// instead of the <see cref="LinkTargetIDList"/> structure on platforms that support it.
        /// </summary>
        public struct VistaAndAboveIdListDataBlock
        {
            /// <summary>
            /// An <see cref="IDList"/> structure
            /// </summary>
            public IDList IdList;
        }

        #endregion Extra Data
    }
}