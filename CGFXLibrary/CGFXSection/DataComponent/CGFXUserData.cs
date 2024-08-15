using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary.CGFXSection.DataComponent
{
    /// <summary>
    /// UserData Section
    /// </summary>
    public class CGFXUserData
    {
        /// <summary>
        /// UserData (String) => Flag : 00 00 00 10
        /// </summary>
        public class StringData : IO.BinaryIOInterface.BinaryIO
        {
            public long EndPos;

            public string UDName;

            public int UserDataStringNameOffset { get; set; } //0x4
            public byte[] Flag2 { get; set; } //0x4(Default Value : 02 00 00 00(?))
            public byte[] Flag3 { get; set; } //0x4(Default Value : 02 00 00 00(?))
            public int STRING_ValueCount { get; set; }

            public List<UserDataItem_String> UserDataItem_String_List { get; set; }
            public class UserDataItem_String
            {
                public int Offset { get; set; } //0x4
                public string StringData { get; set; }

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

                public UserDataItem_String(int InputOffset, string InputString)
                {
                    Offset = InputOffset;
                    StringData = InputString;
                }

                public override string ToString()
                {
                    return "StringValue";
                }
            }

            public override void Read(BinaryReader br, byte[] BOM)
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
            }

            public override void Write(BinaryWriter bw, byte[] BOM = null)
            {
                throw new NotImplementedException();
            }

            public StringData()
            {
                UserDataStringNameOffset = 0;
                Flag2 = new byte[4];
                Flag3 = new byte[4];
                STRING_ValueCount = 0;
                UserDataItem_String_List = new List<UserDataItem_String>();
            }

            public override string ToString()
            {
                return "UserData (String) : " + UDName;
            }
        }

        /// <summary>
        /// UserData (Int32) => Flag : 00 00 00 20
        /// </summary>
        public class Int32Data : IO.BinaryIOInterface.BinaryIO
        {
            public long EndPos;
            public string UDName;

            
            public int UserDataInt32NameOffset { get; set; } //0x4
            public byte[] Flag2 { get; set; } //0x4(Default Value : 01 00 00 00(?))
            public int INT32_ValueCount { get; set; } //0x4

            public List<UserDataItem_INT32> UserDataItem_Int32Data_List { get; set; }
            public class UserDataItem_INT32
            {
                public int UserDataItem_Int32_Value { get; set; } //0x4

                public void ReadData(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    UserDataItem_Int32_Value = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                }

                public UserDataItem_INT32(int Input)
                {
                    UserDataItem_Int32_Value = Input;
                }

                public override string ToString()
                {
                    return "Int32Value";
                }
            }

            /// <summary>
            /// Read UserData (Int32)
            /// </summary>
            /// <param name="br">BinaryReader</param>
            /// <param name="BOM">ByteOrder</param>
            public override void Read(BinaryReader br, byte[] BOM)
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

            }

            public override void Write(BinaryWriter bw, byte[] BOM = null)
            {
                throw new NotImplementedException();
            }

            public Int32Data()
            {
                UserDataInt32NameOffset = 0;
                Flag2 = new byte[4];
                INT32_ValueCount = 0;
                UserDataItem_Int32Data_List = new List<UserDataItem_INT32>();
            }

            public override string ToString()
            {
                return "UserData (Int32) : " + UDName;
            }
        }

        /// <summary>
        /// UserData (RealNumber) => Flag : 00 00 00 80
        /// </summary>
        public class RealNumber : IO.BinaryIOInterface.BinaryIO
        {
            public UD_FloatType UD_Float_Type { get; set; }
            public enum UD_FloatType
            {
                NOTSET = -1,
                Float = 0,
                RealNumber = 1,
                BYTE = 2
            }

            public long EndPos;

            public string FloatNumberName;
            public string RealNumberName;

            public int FloatNumberNameOffset { get; set; } //0x4
            public int RealNumberNameOffset { get; set; } //0x4(Using Fog Animation, ColorData, Default = 0)

            public int REALNUMBERCount { get; set; } //0x4

            public List<UserDataItem_REALNUMBER> UserDataItem_RealNumber_List { get; set; }
            public class UserDataItem_REALNUMBER
            {
                public UD_FloatType UD_FloatType { get; set; }
                public object Value { get; set; } //0x4

                public void ReadData(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    if (UD_FloatType == UD_FloatType.Float)
                    {
                        Value = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }
                    else if (UD_FloatType == UD_FloatType.RealNumber)
                    {
                        Value = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }
                    else if (UD_FloatType == UD_FloatType.BYTE)
                    {
                        Value = br.ReadByte();
                    }

                    
                }

                public T GetFealNumberValue<T>()
                {
                    return (T)(object)Value;
                }

                public UserDataItem_REALNUMBER(UD_FloatType uD_FloatType, object value = null)
                {
                    if (uD_FloatType == UD_FloatType.Float)
                    {
                        UD_FloatType = uD_FloatType;
                        Value = Convert.ToSingle(value);
                    }
                    else if (uD_FloatType == UD_FloatType.RealNumber)
                    {
                        UD_FloatType = uD_FloatType;
                        Value = Convert.ToInt32(value);
                    }

                    Value = value;
                }

                public override string ToString()
                {
                    return "RealNumberValue";
                }
            }

            /// <summary>
            /// Read RealNumber Data
            /// </summary>
            /// <param name="br"></param>
            /// <param name="BOM"></param>
            public void ReadUDRealNumber(BinaryReader br, byte[] BOM)
            {
                UD_Float_Type = UD_FloatType.NOTSET;

                EndianConvert endianConvert = new EndianConvert(BOM);
                FloatNumberNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (FloatNumberNameOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move NameOffset
                    br.BaseStream.Seek(FloatNumberNameOffset, SeekOrigin.Current);

                    ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                    readByteLine.ReadByte(br, 0x00);

                    FloatNumberName = new string(readByteLine.ConvertToCharArray());

                    br.BaseStream.Position = Pos;

                    UD_Float_Type = UD_FloatType.Float;
                }

                RealNumberNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (RealNumberNameOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move NameOffset
                    br.BaseStream.Seek(RealNumberNameOffset, SeekOrigin.Current);

                    ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                    readByteLine.ReadByte(br, 0x00);

                    RealNumberName = new string(readByteLine.ConvertToCharArray());

                    br.BaseStream.Position = Pos;

                    UD_Float_Type = UD_FloatType.RealNumber;
                }

                if (UD_Float_Type == UD_FloatType.NOTSET)
                {
                    UD_Float_Type = UD_FloatType.BYTE;
                }

                REALNUMBERCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                for (int UDStrCount = 0; UDStrCount < REALNUMBERCount; UDStrCount++)
                {
                    UserDataItem_REALNUMBER userDataItem_REALNUMBER = new UserDataItem_REALNUMBER(UD_Float_Type);
                    userDataItem_REALNUMBER.ReadData(br, BOM);

                    UserDataItem_RealNumber_List.Add(userDataItem_REALNUMBER);
                }

                if (UD_Float_Type == UD_FloatType.Float || UD_Float_Type == UD_FloatType.RealNumber)
                {
                    EndPos = br.BaseStream.Position + (4 * REALNUMBERCount);
                }
                else if (UD_Float_Type == UD_FloatType.BYTE)
                {
                    EndPos = br.BaseStream.Position + (1 * REALNUMBERCount);
                }

                //EndPos = br.BaseStream.Position;
            }

            public void WriteUDRealNumber(BinaryWriter bw, byte[] BOM)
            {
                throw new NotImplementedException();
            }

            public override void Read(BinaryReader br, byte[] BOM)
            {
                ReadUDRealNumber(br, BOM);
            }

            public override void Write(BinaryWriter bw, byte[] BOM)
            {
                WriteUDRealNumber(bw, BOM);
            }

            public RealNumber(UD_FloatType UserDataFoat_ValueType)
            {
                UD_Float_Type = UserDataFoat_ValueType;

                if (UserDataFoat_ValueType == UD_FloatType.Float)
                {
                    
                }
                else if (UserDataFoat_ValueType == UD_FloatType.RealNumber)
                {

                }

                FloatNumberNameOffset = 0;
                RealNumberNameOffset = 0;
                REALNUMBERCount = 0;
                UserDataItem_RealNumber_List = new List<UserDataItem_REALNUMBER>();

                //    if (realNumberType == RealNumberType.Color) color = new Color(0, 0, 0, 0);
                //    else if (realNumberType == RealNumberType.Float) FloatValue = 0;
            }

            public RealNumber()
            {
                UD_Float_Type = UD_FloatType.NOTSET;
                FloatNumberNameOffset = 0;
                RealNumberNameOffset = 0;
                REALNUMBERCount = 0;
                UserDataItem_RealNumber_List = new List<UserDataItem_REALNUMBER>();
            }

            public override string ToString()
            {
                return "UserData (Float)";
            }
        }
    }
}
