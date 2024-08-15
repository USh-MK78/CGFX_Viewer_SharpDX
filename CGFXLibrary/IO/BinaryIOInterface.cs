using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary.IO
{
    /// <summary>
    /// BinaryIO Class
    /// </summary>
    public class BinaryIOInterface
    {
        public enum Endian
        {
            LittleEndian,
            BigEndian,
        }

        /// <summary>
        /// BinaryIO (Interface)
        /// </summary>
        public interface IBinaryIO
        {
            /// <summary>
            /// BinaryReader
            /// </summary>
            BinaryReader BinaryReader { get; set; }

            /// <summary>
            /// BinaryWriter
            /// </summary>
            BinaryWriter BinaryWriter { get; set; }
        }

        /// <summary>
        /// Binary IO (Abstract)
        /// </summary>
        public abstract class BinaryIO : IBinaryIO
        {
            public byte[] BOM { get; set; }
            public BinaryReader br { get; set; }
            public BinaryWriter bw { get; set; }
            BinaryReader IBinaryIO.BinaryReader { get => br; set => br = value; }
            BinaryWriter IBinaryIO.BinaryWriter { get => bw; set => bw = value; }

            /// <summary>
            /// BinaryReader.Read();
            /// </summary>
            public virtual void Read()
            {
                Read(br, BOM);
            }

            /// <summary>
            /// BinaryReader.Write();
            /// </summary>
            public virtual void Write()
            {
                Write(bw, BOM);
            }

            /// <summary>
            /// BinaryReader.Read();
            /// </summary>
            public abstract void Read(BinaryReader br, byte[] BOM = null);

            /// <summary>
            /// BinaryReader.Write();
            /// </summary>
            public abstract void Write(BinaryWriter bw, byte[] BOM = null);

            public BinaryIO(BinaryReader br)
            {
                this.BOM = new byte[2];
                this.br = br;
            }

            public BinaryIO(BinaryWriter bw)
            {
                this.BOM = new byte[2];
                this.bw = bw;
            }

            public BinaryIO(BinaryReader br, byte[] BOM)
            {
                this.BOM = BOM;
                this.br = br;
            }

            /// <summary>
            /// Initialize BinaryIO
            /// </summary>
            /// <param name="bw">BinaryWriter</param>
            /// <param name="BOM"></param>
            public BinaryIO(BinaryWriter bw, byte[] BOM)
            {
                this.BOM = BOM;
                this.bw = bw;
            }

            /// <summary>
            /// Initialize BinaryIO
            /// </summary>
            public BinaryIO()
            {
                BOM = new byte[2];
                this.br = null;
                this.bw = null;
            }
        }
    }
}
