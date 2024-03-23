using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX.Utilities;
using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Media;

namespace CGFX_Viewer_SharpDX.MeshBuilderComponent.Mesh.MeshGeometry
{
    [DataContract]
    public class CGFXMeshGeometry3D : Geometry3D
    {
        private static readonly PropertyChangedEventArgs textureCoord0ChangedArgs = new PropertyChangedEventArgs(nameof(TextureCoordinates_0));
        private static readonly PropertyChangedEventArgs textureCoord1ChangedArgs = new PropertyChangedEventArgs(nameof(TextureCoordinates_1));
        private static readonly PropertyChangedEventArgs textureCoord2ChangedArgs = new PropertyChangedEventArgs(nameof(TextureCoordinates_2));

        /// <summary>
        /// Used to scale up small triangle during hit test.
        /// </summary>
        public static float SmallTriangleHitTestScaling = 1e3f;
        /// <summary>
        /// Used to determine if the triangle is small.
        /// Small triangle is defined as any edge length square is smaller than
        /// <see cref="SmallTriangleEdgeLengthSquare"/>.
        /// </summary>
        public static float SmallTriangleEdgeLengthSquare = 1e-3f;
        /// <summary>
        /// Used to enable small triangle hit test. It uses <see cref="SmallTriangleEdgeLengthSquare"/>
        /// to determine if triangle is too small. If it is too small, scale up the triangle before
        /// hit test.
        /// </summary>
        public static bool EnableSmallTriangleHitTestScaling = true;
        /// <summary>
        /// Does not raise property changed event
        /// </summary>
        [DataMember]
        public Vector3Collection Normals
        {
            get; set;
        }

        #region Texture Coordinates 0, 1, 2
        private Vector2Collection textureCoordinates_0 = null;
        /// <summary>
        /// Texture Coordinates 0
        /// </summary>
        [DataMember]
        public Vector2Collection TextureCoordinates_0
        {
            get
            {
                return textureCoordinates_0;
            }
            set
            {
                if (Set(ref textureCoordinates_0, value, false))
                {
                    RaisePropertyChanged(textureCoord0ChangedArgs);
                }
            }
        }

        private Vector2Collection textureCoordinates_1 = null;
        /// <summary>
        /// Texture Coordinates 1
        /// </summary>
        [DataMember]
        public Vector2Collection TextureCoordinates_1
        {
            get
            {
                return textureCoordinates_1;
            }
            set
            {
                if (Set(ref textureCoordinates_1, value, false))
                {
                    RaisePropertyChanged(textureCoord1ChangedArgs);
                }
            }
        }

        private Vector2Collection textureCoordinates_2 = null;
        /// <summary>
        /// Texture Coordinates 2
        /// </summary>
        [DataMember]
        public Vector2Collection TextureCoordinates_2
        {
            get
            {
                return textureCoordinates_2;
            }
            set
            {
                if (Set(ref textureCoordinates_2, value, false))
                {
                    RaisePropertyChanged(textureCoord2ChangedArgs);
                }
            }
        }
        #endregion


        /// <summary>
        /// Does not raise property changed event
        /// </summary>
        [DataMember]
        public Vector3Collection Tangents
        {
            get; set;
        }

        /// <summary>
        /// Does not raise property changed event
        /// </summary>
        [DataMember]
        public Vector3Collection BiTangents
        {
            get; set;
        }

        public IEnumerable<Triangle> Triangles
        {
            get
            {
                for (var i = 0; i < Indices.Count; i += 3)
                {
                    yield return new Triangle() { P0 = Positions[Indices[i]], P1 = Positions[Indices[i + 1]], P2 = Positions[Indices[i + 2]], };
                }
            }
        }

        /// <summary>
        /// A proxy member for <see cref="Geometry3D.Indices"/>
        /// </summary>
        [IgnoreDataMember]
        public IntCollection TriangleIndices
        {
            get
            {
                return Indices;
            }
            set
            {
                Indices = new IntCollection(value);
            }
        }

