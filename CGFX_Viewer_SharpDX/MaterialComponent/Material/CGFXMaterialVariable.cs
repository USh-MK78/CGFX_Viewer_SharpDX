using HelixToolkit.Wpf.SharpDX.Core.Components;
using HelixToolkit.Wpf.SharpDX.Model;
using HelixToolkit.Wpf.SharpDX.Render;
using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using HelixToolkit.Wpf.SharpDX.Shaders;
using HelixToolkit.Wpf.SharpDX.ShaderManager;
using HelixToolkit.Wpf.SharpDX.Utilities;
using System.Runtime.CompilerServices;
using static CGFX_Viewer_SharpDX.Component.Data.TextureConbinerStage;

namespace CGFX_Viewer_SharpDX.Component.Material
{
    //Custom : CustomMaterialVariable
    //Default : MaterialVariable
    public class CGFXMaterialVariable : MaterialVariable
    {
        //private const int NUMTEXTURES = 6;
        //private const int DiffuseIdx = 0;
        //private const int AlphaIdx = 1;
        //private const int NormalIdx = 2;
        //private const int DisplaceIdx = 3;
        //private const int SpecularColorIdx = 4;
        //private const int EmissiveIdx = 5;

        private readonly ITextureResourceManager textureManager;
        private readonly IStatePoolManager statePoolManager;

        //Texture
        private ShaderResourceViewProxy[] textureResources = new ShaderResourceViewProxy[6];
        private ShaderResourceViewProxy[] AlphaMapResources = new ShaderResourceViewProxy[6];

        private SamplerStateProxy surfaceSampler;
        private SamplerStateProxy displacementSampler;
        private SamplerStateProxy shadowSampler;

        private int texSlot0 { get; set; }
        private int texSlot1 { get; set; }
        private int texSlot2 { get; set; }
        private int texSlot3 { get; set; }

        private int AlphaMapSlot0 { get; set; }
        private int AlphaMapSlot1 { get; set; }
        private int AlphaMapSlot2 { get; set; }
        private int AlphaMapSlot3 { get; set; }

        private int texShadowSlot { get; set; }
        private int texDisplaceSlot { get; set; }
        private int texSSAOSlot { get; set; }
        private int texEnvironmentSlot { get; set; }

        //private int texDiffuseSlot;
        ////private int texAlphaSlot;
        //private int texNormalSlot;
        //private int texDisplaceSlot;
        //private int texShadowSlot;
        //private int texSpecularSlot;
        ////private int texEmissiveSlot;
        //private int texSSAOSlot;
        //private int texEnvironmentSlot;
        private int samplerDiffuseSlot;
        private int samplerDisplaceSlot;
        private int samplerShadowSlot;
        private uint textureIndex;
        private bool enableTessellation;
        private readonly CGFXMaterialCore material;
        private bool HasTextures => textureIndex != 0;

        public ShaderPass MaterialPass { get; }
        public ShaderPass OITPass { get; }
        public ShaderPass OITDepthPeelingInit { get; }
        public ShaderPass OITDepthPeeling { get; }
        public ShaderPass ShadowPass { get; }
        public ShaderPass WireframePass { get; }
        public ShaderPass WireframeOITPass { get; }
        public ShaderPass WireframeOITDPPass { get; }
        public ShaderPass TessellationPass { get; }
        public ShaderPass TessellationOITPass { get; }
        public ShaderPass TessellationOITDPPass { get; }
        public ShaderPass DepthPass { get; }

        //public string ShaderAlphaTexName { get; } = "texAlphaMap";
        //public string ShaderDiffuseTexName { get; } = "texDiffuseMap";
        //public string ShaderNormalTexName { get; } = "texNormalMap";
        public string ShaderDisplaceTexName { get; } = "texDisplacementMap";
        public string ShaderShadowTexName { get; } = "texShadowMap";
        //public string ShaderSpecularTexName { get; } = "texSpecularMap";
        //public string ShaderEmissiveTexName { get; } = "texEmissiveMap";


        public string ShaderSamplerDiffuseTexName { get; } = "samplerSurface";
        public string ShaderSamplerDisplaceTexName { get; } = "samplerDisplace";
        public string ShaderSamplerShadowMapName { get; } = "samplerShadow";


        public bool EnableTessellation
        {
            get
            {
                return enableTessellation;
            }
            private set
            {
                if (DisposeObject.Set(ref enableTessellation, value))
                {
                    UpdateMappings(currentMaterialPass);
                    InvalidateRenderer();
                }
            }
        }

        private ShaderPass currentMaterialPass
        {
            get
            {
                if (!EnableTessellation)
                {
                    return MaterialPass;
                }

                return TessellationPass;
            }
        }

        public struct CGFXBufferLayout
        {

        }

        //Debug
        //StructSize => technique.ConstantBufferPool.Items[N].StructSize, BufferMeshNode => OnCreateRenderTechnique()
        public static ConstantBufferDescription CGFXMeshConstantBufferDescription = new ConstantBufferDescription("cgfxMesh", 1136); //Prev => 1072

