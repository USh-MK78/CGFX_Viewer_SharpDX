using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGFXLibrary;

namespace CGFX_Viewer_SharpDX.PropertyGridForms.General.UserDataForm
{
	public class UserDataEntryPropertyGrid
	{
        [TypeConverter(typeof(CGFXPropertyGridSet.CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public StringData String_Data { get; set; }
        public class StringData
        {
            public string UDName { get; set; }

            //Flag : 00 00 00 10
            public byte[] Flag2 { get; set; } //0x4(Default Value : 02 00 00 00(?))
            public byte[] Flag3 { get; set; } //0x4(Default Value : 02 00 00 00(?))
            public int STRING_ValueCount { get; set; }

            public List<CGFXLibrary.CGFXSection.DataComponent.CGFXUserData.StringData.UserDataItem_String> UserDataStringList { get; set; }

            public StringData(CGFXLibrary.CGFXSection.DataComponent.CGFXUserData.StringData stringData)
            {
                UDName = stringData.UDName;
                //UserDataStringNameOffset = 0;
                Flag2 = stringData.Flag2; //Default Value : 02 00 00 00(?)
                Flag3 = stringData.Flag3; //Default Value : 02 00 00 00(?)
                STRING_ValueCount = stringData.STRING_ValueCount;
                UserDataStringList = stringData.UserDataItem_String_List;
            }
        }

        [TypeConverter(typeof(CGFXPropertyGridSet.CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public Int32Data Int32_Data { get; set; }
        public class Int32Data
        {
            public string UDName { get; set; }

            //Flag : 00 00 00 20
            public byte[] Flag2 { get; set; } //0x4(Default Value : 01 00 00 00(?))
            public int INT32_ValueCount { get; set; } //0x4

            public List<CGFXLibrary.CGFXSection.DataComponent.CGFXUserData.Int32Data.UserDataItem_INT32> UserDataInt32List { get; set; }

            public Int32Data(CGFXLibrary.CGFXSection.DataComponent.CGFXUserData.Int32Data int32Data)
            {
                UDName = int32Data.UDName;
                //UserDataInt32NameOffset = 0;
                Flag2 = int32Data.Flag2;
                INT32_ValueCount = int32Data.INT32_ValueCount;
                UserDataInt32List = int32Data.UserDataItem_Int32Data_List;
            }
        }

        [TypeConverter(typeof(CGFXPropertyGridSet.CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public RealNumber RealNumber_Data { get; set; }
        public class RealNumber
        {
            public CGFXLibrary.CGFXSection.DataComponent.CGFXUserData.RealNumber.UD_FloatType UD_FloatType { get; set; }
            public string UD_FloatNumberName { get; set; }
            public string UD_RealNumberName { get; set; }

            //Flag : 00 00 00 80
            public int REALNUMBERCount { get; set; } //0x4

            public int UnkData { get; set; }

            public List<CGFXLibrary.CGFXSection.DataComponent.CGFXUserData.RealNumber.UserDataItem_REALNUMBER> UserDataRealNumberList { get; set; }

            public RealNumber(CGFXLibrary.CGFXSection.DataComponent.CGFXUserData.RealNumber realNumber)
            {
                UD_FloatType = realNumber.UD_Float_Type;
                UD_FloatNumberName = realNumber.FloatNumberName;
                UD_RealNumberName = realNumber.RealNumberName;
                REALNUMBERCount = realNumber.REALNUMBERCount;
                UserDataRealNumberList = realNumber.UserDataItem_RealNumber_List;
            }
        }

        public UserDataEntryPropertyGrid(CGFXFormat.CGFXData userDataEntry)
        {
            if (userDataEntry.CGFXDataSection.GetType().IsAssignableFrom(typeof(CGFXLibrary.CGFXSection.DataComponent.CGFXUserData.StringData)))
            {
                String_Data = new StringData(userDataEntry.GetCGFXData<CGFXLibrary.CGFXSection.DataComponent.CGFXUserData.StringData>());
            }
            if (userDataEntry.CGFXDataSection.GetType().IsAssignableFrom(typeof(CGFXLibrary.CGFXSection.DataComponent.CGFXUserData.Int32Data)))
            {
                Int32_Data = new Int32Data(userDataEntry.GetCGFXData<CGFXLibrary.CGFXSection.DataComponent.CGFXUserData.Int32Data>());
            }
            if (userDataEntry.CGFXDataSection.GetType().IsAssignableFrom(typeof(CGFXLibrary.CGFXSection.DataComponent.CGFXUserData.RealNumber)))
            {
                RealNumber_Data = new RealNumber(userDataEntry.GetCGFXData<CGFXLibrary.CGFXSection.DataComponent.CGFXUserData.RealNumber>());
            }
        }
    }
}
