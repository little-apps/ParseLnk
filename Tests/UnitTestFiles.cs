using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using NUnit.Framework;
using ParseLnk;
using ParseLnk.Exceptions;
using ParseLnk.Interop;

namespace Tests
{
    [TestFixture]
    public class UnitTestFiles
    {
        private static string ResourcePrefix => "Tests.TestData";
        
        [Test]
        public void TestCommonNetworkRelativeLinkDeviceName()
        {
            var parseLnk = new Parser(GetTestData("CommonNetworkRelativeLinkDeviceName.bin"));
            parseLnk.Parse();

            Assert.True(
                parseLnk.LinkInfo.Header.Flags.HasFlag(Enums.LinkInfoFlags.CommonNetworkRelativeLinkAndPathSuffix));
            Assert.AreEqual("D:", parseLnk.LinkInfo.CommonNetworkRelativeLink.DeviceName);
        }

        [Test]
        public void TestCommonNetworkRelativeLinkNetDeviceNameUni()
        {
            var parseLnk = new Parser(GetTestData("CommonNetworkRelativeLinkNetDeviceNameUni.bin"));
            parseLnk.Parse();

            Assert.True(
                parseLnk.LinkInfo.Header.Flags.HasFlag(Enums.LinkInfoFlags.CommonNetworkRelativeLinkAndPathSuffix));
            Assert.AreEqual(@"\\server\share", parseLnk.LinkInfo.CommonNetworkRelativeLink.NetNameUnicode);
            Assert.AreEqual("D:", parseLnk.LinkInfo.CommonNetworkRelativeLink.DeviceNameUnicode);
        }

        [Test]
        public void TestCommonNetworkRelativeLinkNetName()
        {
            var parseLnk = new Parser(GetTestData("CommonNetworkRelativeLinkNetName.bin"));
            parseLnk.Parse();

            Assert.True(
                parseLnk.LinkInfo.Header.Flags.HasFlag(Enums.LinkInfoFlags.CommonNetworkRelativeLinkAndPathSuffix));
            Assert.AreEqual(@"\\server\share", parseLnk.LinkInfo.CommonNetworkRelativeLink.NetName);
        }

        [Test]
        public void TestCommonNetworkRelativeLinkNetNameUni()
        {
            var parseLnk = new Parser(GetTestData("CommonNetworkRelativeLinkNetNameUni.bin"));
            parseLnk.Parse();

            Assert.True(
                parseLnk.LinkInfo.Header.Flags.HasFlag(Enums.LinkInfoFlags.CommonNetworkRelativeLinkAndPathSuffix));
            Assert.AreEqual(@"\\server\share", parseLnk.LinkInfo.CommonNetworkRelativeLink.NetNameUnicode);
        }

        [Test]
        public void TestCommonNetworkRelativeLinkNetProviderType()
        {
            var parseLnk = new Parser(GetTestData("CommonNetworkRelativeLinkNetProviderType.bin"));
            parseLnk.Parse();

            Assert.True(
                parseLnk.LinkInfo.CommonNetworkRelativeLink.Header.Flags.HasFlag(
                    Enums.CommonNetworkRelativeLinkFlags.ValidNetType));
            Assert.NotZero((int) parseLnk.LinkInfo.CommonNetworkRelativeLink.Header.NetProviderType);
            Assert.True(Enum.IsDefined(typeof(Enums.NetworkProviderType),
                parseLnk.LinkInfo.CommonNetworkRelativeLink.Header.NetProviderType));
        }

        [Test]
        public void TestLinkInfoCommonPathSuffix()
        {
            var parseLnk = new Parser(GetTestData("LinkInfoCommonPathSuffix.bin"));
            parseLnk.Parse();

            Assert.Positive(parseLnk.LinkInfo.Header.CommonPathSuffixOffset);
            Assert.AreEqual(".exe", parseLnk.LinkInfo.CommonPathSuffix);
        }

        [Test]
        public void TestLinkInfoVolumeId()
        {
            var parseLnk = new Parser(GetTestData("LinkInfoVolumeId.bin"));
            parseLnk.Parse();

            Assert.Positive(parseLnk.LinkInfo.Header.VolumeIdOffset);
            Assert.Positive(parseLnk.LinkInfo.VolumeId.Header.Size);
            Assert.Positive(parseLnk.LinkInfo.VolumeId.Header.SerialNumber);
            Assert.Positive(parseLnk.LinkInfo.VolumeId.Header.VolumeLabelOffset);
            Assert.AreEqual("VolLabel", parseLnk.LinkInfo.VolumeId.Data);
        }
        
