using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CGFXLibrary.CGFXFormat.CGFX.DATA;
using static CGFXLibrary.CGFXSection.MTOB;

namespace CGFXLibrary.CGFXSection
{
    /// <summary>
    /// Fragment Light
    /// </summary>
    public class CFLT
    {
        public string Name;
        public char[] CFLT_Header { get; set; }
        public byte[] Revision { get; set; }
        public int NameOffset { get; set; }

        public byte[] UnknownBytes0 { get; set; } //0x8

        public int IsTwoSidedLightingValue { get; set; }
        public int UnknownData2 { get; set; }
        public int UnknownData3 { get; set; }
        public int UnknownData4 { get; set; }
        public int UnknownData5 { get; set; }

        public int UnknownDICTOffset { get; set; }
        public DICT UnknownDICT { get; set; }

        #region 1
        public float UnknownData6 { get; set; }
        public float UnknownData7 { get; set; }
        public float UnknownData8 { get; set; }
        public float UnknownData9 { get; set; }
        public float UnknownData10 { get; set; }
        public float UnknownData11 { get; set; }
        public float UnknownData12 { get; set; }
        public float UnknownData13 { get; set; }
        public float UnknownData14 { get; set; }

        public MatrixData.Matrix3x4 UnknownMatrix34_0 { get; set; }
        public MatrixData.Matrix3x4 UnknownMatrix34_1 { get; set; }
        #endregion

        public int UnknownData15 { get; set; }

        public enum FragmentLightType
        {
            Directional = 0,
            Point = 1,
            Spot = 2
        }

        public FragmentLightType FragmentLight_Type { get; set; }
        public int FragmentLightTypeValue { get; set; }

        public byte[] UnknownBytes1 { get; set; } //0xC

        public DiffuseColor Diffuse { get; set; }
        public class DiffuseColor
        {
            public float A { get; set; }
            public float R { get; set; }
            public float G { get; set; }
            public float B { get; set; }
            
            public void ReadDiffuseColor(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }

            public DiffuseColor()
            {
                A = 1;
                R = 1;
                G = 1;
                B = 1;
            }
        }

        public SpecularColor0 Specular0 { get; set; }
        public class SpecularColor0
        {
            public float A { get; set; }
            public float R { get; set; }
            public float G { get; set; }
            public float B { get; set; }

            public void ReadSpecularColor0(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }

            public SpecularColor0()
            {
                A = 1;
                R = 1;
                G = 1;
                B = 1;
            }
        }

        public SpecularColor1 Specular1 { get; set; }
        public class SpecularColor1
        {
            public float A { get; set; }
            public float R { get; set; }
            public float G { get; set; }
            public float B { get; set; }

            public void ReadSpecularColor1(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }

            public SpecularColor1()
            {
                A = 1;
                R = 1;
                G = 1;
                B = 1;
            }
        }

        public float Unknowndata0 { get; set; }
        public float Unknowndata1 { get; set; }

        public DiffuseColorBit Diffuse_Bit { get; set; }
        public class DiffuseColorBit
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public void ReadDiffuseColorBit(BinaryReader br)
            {
                R = br.ReadByte();
                G = br.ReadByte();
                B = br.ReadByte();
                A = br.ReadByte();
            }

            public DiffuseColorBit(byte R, byte G, byte B, byte A)
            {
                this.R = R;
                this.G = G;
                this.B = B;
                this.A = A;
            }

            public DiffuseColorBit()
            {
                R = 0xFF;
                G = 0xFF;
                B = 0xFF;
                A = 0xFF;
            }
        }

        public Specular0_ColorBit Specular0_Bit { get; set; }
        public class Specular0_ColorBit
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public void ReadSpecular0ColorBit(BinaryReader br)
            {
                R = br.ReadByte();
                G = br.ReadByte();
                B = br.ReadByte();
                A = br.ReadByte();
            }

            public Specular0_ColorBit(byte R, byte G, byte B, byte A)
            {
                this.R = R;
                this.G = G;
                this.B = B;
                this.A = A;
            }

