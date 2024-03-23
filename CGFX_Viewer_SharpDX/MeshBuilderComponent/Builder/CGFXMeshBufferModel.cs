using CGFX_Viewer_SharpDX.MeshBuilderComponent.Mesh.MeshGeometry;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX.Render;
using HelixToolkit.Wpf.SharpDX.Utilities;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CGFX_Viewer_SharpDX.MeshBuilderComponent.Builder
{
    public class CGFXBufferModelStruct
    {
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct CGFXVertex
        {
            public Vector4 Position;
            public Vector3 Normal;
            public Vector3 Tangent;
            public Vector3 BiTangent;

            public const int SizeInBytes = 4 * (4 + 3 + 3 + 3);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct MultiTextureCoordinates
        {
            public Vector2 TexCoord0;
            public Vector2 TexCoord1;
            public Vector2 TexCoord2;
            //public Vector2 TexCoord3;

            public const int SizeInBytes = 4 * (2 + 2 + 2);
        }
    }

    public abstract class CGFXMeshBufferModel<VertexStruct> : CGFXGeometryBufferModel where VertexStruct : struct
    {
        protected static readonly VertexStruct[] emptyVerts = new VertexStruct[0];
        protected static readonly int[] emptyIndices = new int[0];

        /// <summary>
        /// Initializes a new instance of the <see cref="CGFXMeshBufferModel{VertexStruct}"/> class.
        /// </summary>
        /// <param name="structSize">Size of the structure.</param>
        /// <param name="dynamic">Create dynamic buffer or immutable buffer</param>
        public CGFXMeshBufferModel(int structSize, bool dynamic = false)
            : base(PrimitiveTopology.TriangleList,
                  dynamic ? new DynamicBufferProxy(structSize, BindFlags.VertexBuffer) : new ImmutableBufferProxy(structSize, BindFlags.VertexBuffer) as IElementsBufferProxy,
                  dynamic ? new DynamicBufferProxy(sizeof(int), BindFlags.IndexBuffer) : new ImmutableBufferProxy(sizeof(int), BindFlags.IndexBuffer) as IElementsBufferProxy)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CGFXMeshBufferModel{VertexStruct}"/> class.
        /// </summary>
        /// <param name="structSize">Size of the structure.</param>
        /// <param name="topology">The topology.</param>
        /// <param name="dynamic">Create dynamic buffer or immutable buffer</param>
        public CGFXMeshBufferModel(int structSize, PrimitiveTopology topology, bool dynamic = false)
            : base(topology,
                  dynamic ? new DynamicBufferProxy(structSize, BindFlags.VertexBuffer) : new ImmutableBufferProxy(structSize, BindFlags.VertexBuffer) as IElementsBufferProxy,
                  dynamic ? new DynamicBufferProxy(sizeof(int), BindFlags.IndexBuffer) : new ImmutableBufferProxy(sizeof(int), BindFlags.IndexBuffer) as IElementsBufferProxy)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CGFXMeshBufferModel{VertexStruct}"/> class.
        /// </summary>
        /// <param name="topology">The topology.</param>
        /// <param name="vertexBuffers"></param>
        /// <param name="dynamic">Create dynamic buffer or immutable buffer</param>
        public CGFXMeshBufferModel(PrimitiveTopology topology, IElementsBufferProxy[] vertexBuffers, bool dynamic = false)
            : base(topology,
                  vertexBuffers,
                  dynamic ? new DynamicBufferProxy(sizeof(int), BindFlags.IndexBuffer) : new ImmutableBufferProxy(sizeof(int), BindFlags.IndexBuffer) as IElementsBufferProxy)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CGFXMeshBufferModel{VertexStruct}"/> class.
        /// </summary>
        /// <param name="topology">The topology.</param>
        /// <param name="vertexBuffer">The vertex buffer.</param>
        /// <param name="indexBuffer">The index buffer.</param>
        protected CGFXMeshBufferModel(PrimitiveTopology topology, IElementsBufferProxy vertexBuffer, IElementsBufferProxy indexBuffer)
            : base(topology, vertexBuffer, indexBuffer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CGFXMeshBufferModel{VertexStruct}"/> class.
        /// </summary>
        /// <param name="topology">The topology.</param>
        /// <param name="vertexBuffer">The vertex buffer.</param>
        /// <param name="indexBuffer">The index buffer.</param>
        protected CGFXMeshBufferModel(PrimitiveTopology topology, IElementsBufferProxy[] vertexBuffer, IElementsBufferProxy indexBuffer)
            : base(topology, vertexBuffer, indexBuffer)
        {
        }


        /// <summary>
        /// Called when [create index buffer].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="geometry">The geometry.</param>
        /// <param name="deviceResources">The device resources.</param>
        protected override void OnCreateIndexBuffer(DeviceContextProxy context, IElementsBufferProxy buffer, Geometry3D geometry, IDeviceResources deviceResources)
        {
            if (geometry != null && geometry.Indices != null && geometry.Indices.Count > 0)
            {
                buffer.UploadDataToBuffer(context, geometry.Indices, geometry.Indices.Count, 0, geometry.PreDefinedIndexCount);
            }
            else
            {
                buffer.UploadDataToBuffer(context, emptyIndices, 0);
                //buffer.DisposeAndClear();
            }
        }
    }

    public class CGFXMeshGeometryBufferModel : CGFXMeshBufferModel<CGFXBufferModelStruct.CGFXVertex>
    {
        private static readonly Vector2[] emptyTextureArray0 = new Vector2[0]; //TexCoord_0
        private static readonly Vector2[] emptyTextureArray1 = new Vector2[0]; //TexCoord_1
        private static readonly Vector2[] emptyTextureArray2 = new Vector2[0]; //TexCoord_2
        private static readonly Vector4[] emptyColorArray = new Vector4[0]; //Color

        /// <summary>
        /// Initializes a new instance of the <see cref="CGFXMeshGeometryBufferModel"/> class.
        /// </summary>
        public CGFXMeshGeometryBufferModel()
            : base(PrimitiveTopology.TriangleList,
                  new[]
                  {
                          new ImmutableBufferProxy(CGFXBufferModelStruct.CGFXVertex.SizeInBytes, BindFlags.VertexBuffer),
                          new ImmutableBufferProxy(Vector2.SizeInBytes, BindFlags.VertexBuffer), //0
                          new ImmutableBufferProxy(Vector2.SizeInBytes, BindFlags.VertexBuffer), //1
                          new ImmutableBufferProxy(Vector2.SizeInBytes, BindFlags.VertexBuffer), //2
                          new ImmutableBufferProxy(Vector4.SizeInBytes, BindFlags.VertexBuffer)
                  } as IElementsBufferProxy[])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CGFXMeshGeometryBufferModel"/> class.
        /// </summary>
        /// <param name="buffers">The buffers.</param>
        /// <param name="isDynamic"></param>
        public CGFXMeshGeometryBufferModel(IElementsBufferProxy[] buffers, bool isDynamic)
            : base(PrimitiveTopology.TriangleList, buffers, isDynamic)
        {
        }
        /// <summary>
        /// Determines whether [is vertex buffer changed] [the specified property name].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="bufferIndex"></param>
        /// <returns>
        ///   <c>true</c> if [is vertex buffer changed] [the specified property name]; otherwise, <c>false</c>.
        /// </returns>
        protected override bool IsVertexBufferChanged(string propertyName, int bufferIndex)
        {
            switch (bufferIndex)
            {
                case 0:
                    return base.IsVertexBufferChanged(propertyName, bufferIndex);
                case 1:
                    return propertyName.Equals(nameof(Mesh.MeshGeometry.CGFXMeshGeometry3D.TextureCoordinates_0));
                case 2:
                    return propertyName.Equals(nameof(Mesh.MeshGeometry.CGFXMeshGeometry3D.TextureCoordinates_1));
                case 3:
                    return propertyName.Equals(nameof(Mesh.MeshGeometry.CGFXMeshGeometry3D.TextureCoordinates_2));
                case 4:
                    return propertyName.Equals(nameof(Mesh.MeshGeometry.CGFXMeshGeometry3D.Colors));
                default:
                    return false;
            }
        }
        /// <summary>
        /// Called when [create vertex buffer].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="bufferIndex">Index of the buffer.</param>
        /// <param name="geometry">The geometry.</param>
        /// <param name="deviceResources">The device resources.</param>
        protected override void OnCreateVertexBuffer(DeviceContextProxy context, IElementsBufferProxy buffer, int bufferIndex, Geometry3D geometry, IDeviceResources deviceResources)
        {
            if (geometry is CGFXMeshGeometry3D mesh)
            {
                switch (bufferIndex)
                {
                    case 0:
                        // -- set geometry if given
                        if (geometry.Positions != null && geometry.Positions.Count > 0)
                        {
                            // --- get geometry
                            var data = BuildVertexArray(mesh);
                            buffer.UploadDataToBuffer(context, data, geometry.Positions.Count, 0, geometry.PreDefinedVertexCount);
                        }
                        else
                        {
                            //buffer.DisposeAndClear();
                            buffer.UploadDataToBuffer(context, emptyVerts, 0);
                        }
                        break;
                    case 1:
                        if (mesh.TextureCoordinates_0 != null && mesh.TextureCoordinates_0.Count > 0)
                        {
                            buffer.UploadDataToBuffer(context, mesh.TextureCoordinates_0, mesh.TextureCoordinates_0.Count, 0, geometry.PreDefinedVertexCount);
                        }
                        else
                        {
                            buffer.UploadDataToBuffer(context, emptyTextureArray0, 0);
                        }
                        break;
                    case 2:
                        if (mesh.TextureCoordinates_1 != null && mesh.TextureCoordinates_1.Count > 0)
                        {
                            buffer.UploadDataToBuffer(context, mesh.TextureCoordinates_1, mesh.TextureCoordinates_1.Count, 0, geometry.PreDefinedVertexCount);
                        }
                        else
                        {
                            buffer.UploadDataToBuffer(context, emptyTextureArray1, 0);
                        }
                        break;
                    case 3:
                        if (mesh.TextureCoordinates_2 != null && mesh.TextureCoordinates_2.Count > 0)
                        {
                            buffer.UploadDataToBuffer(context, mesh.TextureCoordinates_2, mesh.TextureCoordinates_2.Count, 0, geometry.PreDefinedVertexCount);
                        }
                        else
                        {
                            buffer.UploadDataToBuffer(context, emptyTextureArray2, 0);
                        }
                        break;
                    case 4:
                        if (geometry.Colors != null && geometry.Colors.Count > 0)
                        {
                            buffer.UploadDataToBuffer(context, geometry.Colors, geometry.Colors.Count, 0, geometry.PreDefinedVertexCount);
                        }
                        else
                        {
                            buffer.UploadDataToBuffer(context, emptyColorArray, 0);
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Builds the vertex array.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns></returns>
        private CGFXBufferModelStruct.CGFXVertex[] BuildVertexArray(CGFXMeshGeometry3D geometry)
        {
            //var geometry = this.geometryInternal as Mesh.MeshGeometry.BufferMeshGeometry3D;
            var positions = geometry.Positions.GetEnumerator();
            var vertexCount = geometry.Positions.Count;

            var normals = geometry.Normals != null ? geometry.Normals.GetEnumerator() : Enumerable.Repeat(Vector3.Zero, vertexCount).GetEnumerator();
            var tangents = geometry.Tangents != null ? geometry.Tangents.GetEnumerator() : Enumerable.Repeat(Vector3.Zero, vertexCount).GetEnumerator();
            var bitangents = geometry.BiTangents != null ? geometry.BiTangents.GetEnumerator() : Enumerable.Repeat(Vector3.Zero, vertexCount).GetEnumerator();

            var array = ThreadBufferManager<CGFXBufferModelStruct.CGFXVertex>.GetBuffer(vertexCount);
            for (var i = 0; i < vertexCount; i++)
            {
                positions.MoveNext();
                normals.MoveNext();
                tangents.MoveNext();
                bitangents.MoveNext();
                array[i].Position = new Vector4(positions.Current, 1f);
                array[i].Normal = normals.Current;
                array[i].Tangent = tangents.Current;
                array[i].BiTangent = bitangents.Current;
            }
            normals.Dispose();
            tangents.Dispose();
            bitangents.Dispose();
            positions.Dispose();
            return array;
        }

        /// <summary>
        /// Builds the Multi Texture Coordinates array.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns></returns>
        private CGFXBufferModelStruct.MultiTextureCoordinates[] BuildMultiTexCoordsArray(CGFXMeshGeometry3D geometry)
        {
            //var geometry = this.geometryInternal as Mesh.MeshGeometry.BufferMeshGeometry3D;
            //var positions = geometry.Positions.GetEnumerator();
            var vertexCount = geometry.Positions.Count;

            var texCoord_0 = geometry.TextureCoordinates_0 != null ? geometry.TextureCoordinates_0.GetEnumerator() : Enumerable.Repeat(Vector2.Zero, vertexCount).GetEnumerator();
            var texCoord_1 = geometry.TextureCoordinates_1 != null ? geometry.TextureCoordinates_1.GetEnumerator() : Enumerable.Repeat(Vector2.Zero, vertexCount).GetEnumerator();
            var texCoord_2 = geometry.TextureCoordinates_2 != null ? geometry.TextureCoordinates_2.GetEnumerator() : Enumerable.Repeat(Vector2.Zero, vertexCount).GetEnumerator();

            var array = ThreadBufferManager<CGFXBufferModelStruct.MultiTextureCoordinates>.GetBuffer(vertexCount);
            for (var i = 0; i < vertexCount; i++)
            {
                texCoord_0.MoveNext();
                texCoord_1.MoveNext();
                texCoord_2.MoveNext();
                array[i].TexCoord0 = texCoord_0.Current;
                array[i].TexCoord1 = texCoord_1.Current;
                array[i].TexCoord2 = texCoord_2.Current;
            }

            texCoord_0.Dispose();
            texCoord_1.Dispose();
            texCoord_2.Dispose();
            return array;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class DynamicCGFXMeshBufferModel : CGFXMeshGeometryBufferModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicCGFXBufferModel"/> class.
        /// </summary>
        public DynamicCGFXMeshBufferModel() : base(new[]
        {
            new DynamicBufferProxy(CGFXBufferModelStruct.CGFXVertex.SizeInBytes, BindFlags.VertexBuffer),
            new DynamicBufferProxy(Vector2.SizeInBytes, BindFlags.VertexBuffer),
            new DynamicBufferProxy(Vector2.SizeInBytes, BindFlags.VertexBuffer),
            new DynamicBufferProxy(Vector2.SizeInBytes, BindFlags.VertexBuffer),
            new DynamicBufferProxy(Vector4.SizeInBytes, BindFlags.VertexBuffer)
        } as IElementsBufferProxy[], true)
        { }
    }
}
