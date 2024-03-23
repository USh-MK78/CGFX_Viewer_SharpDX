using HelixToolkit.Wpf.SharpDX.Model;
using HelixToolkit.Wpf.SharpDX.Model.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HelixToolkit.Wpf.SharpDX;

namespace CGFX_Viewer_SharpDX.MeshBuilderComponent.Mesh.GoemetryModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="CGFXGeometryModel3D" />
    public abstract class CGFXMaterialGeometryModel3D : CGFXGeometryModel3D
    {
        #region Dependency Properties
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty MaterialProperty =
            DependencyProperty.Register("Material", typeof(Material), typeof(CGFXMaterialGeometryModel3D), new PropertyMetadata(null, (d, e) =>
            {
                ((d as Element3DCore).SceneNode as Node.CGFXMaterialGeometryNode).Material = e.NewValue as Material;
            }));

        /// <summary>
        /// Specifiy if model material is transparent. 
        /// During rendering, transparent objects are rendered after opaque objects. Transparent objects' order in scene graph are preserved.
        /// </summary>
        public static readonly DependencyProperty IsTransparentProperty =
            DependencyProperty.Register("IsTransparent", typeof(bool), typeof(CGFXMaterialGeometryModel3D), new PropertyMetadata(false, (d, e) =>
            {
                ((d as Element3DCore).SceneNode as Node.CGFXMaterialGeometryNode).IsTransparent = (bool)e.NewValue;
            }));

        /// <summary>
        /// 
        /// </summary>
        public Material Material
        {
            get
            {
                return (Material)this.GetValue(MaterialProperty);
            }
            set
            {
                this.SetValue(MaterialProperty, value);
            }
        }

        /// <summary>
        /// Specifiy if model material is transparent. 
        /// During rendering, transparent objects are rendered after opaque objects. Transparent objects' order in scene graph are preserved.
        /// </summary>
        public bool IsTransparent
        {
            get
            {
                return (bool)GetValue(IsTransparentProperty);
            }
            set
            {
                SetValue(IsTransparentProperty, value);
            }
        }
        #endregion
        /// <summary>
        /// Assigns the default values to scene node.
        /// </summary>
        /// <param name="node">The node.</param>
        protected override void AssignDefaultValuesToSceneNode(SceneNode node)
        {
            if (node is Node.CGFXMaterialGeometryNode n)
            {
                n.Material = this.Material;
            }
            base.AssignDefaultValuesToSceneNode(node);
        }
    }
}
