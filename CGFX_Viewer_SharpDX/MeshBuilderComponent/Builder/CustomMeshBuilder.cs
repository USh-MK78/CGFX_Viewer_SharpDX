using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using SharpDX;
using Vector3D = global::SharpDX.Vector3;
using Vector3DCollection = HelixToolkit.Wpf.SharpDX.Vector3Collection;
using Point3DCollection = HelixToolkit.Wpf.SharpDX.Vector3Collection;
using PointCollection = HelixToolkit.Wpf.SharpDX.Vector2Collection;
using Int32Collection = HelixToolkit.Wpf.SharpDX.IntCollection;
using MeshFaces = HelixToolkit.Wpf.SharpDX.MeshFaces;
using CGFX_Viewer_SharpDX.MeshBuilderComponent.Mesh.MeshGeometry;

namespace CGFX_Viewer_SharpDX.MeshBuilderComponent.Builder
{
    ///// <summary>
    ///// Box face enumeration.
    ///// </summary>
    //[Flags]
    //public enum BoxFaces
    //{
    //    /// <summary>
    //    /// The top.
    //    /// </summary>
    //    PositiveZ = 0x1,
    //    /// <summary>
    //    /// The top.
    //    /// </summary>
    //    Top = PositiveZ,

    //    /// <summary>
    //    /// The bottom.
    //    /// </summary>
    //    NegativeZ = 0x2,
    //    /// <summary>
    //    /// The bottom.
    //    /// </summary>
    //    Bottom = NegativeZ,

    //    /// <summary>
    //    /// The left side.
    //    /// </summary>
    //    NegativeY = 0x4,
    //    /// <summary>
    //    /// The left side.
    //    /// </summary>
    //    Left = NegativeY,

    //    /// <summary>
    //    /// The right side.
    //    /// </summary>
    //    PositiveY = 0x8,
    //    /// <summary>
    //    /// The right side.
    //    /// </summary>
    //    Right = PositiveY,

    //    /// <summary>
    //    /// The front side.
    //    /// </summary>
    //    PositiveX = 0x10,
    //    /// <summary>
    //    /// The front side.
    //    /// </summary>
    //    Front = PositiveX,

    //    /// <summary>
    //    /// The back side.
    //    /// </summary>
    //    NegativeX = 0x20,
    //    /// <summary>
    //    /// The back side.
    //    /// </summary>
    //    Back = NegativeX,

    //    /// <summary>
    //    /// All sides.
    //    /// </summary>
    //    All = PositiveZ | NegativeZ | NegativeY | PositiveY | PositiveX | NegativeX
    //}
    ///// <summary>
    ///// MeshFaces.
    ///// </summary>
    //public enum MeshFaces
    //{
    //    /// <summary>
    //    /// Normal Face (3 Points).
    //    /// </summary>
    //    Default,
    //    /// <summary>
    //    /// Face with 4 Points.
    //    /// </summary>
    //    QuadPatches,
    //}

    internal class CustomMeshBuilder
    {
        #region Variables and Properties

        /// <summary>
        /// The positions.
        /// </summary>
        private Point3DCollection positions;
        public Point3DCollection Positions
        {
            get
            {
                return this.positions;
            }
        }

        /// <summary>
        /// The triangle indices.
        /// </summary>
        private Int32Collection triangleIndices;
        public Int32Collection TriangleIndices
        {
            get
            {
                return this.triangleIndices;
            }
        }

        /// <summary>
        /// The normal vectors.
        /// </summary>
        private Vector3DCollection normals;
        public Vector3DCollection Normals
        {
            get
            {
                return this.normals;
            }
            set
            {
                this.normals = value;
            }
        }

        /// <summary>
        /// The Tangents.
        /// </summary>
        private Vector3DCollection tangents;
        public Vector3DCollection Tangents
        {
            get
            {
                return this.tangents;
            }
            set
            {
                this.tangents = value;
            }
        }

        /// <summary>
        /// The Bi-Tangents.
        /// </summary>
        private Vector3DCollection bitangents;
        public Vector3DCollection BiTangents
        {
            get
            {
                return this.bitangents;
            }
            set
            {
                this.bitangents = value;
            }
        }

        #region Texture Coordinates 0, 1, 2
        /// <summary>
        /// The texture coordinates 0
        /// </summary>
        private PointCollection textureCoordinates_0;
        public PointCollection TextureCoordinates_0
        {
            get
            {
                return this.textureCoordinates_0;
            }
            set
            {
                this.textureCoordinates_0 = value;
            }
        }

        /// <summary>
        /// The texture coordinates 1
        /// </summary>
        private PointCollection textureCoordinates_1;
        public PointCollection TextureCoordinates_1
        {
            get
            {
                return this.textureCoordinates_1;
            }
            set
            {
                this.textureCoordinates_1 = value;
            }
        }

        /// <summary>
        /// The texture coordinates 2
        /// </summary>
        private PointCollection textureCoordinates_2;
        public PointCollection TextureCoordinates_2
        {
            get
            {
                return this.textureCoordinates_2;
            }
            set
            {
                this.textureCoordinates_2 = value;
            }
        }
        #endregion

        /// <summary>
        /// Do we have Normals or not.
        /// </summary>
        public bool HasNormals
        {
            get
            {
                return this.normals != null;
            }
        }

        /// <summary>
        /// Do we have Tangents or not.
        /// </summary>
        public bool HasTangents
        {
            get
            {
                return this.tangents != null;
            }
        }

        #region HasTexCoord 0, 1, 2
        /// <summary>
        /// Do we have Texture Coordinates or not. (0)
        /// </summary>
        public bool HasTexCoords0
        {
            get
            {
                return this.textureCoordinates_0 != null;
            }
        }

        /// <summary>
        /// Do we have Texture Coordinates or not. (1)
        /// </summary>
        public bool HasTexCoords1
        {
            get
            {
                return this.textureCoordinates_1 != null;
            }
        }

        /// <summary>
        /// Do we have Texture Coordinates or not. (2)
        /// </summary>
        public bool HasTexCoords2
        {
            get
            {
                return this.textureCoordinates_2 != null;
            }
        }
        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether to create normal vectors.
        /// </summary>
        /// <value>
        /// <c>true</c> if normal vectors should be created; otherwise, <c>false</c>.
        /// </value>
        public bool CreateNormals
        {
            get
            {
                return this.normals != null;
            }
            set
            {
                if (value && this.normals == null)
                {
                    this.normals = new Vector3DCollection();
                }
                if (!value)
                {
                    this.normals = null;
                }
            }
        }

