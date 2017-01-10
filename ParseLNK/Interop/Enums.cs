using System;

namespace ParseLnk.Interop
{
    public static class Enums
    {
        [Flags]
        public enum LinkFlags
        {
            HasLinkTargetIdList = 1 << 0,
            HasLinkInfo = 1 << 1,
            HasName = 1 << 2,
            HasRelativePath = 1 << 3,
            HasWorkingDir = 1 << 4,
            HasArguments = 1 << 5,
            HasIconLocation = 1 << 6,
            IsUnicode = 1 << 7,
            ForceNoLinkInfo = 1 << 8,
            HasExpString = 1 << 9,
            RunInSeperateProcess = 1 << 10,
            Unused1 = 1 << 11,
            HasDarwinId = 1 << 12,
            RunAsUser = 1 << 13,
            HasExpIcon = 1 << 14,
            NoPidlAlias = 1 << 15,
            Unused2 = 1 << 16,
            RunWithShimLayer = 1 << 17,
            ForceNoLinkTrack = 1 << 18,
            EnableTargetMetadata = 1 << 19,
            DisableKnownFolderTracking = 1 << 20,
            DisableKnownFolderAlias = 1 << 21,
            AllowLinkToLink = 1 << 22,
            UnaliasOnSave = 1 << 24,
            PreferEnviromentPath = 1 << 25,
            KeepLocalIdListForUncTarget = 1 << 26
        }

        [Flags]
        public enum HotKeyFlagsHigh : byte
        {
            KeyShift = 0x01,
            KeyCtrl = 0x02,
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
        [Flags]
        public enum FillAttributes : ushort
        {
            ForegroundBlue = 0x0001,
            ForegroundGreen = 0x0002,
            ForegroundRed = 0x0004,
            ForegroundIntensity = 0x0008,
            BackgroundBlue = 0x0010,
            BackgroundGreen = 0x0020,
            BackgroundRed = 0x0040,
            BackgroundIntensity = 0x0080
        }

        public enum FontFamily : uint
        {
            DontCare = 0x0000,
            Roman = 0x0010,
            Swiss = 0x0020,
            Modern = 0x0030,
            Script = 0x0040,
            Decorative = 0x0050
        }
        #endregion
    }
}
