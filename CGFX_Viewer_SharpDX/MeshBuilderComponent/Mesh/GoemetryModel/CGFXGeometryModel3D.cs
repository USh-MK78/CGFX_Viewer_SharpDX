﻿using HelixToolkit.Wpf.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX.Model.Scene;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HelixToolkit.Wpf.SharpDX.Model;
using SharpDX.Direct3D11;

namespace CGFX_Viewer_SharpDX.MeshBuilderComponent.Mesh.GoemetryModel
{
    /// <summary>
    /// Provides a base class for a scene model which contains geometry
    /// </summary>
    public abstract class CGFXGeometryModel3D : Element3D, IHitable, IThrowingShadow, IApplyPostEffect
    {
        #region DependencyProperties        
        /// <summary>
        /// The geometry property
        /// </summary>
        public static readonly DependencyProperty GeometryProperty =
            DependencyProperty.Register("Geometry", typeof(Geometry3D), typeof(CGFXGeometryModel3D), new PropertyMetadata(null,
                (d, e) =>
                {
                    ((d as Element3DCore).SceneNode as Node.CGFXGeometryNode).Geometry = e.NewValue as Geometry3D;
                }));
        public static readonly DependencyProperty IsThrowingShadowProperty =
                DependencyProperty.Register("IsThrowingShadow", typeof(bool), typeof(CGFXGeometryModel3D), new PropertyMetadata(false, (d, e) =>
                {
                    if ((d as Element3D).SceneNode is IThrowingShadow t)
                    {
                        t.IsThrowingShadow = (bool)e.NewValue;
                    }
                }));

        /// <summary>
        /// The depth bias property
        /// </summary>
        public static readonly DependencyProperty DepthBiasProperty =
            DependencyProperty.Register("DepthBias", typeof(int), typeof(CGFXGeometryModel3D), new PropertyMetadata(0, (d, e) =>
            {
                ((d as Element3DCore).SceneNode as Node.CGFXGeometryNode).DepthBias = (int)e.NewValue;
            }));
        /// <summary>
        /// The slope scaled depth bias property
        /// </summary>
        public static readonly DependencyProperty SlopeScaledDepthBiasProperty =
            DependencyProperty.Register("SlopeScaledDepthBias", typeof(double), typeof(CGFXGeometryModel3D), new PropertyMetadata(0.0, (d, e) =>
            {
                ((d as Element3DCore).SceneNode as Node.CGFXGeometryNode).SlopeScaledDepthBias = (float)(double)e.NewValue;
            }));
        /// <summary>
        /// The is selected property
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(CGFXGeometryModel3D), new PropertyMetadata(false));
        /// <summary>
        /// The is multisample enabled property
        /// </summary>
        public static readonly DependencyProperty IsMultisampleEnabledProperty =
            DependencyProperty.Register("IsMultisampleEnabled", typeof(bool), typeof(CGFXGeometryModel3D), new PropertyMetadata(true, (d, e) =>
            {
                ((d as Element3DCore).SceneNode as Node.CGFXGeometryNode).IsMSAAEnabled = (bool)e.NewValue;
            }));
        /// <summary>
        /// The fill mode property
        /// </summary>
        public static readonly DependencyProperty FillModeProperty = DependencyProperty.Register("FillMode", typeof(FillMode), typeof(CGFXGeometryModel3D),
            new PropertyMetadata(FillMode.Solid, (d, e) =>
            {
                ((d as Element3DCore).SceneNode as Node.CGFXGeometryNode).FillMode = (FillMode)e.NewValue;
            }));
        /// <summary>
        /// The is scissor enabled property
        /// </summary>
        public static readonly DependencyProperty IsScissorEnabledProperty =
            DependencyProperty.Register("IsScissorEnabled", typeof(bool), typeof(CGFXGeometryModel3D), new PropertyMetadata(true, (d, e) =>
            {
                ((d as Element3DCore).SceneNode as Node.CGFXGeometryNode).IsScissorEnabled = (bool)e.NewValue;
            }));
        /// <summary>
        /// The enable view frustum check property
        /// </summary>
        public static readonly DependencyProperty EnableViewFrustumCheckProperty =
            DependencyProperty.Register("EnableViewFrustumCheck", typeof(bool), typeof(CGFXGeometryModel3D), new PropertyMetadata(true,
                (d, e) =>
                {
                    ((d as Element3DCore).SceneNode as Node.CGFXGeometryNode).EnableViewFrustumCheck = (bool)e.NewValue;
                }));
        /// <summary>
        /// The is depth clip enabled property
        /// </summary>
        public static readonly DependencyProperty IsDepthClipEnabledProperty = DependencyProperty.Register("IsDepthClipEnabled", typeof(bool), typeof(CGFXGeometryModel3D),
            new PropertyMetadata(true, (d, e) =>
            {
                ((d as Element3DCore).SceneNode as Node.CGFXGeometryNode).IsDepthClipEnabled = (bool)e.NewValue;
            }));


        /// <summary>
        /// The post effects property
        /// </summary>
        public static readonly DependencyProperty PostEffectsProperty =
            DependencyProperty.Register("PostEffects", typeof(string), typeof(CGFXGeometryModel3D), new PropertyMetadata(string.Empty, (d, e) =>
            {
                ((d as Element3DCore).SceneNode as Node.CGFXGeometryNode).PostEffects = e.NewValue as string;
            }));

        /// <summary>
        /// The always hittable property
        /// </summary>
        public static readonly DependencyProperty AlwaysHittableProperty =
            DependencyProperty.Register("AlwaysHittable", typeof(bool), typeof(CGFXGeometryModel3D), new PropertyMetadata(false, (d, e) =>
            {
                ((d as Element3DCore).SceneNode as Node.CGFXGeometryNode).AlwaysHittable = (bool)e.NewValue;
            }));