        /// <summary>
        /// Initialize CGFXMaterialVariable
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="technique"></param>
        /// <param name="materialCore"></param>
        /// <param name="defaultPassName">ShaderPassDescriptionで設定した名前を指定</param>
        public CGFXMaterialVariable(IEffectsManager manager, IRenderTechnique technique, CGFXMaterialCore materialCore, string defaultPassName = "CGFXMeshShader") : base(manager, technique, CGFXMeshConstantBufferDescription, materialCore)
        {
            material = materialCore;
            //texDiffuseSlot = (texAlphaSlot = (texDisplaceSlot = (texNormalSlot = -1)));
            texSlot0 = (texDisplaceSlot = (texSlot1 = -1));
            samplerDiffuseSlot = (samplerDisplaceSlot = (samplerShadowSlot = -1));
            textureManager = manager.MaterialTextureManager;
            statePoolManager = manager.StateManager;
            MaterialPass = technique[defaultPassName]; //<-Break
            OITPass = technique["MeshOITPass"];
            OITDepthPeelingInit = technique["OITDepthPeelingFirst"];
            OITDepthPeeling = technique["OITDepthPeeling"];
            ShadowPass = technique["RenderShadow"];
            WireframePass = technique["Wireframe"];
            WireframeOITPass = technique["WireframeOIT"];
            WireframeOITDPPass = technique["WireframeOITDP"];
            TessellationPass = technique["MeshTriTessellation"];
            TessellationOITPass = technique["MeshTriTessellationOIT"];
            TessellationOITDPPass = technique["MeshPBRTriTessellationOITDP"];
            DepthPass = technique["DepthPrepass"];
            UpdateMappings(MaterialPass);
            EnableTessellation = materialCore.EnableTessellation;
        }

