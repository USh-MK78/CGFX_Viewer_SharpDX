using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGFXLibrary;
using CGFXLibrary.CGFXSection;

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
            SOBJ_Header = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.SOBJ_Header;
            Revision = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Revision;

            SOBJNameOffset = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.SOBJNameOffset;
            Name = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Name;

            UnknownData2 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.UnknownData2;
            UnknownOffset1 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.UnknownOffset1;

            ShapeIndex = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.ShapeIndex;
            MaterialIndex = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.MaterialIndex;

            UnknownColorSet = new UnknownColor
            {
                R = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.UnknownColorSet.R,
                G = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.UnknownColorSet.G,
                B = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.UnknownColorSet.B,
                A = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.UnknownColorSet.A
            };

            IsVisible = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.IsVisible;
            RenderPriority = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.RenderPriority;
            OwnerModelOffset = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.OwnerModelOffset;
            MeshNodeVisibilityIndex = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.MeshNodeVisibilityIndex;

            UnknownBytes = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.UnknownBytes;

            Unknown1 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Unknown1;
            Unknown2 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Unknown2;
            Unknown3 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Unknown3;
            MeshIndex = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.MeshIndex;
            Unknown5 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Unknown5;
            Unknown6 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Unknown6;
            Unknown7 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Unknown7;
            Unknown8 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Unknown8;
            Unknown9 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Unknown9;
            Unknown10 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Unknown10;
            Unknown11 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Unknown11;
            Unknown12 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Unknown12;
            Unknown13 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Unknown13;
            Unknown14 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Unknown14;
            Unknown15 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Unknown15;
            Unknown16 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Unknown16;
            Unknown17 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Unknown17;
            MeshNameOffset = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.MeshNameOffset;
            MeshName = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.MeshName;

            Unknown19 = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.Unknown19;

            MeshNodeNameOffset = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.MeshNodeNameOffset;
            MeshNodeName = meshData.SOBJData.SOBJ_Mesh_Section.Meshes.MeshNodeName;

            //SOBJ_Header = meshData.SOBJData.Meshes.SOBJ_Header;
            //Revision = meshData.SOBJData.Meshes.Revision;

            //SOBJNameOffset = meshData.SOBJData.Meshes.SOBJNameOffset;
            //Name = meshData.SOBJData.Meshes.Name;

            //UnknownData2 = meshData.SOBJData.Meshes.UnknownData2;
            //UnknownOffset1 = meshData.SOBJData.Meshes.UnknownOffset1;

            //ShapeIndex = meshData.SOBJData.Meshes.ShapeIndex;
            //MaterialIndex = meshData.SOBJData.Meshes.MaterialIndex;

            //UnknownColorSet = new UnknownColor
            //{
            //    R = meshData.SOBJData.Meshes.UnknownColorSet.R,
            //    G = meshData.SOBJData.Meshes.UnknownColorSet.G,
            //    B = meshData.SOBJData.Meshes.UnknownColorSet.B,
            //    A = meshData.SOBJData.Meshes.UnknownColorSet.A
            //};

            //IsVisible = meshData.SOBJData.Meshes.IsVisible;
            //RenderPriority = meshData.SOBJData.Meshes.RenderPriority;
            //OwnerModelOffset = meshData.SOBJData.Meshes.OwnerModelOffset;           
            //MeshNodeVisibilityIndex = meshData.SOBJData.Meshes.MeshNodeVisibilityIndex;

            //UnknownBytes = meshData.SOBJData.Meshes.UnknownBytes;

            //Unknown1 = meshData.SOBJData.Meshes.Unknown1;
            //Unknown2 = meshData.SOBJData.Meshes.Unknown2;
            //Unknown3 = meshData.SOBJData.Meshes.Unknown3;
            //MeshIndex = meshData.SOBJData.Meshes.MeshIndex;
            //Unknown5 = meshData.SOBJData.Meshes.Unknown5;
            //Unknown6 = meshData.SOBJData.Meshes.Unknown6;
            //Unknown7 = meshData.SOBJData.Meshes.Unknown7;
            //Unknown8 = meshData.SOBJData.Meshes.Unknown8;
            //Unknown9 = meshData.SOBJData.Meshes.Unknown9;
            //Unknown10 = meshData.SOBJData.Meshes.Unknown10;
            //Unknown11 = meshData.SOBJData.Meshes.Unknown11;
            //Unknown12 = meshData.SOBJData.Meshes.Unknown12;
            //Unknown13 = meshData.SOBJData.Meshes.Unknown13;
            //Unknown14 = meshData.SOBJData.Meshes.Unknown14;
            //Unknown15 = meshData.SOBJData.Meshes.Unknown15;
            //Unknown16 = meshData.SOBJData.Meshes.Unknown16;
            //Unknown17 = meshData.SOBJData.Meshes.Unknown17;
            //MeshNameOffset = meshData.SOBJData.Meshes.MeshNameOffset;
            //MeshName = meshData.SOBJData.Meshes.MeshName;

            //Unknown19 = meshData.SOBJData.Meshes.Unknown19;

            //MeshNodeNameOffset = meshData.SOBJData.Meshes.MeshNodeNameOffset;
            //MeshNodeName = meshData.SOBJData.Meshes.MeshNodeName;

            //unknownDataSection2
        }
    }
}
