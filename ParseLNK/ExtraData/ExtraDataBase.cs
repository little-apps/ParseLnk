using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public abstract class ExtraDataBase<T> where T : struct
    {
        public readonly Structs.ExtraDataHeader Header;
        protected readonly Stream Stream;

        public T Body { get; protected set; }

        protected byte[] BodyBytes { get; private set; }
        protected uint BodyLength { get; }

        

        protected ExtraDataBase(Stream stream, Structs.ExtraDataHeader header)
        {
            Stream = stream;
            Header = header;
            BodyLength = (uint) (Header.Size - Marshal.SizeOf<Structs.ExtraDataHeader>());

            Debug.Assert(Stream.Length - Stream.Position >= BodyLength);

            ReadBytes();
        }

        /// <summary>
        /// Reads the body (everything after the block size and signature) as bytes into BodyBytes
        /// and advances the Stream position
        /// </summary>
        private void ReadBytes()
        {
            BodyBytes = new byte[BodyLength];
            Stream.Read(BodyBytes, 0, BodyBytes.Length);
        }

        /// <summary>
        /// Reads the specified type (T) into Body
        /// </summary>
        // ReSharper disable once MemberCanBeProtected.Global
        public virtual void Read()
        {
            Body = BodyBytes.ReadStruct<T>();
        }
    }
}