        /// <summary>
        /// CGFXMaterialのDependencyPropertyの内容と関連
        /// </summary>
        protected override void OnInitialPropertyBindings()
        {
            base.OnInitialPropertyBindings();
            //AddPropertyBinding(PropertyName, delegate { WriteValue(PropertyName[cBuffer], Property[Core]); });

            #region delete
            //AddPropertyBinding("DiffuseColor", delegate { WriteValue("vMaterialDiffuse", material.DiffuseColor); });
            //AddPropertyBinding("AmbientColor", delegate { WriteValue("vMaterialAmbient", material.AmbientColor); });
            //AddPropertyBinding("EmissiveColor", delegate { WriteValue("vMaterialEmissive", material.EmissiveColor); });
            //AddPropertyBinding("ReflectiveColor", delegate { WriteValue("vMaterialReflect", material.ReflectiveColor); });
            //AddPropertyBinding("SpecularColor", delegate { WriteValue("vMaterialSpecular", material.SpecularColor); });
            #endregion


            AddPropertyBinding("SpecularShininess", delegate { WriteValue("sMaterialShininess", material.SpecularShininess); });


            //BaseColor
            //AddPropertyBinding("DiffuseColor", delegate { WriteValue("vMaterialDiffuse", material.BaseColor.DiffuseColor); });
            //AddPropertyBinding("AmbientColor", delegate { WriteValue("vMaterialAmbient", material.BaseColor.AmbientColor); });
            //AddPropertyBinding("EmissiveColor", delegate { WriteValue("vMaterialEmissive", material.BaseColor.EmissiveColor); });
            //AddPropertyBinding("ReflectiveColor", delegate { WriteValue("vMaterialReflect", material.BaseColor.ReflectiveColor); });
            //AddPropertyBinding("SpecularColor", delegate { WriteValue("vMaterialSpecular", material.BaseColor.SpecularColor); });

            //BaseColor
            AddPropertyBinding("BaseColor", delegate
            {
                Data.ColorProperty.Base baseColor = material.BaseColor;
                WriteValue("vMaterialDiffuse", baseColor.DiffuseColor);
                WriteValue("vMaterialAmbient", baseColor.AmbientColor);
                WriteValue("vMaterialEmissive", baseColor.EmissiveColor);
                WriteValue("vMaterialSpecular", baseColor.SpecularColor); //sp0
                WriteValue("vMaterialReflect", baseColor.ReflectiveColor); //sp1
            });


            ////BaseColor
            //AddPropertyBinding("BaseColor", delegate
            //{
            //    Data.ColorProperty.Base baseColor = material.BaseColor;
            //    WriteValue("DiffuseColor", baseColor.DiffuseColor);
            //    WriteValue("AmbientColor", baseColor.AmbientColor);
            //    WriteValue("EmissiveColor", baseColor.EmissiveColor);
            //    WriteValue("ReflectiveColor", baseColor.ReflectiveColor);
            //    WriteValue("SpecularColor", baseColor.SpecularColor);
            //});

            //Constant Color
            AddPropertyBinding("ConstantColor", delegate
            {
                Data.ColorProperty.ConstantColor constantColor = material.ConstantColor;
                WriteValue("ConstantColor0", constantColor.ConstantColor0);
                WriteValue("ConstantColor1", constantColor.ConstantColor1);
                WriteValue("ConstantColor2", constantColor.ConstantColor2);
                WriteValue("ConstantColor3", constantColor.ConstantColor3);
                WriteValue("ConstantColor4", constantColor.ConstantColor4);
                WriteValue("ConstantColor5", constantColor.ConstantColor5);
            });

            //Blend Setting
            AddPropertyBinding("Blend", delegate
            {
                Data.Blending blending = material.Blend;
                WriteValue("BlendMode", (int)blending.BLEND_MODE);

                if (blending.BLEND_MODE == Data.Blending.BLENDMODE.BLEND && blending.BLEND_MODE == Data.Blending.BLENDMODE.SEPARATE)
                {
                    var dBlend = blending.Blend_Equation;

                    var colorEquation = dBlend.Value.ColorBlendMode;
                    WriteValue("ColorBlendEquationType", (int)dBlend.Value.ColorBlendEquationType);
                    WriteValue("ColorBlendSRC", colorEquation.SRC);
                    WriteValue("ColorBlendDEST", colorEquation.DEST);

                    var alphaEquation = dBlend.Value.AlphaBlendMode;
                    WriteValue("AlphaBlendEquationType", (int)dBlend.Value.AlphaBlendEquationType);
                    WriteValue("AlphaBlendSRC", alphaEquation.SRC);
                    WriteValue("AlphaBlendDEST", alphaEquation.DEST);

                }
                else if (blending.BLEND_MODE == Data.Blending.BLENDMODE.LOGICAL)
                {
                    var d = blending.LogicalBlend_Equation;
                    WriteValue("LogicalBlendMode", (int)d.Value.BLEND_ELEMENT_LOGICAL);
                }
                else if (blending.BLEND_MODE == Data.Blending.BLENDMODE.NONE) return;
            });


            AddPropertyBinding("TextureCombinerEquation", delegate
            {
                List<Data.TextureConbinerStage.TextureConbinerEquation> textureConbinerEquations = material.TextureConbinerEquationList;

                //List<int[]> d = new List<int[]>();

                //List<int[][]> ints1 = new List<int[][]>();

                //int StageCount = 1;
                //foreach (var c in textureConbinerEquations)
                //{
                //    //TextureConbinerEquation def = new TextureConbinerEquation(Equation.DefaultValueColorEquation(), Equation.DefaultValueAlphaEquation());
                //    if (c.Equals(TextureConbinerEquation.DefaultTextureConbinerEquation()) != true)
                //    {
                //        StageCount++;
                //    }
                //}


                WriteValue("StageCount", textureConbinerEquations.Count());

                for (int i = 0; i < textureConbinerEquations.Count; i++)
                {
                    int color_combinerEqation = textureConbinerEquations[i].ColorEquation.ConbinerEquation;
                    int color_ScaleFactor = textureConbinerEquations[i].ColorEquation.ScaleFactor;
                    bool color_IsBuffering = textureConbinerEquations[i].ColorEquation.IsBuffering;

                    int col_src_a_src = textureConbinerEquations[i].ColorEquation.SOURCE_A.SOURCE;
                    int col_src_a_op = (int)textureConbinerEquations[i].ColorEquation.SOURCE_A.GetOPERAND<Data.TextureConbinerStage.COLOR_OPERAND>();

                    int col_src_b_src = textureConbinerEquations[i].ColorEquation.SOURCE_B.SOURCE;
                    int col_src_b_op = (int)textureConbinerEquations[i].ColorEquation.SOURCE_B.GetOPERAND<Data.TextureConbinerStage.COLOR_OPERAND>();

                    int col_src_c_src = textureConbinerEquations[i].ColorEquation.SOURCE_C.SOURCE;
                    int col_src_c_op = (int)textureConbinerEquations[i].ColorEquation.SOURCE_C.GetOPERAND<Data.TextureConbinerStage.COLOR_OPERAND>();

                    //int col_src_a_src = textureConbinerEquations[i].ColorEquation.SOURCE_A.SOURCE;
                    //int col_src_a_op = textureConbinerEquations[i].ColorEquation.SOURCE_A.OPERAND;

                    //int col_src_b_src = textureConbinerEquations[i].ColorEquation.SOURCE_B.SOURCE;
                    //int col_src_b_op = textureConbinerEquations[i].ColorEquation.SOURCE_B.OPERAND;

                    //int col_src_c_src = textureConbinerEquations[i].ColorEquation.SOURCE_C.SOURCE;
                    //int col_src_c_op = textureConbinerEquations[i].ColorEquation.SOURCE_C.OPERAND;

                    WriteValue("colorCombinerEquation" + i, color_combinerEqation);
                    WriteValue("colorCombinerScale" + i, color_ScaleFactor);
                    WriteValue("colorIsBuffering" + i, color_IsBuffering);
                    WriteValue("colorSRC_A_Src" + i, col_src_a_src);
                    WriteValue("colorSRC_A_Op" + i, col_src_a_op);
                    WriteValue("colorSRC_B_Src" + i, col_src_b_src);
                    WriteValue("colorSRC_B_Op" + i, col_src_b_op);
                    WriteValue("colorSRC_C_Src" + i, col_src_c_src);
                    WriteValue("colorSRC_C_Op" + i, col_src_c_op);

                    //WriteValue("colorCombinerEquation" + i.ToString(), color_combinerEqation);
                    //WriteValue("colorCombinerScale" + i.ToString(), color_ScaleFactor);
                    //WriteValue("colorIsBuffering" + i.ToString(), color_IsBuffering);
                    //WriteValue("colorSRC_A_Src" + i.ToString(), col_src_a_src);
                    //WriteValue("colorSRC_A_Op" + i.ToString(), col_src_a_op);
                    //WriteValue("colorSRC_B_Src" + i.ToString(), col_src_b_src);
                    //WriteValue("colorSRC_B_Op" + i.ToString(), col_src_b_op);
                    //WriteValue("colorSRC_C_Src" + i.ToString(), col_src_c_src);
                    //WriteValue("colorSRC_C_Op" + i.ToString(), col_src_c_op);
                }

                for (int i = 0; i < textureConbinerEquations.Count; i++)
                {
                    int alpha_combinerEqation = textureConbinerEquations[i].AlphaEquation.ConbinerEquation;
                    int alpha_ScaleFactor = textureConbinerEquations[i].AlphaEquation.ScaleFactor;
                    bool alpha_IsBuffering = textureConbinerEquations[i].AlphaEquation.IsBuffering;

                    int alpha_src_a_src = textureConbinerEquations[i].AlphaEquation.SOURCE_A.SOURCE;
                    int alpha_src_a_op = (int)textureConbinerEquations[i].AlphaEquation.SOURCE_A.GetOPERAND<Data.TextureConbinerStage.ALPHA_OPERAND>();

                    int alpha_src_b_src = textureConbinerEquations[i].AlphaEquation.SOURCE_B.SOURCE;
                    int alpha_src_b_op = (int)textureConbinerEquations[i].AlphaEquation.SOURCE_B.GetOPERAND<Data.TextureConbinerStage.ALPHA_OPERAND>();

                    int alpha_src_c_src = textureConbinerEquations[i].AlphaEquation.SOURCE_C.SOURCE;
                    int alpha_src_c_op = (int)textureConbinerEquations[i].AlphaEquation.SOURCE_C.GetOPERAND<Data.TextureConbinerStage.ALPHA_OPERAND>();

                    WriteValue("alphaCombinerEquation" + i.ToString(), alpha_combinerEqation);
                    WriteValue("alphaCombinerScale" + i.ToString(), alpha_ScaleFactor);
                    WriteValue("alphaIsBuffering" + i.ToString(), alpha_IsBuffering);
                    WriteValue("alphaSRC_A_Src" + i.ToString(), alpha_src_a_src);
                    WriteValue("alphaSRC_A_Op" + i.ToString(), alpha_src_a_op);
                    WriteValue("alphaSRC_B_Src" + i.ToString(), alpha_src_b_src);
                    WriteValue("alphaSRC_B_Op" + i.ToString(), alpha_src_b_op);
                    WriteValue("alphaSRC_C_Src" + i.ToString(), alpha_src_c_src);
                    WriteValue("alphaSRC_C_Op" + i.ToString(), alpha_src_c_op);
                }



                //foreach (Data.TextureConbinerStage.TextureConbinerEquation h in textureConbinerEquations)
                //{
                //    int color_combinerEqation = h.ColorEquation.ConbinerEquation;
                //    int color_ScaleFactor = h.ColorEquation.ScaleFactor;
                //    bool color_IsBuffering = h.ColorEquation.IsBuffering;

                //    int col_src_a_src = h.ColorEquation.SOURCE_A.SOURCE;
                //    int col_src_a_op = h.ColorEquation.SOURCE_A.OPERAND;

                //    int col_src_b_src = h.ColorEquation.SOURCE_B.SOURCE;
                //    int col_src_b_op = h.ColorEquation.SOURCE_B.OPERAND;

                //    int col_src_c_src = h.ColorEquation.SOURCE_C.SOURCE;
                //    int col_src_c_op = h.ColorEquation.SOURCE_C.OPERAND;

                //    int[][] ints2 = new int[][] { new int[] { col_src_a_src, col_src_a_op }, new int[] { col_src_b_src, col_src_b_op }, new int[] { col_src_c_src, col_src_c_op } };
                //    //int[,] ints2 = new int[,] { { col_src_a_op, col_src_a_src }, { col_src_b_op, col_src_b_src }, { col_src_c_op, col_src_c_src } };

                //    //var k = ints2.ToList().Select(x => x.Length).Sum();
                //    //IntPtr intPtr_ColorCombinerEquation = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * ints2.ToList().Select(x=>x.Length).Sum());
                //    //WriteValue("as1", (long)intPtr_ColorCombinerEquation);

                //    //IntPtr hn = (IntPtr)10;
                //    //IntPtr d = new IntPtr((long)hn);


                //    //WriteValue("Color_CombinerEquation", color_combinerEqation);
                //    //WriteValue("Color_ScaleFactor", color_ScaleFactor);
                //    //Wri

                //    //WriteValue("ColorSRC_A_Operand", col_src_a_op);
                //    //WriteValue("ColorSRC_A_Source", col_src_a_src);

                //    //WriteValue("ColorSRC_B_Operand", col_src_b_op);
                //    //WriteValue("ColorSRC_B_Source", col_src_b_src);

                //    //WriteValue("ColorSRC_C_Operand", col_src_c_op);
                //    //WriteValue("ColorSRC_C_Source", col_src_c_src);


                //    var alpha_combinerEqation = h.AlphaEquation.GetConbinerEquation();
                //    var alpha_ScaleFactor = h.AlphaEquation.ScaleFactor;
                //    var alpha_IsBuffering = h.AlphaEquation.IsBuffering;

                //    var alpha_src_a_op = h.AlphaEquation.SOURCE_A.GetOPERAND();
                //    var alpha_src_a_src = h.AlphaEquation.SOURCE_A.GetSOURCE();

                //    var alpha_src_b_op = h.AlphaEquation.SOURCE_B.GetOPERAND();
                //    var alpha_src_b_src = h.AlphaEquation.SOURCE_B.GetSOURCE();

                //    var alpha_src_c_op = h.AlphaEquation.SOURCE_C.GetOPERAND();
                //    var alpha_src_c_src = h.AlphaEquation.SOURCE_C.GetSOURCE();

                //var j = GetStorage();
                //var ids = GetStorageID();
                //int[] ints = new int[] { 0, 1, 2, 3 };
                //WriteValueArray("as1", ints);



                //    //h.ColorEquation.SOURCE_A.
                //}

                //var k = ints1.ToList().Select(x => x.ToList().Select(y => y.Length).Sum()).Sum();
                //IntPtr intPtr_ColorCombinerEquation = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * k);
                ////WriteValue("CVS[6][3][2]", (int)intPtr_ColorCombinerEquation);
                //WriteValue("CVS[6][3][2]", (int)intPtr_ColorCombinerEquation);
            });


            //AddPropertyBinding("UVTransform", delegate
            //{
            //    Matrix matrix = material.UVTransform;
            //    WriteValue("uvTransformR1", matrix.Column1);
            //    WriteValue("uvTransformR2", matrix.Column2);
            //});




            AddPropertyBinding("UVTransform", delegate
            {
                List<UVTransform> uvTransforms = material.UVTransform;

                for (int uvCount = 0; uvCount < uvTransforms.Count; uvCount++)
                {
                    Matrix matrix = uvTransforms[uvCount];
                    WriteValue("uvTransformR1" + uvCount, matrix.Column1);
                    WriteValue("uvTransformR2" + uvCount, matrix.Column2);
                }
            });

            AddPropertyBinding("EnableAutoTangent", delegate { WriteValue("bAutoTengent", material.EnableAutoTangent); });
            AddPropertyBinding("MaxTessellationDistance", delegate { WriteValue("maxTessDistance", material.MaxTessellationDistance); });
            AddPropertyBinding("MaxDistanceTessellationFactor", delegate { WriteValue("maxTessFactor", material.MaxDistanceTessellationFactor); });
            AddPropertyBinding("MinTessellationDistance", delegate { WriteValue("minTessDistance", material.MinTessellationDistance); });
            AddPropertyBinding("MinDistanceTessellationFactor", delegate { WriteValue("minTessFactor", material.MinDistanceTessellationFactor); });

            //Lighting
            AddPropertyBinding("IsFragmentLighting", delegate { WriteValue("bIsFragmentLighting", material.IsFragmentLighting); });
            AddPropertyBinding("IsVertexLighting", delegate { WriteValue("bIsVertexLighting", material.IsVertexLighting); });
            AddPropertyBinding("IsHemisphereLighting", delegate { WriteValue("bIsHemisphereLighting", material.IsHemisphereLighting); });

            //AddPropertyBinding("RenderDiffuseMap", delegate { WriteValue("bHasDiffuseMap", material.RenderDiffuseMap ? 1 : 0); });
            //AddPropertyBinding("RenderNormalMap", delegate { WriteValue("bHasNormalMap", material.RenderNormalMap ? 1 : 0); });
            //AddPropertyBinding("RenderSpecularMap", delegate { WriteValue("bHasSpecularMap", material.RenderSpecularColorMap ? 1 : 0); });


            //AddPropertyBinding("DisplacementMapScaleMask", delegate { WriteValue("displacementMapScaleMask", material.DisplacementMapScaleMask); });
            AddPropertyBinding("RenderShadowMap", delegate
            {
                WriteValue("bRenderShadowMap", material.RenderShadowMap ? 1 : 0);
            });
            AddPropertyBinding("RenderEnvironmentMap", delegate
            {
                WriteValue("bHasCubeMap", material.RenderEnvironmentMap ? 1 : 0);
            });

            AddPropertyBinding("EnableFlatShading", delegate { WriteValue("bRenderFlat", material.EnableFlatShading); });
            AddPropertyBinding("VertexColorBlendingFactor", delegate { WriteValue("vertColorBlending", material.VertexColorBlendingFactor); });

            #region Add Texture
            //AddPropertyBinding("DiffuseMap", delegate
            //{
            //    CreateTextureView(material.DiffuseMap, 0); //TextureViewの作成
            //    TriggerPropertyAction("RenderDiffuseMap"); //指定した名前のキーを検索し、値を代入する
            //});
            //AddPropertyBinding("DiffuseAlphaMap", delegate
            //{
            //    CreateTextureView(material.DiffuseAlphaMap, 1);
            //    TriggerPropertyAction("RenderDiffuseAlphaMap");
            //});
            //AddPropertyBinding("NormalMap", delegate
            //{
            //    CreateTextureView(material.NormalMap, 2);
            //    TriggerPropertyAction("RenderNormalMap");
            //});
            //AddPropertyBinding("DisplacementMap", delegate
            //{
            //    CreateTextureView(material.DisplacementMap, 3);
            //    TriggerPropertyAction("RenderDisplacementMap");
            //});
            //AddPropertyBinding("SpecularColorMap", delegate
            //{
            //    CreateTextureView(material.SpecularColorMap, 4);
            //    TriggerPropertyAction("RenderSpecularColorMap");
            //});
            #endregion


            //AddPropertyBinding("BaseColor", delegate
            //{
            //    Data.ColorProperty.Base baseColor = material.BaseColor;
            //    WriteValue()

            //    Matrix matrix = material.UVTransform;
            //    WriteValue("uvTransformR1", matrix.Column1);
            //    WriteValue("uvTransformR2", matrix.Column2);
            //});

            //[TODO]
            //・Render判定(bool)を削除する
            //・テクスチャの書き込み
            //・TextureTypeの内容をETC1などに変更する
            //・hlslでも同様の変更を加える
            AddPropertyBinding("TextureSlotList", delegate
            {
                foreach (var h in material.TextureSlotList)
                {
                    //AlphaMap
                    var d = h.Value.AlphaMapSlot;
                    CreateTextureView(h.Value.AlphaMapSlot, AlphaMapResources, h.Key);

                    //Texture (Diffuse, Normal, Specular etc...)
                    CreateTextureView(h.Value.TextureModel, textureResources, h.Key);


                    ////Data.Texture.TextureType.ETC1, Data.Texture.TextureType.ETC1A4, ...
                    //if (h.Value.TextureType == Data.Texture.TextureType.Diffuse)
                    //{
                    //    //TriggerPropertyAction("RenderDiffuseMap"); //Delete
                    //}
                    //if (h.Value.TextureType == Data.Texture.TextureType.Normal)
                    //{
                    //    //TriggerPropertyAction("RenderNormalMap"); //Delete
                    //}
                    //if (h.Value.TextureType == Data.Texture.TextureType.Specular)
                    //{
                    //    //TriggerPropertyAction("RenderSpecularMap"); //Delete
                    //}
                }
            });






            //foreach (var h in material.TextureSlotList)
            //{
            //    if (h.Value.TextureType == Data.Texture.TextureType.Diffuse)
            //    {
            //        CreateTextureView(h.Value.TextureModel, h.Key);
            //    }



            //    AddPropertyBinding("SpecularColorMap", delegate
            //    {
            //        CreateTextureView(h.Value, h.Key);
            //        TriggerPropertyAction("RenderSpecularColorMap");
            //    });
            //}




            AddPropertyBinding("DiffuseMapSampler", delegate
            {
                SamplerStateProxy samplerStateProxy2 = statePoolManager.Register(material.DiffuseMapSampler);
                DisposeObject.RemoveAndDispose(ref surfaceSampler);
                surfaceSampler = samplerStateProxy2;
            });
            AddPropertyBinding("DisplacementMapSampler", delegate
            {
                SamplerStateProxy samplerStateProxy = statePoolManager.Register(material.DisplacementMapSampler);
                DisposeObject.RemoveAndDispose(ref displacementSampler);
                displacementSampler = samplerStateProxy;
            });
            //AddPropertyBinding("EmissiveMap", delegate
            //{
            //    CreateTextureView(material.EmissiveMap, 5);
            //    TriggerPropertyAction("RenderEmissiveMap");
            //});
            AddPropertyBinding("EnableTessellation", delegate
            {
                EnableTessellation = material.EnableTessellation;
            });
            shadowSampler = statePoolManager.Register(DefaultSamplers.ShadowSampler);
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private void CreateTextureView(TextureModel textureModel, int index)
        //{
        //    ShaderResourceViewProxy shaderResourceViewProxy = ((textureModel == null) ? null : textureManager.Register(textureModel));
        //    DisposeObject.RemoveAndDispose(ref textureResources[index]);
        //    textureResources[index] = shaderResourceViewProxy;
        //    if (textureResources[index] != null)
        //    {
        //        textureIndex |= (uint)(1 << index);
        //    }
        //    else
        //    {
        //        textureIndex &= (uint)(~(1 << index));
        //    }
        //}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CreateTextureView(TextureModel textureModel, ShaderResourceViewProxy[] textureResources, int index)
        {
            ShaderResourceViewProxy shaderResourceViewProxy = ((textureModel == null) ? null : textureManager.Register(textureModel));
            DisposeObject.RemoveAndDispose(ref textureResources[index]);
            textureResources[index] = shaderResourceViewProxy;
            if (textureResources[index] != null)
            {
                textureIndex |= (uint)(1 << index);
            }
            else
            {
                textureIndex &= (uint)(~(1 << index));
            }
        }

        public override bool BindMaterialResources(RenderContext context, DeviceContextProxy deviceContext, ShaderPass shaderPass)
        {
            if (HasTextures)
            {
                OnBindMaterialTextures(deviceContext, shaderPass.VertexShader);
                OnBindMaterialTextures(deviceContext, shaderPass.DomainShader);
                OnBindMaterialTextures(context, deviceContext, shaderPass.PixelShader);
            }

            if (material.RenderShadowMap && context.IsShadowMapEnabled)
            {
                shaderPass.PixelShader.BindTexture(deviceContext, texShadowSlot, context.SharedResource.ShadowView);
                shaderPass.PixelShader.BindSampler(deviceContext, samplerShadowSlot, shadowSampler);
            }

            shaderPass.PixelShader.BindTexture(deviceContext, texSSAOSlot, context.SharedResource.SSAOMap);
            shaderPass.PixelShader.BindTexture(deviceContext, texEnvironmentSlot, context.SharedResource.EnvironementMap);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnBindMaterialTextures(DeviceContextProxy context, VertexShader shader)
        {
            if (!shader.IsNULL)
            {
                int shaderStageIndex = shader.ShaderStageIndex;
                shader.BindTexture(context, texDisplaceSlot, textureResources[3]);
                shader.BindSampler(context, samplerDisplaceSlot, displacementSampler);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnBindMaterialTextures(DeviceContextProxy context, DomainShader shader)
        {
            if (!shader.IsNULL)
            {
                int shaderStageIndex = shader.ShaderStageIndex;
                shader.BindTexture(context, texDisplaceSlot, textureResources[3]);
                shader.BindSampler(context, samplerDisplaceSlot, displacementSampler);

                //D3D11.BlendStateDescription blendStateDescription = D3D11.BlendStateDescription.Default();
                //blendStateDescription.GetType().GetMethod("__MarshalFrom").Invoke(blendStateDescription, new object[] {BlendStateDescription});

                //SharpDX.Direct3D11.DeviceContext f = (SharpDX.Direct3D11.DeviceContext)context;
                //f.OutputMerger.SetBlendState(new D3D11.BlendState(f.Device, D3D11.BlendStateDescription.Default()), new RawColor4(1, 1, 1, 1), 0x00000000);

                //f.OutputMerger.GetRenderTargets(out D3D11.Blend)
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnBindMaterialTextures(RenderContext context, DeviceContextProxy deviceContext, PixelShader shader)
        {
            if (!shader.IsNULL)
            {
                //HLSL内のテクスチャスロット番号(Buffer : register(tX))とtextureResourceを使用してテクスチャをバインドする
                //BindTexture(deviceContext, RegisterNumber, Resource);
                int shaderStageIndex = shader.ShaderStageIndex;
                shader.BindTexture(deviceContext, texSlot0, textureResources[0]);
                shader.BindTexture(deviceContext, texSlot1, textureResources[1]);
                shader.BindTexture(deviceContext, texSlot2, textureResources[2]);
                shader.BindTexture(deviceContext, texSlot3, textureResources[3]);


                shader.BindTexture(deviceContext, AlphaMapSlot0, AlphaMapResources[0]);
                shader.BindTexture(deviceContext, AlphaMapSlot1, AlphaMapResources[1]);
                shader.BindTexture(deviceContext, AlphaMapSlot2, AlphaMapResources[2]);
                shader.BindTexture(deviceContext, AlphaMapSlot3, AlphaMapResources[3]);


                //shader.BindTexture(deviceContext, texAlphaSlot, textureResources[1]);
                //shader.BindTexture(deviceContext, texEmissiveSlot, textureResources[5]);
                shader.BindSampler(deviceContext, samplerDiffuseSlot, surfaceSampler);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateMappings(ShaderPass shaderPass)
        {
            //HLSL内のテクスチャスロット番号(Buffer : register(tX))をTryGetBindSlot()を使用して取得(HLSLで定義したバッファの名前を入力)
            texSlot0 = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot("Texture0");
            texSlot1 = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot("Texture1");
            texSlot2 = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot("Texture2");
            texSlot3 = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot("Texture3");

            //string h = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetName(66);


            AlphaMapSlot0 = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot("AlphaMap0");
            AlphaMapSlot1 = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot("AlphaMap1");
            AlphaMapSlot2 = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot("AlphaMap2");
            AlphaMapSlot3 = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot("AlphaMap3");


            texShadowSlot = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot("texShadowMap");
            texSSAOSlot = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot("texSSAOMap");
            texEnvironmentSlot = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot("texCubeMap");
            samplerDiffuseSlot = shaderPass.PixelShader.SamplerMapping.TryGetBindSlot("samplerSurface");
            samplerShadowSlot = shaderPass.PixelShader.SamplerMapping.TryGetBindSlot("samplerShadow");
            if (!shaderPass.DomainShader.IsNULL && material.EnableTessellation)
            {
                texDisplaceSlot = shaderPass.DomainShader.ShaderResourceViewMapping.TryGetBindSlot("texDisplacementMap");
                samplerDisplaceSlot = shaderPass.DomainShader.SamplerMapping.TryGetBindSlot("samplerDisplace");
            }
            else
            {
                texDisplaceSlot = shaderPass.VertexShader.ShaderResourceViewMapping.TryGetBindSlot("texDisplacementMap");
                samplerDisplaceSlot = shaderPass.VertexShader.SamplerMapping.TryGetBindSlot("samplerDisplace");
            }



            ////HLSL内のテクスチャスロット番号(Buffer : register(tX))をTryGetBindSlot()を使用して取得(HLSLで定義したバッファの名前を入力)
            //texDiffuseSlot = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot(ShaderDiffuseTexName);
            ////texAlphaSlot = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot(ShaderAlphaTexName);
            //texNormalSlot = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot(ShaderNormalTexName);
            //texShadowSlot = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot(ShaderShadowTexName);
            //texSpecularSlot = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot(ShaderSpecularTexName);
            ////texEmissiveSlot = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot(ShaderEmissiveTexName);
            //texSSAOSlot = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot("texSSAOMap");
            //texEnvironmentSlot = shaderPass.PixelShader.ShaderResourceViewMapping.TryGetBindSlot("texCubeMap");
            //samplerDiffuseSlot = shaderPass.PixelShader.SamplerMapping.TryGetBindSlot(ShaderSamplerDiffuseTexName);
            //samplerShadowSlot = shaderPass.PixelShader.SamplerMapping.TryGetBindSlot(ShaderSamplerShadowMapName);
            //if (!shaderPass.DomainShader.IsNULL && material.EnableTessellation)
            //{
            //    texDisplaceSlot = shaderPass.DomainShader.ShaderResourceViewMapping.TryGetBindSlot("texDisplacementMap");
            //    samplerDisplaceSlot = shaderPass.DomainShader.SamplerMapping.TryGetBindSlot("samplerDisplace");
            //}
            //else
            //{
            //    texDisplaceSlot = shaderPass.VertexShader.ShaderResourceViewMapping.TryGetBindSlot("texDisplacementMap");
            //    samplerDisplaceSlot = shaderPass.VertexShader.SamplerMapping.TryGetBindSlot("samplerDisplace");
            //}
        }


        protected override void OnDispose(bool disposeManagedResources)
        {
            for (int i = 0; i < textureResources.Length; i++)
            {
                DisposeObject.RemoveAndDispose(ref textureResources[i]);
            }

            for (int i = 0; i < AlphaMapResources.Length; i++)
            {
                DisposeObject.RemoveAndDispose(ref AlphaMapResources[i]);
            }

            DisposeObject.RemoveAndDispose(ref surfaceSampler);
            DisposeObject.RemoveAndDispose(ref displacementSampler);
            DisposeObject.RemoveAndDispose(ref shadowSampler);
            base.OnDispose(disposeManagedResources);
        }

        public override ShaderPass GetPass(RenderType renderType, RenderContext context)
        {
            if (renderType == RenderType.Transparent)
            {
                switch (context.OITRenderStage)
                {
                    case OITRenderStage.SinglePassWeighted:
                        if (!EnableTessellation)
                        {
                            return OITPass;
                        }

                        return TessellationOITPass;
                    case OITRenderStage.DepthPeelingInitMinMaxZ:
                        return OITDepthPeelingInit;
                    case OITRenderStage.DepthPeeling:
                        if (!EnableTessellation)
                        {
                            return OITDepthPeeling;
                        }

                        return TessellationOITDPPass;
                }
            }

            return currentMaterialPass;
        }

        public override ShaderPass GetShadowPass(RenderType renderType, RenderContext context)
        {
            return ShadowPass;
        }

        public override ShaderPass GetWireframePass(RenderType renderType, RenderContext context)
        {
            if (renderType == RenderType.Transparent)
            {
                switch (context.OITRenderStage)
                {
                    case OITRenderStage.SinglePassWeighted:
                        return WireframeOITPass;
                    case OITRenderStage.DepthPeelingInitMinMaxZ:
                        return OITDepthPeelingInit;
                    case OITRenderStage.DepthPeeling:
                        return WireframeOITDPPass;
                }
            }

            return WireframePass;
        }

        public override ShaderPass GetDepthPass(RenderType renderType, RenderContext context)
        {
            return DepthPass;
        }

        public override void Draw(DeviceContextProxy deviceContext, IAttachableBufferModel bufferModel, int instanceCount)
        {
            MaterialVariable.DrawIndexed(deviceContext, bufferModel.IndexBuffer.ElementCount, instanceCount);
        }
    }
}
