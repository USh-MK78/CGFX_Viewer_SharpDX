using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary.SOBJ_Format.ShapeData
{
    /// <summary>
    /// SOBJ (Shape) => Flag : 10 00 00 01
    /// </summary>
    public class Shape
    {
        public string Name;
        public char[] SOBJ_Header { get; set; }
        public int Revision { get; set; }
        public int SOBJNameOffset { get; set; }
        public int UnknownData2 { get; set; }
        public int UnknownData3 { get; set; }
        public int ShapeFlag { get; set; }
        public int OrientedBoundingBoxOffset { get; set; }
        public BoundingBox OrientedBoundingBox { get; set; }
        public class BoundingBox
        {
            public Flags Flags;
            public Vector3 Position { get; set; }
            public MatrixData.Matrix_BoundingBox Matrix_BoundingBox { get; set; }
            public Vector3 Size { get; set; }

            public void Read(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                Flags = new Flags(br.ReadBytes(4));
                if (Flags.IdentFlag.SequenceEqual(new byte[] { 0x00, 0x00, 0x00, 0x80 }) == true)
                {
                    Position = Converter.ByteArrayToVector3(new byte[][] { endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)) });
                    Matrix_BoundingBox.ReadBoundingBoxMatrix(br, BOM);
                    Size = Converter.ByteArrayToVector3(new byte[][] { endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)) });
                }
            }

            public BoundingBox(Flags flags)
            {
                Flags = flags;
                Position = new Vector3(0, 0, 0);
                Matrix_BoundingBox = new MatrixData.Matrix_BoundingBox(1, 0, 0, 0, 1, 0, 0, 0, 1);
                Size = new Vector3(1, 1, 1);
            }
        }
        public Vector3 PositionOffset { get; set; }
        public int PrimitiveSetCount { get; set; }
        public int PrimitiveSetListOffset { get; set; }
        public List<PrimitiveSet> primitiveSets { get; set; }
        public class PrimitiveSet
        {
            public int PrimitiveSetOffset { get; set; }
            public int RelatedBoneCount { get; set; }
            public int RelatedBoneListOffset { get; set; }
            public List<int> RelatedBoneList { get; set; }
            public int SkinningMode { get; set; }
            public int PrimitiveCount { get; set; }
            public int PrimitiveOffsetListOffset { get; set; }
            public List<Primitive> Primitives { get; set; }
            public class Primitive
            {
                public int PrimitiveOffset { get; set; }
                public int IndexStreamCount { get; set; }
                public int IndexStreamOffsetListOffset { get; set; }
                public List<IndexStreamCtr> IndexStreamCtrList { get; set; }
                public class IndexStreamCtr
                {
                    public int IndexStreamOffset { get; set; }
                    public Flags Flags { get; set; } //IdentFlag : 0x01 -> Byte, 0x03 -> Short
                    public byte PrimitiveMode { get; set; }
                    public bool IsVisible { get; set; }
                    public short UnknownData { get; set; }
                    public int FaceDataLength { get; set; }
                    public int FaceDataOffset { get; set; }
                    public List<Face> Faces { get; set; }
                    public List<int> FaceArray
                    {
                        get
                        {
                            List<int> res = new List<int>();
                            foreach (var r in Faces)
                            {
                                res.Add(r.f0);
                                res.Add(r.f1);
                                res.Add(r.f2);
                            }

                            return res;
                        }
                    }

                    public int BufferObject { get; set; }
                    public int LocationFlag { get; set; }
                    public int CommandCache { get; set; }
                    public int CommandCacheSize { get; set; }
                    public int LocationAddress { get; set; }
                    public int MemoryArea { get; set; }
                    public int BoundingBoxOffset { get; set; }
                    public BoundingBox BoundingBox { get; set; }

                    public void ReadIndexStreamCtr(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        IndexStreamOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (IndexStreamOffset != 0)
                        {
                            long Pos = br.BaseStream.Position;

                            br.BaseStream.Seek(-4, SeekOrigin.Current);

                            //Move Offset
                            br.BaseStream.Seek(IndexStreamOffset, SeekOrigin.Current);

                            #region Read IndexStreamCtr
                            Flags = new Flags(br.ReadBytes(4));
                            PrimitiveMode = br.ReadByte();
                            IsVisible = Converter.ByteToBoolean(br.ReadByte());
                            UnknownData = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                            FaceDataLength = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            FaceDataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            if (FaceDataOffset != 0)
                            {
                                long Pos2 = br.BaseStream.Position;

                                br.BaseStream.Seek(-4, SeekOrigin.Current);

                                //Move Offset
                                br.BaseStream.Seek(FaceDataOffset, SeekOrigin.Current);

                                if (Flags.IdentFlag[0] == 0x01)
                                {
                                    int count = (int)(FaceDataLength / 3);
                                    for (int i = 0; i < count; i++)
                                    {
                                        Faces.Add(new Face(br.ReadByte(), br.ReadByte(), br.ReadByte()));
                                    }
                                }
                                if (Flags.IdentFlag[0] == 0x03)
                                {
                                    int count = (int)(FaceDataLength / 2 / 3);
                                    for (int i = 0; i < count; i++)
                                    {
                                        var t0 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                                        var t1 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                                        var t2 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);

                                        Faces.Add(new Face(t0, t1, t2));
                                    }
                                }

                                br.BaseStream.Position = Pos2;
                            }

                            BufferObject = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            LocationFlag = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            CommandCache = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            CommandCacheSize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            LocationAddress = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            MemoryArea = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            BoundingBoxOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            if (BoundingBoxOffset != 0)
                            {
                                long Pos3 = br.BaseStream.Position;

                                br.BaseStream.Seek(-4, SeekOrigin.Current);

                                //Move Offset
                                br.BaseStream.Seek(BoundingBoxOffset, SeekOrigin.Current);

                                //Flags flags = new Flags(br.ReadBytes(4));
                                BoundingBox boundingBox = new BoundingBox(new Flags(new List<byte>().ToArray()));
                                boundingBox.Read(br, BOM);
                                BoundingBox = boundingBox;

                                br.BaseStream.Position = Pos3;
                            }
                            if (BoundingBoxOffset == 0) BoundingBox = null;
                            #endregion

                            br.BaseStream.Position = Pos;
                        }
                    }

                    public IndexStreamCtr()
                    {
                        IndexStreamOffset = 0;
                        Flags = new Flags(new List<byte>().ToArray());
                        PrimitiveMode = 0;
                        IsVisible = new bool();
                        UnknownData = 0;
                        FaceDataLength = 0;
                        FaceDataOffset = 0;
                        Faces = new List<Face>();
                        BufferObject = 0;
                        LocationFlag = 0;
                        CommandCache = 0;
                        CommandCacheSize = 0;
                        LocationAddress = 0;
                        MemoryArea = 0;
                        BoundingBoxOffset = 0;
                        BoundingBox = new BoundingBox(new Flags(new List<byte>().ToArray()));
                    }
                }
                public int BufferObjectCount { get; set; }
                public int BufferObjectListOffset { get; set; }
                public List<int> BufferObjectList { get; set; }
                public int Flags { get; set; }
                public int CommandAllocator { get; set; }
                //face info array
                //unknown 2 array



                public void ReadPrimitive(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    PrimitiveOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    IndexStreamCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    IndexStreamOffsetListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (IndexStreamOffsetListOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move Offset
                        br.BaseStream.Seek(IndexStreamOffsetListOffset, SeekOrigin.Current);

                        for (int i = 0; i < IndexStreamCount; i++)
                        {
                            IndexStreamCtr indexStreamCtr = new IndexStreamCtr();
                            indexStreamCtr.ReadIndexStreamCtr(br, BOM);
                            IndexStreamCtrList.Add(indexStreamCtr);
                        }

                        br.BaseStream.Position = Pos;
                    }

                    BufferObjectCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    BufferObjectListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (BufferObjectListOffset != 0)
                    {
                        long Pos2 = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(BufferObjectListOffset, SeekOrigin.Current);

                        BinaryReadHelper binaryReadHelper = new BinaryReadHelper(br, BOM);
                        var s = binaryReadHelper.ReadArray<int>(BufferObjectCount, 4);
                        BufferObjectList = s.ToList();

                        br.BaseStream.Position = Pos2;
                    }

                    Flags = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    CommandAllocator = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                }

                public Primitive()
                {
                    PrimitiveOffset = 0;
                    IndexStreamCount = 0;
                    IndexStreamOffsetListOffset = 0;
                    IndexStreamCtrList = new List<IndexStreamCtr>();
                    BufferObjectCount = 0;
                    BufferObjectListOffset = 0;
                    BufferObjectList = new List<int>();
                    Flags = 0;
                    CommandAllocator = 0;
                }
            }

            public List<List<Primitive.IndexStreamCtr>> GetIndexStreamCtrPrimitive()
            {
                return Primitives.Select(x => x.IndexStreamCtrList).ToList();
            }

            public void ReadPrimitiveSet(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                PrimitiveSetOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (PrimitiveSetOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move Offset
                    br.BaseStream.Seek(PrimitiveSetOffset, SeekOrigin.Current);

                    #region Read RelatedBone
                    RelatedBoneCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    RelatedBoneListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (RelatedBoneListOffset != 0)
                    {
                        long Pos2 = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(RelatedBoneListOffset, SeekOrigin.Current);

                        BinaryReadHelper binaryReadHelper = new BinaryReadHelper(br, BOM);
                        var s = binaryReadHelper.ReadArray<int>(RelatedBoneCount, 4);
                        RelatedBoneList = s.ToList();

                        br.BaseStream.Position = Pos2;
                    }

                    SkinningMode = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    PrimitiveCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    PrimitiveOffsetListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (PrimitiveOffsetListOffset != 0)
                    {
                        long Pos2 = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move Offset
                        br.BaseStream.Seek(PrimitiveOffsetListOffset, SeekOrigin.Current);

                        for (int i = 0; i < PrimitiveCount; i++)
                        {
                            //Read Primitive
                            Primitive primitive = new Primitive();
                            primitive.ReadPrimitive(br, BOM);
                            Primitives.Add(primitive);
                        }

                        br.BaseStream.Position = Pos2;
                    }
                    #endregion

                    br.BaseStream.Position = Pos;
                }
            }

            public PrimitiveSet()
            {
                RelatedBoneCount = 0;
                RelatedBoneListOffset = 0;
                RelatedBoneList = new List<int>();
                SkinningMode = 0;
                PrimitiveCount = 0;
                PrimitiveOffsetListOffset = 0;
                Primitives = new List<Primitive>();
            }
        }
        public int BaseAddress { get; set; }
        public int VertexAttributeCount { get; set; }
        public int VertexAttributeOffsetListOffset { get; set; }
        public List<VertexAttribute> VertexAttributes { get; set; }
        public class VertexAttribute
        {
            public int VertexAttributeOffset { get; set; }
            public Flags Flag { get; set; }
            public Stream Streams { get; set; }
            public class Stream
            {
                //public int BufferObject { get; set; }
                //public int LocationFlag { get; set; }
                public CGFXLibrary.VertexAttribute.Usage VertexAttributeUsageFlag { get; set; }
                public CGFXLibrary.VertexAttribute.Flag VertexAttributeFlag { get; set; }
                public int VertexStreamLength { get; set; }
                public int VertexStreamOffset { get; set; }
                public List<byte> VertexStreamList { get; set; }
                public int LocationAddress { get; set; } //VertexStreamOffset seems to be used when this one is 0...
                public int MemoryArea { get; set; }
                public List<Polygon> PolygonList { get; set; }

                public int UnknownData1 { get; set; }
                public int UnknownData2 { get; set; }

                public int VertexDataEntrySize { get; set; }
                public int NrVertexStreams { get; set; }
                public int VertexStreamsOffsetListOffset { get; set; }
                public List<VertexStream> VertexStreams { get; set; }
                public class VertexStream
                {
                    public int VertexStreamsOffset { get; set; }

                    public Flags Flags { get; set; }
                    //public int VertexAttributeUsageNum { get; set; }
                    //public CGFX_Viewer.VertexAttribute.Usage.UsageType VertexAttributeUsageFlag => (CGFX_Viewer.VertexAttribute.Usage.UsageType)VertexAttributeUsageNum;
                    //public int VertexAttributeFlagNum { get; set; }
                    //public CGFX_Viewer.VertexAttribute.Flag.FlagType VertexAttributeFlag => (CGFX_Viewer.VertexAttribute.Flag.FlagType)VertexAttributeFlagNum;
                    public CGFXLibrary.VertexAttribute.Usage VertexAttributeUsageFlag { get; set; }
                    public CGFXLibrary.VertexAttribute.Flag VertexAttributeFlag { get; set; }

                    public int BufferObject { get; set; }
                    public int LocationFlag { get; set; } //0x10
                    public int VertexStreamLength { get; set; }
                    public int VertexStreamOffset { get; set; }

                    public int LocationAddress { get; set; }
                    public int MemoryArea { get; set; } //0x20
                    public Component Components { get; set; }
                    public class Component
                    {
                        public Flags Flags { get; set; }

                        public FormatType FormatTypes
                        {
                            get => (FormatType)Flags.IdentFlag[0];
                            set => Flags.IdentFlag[0] = Convert.ToByte(Enum.ToObject(typeof(FormatType), value));
                        }

                        public enum FormatType
                        {
                            BYTE = 0,
                            UNSIGNED_BYTE = 1,
                            SHORT = 2,//might also be unsigned short
                            FLOAT = 6
                        }

                        public int GetFormatTypeLength()
                        {
                            int n = -1;
                            if (FormatTypes == FormatType.BYTE || FormatTypes == FormatType.UNSIGNED_BYTE) n = 1;
                            if (FormatTypes == FormatType.SHORT) n = 2;
                            if (FormatTypes == FormatType.FLOAT) n = 4;
                            return n;
                        }

                        //public int FormatType { get; set; }
                        public int ComponentCount { get; set; } //For example XYZ = 3, ST = 2, RGBA = 4

                        public ComponentType ComponentTypeFlag => (ComponentType)ComponentCount;
                        public enum ComponentType
                        {
                            ST = 2,
                            XYZ = 3,
                            RGBA = 4
                        }

                        //public List<float> Vs { get; set; }


                        public void ReadComponent(BinaryReader br, byte[] BOM)
                        {
                            EndianConvert endianConvert = new EndianConvert(BOM);
                            Flags = new Flags(br.ReadBytes(4));
                            //FormatType = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            ComponentCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        }

                        public Component()
                        {
                            Flags = new Flags(new List<byte>().ToArray());
                            //FormatType = 0;
                            ComponentCount = 0;
                        }
                    }

                    //public int FormatType { get; set; }
                    //public int ComponentCount { get; set; }
                    public float Scale { get; set; }
                    public int Offset { get; set; }

                    public void ReadVertexData(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        VertexStreamsOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (VertexStreamsOffset != 0)
                        {
                            long Pos = br.BaseStream.Position;

                            br.BaseStream.Seek(-4, SeekOrigin.Current);

                            //Move NameOffset
                            br.BaseStream.Seek(VertexStreamsOffset, SeekOrigin.Current);

                            Flags = new Flags(br.ReadBytes(4));
                            //VertexAttributeUsageNum = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //VertexAttributeFlagNum = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            VertexAttributeUsageFlag = new CGFXLibrary.VertexAttribute.Usage(BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0));
                            VertexAttributeFlag = new CGFXLibrary.VertexAttribute.Flag(BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0));


                            BufferObject = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            LocationFlag = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            VertexStreamLength = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            VertexStreamOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            if (VertexStreamOffset != 0) return; //Unused(?)
                            LocationAddress = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            MemoryArea = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            Components.ReadComponent(br, BOM);
                            //FormatType = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //ComponentCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            Scale = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                            Offset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                            br.BaseStream.Position = Pos;
                        }
                    }

                    public VertexStream()
                    {
                        VertexStreamsOffset = 0;

                        Flags = new Flags(new byte[] { 0x00, 0x00, 0x00, 0x00 });
                        //VertexAttributeUsageNum = 0;
                        //VertexAttributeFlagNum = 0;
                        VertexAttributeUsageFlag = new CGFXLibrary.VertexAttribute.Usage(-1);
                        VertexAttributeFlag = new CGFXLibrary.VertexAttribute.Flag(-1);

                        BufferObject = 0;
                        LocationFlag = 0;
                        VertexStreamLength = 0;
                        VertexStreamOffset = 0;

                        LocationAddress = 0;
                        MemoryArea = 0;
                        Components = new Component();
                        //FormatType = 0;
                        //ComponentCount = 0;
                        Scale = 0;
                        Offset = 0;
                    }

                    #region cls
                    //public int BufferObject { get; set; }
                    //public int LocationFlag { get; set; }
                    //public int VertexStreamLength { get; set; }
                    //                     public int VertexStreamOffset { get; set; }
                    //                     public List<VertexData> VertexDatas { get; set; }
                    //                     public class VertexData
                    //{
                    //                         public int BufferObject { get; set; }
                    //                         public int LocationFlag { get; set; } //0x10
                    //                         public int VertexStreamLength { get; set; }
                    //                         public int VertexStreamOffset { get; set; }

                    //                         public int LocationAddress { get; set; }
                    //                         public int MemoryArea { get; set; } //0x20
                    //                         public Component Components { get; set; }
                    //                         public class Component
                    //	{
                    //                             public Flags Flags { get; set; }

                    //                             public FormatType FormatTypes
                    //		{
                    //			get => (FormatType)Flags.IdentFlag[0];
                    //			set => Flags.IdentFlag[0] = Convert.ToByte(Enum.ToObject(typeof(FormatType), value));
                    //		}
                    //		public enum FormatType
                    //		{
                    //                                 BYTE = 0,
                    //                                 UNSIGNED_BYTE = 1,
                    //                                 SHORT = 2,//might also be unsigned short
                    //                                 FLOAT = 6
                    //                             }

                    //                             //public int FormatType { get; set; }
                    //                             public int ComponentCount { get; set; } //For example XYZ = 3, ST = 2, RGBA = 4
                    //                             //public List<float> Vs { get; set; }


                    //                             public void ReadComponent(BinaryReader br, byte[] BOM)
                    //		{
                    //                                 EndianConvert endianConvert = new EndianConvert(BOM);
                    //                                 Flags = new Flags(br.ReadBytes(4));
                    //                                 //FormatType = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                                 ComponentCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                             }

                    //                             public Component()
                    //		{
                    //                                 Flags = new Flags(new List<byte>().ToArray());
                    //                                 //FormatType = 0;
                    //                                 ComponentCount = 0;
                    //                             }
                    //                         }

                    //                         //public int FormatType { get; set; }
                    //                         //public int ComponentCount { get; set; }
                    //                         public float Scale { get; set; }
                    //                         public int Offset { get; set; }

                    //                         public void ReadVertexData(BinaryReader br, byte[] BOM)
                    //	{
                    //                             EndianConvert endianConvert = new EndianConvert(BOM);
                    //                             BufferObject = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                             LocationFlag = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                             VertexStreamLength = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                             VertexStreamOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                             if (VertexStreamOffset != 0) return; //Unused(?)
                    //                             LocationAddress = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                             MemoryArea = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                             Components.ReadComponent(br, BOM);
                    //                             //FormatType = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                             //ComponentCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                             Scale = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                             Offset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                         }

                    //                         public VertexData()
                    //	{
                    //                             BufferObject = 0;
                    //                             LocationFlag = 0;
                    //                             VertexStreamLength = 0;
                    //                             VertexStreamOffset = 0;

                    //                             LocationAddress = 0;
                    //                             MemoryArea = 0;
                    //                             Components = new Component();
                    //                             //FormatType = 0;
                    //                             //ComponentCount = 0;
                    //                             Scale = 0;
                    //                             Offset = 0;
                    //                         }
                    //                     }
                    //public int LocationAddress { get; set; }
                    //                     public int VertexDataEntrySize { get; set; }//Stride
                    //                     public int NrVertexStreams { get; set; } //Nr Attributes
                    //                     public int VertexStreamsOffsetArrayOffset { get; set; }

                    //                     public void ReadVertexStream(BinaryReader br, byte[] BOM)
                    //{
                    //                         EndianConvert endianConvert = new EndianConvert(BOM);
                    //                         BufferObject = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                         LocationFlag = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                         VertexStreamLength = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                         VertexStreamOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                         if (VertexStreamOffset != 0)
                    //	{
                    //                             long Pos = br.BaseStream.Position;

                    //                             br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //                             //Move Offset
                    //                             br.BaseStream.Seek(VertexStreamOffset, SeekOrigin.Current);

                    //                             //for (int i = 0; i < VertexStreamLength; i++)

                    //                             br.BaseStream.Position = Pos;
                    //                         }
                    //                         LocationAddress = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                         VertexDataEntrySize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);//Stride
                    //                         NrVertexStreams = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);//Nr Attributes
                    //                         VertexStreamsOffsetArrayOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                         if (VertexStreamsOffsetArrayOffset != 0)
                    //	{
                    //                             long Pos = br.BaseStream.Position;

                    //                             br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //                             //Move Offset
                    //                             br.BaseStream.Seek(VertexStreamsOffsetArrayOffset, SeekOrigin.Current);

                    //                             //for (int i = 0; i < NrVertexStreams; i++)
                    //                             //{

                    //                             //}

                    //                             br.BaseStream.Position = Pos;



                    //                         }

                    //                         //MemoryArea = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                         //FormatType = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                         //ComponentCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                         //Scale = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                         //Offset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //                     }

                    //                     public VertexStream()
                    //{
                    //                         BufferObject = 0;
                    //                         LocationFlag = 0;
                    //                         VertexStreamLength = 0;
                    //                         VertexStreamOffset = 0;
                    //                         VertexDatas = new List<VertexData>();
                    //                         LocationAddress = 0;
                    //                         VertexDataEntrySize = 0;
                    //                         NrVertexStreams = 0;
                    //                         VertexStreamsOffsetArrayOffset = 0;

                    //                         //FormatType = 0;
                    //                         //ComponentCount = 0;
                    //                         //Scale = 0;
                    //                         //Offset = 0;
                    //                     }
                    #endregion
                }

                public void ReadStream(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    //BufferObject = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    //LocationFlag = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    VertexAttributeUsageFlag = new CGFXLibrary.VertexAttribute.Usage(BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0));
                    VertexAttributeFlag = new CGFXLibrary.VertexAttribute.Flag(BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0));
                    VertexStreamLength = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    VertexStreamOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (VertexStreamOffset != 0)
                    {

                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(VertexStreamOffset, SeekOrigin.Current);

                        BinaryReadHelper binaryReadHelper = new BinaryReadHelper(br, BOM);
                        var s = binaryReadHelper.ReadArray(VertexStreamLength);
                        VertexStreamList = s.ToList();

                        br.BaseStream.Position = Pos;
                    }

                    MemoryArea = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0); //DataOffset
                    LocationAddress = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0); //DataSize

                    long Pos1 = MemoryArea != 0 ? br.BaseStream.Position : 0;

                    #region Del
                    //if (MemoryArea != 0)
                    //{
                    //    long Pos = br.BaseStream.Position;

                    //    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //    //Move NameOffset
                    //    br.BaseStream.Seek(MemoryArea, SeekOrigin.Current);

                    //    int PolygonCount = LocationAddress / 32; //Position, Normal, TextureCoordinate
                    //    for (int i = 0; i < PolygonCount; i++)
                    //    {
                    //        Polygon polygon = new Polygon()
                    //        {
                    //            Vertex = Converter.ByteArrayToPoint3D(new byte[][] { endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)) }),
                    //            Normal = Converter.ByteArrayToVector3D(new byte[][] { endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)) }),
                    //            TexCoord = new Polygon.TextureCoordinate(BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0), BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0))
                    //        };

                    //        PolygonList.Add(polygon);
                    //    }

                    //    br.BaseStream.Position = Pos;
                    //}
                    #endregion

                    //8byte(?)
                    UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    VertexDataEntrySize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    NrVertexStreams = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    VertexStreamsOffsetListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (VertexStreamsOffsetListOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(VertexStreamsOffsetListOffset, SeekOrigin.Current);

                        for (int i = 0; i < NrVertexStreams; i++)
                        {
                            VertexStream vertexStream = new VertexStream();
                            vertexStream.ReadVertexData(br, BOM);
                            VertexStreams.Add(vertexStream);
                        }

                        br.BaseStream.Position = Pos;
                    }

                    if (VertexAttributeFlag.FlagTypes == CGFXLibrary.VertexAttribute.Flag.FlagType.Interleave)
                    {
                        //Interleave
                        if (VertexStreamOffset == 0)
                        {
                            int AllComponentLength = 0;
                            foreach (var tr in VertexStreams) AllComponentLength += tr.Components.ComponentCount * tr.Components.GetFormatTypeLength();
                            var Count = MemoryArea / AllComponentLength;

                            br.BaseStream.Position = Pos1;

                            br.BaseStream.Seek(-4, SeekOrigin.Current);

                            //Move NameOffset
                            br.BaseStream.Seek(LocationAddress, SeekOrigin.Current);

                            for (int iu = 0; iu < Count; iu++)
                            {
                                Polygon polygon = new Polygon();

                                foreach (var vertexStream in VertexStreams)
                                {
                                    int CompCount = vertexStream.Components.GetFormatTypeLength();

                                    if (vertexStream.VertexAttributeUsageFlag.UsageTypes == CGFXLibrary.VertexAttribute.Usage.UsageType.Position)
                                    {
                                        polygon.Scale_Factor.VtScale = vertexStream.Scale;
                                        polygon.Vertex = Converter.ByteArrayToVector3(new byte[][] { br.ReadBytes(CompCount), br.ReadBytes(CompCount), br.ReadBytes(CompCount) });
                                    }
                                    else if (vertexStream.VertexAttributeUsageFlag.UsageTypes == CGFXLibrary.VertexAttribute.Usage.UsageType.Normal)
                                    {
                                        polygon.Scale_Factor.NrScale = vertexStream.Scale;
                                        polygon.Normal = Converter.ByteArrayToVector3(new byte[][] { br.ReadBytes(CompCount), br.ReadBytes(CompCount), br.ReadBytes(CompCount) });
                                    }
                                    else if (vertexStream.VertexAttributeUsageFlag.UsageTypes == CGFXLibrary.VertexAttribute.Usage.UsageType.Tangent)
                                    {
                                        polygon.Tangent = Converter.ByteArrayToVector3(new byte[][] { br.ReadBytes(CompCount), br.ReadBytes(CompCount), br.ReadBytes(CompCount) });
                                    }
                                    else if (vertexStream.VertexAttributeUsageFlag.UsageTypes == CGFXLibrary.VertexAttribute.Usage.UsageType.TextureCoordinate0)
                                    {
                                        polygon.Scale_Factor.TexCoord0Scale = vertexStream.Scale;
                                        polygon.TexCoord0 = new Vector2(BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0), BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0));
                                    }
                                    else if (vertexStream.VertexAttributeUsageFlag.UsageTypes == CGFXLibrary.VertexAttribute.Usage.UsageType.TextureCoordinate1)
                                    {
                                        polygon.Scale_Factor.TexCoord1Scale = vertexStream.Scale;
                                        polygon.TexCoord1 = new Vector2(BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0), BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0));
                                    }
                                    else if (vertexStream.VertexAttributeUsageFlag.UsageTypes == CGFXLibrary.VertexAttribute.Usage.UsageType.TextureCoordinate2)
                                    {
                                        polygon.Scale_Factor.TexCoord2Scale = vertexStream.Scale;
                                        polygon.TexCoord2 = new Vector2(BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0), BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0));
                                    }
                                    else if (vertexStream.VertexAttributeUsageFlag.UsageTypes == CGFXLibrary.VertexAttribute.Usage.UsageType.Color)
                                    {
                                        polygon.ColorData = new Polygon.Color(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte());
                                    }
                                }

                                PolygonList.Add(polygon);
                            }


                        }
                        if (VertexStreamOffset != 0)
                        {
                            var f = VertexStreamLength / VertexDataEntrySize;

                            Console.WriteLine("Breakpoint");
                            //System.Windows.Forms.MessageBox.Show("Breakpoint");
                        }

                    }

                    if (VertexAttributeFlag.FlagTypes == CGFXLibrary.VertexAttribute.Flag.FlagType.VertexParam)
                    {

                    }

                    #region Del
                    //if (VertexStreamOffset == 0)
                    //{
                    //    int AllComponentLength = 0;
                    //    foreach (var tr in VertexStreams) AllComponentLength += tr.Components.ComponentCount * tr.Components.GetFormatTypeLength();
                    //    var Count = MemoryArea / AllComponentLength;

                    //    br.BaseStream.Position = Pos1;

                    //    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //    //Move NameOffset
                    //    br.BaseStream.Seek(LocationAddress, SeekOrigin.Current);

                    //    for (int iu = 0; iu < Count; iu++)
                    //    {
                    //        Polygon polygon1 = new Polygon();

                    //        foreach (var h in VertexStreams)
                    //        {
                    //            //var g = h.VertexAttributeFlag;

                    //            int CompCount = h.Components.GetFormatTypeLength();

                    //            if (h.VertexAttributeUsageFlag.UsageTypes == CGFX_Viewer.VertexAttribute.Usage.UsageType.Position)
                    //            {
                    //                polygon1.Vertex = Converter.ByteArrayToPoint3D(new byte[][] { br.ReadBytes(CompCount), br.ReadBytes(CompCount), br.ReadBytes(CompCount) });
                    //            }
                    //            else if (h.VertexAttributeUsageFlag.UsageTypes == CGFX_Viewer.VertexAttribute.Usage.UsageType.Normal)
                    //            {
                    //                polygon1.Normal = Converter.ByteArrayToVector3D(new byte[][] { br.ReadBytes(CompCount), br.ReadBytes(CompCount), br.ReadBytes(CompCount) });
                    //            }
                    //            else if (h.VertexAttributeUsageFlag.UsageTypes == CGFX_Viewer.VertexAttribute.Usage.UsageType.Tangent)
                    //            {
                    //                return;
                    //            }
                    //            else if (h.VertexAttributeUsageFlag.UsageTypes == CGFX_Viewer.VertexAttribute.Usage.UsageType.TextureCoordinate0)
                    //            {
                    //                polygon1.TexCoord = new Polygon.TextureCoordinate(BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0), BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0));
                    //            }
                    //            else if (h.VertexAttributeUsageFlag.UsageTypes == CGFX_Viewer.VertexAttribute.Usage.UsageType.TextureCoordinate1)
                    //            {
                    //                polygon1.TexCoord2 = new Polygon.TextureCoordinate(BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0), BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0));
                    //            }
                    //            else if (h.VertexAttributeUsageFlag.UsageTypes == CGFX_Viewer.VertexAttribute.Usage.UsageType.TextureCoordinate2)
                    //            {
                    //                polygon1.TexCoord3 = new Polygon.TextureCoordinate(BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0), BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(CompCount)), 0));
                    //            }
                    //            else if (h.VertexAttributeUsageFlag.UsageTypes == CGFX_Viewer.VertexAttribute.Usage.UsageType.Color)
                    //            {
                    //                polygon1.ColorData = new Polygon.Color(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte());
                    //            }
                    //        }

                    //        PolygonList.Add(polygon1);
                    //    }

                    //}
                    //if (VertexStreamOffset != 0)
                    //{
                    //    var f = VertexStreamLength / VertexDataEntrySize;
                    //}

                    ////int q = VertexStreamLength / VertexDataEntrySize;
                    #endregion

                }

                public Stream()
                {
                    //BufferObject = 0;
                    //LocationFlag = 0;
                    VertexAttributeUsageFlag = new CGFXLibrary.VertexAttribute.Usage(-1);
                    VertexAttributeFlag = new CGFXLibrary.VertexAttribute.Flag(-1);
                    VertexStreamLength = 0;
                    VertexStreamOffset = 0;
                    VertexStreamList = new List<byte>();
                    LocationAddress = 0;
                    MemoryArea = 0;
                    PolygonList = new List<Polygon>();

                    UnknownData1 = 0;
                    UnknownData2 = 0;

                    VertexDataEntrySize = 0;
                    NrVertexStreams = 0;
                    VertexStreamsOffsetListOffset = 0;
                    VertexStreams = new List<VertexStream>();
                }
            }

            public Param Params { get; set; }
            public class Param
            {
                public CGFXLibrary.VertexAttribute.Usage VertexAttributeUsageFlag { get; set; }
                public CGFXLibrary.VertexAttribute.Flag VertexAttributeFlag { get; set; }
                public Component Components { get; set; }
                public class Component
                {
                    public Flags Flags { get; set; }

                    public FormatType FormatTypes
                    {
                        get => (FormatType)Flags.IdentFlag[0];
                        set => Flags.IdentFlag[0] = Convert.ToByte(Enum.ToObject(typeof(FormatType), value));
                    }

                    public enum FormatType
                    {
                        BYTE = 0,
                        UNSIGNED_BYTE = 1,
                        SHORT = 2,//might also be unsigned short
                        FLOAT = 6
                    }

                    public int GetFormatTypeLength()
                    {
                        int n = -1;
                        if (FormatTypes == FormatType.BYTE || FormatTypes == FormatType.UNSIGNED_BYTE) n = 1;
                        if (FormatTypes == FormatType.SHORT) n = 2;
                        if (FormatTypes == FormatType.FLOAT) n = 4;
                        return n;
                    }

                    //public int FormatType { get; set; }

                    /// <summary>
                    /// For example XYZ = 3, ST = 2, RGBA = 4
                    /// </summary>
                    public int ComponentCount { get; set; }

                    public ComponentType ComponentTypeFlag => (ComponentType)ComponentCount;
                    public enum ComponentType
                    {
                        ST = 2,
                        XYZ = 3,
                        RGBA = 4
                    }

                    //public List<float> Vs { get; set; }


                    public void ReadComponent(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        Flags = new Flags(br.ReadBytes(4));
                        //FormatType = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        ComponentCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }

                    public Component()
                    {
                        Flags = new Flags(new List<byte>().ToArray());
                        //FormatType = 0;
                        ComponentCount = 0;
                    }
                }
                public float Scale { get; set; }
                public int AttributeCount { get; set; }
                public int AttributeListOffset { get; set; }
                public List<float> AttributeList { get; set; }

                //public List<Polygon> PolygonList { get; set; }

                public void ReadParam(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    VertexAttributeUsageFlag = new CGFXLibrary.VertexAttribute.Usage(BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0));
                    VertexAttributeFlag = new CGFXLibrary.VertexAttribute.Flag(BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0));
                    Components.ReadComponent(br, BOM);
                    Scale = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    AttributeCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    AttributeListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (AttributeListOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(AttributeListOffset, SeekOrigin.Current);

                        BinaryReadHelper binaryReadHelper = new BinaryReadHelper(br, BOM);
                        var s = binaryReadHelper.ReadArray<float>(AttributeCount, 4);
                        AttributeList = s.ToList();

                        br.BaseStream.Position = Pos;
                    }

                    //VertexData(?)
                }

                public Param()
                {
                    VertexAttributeUsageFlag = new CGFXLibrary.VertexAttribute.Usage(-1);
                    VertexAttributeFlag = new CGFXLibrary.VertexAttribute.Flag(-1);
                    Components = new Component();
                    Scale = 0;
                    AttributeCount = 0;
                    AttributeListOffset = 0;
                    AttributeList = new List<float>();
                }
            }

            public void ReadVertexAttribute(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                VertexAttributeOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (VertexAttributeOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move Offset
                    br.BaseStream.Seek(VertexAttributeOffset, SeekOrigin.Current);

                    Flag = new Flags(br.ReadBytes(4));
                    if (Flag.IdentFlag.SequenceEqual(new byte[] { 0x02, 0x00, 0x00, 0x40 })) Streams.ReadStream(br, BOM);
                    if (Flag.IdentFlag.SequenceEqual(new byte[] { 0x00, 0x00, 0x00, 0x80 })) Params.ReadParam(br, BOM);

                    br.BaseStream.Position = Pos;
                }
            }

            public VertexAttribute()
            {
                VertexAttributeOffset = 0;
                Flag = new Flags(new List<byte>().ToArray());
                Streams = new Stream();
                Params = new Param();
            }
        }
        public int BlendShapeOffset { get; set; }
        public BlendShape BlendShapes { get; set; }
        public class BlendShape
        {
            public int UnknownData1 { get; set; }
            public int UnknownData2 { get; set; }
            public int UnknownData3 { get; set; }
            public int UnknownData4 { get; set; }
            public int UnknownData5 { get; set; }

            public void ReadBlendShape(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownData4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownData5 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            }

            public BlendShape()
            {
                UnknownData1 = 0;
                UnknownData2 = 0;
                UnknownData3 = 0;
                UnknownData4 = 0;
                UnknownData5 = 0;
            }
        }

        public void Read_ShapeData(BinaryReader br, byte[] BOM)
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
            UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            ShapeFlag = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            OrientedBoundingBoxOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (OrientedBoundingBoxOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move NameOffset
                br.BaseStream.Seek(OrientedBoundingBoxOffset, SeekOrigin.Current);

                Flags flags = new Flags(br.ReadBytes(4));
                BoundingBox boundingBox = new BoundingBox(flags);
                boundingBox.Read(br, BOM);
                OrientedBoundingBox = boundingBox;

                br.BaseStream.Position = Pos;
            }

            PositionOffset = Converter.ByteArrayToVector3(new byte[][] { endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)), endianConvert.Convert(br.ReadBytes(4)) });
            PrimitiveSetCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            PrimitiveSetListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (PrimitiveSetListOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move Offset
                br.BaseStream.Seek(PrimitiveSetListOffset, SeekOrigin.Current);

                //Read PrimitiveSet
                for (int yi = 0; yi < PrimitiveSetCount; yi++)
                {
                    PrimitiveSet primitiveSet = new PrimitiveSet();
                    primitiveSet.ReadPrimitiveSet(br, BOM);
                    primitiveSets.Add(primitiveSet);
                }

                br.BaseStream.Position = Pos;
            }

            BaseAddress = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            VertexAttributeCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            VertexAttributeOffsetListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (VertexAttributeOffsetListOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move Offset
                br.BaseStream.Seek(VertexAttributeOffsetListOffset, SeekOrigin.Current);

                for (int yi = 0; yi < VertexAttributeCount; yi++)
                {
                    //Read VertexAttribute
                    VertexAttribute vertexAttribute = new VertexAttribute();
                    vertexAttribute.ReadVertexAttribute(br, BOM);
                    VertexAttributes.Add(vertexAttribute);
                }

                br.BaseStream.Position = Pos;
            }

            BlendShapeOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (BlendShapeOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move Offset
                br.BaseStream.Seek(BlendShapeOffset, SeekOrigin.Current);

                BlendShapes.ReadBlendShape(br, BOM);

                br.BaseStream.Position = Pos;
            }
        }

        public Shape()
        {
            SOBJ_Header = "SOBJ".ToCharArray();
            Revision = 0;
            SOBJNameOffset = 0;
            Revision = 0;
            SOBJNameOffset = 0;
            UnknownData2 = 0;
            UnknownData3 = 0;
            ShapeFlag = 0;
            OrientedBoundingBoxOffset = 0;
            OrientedBoundingBox = new BoundingBox(new Flags(new List<byte>().ToArray()));
            PositionOffset = new Vector3(0, 0, 0);
            PrimitiveSetCount = 0;
            PrimitiveSetListOffset = 0;
            primitiveSets = new List<PrimitiveSet>();
            BaseAddress = 0;
            VertexAttributeCount = 0;
            VertexAttributeOffsetListOffset = 0;
            VertexAttributes = new List<VertexAttribute>();
            BlendShapeOffset = 0;
            BlendShapes = new BlendShape();
        }

        public override string ToString()
        {
            return Name;
        }
    }

}
