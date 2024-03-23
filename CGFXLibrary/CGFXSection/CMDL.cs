using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CGFXLibrary.CGFXFormat;

namespace CGFXLibrary.CGFXSection
{
    /// <summary>
    /// Model
    /// </summary>
    public class CMDL
    {
        public string Name;

        //public byte[] CMDL_BitFlags { get; set; } //0x4(bit7:Has Skeleton Object)
        public char[] CMDL_Header { get; set; } //0x4
        public byte[] CMDL_Revision { get; set; } //0x4
        public int CMDL_ModelNameOffset { get; set; } //0x4
        public int CMDL_UserDataDICTCount { get; set; } //0x4
        public int CMDL_UserDataDICTOffset { get; set; } //0x4
        public DICT CMDL_UserData { get; set; }

        public bool IsBranchVisibleFlag1 { get; set; } //0x4
        public bool IsBranchVisibleFlag2 { get; set; } //0x4

        public CMDL_UnknownSection1 CMDLUnknownSection1 { get; set; }
        public class CMDL_UnknownSection1
        {
            public byte[] Unknown_Byte5 { get; set; } //0x4
            public byte[] Unknown_Byte6 { get; set; } //0x4

            public CMDL_UnknownSection1()
            {
                Unknown_Byte5 = new List<byte>().ToArray();
                Unknown_Byte6 = new List<byte>().ToArray();
            }

            public void Read_UnkSection1(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                Unknown_Byte5 = endianConvert.Convert(br.ReadBytes(4));
                Unknown_Byte6 = endianConvert.Convert(br.ReadBytes(4));
            }
        }

        public int CMDL_NumOfEntries_Animation_DICT { get; set; } //Null : 00 00 00 00
        public int CMDL_Offset_Animation_DICT { get; set; } //Null : 00 00 00 00
        public DICT AnimationDICT { get; set; }

        public Transform.Scale Transform_Scale { get; set; }
        public Transform.Rotate Transform_Rotate { get; set; }
        public Transform.Translate Transform_Translate { get; set; }
        public MatrixData.LocalMatrix CMDL_4x4_Matrix { get; set; }
        public MatrixData.WorldMatrix_Transform CMDL_4x4_Matrix_Transform { get; set; }

        //Mesh
        public int NumOfMeshSOBJEntries { get; set; } //0x4
        public int MeshSOBJListOffset { get; set; } //0x4
        public List<MeshData> meshDatas { get; set; }
        public class MeshData
        {
            public int MeshDataOffset { get; set; }
            public CGFXData SOBJData { get; set; }

            public void Read(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                MeshDataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (MeshDataOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move DataOffset
                    br.BaseStream.Seek(MeshDataOffset, SeekOrigin.Current);

                    SOBJData = new CGFXData(br.ReadBytes(4));
                    SOBJData.Reader(br, BOM);

                    br.BaseStream.Position = Pos;
                }
            }

            public MeshData()
            {
                MeshDataOffset = 0;
                SOBJData = new CGFXData(new byte[4]);
            }

            public override string ToString()
            {
                return SOBJData.SOBJ_Mesh_Section.Meshes.Name;
            }
        }

        public int NumOfMTOB_DICTEntries { get; set; } //0x4
        public int MTOB_DICTOffset { get; set; } //0x4
        public DICT MTOB_DICT { get; set; }

        //Shape
        public int NumOfVertexInfoSOBJEntries { get; set; } //0x4
        public int VertexInfoSOBJListOffset { get; set; } //0x4
        public List<ShapeData> shapeDatas { get; set; }
        public class ShapeData
        {
            public int shapeDataOffset { get; set; }
            public CGFXData SOBJData { get; set; }

            public void Read(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                shapeDataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (shapeDataOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move DataOffset
                    br.BaseStream.Seek(shapeDataOffset, SeekOrigin.Current);

                    SOBJData = new CGFXData(br.ReadBytes(4));
                    SOBJData.Reader(br, BOM);

                    br.BaseStream.Position = Pos;
                }
            }

            public ShapeData()
            {
                shapeDataOffset = 0;
                SOBJData = new CGFXData(new byte[4]);
            }

            //public override string ToString()
            //{
            //    return SOBJData.SOBJ_Shape_Section.Shapes.Name;
            //}
        }

        public int NumOfUnknownDICTEntries { get; set; }
        public int UnknownDICTOffset { get; set; }
        public DICT UnknownDICT { get; set; }

        public bool IsVisible { get; set; } //0x1
        public bool IsNonuniformScalable { get; set; } //0x1
        public byte[] UnknownData3 { get; set; } //0x2

        public byte[] UnknownData2 { get; set; }
        public int LayerID { get; set; }
        //public byte[] UnknownData4 { get; set; }

        //public CMDL_SkeletonInfo CMDLSkeletonInfo { get; set; }
        //public class CMDL_SkeletonInfo
        //{
        //    public byte[] SkeletonInfoSOBJOffset { get; set; } //0x4(Null = Delete)
        //    public byte[] CMDL_ZEROPadding { get; set; } //00 00 00 00
        //}

        //public List<CMDL_VertexInfoSOBJ_1_List> CMDL_VertexInfoSOBJ_List1 { get; set; }
        //public class CMDL_VertexInfoSOBJ_1_List
        //{
        //    public byte[] Offset { get; set; } //0x4 (NumOfVertexInfoSOBJEntries_1)
        //}

        //public List<CMDL_VertexInfoSOBJ_2_List> CMDL_VertexInfoSOBJ_List2 { get; set; }
        //public class CMDL_VertexInfoSOBJ_2_List
        //{
        //    public byte[] Offset { get; set; } //0x4 (NumOfVertexInfoSOBJEntries_2)
        //}