            public Specular0_ColorBit()
            {
                R = 0xFF;
                G = 0xFF;
                B = 0xFF;
                A = 0xFF;
            }
        }

        public Specular1_ColorBit Specular1_Bit { get; set; }
        public class Specular1_ColorBit
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public void ReadSpecular1ColorBit(BinaryReader br)
            {
                R = br.ReadByte();
                G = br.ReadByte();
                B = br.ReadByte();
                A = br.ReadByte();
            }

            public Specular1_ColorBit(byte R, byte G, byte B, byte A)
            {
                this.R = R;
                this.G = G;
                this.B = B;
                this.A = A;
            }

            public Specular1_ColorBit()
            {
                R = 0xFF;
                G = 0xFF;
                B = 0xFF;
                A = 0xFF;
            }
        }

        public DirectionValue Direction_Value { get; set; }
        /// <summary>
        /// Default Direction Value (?)
        /// </summary>
        public class DirectionValue
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }

            public Vector3 ToVector3()
            {
                return new Vector3(X, Y, Z);
            }

            public void ReadDirectionValue(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                X = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                Y = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                Z = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }

            public DirectionValue()
            {
                X = 0;
                Y = -1;
                Z = 0;
            }
        }

        public PositionValue Position_Value { get; set; }
        /// <summary>
        /// Default Position Value (?)
        /// </summary>
        public class PositionValue
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }

            public Vector3 ToVector3()
            {
                return new Vector3(X, Y, Z);
            }

            public void ReadPositionValue(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                X = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                Y = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                Z = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }

            public PositionValue()
            {
                X = 0;
                Y = 0;
                Z = 0;
            }
        }

        //16 bytes

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

        public void ReadCFLT(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            CFLT_Header = br.ReadChars(4);
            if (new string(CFLT_Header) != "CFLT") throw new Exception("不明なフォーマットです");

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

            UnknownBytes0 = br.ReadBytes(8);

            IsTwoSidedLightingValue = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData5 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            UnknownDICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (UnknownDICTOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(UnknownDICTOffset, SeekOrigin.Current);

                UnknownDICT.ReadDICT(br, BOM);

                br.BaseStream.Position = Pos;
            }

            UnknownData6 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData7 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData8 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData9 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData10 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData11 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData12 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData13 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData14 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);

            UnknownMatrix34_0 = GetMatrix(br, BOM);
            UnknownMatrix34_1 = GetMatrix(br, BOM);

            UnknownData15 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            FragmentLightTypeValue = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            UnknownBytes1 = br.ReadBytes(12);

            Diffuse.ReadDiffuseColor(br, BOM);
            Specular0.ReadSpecularColor0(br, BOM);
            Specular1.ReadSpecularColor1(br, BOM);

            Unknowndata0 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            Unknowndata1 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);

            Diffuse_Bit.ReadDiffuseColorBit(br);
            Specular0_Bit.ReadSpecular0ColorBit(br);
            Specular1_Bit.ReadSpecular1ColorBit(br);

            Direction_Value.ReadDirectionValue(br, BOM);
            Position_Value.ReadPositionValue(br, BOM);

            //16 bytes
        }

        public CFLT()
        {
            CFLT_Header = "CFLT".ToArray();
            Revision = new byte[4];
            NameOffset = 0;

            UnknownBytes0 = new List<byte>().ToArray(); //0x8

            IsTwoSidedLightingValue = 1;
            UnknownData2 = 0;
            UnknownData3 = 0;
            UnknownData4 = 0;
            UnknownData5 = 0;

            UnknownDICTOffset = 0;
            UnknownDICT = new DICT();

            UnknownData6 = 0;
            UnknownData7 = 0;
            UnknownData8 = 0;
            UnknownData9 = 0;
            UnknownData10 = 0;
            UnknownData11 = 0;
            UnknownData12 = 0;
            UnknownData13 = 0;
            UnknownData14 = 0;

            UnknownMatrix34_0 = new MatrixData.Matrix3x4();
            UnknownMatrix34_1 = new MatrixData.Matrix3x4();

            UnknownData15 = 0;

            FragmentLightTypeValue = 0;

            UnknownBytes1 = new List<byte>().ToArray(); //12 byte

            Diffuse = new DiffuseColor();
            Specular0 = new SpecularColor0();
            Specular1 = new SpecularColor1();

            Unknowndata0 = 0;
            Unknowndata1 = 1;

            Diffuse_Bit = new DiffuseColorBit();
            Specular0_Bit = new Specular0_ColorBit();
            Specular1_Bit = new Specular1_ColorBit();

            Direction_Value = new DirectionValue();
            Position_Value = new PositionValue();

            //16 bytes
        }
    }
}
