using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CGFXLibrary.CGFXFormat;

namespace CGFXLibrary.CGFXSection.DataComponent
{
    public class AnimationData : IO.BinaryIOInterface.BinaryIO
    {
        //Animation0 => Flag : 08 00 00 00
        //Animation1 => Flag : 20 00 00 00
        //Animation2 => Flag : 28 00 00 00
        //Animation3 => Flag : 30 00 00 00
        //Animation4 => Flag : 38 00 00 00
        public uint Flag { get; set; } //bitOperation

        //public 

        public string Name; //AttributeNameArrayOffset
        public int AttributeNameArrayOffset { get; set; } //Name (?)

        public AttributeData AttributeDataSet { get; set; }

        /// <summary>
        /// ReadCANM_AnimationDataSet
        /// </summary>
        /// <param name="br"></param>
        /// <param name="BOM"></param>
        public override void Read(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            Flag = BitConverter.ToUInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            AttributeNameArrayOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (AttributeNameArrayOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move NameOffset
                br.BaseStream.Seek(AttributeNameArrayOffset, SeekOrigin.Current);

                ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                readByteLine.ReadByte(br, 0x00);

                Name = new string(readByteLine.ConvertToCharArray());

                br.BaseStream.Position = Pos;
            }

            AttributeDataSet.Read_AttributeData(br, BOM);
        }

        public override void Write(BinaryWriter bw, byte[] BOM = null)
        {
            throw new NotImplementedException();
        }

        public AnimationData()
        {
            AttributeNameArrayOffset = 0;
            AttributeDataSet = new AttributeData();
        }
    }

    public class AttributeData
    {
        public enum AnimationAttributeType
        {
            Float = 0,
            Int = 1,
            Boolean = 2,
            Vector2 = 3,
            Vector3 = 4,
            Transform = 5,
            RGBA_Color = 6,
            Texture = 7,
            BakedTransform = 8,
            FullBaked = 9
        }

        //public int PathDataOffset { get; set; }

        public int AnimationAttributeTypeValue { get; set; } //Type : 1 => KeyData1, 3 => KeyData1, 2 , 7 => TXOB
        public AnimationAttributeType AnimationAttribute_Type
        {
            get
            {
                return (AnimationAttributeType)Enum.ToObject(typeof(AnimationAttributeType), AnimationAttributeTypeValue);
            }
        }

        public class AnimationKeyData
        {
            public float StartFrame { get; set; }
            public float EndFrame { get; set; }
            public byte PreRepeatMethod { get; set; }
            public byte PostRepeatMethod { get; set; }
            public ushort Padding { get; set; }
            public uint Flags { get; set; }

            //

            public AnimationKeyData()
            {

            }
        }



        public class KeyData
        {
            public byte[] UnknownByteArray0 { get; set; } //0x4
            public float UnknownFloatValue0 { get; set; }
            public float UnknownFloatValue1 { get; set; }

            public int UnknownIntValue0 { get; set; }
            //public int UnknownIntValue1 { get; set; }
            public int UnknownDataCount { get; set; }

            public List<UnknownDataSet> UnknownDataSetList { get; set; }
            public class UnknownDataSet
            {
                public int UnknownDataOffset { get; set; }
                public UnknownData Unknown_Data { get; set; }
                public class UnknownData
                {
                    public struct Curve2D
                    {
                        public float X { get; set; }
                        public float Y { get; set; }

                        public static int GetLength()
                        {
                            return sizeof(float) + sizeof(float);
                        }

                        public Curve2D(float X, float Y)
                        {
                            this.X = X;
                            this.Y = Y;
                        }
                    }

                    public float UnknownFloatValue3 { get; set; }
                    public float UnknownFloatValue4 { get; set; }

                    public int UnknownIntValue3 { get; set; }
                    public int UnknownDataCount { get; set; }

                    public List<Curve2D> Curve2D_List { get; set; }

                    public float UnknownFloatValue8 { get; set; }

                    public void Read_UnknownData(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        UnknownFloatValue3 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownFloatValue4 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);

                        UnknownIntValue3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownDataCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (UnknownDataCount != 0)
                        {
                            for (int i = 0; i < UnknownDataCount; i++)
                            {
                                Curve2D curve2D = new Curve2D(BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0), BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0));
                                Curve2D_List.Add(curve2D);
                            }
                        }

                        UnknownFloatValue8 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }

                    public UnknownData()
                    {
                        UnknownFloatValue3 = 0;
                        UnknownFloatValue4 = 0;

                        UnknownIntValue3 = 0;
                        UnknownDataCount = 0;
                        Curve2D_List = new List<Curve2D>();

                        UnknownFloatValue8 = 0;
                    }
                }

                public void Read_UnknownDataSet(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    UnknownDataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (UnknownDataOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(UnknownDataOffset, SeekOrigin.Current);

                        Unknown_Data.Read_UnknownData(br, BOM);

                        br.BaseStream.Position = Pos;
                    }
                }

