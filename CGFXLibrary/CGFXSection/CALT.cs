using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary.CGFXSection
{
    /// <summary>
    /// Ambient Light (Flag : 0x22040040)
    /// </summary>
    public class CALT : IO.BinaryIOInterface.BinaryIO
    {
        public string Name;
        public char[] CALT_Header { get; set; }
        public byte[] Revision { get; set; }
        public int NameOffset { get; set; }

        public int UserDataDICTCount { get; set; }
        public int UserDataDICTOffset { get; set; }
        public DICT UserDataDICT { get; set; }

        public int UnknownData0 { get; set; }
        public int UnknownData1 { get; set; }
        public int UnknownData3 { get; set; }
        public int UnknownData4 { get; set; }

        public int UnknownData5 { get; set; }
        public int UnknownDICTOffset { get; set; }
        public DICT UnknownDICT { get; set; }

        public float UnknownFloatData0 { get; set; }
        public float UnknownFloatData1 { get; set; }
        public float UnknownFloatData2 { get; set; }
        public float UnknownFloatData3 { get; set; }
        public float UnknownFloatData4 { get; set; }
        public float UnknownFloatData5 { get; set; }
        public float UnknownFloatData6 { get; set; }
        public float UnknownFloatData7 { get; set; }
        public float UnknownFloatData8 { get; set; }

        public MatrixData.Matrix3x4 UnknownMatrixData_0 { get; set; }
        public MatrixData.Matrix3x4 UnknownMatrixData_1 { get; set; }

        public bool IsEnabled { get; set; }

        public AmbientColor AmbientColorData { get; set; }
        public class AmbientColor
        {
            public float R { get; set; }
            public float G { get; set; }
            public float B { get; set; }
            public float A { get; set; }

            public void ReadColor(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }

            public AmbientColor(float R, float G, float B, float A)
            {
                this.R = R;
                this.G = G;
                this.B = B;
                this.A = A;
            }
        }

        public AmbientColorBit AmbientColorBitData { get; set; }
        public class AmbientColorBit
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public void ReadColorBit(BinaryReader br)
            {
                R = br.ReadByte();
                G = br.ReadByte();
                B = br.ReadByte();
                A = br.ReadByte();
            }

            public AmbientColorBit(byte R, byte G, byte B, byte A)
            {
                this.R = R;
                this.G = G;
                this.B = B;
                this.A = A;
            }
        }

        public int UnknownData6 { get; set; }

        public MatrixData.Matrix3x4 GetMatrix(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);

            float[] ary = new float[12];
            for (int i = 0; i < 12; i++)
            {
                ary[i] = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }

            return new MatrixData.Matrix3x4(ary);
        }

        public void ReadCALT(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            CALT_Header = br.ReadChars(4);
            if (new string(CALT_Header) != "CALT") throw new Exception("不明なフォーマットです");

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

            UserDataDICTCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UserDataDICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (UserDataDICTOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move Offset
                br.BaseStream.Seek(UserDataDICTOffset, SeekOrigin.Current);

                UserDataDICT.ReadDICT(br, BOM, true);

                br.BaseStream.Position = Pos;
            }

            UnknownData0 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            UnknownData5 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            UnknownDICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (UnknownDICTOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move Offset
                br.BaseStream.Seek(UnknownDICTOffset, SeekOrigin.Current);

                UnknownDICT.ReadDICT(br, BOM, false);

                br.BaseStream.Position = Pos;
            }

            UnknownFloatData0 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownFloatData1 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownFloatData2 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownFloatData3 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownFloatData4 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownFloatData5 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownFloatData6 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownFloatData7 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownFloatData8 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);

            UnknownMatrixData_0 = GetMatrix(br, BOM);
            UnknownMatrixData_1 = GetMatrix(br, BOM);

            IsEnabled = Convert.ToBoolean(BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0));

            AmbientColorData.ReadColor(br, BOM);
            AmbientColorBitData.ReadColorBit(br);

            UnknownData6 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
        }

        public override void Read(BinaryReader br, byte[] BOM)
        {
            ReadCALT(br, BOM);
        }

        public override void Write(BinaryWriter bw, byte[] BOM)
        {
            throw new NotImplementedException();
        }

        public CALT()
        {
            CALT_Header = "CALT".ToArray();
            Revision = new byte[4];
            NameOffset = 0;

            UserDataDICTCount = 0;
            UserDataDICTOffset = 0;
            UserDataDICT = new DICT();

            UnknownData0 = 0;
            UnknownData1 = 0;
            UnknownData3 = 0;
            UnknownData4 = 0;
            UnknownData5 = 0;

            UnknownDICTOffset = 0;
            UnknownDICT = new DICT();

            UnknownFloatData0 = 0;
            UnknownFloatData1 = 0;
            UnknownFloatData2 = 0;
            UnknownFloatData3 = 0;
            UnknownFloatData4 = 0;
            UnknownFloatData5 = 0;
            UnknownFloatData6 = 0;
            UnknownFloatData7 = 0;
            UnknownFloatData8 = 0;

            UnknownMatrixData_0 = new MatrixData.Matrix3x4();
            UnknownMatrixData_1 = new MatrixData.Matrix3x4();

            IsEnabled = true;

            AmbientColorData = new AmbientColor(0, 0, 0, 0);
            AmbientColorBitData = new AmbientColorBit(0x00, 0x00, 0x00, 0x00);

            UnknownData6 = 0;
        }
    }
}
