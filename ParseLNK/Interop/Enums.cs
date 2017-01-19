using System;
using ParseLnk.ExtraData;

namespace ParseLnk.Interop
{
    public static class Enums
    {
        /// <summary>
        /// The LinkFlags structure defines bits that specify which shell link structures are present in the file format after the <see cref="Structs.ShellLinkHeader"/> structure
        /// </summary>
        [Flags]
        public enum LinkFlags
        {
            /// <summary>
            /// The shell link is saved with an item ID list (IDList). If this bit is set, a <see cref="Structs.LinkTargetIDList"/> structure MUST follow the <see cref="Structs.ShellLinkHeader"/>. If this bit is not set, this structure MUST NOT be present.
            /// </summary>
            HasLinkTargetIdList = 1 << 0,
            /// <summary>
            /// The shell link is saved with link information. If this bit is set, a <see cref="Structs.LinkInfo"/> structure MUST be present. If this bit is not set, this structure MUST NOT be present.
            /// </summary>
            HasLinkInfo = 1 << 1,
            /// <summary>
            /// The shell link is saved with a name string. If this bit is set, a NAME_STRING <see cref="Structs.StringData"/> structure MUST be present. If this bit is not set, this structure MUST NOT be present.
            /// </summary>
            HasName = 1 << 2,
            /// <summary>
            /// The shell link is saved with a relative path string. If this bit is set, a RELATIVE_PATH <see cref="Structs.StringData"/> structure MUST be present. If this bit is not set, this structure MUST NOT be present.
            /// </summary>
            HasRelativePath = 1 << 3,
            /// <summary>
            /// The shell link is saved with a working directory string. If this bit is set, a WORKING_DIR <see cref="Structs.StringData"/> structure MUST be present. If this bit is not set, this structure MUST NOT be present.
            /// </summary>
            HasWorkingDir = 1 << 4,
            /// <summary>
            /// The shell link is saved with command line arguments. If this bit is set, a COMMAND_LINE_ARGUMENTS <see cref="Structs.StringData"/> structure MUST be present. If this bit is not set, this structure MUST NOT be present.
            /// </summary>
            HasArguments = 1 << 5,
            /// <summary>
            /// The shell link is saved with an icon location string. If this bit is set, an ICON_LOCATION <see cref="Structs.StringData"/> structure MUST be present. If this bit is not set, this structure MUST NOT be present.
            /// </summary>
            HasIconLocation = 1 << 6,
            /// <summary>
            /// The shell link contains Unicode encoded strings. This bit SHOULD be set. If this bit is set, the <see cref="Structs.StringData"/> section contains Unicode-encoded strings; otherwise, it contains strings that are encoded using the system default code page.
            /// </summary>
            IsUnicode = 1 << 7,
            /// <summary>
            /// The <see cref="Structs.LinkInfo"/> structure is ignored.
            /// </summary>
            ForceNoLinkInfo = 1 << 8,
            /// <summary>
            /// The shell link is saved with an <see cref="EnvironmentVariableDataBlock"/>.
            /// </summary>
            HasExpString = 1 << 9,
            /// <summary>
            /// The target is run in a separate virtual machine when launching a link target that is a 16-bit application.
            /// </summary>
            RunInSeperateProcess = 1 << 10,
            /// <summary>
            /// A bit that is undefined and MUST be ignored.
            /// </summary>
            Unused1 = 1 << 11,
            /// <summary>
            /// The shell link is saved with a <see cref="DarwinDataBlock"/>.
            /// </summary>
            HasDarwinId = 1 << 12,
            /// <summary>
            /// The application is run as a different user when the target of the shell link is activated.
            /// </summary>
            RunAsUser = 1 << 13,
            /// <summary>
            /// The shell link is saved with an <see cref="IconEnvironmentDataBlock"/>.
            /// </summary>
            HasExpIcon = 1 << 14,
            /// <summary>
            /// The file system location is represented in the shell namespace when the path to an item is parsed into an <see cref="Structs.IDList"/>.
            /// </summary>
            NoPidlAlias = 1 << 15,
            /// <summary>
            /// A bit that is undefined and MUST be ignored.
            /// </summary>
            Unused2 = 1 << 16,
            /// <summary>
            /// The shell link is saved with a <see cref="ShimDataBlock"/>.
            /// </summary>
            RunWithShimLayer = 1 << 17,
            /// <summary>
            /// The <see cref="TrackerDataBlock"/> is ignored.
            /// </summary>
            ForceNoLinkTrack = 1 << 18,
            /// <summary>
            /// The shell link attempts to collect target properties and store them in the <see cref="PropertyStoreDataBlock"/> when the link target is set.
            /// </summary>
            EnableTargetMetadata = 1 << 19,
            /// <summary>
            /// The <see cref="EnvironmentVariableDataBlock"/> is ignored.
            /// </summary>
            DisableLinkPathTracking = 1 << 20,
            /// <summary>
            /// The <see cref="SpecialFolderDataBlock"/> and the <see cref="KnownFolderDataBlock"/> are ignored when loading the shell link. If this bit is set, these extra data blocks SHOULD NOT be saved when saving the shell link.
            /// </summary>
            DisableKnownFolderTracking = 1 << 21,
            /// <summary>
            /// If the link has a <see cref="KnownFolderDataBlock"/>, the unaliased form of the known folder IDList SHOULD be used when translating the target IDList at the time that the link is loaded.
            /// </summary>
            DisableKnownFolderAlias = 1 << 22,
            /// <summary>
            /// Creating a link that references another link is enabled. Otherwise, specifying a link as the target <see cref="Structs.IDList"/> SHOULD NOT be allowed.
            /// </summary>
            AllowLinkToLink = 1 << 23,
            /// <summary>
            /// When saving a link for which the target <see cref="Structs.IDList"/> is under a known folder, either the unaliased form of that known folder or the target SHOULD be used.
            /// </summary>
            UnaliasOnSave = 1 << 24,
            /// <summary>
            /// The target <see cref="Structs.IDList"/> SHOULD NOT be stored; instead, the path specified in the <see cref="EnvironmentVariableDataBlock"/> SHOULD be used to refer to the target.
            /// </summary>
            PreferEnviromentPath = 1 << 25,
            /// <summary>
            /// When the target is a UNC name that refers to a location on a local machine, the local path IDList in the <see cref="PropertyStoreDataBlock"/> SHOULD be stored, so it can be used when the link is loaded on the local machine.
            /// </summary>
            KeepLocalIdListForUncTarget = 1 << 26
        }

