using CGFXLibrary;
using CGFXLibrary.CGFXSection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFX_Viewer_SharpDX.CGFXPropertyGridSet
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    public class LUTS_PropertyGrid
    {
        public string Name { get; set; }

        public List<CGFXFormat.CGFXData> UserData_List = new List<CGFXFormat.CGFXData>();
        [Editor(typeof(CGFX_CustomPropertyGridClass.UserDataDictionaryEditor), typeof(UITypeEditor))]
        public List<CGFXFormat.CGFXData> userDataList { get => UserData_List; set => UserData_List = value; }

        public List<CGFXFormat.CGFXData> LUTS_ValueData_List = new List<CGFXFormat.CGFXData>();
        [Editor(typeof(CGFX_CustomPropertyGridClass.UserDataDictionaryEditor), typeof(UITypeEditor))]
        public List<CGFXFormat.CGFXData> LUTS_ValueDataList { get => LUTS_ValueData_List; set => LUTS_ValueData_List = value; }

        public LUTS_PropertyGrid(LUTS LUTS)
        {
            Name = LUTS.Name;
            userDataList = LUTS.UserdataDICT.DICT_Entries.Select(x => x.CGFXData).ToList();
            LUTS_ValueDataList = LUTS.LUTS_DICTData.DICT_Entries.Select(x => x.CGFXData).ToList();
        }
    }
}