                public UnknownDataSet()
                {
                    UnknownDataOffset = 0;
                    Unknown_Data = new UnknownData();
                }
            }

            public void Read_KeyData(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                UnknownByteArray0 = endianConvert.Convert(br.ReadBytes(4));
                UnknownFloatValue0 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownFloatValue1 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownIntValue0 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownDataCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                //UnknownDataCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (UnknownDataCount != 0)
                {
                    for (int i = 0; i < UnknownDataCount; i++)
                    {
                        UnknownDataSet unknownDataSet = new UnknownDataSet();
                        unknownDataSet.Read_UnknownDataSet(br, BOM);
                        UnknownDataSetList.Add(unknownDataSet);
                    }
                }
            }

            public KeyData()
            {
                UnknownByteArray0 = new byte[4];
                UnknownFloatValue0 = 0;
                UnknownFloatValue1 = 0;
                UnknownIntValue0 = 0;
                //UnknownIntValue1 = 0;
                UnknownDataCount = 0;
                UnknownDataSetList = new List<UnknownDataSet>();
            }
        }

        public Type0_Attribute Type0Attribute { get; set; }
        public class Type0_Attribute
        {
            public int UnknownDataOffset1 { get; set; }
            public KeyData KeyData1 { get; set; }

            public void Read_Type0Attr(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                UnknownDataOffset1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (UnknownDataOffset1 != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move NameOffset
                    br.BaseStream.Seek(UnknownDataOffset1, SeekOrigin.Current);

                    KeyData1.Read_KeyData(br, BOM);

                    br.BaseStream.Position = Pos;
                }
            }

            public Type0_Attribute()
            {
                UnknownDataOffset1 = 0;
                KeyData1 = new KeyData();
            }
        }

        //public class Type1_Attribute
        //{

        //}

        public Type3_Attribute Type3Attribute { get; set; }
        public class Type3_Attribute
        {
            public int UnknownDataOffset1 { get; set; }
            public KeyData KeyData1 { get; set; }

            public int UnknownDataOffset2 { get; set; }
            public KeyData KeyData2 { get; set; }

            public void Read_Type3Attr(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);

                UnknownDataOffset1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (UnknownDataOffset1 != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move NameOffset
                    br.BaseStream.Seek(UnknownDataOffset1, SeekOrigin.Current);

                    KeyData1.Read_KeyData(br, BOM);

                    br.BaseStream.Position = Pos;
                }

                UnknownDataOffset2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (UnknownDataOffset2 != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move NameOffset
                    br.BaseStream.Seek(UnknownDataOffset2, SeekOrigin.Current);

                    KeyData2.Read_KeyData(br, BOM);

                    br.BaseStream.Position = Pos;
                }
            }

            public Type3_Attribute()
            {
                UnknownDataOffset1 = 0;
                KeyData1 = new KeyData();

                UnknownDataOffset2 = 0;
                KeyData2 = new KeyData();
            }
        }

        public Type7_Attribute Type7Attribute { get; set; }
        public class Type7_Attribute
        {
            public int UnknownDataOffset1 { get; set; }
            public KeyData KeyData1 { get; set; }

            public TXOBInfo TXOBInfoData { get; set; }
            public class TXOBInfo
            {
                public int TXOBDataCount { get; set; }
                public int TXOBDataListOffset { get; set; }

                public List<TXOBData> TXOBData_List { get; set; }
                public class TXOBData
                {
                    public int DataOffset { get; set; }
                    public CGFXData CGFXData { get; set; }

                    public void Read_TXOBData(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        DataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (DataOffset != 0)
                        {
                            long Pos = br.BaseStream.Position;

                            br.BaseStream.Seek(-4, SeekOrigin.Current);

                            //Move TXOBData
                            br.BaseStream.Seek(DataOffset, SeekOrigin.Current);

                            CGFXData = new CGFXData(null, true);
                            CGFXData.Reader(br, BOM);

                            br.BaseStream.Position = Pos;
                        }
                    }

                    public TXOBData()
                    {
                        DataOffset = 0;
                        CGFXData = new CGFXData(null, true);
                    }
                }

                public void Read_TXOBInfo(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    TXOBDataCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    TXOBDataListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (TXOBDataListOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move TXOBDataListOffset
                        br.BaseStream.Seek(TXOBDataListOffset, SeekOrigin.Current);

                        for (int i = 0; i < TXOBDataCount; i++)
                        {
                            TXOBData tXOBData = new TXOBData();
                            tXOBData.Read_TXOBData(br, BOM);
                        }

                        br.BaseStream.Position = Pos;
                    }
                }

                public TXOBInfo()
                {
                    TXOBDataCount = 0;
                    TXOBDataListOffset = 0;
                    TXOBData_List = new List<TXOBData>();
                }
            }

            public void Read_Type7Attr(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);

                UnknownDataOffset1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (UnknownDataOffset1 != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move NameOffset
                    br.BaseStream.Seek(UnknownDataOffset1, SeekOrigin.Current);

                    KeyData1.Read_KeyData(br, BOM);

                    br.BaseStream.Position = Pos;
                }

                TXOBInfoData.Read_TXOBInfo(br, BOM);
            }

            public Type7_Attribute()
            {
                UnknownDataOffset1 = 0;
                KeyData1 = new KeyData();

                TXOBInfoData = new TXOBInfo();
            }
        }

        //public class Type9_Attribute
        //{

        //}

        public void Read_AttributeData(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            //PathDataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            AnimationAttributeTypeValue = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (AnimationAttribute_Type == AnimationAttributeType.Vector2)
            {

            }
            else if (AnimationAttribute_Type == AnimationAttributeType.BakedTransform)
            {

            }

            //if (Attribute_Type == AnimationAttributeType.Type0)
            //{
            //    Type0Attribute.Read_Type0Attr(br, BOM);
            //}
            //else if (Attribute_Type == AnimationAttributeType.Type3)
            //{
            //    Type3Attribute.Read_Type3Attr(br, BOM);
            //}
            //else if (Attribute_Type == AnimationAttributeType.Type7)
            //{
            //    Type7Attribute.Read_Type7Attr(br, BOM);
            //}
        }

        public AttributeData()
        {
            AnimationAttributeTypeValue = 0;
            Type0Attribute = new Type0_Attribute();
            Type3Attribute = new Type3_Attribute();
            Type7Attribute = new Type7_Attribute();
        }
    }


}