        #region Create TextureCoordinates 0, 1, 2
        /// <summary>
        /// Gets or sets a value indicating whether to create texture coordinates.
        /// </summary>
        /// <value>
        /// <c>true</c> if texture coordinates should be created; otherwise, <c>false</c>.
        /// </value>
        public bool CreateTextureCoordinates_0
        {
            get
            {
                return this.textureCoordinates_0 != null;
            }
            set
            {
                if (value && this.textureCoordinates_0 == null)
                {
                    this.textureCoordinates_0 = new PointCollection();
                }
                if (!value)
                {
                    this.textureCoordinates_0 = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to create texture coordinates.
        /// </summary>
        /// <value>
        /// <c>true</c> if texture coordinates should be created; otherwise, <c>false</c>.
        /// </value>
        public bool CreateTextureCoordinates_1
        {
            get
            {
                return this.textureCoordinates_1 != null;
            }
            set
            {
                if (value && this.textureCoordinates_1 == null)
                {
                    this.textureCoordinates_1 = new PointCollection();
                }
                if (!value)
                {
                    this.textureCoordinates_1 = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to create texture coordinate 2
        /// </summary>
        /// <value>
        /// <c>true</c> if texture coordinates should be created; otherwise, <c>false</c>.
        /// </value>
        public bool CreateTextureCoordinates_2
        {
            get
            {
                return this.textureCoordinates_2 != null;
            }
            set
            {
                if (value && this.textureCoordinates_2 == null)
                {
                    this.textureCoordinates_2 = new PointCollection();
                }
                if (!value)
                {
                    this.textureCoordinates_2 = null;
                }
            }
        }
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMeshBuilder"/> class.
        /// </summary>
        /// <remarks>
        /// Normal and texture coordinate generation are included.
        /// </remarks>
        public CustomMeshBuilder()
            : this(true, true, true, true, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMeshBuilder"/> class.
        /// </summary>
        /// <param name="Layout">
        /// Buffer layout
        /// </param>
        /// <param name="generateNormals">
        /// Generate normal vectors.
        /// </param>
        /// <param name="generateTexCoords0">
        /// Generate texture coordinates 0
        /// </param>
        /// <param name="generateTexCoords1">
        /// Generate texture coordinates 1
        /// </param>
        /// <param name="generateTexCoords2">
        /// Generate texture coordinates 2
        /// </param>
        /// <param name="tangentSpace">
        /// Generate tangents.
        /// </param>
        public CustomMeshBuilder(bool generateNormals = true, bool generateTexCoords0 = true, bool generateTexCoords1 = false, bool generateTexCoords2 = false, bool tangentSpace = false)
        {
            this.positions = new Point3DCollection();
            this.triangleIndices = new Int32Collection();
            if (generateNormals) this.normals = new Vector3DCollection();
            if (generateTexCoords0) this.textureCoordinates_0 = new PointCollection();
            if (generateTexCoords1) this.textureCoordinates_1 = new PointCollection();
            if (generateTexCoords2) this.textureCoordinates_2 = new PointCollection();
            if (tangentSpace)
            {
                this.tangents = new Vector3DCollection();
                this.bitangents = new Vector3DCollection();
            }
        }
        #endregion

        #region Geometric Base Functions
        /// <summary>
        /// Calculate the Mesh's Normals
        /// </summary>
        /// <param name="positions">The Positions.</param>
        /// <param name="triangleIndices">The TriangleIndices.</param>
        /// <param name="normals">The calcualted Normals.</param>
        private static void ComputeNormals(Point3DCollection positions, Int32Collection triangleIndices, out Vector3DCollection normals)
        {
            normals = new Vector3DCollection(positions.Count);
            for (var i = 0; i < positions.Count; i++)
            {
                normals.Add(new Vector3D(0, 0, 0));
            }
            for (var t = 0; t < triangleIndices.Count; t += 3)
            {
                var i1 = triangleIndices[t];
                var i2 = triangleIndices[t + 1];
                var i3 = triangleIndices[t + 2];
                var v1 = positions[i1];
                var v2 = positions[i2];
                var v3 = positions[i3];
                var p1 = v2 - v1;
                var p2 = v3 - v1;
                var n = Vector3.Cross(p1, p2);
                // angle
                p1.Normalize();
                p2.Normalize();
                var a = (float)Math.Acos(CustomMeshBuilderHelper.DotProduct(ref p1, ref p2));
                n.Normalize();
                normals[i1] += (a * n);
                normals[i2] += (a * n);
                normals[i3] += (a * n);
            }
            for (var i = 0; i < normals.Count; i++)
            {
                //Cannot use normals[i].normalize() if using Media3D.Vector3DCollection. Does not change the internal value in Vector3DCollection.
                var n = normals[i];
                n.Normalize();
                normals[i] = n;
            }
        }
        /// <summary>
        /// Calculate the Mesh's Tangents
        /// </summary>
        /// <param name="meshFaces">The Faces of the Mesh</param>
        public void ComputeTangents(MeshFaces meshFaces, int TextureCoordinateIndex = 0)
        {
            PointCollection textureCoordinates = null;
            if (TextureCoordinateIndex == 0) textureCoordinates = this.textureCoordinates_0;
            else if (TextureCoordinateIndex == 1) textureCoordinates = this.textureCoordinates_1;
            else if (TextureCoordinateIndex == 2) textureCoordinates = this.textureCoordinates_2;

            switch (meshFaces)
            {
                case MeshFaces.Default:
                    if (this.positions != null & this.triangleIndices != null & this.normals != null & textureCoordinates != null)
                    {
                        Vector3DCollection t1, t2;
                        ComputeTangents(this.positions, this.normals, textureCoordinates, this.triangleIndices, out t1, out t2);
                        this.tangents = t1;
                        this.bitangents = t2;
                    }
                    break;
                case MeshFaces.QuadPatches:
                    if (this.positions != null & this.triangleIndices != null & this.normals != null & textureCoordinates != null)
                    {
                        Vector3DCollection t1, t2;
                        ComputeTangentsQuads(this.positions, this.normals, textureCoordinates, this.triangleIndices, out t1, out t2);
                        this.tangents = t1;
                        this.bitangents = t2;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Tangent Space computation for IndexedTriangle meshes
        /// Based on:
        /// http://www.terathon.com/code/tangent.html
        /// </summary>
        public static void ComputeTangents(Point3DCollection positions, Vector3DCollection normals, PointCollection textureCoordinates, Int32Collection triangleIndices,
            out Vector3DCollection tangents, out Vector3DCollection bitangents)
        {
            var tan1 = new Vector3D[positions.Count];
            for (var t = 0; t < triangleIndices.Count; t += 3)
            {
                var i1 = triangleIndices[t];
                var i2 = triangleIndices[t + 1];
                var i3 = triangleIndices[t + 2];
                var v1 = positions[i1];
                var v2 = positions[i2];
                var v3 = positions[i3];
                var w1 = textureCoordinates[i1];
                var w2 = textureCoordinates[i2];
                var w3 = textureCoordinates[i3];
                var x1 = v2.X - v1.X;
                var x2 = v3.X - v1.X;
                var y1 = v2.Y - v1.Y;
                var y2 = v3.Y - v1.Y;
                var z1 = v2.Z - v1.Z;
                var z2 = v3.Z - v1.Z;
                var s1 = w2.X - w1.X;
                var s2 = w3.X - w1.X;
                var t1 = w2.Y - w1.Y;
                var t2 = w3.Y - w1.Y;
                var r = 1.0f / (s1 * t2 - s2 * t1);
                var udir = new Vector3D((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                tan1[i1] += udir;
                tan1[i2] += udir;
                tan1[i3] += udir;
            }
            tangents = new Vector3DCollection(positions.Count);
            bitangents = new Vector3DCollection(positions.Count);
            for (var i = 0; i < positions.Count; i++)
            {
                var n = normals[i];
                var t = tan1[i];
                t = (t - n * CustomMeshBuilderHelper.DotProduct(ref n, ref t));
                t.Normalize();
                var b = Vector3.Cross(n, t);
                tangents.Add(t);
                bitangents.Add(b);
            }
        }

        /// <summary>
        /// Calculate the Tangents for a Quad.
        /// </summary>
        /// <param name="positions">The Positions.</param>
        /// <param name="normals">The Normals.</param>
        /// <param name="textureCoordinates">The TextureCoordinates.</param>
        /// <param name="indices">The Indices.</param>
        /// <param name="tangents">The calculated Tangens.</param>
        /// <param name="bitangents">The calculated Bi-Tangens.</param>
        public static void ComputeTangentsQuads(Point3DCollection positions, Vector3DCollection normals, PointCollection textureCoordinates, Int32Collection indices,
            out Vector3DCollection tangents, out Vector3DCollection bitangents)
        {
            var tan1 = new Vector3D[positions.Count];
            for (var t = 0; t < indices.Count; t += 4)
            {
                var i1 = indices[t];
                var i2 = indices[t + 1];
                var i3 = indices[t + 2];
                var i4 = indices[t + 3];
                var v1 = positions[i1];
                var v2 = positions[i2];
                var v3 = positions[i3];
                var v4 = positions[i4];
                var w1 = textureCoordinates[i1];
                var w2 = textureCoordinates[i2];
                var w3 = textureCoordinates[i3];
                var w4 = textureCoordinates[i4];
                var x1 = v2.X - v1.X;
                var x2 = v4.X - v1.X;
                var y1 = v2.Y - v1.Y;
                var y2 = v4.Y - v1.Y;
                var z1 = v2.Z - v1.Z;
                var z2 = v4.Z - v1.Z;
                var s1 = w2.X - w1.X;
                var s2 = w4.X - w1.X;
                var t1 = w2.Y - w1.Y;
                var t2 = w4.Y - w1.Y;
                var r = 1.0f / (s1 * t2 - s2 * t1);
                var udir = new Vector3D((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                tan1[i1] += udir;
                tan1[i2] += udir;
                tan1[i3] += udir;
                tan1[i4] += udir;
            }
            tangents = new Vector3DCollection(positions.Count);
            bitangents = new Vector3DCollection(positions.Count);
            for (var i = 0; i < positions.Count; i++)
            {
                var n = normals[i];
                var t = tan1[i];
                t = (t - n * CustomMeshBuilderHelper.DotProduct(ref n, ref t));
                t.Normalize();
                var b = Vector3.Cross(n, t);
                tangents.Add(t);
                bitangents.Add(b);
            }
        }

        /// <summary>
        /// Calculate the Tangents for a BufferMeshGeometry3D.
        /// </summary>
        /// <param name="meshGeometry">BufferMeshGeometry3D</param>
        public static void ComputeTangents(CGFXMeshGeometry3D meshGeometry, int TextureCoordinateIndex)
        {
            PointCollection textureCoordinates = null;
            if (TextureCoordinateIndex == 0) textureCoordinates = meshGeometry.TextureCoordinates_0;
            else if (TextureCoordinateIndex == 1) textureCoordinates = meshGeometry.TextureCoordinates_1;
            else if (TextureCoordinateIndex == 2) textureCoordinates = meshGeometry.TextureCoordinates_2;

            Vector3DCollection t1, t2;
            ComputeTangents(meshGeometry.Positions, meshGeometry.Normals, textureCoordinates, meshGeometry.TriangleIndices, out t1, out t2);
#if SHARPDX
            meshGeometry.Tangents = new Vector3DCollection(t1);
            meshGeometry.BiTangents = new Vector3DCollection(t2);
#endif
        }

        /// <summary>
        /// Calculate the Normals and Tangents for all MeshFaces.
        /// </summary>
        /// <param name="meshFaces">The MeshFaces.</param>
        /// <param name="tangents">Also calculate the Tangents or not.</param>
        public void ComputeNormalsAndTangents(MeshFaces meshFaces, int TextureCoordinateIndex, bool tangents = false)
        {
            PointCollection textureCoordinates = null;
            if (TextureCoordinateIndex == 0) textureCoordinates = this.textureCoordinates_0;
            else if (TextureCoordinateIndex == 1) textureCoordinates = this.textureCoordinates_1;
            else if (TextureCoordinateIndex == 2) textureCoordinates = this.textureCoordinates_2;

            if (!this.HasNormals & positions != null & triangleIndices != null)
            {
                ComputeNormals(positions, triangleIndices, out normals);
            }
            switch (meshFaces)
            {
                case MeshFaces.Default:
                    if (tangents & this.HasNormals & textureCoordinates != null)
                    {
                        Vector3DCollection t1, t2;
                        ComputeTangents(this.positions, this.normals, textureCoordinates, this.triangleIndices, out t1, out t2);
                        this.tangents = t1;
                        this.bitangents = t2;
                    }
                    break;
                case MeshFaces.QuadPatches:
                    if (tangents & this.HasNormals & textureCoordinates != null)
                    {
                        Vector3DCollection t1, t2;
                        ComputeTangentsQuads(this.positions, this.normals, textureCoordinates, this.triangleIndices, out t1, out t2);
                        this.tangents = t1;
                        this.bitangents = t2;
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        public void Add(Vector3 Position, Vector3 Normal, Vector3 Tangent, Vector3 BiTangent, Vector2 TexCoord0, Vector2 TexCoord1, Vector2 TexCoord2)
        {
            Positions.Add(Position);
            Normals.Add(Normal);
            Tangents.Add(Tangent);
            BiTangents.Add(BiTangent);

            //TODO : Color

            TextureCoordinates_0.Add(TexCoord0);
            TextureCoordinates_1.Add(TexCoord1);
            TextureCoordinates_2.Add(TexCoord2);
        }

        //        #region Add Geometry
        //        /// <summary>
        //        /// Adds a polygon.
        //        /// </summary>
        //        /// <param name="points">
        //        /// The points of the polygon.
        //        /// </param>
        //        /// <remarks>
        //        /// If the number of points is greater than 4, a triangle fan is used.
        //        /// </remarks>
        //        public void AddPolygon(IList<Point3D> points)
        //        {
        //            switch (points.Count)
        //            {
        //                case 3:
        //                    this.AddTriangle(points[0], points[1], points[2]);
        //                    break;
        //                case 4:
        //                    this.AddQuad(points[0], points[1], points[2], points[3]);
        //                    break;
        //                default:
        //                    this.AddTriangleFan(points);
        //                    break;
        //            }
        //        }
        //        /// <summary>
        //        /// Adds a polygon specified by vertex index (uses a triangle fan).
        //        /// </summary>
        //        /// <param name="vertexIndices">The vertex indices.</param>
        //        public void AddPolygon(IList<int> vertexIndices)
        //        {
        //            var n = vertexIndices.Count;
        //            for (var i = 0; i + 2 < n; i++)
        //            {
        //                //this.BufferDictionary.Where(x => (Buffer.BufferType)x.Value[0] == Buffer.BufferType.TriangleIndices).Select(x => x.Value[2]).Any(x => x is Int32Collection).
        //                //this.BufferDictionary.Where(x => (Buffer.BufferType)x.Value[0] == Buffer.BufferType.TriangleIndices).ToDictionary()[0][2] as Int32Collection

        //                //(this.BufferDictionary[1][4] as Point3DCollection).Add(new Point3D());
        //                this.triangleIndices.Add(vertexIndices[0]);
        //                this.triangleIndices.Add(vertexIndices[i + 1]);
        //                this.triangleIndices.Add(vertexIndices[i + 2]);
        //            }
        //        }


        //        /// <summary>
        //        /// Adds a quad (exactely 4 indices)
        //        /// </summary>
        //        /// <param name="vertexIndices">The vertex indices.</param>
        //        public void AddQuad(IList<int> vertexIndices)
        //        {
        //            for (var i = 0; i < 4; i++)
        //            {
        //                this.triangleIndices.Add(vertexIndices[i]);
        //            }
        //        }
        //        /// <summary>
        //        /// Adds a quadrilateral polygon.
        //        /// </summary>
        //        /// <param name="p0">
        //        /// The first point.
        //        /// </param>
        //        /// <param name="p1">
        //        /// The second point.
        //        /// </param>
        //        /// <param name="p2">
        //        /// The third point.
        //        /// </param>
        //        /// <param name="p3">
        //        /// The fourth point.
        //        /// </param>
        //        /// <remarks>
        //        /// See http://en.wikipedia.org/wiki/Quadrilateral.
        //        /// </remarks>
        //        public void AddQuad(Point3D p0, Point3D p1, Point3D p2, Point3D p3)
        //        {
        //            //// The nodes are arranged in counter-clockwise order
        //            //// p3               p2
        //            //// +---------------+
        //            //// |               |
        //            //// |               |
        //            //// +---------------+
        //            //// origin               p1
        //            var uv0 = new Point(0, 0);
        //            var uv1 = new Point(1, 0);
        //            var uv2 = new Point(1, 1);
        //            var uv3 = new Point(0, 1);
        //            this.AddQuad(p0, p1, p2, p3, uv0, uv1, uv2, uv3);
        //        }
        //        /// <summary>
        //        /// Adds a quadrilateral polygon.
        //        /// </summary>
        //        /// <param name="p0">
        //        /// The first point.
        //        /// </param>
        //        /// <param name="p1">
        //        /// The second point.
        //        /// </param>
        //        /// <param name="p2">
        //        /// The third point.
        //        /// </param>
        //        /// <param name="p3">
        //        /// The fourth point.
        //        /// </param>
        //        /// <param name="uv0">
        //        /// The first texture coordinate.
        //        /// </param>
        //        /// <param name="uv1">
        //        /// The second texture coordinate.
        //        /// </param>
        //        /// <param name="uv2">
        //        /// The third texture coordinate.
        //        /// </param>
        //        /// <param name="uv3">
        //        /// The fourth texture coordinate.
        //        /// </param>
        //        /// <remarks>
        //        /// See http://en.wikipedia.org/wiki/Quadrilateral.
        //        /// </remarks>
        //        public void AddQuad(Point3D p0, Point3D p1, Point3D p2, Point3D p3, Point uv0, Point uv1, Point uv2, Point uv3)
        //        {
        //            //// The nodes are arranged in counter-clockwise order
        //            //// p3               p2
        //            //// +---------------+
        //            //// |               |
        //            //// |               |
        //            //// +---------------+
        //            //// origin               p1
        //            var i0 = this.positions.Count;

        //            this.positions.Add(p0);
        //            this.positions.Add(p1);
        //            this.positions.Add(p2);
        //            this.positions.Add(p3);

        //            if (this.textureCoordinates != null)
        //            {
        //                this.textureCoordinates.Add(uv0);
        //                this.textureCoordinates.Add(uv1);
        //                this.textureCoordinates.Add(uv2);
        //                this.textureCoordinates.Add(uv3);
        //            }

        //            if (this.normals != null)
        //            {
        //                var p10 = p1 - p0;
        //                var p30 = p3 - p0;
        //                var w = Vector3.Cross(p10, p30);
        //                w.Normalize();
        //                this.normals.Add(w);
        //                this.normals.Add(w);
        //                this.normals.Add(w);
        //                this.normals.Add(w);
        //            }

        //            this.triangleIndices.Add(i0 + 0);
        //            this.triangleIndices.Add(i0 + 1);
        //            this.triangleIndices.Add(i0 + 2);

        //            this.triangleIndices.Add(i0 + 2);
        //            this.triangleIndices.Add(i0 + 3);
        //            this.triangleIndices.Add(i0 + 0);
        //        }
        //        /// <summary>
        //        /// Adds a list of quadrilateral polygons.
        //        /// </summary>
        //        /// <param name="quadPositions">
        //        /// The points.
        //        /// </param>
        //        /// <param name="quadNormals">
        //        /// The normal vectors.
        //        /// </param>
        //        /// <param name="quadTextureCoordinates">
        //        /// The texture coordinates.
        //        /// </param>
        //        public void AddQuads(
        //            IList<Point3D> quadPositions, IList<Vector3D> quadNormals, IList<Point> quadTextureCoordinates)
        //        {
        //            if (quadPositions == null)
        //            {
        //                throw new ArgumentNullException(nameof(quadPositions));
        //            }

        //            if (this.normals != null && quadNormals == null)
        //            {
        //                throw new ArgumentNullException(nameof(quadNormals));
        //            }

        //            if (this.textureCoordinates != null && quadTextureCoordinates == null)
        //            {
        //                throw new ArgumentNullException(nameof(quadTextureCoordinates));
        //            }

        //            if (quadNormals != null && quadNormals.Count != quadPositions.Count)
        //            {
        //                throw new InvalidOperationException(WrongNumberOfNormals);
        //            }

        //            if (quadTextureCoordinates != null && quadTextureCoordinates.Count != quadPositions.Count)
        //            {
        //                throw new InvalidOperationException(WrongNumberOfTextureCoordinates);
        //            }

        //            Debug.Assert(quadPositions.Count > 0 && quadPositions.Count % 4 == 0, "Wrong number of positions.");

        //            var index0 = this.positions.Count;
        //            foreach (var p in quadPositions)
        //            {
        //                this.positions.Add(p);
        //            }

        //            if (this.textureCoordinates != null && quadTextureCoordinates != null)
        //            {
        //                foreach (var tc in quadTextureCoordinates)
        //                {
        //                    this.textureCoordinates.Add(tc);
        //                }
        //            }

        //            if (this.normals != null && quadNormals != null)
        //            {
        //                foreach (var n in quadNormals)
        //                {
        //                    this.normals.Add(n);
        //                }
        //            }

        //            var indexEnd = this.positions.Count;
        //            for (var i = index0; i + 3 < indexEnd; i++)
        //            {
        //                this.triangleIndices.Add(i);
        //                this.triangleIndices.Add(i + 1);
        //                this.triangleIndices.Add(i + 2);

        //                this.triangleIndices.Add(i + 2);
        //                this.triangleIndices.Add(i + 3);
        //                this.triangleIndices.Add(i);
        //            }
        //        }


        //        /// <summary>
        //        /// Adds a triangle (exactely 3 indices)
        //        /// </summary>
        //        /// <param name="vertexIndices">The vertex indices.</param>
        //        public void AddTriangle(IList<int> vertexIndices)
        //        {
        //            for (var i = 0; i < 3; i++)
        //            {
        //                this.triangleIndices.Add(vertexIndices[i]);
        //            }
        //        }
        //        /// <summary>
        //        /// Adds a triangle.
        //        /// </summary>
        //        /// <param name="p0">
        //        /// The first point.
        //        /// </param>
        //        /// <param name="p1">
        //        /// The second point.
        //        /// </param>
        //        /// <param name="p2">
        //        /// The third point.
        //        /// </param>
        //        public void AddTriangle(Point3D p0, Point3D p1, Point3D p2)
        //        {
        //            var uv0 = new Point(0, 0);
        //            var uv1 = new Point(1, 0);
        //            var uv2 = new Point(0, 1);
        //            this.AddTriangle(p0, p1, p2, uv0, uv1, uv2);
        //        }
        //        /// <summary>
        //        /// Adds a triangle.
        //        /// </summary>
        //        /// <param name="p0">
        //        /// The first point.
        //        /// </param>
        //        /// <param name="p1">
        //        /// The second point.
        //        /// </param>
        //        /// <param name="p2">
        //        /// The third point.
        //        /// </param>
        //        /// <param name="uv0">
        //        /// The first texture coordinate.
        //        /// </param>
        //        /// <param name="uv1">
        //        /// The second texture coordinate.
        //        /// </param>
        //        /// <param name="uv2">
        //        /// The third texture coordinate.
        //        /// </param>
        //        public void AddTriangle(Point3D p0, Point3D p1, Point3D p2, Point uv0, Point uv1, Point uv2)
        //        {
        //            var i0 = this.positions.Count;

        //            this.positions.Add(p0);
        //            this.positions.Add(p1);
        //            this.positions.Add(p2);

        //            if (this.textureCoordinates != null)
        //            {
        //                this.textureCoordinates.Add(uv0);
        //                this.textureCoordinates.Add(uv1);
        //                this.textureCoordinates.Add(uv2);
        //            }

        //            if (this.normals != null)
        //            {
        //                var p10 = p1 - p0;
        //                var p20 = p2 - p0;
        //                var w = Vector3.Cross(p10, p20);
        //                w.Normalize();
        //                this.normals.Add(w);
        //                this.normals.Add(w);
        //                this.normals.Add(w);
        //            }

        //            this.triangleIndices.Add(i0 + 0);
        //            this.triangleIndices.Add(i0 + 1);
        //            this.triangleIndices.Add(i0 + 2);
        //        }
        //        /// <summary>
        //        /// Adds a triangle fan.
        //        /// </summary>
        //        /// <param name="vertices">
        //        /// The vertex indices of the triangle fan.
        //        /// </param>
        //        public void AddTriangleFan(IList<int> vertices)
        //        {
        //            for (var i = 0; i + 2 < vertices.Count; i++)
        //            {
        //                this.triangleIndices.Add(vertices[0]);
        //                this.triangleIndices.Add(vertices[i + 1]);
        //                this.triangleIndices.Add(vertices[i + 2]);
        //            }
        //        }
        //        /// <summary>
        //        /// Adds a triangle fan to the mesh
        //        /// </summary>
        //        /// <param name="fanPositions">
        //        /// The points of the triangle fan.
        //        /// </param>
        //        /// <param name="fanNormals">
        //        /// The normal vectors of the triangle fan.
        //        /// </param>
        //        /// <param name="fanTextureCoordinates">
        //        /// The texture coordinates of the triangle fan.
        //        /// </param>
        //        public void AddTriangleFan(
        //            IList<Point3D> fanPositions, IList<Vector3D> fanNormals = null, IList<Point> fanTextureCoordinates = null)
        //        {
        //            if (this.positions == null)
        //            {
        //                throw new ArgumentNullException(nameof(fanPositions));
        //            }

        //            if (this.normals != null && fanNormals == null)
        //            {
        //                throw new ArgumentNullException(nameof(fanNormals));
        //            }

        //            if (this.textureCoordinates != null && fanTextureCoordinates == null)
        //            {
        //                throw new ArgumentNullException(nameof(fanTextureCoordinates));
        //            }

        //            if (fanPositions.Count < 3)
        //                return;

        //            var index0 = this.positions.Count;
        //            foreach (var p in fanPositions)
        //            {
        //                this.positions.Add(p);
        //            }

        //            if (this.textureCoordinates != null && fanTextureCoordinates != null)
        //            {
        //                foreach (var tc in fanTextureCoordinates)
        //                {
        //                    this.textureCoordinates.Add(tc);
        //                }
        //            }

        //            if (this.normals != null && fanNormals != null)
        //            {
        //                foreach (var n in fanNormals)
        //                {
        //                    this.normals.Add(n);
        //                }
        //            }

        //            var indexEnd = this.positions.Count;
        //            for (var i = index0; i + 2 < indexEnd; i++)
        //            {
        //                this.triangleIndices.Add(index0);
        //                this.triangleIndices.Add(i + 1);
        //                this.triangleIndices.Add(i + 2);
        //            }
        //        }
        //        /// <summary>
        //        /// Adds a list of triangles.
        //        /// </summary>
        //        /// <param name="trianglePositions">
        //        /// The points (the number of points must be a multiple of 3).
        //        /// </param>
        //        /// <param name="triangleNormals">
        //        /// The normal vectors (corresponding to the points).
        //        /// </param>
        //        /// <param name="triangleTextureCoordinates">
        //        /// The texture coordinates (corresponding to the points).
        //        /// </param>
        //        public void AddTriangles(
        //            IList<Point3D> trianglePositions, IList<Vector3D> triangleNormals = null, IList<Point> triangleTextureCoordinates = null)
        //        {
        //            if (trianglePositions == null)
        //            {
        //                throw new ArgumentNullException(nameof(trianglePositions));
        //            }

        //            if (this.normals != null && triangleNormals == null)
        //            {
        //                throw new ArgumentNullException(nameof(triangleNormals));
        //            }

        //            if (this.textureCoordinates != null && triangleTextureCoordinates == null)
        //            {
        //                throw new ArgumentNullException(nameof(triangleTextureCoordinates));
        //            }

        //            if (trianglePositions.Count % 3 != 0)
        //            {
        //                throw new InvalidOperationException(WrongNumberOfPositions);
        //            }

        //            if (triangleNormals != null && triangleNormals.Count != trianglePositions.Count)
        //            {
        //                throw new InvalidOperationException(WrongNumberOfNormals);
        //            }

        //            if (triangleTextureCoordinates != null && triangleTextureCoordinates.Count != trianglePositions.Count)
        //            {
        //                throw new InvalidOperationException(WrongNumberOfTextureCoordinates);
        //            }

        //            var index0 = this.positions.Count;
        //            foreach (var p in trianglePositions)
        //            {
        //                this.positions.Add(p);
        //            }

        //            if (this.textureCoordinates != null && triangleTextureCoordinates != null)
        //            {
        //                foreach (var tc in triangleTextureCoordinates)
        //                {
        //                    this.textureCoordinates.Add(tc);
        //                }
        //            }

        //            if (this.normals != null && triangleNormals != null)
        //            {
        //                foreach (var n in triangleNormals)
        //                {
        //                    this.normals.Add(n);
        //                }
        //            }

        //            var indexEnd = this.positions.Count;
        //            for (var i = index0; i < indexEnd; i++)
        //            {
        //                this.triangleIndices.Add(i);
        //            }
        //        }
        //        /// <summary>
        //        /// Adds a triangle strip to the mesh.
        //        /// </summary>
        //        /// <param name="stripPositions">
        //        /// The points of the triangle strip.
        //        /// </param>
        //        /// <param name="stripNormals">
        //        /// The normal vectors of the triangle strip.
        //        /// </param>
        //        /// <param name="stripTextureCoordinates">
        //        /// The texture coordinates of the triangle strip.
        //        /// </param>
        //        /// <remarks>
        //        /// See http://en.wikipedia.org/wiki/Triangle_strip.
        //        /// </remarks>
        //        public void AddTriangleStrip(
        //            IList<Point3D> stripPositions, IList<Vector3D> stripNormals = null, IList<Point> stripTextureCoordinates = null)
        //        {
        //            if (stripPositions == null)
        //            {
        //                throw new ArgumentNullException(nameof(stripPositions));
        //            }

        //            if (this.normals != null && stripNormals == null)
        //            {
        //                throw new ArgumentNullException(nameof(stripNormals));
        //            }

        //            if (this.textureCoordinates != null && stripTextureCoordinates == null)
        //            {
        //                throw new ArgumentNullException(nameof(stripTextureCoordinates));
        //            }

        //            if (stripNormals != null && stripNormals.Count != stripPositions.Count)
        //            {
        //                throw new InvalidOperationException(WrongNumberOfNormals);
        //            }

        //            if (stripTextureCoordinates != null && stripTextureCoordinates.Count != stripPositions.Count)
        //            {
        //                throw new InvalidOperationException(WrongNumberOfTextureCoordinates);
        //            }

        //            var index0 = this.positions.Count;
        //            for (var i = 0; i < stripPositions.Count; i++)
        //            {
        //                this.positions.Add(stripPositions[i]);
        //                if (this.normals != null && stripNormals != null)
        //                {
        //                    this.normals.Add(stripNormals[i]);
        //                }

        //                if (this.textureCoordinates != null && stripTextureCoordinates != null)
        //                {
        //                    this.textureCoordinates.Add(stripTextureCoordinates[i]);
        //                }
        //            }

        //            var indexEnd = this.positions.Count;
        //            for (var i = index0; i + 2 < indexEnd; i += 2)
        //            {
        //                this.triangleIndices.Add(i);
        //                this.triangleIndices.Add(i + 1);
        //                this.triangleIndices.Add(i + 2);

        //                if (i + 3 < indexEnd)
        //                {
        //                    this.triangleIndices.Add(i + 1);
        //                    this.triangleIndices.Add(i + 3);
        //                    this.triangleIndices.Add(i + 2);
        //                }
        //            }
        //        }
        //        #endregion Add Geometry


        //        #region Helper Functions
        //        /// <summary>
        //        /// Appends the specified mesh.
        //        /// </summary>
        //        /// <param name="mesh">
        //        /// The mesh.
        //        /// </param>
        //        public void Append(MeshBuilder mesh)
        //        {
        //            if (mesh == null)
        //            {
        //                throw new ArgumentNullException(nameof(mesh));
        //            }

        //            this.Append(mesh.Positions, mesh.TriangleIndices, mesh.Normals, mesh.TextureCoordinates);
        //        }

        //#if !NETFX_CORE
        //        /// <summary>
        //        /// Appends the specified mesh.
        //        /// </summary>
        //        /// <param name="mesh">
        //        /// The mesh.
        //        /// </param>
        //        public void Append(MeshGeometry3D mesh)
        //        {
        //            if (mesh == null)
        //            {
        //                throw new ArgumentNullException(nameof(mesh));
        //            }

        //            this.Append(mesh.Positions, mesh.TriangleIndices, this.normals != null ? mesh.Normals : null, this.textureCoordinates != null ? mesh.TextureCoordinates : null);
        //        }
        //#endif

        //        /// <summary>
        //        /// Appends the specified points and triangles.
        //        /// </summary>
        //        /// <param name="positionsToAppend">
        //        /// The points to append.
        //        /// </param>
        //        /// <param name="triangleIndicesToAppend">
        //        /// The triangle indices to append.
        //        /// </param>
        //        /// <param name="normalsToAppend">
        //        /// The normal vectors to append.
        //        /// </param>
        //        /// <param name="textureCoordinatesToAppend">
        //        /// The texture coordinates to append.
        //        /// </param>
        //        public void Append(
        //            IList<Point3D> positionsToAppend, IList<int> triangleIndicesToAppend,
        //            IList<Vector3D> normalsToAppend = null, IList<Point> textureCoordinatesToAppend = null)
        //        {
        //            if (positionsToAppend == null)
        //            {
        //                throw new ArgumentNullException(nameof(positionsToAppend));
        //            }

        //            if (this.normals != null && normalsToAppend == null)
        //            {
        //                throw new InvalidOperationException(SourceMeshNormalsShouldNotBeNull);
        //            }

        //            if (this.textureCoordinates != null && textureCoordinatesToAppend == null)
        //            {
        //                throw new InvalidOperationException(SourceMeshTextureCoordinatesShouldNotBeNull);
        //            }

        //            if (normalsToAppend != null && normalsToAppend.Count != positionsToAppend.Count)
        //            {
        //                throw new InvalidOperationException(WrongNumberOfNormals);
        //            }

        //            if (textureCoordinatesToAppend != null && textureCoordinatesToAppend.Count != positionsToAppend.Count)
        //            {
        //                throw new InvalidOperationException(WrongNumberOfTextureCoordinates);
        //            }

        //            var index0 = this.positions.Count;
        //            foreach (var p in positionsToAppend)
        //            {
        //                this.positions.Add(p);
        //            }

        //            if (this.normals != null && normalsToAppend != null)
        //            {
        //                foreach (var n in normalsToAppend)
        //                {
        //                    this.normals.Add(n);
        //                }
        //            }

        //            if (this.textureCoordinates != null && textureCoordinatesToAppend != null)
        //            {
        //                foreach (var t in textureCoordinatesToAppend)
        //                {
        //                    this.textureCoordinates.Add(t);
        //                }
        //            }

        //            foreach (var i in triangleIndicesToAppend)
        //            {
        //                this.triangleIndices.Add(index0 + i);
        //            }
        //        }

        //#if !NETFX_CORE
        //        /// <summary>
        //        /// Checks the performance limits.
        //        /// </summary>
        //        /// <remarks>
        //        /// See <a href="https://msdn.microsoft.com/en-us/library/bb613553(v=vs.100).aspx">MSDN</a>.
        //        /// Try to keep mesh sizes under these limits:
        //        /// Positions : 20,001 point instances
        //        /// TriangleIndices : 60,003 integer instances
        //        /// </remarks>
        //        public void CheckPerformanceLimits()
        //        {
        //            if (this.positions.Count > 20000)
        //            {
        //                Trace.WriteLine(string.Format("Too many positions ({0}).", this.positions.Count));
        //            }

        //            if (this.triangleIndices.Count > 60002)
        //            {
        //                Trace.WriteLine(string.Format("Too many triangle indices ({0}).", this.triangleIndices.Count));
        //            }
        //        }
        //#endif

        //        /// <summary>
        //        /// Makes sure no triangles share the same vertex.
        //        /// </summary>
        //        private void NoSharedVertices()
        //        {
        //            var p = new Point3DCollection();
        //            var ti = new Int32Collection();
        //            Vector3DCollection n = null;
        //            if (this.normals != null)
        //            {
        //                n = new Vector3DCollection();
        //            }

        //            PointCollection tc = null;
        //            if (this.textureCoordinates != null)
        //            {
        //                tc = new PointCollection();
        //            }

        //            for (var i = 0; i < this.triangleIndices.Count; i += 3)
        //            {
        //                var i0 = i;
        //                var i1 = i + 1;
        //                var i2 = i + 2;
        //                var index0 = this.triangleIndices[i0];
        //                var index1 = this.triangleIndices[i1];
        //                var index2 = this.triangleIndices[i2];
        //                var p0 = this.positions[index0];
        //                var p1 = this.positions[index1];
        //                var p2 = this.positions[index2];
        //                p.Add(p0);
        //                p.Add(p1);
        //                p.Add(p2);
        //                ti.Add(i0);
        //                ti.Add(i1);
        //                ti.Add(i2);
        //                if (n != null)
        //                {
        //                    n.Add(this.normals[index0]);
        //                    n.Add(this.normals[index1]);
        //                    n.Add(this.normals[index2]);
        //                }

        //                if (tc != null)
        //                {
        //                    tc.Add(this.textureCoordinates[index0]);
        //                    tc.Add(this.textureCoordinates[index1]);
        //                    tc.Add(this.textureCoordinates[index2]);
        //                }
        //            }

        //            this.positions = p;
        //            this.triangleIndices = ti;
        //            this.normals = n;
        //            this.textureCoordinates = tc;
        //        }
        //        /// <summary>
        //        /// Scales the positions (and normal vectors).
        //        /// </summary>
        //        /// <param name="scaleX">
        //        /// The X scale factor.
        //        /// </param>
        //        /// <param name="scaleY">
        //        /// The Y scale factor.
        //        /// </param>
        //        /// <param name="scaleZ">
        //        /// The Z scale factor.
        //        /// </param>
        //        public void Scale(double scaleX, double scaleY, double scaleZ)
        //        {
        //            for (var i = 0; i < this.Positions.Count; i++)
        //            {
        //                this.Positions[i] = new Point3D(
        //                    this.Positions[i].X * (DoubleOrSingle)scaleX, this.Positions[i].Y * (DoubleOrSingle)scaleY, this.Positions[i].Z * (DoubleOrSingle)scaleZ);
        //            }

        //            if (this.Normals != null)
        //            {
        //                for (var i = 0; i < this.Normals.Count; i++)
        //                {
        //                    var v = new Vector3D(
        //                        this.Normals[i].X * (DoubleOrSingle)scaleX, this.Normals[i].Y * (DoubleOrSingle)scaleY, this.Normals[i].Z * (DoubleOrSingle)scaleZ);
        //                    v.Normalize();
        //                    this.Normals[i] = v;
        //                }
        //            }
        //        }
        //        /// <summary>
        //        /// Subdivides each triangle into four sub-triangles.
        //        /// </summary>
        //        private void Subdivide4()
        //        {
        //            // Each triangle is divided into four subtriangles, adding new vertices in the middle of each edge.
        //            var ip = this.Positions.Count;
        //            var ntri = this.TriangleIndices.Count;
        //            for (var i = 0; i < ntri; i += 3)
        //            {
        //                var i0 = this.TriangleIndices[i];
        //                var i1 = this.TriangleIndices[i + 1];
        //                var i2 = this.TriangleIndices[i + 2];
        //                var p0 = this.Positions[i0];
        //                var p1 = this.Positions[i1];
        //                var p2 = this.Positions[i2];
        //                var v01 = p1 - p0;
        //                var v12 = p2 - p1;
        //                var v20 = p0 - p2;
        //                var p01 = p0 + (v01 * 0.5f);
        //                var p12 = p1 + (v12 * 0.5f);
        //                var p20 = p2 + (v20 * 0.5f);

        //                var i01 = ip++;
        //                var i12 = ip++;
        //                var i20 = ip++;

        //                this.Positions.Add(p01);
        //                this.Positions.Add(p12);
        //                this.Positions.Add(p20);

        //                if (this.normals != null)
        //                {
        //                    var n = this.Normals[i0];
        //                    this.Normals.Add(n);
        //                    this.Normals.Add(n);
        //                    this.Normals.Add(n);
        //                }

        //                if (this.textureCoordinates != null)
        //                {
        //                    var uv0 = this.TextureCoordinates[i0];
        //                    var uv1 = this.TextureCoordinates[i0 + 1];
        //                    var uv2 = this.TextureCoordinates[i0 + 2];
        //                    var t01 = uv1 - uv0;
        //                    var t12 = uv2 - uv1;
        //                    var t20 = uv0 - uv2;
        //                    var u01 = uv0 + (t01 * 0.5f);
        //                    var u12 = uv1 + (t12 * 0.5f);
        //                    var u20 = uv2 + (t20 * 0.5f);
        //                    this.TextureCoordinates.Add(u01);
        //                    this.TextureCoordinates.Add(u12);
        //                    this.TextureCoordinates.Add(u20);
        //                }

        //                // TriangleIndices[i ] = i0;
        //                this.TriangleIndices[i + 1] = i01;
        //                this.TriangleIndices[i + 2] = i20;

        //                this.TriangleIndices.Add(i01);
        //                this.TriangleIndices.Add(i1);
        //                this.TriangleIndices.Add(i12);

        //                this.TriangleIndices.Add(i12);
        //                this.TriangleIndices.Add(i2);
        //                this.TriangleIndices.Add(i20);

        //                this.TriangleIndices.Add(i01);
        //                this.TriangleIndices.Add(i12);
        //                this.TriangleIndices.Add(i20);
        //            }
        //        }
        //        /// <summary>
        //        /// Subdivides each triangle into six triangles. Adds a vertex at the midpoint of each triangle.
        //        /// </summary>
        //        /// <remarks>
        //        /// See <a href="http://en.wikipedia.org/wiki/Barycentric_subdivision">wikipedia</a>.
        //        /// </remarks>
        //        private void SubdivideBarycentric()
        //        {
        //            // The BCS of a triangle S divides it into six triangles; each part has one vertex v2 at the
        //            // barycenter of S, another one v1 at the midpoint of some side, and the last one v0 at one
        //            // of the original vertices.
        //            var im = this.Positions.Count;
        //            var ntri = this.TriangleIndices.Count;
        //            for (var i = 0; i < ntri; i += 3)
        //            {
        //                var i0 = this.TriangleIndices[i];
        //                var i1 = this.TriangleIndices[i + 1];
        //                var i2 = this.TriangleIndices[i + 2];
        //                var p0 = this.Positions[i0];
        //                var p1 = this.Positions[i1];
        //                var p2 = this.Positions[i2];
        //                var v01 = p1 - p0;
        //                var v12 = p2 - p1;
        //                var v20 = p0 - p2;
        //                var p01 = p0 + (v01 * 0.5f);
        //                var p12 = p1 + (v12 * 0.5f);
        //                var p20 = p2 + (v20 * 0.5f);
        //                var m = new Point3D((p0.X + p1.X + p2.X) / 3, (p0.Y + p1.Y + p2.Y) / 3, (p0.Z + p1.Z + p2.Z) / 3);

        //                var i01 = im + 1;
        //                var i12 = im + 2;
        //                var i20 = im + 3;

        //                this.Positions.Add(m);
        //                this.Positions.Add(p01);
        //                this.Positions.Add(p12);
        //                this.Positions.Add(p20);

        //                if (this.normals != null)
        //                {
        //                    var n = this.Normals[i0];
        //                    this.Normals.Add(n);
        //                    this.Normals.Add(n);
        //                    this.Normals.Add(n);
        //                    this.Normals.Add(n);
        //                }

        //                if (this.textureCoordinates != null)
        //                {
        //                    var uv0 = this.TextureCoordinates[i0];
        //                    var uv1 = this.TextureCoordinates[i0 + 1];
        //                    var uv2 = this.TextureCoordinates[i0 + 2];
        //                    var t01 = uv1 - uv0;
        //                    var t12 = uv2 - uv1;
        //                    var t20 = uv0 - uv2;
        //                    var u01 = uv0 + (t01 * 0.5f);
        //                    var u12 = uv1 + (t12 * 0.5f);
        //                    var u20 = uv2 + (t20 * 0.5f);
        //                    var uvm = new Point((uv0.X + uv1.X) * 0.5f, (uv0.Y + uv1.Y) * 0.5f);
        //                    this.TextureCoordinates.Add(uvm);
        //                    this.TextureCoordinates.Add(u01);
        //                    this.TextureCoordinates.Add(u12);
        //                    this.TextureCoordinates.Add(u20);
        //                }

        //                // TriangleIndices[i ] = i0;
        //                this.TriangleIndices[i + 1] = i01;
        //                this.TriangleIndices[i + 2] = im;

        //                this.TriangleIndices.Add(i01);
        //                this.TriangleIndices.Add(i1);
        //                this.TriangleIndices.Add(im);

        //                this.TriangleIndices.Add(i1);
        //                this.TriangleIndices.Add(i12);
        //                this.TriangleIndices.Add(im);

        //                this.TriangleIndices.Add(i12);
        //                this.TriangleIndices.Add(i2);
        //                this.TriangleIndices.Add(im);

        //                this.TriangleIndices.Add(i2);
        //                this.TriangleIndices.Add(i20);
        //                this.TriangleIndices.Add(im);

        //                this.TriangleIndices.Add(i20);
        //                this.TriangleIndices.Add(i0);
        //                this.TriangleIndices.Add(im);

        //                im += 4;
        //            }
        //        }
        //        /// <summary>
        //        /// Performs a linear subdivision of the mesh.
        //        /// </summary>
        //        /// <param name="barycentric">
        //        /// Add a vertex in the center if set to <c>true</c> .
        //        /// </param>
        //        public void SubdivideLinear(bool barycentric = false)
        //        {
        //            if (barycentric)
        //            {
        //                this.SubdivideBarycentric();
        //            }
        //            else
        //            {
        //                this.Subdivide4();
        //            }
        //        }
        //        #endregion Helper Functions


        #region Exporter Functions
        public CGFXMeshGeometry3D ToCGFXMeshGeometry3D()
        {
            return new CGFXMeshGeometry3D()
            {
                TriangleIndices = triangleIndices,
                Positions = positions,
                Normals = normals,
                Tangents = tangents,
                BiTangents = bitangents,

                //TODO : Color

                TextureCoordinates_0 = textureCoordinates_0,
                TextureCoordinates_1 = textureCoordinates_1,
                TextureCoordinates_2 = textureCoordinates_2
            };
        }

        /// <summary>
        /// Generate a CGFXMeshGeometry3D from the generated Data.
        /// </summary>
        /// <param name="TextureCoordinateSetNum">TextureCoordinate number to used when generating tangents.</param>
        /// <returns></returns>
        public CGFXMeshGeometry3D ToMesh(int TextureCoordinateSetNum = 0)
        {
            return ToCGFXMeshGeometry3D(TextureCoordinateSetNum);
        }

        /// <summary>
        /// Converts the geometry to a <see cref="CGFXMeshGeometry3D"/> .        
        /// </summary>
        public CGFXMeshGeometry3D ToCGFXMeshGeometry3D(int TextureCoordinateSetNum = 0)
        {

            if (this.HasTangents && this.tangents.Count == 0)
            {
                PointCollection textureCoordinates = null;
                if (TextureCoordinateSetNum == 0) textureCoordinates = this.textureCoordinates_0;
                else if (TextureCoordinateSetNum == 1) textureCoordinates = this.textureCoordinates_1;
                else if (TextureCoordinateSetNum == 2) textureCoordinates = this.textureCoordinates_2;

                Vector3DCollection tan, bitan;
                ComputeTangents(this.positions, this.normals, textureCoordinates, this.triangleIndices, out tan, out bitan);
                this.tangents.AddRange(tan);
                this.bitangents.AddRange(bitan);
            }

            return new CGFXMeshGeometry3D()
            {
                Positions = this.positions,
                Indices = this.triangleIndices,
                Normals = (this.HasNormals) ? this.normals : null,
                Tangents = (this.HasTangents) ? this.tangents : null,
                BiTangents = (this.HasTangents) ? this.bitangents : null,

                //TODO : Color

                TextureCoordinates_0 = (this.HasTexCoords0) ? this.textureCoordinates_0 : null,
                TextureCoordinates_1 = (this.HasTexCoords1) ? this.textureCoordinates_1 : null,
                TextureCoordinates_2 = (this.HasTexCoords2) ? this.textureCoordinates_2 : null
            };
        }

        #endregion
    }
}
