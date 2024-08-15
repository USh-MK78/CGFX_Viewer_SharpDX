using CGFX_Viewer_SharpDX.PropertyGridForms.Section.CANM;
using CGFXLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace CGFX_Viewer_SharpDX.CGFXPropertyGridSet
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    public class CANM_PropertyGrid
    {
        public string Name { get; set; }
        public string AttributeNmae { get; set; }


        public int UnknownData0 { get; set; }
        public float AnimationFrameCount { get; set; }
        public int AnimationCount { get; set; }

        //public byte[] UnknownByteData { get; set; } //0x8
        public DICT UserDataDICT { get; set; }

        public List<CGFXLibrary.CGFXSection.DataComponent.AnimationData> CANM_AnimationData_List = new List<CGFXLibrary.CGFXSection.DataComponent.AnimationData>();
        [Editor(typeof(CANM_Editor), typeof(UITypeEditor))]
        public List<CGFXLibrary.CGFXSection.DataComponent.AnimationData> CANM_AnimationDataList { get => CANM_AnimationData_List; set => CANM_AnimationData_List = value; }

        public CANM_PropertyGrid(CGFXLibrary.CGFXSection.CANM CANM)
        {
            Name = CANM.Name;
            AttributeNmae = CANM.CANM_TangentAttributeName;

            UnknownData0 = CANM.LoopMode;
            AnimationFrameCount = CANM.AnimFrameCount;
            AnimationCount = CANM.MemberAnimationDICTCount;
            UserDataDICT = CANM.UserDataDICT;


            CANM_AnimationDataList = CANM.MemberAnimationDICT.DICT_Entries.Select(x => (CGFXLibrary.CGFXSection.DataComponent.AnimationData)x.CGFXData.CGFXDataSection).ToList();
        }
    }

    //CANMViewer
    public class CANM_Editor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (svc != null && value != null)
            {
                CANMEditor form = new CANMEditor(value as List<CGFXLibrary.CGFXSection.DataComponent.AnimationData>);
                form.ShowDialog();

                value = form.AnimationDataList;
            }
            return value; // can also replace the wrapper object here
        }
    }
}
