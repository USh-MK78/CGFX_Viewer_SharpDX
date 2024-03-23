using HelixToolkit.Wpf.SharpDX.Shaders;
using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CGFX_Viewer_SharpDX.EffectManagerHelper;
using ShaderPass = CGFX_Viewer_SharpDX.EffectManagerHelper.ShaderPass;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Runtime.CompilerServices;
using CGFX_Viewer_SharpDX.Component.Mesh;
using CGFX_Viewer_SharpDX.Component.Material;
using Assimp;
using HelixToolkit.Wpf.SharpDX.Model.Scene;
//using CGFX_Viewer_SharpDX.Component;

namespace CGFX_Viewer_SharpDX
{
    public class CGFXEffectManager
    {
        public static DefaultEffectsManager Create()
        {
            Dictionary<string, General.ShaderDescriptionSetting> SDSDict = new Dictionary<string, General.ShaderDescriptionSetting>();
            SDSDict.Add("0", new General.ShaderDescriptionSetting("vsCGFXMaterial", ShaderStage.Vertex, General.ReadShaderCode("vsCGFXMaterial.cso")));
            SDSDict.Add("1", new General.ShaderDescriptionSetting("psCGFXMaterial", ShaderStage.Pixel, General.ReadShaderCode("psCGFXMaterial.cso")));

            List<ShaderPass> shaderPasses0 = new List<ShaderPass>();
            shaderPasses0.Add(new ShaderPass("CGFXMeshShader", new ShaderDescription[] { SDSDict["0"].ShaderDescription, SDSDict["1"].ShaderDescription }));

            //Vertex Shader(*.hlsl)で定義した構造体
            InputElement[] VS_InputElements = new InputElement[] //InputElement[n], slot => Bufferに書き込む順番
            {
                    new InputElement("POSITION", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0), //CGFXVertex
                    new InputElement("NORMAL", 0, Format.R32G32B32_Float, InputElement.AppendAligned, 0), //CGFXVertex
                    new InputElement("TANGENT", 0, Format.R32G32B32_Float, InputElement.AppendAligned, 0), //CGFXVertex
                    new InputElement("BINORMAL", 0, Format.R32G32B32_Float, InputElement.AppendAligned, 0), //CGFXVertex

                    //new InputElement("COLOR", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 1),
                    //new InputElement("TEXCOORD", 0, Format.R32G32_Float, InputElement.AppendAligned, 2),
                    //new InputElement("TEXCOORD", 1, Format.R32G32_Float, InputElement.AppendAligned, 3),
                    //new InputElement("TEXCOORD", 2, Format.R32G32_Float, InputElement.AppendAligned, 4),


                    
                    new InputElement("TEXCOORD", 0, Format.R32G32_Float, InputElement.AppendAligned, 1),
                    new InputElement("TEXCOORD", 1, Format.R32G32_Float, InputElement.AppendAligned, 2),
                    new InputElement("TEXCOORD", 2, Format.R32G32_Float, InputElement.AppendAligned, 3),
                    new InputElement("COLOR", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 4),


                    //new InputElement("TEXCOORD", 3, Format.R32G32_Float, InputElement.AppendAligned, 0),

                    new InputElement("TEXCOORD", 3, Format.R32G32B32A32_Float, InputElement.AppendAligned, 5, InputClassification.PerInstanceData, 1),
                    new InputElement("TEXCOORD", 4, Format.R32G32B32A32_Float, InputElement.AppendAligned, 5, InputClassification.PerInstanceData, 1),
                    new InputElement("TEXCOORD", 5, Format.R32G32B32A32_Float, InputElement.AppendAligned, 5, InputClassification.PerInstanceData, 1),
                    new InputElement("TEXCOORD", 6, Format.R32G32B32A32_Float, InputElement.AppendAligned, 5, InputClassification.PerInstanceData, 1)
            };

            Dictionary<string, Shader> ShaderDict = new Dictionary<string, Shader>();
            ShaderDict.Add("CGFXShaderSampling", new Shader(SDSDict["0"].ShaderDescription.ByteCode, shaderPasses0.ToArray(), VS_InputElements));

            EffectManager effectManager = new EffectManager(ShaderDict);
            return effectManager.CreateEffectManager();
        }
    }
}
