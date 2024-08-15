using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CGFXLibrary.CGFXFormat;

namespace CGFXLibrary.CGFXSection.DataComponent
{
    /// <summary>
    /// Flag : 00 00 04 00
    /// </summary>
    public class CFOG_UnknownData0 : IO.BinaryIOInterface.BinaryIO
    {
        public string Name;

        public int NameOffset { get; set; }
        public int UnknownData1 { get; set; }
        public int UnknownData2 { get; set; }

        public byte[] CFOG_UnknownBytes_9 { get; set; } //0x4
        public byte[] CFOG_UnknownBytes_10 { get; set; } //0x4
        public byte[] CFOG_UnknownBytes_11 { get; set; } //0x4
        public byte[] CFOG_UnknownBytes_12 { get; set; } //0x4
        //public CGFXData UnknownRealNumberData { get; set; }

        public void Read_CFOG_UnknownData0(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
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

            UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            CFOG_UnknownBytes_9 = endianConvert.Convert(br.ReadBytes(4));
            CFOG_UnknownBytes_10 = endianConvert.Convert(br.ReadBytes(4));
            CFOG_UnknownBytes_11 = endianConvert.Convert(br.ReadBytes(4));
            CFOG_UnknownBytes_12 = endianConvert.Convert(br.ReadBytes(4));
        }

        public override void Read(BinaryReader br, byte[] BOM = null)
        {
            Read_CFOG_UnknownData0(br, BOM);
        }

        public override void Write(BinaryWriter bw, byte[] BOM = null)
        {
            throw new NotImplementedException();
        }

        public CFOG_UnknownData0()
        {
            NameOffset = 0;
            UnknownData1 = 0;
            UnknownData2 = 0;

            CFOG_UnknownBytes_9 = new byte[4];
            CFOG_UnknownBytes_10 = new byte[4];
            CFOG_UnknownBytes_11 = new byte[4];
            CFOG_UnknownBytes_12 = new byte[4];
            //UnknownRealNumberData = new CGFXData(null, true);
        }
    }
}