        [Flags]
        public enum HotKeyFlagsHigh : byte
        {
            /// <summary>
            /// The "SHIFT" key on the keyboard.
            /// </summary>
            KeyShift = 0x01,
            /// <summary>
            /// The "CTRL" key on the keyboard.
            /// </summary>
            KeyCtrl = 0x02,
            /// <summary>
            /// The "ALT" key on the keyboard.
            /// </summary>
            KeyAlt = 0x04
        }
        
        public enum ShowWindowCommands
        {
            Normal = 0x00000001,
            Maximized = 0x00000003,
            MinimizedNoActive = 0x00000007,
        }

        /// <summary>
        /// Flags that specify whether the VolumeID, LocalBasePath, LocalBasePathUnicode, and CommonNetworkRelativeLink fields are present in this structure
        /// </summary>
        [Flags]
        public enum LinkInfoFlags
        {
            /// <summary>
            /// If set, the VolumeID and LocalBasePath fields are present, and their locations are specified by the values of the VolumeIDOffset and LocalBasePathOffset fields, respectively. If the value of the LinkInfoHeaderSize field is greater than or equal to 0x00000024, the LocalBasePathUnicode field is present, and its location is specified by the value of the LocalBasePathOffsetUnicode field.
            /// If not set, the VolumeID, LocalBasePath, and LocalBasePathUnicode fields are not present, and the values of the VolumeIDOffset and LocalBasePathOffset fields are zero. If the value of the LinkInfoHeaderSize field is greater than or equal to 0x00000024, the value of the LocalBasePathOffsetUnicode field is zero.
            /// </summary>
            VolumeIdAndLocalBasePath = 1<<0,
            /// <summary>
            /// If set, the CommonNetworkRelativeLink field is present, and its location is specified by the value of the CommonNetworkRelativeLinkOffset field.
            /// If not set, the CommonNetworkRelativeLink field is not present, and the value of the CommonNetworkRelativeLinkOffset field is zero.
            /// </summary>
            CommonNetworkRelativeLinkAndPathSuffix = 1<<1
        }

        [Flags]
        public enum CommonNetworkRelativeLinkFlags
        {
            /// <summary>
            /// If set, the DeviceNameOffset field contains an offset to the device name. If not set,
            /// the DeviceNameOffset field does not contain an offset to the device name, and its
            /// value MUST be zero.
            /// </summary>
            ValidDevice = 1<<0,
            /// <summary>
            /// If set, the NetProviderType field contains the network provider type. If not set, the
            /// NetProviderType field does not contain the network provider type, and its value MUST
            /// be zero.
            /// </summary>
            ValidNetType = 1<<1
        }