        public void ReadCMDL(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            CMDL_Header = br.ReadChars(4);
            if (new string(CMDL_Header) != "CMDL") throw new Exception("不明なフォーマットです");

            CMDL_Revision = endianConvert.Convert(br.ReadBytes(4));
            CMDL_ModelNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (CMDL_ModelNameOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move NameOffset
                br.BaseStream.Seek(CMDL_ModelNameOffset, SeekOrigin.Current);

                ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                readByteLine.ReadByte(br, 0x00);

                Name = new string(readByteLine.ConvertToCharArray());

                br.BaseStream.Position = Pos;
            }

            CMDL_UserDataDICTCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            CMDL_UserDataDICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (CMDL_UserDataDICTOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(CMDL_UserDataDICTOffset, SeekOrigin.Current);

                CMDL_UserData.ReadDICT(br, BOM);

                br.BaseStream.Position = Pos;
            }

            IsBranchVisibleFlag1 = BitConverter.ToBoolean(endianConvert.Convert(br.ReadBytes(4)), 0);
            IsBranchVisibleFlag2 = BitConverter.ToBoolean(endianConvert.Convert(br.ReadBytes(4)), 0);

            CMDLUnknownSection1.Read_UnkSection1(br, BOM);
            CMDL_NumOfEntries_Animation_DICT = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            CMDL_Offset_Animation_DICT = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (CMDL_Offset_Animation_DICT != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(CMDL_Offset_Animation_DICT, SeekOrigin.Current);

                AnimationDICT.ReadDICT(br, BOM);

                br.BaseStream.Position = Pos;
            }

            Transform_Scale.ReadScale(br, BOM);
            Transform_Rotate.ReadRotate(br, BOM);
            Transform_Translate.ReadTranslate(br, BOM);
            CMDL_4x4_Matrix.ReadLocalMatrix(br, BOM);
            CMDL_4x4_Matrix_Transform.ReadMatrix4x4Transform(br, BOM);

            //Mesh
            NumOfMeshSOBJEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            MeshSOBJListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (MeshSOBJListOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(MeshSOBJListOffset, SeekOrigin.Current);

                for (int i = 0; i < NumOfMeshSOBJEntries; i++)
                {
                    MeshData meshData = new MeshData();
                    meshData.Read(br, BOM);
                    meshDatas.Add(meshData);
                }

                br.BaseStream.Position = Pos;
            }

            NumOfMTOB_DICTEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            MTOB_DICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (MTOB_DICTOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(MTOB_DICTOffset, SeekOrigin.Current);

                MTOB_DICT.ReadDICT(br, BOM);

                br.BaseStream.Position = Pos;
            }

            //Shape
            NumOfVertexInfoSOBJEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            VertexInfoSOBJListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (VertexInfoSOBJListOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(VertexInfoSOBJListOffset, SeekOrigin.Current);

                for (int i = 0; i < NumOfVertexInfoSOBJEntries; i++)
                {
                    ShapeData shapeData = new ShapeData();
                    shapeData.Read(br, BOM);
                    shapeDatas.Add(shapeData);
                }

                br.BaseStream.Position = Pos;
            }

            //SHDR
            NumOfUnknownDICTEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownDICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (UnknownDICTOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(UnknownDICTOffset, SeekOrigin.Current);

                //No IdentFlag, NameOffset(0x4), Unknown(0x4)
                UnknownDICT.ReadDICT(br, BOM);

                br.BaseStream.Position = Pos;
            }

            IsVisible = Converter.ByteToBoolean(br.ReadByte());
            IsNonuniformScalable = Converter.ByteToBoolean(br.ReadByte());
            UnknownData3 = endianConvert.Convert(br.ReadBytes(2));

            UnknownData2 = endianConvert.Convert(br.ReadBytes(4));
            LayerID = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            //UnknownData4 = endianConvert.Convert(br.ReadBytes(4));
        }

        public CMDL()
        {
            CMDL_Header = "CMDL".ToCharArray();
            CMDL_Revision = new List<byte>().ToArray();
            CMDL_ModelNameOffset = 0;
            CMDL_UserDataDICTCount = 0;
            CMDL_UserDataDICTOffset = 0;
            CMDL_UserData = new DICT();
            IsBranchVisibleFlag1 = new bool();
            IsBranchVisibleFlag2 = new bool();
            CMDLUnknownSection1 = new CMDL_UnknownSection1();
            CMDL_NumOfEntries_Animation_DICT = 0;
            CMDL_Offset_Animation_DICT = 0;
            AnimationDICT = new DICT();
            Transform_Scale = new Transform.Scale();
            Transform_Rotate = new Transform.Rotate();
            Transform_Translate = new Transform.Translate();
            CMDL_4x4_Matrix = new MatrixData.LocalMatrix();
            CMDL_4x4_Matrix_Transform = new MatrixData.WorldMatrix_Transform();
            NumOfMeshSOBJEntries = 0;
            MeshSOBJListOffset = 0;
            meshDatas = new List<MeshData>();
            NumOfMTOB_DICTEntries = 0;
            MTOB_DICTOffset = 0;
            MTOB_DICT = new DICT();
            NumOfVertexInfoSOBJEntries = 0;
            VertexInfoSOBJListOffset = 0;
            shapeDatas = new List<ShapeData>();
            NumOfUnknownDICTEntries = 0;
            UnknownDICTOffset = 0;
            UnknownDICT = new DICT();
            IsVisible = new bool();
            IsNonuniformScalable = new bool();
            UnknownData3 = new List<byte>().ToArray();
            UnknownData2 = new List<byte>().ToArray();
            LayerID = 0;
            //UnknownData4 = new List<byte>().ToArray();
        }
    }
}
