using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary.SOBJ_Format.MeshData
{
    /// <summary>
    /// SOBJ (Mesh) => Flag : 01 00 00 00
    /// </summary>
    public class Mesh
    {
        public string Name;
        public string MeshName;
        public string MeshNodeName;

        public char[] SOBJ_Header { get; set; }
        public int Revision { get; set; }
        public int SOBJNameOffset { get; set; }
        public int UnknownData2 { get; set; }
        public int UnknownOffset1 { get; set; } //Array (float (?))
        public int ShapeIndex { get; set; }
        public int MaterialIndex { get; set; }

        public UnknownColor UnknownColorSet { get; set; }
        public class UnknownColor
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public System.Windows.Media.Color GetColor()
            {
                return System.Windows.Media.Color.FromArgb(A, R, G, B);
            }

            public void Read_UnknownColor(BinaryReader br)
            {
                R = br.ReadByte();
                G = br.ReadByte();
                B = br.ReadByte();
                A = br.ReadByte();
            }

            public UnknownColor(int Input_R, int Input_G, int Input_B, int Input_A)
            {
                R = (byte)Input_R;
                G = (byte)Input_G;
                B = (byte)Input_B;
                A = (byte)Input_A;
            }

            public UnknownColor()
            {
                R = 0;
                G = 0;
                B = 0;
                A = 0;
            }
        }

        public bool IsVisible { get; set; }
        public byte RenderPriority { get; set; }
        public short OwnerModelOffset { get; set; }
        public short MeshNodeVisibilityIndex { get; set; }

        public byte[] UnknownBytes { get; set; }

        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }
        public int Unknown3 { get; set; }
        public int MeshIndex { get; set; }
        public int Unknown5 { get; set; }
        public int Unknown6 { get; set; }
        public int Unknown7 { get; set; }
        public int Unknown8 { get; set; }
        public int Unknown9 { get; set; }
        public int Unknown10 { get; set; }
        public int Unknown11 { get; set; }
        public int Unknown12 { get; set; }
        public int Unknown13 { get; set; }
        public int Unknown14 { get; set; }
        public int Unknown15 { get; set; }
        public int Unknown16 { get; set; }
        public int Unknown17 { get; set; }
        public int MeshNameOffset { get; set; }
        public int Unknown19 { get; set; }

        public int MeshNodeNameOffset { get; set; }

        public UnknownDataSection2 unknownDataSection2 { get; set; }
        public class UnknownDataSection2
        {
            public int UnknownData1 { get; set; }
            public int UnknownData2 { get; set; }
            public int UnknownData3 { get; set; }
            public int UnknownData4 { get; set; }

            public UnknownDataSection2()
            {
                UnknownData1 = 0;
                UnknownData2 = 0;
                UnknownData3 = 0;
                UnknownData4 = 0;
            }

            public void Read_UnknownDataSection2(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownData4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            }
        }

        public void Read_MeshData(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            SOBJ_Header = br.ReadChars(4);
            if (new string(SOBJ_Header) != "SOBJ") throw new Exception("不明なフォーマットです");
            Revision = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            SOBJNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (SOBJNameOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move NameOffset
                br.BaseStream.Seek(SOBJNameOffset, SeekOrigin.Current);

                ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                readByteLine.ReadByte(br, 0x00);

                Name = new string(readByteLine.ConvertToCharArray());

                br.BaseStream.Position = Pos;
            }

            UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownOffset1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            ShapeIndex = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            MaterialIndex = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            //Color 4byte
            UnknownColorSet.Read_UnknownColor(br);

            IsVisible = Converter.ByteToBoolean(br.ReadByte());
            RenderPriority = br.ReadByte();
            OwnerModelOffset = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0); //This - ThisPosition
            MeshNodeVisibilityIndex = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);

            UnknownBytes = br.ReadBytes(2);

            Unknown1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            Unknown2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            Unknown3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            MeshIndex = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            Unknown5 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            Unknown6 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            Unknown7 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            Unknown8 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            Unknown9 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            Unknown10 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            Unknown11 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            Unknown12 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            Unknown13 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            Unknown14 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            Unknown15 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            Unknown16 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            Unknown17 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            MeshNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (MeshNameOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move NameOffset
                br.BaseStream.Seek(MeshNameOffset, SeekOrigin.Current);

                ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                readByteLine.ReadByte(br, 0x00);

                MeshName = new string(readByteLine.ConvertToCharArray());

                br.BaseStream.Position = Pos;
            }

            Unknown19 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            MeshNodeNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (MeshNodeNameOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move NameOffset
                br.BaseStream.Seek(MeshNodeNameOffset, SeekOrigin.Current);

                ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                readByteLine.ReadByte(br, 0x00);

                MeshNodeName = new string(readByteLine.ConvertToCharArray());

                br.BaseStream.Position = Pos;
            }

            unknownDataSection2.Read_UnknownDataSection2(br, BOM);
        }

        public Mesh()
        {
            SOBJ_Header = "SOBJ".ToCharArray();
            Revision = 0;
            SOBJNameOffset = 0;
            UnknownData2 = 0;
            UnknownOffset1 = 0;
            ShapeIndex = 0;
            MaterialIndex = 0;

            UnknownColorSet = new UnknownColor();

            IsVisible = new bool();
            RenderPriority = 0;
            OwnerModelOffset = 0;
            MeshNodeVisibilityIndex = 0;

            UnknownBytes = new List<byte>().ToArray();

            Unknown1 = 0;
            Unknown2 = 0;
            Unknown3 = 0;
            MeshIndex = 0;
            Unknown5 = 0;
            Unknown6 = 0;
            Unknown7 = 0;
            Unknown8 = 0;
            Unknown9 = 0;
            Unknown10 = 0;
            Unknown11 = 0;
            Unknown12 = 0;
            Unknown13 = 0;
            Unknown14 = 0;
            Unknown15 = 0;
            Unknown16 = 0;
            Unknown17 = 0;
            MeshNameOffset = 0;
            Unknown19 = 0;
            MeshNodeNameOffset = 0;
            unknownDataSection2 = new UnknownDataSection2();
        }
    }
}
