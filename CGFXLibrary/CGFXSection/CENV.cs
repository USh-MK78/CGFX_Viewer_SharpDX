using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary.CGFXSection
{
    /// <summary>
    /// Environments
    /// </summary>
    public class CENV
    {
        public string Name;
        public char[] CENV_Header { get; set; } //0x4
        public byte[] CENV_Revision { get; set; } //0x4

        //CENV_Name
        public int CENV_NameOffset { get; set; }

        //UserData
        public int CENV_NumOfUserDataDICTOffsetEntries { get; set; } //0x4
        public int CENV_UserDataDICTOffset { get; set; }  //0x4
        public DICT CENV_UserData_DICT { get; set; }

        //Camera
        public int CENV_NumOfCCAMOffsetListEntries { get; set; }
        public int CENV_CCAMNameSetListOffset { get; set; }
        public List<CENV_CCAMNameSet> CENV_CCAMNameSet_List { get; set; }
        public class CENV_CCAMNameSet
        {
            public int CCAMNameSetOffset { get; set; }
            public string Name => CCAMName.Name;

            public CENV_CCAMName CCAMName { get; set; }
            public class CENV_CCAMName
            {
                public string Name;
                public int CCAM_Number { get; set; } //0x4
                public int CCAM_StringOffset { get; set; } //0x4
                public int UnknownData { get; set; } //0x4

                public void ReadCCAMName(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    CCAM_Number = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    CCAM_StringOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (CCAM_StringOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(CCAM_StringOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        Name = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    UnknownData = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                }

                //public CENV_CCAMName(string s)
                //{

                //}

                public CENV_CCAMName()
                {
                    Name = " ";
                    CCAM_Number = -1;
                    CCAM_StringOffset = 0;
                    UnknownData = 0;
                }
            }

            public void Read_CENV_CCAMNameSet(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                CCAMNameSetOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (CCAMNameSetOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move NameOffset
                    br.BaseStream.Seek(CCAMNameSetOffset, SeekOrigin.Current);

                    CCAMName.ReadCCAMName(br, BOM);

                    br.BaseStream.Position = Pos;
                }
            }

            public CENV_CCAMNameSet()
            {
                CCAMNameSetOffset = 0;
                CCAMName = new CENV_CCAMName();
            }
        }

        //Light
        public int CENV_NumOfLIGHTOffsetEntries { get; set; }  //0x4
        public int CENV_LightNameSetListOffset { get; set; }  //0x4
        public List<CENV_LIGHTNameGroup> CENV_LightNameGroup { get; set; }
        public class CENV_LIGHTNameGroup
        {
            public int CENV_LIGHTGroupOffset { get; set; }

            public CENV_LightNameGroupSet LightNameGroupSet { get; set; }
            public class CENV_LightNameGroupSet
            {
                public int LightGroupNumber { get; set; }
                public int CENV_LIGHTNameSetCount { get; set; }
                public int CENV_LIGHTNameSetListOffset { get; set; } //0x4(04 00 00 00)

                public List<CENV_LIGHTNameSet> CENV_LightNameSet_List { get; set; }
                public class CENV_LIGHTNameSet
                {
                    public int CENV_LIGHTNameOffset { get; set; }
                    public CENV_LightName LightName { get; set; }
                    public class CENV_LightName
                    {
                        public string LIGHT_Name;
                        public int LIGHT_Number { get; set; }
                        public int LIGHTNameOffset { get; set; }
                        public int UnknownData { get; set; }

                        public void ReadLightName(BinaryReader br, byte[] BOM)
                        {
                            EndianConvert endianConvert = new EndianConvert(BOM);
                            LIGHT_Number = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            LIGHTNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            if (LIGHTNameOffset != 0)
                            {
                                long Pos = br.BaseStream.Position;

                                br.BaseStream.Seek(-4, SeekOrigin.Current);

                                //Move NameOffset
                                br.BaseStream.Seek(LIGHTNameOffset, SeekOrigin.Current);

                                ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                                readByteLine.ReadByte(br, 0x00);

                                LIGHT_Name = new string(readByteLine.ConvertToCharArray());

                                br.BaseStream.Position = Pos;
                            }

                            UnknownData = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        }

                        public CENV_LightName()
                        {
                            LIGHT_Name = " ";
                            LIGHT_Number = -1; //Count
                            LIGHTNameOffset = 0;
                            UnknownData = 0;
                        }

                        public override string ToString()
                        {
                            return LIGHT_Name;
                        }
                    }

                    public void ReadLightNameSet(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        CENV_LIGHTNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (CENV_LIGHTNameOffset != 0)
                        {
                            long Pos = br.BaseStream.Position;

                            br.BaseStream.Seek(-4, SeekOrigin.Current);

                            //Move Offset
                            br.BaseStream.Seek(CENV_LIGHTNameOffset, SeekOrigin.Current);

                            //LightName
                            LightName.ReadLightName(br, BOM);

                            br.BaseStream.Position = Pos;
                        }
                    }

                    public CENV_LIGHTNameSet()
                    {
                        CENV_LIGHTNameOffset = 0;
                        LightName = new CENV_LightName();
                    }

                    public override string ToString()
                    {
                        return LightName.LIGHT_Name;
                    }
                }

                public void Read_LightNameGroupSet(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    LightGroupNumber = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    CENV_LIGHTNameSetCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    CENV_LIGHTNameSetListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (CENV_LIGHTNameSetListOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move Offset
                        br.BaseStream.Seek(CENV_LIGHTNameSetListOffset, SeekOrigin.Current);

                        for (int i = 0; i < CENV_LIGHTNameSetCount; i++)
                        {
                            CENV_LIGHTNameSet cENV_LIGHTNameSet = new CENV_LIGHTNameSet();
                            cENV_LIGHTNameSet.ReadLightNameSet(br, BOM);
                            CENV_LightNameSet_List.Add(cENV_LIGHTNameSet);
                        }

                        br.BaseStream.Position = Pos;
                    }
                }

                public CENV_LightNameGroupSet()
                {
                    LightGroupNumber = -1; //Count
                    CENV_LIGHTNameSetCount = 0;
                    CENV_LIGHTNameSetListOffset = 0; //0x4 (固定)
                    CENV_LightNameSet_List = new List<CENV_LIGHTNameSet>();
                }

                public override string ToString()
                {
                    return "LightGroup : " + "ID : " + LightGroupNumber + " | " + "Count : " + CENV_LIGHTNameSetCount;
                }
            }

            public void Read_CENVLightNameSet(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                CENV_LIGHTGroupOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (CENV_LIGHTGroupOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move Offset
                    br.BaseStream.Seek(CENV_LIGHTGroupOffset, SeekOrigin.Current);

                    LightNameGroupSet.Read_LightNameGroupSet(br, BOM);

                    br.BaseStream.Position = Pos;
                }
            }

            public CENV_LIGHTNameGroup()
            {
                CENV_LIGHTGroupOffset = 0;
                LightNameGroupSet = new CENV_LightNameGroupSet();
            }
        }

        //Fog
        public int CENV_NumOfFogNameSetEntries { get; set; }
        public int CENV_FogNameSetListOffset { get; set; }
        public List<CENV_FogNameSet> CENV_FogNameSet_List { get; set; }
        public class CENV_FogNameSet
        {
            public int FogNameSetOffset { get; set; }
            public string Name => FogNameSet.Name;

            public CENV_FogName FogNameSet { get; set; }
            public class CENV_FogName
            {
                public string Name;
                public int Fog_Number { get; set; } //0x4, 1～N
                public int FogNameOffset { get; set; } //0x4
                public int UnknownData { get; set; } //0x4

                public void ReadFogName(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    Fog_Number = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    FogNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (FogNameOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move NameOffset
                        br.BaseStream.Seek(FogNameOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);

                        Name = new string(readByteLine.ConvertToCharArray());

                        br.BaseStream.Position = Pos;
                    }

                    UnknownData = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                }

                public CENV_FogName()
                {
                    Name = " ";
                    Fog_Number = -1;
                    FogNameOffset = 0;
                    UnknownData = 0;
                }
            }

            public void Read_CENV_FogNameSet(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                FogNameSetOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (FogNameSetOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move NameOffset
                    br.BaseStream.Seek(FogNameSetOffset, SeekOrigin.Current);

                    FogNameSet.ReadFogName(br, BOM);

                    br.BaseStream.Position = Pos;
                }
            }

            public CENV_FogNameSet()
            {
                FogNameSetOffset = 0;
                FogNameSet = new CENV_FogName();
            }
        }

        public void Read_CENV(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            CENV_Header = br.ReadChars(4);
            if (new string(CENV_Header) != "CENV") throw new Exception("不明なフォーマットです");

            CENV_Revision = endianConvert.Convert(br.ReadBytes(4));
            CENV_NameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (CENV_NameOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move NameOffset
                br.BaseStream.Seek(CENV_NameOffset, SeekOrigin.Current);

                ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                readByteLine.ReadByte(br, 0x00);

                Name = new string(readByteLine.ConvertToCharArray());

                br.BaseStream.Position = Pos;
            }

            CENV_NumOfUserDataDICTOffsetEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            CENV_UserDataDICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (CENV_UserDataDICTOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(CENV_UserDataDICTOffset, SeekOrigin.Current);

                CENV_UserData_DICT.ReadDICT(br, BOM);

                br.BaseStream.Position = Pos;
            }

            CENV_NumOfCCAMOffsetListEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            CENV_CCAMNameSetListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (CENV_CCAMNameSetListOffset != 0) //CENV_CCAMListOffset != 0
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(CENV_CCAMNameSetListOffset, SeekOrigin.Current);

                for (int i = 0; i < CENV_NumOfCCAMOffsetListEntries; i++)
                {
                    CENV_CCAMNameSet cENV_CCAMNameSet = new CENV_CCAMNameSet();
                    cENV_CCAMNameSet.Read_CENV_CCAMNameSet(br, BOM);
                    CENV_CCAMNameSet_List.Add(cENV_CCAMNameSet);
                }

                br.BaseStream.Position = Pos;
            }

            CENV_NumOfLIGHTOffsetEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            CENV_LightNameSetListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (CENV_LightNameSetListOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(CENV_LightNameSetListOffset, SeekOrigin.Current);

                for (int i = 0; i < CENV_NumOfLIGHTOffsetEntries; i++)
                {
                    //Light Group
                    CENV_LIGHTNameGroup cENV_LIGHTNameGroup = new CENV_LIGHTNameGroup();
                    cENV_LIGHTNameGroup.Read_CENVLightNameSet(br, BOM);
                    CENV_LightNameGroup.Add(cENV_LIGHTNameGroup);
                }

                br.BaseStream.Position = Pos;
            }

            CENV_NumOfFogNameSetEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            CENV_FogNameSetListOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (CENV_FogNameSetListOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(CENV_FogNameSetListOffset, SeekOrigin.Current);

                for (int i = 0; i < CENV_NumOfFogNameSetEntries; i++)
                {
                    CENV_FogNameSet cENV_FogNameSet = new CENV_FogNameSet();
                    cENV_FogNameSet.Read_CENV_FogNameSet(br, BOM);
                    CENV_FogNameSet_List.Add(cENV_FogNameSet);
                }

                br.BaseStream.Position = Pos;
            }
        }

        public CENV()
        {
            CENV_Header = "CENV".ToCharArray();
            CENV_Revision = new byte[4]; //01 00 00 00 => Data : 00 00 00 01
            CENV_NameOffset = 0;

            CENV_NumOfUserDataDICTOffsetEntries = 0;
            CENV_UserDataDICTOffset = 0;
            CENV_UserData_DICT = new DICT();

            CENV_NumOfCCAMOffsetListEntries = 0;
            CENV_CCAMNameSetListOffset = 0;
            CENV_CCAMNameSet_List = new List<CENV_CCAMNameSet>();

            CENV_NumOfLIGHTOffsetEntries = 0;
            CENV_LightNameSetListOffset = 0;
            CENV_LightNameGroup = new List<CENV_LIGHTNameGroup>();

            CENV_NumOfFogNameSetEntries = 0;
            CENV_FogNameSetListOffset = 0;
            CENV_FogNameSet_List = new List<CENV_FogNameSet>();
        }
    }
}
