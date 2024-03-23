using HelixToolkit.Wpf.SharpDX.Model;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CGFX_Viewer_SharpDX.MeshBuilderComponent.Mesh.MeshGeometry
{
    //public abstract class CGFXGeometry3D : Geometry3D
    //{
    //    #region PropertyChangedEventArgs
    //    private static readonly PropertyChangedEventArgs vertexBufferPropChanged = new PropertyChangedEventArgs(VertexBuffer);
    //    private static readonly PropertyChangedEventArgs triangleBufferPropChanged = new PropertyChangedEventArgs(TriangleBuffer);
    //    private static readonly PropertyChangedEventArgs colorsPropChanged = new PropertyChangedEventArgs(nameof(Colors));
    //    private static readonly PropertyChangedEventArgs positionPropChanged = new PropertyChangedEventArgs(nameof(Positions));
    //    private static readonly PropertyChangedEventArgs indicesPropChanged = new PropertyChangedEventArgs(nameof(Indices));
    //    #endregion

    //    public new Guid GUID { set; get; } = Guid.NewGuid();

    //    /// <summary>
    //    /// Indices, can be triangle list, line list, etc.
    //    /// </summary>
    //    public IntCollection indices = null;
    //    public new IntCollection Indices
    //    {
    //        get
    //        {
    //            return indices;
    //        }
    //        set
    //        {
    //            if (Set(ref indices, value, false))
    //            {
    //                ClearOctree();
    //                RaisePropertyChanged(indicesPropChanged);
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Vertex Positions
    //    /// </summary>
    //    private Vector3Collection position = null;
    //    public new Vector3Collection Positions
    //    {
    //        get
    //        {
    //            return position;
    //        }
    //        set
    //        {
    //            if (Set(ref position, value, false))
    //            {
    //                ClearOctree();
    //                RaisePropertyChanged(positionPropChanged);
    //                UpdateBounds();
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Colors
    //    /// </summary>
    //    private Color4Collection colors = null;
    //    public new Color4Collection Colors
    //    {
    //        get
    //        {
    //            return colors;
    //        }
    //        set
    //        {
    //            if (Set(ref colors, value, false))
    //            {
    //                RaisePropertyChanged(colorsPropChanged);
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Gets or sets a value indicating whether [octree dirty], needs update.
    //    /// </summary>
    //    /// <value>
    //    ///   <c>true</c> if [octree dirty]; otherwise, <c>false</c>.
    //    /// </value>
    //    public new bool OctreeDirty { get; private set; } = true;

    //    /// <summary>
    //    /// Gets a value indicating whether the geometry data are transient. Call <see cref="SetAsTransient"/> to set this flag to true.
    //    /// <para>  When this is true, geometry3D data will be cleared once being loaded into GPU.  </para>
    //    /// <para> This geometry3D can only be used by one Model3D in one Viewport. Must not be shared. Hit test is disabled as well. </para>
    //    /// <para> Useful when loading a large geometry for view only and free up memory after geometry data being uploaded to GPU. </para>
    //    /// </summary>
    //    /// <value>
    //    ///   <c>true</c> if this instance is transient; otherwise, <c>false</c>.
    //    /// </value>
    //    public new bool IsTransient { private set; get; } = false;

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="CGFXGeometry3D"/> class.
    //    /// </summary>
    //    public CGFXGeometry3D()
    //    {
    //        OctreeParameter.PropertyChanged += this.OctreeParameter_PropertyChanged;
    //    }

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="CGFXGeometry3D"/> class.
    //    /// </summary>
    //    /// <param name="isDynamic">if set to <c>true</c> [is dynamic].</param>
    //    public CGFXGeometry3D(bool isDynamic)
    //        : this()
    //    {
    //        IsDynamic = isDynamic;
    //    }

    //    private void OctreeParameter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //    {
    //        this.OctreeDirty = true;
    //    }

    //    /// <summary>
    //    /// Call to manually update vertex buffer. Use with <see cref="ObservableObject.DisablePropertyChangedEvent"/>
    //    /// <para>This is useful if user want to reuse existing <see cref="Positions"/> list and update vertex value inside the list.</para>
    //    /// <para>Note: For performance purpose, this will not cause bounding box update. 
    //    /// User must manually call <see cref="UpdateBounds"/> to refresh geometry bounding box.</para>
    //    /// </summary>
    //    public new void UpdateVertices()
    //    {
    //        RaisePropertyChanged(vertexBufferPropChanged);
    //    }
    //    /// <summary>
    //    /// Call to manually update triangle buffer.
    //    /// <para>This is useful if user want to reuse existing <see cref="Indices"/> object and update index value inside the list.</para>
    //    /// </summary>
    //    public new void UpdateTriangles()
    //    {
    //        RaisePropertyChanged(triangleBufferPropChanged);
    //    }

    //    /// <summary>
    //    /// Call to manually update vertex color buffer.
    //    /// <para>This is useful if user want to reuse existing <see cref="Colors"/> object and update color value inside the list.</para>
    //    /// <para>Make sure the <see cref="Colors"/> count equal to the <see cref="Positions"/> count</para>
    //    /// </summary>
    //    public new void UpdateColors()
    //    {
    //        RaisePropertyChanged(colorsPropChanged);
    //    }

    //    /// <summary>
    //    /// Assigns internal properties to another geometry3D. This does not assign <see cref="IsDynamic"/>/<see cref="PreDefinedIndexCount"/>/<see cref="PreDefinedVertexCount"/>
    //    /// <para>
    //    /// Following properties are assigned:
    //    /// <see cref="Positions"/>, <see cref="Indices"/>, <see cref="VertexColors"/>, <see cref="Bound"/>, <see cref="BoundingSphere"/>, <see cref="Octree"/>, <see cref="OctreeParameter"/>
    //    /// </para>
    //    /// <para>Override <see cref="OnAssignTo(CGFXGeometry3D)"/> to assign custom properties in child class</para>
    //    /// </summary>
    //    /// <param name="target">The target.</param>
    //    public void AssignTo(CGFXGeometry3D target)
    //    {
    //        target.DisableUpdateBound = true;
    //        target.Positions = this.Positions;
    //        target.ClearOctree();
    //        target.DisableUpdateBound = false;
    //        target.Indices = this.Indices;
    //        target.Colors = this.Colors;
    //        target.Bound = this.Bound;
    //        target.BoundingSphere = this.BoundingSphere;
    //        target.OctreeParameter.MinimumOctantSize = OctreeParameter.MinimumOctantSize;
    //        target.OctreeParameter.MinObjectSizeToSplit = OctreeParameter.MinObjectSizeToSplit;
    //        target.OctreeParameter.Cubify = OctreeParameter.Cubify;
    //        target.OctreeParameter.EnableParallelBuild = OctreeParameter.EnableParallelBuild;
    //        if (Octree != null)
    //        {
    //            target.ManualSetOctree(Octree);
    //        }
    //        OnAssignTo(target);
    //    }

    //    protected virtual void OnAssignTo(CGFXGeometry3D target)
    //    {

    //    }

    //    /// <summary>
    //    /// Manually call this function to update AABB and Bounding Sphere
    //    /// </summary>
    //    public virtual void UpdateBounds(int PositionBufIndex)
    //    {
    //        if (DisableUpdateBound) return;
    //        else if (position == null || position.Count == 0)
    //        {
    //            Bound = new BoundingBox();
    //            BoundingSphere = new BoundingSphere();
    //        }
    //        else
    //        {
    //            Bound = BoundingBoxExtensions.FromPoints(Positions);
    //            BoundingSphere = BoundingSphereExtensions.FromPoints(Positions);
    //        }
    //        if (Bound.Maximum.IsUndefined() || Bound.Minimum.IsUndefined() || BoundingSphere.Center.IsUndefined() || float.IsInfinity(Bound.Center.X) || float.IsInfinity(Bound.Center.Y) || float.IsInfinity(Bound.Center.Z))
    //        {
    //            throw new Exception("Position vertex contains invalid value(Example: Float.NaN, Float.Infinity).");
    //        }
    //    }

    //    /// <summary>
    //    /// Clears all geometry data.
    //    /// </summary>
    //    public new void ClearAllGeometryData()
    //    {
    //        Positions?.Clear();
    //        Positions?.TrimExcess();
    //        Indices?.Clear();
    //        Indices?.TrimExcess();
    //        Colors?.Clear();
    //        Colors?.TrimExcess();
    //        OnClearAllGeometryData();
    //    }
    //}
}
