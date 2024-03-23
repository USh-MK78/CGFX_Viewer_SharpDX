using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Shaders;
using SharpDX.Direct3D11;
//using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFX_Viewer_SharpDX
{
    public class EffectManagerHelper
    {
        public class General
        {
            /// <summary>
            /// Read Shader code
            /// </summary>
            /// <param name="path">CSO file Path</param>
            /// <returns></returns>
            /// <exception cref="ArgumentException"></exception>
            public static byte[] ReadShaderCode(string path)
            {
                if (File.Exists(path))
                {
                    return File.ReadAllBytes(path);
                }
                else
                {
                    throw new ArgumentException($"Shader File not found: {path}");
                }
            }

            public class ShaderDescriptionSetting
            {
                public string Name { get; set; }
                public ShaderStage ShaderStage { get; set; }
                public ShaderReflector ShaderReflector { get; set; }
                public byte[] ShaderCode { get; set; }

                public ShaderDescription ShaderDescription => CreateShaderDesc();
                public ShaderDescription CreateShaderDesc()
                {
                    return new ShaderDescription(Name, ShaderStage, ShaderReflector, ShaderCode);
                }

                /// <summary>
                /// Initialize ShaderDescriptionSetting
                /// </summary>
                /// <param name="Name"></param>
                /// <param name="ShaderStage"></param>
                /// <param name="ShaderReflector"></param>
                /// <param name="ShaderCode"></param>
                public ShaderDescriptionSetting(string Name, ShaderStage ShaderStage, ShaderReflector ShaderReflector, byte[] ShaderCode)
                {
                    this.Name = Name;
                    this.ShaderStage = ShaderStage;
                    this.ShaderReflector = ShaderReflector;
                    this.ShaderCode = ShaderCode;
                }

                /// <summary>
                /// Initialize ShaderDescriptionSetting (Default)
                /// </summary>
                /// <param name="Name">Name</param>
                /// <param name="ShaderStage">Shader Type</param>
                /// <param name="ShaderCode">*.cso file path</param>
                public ShaderDescriptionSetting(string Name, ShaderStage ShaderStage, byte[] ShaderCode)
                {
                    this.Name = Name;
                    this.ShaderStage = ShaderStage;
                    this.ShaderReflector = new ShaderReflector();
                    this.ShaderCode = ShaderCode;
                }
            }

            //Import, Export etc..
        }

        public class Shader
        {
            public byte[] PSInput_Main_ShaderByteCode; //PSInput main(VSInput input)
            public ShaderPass[] ShaderPasses;
            public InputElement[] InputElementData;

            /// <summary>
            /// Initialize Shader
            /// </summary>
            /// <param name="PSInput_Main_ShaderByteCode">Input *.cso</param>
            /// <param name="shaderPasses"></param>
            /// <param name="InputElements">*.hlslで定義した構造体と同じものを定義</param>
            public Shader(byte[] PSInput_Main_ShaderByteCode, ShaderPass[] shaderPasses, InputElement[] InputElements)
            {
                this.PSInput_Main_ShaderByteCode = PSInput_Main_ShaderByteCode;
                ShaderPasses = shaderPasses;
                InputElementData = InputElements;
            }
        }

        public class ShaderPass
        {
            public string PassName { get; set; }
            public ShaderDescription[] ShaderDescriptions { get; set; }
            public BlendStateDescription BlendStateDescription { get; set; }
            public DepthStencilStateDescription DepthStencilStateDescription { get; set; }

            /// <summary>
            /// Initialize ShaderPass
            /// </summary>
            /// <param name="Name"></param>
            /// <param name="shaderDescriptions">VSInput_SVTarget, PSInput main(VSInput input) etc...</param>
            /// <param name="blendStateDescription"></param>
            /// <param name="depthStencilStateDescription"></param>
            public ShaderPass(string Name, ShaderDescription[] shaderDescriptions, BlendStateDescription blendStateDescription, DepthStencilStateDescription depthStencilStateDescription)
            {
                PassName = Name;
                ShaderDescriptions = shaderDescriptions;
                BlendStateDescription = blendStateDescription;
                DepthStencilStateDescription = depthStencilStateDescription;
            }

            /// <summary>
            /// Initialize ShaderPass (BlendState, DepthStencil => Default)
            /// </summary>
            /// <param name="Name"></param>
            /// <param name="shaderDescriptions"></param>
            public ShaderPass(string Name, ShaderDescription[] shaderDescriptions)
            {
                PassName = Name;
                ShaderDescriptions = shaderDescriptions;
                BlendStateDescription = DefaultBlendStateDescriptions.BSAlphaBlend;
                DepthStencilStateDescription = DefaultDepthStencilDescriptions.DSSDepthLess;
            }
        }

        public class EffectManager : DefaultEffectsManager
        {
            public Dictionary<string, Shader> ShaderDictionary;

            /// <summary>
            /// Create EffectManager
            /// </summary>
            /// <returns>DefaultEffectManager</returns>
            public DefaultEffectsManager CreateEffectManager()
            {
                foreach (var sh in ShaderDictionary)
                {
                    TechniqueDescription techniqueDesc = new TechniqueDescription(sh.Key);
                    techniqueDesc.PassDescriptions = new List<ShaderPassDescription>();
                    techniqueDesc.InputLayoutDescription = new InputLayoutDescription(sh.Value.PSInput_Main_ShaderByteCode, sh.Value.InputElementData);

                    foreach (var ShPass in sh.Value.ShaderPasses)
                    {
                        ShaderPassDescription ShaderPassDesc = new ShaderPassDescription(ShPass.PassName)
                        {
                            ShaderList = ShPass.ShaderDescriptions,
                            BlendStateDescription = ShPass.BlendStateDescription,
                            DepthStencilStateDescription = ShPass.DepthStencilStateDescription
                        };

                        techniqueDesc.PassDescriptions.Add(ShaderPassDesc);
                    }

                    AddTechnique(techniqueDesc);
                }

                //this.GeometryBufferManager

                return this;
            }

            /// <summary>
            /// Initialize EffectManager
            /// </summary>
            /// <param name="ShaderDictionary"></param>
            public EffectManager(Dictionary<string, Shader> ShaderDictionary)
            {
                this.ShaderDictionary = ShaderDictionary;
            }
        }
    }
}
