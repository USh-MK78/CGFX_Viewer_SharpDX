using CGFX_Viewer_SharpDX.CGFXPropertyGridSet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGFXLibrary;

namespace CGFX_Viewer_SharpDX.PropertyGridForms.Section.CMDL.ShapeData.VertexAttribute.Param
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    public class Param_PropertyGrid
    {
        //public int FormatType { get; set; }
        //public int ComponentCount { get; set; } //For example XYZ = 3, ST = 2, RGBA = 4
        public CGFXLibrary.VertexAttribute.Usage VertexAttributeUsageFlag { get; set; }
        public CGFXLibrary.VertexAttribute.Flag VertexAttributeFlag { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public Component Components { get; set; }
        public class Component
        {
            public Flags Flags { get; set; }

            public FormatType FormatTypes
            {
                get => (FormatType)Flags.IdentFlag[0];
                set => Flags.IdentFlag[0] = Convert.ToByte(Enum.ToObject(typeof(FormatType), value));
            }

            public enum FormatType
            {
                BYTE = 0,
                UNSIGNED_BYTE = 1,
                SHORT = 2,//might also be unsigned short
                FLOAT = 6
            }

            public int ComponentCount { get; set; } //For example XYZ = 3, ST = 2, RGBA = 4
            public ComponentType ComponentTypeFlag => (ComponentType)ComponentCount;
            public enum ComponentType
            {
                ST = 2,
                XYZ = 3,
                RGBA = 4
            }

            public Component(SOBJ.Shape.VertexAttribute.Param.Component component)
            {
                Flags = component.Flags;
                ComponentCount = component.ComponentCount;
            }

            public Component()
            {
                Flags = new Flags(new List<byte>().ToArray());
                ComponentCount = 0;
            }
        }

        public float Scale { get; set; }
        public int AttributeCount { get; set; }
        public int AttributeListOffset { get; set; }
        public List<float> AttributeList { get; set; }

        public Param_PropertyGrid(SOBJ.Shape.VertexAttribute.Param param)
        {
            //FormatType = param.FormatType;
            //ComponentCount = param.ComponentCount;
            VertexAttributeUsageFlag = param.VertexAttributeUsageFlag;
            VertexAttributeFlag = param.VertexAttributeFlag;
            Components = new Component(param.Components);

            Scale = param.Scale;
            AttributeCount = param.AttributeCount;
            AttributeListOffset = param.AttributeListOffset;
            AttributeList = param.AttributeList;
        }

        public Param_PropertyGrid()
        {
            //FormatType = 0;
            //ComponentCount = 0;
            VertexAttributeUsageFlag = new CGFXLibrary.VertexAttribute.Usage(-1);
            VertexAttributeFlag = new CGFXLibrary.VertexAttribute.Flag(-1);
            Components = new Component();
            Scale = 0;
            AttributeCount = 0;
            AttributeListOffset = 0;
            AttributeList = new List<float>();
        }
    }
}