        /// <summary>
        /// Merge meshes into one
        /// </summary>
        /// <param name="meshes"></param>
        /// <returns></returns>
        public static CGFXMeshGeometry3D Merge(params CGFXMeshGeometry3D[] meshes)
        {
            var positions = new Vector3Collection();
            var indices = new IntCollection();

            var normals = meshes.All(x => x.Normals != null) ? new Vector3Collection() : null;
            var colors = meshes.All(x => x.Colors != null) ? new Color4Collection() : null;
            var textureCoods_0 = meshes.All(x => x.TextureCoordinates_0 != null) ? new Vector2Collection() : null;
            var textureCoods_1 = meshes.All(x => x.TextureCoordinates_1 != null) ? new Vector2Collection() : null;
            var textureCoods_2 = meshes.All(x => x.TextureCoordinates_2 != null) ? new Vector2Collection() : null;
            var tangents = meshes.All(x => x.Tangents != null) ? new Vector3Collection() : null;
            var bitangents = meshes.All(x => x.BiTangents != null) ? new Vector3Collection() : null;

            var index = 0;
            foreach (var part in meshes)
            {
                positions.AddRange(part.Positions);
                indices.AddRange(part.Indices.Select(x => x + index));
                index += part.Positions.Count;
            }

            if (normals != null) normals = new Vector3Collection(meshes.SelectMany(x => x.Normals));
            if (colors != null) colors = new Color4Collection(meshes.SelectMany(x => x.Colors));
            if (textureCoods_0 != null) textureCoods_0 = new Vector2Collection(meshes.SelectMany(x => x.TextureCoordinates_0));
            if (textureCoods_1 != null) textureCoods_1 = new Vector2Collection(meshes.SelectMany(x => x.TextureCoordinates_1));
            if (textureCoods_2 != null) textureCoods_2 = new Vector2Collection(meshes.SelectMany(x => x.TextureCoordinates_2));
            if (tangents != null) tangents = new Vector3Collection(meshes.SelectMany(x => x.Tangents));
            if (bitangents != null) bitangents = new Vector3Collection(meshes.SelectMany(x => x.BiTangents));

            var mesh = new CGFXMeshGeometry3D()
            {
                Positions = positions,
                Indices = indices,
                Normals = normals,
                Colors = colors,
                TextureCoordinates_0 = textureCoods_0,
                TextureCoordinates_1 = textureCoods_1,
                TextureCoordinates_2 = textureCoods_2,
                Tangents = tangents,
                BiTangents = bitangents
            };
            return mesh;
        }


        protected override IOctreeBasic CreateOctree(OctreeBuildParameter parameter)
        {
            return new StaticMeshGeometryOctree(this.Positions, this.Indices, parameter);
        }

        protected override void OnAssignTo(Geometry3D target)
        {
            base.OnAssignTo(target);
            if (target is CGFXMeshGeometry3D mesh)
            {
                mesh.Normals = this.Normals;
                mesh.TextureCoordinates_0 = this.TextureCoordinates_0;
                mesh.TextureCoordinates_1 = this.TextureCoordinates_1;
                mesh.TextureCoordinates_2 = this.TextureCoordinates_2;
                mesh.Tangents = this.Tangents;
                mesh.BiTangents = this.BiTangents;
            }
        }

        /// <summary>
        /// Callers should set this property to true before calling HitTest if the callers need multiple hits throughout the geometry.
        /// This is useful when the geometry is cut by a plane.
        /// </summary>
        public bool ReturnMultipleHitsOnHitTest { get; set; } = false;

