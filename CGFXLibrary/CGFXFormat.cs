using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using SharpDX;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.ComponentModel;
using System.Collections;
using static CGFXLibrary.Flags;
using System.Xml.Linq;
using CGFXLibrary.CGFXSection;

namespace CGFXLibrary
{
    public class CGFXFormat
    {
        public enum CGFXEntryData
        {
            Models = 1, //CMDL
            Textures = 2, //TXOB
            LookupTables = 3, //LUTS
            Materials = 4, //MTOB
            Shaders = 5, //SHDR
            Cameras = 6,
            Lights = 7,
            Fogs = 8, //CFOG
            Environments = 9, //CENV
            Skeleton_Animations = 10,
            Texture_Animations = 11,
            Visibility_Animations = 12,
            Camera_Animations = 13,
            Light_Animations = 14,
            Fog_Animations = 15,
            Emitters = 16
        }

        public static Dictionary<int, string> GetAllCGFXEntryName()
        {
            return Enum.GetValues(typeof(CGFXEntryData)).Cast<CGFXEntryData>().ToDictionary(Key => (int)Key, Value => Enum.GetName(typeof(CGFXEntryData), Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string[] GetCGFXEntryNameArray()
        {
            return Enum.GetNames(typeof(CGFXEntryData));
        }

        public class CGFX
        {
            public char[] CGFX_Header { get; set; } //0x4
            public byte[] BOM { get; set; } //0x2
            public short CGFXHeaderSize { get; set; } //0x2
            public int Revision { get; set; } //0x4
            public int FileSize { get; set; } //0x4
            public int NumOfEntries { get; set; } //0x4
            public DATA Data { get; set; }
            public class DATA
            {
                public char[] DATA_Header { get; set; } //0x4
                public int DATASize { get; set; } //0x4
                public Dictionary<CGFXEntryData, DictOffset> DictOffset_Dictionary { get; set; }

                public class DictOffset
                {
                    public int NumOfEntries { get; set; }
                    public int OffsetToDICT { get; set; }

                    public DictOffset(int NumOfEntries, int OffsetToDICT)
                    {
                        this.NumOfEntries = NumOfEntries;
                        this.OffsetToDICT = OffsetToDICT;
                    }

                    public DictOffset()
                    {
                        NumOfEntries = 0;
                        OffsetToDICT = 0;
                    }

                    public void ReadDictOffset(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        NumOfEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        OffsetToDICT = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }
                }

                public DATA()
                {
                    DATA_Header = "DATA".ToCharArray();
                    DATASize = 0;
                    DictOffset_Dictionary = new Dictionary<CGFXEntryData, DictOffset>();
                    //DictOffset_Dictionary = new Dictionary<string, DictOffset>();
                }

                public void ReadDATA(BinaryReader br, byte[] BOM)
                {
                    DATA_Header = br.ReadChars(4);
                    if (new string(DATA_Header) != "DATA") throw new Exception("不明なフォーマットです");
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    DATASize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    foreach (var EntryName in GetCGFXEntryNameArray())
                    {
                        DictOffset dictOffset = new DictOffset();
                        dictOffset.ReadDictOffset(br, BOM);
                        DictOffset_Dictionary.Add((CGFXEntryData)Enum.Parse(typeof(CGFXEntryData), EntryName), dictOffset);
                    }
                }
            }

            public Dictionary<CGFXEntryData, DICT> DICTAndSectionData { get; set; }

            public void ReadCGFX(BinaryReader br)
            {
                CGFX_Header = br.ReadChars(4);
                if (new string(CGFX_Header) != "CGFX") throw new Exception("不明なフォーマットです");
                BOM = br.ReadBytes(2);

                EndianConvert endianConvert = new EndianConvert(BOM);
                CGFXHeaderSize = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                Revision = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                FileSize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                NumOfEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                Data.ReadDATA(br, BOM);

                foreach (var EntryName in GetCGFXEntryNameArray())
                {
                    CGFXEntryData Entry = (CGFXEntryData)Enum.Parse(typeof(CGFXEntryData), EntryName);
                    if (Data.DictOffset_Dictionary[Entry].NumOfEntries != 0)
                    {
                        DICT dICT = new DICT();
                        dICT.ReadDICT(br, BOM);
                        DICTAndSectionData.Add(Entry, dICT);
                    }
                }
            }

            public CGFX()
            {
                CGFX_Header = "CGFX".ToCharArray();
                BOM = new byte[2];
                CGFXHeaderSize = 0;
                Revision = 0;
                FileSize = 0;
                NumOfEntries = 0;
                Data = new DATA();
                DICTAndSectionData = new Dictionary<CGFXEntryData, DICT>();
                //DICTAndSectionData = new Dictionary<string, DICT>();
            }
        }

        public class CGFXData
        {
            public byte[] IdentificationFlags { get; set; } = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
            public Flags Flag => new Flags(IdentificationFlags);

            #region SectionData
            //Models
            public CMDL CMDLSection { get; set; }

            //Textures
            public TXOB TXOBSection { get; set; }

            public TXOB TXOB_MaterialSection { get; set; }

            //LUTS
            public LUTS LUTSSection { get; set; } //0x00000004

            //Materials
            public MTOB MTOBSection { get; set; } //0x00000008

            //Shaders
            public SHDR SHDRSection { get; set; } //0x01000080

            //Cameras
            public CCAM CCAMSection { get; set; } //0x0A000040

            //Lights
            public CVLT CVLTSection { get; set; } //0x22020040
            public CHLT CHLTSection { get; set; } //0x22010040
            public CALT CALTSection { get; set; } //0x22040040
            public CFLT CFLTSection { get; set; } //0xA2000040

            //Fogs
            public CFOG CFOGSection { get; set; }

            //Environments
            public CENV CENVSection { get; set; }

            //Skeleton Animations
            //Texture Animations
            //Visibility Animations
            //Camera Animations
            //Light Animations
            public CANM CANMSection { get; set; } //0x00000000 (IdentFlag : None)

            //Particle
            public CNOD CNODSection { get; set; } //0x01000040

            //Emitter
            public PEMT PEMTSection { get; set; } //0x06000040

            #endregion

            #region UserData
            public StringData String_Data { get; set; }
            public class StringData
            {
                public long EndPos;

                public string UDName;

                //Flag : 00 00 00 10
                public int UserDataStringNameOffset { get; set; } //0x4
                public byte[] Flag2 { get; set; } //0x4(Default Value : 02 00 00 00(?))
                public byte[] Flag3 { get; set; } //0x4(Default Value : 02 00 00 00(?))
                public int STRING_ValueCount { get; set; }

                public List<UserDataItem_String> UserDataItem_String_List { get; set; }
                public class UserDataItem_String
                {
                    public int Offset { get; set; } //0x4
                    public string StringData { get; set; }

                    public UserDataItem_String(int InputOffset, string InputString)
                    {
                        Offset = InputOffset;
                        StringData = InputString;
                    }

                    public void ReadData(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        Offset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(Offset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadMultiByte(br, 0x00);

                        StringData = new string(readByteLine.ConvertToCharArray().Where(x => x != '\0').ToArray());

                        br.BaseStream.Position = Pos;
                    }
                }

                public List<UserDataItem_String> ReadUserDataStringList(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    UserDataStringNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (UserDataStringNameOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(UserDataStringNameOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        UDName = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    Flag2 = endianConvert.Convert(br.ReadBytes(4));
                    Flag3 = endianConvert.Convert(br.ReadBytes(4));
                    STRING_ValueCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    for (int UDStrCount = 0; UDStrCount < STRING_ValueCount; UDStrCount++)
                    {
                        UserDataItem_String userData = new UserDataItem_String(0, "");
                        userData.ReadData(br, BOM);
                        UserDataItem_String_List.Add(userData);
                    }

                    EndPos = br.BaseStream.Position;

                    return UserDataItem_String_List;
                }

                public StringData()
                {
                    UserDataStringNameOffset = 0;
                    Flag2 = new List<byte>().ToArray();
                    Flag3 = new List<byte>().ToArray();
                    STRING_ValueCount = 0;
                    UserDataItem_String_List = new List<UserDataItem_String>();
                }

                public override string ToString()
                {
                    return UDName;
                }
            }

            public Int32Data Int32_Data { get; set; }
            public class Int32Data
            {
                public long EndPos;
                public string UDName;

                //Flag : 00 00 00 20
                public int UserDataInt32NameOffset { get; set; } //0x4
                public byte[] Flag2 { get; set; } //0x4(Default Value : 01 00 00 00(?))
                public int INT32_ValueCount { get; set; } //0x4

                public List<UserDataItem_INT32> UserDataItem_Int32Data_List { get; set; }
                public class UserDataItem_INT32
                {
                    public int UserDataItem_Int32_Value { get; set; } //0x4

                    public UserDataItem_INT32(int Input)
                    {
                        UserDataItem_Int32_Value = Input;
                    }

                    public void ReadData(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        UserDataItem_Int32_Value = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }
                }

                public List<UserDataItem_INT32> ReadUserDataInt32List(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    UserDataInt32NameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (UserDataInt32NameOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(UserDataInt32NameOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        UDName = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    Flag2 = endianConvert.Convert(br.ReadBytes(4));
                    INT32_ValueCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    for (int UDStrCount = 0; UDStrCount < INT32_ValueCount; UDStrCount++)
                    {
                        UserDataItem_INT32 userData = new UserDataItem_INT32(0);
                        userData.ReadData(br, BOM);
                        UserDataItem_Int32Data_List.Add(userData);
                    }

                    EndPos = br.BaseStream.Position;

                    return UserDataItem_Int32Data_List;
                }

                public Int32Data()
                {
                    UserDataInt32NameOffset = 0;
                    Flag2 = new List<byte>().ToArray();
                    INT32_ValueCount = 0;
                    UserDataItem_Int32Data_List = new List<UserDataItem_INT32>();
                }

                public override string ToString()
                {
                    return UDName;
                }
            }

            public RealNumber RealNumber_Data { get; set; }
            public class RealNumber
            {
                public long EndPos;

                public string UDName;
                public string UDName_Sub;

                //Flag : 00 00 00 80
                public int RealNumberNameOffset { get; set; } //0x4
                public int SubNameStringOffset { get; set; } //0x4(Using Fog Animation, ColorData, Default = 0)
                public int REALNUMBERCount { get; set; } //0x4

                public List<UserDataItem_REALNUMBER> UserDataItem_RealNumber_List { get; set; }
                public class UserDataItem_REALNUMBER
                {
                    public float FloatValue { get; set; }

                    public UserDataItem_REALNUMBER(float value = 0)
                    {
                        FloatValue = value;
                    }

                    //public UserDataItem_REALNUMBER(RealNumberType realNumberType)
                    //{
                    //    RealNumberType = realNumberType;
                    //    FloatValue = 0;
                    //    if (realNumberType == RealNumberType.Color) color = new Color(0, 0, 0, 0);
                    //    else if (realNumberType == RealNumberType.Float) FloatValue = 0;
                    //}

                    public void ReadData(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        FloatValue = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }
                }

                public List<UserDataItem_REALNUMBER> ReadUserDataList(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    RealNumberNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (RealNumberNameOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(RealNumberNameOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        UDName = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    SubNameStringOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (SubNameStringOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(SubNameStringOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        UDName_Sub = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    REALNUMBERCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    for (int UDStrCount = 0; UDStrCount < REALNUMBERCount; UDStrCount++)
                    {
                        UserDataItem_REALNUMBER userDataItem_REALNUMBER = new UserDataItem_REALNUMBER();
                        userDataItem_REALNUMBER.ReadData(br, BOM);

                        UserDataItem_RealNumber_List.Add(userDataItem_REALNUMBER);
                    }

                    EndPos = br.BaseStream.Position;

                    return UserDataItem_RealNumber_List;
                }

                public RealNumber()
                {
                    RealNumberNameOffset = 0;
                    SubNameStringOffset = 0;
                    REALNUMBERCount = 0;
                    UserDataItem_RealNumber_List = new List<UserDataItem_REALNUMBER>();
                }

                public override string ToString()
                {
                    return "Main : " + UDName + " | " + "Sub : " + UDName_Sub;
                }
            }
            #endregion

            #region SOBJSection
            public SOBJ SOBJ_Mesh_Section { get; set; }
            public SOBJ SOBJ_Shape_Section { get; set; }
            #endregion


            public ValueSet ValueData { get; set; }
            public class ValueSet
            {
                public enum ValueType
                {
                    Color4 = 0,
                    Int32 = 1,
                }

                public long EndPos;

                public string Name;

                //Flag : 00 00 00 80
                public int RealNumberNameOffset { get; set; } //0x4
                public int DataTypeValue { get; set; }
                public ValueType DataTypeEnum
                {
                    get
                    {
                        return (ValueType)Enum.ToObject(typeof(ValueType), DataTypeValue);
                    }
                }

                public int REALNUMBERCount { get; set; } //0x4

                public int UnknownData { get; set; }

                public List<ValueDataItem> ValueDataItem_List { get; set; }
                public class ValueDataItem
                {
                    public ValueType ValueItemType;
                    //public float FloatValue { get; set; }
                    public int IntValue { get; set; }
                    public Color color { get; set; }
                    public class Color
                    {
                        public int R { get; set; }
                        public int G { get; set; }
                        public int B { get; set; }
                        public int A { get; set; }

                        public Color(int i1, int i2, int i3, int i4)
                        {
                            R = i1;
                            G = i2;
                            B = i3;
                            A = i4;
                        }

                        public void ReadColorSet(BinaryReader br)
                        {
                            R = br.ReadByte();
                            G = br.ReadByte();
                            B = br.ReadByte();
                            A = br.ReadByte();
                        }
                    }

                    public ValueDataItem(ValueType realNumberType)
                    {
                        ValueItemType = realNumberType;
                        if (realNumberType == ValueType.Color4) color = new Color(0, 0, 0, 0);
                        else if (realNumberType ==  ValueType.Int32) IntValue = 0;
                    }

                    public void ReadData(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        if (ValueItemType == ValueType.Color4)
                        {
                            color.ReadColorSet(br);
                        }
                        else if (ValueItemType == ValueType.Int32)
                        {
                            IntValue = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        }
                    }
                }

                public List<ValueDataItem> ReadValueDataItemList(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    RealNumberNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (RealNumberNameOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(RealNumberNameOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        Name = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    DataTypeValue = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    REALNUMBERCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownData = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    int count = REALNUMBERCount / UnknownData;

                    for (int UDStrCount = 0; UDStrCount < count; UDStrCount++)
                    {
                        ValueDataItem valueDataItem = new ValueDataItem(DataTypeEnum);
                        valueDataItem.ReadData(br, BOM);

                        ValueDataItem_List.Add(valueDataItem);
                    }

                    EndPos = br.BaseStream.Position;

                    return ValueDataItem_List;
                }

                public ValueSet()
                {
                    RealNumberNameOffset = 0;
                    DataTypeValue = 0;
                    REALNUMBERCount = 0;
                    ValueDataItem_List = new List<ValueDataItem>();
                }

                public override string ToString()
                {
                    return "Main : " + Name;
                }
            }

            public NameSet Name_Set { get; set; }
            public class NameSet
            {
                public string Name;
                public int NameOffset { get; set; }
                public int UnknownData1 { get; set; }

                public void ReadNameSet(BinaryReader br, byte[] BOM)
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
                }

                //public NameSet(string Name)
                //{

                //}

                public NameSet()
                {
                    NameOffset = 0;
                    UnknownData1 = 0;
                }

                public override string ToString()
                {
                    return Name;
                }
            }

            public AnimationData AnimationDataSet { get; set; }
            public class AnimationData
            {
                public string Name; //UnknownDataOffset0
                public int UnknownData0 { get; set; }
                public int UnknownDataOffset0 { get; set; } //Name (?)
                public int UnknownData1 { get; set; }
                public int UnknownData2 { get; set; }
                public int UnknownDataOffset1 { get; set; }
                //public AnimationData Animation_Data { get; set; }

                public void ReadCANM_AnimationDataSet(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    UnknownData0 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownDataOffset0 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (UnknownDataOffset0 != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(UnknownDataOffset0, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        Name = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownDataOffset1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                }

                public AnimationData()
                {
                    UnknownData0 = 0;
                    UnknownDataOffset0 = 0;
                    UnknownData1 = 0;
                    UnknownData2 = 0;
                    UnknownDataOffset1 = 0;
                }
            }


            #region NewReadMethod (WIP)
            //public void Test<T>(BinaryReader br, byte[] BOM, bool HasHeader, bool HasIdentFlag)
            //{
            //    if (HasIdentFlag == true)
            //    {

            //    }
            //    else
            //    {

            //    }

            //    if (HasHeader == true)
            //    {
            //        string dentFlagOrHeaderStr = System.Text.Encoding.UTF8.GetString(IdentificationFlags);
            //        if (dentFlagOrHeaderStr == "") return;
            //    }
            //    else
            //    {
            //        if (typeof(T) == typeof(ValueSet))
            //        {

            //        }
            //    }
            //}
            #endregion

            public void Reader(BinaryReader br, byte[] BOM)
            {
                string dentFlagOrHeaderStr = System.Text.Encoding.UTF8.GetString(IdentificationFlags);
                if (dentFlagOrHeaderStr == "CANM")
                {
                    //CANM
                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                    char[] ty = br.ReadChars(4);
                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                    if (new string(ty) == "CANM")
                    {
                        CANMSection.ReadCANM(br, BOM);
                    }
                }
                else
                {
                    if (Flag.GetIdentFlagUInt() == 0)
                    {
                        //CANM (DataSection (?))
                        AnimationDataSet.ReadCANM_AnimationDataSet(br, BOM);
                    }
                    else if (Flag.GetIdentFlagUInt() > 0)
                    {
                        bool IsUserData = (Flag.GetIdentFlagUInt() & 0x0FFFFFFF) == 0 ? true : false;
                        if (IsUserData)
                        {
                            if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F32))
                            {
                                //UserData (Real Number, Float)
                                RealNumber_Data.ReadUserDataList(br, BOM);
                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F31))
                            {
                                //NameSet
                                Name_Set.ReadNameSet(br, BOM);

                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F30))
                            {
                                //UserData (Int32)
                                Int32_Data.ReadUserDataInt32List(br, BOM);
                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F29))
                            {
                                //UserData (String)
                                String_Data.ReadUserDataStringList(br, BOM);
                            }
                        }
                        else
                        {
                            if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F32))
                            {
                                //Shader
                                if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F1))
                                {
                                    //SHDR
                                    char[] ty = br.ReadChars(4);
                                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                                    if (new string(ty) == "SHDR")
                                    {
                                        //SHDR
                                        SHDRSection.ReadSHDR(br, BOM);
                                    }
                                }
                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F31))
                            {
                                if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F6))
                                {
                                    if ((Flag.GetIdentFlagUInt() & 0x0000FF00) == 0)
                                    {
                                        //Fragment
                                        char[] ty = br.ReadChars(4);
                                        br.BaseStream.Seek(-4, SeekOrigin.Current);
                                        if (new string(ty) == "CFLT")
                                        {
                                            //CFLT
                                            CFLTSection.ReadCFLT(br, BOM);
                                        }
                                    }
                                    else if ((Flag.GetIdentFlagUInt() & 0x0000FF00) != 0)
                                    {
                                        if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F9))
                                        {
                                            //Hemisphere
                                            char[] ty = br.ReadChars(4);
                                            br.BaseStream.Seek(-4, SeekOrigin.Current);
                                            if (new string(ty) == "CHLT")
                                            {
                                                //CHLT
                                                CHLTSection.ReadCHLT(br, BOM);
                                            }
                                        }
                                        else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F10))
                                        {
                                            //VertexLight
                                            char[] ty = br.ReadChars(4);
                                            br.BaseStream.Seek(-4, SeekOrigin.Current);
                                            if (new string(ty) == "CVLT")
                                            {
                                                //CVLT
                                                CVLTSection.ReadCVLT(br, BOM);
                                            }
                                        }
                                        else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F11))
                                        {
                                            //AmbientLight
                                            char[] ty = br.ReadChars(4);
                                            br.BaseStream.Seek(-4, SeekOrigin.Current);
                                            if (new string(ty) == "CALT")
                                            {
                                                //CALT
                                                CALTSection.ReadCALT(br, BOM);
                                            }
                                        }
                                    }
                                }
                                else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F4))
                                {
                                    //Has CameraSection
                                    if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F2))
                                    {
                                        //CCAM
                                        char[] ty = br.ReadChars(4);
                                        br.BaseStream.Seek(-4, SeekOrigin.Current);
                                        if (new string(ty) == "CCAM")
                                        {
                                            CCAMSection.ReadCCAM(br, BOM);
                                        }
                                    }
                                }
                                else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F2))
                                {
                                    if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F5))
                                    {
                                        if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F8))
                                        {
                                            //CMDL (Primitive)
                                            char[] ty = br.ReadChars(4);
                                            br.BaseStream.Seek(-4, SeekOrigin.Current);
                                            if (new string(ty) == "CMDL")
                                            {
                                                CMDLSection.ReadCMDL(br, BOM);
                                            }
                                        }
                                        else if (!Flag.GetFlags().HasFlag(CGFXIdentFlag.F8))
                                        {
                                            //CMDL (SimplificationSkeletalModel)
                                            char[] ty = br.ReadChars(4);
                                            br.BaseStream.Seek(-4, SeekOrigin.Current);
                                            if (new string(ty) == "CMDL")
                                            {
                                                CMDLSection.ReadCMDL(br, BOM);
                                            }
                                        }
                                    }
                                    else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F7))
                                    {
                                        //CFOG
                                        char[] ty = br.ReadChars(4);
                                        br.BaseStream.Seek(-4, SeekOrigin.Current);
                                        if (new string(ty) == "CFOG")
                                        {
                                            //CFOG
                                            CFOGSection.ReadCFOG(br, BOM);
                                        }
                                    }
                                }
                                else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F3 & CGFXIdentFlag.F2))
                                {
                                    //PEMT
                                    char[] ty = br.ReadChars(4);
                                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                                    if (new string(ty) == "PEMT")
                                    {
                                        //PEMT
                                        PEMTSection.ReadPEMT(br, BOM);
                                    }
                                }
                                else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F1))
                                {
                                    //CNOD
                                    char[] ty = br.ReadChars(4);
                                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                                    if (new string(ty) == "CNOD")
                                    {
                                        //CNOD
                                        CNODSection.ReadCNOD(br, BOM);
                                    }
                                }
                                else
                                {

                                }
                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F30))
                            {
                                //Texture
                                if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F3))
                                {
                                    char[] ty = br.ReadChars(4);
                                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                                    if (new string(ty) == "TXOB")
                                    {
                                        //TXOB(Material)
                                        TXOB_MaterialSection.MaterialInfoSection.ReadTXOB(br, new byte[] { 0xFF, 0xFE });
                                    }
                                }
                                else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F5) | Flag.GetFlags().HasFlag(CGFXIdentFlag.F1))
                                {
                                    char[] ty = br.ReadChars(4);
                                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                                    if (new string(ty) == "TXOB")
                                    {
                                        //TXOB(Texture:Shader)
                                        TXOBSection.TextureSection.ReadTXOB(br, new byte[] { 0xFF, 0xFE });
                                    }
                                }
                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F29))
                            {
                                //Geometry
                                if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F1))
                                {
                                    //SOBJ (Shape)
                                    char[] ty = br.ReadChars(4);
                                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                                    if (new string(ty) == "SOBJ")
                                    {
                                        SOBJ_Shape_Section.Read_SOBJ(br, BOM);
                                    }
                                }

                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F28))
                            {
                                //Material
                                char[] ty = br.ReadChars(4);
                                br.BaseStream.Seek(-4, SeekOrigin.Current);
                                if (new string(ty) == "MTOB")
                                {
                                    //MTOB
                                    MTOBSection.ReadMTOB(br, BOM);
                                }
                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F27))
                            {
                                //LookUpTable
                                char[] ty = br.ReadChars(4);
                                br.BaseStream.Seek(-4, SeekOrigin.Current);
                                if (new string(ty) == "LUTS")
                                {
                                    //LUTS
                                    LUTSSection.ReadLUTS(br, BOM);
                                }
                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F26))
                            {
                                //SOBJ_Skeleton

                                //char[] ty = br.ReadChars(4);
                                //br.BaseStream.Seek(-4, SeekOrigin.Current);
                                //if (new string(ty) == "SOBJ")
                                //{

                                //}
                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F25))
                            {
                                //SOBJ_Mesh
                                char[] ty = br.ReadChars(4);
                                br.BaseStream.Seek(-4, SeekOrigin.Current);
                                if (new string(ty) == "SOBJ")
                                {
                                    SOBJ_Mesh_Section.Read_SOBJ(br, BOM);
                                }
                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F24))
                            {
                                //CENV
                                char[] ty = br.ReadChars(4);
                                br.BaseStream.Seek(-4, SeekOrigin.Current);
                                if (new string(ty) == "CENV")
                                {
                                    //CENV
                                    CENVSection.Read_CENV(br, BOM);
                                }
                            }
                        }
                    }
                }
            }


            //public enum Key
            //{
            //    ValueSet = 0,

            //}

            //public CGFXData(byte[] IdentificationFlag, Key key)
            //{
            //    IdentificationFlags = IdentificationFlag;
            //    if (key == Key.ValueSet)
            //    {
            //        ValueData = new ValueSet();
            //    }
            //}

            public CGFXData(byte[] IdentificationFlag)
            {
                IdentificationFlags = IdentificationFlag;

                if (System.Text.Encoding.UTF8.GetString(IdentificationFlags) == "CANM")
                {
                    //CANM (0x00000000, No IdentFlag)
                    CANMSection = new CANM();
                }
                else
                {
                    if (Flag.GetIdentFlagUInt() == 0)
                    {
                        //0x00000000, No IdentFlag
                        AnimationDataSet = new AnimationData();
                    }
                    else if (Flag.GetIdentFlagUInt() > 0) //0x00000001~
                    {
                        bool IsUserData = (Flag.GetIdentFlagUInt() & 0x0FFFFFFF) == 0 ? true : false;
                        if (IsUserData)
                        {
                            if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F32)) RealNumber_Data = new RealNumber();
                            //else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F31)) NativeDataSections.CMDL_Native.MaterialName_Set = new NativeDataSection.CMDL.MaterialNameSet();
                            //else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F31)) Name_Set = new NameSet();
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F30)) Int32_Data = new Int32Data();
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F29)) String_Data = new StringData();
                        }
                        else
                        {
                            if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F32))
                            {
                                //Shader
                                if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F1)) SHDRSection = new SHDR();
                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F31))
                            {
                                if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F6))
                                {
                                    if ((Flag.GetIdentFlagUInt() & 0x0000FF00) == 0) CFLTSection = new CFLT();
                                    else if ((Flag.GetIdentFlagUInt() & 0x0000FF00) != 0) CHLTSection = new CHLT();
                                    {
                                        if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F9)) CHLTSection = new CHLT();
                                        else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F10)) CVLTSection = new CVLT();
                                        else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F11)) CALTSection = new CALT();
                                    }
                                }
                                else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F4))
                                {
                                    //Has CameraSection
                                    if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F2)) CCAMSection = new CCAM();
                                }
                                else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F2))
                                {
                                    if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F5))
                                    {
                                        if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F8)) CMDLSection = new CMDL(); //Primitive
                                        else CMDLSection = new CMDL(); //CMDL (SimplificationSkeletalModel)
                                    }
                                    else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F7)) CFOGSection = new CFOG();
                                    else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F3 & CGFXIdentFlag.F2)) PEMTSection = new PEMT();
                                    else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F1)) CNODSection = new CNOD();
                                }
                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F30))
                            {
                                //Texture
                                if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F3)) TXOB_MaterialSection = new TXOB(TXOB.Type.MaterialInfo);
                                else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F5) | Flag.GetFlags().HasFlag(CGFXIdentFlag.F1)) TXOBSection = new TXOB(TXOB.Type.Texture);
                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F29))
                            {
                                //Geometry
                                if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F1))
                                {
                                    //SOBJ (Shape)
                                    SOBJ_Shape_Section = new SOBJ(SOBJ.SOBJType.Shape);
                                }

                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F28))
                            {
                                MTOBSection = new MTOB();
                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F27)) LUTSSection = new LUTS();
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F26))
                            {
                                //SOBJ_Skeleton
                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F25))
                            {
                                //SOBJ_Mesh
                                SOBJ_Mesh_Section = new SOBJ(SOBJ.SOBJType.Mesh);
                            }
                            else if (Flag.GetFlags().HasFlag(CGFXIdentFlag.F24)) CENVSection = new CENV();
                        }
                    }
                }

                //UserData = new UserData();
                Name_Set = new NameSet();
            }
        }
    }
}