        /// <summary>
        /// Gets or sets the geometry.
        /// </summary>
        /// <value>
        /// The geometry.
        /// </value>
        public Geometry3D Geometry
        {
            get
            {
                return (Geometry3D)this.GetValue(GeometryProperty);
            }
            set
            {
                this.SetValue(GeometryProperty, value);
            }
        }
        /// <summary>
        /// <see cref="IThrowingShadow.IsThrowingShadow"/>
        /// </summary>
        public bool IsThrowingShadow
        {
            set
            {
                SetValue(IsThrowingShadowProperty, value);
            }
            get
            {
                return (bool)GetValue(IsThrowingShadowProperty);
            }
        }
        /// <summary>
        /// List of instance matrix.
        /// </summary>
        public static readonly DependencyProperty InstancesProperty =
            DependencyProperty.Register("Instances", typeof(IList<Matrix>), typeof(CGFXGeometryModel3D), new PropertyMetadata(null, (d, e) =>
            {
                ((d as Element3DCore).SceneNode as Node.CGFXGeometryNode).Instances = e.NewValue as IList<Matrix>;
            }));

        /// <summary>
        /// List of instance matrix. 
        /// </summary>
        public IList<Matrix> Instances
        {
            get
            {
                return (IList<Matrix>)this.GetValue(InstancesProperty);
            }
            set
            {
                this.SetValue(InstancesProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the depth bias.
        /// </summary>
        /// <value>
        /// The depth bias.
        /// </value>
        public int DepthBias
        {
            get
            {
                return (int)this.GetValue(DepthBiasProperty);
            }
            set
            {
                this.SetValue(DepthBiasProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets the slope scaled depth bias.
        /// </summary>
        /// <value>
        /// The slope scaled depth bias.
        /// </value>
        public double SlopeScaledDepthBias
        {
            get
            {
                return (double)this.GetValue(SlopeScaledDepthBiasProperty);
            }
            set
            {
                this.SetValue(SlopeScaledDepthBiasProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected
        {
            get
            {
                return (bool)this.GetValue(IsSelectedProperty);
            }
            set
            {
                this.SetValue(IsSelectedProperty, value);
            }
        }

        /// <summary>
        /// Only works under FillMode = Wireframe. MSAA is determined by viewport MSAA settings for FillMode = Solid
        /// </summary>
        public bool IsMultisampleEnabled
        {
            set
            {
                SetValue(IsMultisampleEnabledProperty, value);
            }
            get
            {
                return (bool)GetValue(IsMultisampleEnabledProperty);
            }
        }
        /// <summary>
        /// Gets or sets the fill mode.
        /// </summary>
        /// <value>
        /// The fill mode.
        /// </value>
        public FillMode FillMode
        {
            set
            {
                SetValue(FillModeProperty, value);
            }
            get
            {
                return (FillMode)GetValue(FillModeProperty);
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is scissor enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is scissor enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsScissorEnabled
        {
            set
            {
                SetValue(IsScissorEnabledProperty, value);
            }
            get
            {
                return (bool)GetValue(IsScissorEnabledProperty);
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is depth clip enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is depth clip enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsDepthClipEnabled
        {
            set
            {
                SetValue(IsDepthClipEnabledProperty, value);
            }
            get
            {
                return (bool)GetValue(IsDepthClipEnabledProperty);
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [enable view frustum check].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable view frustum check]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableViewFrustumCheck
        {
            set
            {
                SetValue(EnableViewFrustumCheckProperty, value);
            }
            get
            {
                return (bool)GetValue(EnableViewFrustumCheckProperty);
            }
        }

        public string PostEffects
        {
            get
            {
                return (string)GetValue(PostEffectsProperty);
            }
            set
            {
                SetValue(PostEffectsProperty, value);
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [always hittable] even it is not rendering.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [always hittable]; otherwise, <c>false</c>.
        /// </value>
        public bool AlwaysHittable
        {
            get
            {
                return (bool)GetValue(AlwaysHittableProperty);
            }
            set
            {
                SetValue(AlwaysHittableProperty, value);
            }
        }
        #endregion

        protected override void AssignDefaultValuesToSceneNode(SceneNode node)
        {
            if (node is Node.CGFXGeometryNode n)
            {
                n.DepthBias = this.DepthBias;
                n.IsDepthClipEnabled = this.IsDepthClipEnabled;
                n.SlopeScaledDepthBias = (float)this.SlopeScaledDepthBias;
                n.IsMSAAEnabled = this.IsMultisampleEnabled;
                n.FillMode = this.FillMode;
                n.IsScissorEnabled = this.IsScissorEnabled;
                n.EnableViewFrustumCheck = this.EnableViewFrustumCheck;
                n.PostEffects = this.PostEffects;
                n.AlwaysHittable = this.AlwaysHittable;
            }
            base.AssignDefaultValuesToSceneNode(node);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public sealed class BoundChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The new bound
        /// </summary>
        public readonly BoundingBox NewBound;
        /// <summary>
        /// The old bound
        /// </summary>
        public readonly BoundingBox OldBound;
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundChangedEventArgs"/> class.
        /// </summary>
        /// <param name="newBound">The new bound.</param>
        /// <param name="oldBound">The old bound.</param>
        public BoundChangedEventArgs(BoundingBox newBound, BoundingBox oldBound)
        {
            NewBound = newBound;
            OldBound = oldBound;
        }
    }
}
