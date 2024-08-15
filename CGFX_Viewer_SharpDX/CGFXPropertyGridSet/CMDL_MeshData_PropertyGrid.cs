using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGFXLibrary;
using CGFXLibrary.CGFXSection;
using CGFXLibrary.SOBJ_Format;

namespace CGFX_Viewer_SharpDX.CGFXPropertyGridSet
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    public class CMDL_MeshData_PropertyGrid
    {
        [ReadOnly(true)]
        public char[] SOBJ_Header { get; set; }

        public int Revision { get; set; }

        [ReadOnly(true)]
        public int SOBJNameOffset { get; set; }
        public string Name;


        public int UnknownData2 { get; set; }
        public int UnknownOffset1 { get; set; } //Array (float (?))
        public int ShapeIndex { get; set; }
        public int MaterialIndex { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public UnknownColor UnknownColorSet { get; set; }
        public class UnknownColor
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public UnknownColor(int Input_R, int Input_G, int Input_B, int Input_A)
            {
                R = (byte)Input_R;
                G = (byte)Input_G;
                B = (byte)Input_B;
                A = (byte)Input_A;
            }

            public UnknownColor()
            {
                R = 0;
                G = 0;
                B = 0;
                A = 0;
            }
        }

        public bool IsVisible { get; set; }
        public byte RenderPriority { get; set; }
        public short OwnerModelOffset { get; set; }
        public short MeshNodeVisibilityIndex { get; set; }

        public byte[] UnknownBytes { get; set; }

        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }
        public int Unknown3 { get; set; }
        public int MeshIndex { get; set; }
        public int Unknown5 { get; set; }
        public int Unknown6 { get; set; }
        public int Unknown7 { get; set; }
        public int Unknown8 { get; set; }
        public int Unknown9 { get; set; }
        public int Unknown10 { get; set; }
        public int Unknown11 { get; set; }
        public int Unknown12 { get; set; }
        public int Unknown13 { get; set; }
        public int Unknown14 { get; set; }
        public int Unknown15 { get; set; }
        public int Unknown16 { get; set; }
        public int Unknown17 { get; set; }

        [ReadOnly(true)]
        public int MeshNameOffset { get; set; }
        public string MeshName;

        public int Unknown19 { get; set; }

        [ReadOnly(true)]
        public int MeshNodeNameOffset { get; set; }
        public string MeshNodeName;

        //unknownDataSection2

        public CMDL_MeshData_PropertyGrid(CMDL.MeshData meshData)
        {
            SOBJ SOBJ = meshData.SOBJData.GetCGFXData<SOBJ>();
            //SOBJ SOBJ = ((SOBJ)meshData.SOBJData.CGFXDataSection);

            SOBJ_Header = SOBJ.Mesh.SOBJ_Header;
            Revision = SOBJ.Mesh.Revision;

            SOBJNameOffset = SOBJ.Mesh.SOBJNameOffset;
            Name = SOBJ.Mesh.Name;

            UnknownData2 = SOBJ.Mesh.UnknownData2;
            UnknownOffset1 = SOBJ.Mesh.UnknownOffset1;

            ShapeIndex = SOBJ.Mesh.ShapeIndex;
            MaterialIndex = SOBJ.Mesh.MaterialIndex;

            UnknownColorSet = new UnknownColor
            {
                R = SOBJ.Mesh.UnknownColorSet.R,
                G = SOBJ.Mesh.UnknownColorSet.G,
                B = SOBJ.Mesh.UnknownColorSet.B,
                A = SOBJ.Mesh.UnknownColorSet.A
            };

            IsVisible = SOBJ.Mesh.IsVisible;
            RenderPriority = SOBJ.Mesh.RenderPriority;
            OwnerModelOffset = SOBJ.Mesh.OwnerModelOffset;
            MeshNodeVisibilityIndex = SOBJ.Mesh.MeshNodeVisibilityIndex;

            UnknownBytes = SOBJ.Mesh.UnknownBytes;

            Unknown1 = SOBJ.Mesh.Unknown1;
            Unknown2 = SOBJ.Mesh.Unknown2;
            Unknown3 = SOBJ.Mesh.Unknown3;
            MeshIndex = SOBJ.Mesh.MeshIndex;
            Unknown5 = SOBJ.Mesh.Unknown5;
            Unknown6 = SOBJ.Mesh.Unknown6;
            Unknown7 = SOBJ.Mesh.Unknown7;
            Unknown8 = SOBJ.Mesh.Unknown8;
            Unknown9 = SOBJ.Mesh.Unknown9;
            Unknown10 = SOBJ.Mesh.Unknown10;
            Unknown11 = SOBJ.Mesh.Unknown11;
            Unknown12 = SOBJ.Mesh.Unknown12;
            Unknown13 = SOBJ.Mesh.Unknown13;
            Unknown14 = SOBJ.Mesh.Unknown14;
            Unknown15 = SOBJ.Mesh.Unknown15;
            Unknown16 = SOBJ.Mesh.Unknown16;
            Unknown17 = SOBJ.Mesh.Unknown17;
            MeshNameOffset = SOBJ.Mesh.MeshNameOffset;
            MeshName = SOBJ.Mesh.MeshName;

            Unknown19 = SOBJ.Mesh.Unknown19;

            MeshNodeNameOffset = SOBJ.Mesh.MeshNodeNameOffset;
            MeshNodeName = SOBJ.Mesh.MeshNodeName;

            //unknownDataSection2
        }
    }
}
