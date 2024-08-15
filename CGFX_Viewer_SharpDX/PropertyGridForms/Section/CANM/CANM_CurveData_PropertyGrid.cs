using CGFX_Viewer_SharpDX.CGFXPropertyGridSet;
using CGFXLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace CGFX_Viewer_SharpDX.PropertyGridForms.Section.CANM
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    public class CANM_CurveData_PropertyGrid
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


        public int AnimationAttributeTypeValue { get; set; } //Type : 1 => KeyData1, 3 => KeyData1, 2 , 7 => TXOB
        public AnimationAttributeType AnimAttributeType
        {
            get
            {
                return (AnimationAttributeType)Enum.ToObject(typeof(AnimationAttributeType), AnimationAttributeTypeValue);
            }
        }

        public class KeyData
        {
            public byte[] UnknownByteArray0 { get; set; } //0x4
            public float UnknownFloatValue0 { get; set; }
            public float UnknownFloatValue1 { get; set; }

            public int UnknownIntValue0 { get; set; }
            public int UnknownDataCount { get; set; }

            public List<UnknownDataSet> UnknownDataSetList { get; set; } = new List<UnknownDataSet>();
            public List<UnknownDataSet> UnknownDataSet_List { get => UnknownDataSetList; set => UnknownDataSetList = value; }

            public class UnknownDataSet
            {
                public int UnknownDataOffset { get; set; }

                [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
                public UnknownData Unknown_Data { get; set; } = new UnknownData();
                public class UnknownData
                {
                    public struct Curve2D
                    {
                        public float X { get; set; }
                        public float Y { get; set; }

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

                    public List<Curve2D> Curve2DList { get; set; } = new List<Curve2D>();
                    public List<Curve2D> Curve2D_List { get => Curve2DList; set => Curve2DList = value; }

                    public float UnknownFloatValue8 { get; set; }

                    public UnknownData(CGFXLibrary.CGFXSection.DataComponent.AttributeData.KeyData.UnknownDataSet.UnknownData unknownData)
                    {
                        UnknownFloatValue3 = unknownData.UnknownFloatValue3;
                        UnknownFloatValue4 = unknownData.UnknownFloatValue4;

                        UnknownIntValue3 = unknownData.UnknownIntValue3;
                        UnknownDataCount = unknownData.UnknownDataCount;

                        for (int i = 0; i < unknownData.UnknownDataCount; i++)
                        {
                            Curve2D_List.Add(new Curve2D(unknownData.Curve2D_List[i].X, unknownData.Curve2D_List[i].Y));
                        }

                        UnknownFloatValue8 = 0;
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

                    public override string ToString()
                    {
                        return "UnknownData";
                    }
                }

                public UnknownDataSet(CGFXLibrary.CGFXSection.DataComponent.AttributeData.KeyData.UnknownDataSet unknownDataSet)
                {
                    UnknownDataOffset = unknownDataSet.UnknownDataOffset;
                    Unknown_Data = new UnknownData(unknownDataSet.Unknown_Data);
                }

                public UnknownDataSet()
                {
                    UnknownDataOffset = 0;
                    Unknown_Data = new UnknownData();
                }
            }

            public KeyData(CGFXLibrary.CGFXSection.DataComponent.AttributeData.KeyData keyData)
            {
                UnknownByteArray0 = keyData.UnknownByteArray0;
                UnknownFloatValue0 = keyData.UnknownFloatValue0;
                UnknownFloatValue1 = keyData.UnknownFloatValue1;
                UnknownIntValue0 = keyData.UnknownIntValue0;
                UnknownDataCount = keyData.UnknownDataCount;

                for (int i = 0; i < keyData.UnknownDataCount; i++)
                {
                    UnknownDataSet_List.Add(new UnknownDataSet(keyData.UnknownDataSetList[i]));
                }
            }

            public KeyData()
            {
                UnknownByteArray0 = new byte[4];
                UnknownFloatValue0 = 0;
                UnknownFloatValue1 = 0;
                UnknownIntValue0 = 0;
                UnknownDataCount = 0;
                UnknownDataSetList = new List<UnknownDataSet>();
            }

            public override string ToString()
            {
                return "KeyData";
            }
        }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public Type0_Attribute Type0Attribute { get; set; } = new Type0_Attribute();
        public class Type0_Attribute
        {
            public int UnknownDataOffset1 { get; set; }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public KeyData KeyData1 { get; set; } = new KeyData();

            public Type0_Attribute(CGFXLibrary.CGFXSection.DataComponent.AttributeData.Type0_Attribute type0_Attribute)
            {
                UnknownDataOffset1 = type0_Attribute.UnknownDataOffset1;
                KeyData1 = new KeyData(type0_Attribute.KeyData1);
            }

            public Type0_Attribute()
            {
                UnknownDataOffset1 = 0;
                KeyData1 = new KeyData();
            }

            public override string ToString()
            {
                return "Type0 Attribute";
            }
        }

        //public class Type1_Attribute
        //{

        //}

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public Type3_Attribute Type3Attribute { get; set; } = new Type3_Attribute();
        public class Type3_Attribute
        {
            public int UnknownDataOffset1 { get; set; }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public KeyData KeyData1 { get; set; } = new KeyData();

            public int UnknownDataOffset2 { get; set; }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public KeyData KeyData2 { get; set; } = new KeyData();

            public Type3_Attribute(CGFXLibrary.CGFXSection.DataComponent.AttributeData.Type3_Attribute type3_Attribute)
            {
                UnknownDataOffset1 = type3_Attribute.UnknownDataOffset1;
                KeyData1 = new KeyData(type3_Attribute.KeyData1);

                UnknownDataOffset2 = type3_Attribute.UnknownDataOffset2;
                KeyData2 = new KeyData(type3_Attribute.KeyData2);
            }

            public Type3_Attribute()
            {
                UnknownDataOffset1 = 0;
                KeyData1 = new KeyData();

                UnknownDataOffset2 = 0;
                KeyData2 = new KeyData();
            }

            public override string ToString()
            {
                return "Type3 Attribute";
            }
        }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public Type7_Attribute Type7Attribute { get; set; } = new Type7_Attribute();
        public class Type7_Attribute
        {
            public int UnknownDataOffset1 { get; set; }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public KeyData KeyData1 { get; set; } = new KeyData();

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public TXOBInfo TXOBInfoData { get; set; } = new TXOBInfo();
            public class TXOBInfo
            {
                public int TXOBDataCount { get; set; }
                public int TXOBDataListOffset { get; set; }

                public List<TXOBData> TXOBDataList { get; set; } = new List<TXOBData>();
                public List<TXOBData> TXOBData_List { get => TXOBDataList; set => TXOBDataList = value; }
                public class TXOBData
                {
                    public int DataOffset { get; set; }
                    //public CGFXData CGFXData { get; set; }

                    public TXOBData(CGFXLibrary.CGFXSection.DataComponent.AttributeData.Type7_Attribute.TXOBInfo.TXOBData tXOBData)
                    {
                        DataOffset = tXOBData.DataOffset;
                        //CGFXData = tXOBData.CGFXData;
                    }

                    public TXOBData()
                    {
                        DataOffset = 0;
                        //CGFXData = new CGFXData(new byte[4]);
                    }
                }

                public TXOBInfo(CGFXLibrary.CGFXSection.DataComponent.AttributeData.Type7_Attribute.TXOBInfo tXOBInfo)
                {
                    TXOBDataCount = tXOBInfo.TXOBDataCount;
                    TXOBDataListOffset = tXOBInfo.TXOBDataListOffset;

                    for (int i = 0; i < tXOBInfo.TXOBDataCount; i++)
                    {
                        TXOBData_List.Add(new TXOBData(tXOBInfo.TXOBData_List[i]));
                    }
                }

                public TXOBInfo()
                {
                    TXOBDataCount = 0;
                    TXOBDataListOffset = 0;
                    TXOBData_List = new List<TXOBData>();
                }
            }

            public Type7_Attribute(CGFXLibrary.CGFXSection.DataComponent.AttributeData.Type7_Attribute type7_Attribute)
            {
                UnknownDataOffset1 = type7_Attribute.UnknownDataOffset1;
                KeyData1 = new KeyData(type7_Attribute.KeyData1);

                TXOBInfoData = new TXOBInfo(type7_Attribute.TXOBInfoData);
            }

            public Type7_Attribute()
            {
                UnknownDataOffset1 = 0;
                KeyData1 = new KeyData();

                TXOBInfoData = new TXOBInfo();
            }

            public override string ToString()
            {
                return "Type7 Attribute";
            }
        }

        //public class Type9_Attribute
        //{

        //}

        public CANM_CurveData_PropertyGrid(CGFXLibrary.CGFXSection.DataComponent.AnimationData animationData)
        {
            AnimationAttributeTypeValue = animationData.AttributeDataSet.AnimationAttributeTypeValue;
            //if (AnimAttributeType == AttributeType.Type0)
            //{
            //    Type0Attribute = new Type0_Attribute(animationData.AttributeDataSet.Type0Attribute);
            //}
            //else if (AnimAttributeType == AttributeType.Type3)
            //{
            //    Type3Attribute = new Type3_Attribute(animationData.AttributeDataSet.Type3Attribute);
            //}
            //else if (AnimAttributeType == AttributeType.Type7)
            //{
            //    Type7Attribute = new Type7_Attribute(animationData.AttributeDataSet.Type7Attribute);
            //}
        }
    }
}
