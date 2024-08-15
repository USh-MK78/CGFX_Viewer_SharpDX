using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX.Model;
using HelixToolkit.Wpf.SharpDX.Shaders;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFX_Viewer_SharpDX.Component.Material
{
    public class CGFXMaterialCore : MaterialCore
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, Data.Texture.TextureModelSlot> textureSlotList;
        public Dictionary<int, Data.Texture.TextureModelSlot> TextureSlotList
        {
            get
            {
                return textureSlotList;
            }
            set
            {
                Set(ref textureSlotList, value, "TextureSlotList");
            }
        }

        private Component.Data.ColorProperty.Base baseColor = Data.ColorProperty.Base.Default();
        public Component.Data.ColorProperty.Base BaseColor
        {
            get
            {
                return baseColor;
            }
            set
            {
                Set(ref baseColor, value, "BaseColor");
            }
        }

        private Component.Data.ColorProperty.ConstantColor constantColor = new Data.ColorProperty.ConstantColor();
        public Component.Data.ColorProperty.ConstantColor ConstantColor
        {
            get
            {
                return constantColor;
            }
            set
            {
                Set(ref constantColor, value, "ConstantColor");
            }
        }

        //private Component.Data.Blending.BlendEquation blendEquation = new Data.Blending.BlendEquation();
        //public Component.Data.Blending.BlendEquation BlendEquation
        //{
        //    get
        //    {
        //        return blendEquation;
        //    }
        //    set
        //    {
        //        Set(ref blendEquation, value, "BlendEquation");
        //    }
        //}

        //private Component.Data.Blending.BlendEquation blendEquation = new Data.Blending.BlendEquation();
        //public Component.Data.Blending.BlendEquation BlendEquation
        //{
        //    get
        //    {
        //        return blendEquation;
        //    }
        //    set
        //    {
        //        Set(ref blendEquation, value, "BlendEquation");
        //    }
        //}

        private Component.Data.Blending blend = new Data.Blending(Data.Blending.BLENDMODE.NONE, null, null);
        public Component.Data.Blending Blend
        {
            get
            {
                return blend;
            }
            set
            {
                Set(ref blend, value, "Blend");
            }
        }


        private List<Component.Data.TextureConbinerStage.TextureConbinerEquation> textureConbinerEquationList = new List<Data.TextureConbinerStage.TextureConbinerEquation>();
        public List<Component.Data.TextureConbinerStage.TextureConbinerEquation> TextureConbinerEquationList
        {
            get
            {
                return textureConbinerEquationList;
            }
            set
            {
                Set(ref textureConbinerEquationList, value, "TextureConbinerEquationList");
            }
        }

        private float specularShininess = 1f;
        public float SpecularShininess
        {
            get
            {
                return specularShininess;
            }
            set
            {
                Set(ref specularShininess, value, "SpecularShininess");
            }
        }

        //private TextureModel diffuseMap;
        //public TextureModel DiffuseMap
        //{
        //    get
        //    {
        //        return diffuseMap;
        //    }
        //    set
        //    {
        //        Set(ref diffuseMap, value, "DiffuseMap");
        //    }
        //}

        //public string DiffuseMapFilePath { get; set; }

        //private TextureModel diffuseAlphaMap;
        //public TextureModel DiffuseAlphaMap
        //{
        //    get
        //    {
        //        return diffuseAlphaMap;
        //    }
        //    set
        //    {
        //        Set(ref diffuseAlphaMap, value, "DiffuseAlphaMap");
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the diffuse alpha map file path. For export only
        ///// </summary>
        //public string DiffuseAlphaMapFilePath { get; set; }

        ///// <summary>
        ///// Gets or sets the NormalMap
        ///// </summary>
        //private TextureModel normalMap;
        //public TextureModel NormalMap
        //{
        //    get
        //    {
        //        return normalMap;
        //    }
        //    set
        //    {
        //        Set(ref normalMap, value, "NormalMap");
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the normal map file path. For export only
        ///// </summary>
        //public string NormalMapFilePath { get; set; }

        ///// <summary>
        ///// Gets or sets the specular color map
        ///// </summary>
        //private TextureModel specularColorMap;
        //public TextureModel SpecularColorMap
        //{
        //    get
        //    {
        //        return specularColorMap;
        //    }
        //    set
        //    {
        //        Set(ref specularColorMap, value, "SpecularColorMap");
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the specular color map file path. For export only
        ///// </summary>
        //public string SpecularColorMapFilePath { get; set; }

        ///// <summary>
        ///// Gets or sets the DisplacementMap
        ///// </summary>
        //private TextureModel displacementMap;
        //public TextureModel DisplacementMap
        //{
        //    get
        //    {
        //        return displacementMap;
        //    }
        //    set
        //    {
        //        Set(ref displacementMap, value, "DisplacementMap");
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the displacement file path. For export only
        ///// </summary>
        //public string DisplacementMapFilePath { get; set; }

        ///// <summary>
        ///// Gets or sets the emissive map
        ///// </summary>
        //private TextureModel emissiveMap;
        //public TextureModel EmissiveMap
        //{
        //    get
        //    {
        //        return emissiveMap;
        //    }
        //    set
        //    {
        //        Set(ref emissiveMap, value, "EmissiveMap");
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the emissive map file path. For export only
        ///// </summary>
        //public string EmissiveMapFilePath { get; set; }

        ///// <summary>
        ///// Gets or sets the DisplacementMapScaleMask
        ///// </summary>
        //private Vector4 displacementMapScaleMask;
        //public Vector4 DisplacementMapScaleMask
        //{
        //    get
        //    {
        //        return displacementMapScaleMask;
        //    }
        //    set
        //    {
        //        Set(ref displacementMapScaleMask, value, "DisplacementMapScaleMask");
        //    }
        //}

        /// <summary>
        /// Gets or sets the uv transform
        /// </summary>
        private List<UVTransform> uvTransform = new List<UVTransform>();
        public List<UVTransform> UVTransform
        {
            get
            {
                return uvTransform;
            }
            set
            {
                Set(ref uvTransform, value, "UVTransform");
            }
        }

        /// <summary>
        /// Gets or sets the DiffuseMapSampler
        /// </summary>
        private List<SamplerStateDescription> diffuseMapSamplerList = new List<SamplerStateDescription>(); //DefaultSamplers.LinearSamplerWrapAni4;
        public List<SamplerStateDescription> DiffuseMapSamplerList
        {
            get
            {
                return diffuseMapSamplerList;
            }
            set
            {
                Set(ref diffuseMapSamplerList, value, "DiffuseMapSamplerList");
            }
        }

        /// <summary>
        /// Gets or sets the DisplacementMapSampler
        /// </summary>
        private SamplerStateDescription displacementMapSampler = DefaultSamplers.LinearSamplerWrapAni1;
        public SamplerStateDescription DisplacementMapSampler
        {
            get
            {
                return displacementMapSampler;
            }
            set
            {
                Set(ref displacementMapSampler, value, "DisplacementMapSampler");
            }
        }

        private bool isFragmentLighting = false;
        public bool IsFragmentLighting
        {
            get
            {
                return isFragmentLighting;
            }
            set
            {
                Set(ref isFragmentLighting, value, "IsFragmentLighting");
            }
        }

        private bool isVertexLighting = false;
        public bool IsVertexLighting
        {
            get
            {
                return isVertexLighting;
            }
            set
            {
                Set(ref isVertexLighting, value, "IsVertexLighting");
            }
        }

        private bool isHemisphereLighting = false;
        public bool IsHemisphereLighting
        {
            get
            {
                return isHemisphereLighting;
            }
            set
            {
                Set(ref isHemisphereLighting, value, "IsHemisphereLighting");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable automatic tangent], true if [enable automatic tangent]; otherwise, false.
        /// </summary>
        private bool enableAutoTangent;
        public bool EnableAutoTangent
        {
            get
            {
                return enableAutoTangent;
            }
            set
            {
                Set(ref enableAutoTangent, value, "EnableAutoTangent");
            }
        }

        private float minTessellationDistance = 10f;
        public float MinTessellationDistance
        {
            get
            {
                return minTessellationDistance;
            }
            set
            {
                Set(ref minTessellationDistance, value, "MinTessellationDistance");
            }
        }

        private float maxTessellationDistance = 100f;
        public float MaxTessellationDistance
        {
            get
            {
                return maxTessellationDistance;
            }
            set
            {
                Set(ref maxTessellationDistance, value, "MaxTessellationDistance");
            }
        }

        /// <summary>
        /// Gets or sets the minimum distance tessellation factor
        /// </summary>
        private float minDistanceTessellationFactor = 2f;
        public float MinDistanceTessellationFactor
        {
            get
            {
                return minDistanceTessellationFactor;
            }
            set
            {
                Set(ref minDistanceTessellationFactor, value, "MinDistanceTessellationFactor");
            }
        }

        /// <summary>
        /// Gets or sets the maximum distance tessellation factor
        /// </summary>
        private float maxDistanceTessellationFactor = 1f;
        public float MaxDistanceTessellationFactor
        {
            get
            {
                return maxDistanceTessellationFactor;
            }
            set
            {
                Set(ref maxDistanceTessellationFactor, value, "MaxDistanceTessellationFactor");
            }
        }

        private MeshTopologyEnum meshType;
        public MeshTopologyEnum MeshType
        {
            get
            {
                return meshType;
            }
            set
            {
                Set(ref meshType, value, "MeshType");
            }
        }

        private bool enableTessellation;
        public bool EnableTessellation
        {
            get
            {
                return enableTessellation;
            }
            set
            {
                Set(ref enableTessellation, value, "EnableTessellation");
            }
        }

        private bool renderShadowMap;
        public bool RenderShadowMap
        {
            get
            {
                return renderShadowMap;
            }
            set
            {
                Set(ref renderShadowMap, value, "RenderShadowMap");
            }
        }

        private bool renderEnvironmentMap;
        public bool RenderEnvironmentMap
        {
            get
            {
                return renderEnvironmentMap;
            }
            set
            {
                Set(ref renderEnvironmentMap, value, "RenderEnvironmentMap");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable flat shading], true if [enable flat shading]; otherwise, false.
        /// </summary>
        private bool enableFlatShading;
        public bool EnableFlatShading
        {
            get
            {
                return enableFlatShading;
            }
            set
            {
                Set(ref enableFlatShading, value, "EnableFlatShading");
            }
        }

        /// <summary>
        /// Gets or sets the vert color blending factor. Diffuse = (1- HelixToolkit.Wpf.SharpDX.Model.PhongMaterialCore.VertexColorBlendingFactor) * Diffuse + HelixToolkit.Wpf.SharpDX.Model.PhongMaterialCore.VertexColorBlendingFactor * Vertex Color
        /// </summary>
        private float vertexColorBlendingFactor;
        public float VertexColorBlendingFactor
        {
            get
            {
                return vertexColorBlendingFactor;
            }
            set
            {
                Set(ref vertexColorBlendingFactor, value, "VertexColorBlendingFactor");
            }
        }

        public override MaterialVariable CreateMaterialVariables(IEffectsManager manager, IRenderTechnique technique)
        {
            return new CGFXMaterialVariable(manager, technique, this);
        }
    }
}
