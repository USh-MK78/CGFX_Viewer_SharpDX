﻿using CGFX_Viewer_SharpDX.CGFXPropertyGridSet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using CGFXLibrary;
using CGFXLibrary.SOBJ_Format.ShapeData;

namespace CGFX_Viewer_SharpDX.PropertyGridForms.Section.CMDL.ShapeData.PrimitiveSet
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    public class PrimitiveSet_PropertyGrid
    {
        public int PrimitiveSetOffset { get; set; }
        public int RelatedBoneCount { get; set; }
        public int RelatedBoneListOffset { get; set; }
        public List<int> RelatedBoneList { get; set; }
        public int SkinningMode { get; set; }
        public int PrimitiveCount { get; set; }
        public int PrimitiveOffsetListOffset { get; set; }

        public List<Shape.PrimitiveSet.Primitive> Primitive_List = new List<Shape.PrimitiveSet.Primitive>();
        [Editor(typeof(Primitive.PrimitiveEditor), typeof(UITypeEditor))]
        public List<Shape.PrimitiveSet.Primitive> PrimitiveList { get => Primitive_List; set => Primitive_List = value; }

        public PrimitiveSet_PropertyGrid(Shape.PrimitiveSet primitiveSet)
        {
            PrimitiveOffsetListOffset = primitiveSet.PrimitiveSetOffset;
            RelatedBoneCount = primitiveSet.RelatedBoneCount;
            RelatedBoneListOffset = primitiveSet.RelatedBoneListOffset;

            RelatedBoneList = primitiveSet.RelatedBoneList;

            SkinningMode = primitiveSet.SkinningMode;

            PrimitiveCount = primitiveSet.PrimitiveCount;
            PrimitiveOffsetListOffset = primitiveSet.PrimitiveOffsetListOffset;

            PrimitiveList = primitiveSet.Primitives;
        }
    }

    //PrimitiveSetEditor
    public class PrimitiveSetEditor : UITypeEditor
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
                PrimitiveSetEditorForm form = new PrimitiveSetEditorForm(value as List<Shape.PrimitiveSet>);
                form.ShowDialog();

                value = form.primitiveSet_List;
            }
            return value; // can also replace the wrapper object here
        }
    }
}
