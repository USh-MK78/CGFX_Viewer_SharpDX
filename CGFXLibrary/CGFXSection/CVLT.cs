using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary.CGFXSection
{
    /// <summary>
    /// Vertex Light
    /// </summary>
    public class CVLT
    {
        public string Name;
        public char[] CVLT_Header { get; set; }
        public byte[] Revision { get; set; }
        public int NameOffset { get; set; }

        public void ReadCVLT(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            CVLT_Header = br.ReadChars(4);
            if (new string(CVLT_Header) != "CVLT") throw new Exception("不明なフォーマットです");

            Revision = endianConvert.Convert(br.ReadBytes(4));
            NameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (NameOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move NameOffset
                br.BaseStream.Seek(NameOffset, SeekOrigin.Current);

                ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                readByteLine.ReadByte(br, 0x00);

                Name = new string(readByteLine.ConvertToCharArray());

                br.BaseStream.Position = Pos;
            }
        }

        public CVLT()
        {
            CVLT_Header = "CVLT".ToArray();
            Revision = new byte[4];
            NameOffset = 0;
        }
    }
}