        /// <summary>
        /// Specifies the type of network provider in <see cref="Structs.CommonNetworkRelativeLink"/>
        /// </summary>
        public enum NetworkProviderType : uint
        {
            WnncNetAvid = 0x001A0000,
            WnncNetDocuspace = 0x001B0000,
            WnncNetMangosoft = 0x001C0000,
            WnncNetSernet = 0x001D0000,
            WnncNetRiverfront1 = 0x001E0000,
            WnncNetRiverfront2 = 0x001F0000,
            WnncNetDecorb = 0x00200000,
            WnncNetProtstor = 0x00210000,
            WnncNetFjRedir = 0x00220000,
            WnncNetDistinct = 0x00230000,
            WnncNetTwins = 0x00240000,
            WnncNetRdr2Sample = 0x00250000,
            WnncNetCsc = 0x00260000,
            WnncNet3In1 = 0x00270000,
            WnncNetExtendnet = 0x00290000,
            WnncNetStac = 0x002A0000,
            WnncNetFoxbat = 0x002B0000,
            WnncNetYahoo = 0x002C0000,
            WnncNetExifs = 0x002D0000,
            WnncNetDav = 0x002E0000,
            WnncNetKnoware = 0x002F0000,
            WnncNetObjectDire = 0x00300000,
            WnncNetMasfax = 0x00310000,
            WnncNetHobNfs = 0x00320000,
            WnncNetShiva = 0x00330000,
            WnncNetIbmal = 0x00340000,
            WnncNetLock = 0x00350000,
            WnncNetTermsrv = 0x00360000,
            WnncNetSrt = 0x00370000,
            WnncNetQuincy = 0x00380000,
            WnncNetOpenafs = 0x00390000,
            WnncNetAvid1 = 0x003A0000,
            WnncNetDfs = 0x003B0000,
            WnncNetKwnp = 0x003C0000,
            WnncNetZenworks = 0x003D0000,
            WnncNetDriveonweb = 0x003E0000,
            WnncNetVmware = 0x003F0000,
            WnncNetRsfx = 0x00400000,
            WnncNetMfiles = 0x00410000,
            WnncNetMsNfs = 0x00420000,
            WnncNetGoogle = 0x00430000
        }

        #region Extra Data
        /// <summary>
        /// The following bit definitions can be combined to specify 16 different values each for the foreground and background colors.
        /// </summary>
        [Flags]
        public enum FillAttributes : ushort
        {
            /// <summary>
            /// The foreground text color contains blue.
            /// </summary>
            ForegroundBlue = 0x0001,
            /// <summary>
            /// The foreground text color contains green.
            /// </summary>
            ForegroundGreen = 0x0002,
            /// <summary>
            /// The foreground text color contains red.
            /// </summary>
            ForegroundRed = 0x0004,
            /// <summary>
            /// The foreground text color is intensified.
            /// </summary>
            ForegroundIntensity = 0x0008,
            /// <summary>
            /// The background text color contains blue.
            /// </summary>
            BackgroundBlue = 0x0010,
            /// <summary>
            /// The background text color contains green.
            /// </summary>
            BackgroundGreen = 0x0020,
            /// <summary>
            /// The background text color contains red.
            /// </summary>
            BackgroundRed = 0x0040,
            /// <summary>
            /// The background text color is intensified.
            /// </summary>
            BackgroundIntensity = 0x0080
        }

        public enum FontFamily : uint
        {
            /// <summary>
            /// The font family is unknown.
            /// </summary>
            DontCare = 0x0000,
            /// <summary>
            /// The font is variable-width with serifs; for example, "Times New Roman".
            /// </summary>
            Roman = 0x0010,
            /// <summary>
            /// The font is variable-width without serifs; for example, "Arial".
            /// </summary>
            Swiss = 0x0020,
            /// <summary>
            /// The font is fixed-width, with or without serifs; for example, "Courier New".
            /// </summary>
            Modern = 0x0030,
            /// <summary>
            /// The font is designed to look like handwriting; for example, "Cursive".
            /// </summary>
            Script = 0x0040,
            /// <summary>
            /// The font is a novelty font; for example, "Old English".
            /// </summary>
            Decorative = 0x0050
        }
        #endregion
    }
}
