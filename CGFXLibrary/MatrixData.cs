using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary
{
    public class MatrixData
    {
        //public Matrix3x4 MatrixData { get; set; }
        public struct Matrix3x4
        {
            public float M11;
            public float M12;
            public float M13;
            public float M14;

            public float M21;
            public float M22;
            public float M23;
            public float M24;

            public float M31;
            public float M32;
            public float M33;
            public float M34;

            public Matrix3x4(float[] array)
            {
                M11 = array[0];
                M12 = array[1];
                M13 = array[2];
                M14 = array[3];

                M21 = array[4];
                M22 = array[5];
                M23 = array[6];
                M24 = array[7];

                M31 = array[8];
                M32 = array[9];
                M33 = array[10];
                M34 = array[11];
            }
        }



        public class LocalMatrix
        {
            //4x4 Matrix(4byte)
            //m11, m12, m13, m14
            //m21, m22, m23, m24
            //m31, m32, m33, m34
            //m41, m42, m43, m44
            public float M11 { get; set; }
            public float M12 { get; set; }
            public float M13 { get; set; }
            public float M14 { get; set; }

            public float M21 { get; set; }
            public float M22 { get; set; }
            public float M23 { get; set; }
            public float M24 { get; set; }

            public float M31 { get; set; }
            public float M32 { get; set; }
            public float M33 { get; set; }
            public float M34 { get; set; }

            public float M41 { get; set; }
            public float M42 { get; set; }
            public float M43 { get; set; }
            public float M44 { get; set; }

            public LocalMatrix()
            {
                M11 = 0;
                M12 = 0;
                M13 = 0;
                M14 = 0;

                M21 = 0;
                M22 = 0;
                M23 = 0;
                M24 = 0;

                M31 = 0;
                M32 = 0;
                M33 = 0;
                M34 = 0;

                M41 = 0;
                M42 = 0;
                M43 = 0;
                M44 = 0;
            }

            public void ReadLocalMatrix(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                M11 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M12 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M13 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M14 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M21 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M22 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M23 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M24 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M31 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M32 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M33 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M34 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //M41 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //M42 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //M43 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //M44 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }
        }

        public class WorldMatrix_Transform
        {
            //4x4 Matrix[Transform](4byte)
            //m11, m12, m13, m14
            //m21, m22, m23, m24
            //m31, m32, m33, m34
            //m41, m42, m43, m44
            public float M11 { get; set; }
            public float M12 { get; set; }
            public float M13 { get; set; }
            public float M14 { get; set; }

            public float M21 { get; set; }
            public float M22 { get; set; }
            public float M23 { get; set; }
            public float M24 { get; set; }

            public float M31 { get; set; }
            public float M32 { get; set; }
            public float M33 { get; set; }
            public float M34 { get; set; }

            public float M41 { get; set; }
            public float M42 { get; set; }
            public float M43 { get; set; }
            public float M44 { get; set; }

            public WorldMatrix_Transform()
            {
                M11 = 0;
                M12 = 0;
                M13 = 0;
                M14 = 0;

                M21 = 0;
                M22 = 0;
                M23 = 0;
                M24 = 0;

                M31 = 0;
                M32 = 0;
                M33 = 0;
                M34 = 0;

                M41 = 0;
                M42 = 0;
                M43 = 0;
                M44 = 0;
            }

            public void ReadMatrix4x4Transform(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                M11 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M12 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M13 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M14 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M21 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M22 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M23 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M24 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M31 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M32 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M33 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M34 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //M41 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //M42 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //M43 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                //M44 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }
        }

        public class Matrix_BoundingBox
        {
            public float M11 { get; set; }
            public float M12 { get; set; }
            public float M13 { get; set; }

            public float M21 { get; set; }
            public float M22 { get; set; }
            public float M23 { get; set; }

            public float M31 { get; set; }
            public float M32 { get; set; }
            public float M33 { get; set; }

            public Matrix_BoundingBox(float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33)
            {
                M11 = m11;
                M12 = m12;
                M13 = m13;

                M21 = m21;
                M22 = m22;
                M23 = m23;

                M31 = m31;
                M32 = m32;
                M33 = m33;
            }

            public void ReadBoundingBoxMatrix(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                M11 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M12 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M13 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M21 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M22 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M23 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M31 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M32 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                M33 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }

            public override string ToString()
            {
                return "Matrix (3 * 3)";
            }
        }
    }
}