        /// <summary>
        /// Tests the file included in the MS-SHLLINK file
        /// </summary>
        [Test]
        public void TestMSExample()
        {
            var parseLnk = new Parser(GetTestData("MSExample.bin"));
            parseLnk.Parse();

            Assert.True(parseLnk.ShellLinkHeader.LinkFlags.HasFlag(Enums.LinkFlags.HasLinkTargetIdList));
            Assert.True(parseLnk.ShellLinkHeader.LinkFlags.HasFlag(Enums.LinkFlags.HasLinkInfo));
            Assert.True(parseLnk.ShellLinkHeader.LinkFlags.HasFlag(Enums.LinkFlags.HasRelativePath));
            Assert.True(parseLnk.ShellLinkHeader.LinkFlags.HasFlag(Enums.LinkFlags.HasWorkingDir));
            Assert.True(parseLnk.ShellLinkHeader.LinkFlags.HasFlag(Enums.LinkFlags.IsUnicode));
            Assert.True(parseLnk.ShellLinkHeader.LinkFlags.HasFlag(Enums.LinkFlags.EnableTargetMetadata));

            Assert.True(parseLnk.ShellLinkHeader.FileAttributes == FileAttributes.Archive);
            
            var expectedDateTime = new DateTime(2008, 9, 12, 20, 27, 17, 101, DateTimeKind.Utc);
            Assert.Zero(DateTime.Compare(expectedDateTime, ToDateTime(parseLnk.ShellLinkHeader.CreationTime)));
            Assert.Zero(DateTime.Compare(expectedDateTime, ToDateTime(parseLnk.ShellLinkHeader.AccessTime)));
            Assert.Zero(DateTime.Compare(expectedDateTime, ToDateTime(parseLnk.ShellLinkHeader.WriteTime)));
            
            Assert.Zero(parseLnk.ShellLinkHeader.FileSize);
            Assert.Zero(parseLnk.ShellLinkHeader.IconIndex);
            Assert.True(parseLnk.ShellLinkHeader.ShowCommand == Enums.ShowWindowCommands.Normal);
            Assert.True(parseLnk.ShellLinkHeader.HotKey.LowByte == 0 && parseLnk.ShellLinkHeader.HotKey.HighByte == 0);

            // TODO: Test IDList

            Assert.True(parseLnk.LinkInfo.Header.Flags == Enums.LinkInfoFlags.VolumeIdAndLocalBasePath);
            Assert.AreEqual(@"C:\test\a.txt", parseLnk.LinkInfo.LocalBasePath);

            Assert.AreEqual(@".\a.txt", parseLnk.StringData.RelativePath);
            Assert.AreEqual(@"C:\test", parseLnk.StringData.WorkingDir);

            Assert.NotZero(parseLnk.ExtraData.TrackerDataBlocks.Count);
            Assert.AreEqual("chris-xps", parseLnk.ExtraData.TrackerDataBlocks[0].Body.MachineId);
        }

        [Test]
        public void TestInvalidLinkHeaderClsid()
        {
            var parseLnk = new Parser(GetTestData("InvalidLinkHeaderClsid.bin"));
            
            var ex = (ShellLinkHeaderException)Assert.Catch(typeof(ShellLinkHeaderException), () => parseLnk.Parse());
            Assert.AreEqual(nameof(parseLnk.ShellLinkHeader.LinkClsid), ex.FieldName);
        }

        private static DateTime ToDateTime(FILETIME time)
        {
            var high = (ulong)time.dwHighDateTime;
            var low = (uint)time.dwLowDateTime;
            var fileTime = (long)((high << 32) + low);
            try
            {
                return DateTime.FromFileTimeUtc(fileTime);
            }
            catch
            {
                return DateTime.FromFileTimeUtc(0xFFFFFFFF);
            }
        }

        private StreamReader GetTestData(string fileName)
        {
            var stream = Assembly.GetCallingAssembly().GetManifestResourceStream($"{ResourcePrefix}.{fileName}");

            if (stream == null)
                throw new FileNotFoundException("Resource could not be found");

            return new StreamReader(stream);
        }
    }
}
