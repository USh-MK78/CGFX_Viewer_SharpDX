using CGFXLibrary;
using CGFXLibrary.CGFXSection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFX_Viewer_SharpDX.CGFXPropertyGridSet
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    public class CENV_PropertyGrid
    {
        public string Name { get; }
        public char[] CENV_Header { get; } //0x4
        public byte[] CENV_Revision { get; } //0x4

        //CENV_Name
        public int CENV_NameOffset { get; }

        //UserData
        public int CENV_NumOfUserDataDICTOffsetEntries { get; set; } //0x4
        public int CENV_UserDataDICTOffset { get; }  //0x4

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public DICT CENV_UserData_DICT { get; set; } = new DICT();

        //Camera
        public int CENV_NumOfCCAMOffsetEntries { get; set; }
        public int CENV_CCAMOffset { get; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomListTypeConverter<CENV_CCAMNameSet>))]
        public List<CENV_CCAMNameSet> CENV_CCAMName_List { get; set; } = new List<CENV_CCAMNameSet>();
        public class CENV_CCAMNameSet
        {
            public int CCAMNameSetOffset { get; }
            public string Name => CCAMName.Name;

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public CENV_CCAMName CCAMName { get; set; } = new CENV_CCAMName();
            public class CENV_CCAMName
            {
                public string Name { get; set; }
                public int CCAM_Number { get; set; }
                public int CCAM_StringOffset { get; }
                public int UnknownData { get; set; }

                public CENV_CCAMName(CENV.CENV_CCAMNameSet.CENV_CCAMName CCAMName)
                {
                    Name = CCAMName.Name;
                    CCAM_Number = CCAMName.CCAM_Number;
                    CCAM_StringOffset = CCAMName.CCAM_StringOffset;
                    UnknownData = CCAMName.UnknownData;
                }

                public CENV_CCAMName()
                {
                    Name = " ";
                    CCAM_Number = -1;
                    CCAM_StringOffset = 0;
                    UnknownData = 0;
                }

                public override string ToString()
                {
                    return Name + " | " + "Number : " + CCAM_Number;
                }
            }

            public CENV_CCAMNameSet(CENV.CENV_CCAMNameSet CCAMNameSet)
            {
                CCAMNameSetOffset = CCAMNameSet.CCAMNameSetOffset;
                CCAMName = new CENV_CCAMName(CCAMNameSet.CCAMName);
            }

            public CENV_CCAMNameSet()
            {
                CCAMNameSetOffset = 0;
                CCAMName = new CENV_CCAMName();
            }

            public override string ToString()
            {
                return "Name : " + Name;
            }
        }

        //Light
        public int CENV_NumOfLIGHTOffsetEntries { get; set; }  //0x4
        public int CENV_LightNameSetListOffset { get; }  //0x4

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomListTypeConverter<CENV_LIGHTNameGroup>))]
        public List<CENV_LIGHTNameGroup> CENV_LightNameGroup { get; set; } = new List<CENV_LIGHTNameGroup>();
        public class CENV_LIGHTNameGroup
        {
            public int CENV_LIGHTGroupOffset { get; }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public CENV_LightNameGroupSet LightNameGroupSet { get; set; } = new CENV_LightNameGroupSet();
            public class CENV_LightNameGroupSet
            {
                public int LightGroupNumber { get; set; }
                public int CENV_LIGHTNameSetCount { get; set; }
                public int CENV_LIGHTNameSetListOffset { get; }

                [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomListTypeConverter<CENV_LIGHTNameSet>))]
                public List<CENV_LIGHTNameSet> CENV_LightNameSet_List { get; set; } = new List<CENV_LIGHTNameSet>();
                public class CENV_LIGHTNameSet
                {
                    public int CENV_LIGHTNameOffset { get; }

                    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
                    public CENV_LightName LightName { get; set; } = new CENV_LightName();
                    public class CENV_LightName
                    {
                        public string LIGHT_Name { get; }
                        public int LIGHT_Number { get; set; }
                        public int LIGHTNameOffset { get; }
                        public int UnknownData { get; set; }

                        public CENV_LightName(CENV.CENV_LIGHTNameGroup.CENV_LightNameGroupSet.CENV_LIGHTNameSet.CENV_LightName LightName)
                        {
                            LIGHT_Name = LightName.LIGHT_Name;
                            LIGHT_Number = LightName.LIGHT_Number;
                            LIGHTNameOffset = LightName.LIGHTNameOffset;
                            UnknownData = LightName.UnknownData;
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
                            return LIGHT_Name + " | " + "SetNumber : " + LIGHT_Number;
                        }
                    }

                    public CENV_LIGHTNameSet(CENV.CENV_LIGHTNameGroup.CENV_LightNameGroupSet.CENV_LIGHTNameSet LIGHTNameSet)
                    {
                        CENV_LIGHTNameOffset = LIGHTNameSet.CENV_LIGHTNameOffset;
                        LightName = new CENV_LightName(LIGHTNameSet.LightName);
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

                public CENV_LightNameGroupSet(CENV.CENV_LIGHTNameGroup.CENV_LightNameGroupSet LightNameGroupSet)
                {
                    LightGroupNumber = LightNameGroupSet.LightGroupNumber;
                    CENV_LIGHTNameSetCount = LightNameGroupSet.CENV_LIGHTNameSetCount;
                    CENV_LIGHTNameSetListOffset = LightNameGroupSet.CENV_LIGHTNameSetListOffset;

                    foreach (var item in LightNameGroupSet.CENV_LightNameSet_List)
                    {
                        CENV_LightNameSet_List.Add(new CENV_LIGHTNameSet(item));
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

            public CENV_LIGHTNameGroup(CENV.CENV_LIGHTNameGroup LIGHTNameGroup)
            {
                CENV_LIGHTGroupOffset = LIGHTNameGroup.CENV_LIGHTGroupOffset;
                LightNameGroupSet = new CENV_LightNameGroupSet(LIGHTNameGroup.LightNameGroupSet);
            }

            public CENV_LIGHTNameGroup()
            {
                CENV_LIGHTGroupOffset = 0;
                LightNameGroupSet = new CENV_LightNameGroupSet();
            }

            public override string ToString()
            {
                return "LightGroupNumber : " + LightNameGroupSet.LightGroupNumber;
            }
        }

        //Fog
        public int CENV_NumOfFogNameSetEntries { get; set; }
        public int CENV_FogNameSetListOffset { get; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomListTypeConverter<CENV_FogNameSet>))]
        public List<CENV_FogNameSet> CENV_FogNameSet_List { get; set; } = new List<CENV_FogNameSet>();
        public class CENV_FogNameSet
        {
            public int FogNameSetOffset { get; }
            public string Name => FogNameSet.Name;

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public CENV_FogName FogNameSet { get; set; } = new CENV_FogName();
            public class CENV_FogName
            {
                public string Name { get; }
                public int Fog_Number { get; set; } //0x4, 1～N
                public int FogNameOffset { get; } 
                public int UnknownData { get; set; }

                public CENV_FogName(CENV.CENV_FogNameSet.CENV_FogName FogName)
                {
                    Name = FogName.Name;
                    Fog_Number = FogName.Fog_Number;
                    FogNameOffset = FogName.FogNameOffset;
                    UnknownData = FogName.UnknownData;
                }

                public CENV_FogName()
                {
                    Name = " ";
                    Fog_Number = -1;
                    FogNameOffset = 0;
                    UnknownData = 0;
                }

                public override string ToString()
                {
                    return Name + " | " + "FogNumber : " + Fog_Number;
                }
            }

            public CENV_FogNameSet(CENV.CENV_FogNameSet FogNameSet)
            {
                FogNameSetOffset = FogNameSet.FogNameSetOffset;
                this.FogNameSet = new CENV_FogName(FogNameSet.FogNameSet);
            }

            public CENV_FogNameSet()
            {
                FogNameSetOffset = 0;
                FogNameSet = new CENV_FogName();
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public CENV_PropertyGrid(CENV CENV)
        {
            CENV_Header = CENV.CENV_Header;
            CENV_Revision = CENV.CENV_Revision; //01 00 00 00 => Data : 00 00 00 01
            CENV_NameOffset = CENV.CENV_NameOffset;

            CENV_NumOfUserDataDICTOffsetEntries = CENV.CENV_NumOfUserDataDICTOffsetEntries;
            CENV_UserDataDICTOffset = CENV.CENV_UserDataDICTOffset;
            CENV_UserData_DICT = CENV.CENV_UserData_DICT;

            CENV_NumOfCCAMOffsetEntries = CENV.CENV_NumOfCCAMOffsetListEntries;
            CENV_CCAMOffset = CENV.CENV_CCAMNameSetListOffset;
            foreach (var item in CENV.CENV_CCAMNameSet_List) CENV_CCAMName_List.Add(new CENV_CCAMNameSet(item));

            CENV_NumOfLIGHTOffsetEntries = CENV.CENV_NumOfLIGHTOffsetEntries;
            CENV_LightNameSetListOffset = CENV.CENV_LightNameSetListOffset;
            foreach (var item in CENV.CENV_LightNameGroup) CENV_LightNameGroup.Add(new CENV_LIGHTNameGroup(item));

            CENV_NumOfFogNameSetEntries = CENV.CENV_NumOfFogNameSetEntries;
            CENV_FogNameSetListOffset = CENV.CENV_FogNameSetListOffset;
            foreach (var item in CENV.CENV_FogNameSet_List) CENV_FogNameSet_List.Add(new CENV_FogNameSet(item));
        }

        public CENV_PropertyGrid()
        {
            Name = " ";
            CENV_Header = "CENV".ToCharArray();
            CENV_Revision = new byte[4]; //01 00 00 00 => Data : 00 00 00 01
            CENV_NameOffset = 0;

            CENV_NumOfUserDataDICTOffsetEntries = 0;
            CENV_UserDataDICTOffset = 0;
            CENV_UserData_DICT = new DICT();

            CENV_NumOfCCAMOffsetEntries = 0;
            CENV_CCAMOffset = 0;
            CENV_CCAMName_List = new List<CENV_CCAMNameSet>();

            CENV_NumOfLIGHTOffsetEntries = 0;
            CENV_LightNameSetListOffset = 0;
            CENV_LightNameGroup = new List<CENV_LIGHTNameGroup>();

            CENV_NumOfFogNameSetEntries = 0;
            CENV_FogNameSetListOffset = 0;
            CENV_FogNameSet_List = new List<CENV_FogNameSet>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
