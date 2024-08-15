using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Model;
using HelixToolkit.Wpf.SharpDX.Shaders;
using HelixToolkit.Wpf.SharpDX.Utilities;
using SharpDX.Direct3D11;
using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlTypes;
using System.CodeDom;

namespace CGFX_Viewer_SharpDX.Component.Material
{
    public class CGFXMaterial : HelixToolkit.Wpf.SharpDX.Material
    {

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TextureSlotListProperty = DependencyProperty.Register("TextureSlotList", typeof(Dictionary<int, Data.Texture.TextureModelSlot>), typeof(CGFXMaterial), new PropertyMetadata(new Dictionary<int, Data.Texture.TextureModelSlot>(), delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).TextureSlotList = (Dictionary<int, Data.Texture.TextureModelSlot>)e.NewValue;
        }));
        public Dictionary<int, Data.Texture.TextureModelSlot> TextureSlotList
        {
            get
            {
                return (Dictionary<int, Data.Texture.TextureModelSlot>)GetValue(TextureSlotListProperty);
            }
            set
            {
                SetValue(TextureSlotListProperty, value);
            }
        }

        //Component.Data.ColorProperty.Base
        public static readonly DependencyProperty BaseColorProperty = DependencyProperty.Register("BaseColor", typeof(Data.ColorProperty.Base), typeof(CGFXMaterial), new PropertyMetadata(Component.Data.ColorProperty.Base.Default(), delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).BaseColor = (Data.ColorProperty.Base)e.NewValue;
        }));
        public Component.Data.ColorProperty.Base BaseColor
        {
            get
            {
                return (Component.Data.ColorProperty.Base)GetValue(BaseColorProperty);
            }
            set
            {
                SetValue(BaseColorProperty, value);
            }
        }

        public static readonly DependencyProperty ConstantColorProperty = DependencyProperty.Register("ConstantColor", typeof(Component.Data.ColorProperty.ConstantColor), typeof(CGFXMaterial), new PropertyMetadata(Component.Data.ColorProperty.ConstantColor.Default(), delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).ConstantColor = (Component.Data.ColorProperty.ConstantColor)e.NewValue;
        }));
        public Component.Data.ColorProperty.ConstantColor ConstantColor
        {
            get
            {
                return (Component.Data.ColorProperty.ConstantColor)GetValue(ConstantColorProperty);
            }
            set
            {
                SetValue(ConstantColorProperty, value);
            }
        }

        public static readonly DependencyProperty BlendingProperty = DependencyProperty.Register("Blending", typeof(Component.Data.Blending), typeof(CGFXMaterial), new PropertyMetadata(new Component.Data.Blending(Data.Blending.BLENDMODE.NONE, null, null), delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).Blend = (Component.Data.Blending)e.NewValue;
        }));
        public Component.Data.Blending Blending
        {
            get
            {
                return (Component.Data.Blending)GetValue(BlendingProperty);
            }
            set
            {
                SetValue(BlendingProperty, value);
            }
        }

        public static readonly DependencyProperty TextureConbinerEquationListProperty = DependencyProperty.Register("TextureConbinerEquationList", typeof(List<Data.TextureConbinerStage.TextureConbinerEquation>), typeof(CGFXMaterial), new PropertyMetadata(new List<Data.TextureConbinerStage.TextureConbinerEquation>(), delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).TextureConbinerEquationList = (List<Data.TextureConbinerStage.TextureConbinerEquation>)e.NewValue;
        }));
        public List<Data.TextureConbinerStage.TextureConbinerEquation> TextureConbinerEquationList
        {
            get
            {
                return (List<Data.TextureConbinerStage.TextureConbinerEquation>)GetValue(TextureConbinerEquationListProperty);
            }
            set
            {
                SetValue(TextureConbinerEquationListProperty, value);
            }
        }


        public static readonly DependencyProperty EnableFlatShadingProperty = DependencyProperty.Register("EnableFlatShading", typeof(bool), typeof(CGFXMaterial), new PropertyMetadata(false, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).EnableFlatShading = (bool)e.NewValue;
        }));
        public bool EnableFlatShading
        {
            get
            {
                return (bool)GetValue(EnableFlatShadingProperty);
            }
            set
            {
                SetValue(EnableFlatShadingProperty, value);
            }
        }

        public static readonly DependencyProperty SpecularShininessProperty = DependencyProperty.Register("SpecularShininess", typeof(float), typeof(CGFXMaterial), new PropertyMetadata(30f, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).SpecularShininess = (float)e.NewValue;
        }));
        public float SpecularShininess
        {
            get
            {
                return (float)GetValue(SpecularShininessProperty);
            }
            set
            {
                SetValue(SpecularShininessProperty, value);
            }
        }

        public static readonly DependencyProperty DiffuseMapSamplerListProperty = DependencyProperty.Register("DiffuseMapSamplerList", typeof(List<SamplerStateDescription>), typeof(CGFXMaterial), new PropertyMetadata(new List<SamplerStateDescription>(), delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).DiffuseMapSamplerList = (List<SamplerStateDescription>)e.NewValue;
        }));
        public List<SamplerStateDescription> DiffuseMapSamplerList
        {
            get
            {
                return (List<SamplerStateDescription>)GetValue(DiffuseMapSamplerListProperty);
            }
            set
            {
                SetValue(DiffuseMapSamplerListProperty, value);
            }
        }

        public static readonly DependencyProperty DisplacementMapSamplerProperty = DependencyProperty.Register("DisplacementMapSampler", typeof(SamplerStateDescription), typeof(CGFXMaterial), new PropertyMetadata(DefaultSamplers.LinearSamplerWrapAni1, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).DisplacementMapSampler = (SamplerStateDescription)e.NewValue;
        }));
        public SamplerStateDescription DisplacementMapSampler
        {
            get
            {
                return (SamplerStateDescription)GetValue(DisplacementMapSamplerProperty);
            }
            set
            {
                SetValue(DisplacementMapSamplerProperty, value);
            }
        }

        //[TypeConverter(typeof(Vector4Converter))]
        //public Vector4 DisplacementMapScaleMask
        //{
        //    get
        //    {
        //        return (Vector4)GetValue(DisplacementMapScaleMaskProperty);
        //    }
        //    set
        //    {
        //        SetValue(DisplacementMapScaleMaskProperty, value);
        //    }
        //}


        //IsFragmentLighting
        public static readonly DependencyProperty IsFragmentLightingProperty = DependencyProperty.Register("IsFragmentLighting", typeof(bool), typeof(CGFXMaterial), new PropertyMetadata(false, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).IsFragmentLighting = (bool)e.NewValue;
        }));
        public bool IsFragmentLighting
        {
            get
            {
                return (bool)GetValue(IsFragmentLightingProperty);
            }
            set
            {
                SetValue(IsFragmentLightingProperty, value);
            }
        }

        //IsVertexLighting
        public static readonly DependencyProperty IsVertexLightingProperty = DependencyProperty.Register("IsVertexLighting", typeof(bool), typeof(CGFXMaterial), new PropertyMetadata(false, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).IsVertexLighting = (bool)e.NewValue;
        }));
        public bool IsVertexLighting
        {
            get
            {
                return (bool)GetValue(IsVertexLightingProperty);
            }
            set
            {
                SetValue(IsVertexLightingProperty, value);
            }
        }

        //IsHemisphereLighting
        public static readonly DependencyProperty IsHemisphereLightingProperty = DependencyProperty.Register("IsHemisphereLighting", typeof(bool), typeof(CGFXMaterial), new PropertyMetadata(false, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).IsHemisphereLighting = (bool)e.NewValue;
        }));
        public bool IsHemisphereLighting
        {
            get
            {
                return (bool)GetValue(IsHemisphereLightingProperty);
            }
            set
            {
                SetValue(IsHemisphereLightingProperty, value);
            }
        }

        /// <summary>
        /// The render shadow map property
        /// </summary>
        public static readonly DependencyProperty RenderShadowMapProperty = DependencyProperty.Register("RenderShadowMap", typeof(bool), typeof(CGFXMaterial), new PropertyMetadata(false, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).RenderShadowMap = (bool)e.NewValue;
        }));
        /// <summary>
        /// Gets or sets a value indicating whether [render shadow map]. Default is false
        /// </summary>
        public bool RenderShadowMap
        {
            get
            {
                return (bool)GetValue(RenderShadowMapProperty);
            }
            set
            {
                SetValue(RenderShadowMapProperty, value);
            }
        }

        //public bool RenderEmissiveMap
        //{
        //    get
        //    {
        //        return (bool)GetValue(RenderEmissiveMapProperty);
        //    }
        //    set
        //    {
        //        SetValue(RenderEmissiveMapProperty, value);
        //    }
        //}



        /// <summary>
        /// The enable automatic tangent
        /// </summary>
        public static readonly DependencyProperty EnableAutoTangentProperty = DependencyProperty.Register("EnableAutoTangent", typeof(bool), typeof(CGFXMaterial), new PropertyMetadata(false, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).EnableAutoTangent = (bool)e.NewValue;
        }));
        /// <summary>
        /// Gets or sets a value indicating whether [enable automatic tangent]
        /// </summary>
        public bool EnableAutoTangent
        {
            get
            {
                return (bool)GetValue(EnableAutoTangentProperty);
            }
            set
            {
                SetValue(EnableAutoTangentProperty, value);
            }
        }

        /// <summary>
        /// The enable tessellation property
        /// </summary>
        public static readonly DependencyProperty EnableTessellationProperty = DependencyProperty.Register("EnableTessellation", typeof(bool), typeof(CGFXMaterial), new PropertyMetadata(false, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).EnableTessellation = (bool)e.NewValue;
        }));
        /// <summary>
        ///  Gets or sets a value indicating whether [enable tessellation],
        /// </summary>
        /// <value>true if [enable tessellation]; otherwise, false.</value>
        public bool EnableTessellation
        {
            get
            {
                return (bool)GetValue(EnableTessellationProperty);
            }
            set
            {
                SetValue(EnableTessellationProperty, value);
            }
        }


        /// <summary>
        /// The tessellation factor at HelixToolkit.Wpf.SharpDX.CGFXMaterial.MaxTessellationDistance property
        /// </summary>
        public static readonly DependencyProperty MaxDistanceTessellationFactorProperty = DependencyProperty.Register("MaxDistanceTessellationFactor", typeof(double), typeof(CGFXMaterial), new PropertyMetadata(1.0, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).MaxDistanceTessellationFactor = (float)(double)e.NewValue;
        }));

        /// <summary>
        /// Gets or sets the tessellation factor at HelixToolkit.Wpf.SharpDX.CGFXMaterial.MaxTessellationDistance.
        /// </summary>
        /// <value>The maximum tessellation factor</value>
        public double MaxDistanceTessellationFactor
        {
            get
            {
                return (double)GetValue(MaxDistanceTessellationFactorProperty);
            }
            set
            {
                SetValue(MaxDistanceTessellationFactorProperty, value);
            }
        }

        //
        // 概要:
        //     The tessellation factor at HelixToolkit.Wpf.SharpDX.CGFXMaterial.MinTessellationDistance property
        public static readonly DependencyProperty MinDistanceTessellationFactorProperty = DependencyProperty.Register("MinDistanceTessellationFactor", typeof(double), typeof(CGFXMaterial), new PropertyMetadata(2.0, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).MinDistanceTessellationFactor = (float)(double)e.NewValue;
        }));
        //
        // 概要:
        //     Gets or sets the tessellation factor at HelixToolkit.Wpf.SharpDX.CGFXMaterial.MinTessellationDistance
        //
        // 値:
        //     The minimum tessellation factor.
        public double MinDistanceTessellationFactor
        {
            get
            {
                return (double)GetValue(MinDistanceTessellationFactorProperty);
            }
            set
            {
                SetValue(MinDistanceTessellationFactorProperty, value);
            }
        }


        //
        // 概要:
        //     The maximum tessellation distance property
        public static readonly DependencyProperty MaxTessellationDistanceProperty = DependencyProperty.Register("MaxTessellationDistance", typeof(double), typeof(CGFXMaterial), new PropertyMetadata(50.0, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).MaxTessellationDistance = (float)(double)e.NewValue;
        }));
        //
        // 概要:
        //     Gets or sets the maximum tessellation distance.
        //
        // 値:
        //     The maximum tessellation distance.
        public double MaxTessellationDistance
        {
            get
            {
                return (double)GetValue(MaxTessellationDistanceProperty);
            }
            set
            {
                SetValue(MaxTessellationDistanceProperty, value);
            }
        }



        //
        // 概要:
        //     The minimum tessellation distance property
        public static readonly DependencyProperty MinTessellationDistanceProperty = DependencyProperty.Register("MinTessellationDistance", typeof(double), typeof(CGFXMaterial), new PropertyMetadata(1.0, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).MinTessellationDistance = (float)(double)e.NewValue;
        }));
        //
        // 概要:
        //     Gets or sets the minimum tessellation distance.
        //
        // 値:
        //     The minimum tessellation distance.
        public double MinTessellationDistance
        {
            get
            {
                return (double)GetValue(MinTessellationDistanceProperty);
            }
            set
            {
                SetValue(MinTessellationDistanceProperty, value);
            }
        }


        /// <summary>
        /// The uv transform property
        /// </summary>
        public static readonly DependencyProperty UVTransformProperty = DependencyProperty.Register("UVTransform", typeof(List<UVTransform>), typeof(CGFXMaterial), new PropertyMetadata(new List<UVTransform>(), delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).UVTransform = (List<UVTransform>)e.NewValue;
        }));
        /// <summary>
        /// Gets or sets the texture uv transform.
        /// </summary>
        /// <value>The uv transform.</value>
        public List<UVTransform> UVTransform
        {
            get
            {
                return (List<UVTransform>)GetValue(UVTransformProperty);
            }
            set
            {
                SetValue(UVTransformProperty, value);
            }
        }


        public static readonly DependencyProperty VertexColorBlendingFactorProperty = DependencyProperty.Register("VertexColorBlendingFactor", typeof(double), typeof(CGFXMaterial), new PropertyMetadata(0.0, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as CGFXMaterial).Core as CGFXMaterialCore).VertexColorBlendingFactor = (float)(double)e.NewValue;
        }));
        /// <summary>
        /// Gets or sets the vertex color blending factor. Final Diffuse Color = (1 - VertexColorBlendingFactor) * Diffuse + VertexColorBlendingFactor * Vertex Color
        /// </summary>
        public double VertexColorBlendingFactor
        {
            get
            {
                return (double)GetValue(VertexColorBlendingFactorProperty);
            }
            set
            {
                SetValue(VertexColorBlendingFactorProperty, value);
            }
        }



        public CGFXMaterial()
        {
        }

        /// <summary>
        /// Initializes a new instance of the CGFXMaterial class.
        /// </summary>
        /// <param name="core"></param>
        public CGFXMaterial(CGFXMaterialCore core) : base(core)
        {
            BaseColor = core.BaseColor;
            ConstantColor = core.ConstantColor;
            Blending = core.Blend;
            TextureConbinerEquationList = core.TextureConbinerEquationList;

            //AmbientColor = core.AmbientColor;
            //DiffuseColor = core.DiffuseColor;
            //DisplacementMap = core.DisplacementMap;
            //EmissiveColor = core.EmissiveColor;
            base.Name = core.Name;
            //NormalMap = core.NormalMap;
            //ReflectiveColor = core.ReflectiveColor;
            //SpecularColor = core.SpecularColor;
            SpecularShininess = core.SpecularShininess;

            //DisplacementMapScaleMask = core.DisplacementMapScaleMask;
            DiffuseMapSamplerList = core.DiffuseMapSamplerList;
            DisplacementMapSampler = core.DisplacementMapSampler;
            MaxTessellationDistance = core.MaxTessellationDistance;
            MinTessellationDistance = core.MinTessellationDistance;
            MaxDistanceTessellationFactor = core.MaxDistanceTessellationFactor;
            MinDistanceTessellationFactor = core.MinDistanceTessellationFactor;
            EnableTessellation = core.EnableTessellation;

            IsFragmentLighting = core.IsFragmentLighting;
            IsVertexLighting = core.IsVertexLighting;
            IsHemisphereLighting = core.IsHemisphereLighting;

            RenderShadowMap = core.RenderShadowMap;

            EnableAutoTangent = core.EnableAutoTangent;
            UVTransform = core.UVTransform;
            EnableFlatShading = core.EnableFlatShading;
            VertexColorBlendingFactor = core.VertexColorBlendingFactor;

            TextureSlotList = core.TextureSlotList;
        }

        public virtual CGFXMaterial CloneMaterial()
        {
            return new CGFXMaterial
            {
                BaseColor = BaseColor,
                ConstantColor = ConstantColor,
                Blending = Blending,
                TextureConbinerEquationList = TextureConbinerEquationList,

                Name = base.Name,
                //NormalMap = NormalMap,
                //ReflectiveColor = ReflectiveColor,
                //SpecularColor = SpecularColor,
                SpecularShininess = SpecularShininess,

                //DisplacementMapScaleMask = DisplacementMapScaleMask,
                DiffuseMapSamplerList = DiffuseMapSamplerList,
                DisplacementMapSampler = DisplacementMapSampler,
                MaxTessellationDistance = (float)MaxTessellationDistance,
                MinTessellationDistance = (float)MinTessellationDistance,
                MaxDistanceTessellationFactor = (float)MaxDistanceTessellationFactor,
                MinDistanceTessellationFactor = (float)MinDistanceTessellationFactor,
                EnableTessellation = EnableTessellation,

                IsFragmentLighting = IsFragmentLighting,
                IsVertexLighting = IsVertexLighting,
                IsHemisphereLighting = IsHemisphereLighting,

                RenderShadowMap = RenderShadowMap,
                //RenderSpecularColorMap = RenderSpecularColorMap,
                //RenderEmissiveMap = RenderEmissiveMap,
                EnableAutoTangent = EnableAutoTangent,
                UVTransform = UVTransform,
                EnableFlatShading = EnableFlatShading,
                VertexColorBlendingFactor = VertexColorBlendingFactor,

                TextureSlotList = TextureSlotList
            };
        }

        protected override Freezable CreateInstanceCore()
        {
            return CloneMaterial();
        }

        protected override MaterialCore OnCreateCore()
        {
            return new CGFXMaterialCore
            {
                BaseColor = BaseColor,
                ConstantColor = ConstantColor,
                Blend = Blending,
                TextureConbinerEquationList = TextureConbinerEquationList,

                Name = base.Name,
                //NormalMap = NormalMap,
                //ReflectiveColor = ReflectiveColor,
                //SpecularColor = SpecularColor,
                SpecularShininess = SpecularShininess,
                //DiffuseMap = DiffuseMap,
                //DiffuseAlphaMap = DiffuseAlphaMap,
                //SpecularColorMap = SpecularColorMap,
                //EmissiveMap = EmissiveMap,
                //DisplacementMapScaleMask = DisplacementMapScaleMask,
                DiffuseMapSamplerList = DiffuseMapSamplerList,
                DisplacementMapSampler = DisplacementMapSampler,
                MaxTessellationDistance = (float)MaxTessellationDistance,
                MinTessellationDistance = (float)MinTessellationDistance,
                MaxDistanceTessellationFactor = (float)MaxDistanceTessellationFactor,
                MinDistanceTessellationFactor = (float)MinDistanceTessellationFactor,
                EnableTessellation = EnableTessellation,

                IsFragmentLighting = IsFragmentLighting,
                IsVertexLighting = IsVertexLighting,
                IsHemisphereLighting = IsHemisphereLighting,

                RenderShadowMap = RenderShadowMap,

                EnableAutoTangent = EnableAutoTangent,
                UVTransform = UVTransform,
                EnableFlatShading = EnableFlatShading,
                VertexColorBlendingFactor = (float)VertexColorBlendingFactor,

                TextureSlotList = TextureSlotList
            };
        }
    }
}

