using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary.SOBJ_Format
{
    /// <summary>
    /// SOBJ
    /// </summary>
    public class SOBJ : IO.BinaryIOInterface.BinaryIO
    {
        public SOBJType Types;
        public enum SOBJType
        {
            Mesh = 0,
            Shape = 1
        }

        public MeshData.Mesh Mesh { get; set; }
        public ShapeData.Shape Shape { get; set; }

        public void Read_SOBJ(BinaryReader br, byte[] BOM)
        {
            if (Types == SOBJType.Mesh)
            {
                //ReadMesh
                Mesh.Read_MeshData(br, BOM);
            }
            else if (Types == SOBJType.Shape)
            {
                //ReadShape
                Shape.Read_ShapeData(br, BOM);
            }
        }

        public void Write_SOBJ(BinaryWriter bw)
        {

        }

        public override void Read(BinaryReader br, byte[] BOM)
        {
            Read_SOBJ(br, BOM);
        }

        public override void Write(BinaryWriter bw, byte[] BOM)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type">SOBJ Format Type</param>
        public SOBJ(SOBJType Type)
        {
            Types = Type;
            if (Type == SOBJType.Mesh)
            {
                Mesh = new MeshData.Mesh();
                Shape = null;
            }
            else if (Type == SOBJType.Shape)
            {
                Mesh = null;
                Shape = new ShapeData.Shape();
            }
        }

        //public SOBJ(Mesh mesh = null, Shape shape = null)
        //{
        //    if (mesh == null && shape == null) throw new Exception("meshとshapeが両方とも空です");
        //    Meshes = mesh;
        //    Shapes = shape;
        //}
    }
}
