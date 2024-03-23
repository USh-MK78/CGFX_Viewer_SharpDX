using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary.CGFXSection
{
    public class CANM
    {
        public string Name;
        public string CANM_AttributeName;

        public char[] CANM_Header { get; set; }
        public byte[] Revision { get; set; }
        public int NameOffset { get; set; }
        public int CANM_AttributeNameOffset { get; set; }

        public int UnknownData0 { get; set; }
        public float UnknownData1 { get; set; }
        public int UnknownData2 { get; set; }

        public int UnknownDICTOffset { get; set; }
        public DICT UnknownDICTData { get; set; }

        public byte[] UnknownByteData { get; set; } //0x8

        public void ReadCANM(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            CANM_Header = br.ReadChars(4);
            if (new string(CANM_Header) != "CANM") throw new Exception("不明なフォーマットです");

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

            CANM_AttributeNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (CANM_AttributeNameOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move NameOffset
                br.BaseStream.Seek(CANM_AttributeNameOffset, SeekOrigin.Current);

                ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                readByteLine.ReadByte(br, 0x00);

                CANM_AttributeName = new string(readByteLine.ConvertToCharArray());

                br.BaseStream.Position = Pos;
            }

            UnknownData0 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData1 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            UnknownDICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (UnknownDICTOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DICTOffset
                br.BaseStream.Seek(UnknownDICTOffset, SeekOrigin.Current);

                //0x00000000, 
                UnknownDICTData.ReadDICT(br, BOM);

                br.BaseStream.Position = Pos;
            }

            UnknownByteData = endianConvert.Convert(br.ReadBytes(8)); //???
        }

        public CANM()
        {
            CANM_Header = "CANM".ToArray();
            Revision = new byte[4];
            NameOffset = 0;
            CANM_AttributeNameOffset = 0;

            UnknownData0 = 0;
            UnknownData1 = 0f;
            UnknownData2 = 0;

            UnknownDICTOffset = 0;
            UnknownDICTData = new DICT();

            UnknownByteData = new byte[8];
        }
    }
}