        public virtual bool HitTest(HitTestContext context, Matrix modelMatrix, ref List<HitTestResult> hits, object originalSource)
        {
            if (Positions == null || Positions.Count == 0
                || Indices == null || Indices.Count == 0)
            {
                return false;
            }
            var isHit = false;
            if (Octree != null)
            {
                isHit = Octree.HitTest(context, originalSource, this, modelMatrix, ReturnMultipleHitsOnHitTest, ref hits);
            }
            else
            {
                var result = new HitTestResult
                {
                    Distance = double.MaxValue
                };
                var modelInvert = modelMatrix.Inverted();
                if (modelInvert == Matrix.Zero)//Check if model matrix can be inverted.
                {
                    return false;
                }
                //transform ray into model coordinates
                var rayModel = new Ray(Vector3.TransformCoordinate(context.RayWS.Position, modelInvert), Vector3.Normalize(Vector3.TransformNormal(context.RayWS.Direction, modelInvert)));

                var b = this.Bound;

                //Do hit test in local space
                if (rayModel.Intersects(ref b))
                {
                    var index = 0;
                    var minDistance = float.MaxValue;

                    foreach (var t in Triangles)
                    {
                        // Used when geometry size is really small, causes hit test failure due to SharpDX.MathUtils.ZeroTolerance.
                        var scaling = 1f;
                        var rayScaled = rayModel;
                        if (EnableSmallTriangleHitTestScaling)
                        {
                            if ((t.P0 - t.P1).LengthSquared() < SmallTriangleEdgeLengthSquare
                                || (t.P1 - t.P2).LengthSquared() < SmallTriangleEdgeLengthSquare
                                || (t.P2 - t.P0).LengthSquared() < SmallTriangleEdgeLengthSquare)
                            {
                                scaling = SmallTriangleHitTestScaling;
                                rayScaled = new Ray(rayModel.Position * scaling, rayModel.Direction);
                            }
                        }
                        var v0 = t.P0 * scaling;
                        var v1 = t.P1 * scaling;
                        var v2 = t.P2 * scaling;

                        if (Collision.RayIntersectsTriangle(ref rayScaled, ref v0, ref v1, ref v2, out float d))
                        {
                            d /= scaling;
                            // For CrossSectionMeshGeometryModel3D another hit than the closest may be the valid one, since the closest one might be removed by a crossing plane
                            if (ReturnMultipleHitsOnHitTest)
                            {
                                minDistance = float.MaxValue;
                            }

                            if (d >= 0 && d < minDistance) // If d is NaN, the condition is false.
                            {
                                minDistance = d;
                                result.IsValid = true;
                                result.ModelHit = originalSource;
                                // transform hit-info to world space now:
                                var pointWorld = Vector3.TransformCoordinate(rayModel.Position + (rayModel.Direction * d), modelMatrix);
                                result.PointHit = pointWorld;
                                result.Distance = (context.RayWS.Position - pointWorld).Length();
                                var p0 = Vector3.TransformCoordinate(t.P0, modelMatrix);
                                var p1 = Vector3.TransformCoordinate(t.P1, modelMatrix);
                                var p2 = Vector3.TransformCoordinate(t.P2, modelMatrix);
                                var n = Vector3.Cross(p1 - p0, p2 - p0);
                                n.Normalize();
                                // transform hit-info to world space now:
                                result.NormalAtHit = n;// Vector3.TransformNormal(n, m).ToVector3D();
                                result.TriangleIndices = new System.Tuple<int, int, int>(Indices[index], Indices[index + 1], Indices[index + 2]);
                                result.Tag = index / 3;
                                result.Geometry = this;
                                isHit = true;
                                if (ReturnMultipleHitsOnHitTest)
                                {
                                    hits.Add(result);
                                    result = new HitTestResult();
                                }
                            }
                        }
                        index += 3;
                    }
                }
                if (isHit && result.IsValid && !ReturnMultipleHitsOnHitTest)
                {
                    hits.Add(result);
                }
            }
            return isHit;
        }

        /// <summary>
        /// Call to manually update texture coordinate buffer.
        /// </summary>
        public void UpdateTextureCoordinates()
        {
            RaisePropertyChanged(nameof(TextureCoordinates_0));
        }

        protected override void OnClearAllGeometryData()
        {
            base.OnClearAllGeometryData();
            Normals?.Clear();
            Normals?.TrimExcess();
            TextureCoordinates_0?.Clear();
            TextureCoordinates_0?.TrimExcess();
            TextureCoordinates_1?.Clear();
            TextureCoordinates_1?.TrimExcess();
            TextureCoordinates_2?.Clear();
            TextureCoordinates_2?.TrimExcess();
            Tangents?.Clear();
            Tangents?.TrimExcess();
            BiTangents?.Clear();
            BiTangents?.TrimExcess();
        }
    }


    public struct BatchedMeshGeometryConfig : IBatchedGeometry
    {
        public Geometry3D Geometry
        {
            private set; get;
        }
        public Matrix ModelTransform
        {
            private set; get;
        }
        public int MaterialIndex
        {
            private set; get;
        }
        public BatchedMeshGeometryConfig(Geometry3D geometry, Matrix modelTransform, int materialIndex)
        {
            Geometry = geometry;
            ModelTransform = modelTransform;
            MaterialIndex = materialIndex;
        }
    }
}
