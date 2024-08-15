#ifndef CGFXBUFFERS_HLSL
#define CGFXBUFFERS_HLSL
#pragma pack_matrix( row_major )

#include"CommonBuffers.hlsl"

#if defined(CGFXMESH) //defined CGFXMESH buffer
cbuffer cgfxMesh : register(b1)
{
    // Common Parameters
    float4x4 mWorld;
    bool bInvertNormal = false;
    bool bHasInstances = false;
    bool bHasInstanceParams = false;
    bool bHasBones = false;
    float4 vParams = float4(0, 0, 0, 0); //Shared with models
    float4 vColor = float4(1, 1, 1, 1); //Shared with models
    float4 wireframeColor;
    bool3 bParams; // Shared with models for enable/disable features
    bool bBatched = false;

    // Material Parameters changable
    float minTessDistance = 1;
    float maxTessDistance = 100;
    float minTessFactor = 4;
    float maxTessFactor = 1;

    float4 vMaterialDiffuse = 0.5f; //Kd := surface material's diffuse coefficient
    float4 vMaterialAmbient = 0.25f; //Ka := surface material's ambient coefficient.
    float4 vMaterialEmissive = 0.0f; //Ke := surface material's emissive coefficient
    float4 vMaterialSpecular = 0.0f; //Ks := surface material's specular coefficient. If using PBR, vMaterialReflect = float4(ConstantAO, ConstantRoughness, ConstantMetallic, ConstantReflectance);
    float4 vMaterialReflect = 0.0f; //Kr := surface material's reflectivity coefficient. If using PBR, vMaterialSpecular = float4(ClearCoat, ClearCoatRoughness, 0, 0)

    bool bIsFragmentLighting = false;
    bool bIsVertexLighting = false;
    bool bIsHemiSphereLighting = false;


    //bool bHasDiffuseMap = false;
    //bool bHasNormalMap = false;
    //bool bHasCubeMap = false;
    //bool bRenderShadowMap = false;
    //bool bHasEmissiveMap = false;
    //bool bHasSpecularMap = false;

    //int bTextureSlotNum[4];
    
    //int bTextureCombinerStageCount[6];
    //int bTexTureCombinerEquationColor[6];


    int BlendMode;
    bool IsLogicaal = false;
    int LOGICALMODE;
    
    int ColorBlendSRC;
    int ColorBlendDEST;
    int ColorBlendEqationType;
    int AlphaBlendSRC;
    int AlphaBlendDEST;
    int AlphaBlendEqationType;
    
    float4 ConstantColor0;
    float4 ConstantColor1;
    float4 ConstantColor2;
    float4 ConstantColor3;
    float4 ConstantColor4;
    float4 ConstantColor5;
    
    int StageCount = -1;
    float3 StageCountpadding;

    int colorCombinerEquation0 = -1;
    int colorCombinerScale0 = -1;
    bool colorIsBuffering0 = -1;
    int colorSRC_A_Op0 = -1;
    int colorSRC_A_Src0 = -1;
    int colorSRC_B_Op0 = -1;
    int colorSRC_B_Src0 = -1;
    int colorSRC_C_Op0 = -1;
    int colorSRC_C_Src0 = -1;
    float3 paddingCE0;
    
    int colorCombinerEquation1 = -1;
    int colorCombinerScale1 = -1;
    bool colorIsBuffering1 = -1;
    int colorSRC_A_Op1 = -1;
    int colorSRC_A_Src1 = -1;
    int colorSRC_B_Op1 = -1;
    int colorSRC_B_Src1 = -1;
    int colorSRC_C_Op1 = -1;
    int colorSRC_C_Src1 = -1;
    float3 paddingCE1;
    
    int colorCombinerEquation2 = -1;
    int colorCombinerScale2 = -1;
    bool colorIsBuffering2 = -1;
    int colorSRC_A_Op2 = -1;
    int colorSRC_A_Src2 = -1;
    int colorSRC_B_Op2 = -1;
    int colorSRC_B_Src2 = -1;
    int colorSRC_C_Op2 = -1;
    int colorSRC_C_Src2 = -1;
    float3 paddingCE2;
    
    int colorCombinerEquation3 = -1;
    int colorCombinerScale3 = -1;
    bool colorIsBuffering3 = -1;
    int colorSRC_A_Op3 = -1;
    int colorSRC_A_Src3 = -1;
    int colorSRC_B_Op3 = -1;
    int colorSRC_B_Src3 = -1;
    int colorSRC_C_Op3 = -1;
    int colorSRC_C_Src3 = -1;
    float3 paddingCE3;
    
    int colorCombinerEquation4 = -1;
    int colorCombinerScale4 = -1;
    bool colorIsBuffering4 = -1;
    int colorSRC_A_Op4 = -1;
    int colorSRC_A_Src4 = -1;
    int colorSRC_B_Op4 = -1;
    int colorSRC_B_Src4 = -1;
    int colorSRC_C_Op4 = -1;
    int colorSRC_C_Src4 = -1;
    float3 paddingCE4;
    
    int colorCombinerEquation5 = -1;
    int colorCombinerScale5 = -1;
    bool colorIsBuffering5 = -1;
    int colorSRC_A_Op5 = -1;
    int colorSRC_A_Src5 = -1;
    int colorSRC_B_Op5 = -1;
    int colorSRC_B_Src5 = -1;
    int colorSRC_C_Op5 = -1;
    int colorSRC_C_Src5 = -1;
    float3 paddingCE5;
    
    
    int alphaCombinerEquation0 = -1;
    int alphaCombinerScale0 = -1;
    bool alphaIsBuffering0 = -1;
    int alphaSRC_A_Op0 = -1;
    int alphaSRC_A_Src0 = -1;
    int alphaSRC_B_Op0 = -1;
    int alphaSRC_B_Src0 = -1;
    int alphaSRC_C_Op0 = -1;
    int alphaSRC_C_Src0 = -1;
    float3 paddingAE0;
    
    int alphaCombinerEquation1 = -1;
    int alphaCombinerScale1 = -1;
    bool alphaIsBuffering1 = -1;
    int alphaSRC_A_Op1 = -1;
    int alphaSRC_A_Src1 = -1;
    int alphaSRC_B_Op1 = -1;
    int alphaSRC_B_Src1 = -1;
    int alphaSRC_C_Op1 = -1;
    int alphaSRC_C_Src1 = -1;
    float3 paddingAE1;
    
    int alphaCombinerEquation2 = -1;
    int alphaCombinerScale2 = -1;
    bool alphaIsBuffering2 = -1;
    int alphaSRC_A_Op2 = -1;
    int alphaSRC_A_Src2 = -1;
    int alphaSRC_B_Op2 = -1;
    int alphaSRC_B_Src2 = -1;
    int alphaSRC_C_Op2 = -1;
    int alphaSRC_C_Src2 = -1;
    float3 paddingAE2;
    
    int alphaCombinerEquation3 = -1;
    int alphaCombinerScale3 = -1;
    bool alphaIsBuffering3 = -1;
    int alphaSRC_A_Op3 = -1;
    int alphaSRC_A_Src3 = -1;
    int alphaSRC_B_Op3 = -1;
    int alphaSRC_B_Src3 = -1;
    int alphaSRC_C_Op3 = -1;
    int alphaSRC_C_Src3 = -1;
    float3 paddingAE3;
    
    int alphaCombinerEquation4 = -1;
    int alphaCombinerScale4 = -1;
    bool alphaIsBuffering4 = -1;
    int alphaSRC_A_Op4 = -1;
    int alphaSRC_A_Src4 = -1;
    int alphaSRC_B_Op4 = -1;
    int alphaSRC_B_Src4 = -1;
    int alphaSRC_C_Op4 = -1;
    int alphaSRC_C_Src4 = -1;
    float3 paddingAE4;
    
    int alphaCombinerEquation5 = -1;
    int alphaCombinerScale5 = -1;
    bool alphaIsBuffering5 = -1;
    int alphaSRC_A_Op5 = -1;
    int alphaSRC_A_Src5 = -1;
    int alphaSRC_B_Op5 = -1;
    int alphaSRC_B_Src5 = -1;
    int alphaSRC_C_Op5 = -1;
    int alphaSRC_C_Src5 = -1;
    float3 paddingAE5;
    
    //int ColorCombinerEquation[6] = { -1, -1, -1, -1, -1, -1 };
    //bool IsBufferingAry[6] = { false, false, false, false, false, false };
    //int CVS[6][3][2] = { { { -1, -1 }, { -1, -1 }, { -1, -1 } }, { { -1, -1 }, { -1, -1 }, { -1, -1 } }, { { -1, -1 }, { -1, -1 }, { -1, -1 } }, { { -1, -1 }, { -1, -1 }, { -1, -1 } }, { { -1, -1 }, { -1, -1 }, { -1, -1 } }, { { -1, -1 }, { -1, -1 }, { -1, -1 } } };

    bool bAutoTengent;
    bool bHasDisplacementMap = false;
    bool bRenderPBR = false;
    bool bRenderFlat = false; //Enable flat normal rendering
    float sMaterialShininess = 1.0f; //Ps := surface material's shininess

    float4 displacementMapScaleMask = float4(0, 0, 0, 1);
    float4 uvTransformR10;
    float4 uvTransformR20;
    float4 uvTransformR11;
    float4 uvTransformR21;
    float4 uvTransformR12;
    float4 uvTransformR22;

    float vertColorBlending;
    float3 padding4;
};
#endif

//cbuffer TextureCombinerStageColor
//{
//    int colorCombinerEquation0 = -1;
//    int colorCombinerScale0 = -1;
//    bool colorIsBuffering0 = -1;
//    int colorSRC_A_Op0 = -1;
//    int colorSRC_A_Src0 = -1;
//    int colorSRC_B_Op0 = -1;
//    int colorSRC_B_Src0 = -1;
//    int colorSRC_C_Op0 = -1;
//    int colorSRC_C_Src0 = -1;
    
//    int colorCombinerEquation1 = -1;
//    int colorCombinerScale1 = -1;
//    bool colorIsBuffering1 = -1;
//    int colorSRC_A_Op1 = -1;
//    int colorSRC_A_Src1 = -1;
//    int colorSRC_B_Op1 = -1;
//    int colorSRC_B_Src1 = -1;
//    int colorSRC_C_Op1 = -1;
//    int colorSRC_C_Src1 = -1;
    
//    int colorCombinerEquation2 = -1;
//    int colorCombinerScale2 = -1;
//    bool colorIsBuffering2 = -1;
//    int colorSRC_A_Op2 = -1;
//    int colorSRC_A_Src2 = -1;
//    int colorSRC_B_Op2 = -1;
//    int colorSRC_B_Src2 = -1;
//    int colorSRC_C_Op2 = -1;
//    int colorSRC_C_Src2 = -1;
    
//    int colorCombinerEquation3 = -1;
//    int colorCombinerScale3 = -1;
//    bool colorIsBuffering3 = -1;
//    int colorSRC_A_Op3 = -1;
//    int colorSRC_A_Src3 = -1;
//    int colorSRC_B_Op3 = -1;
//    int colorSRC_B_Src3 = -1;
//    int colorSRC_C_Op3 = -1;
//    int colorSRC_C_Src3 = -1;
    
//    int colorCombinerEquation4 = -1;
//    int colorCombinerScale4 = -1;
//    bool colorIsBuffering4 = -1;
//    int colorSRC_A_Op4 = -1;
//    int colorSRC_A_Src4 = -1;
//    int colorSRC_B_Op4 = -1;
//    int colorSRC_B_Src4 = -1;
//    int colorSRC_C_Op4 = -1;
//    int colorSRC_C_Src4 = -1;
    
//    int colorCombinerEquation5 = -1;
//    int colorCombinerScale5 = -1;
//    bool colorIsBuffering5 = -1;
//    int colorSRC_A_Op5 = -1;
//    int colorSRC_A_Src5 = -1;
//    int colorSRC_B_Op5 = -1;
//    int colorSRC_B_Src5 = -1;
//    int colorSRC_C_Op5 = -1;
//    int colorSRC_C_Src5 = -1;
//};

//cbuffer TextureCombinerStageAlpha
//{
//    int alphaCombinerEquation0 = -1;
//    int alphaCombinerScale0 = -1;
//    bool alphaIsBuffering0 = -1;
//    int alphaSRC_A_Op0 = -1;
//    int alphaSRC_A_Src0 = -1;
//    int alphaSRC_B_Op0 = -1;
//    int alphaSRC_B_Src0 = -1;
//    int alphaSRC_C_Op0 = -1;
//    int alphaSRC_C_Src0 = -1;
    
//    int alphaCombinerEquation1 = -1;
//    int alphaCombinerScale1 = -1;
//    bool alphaIsBuffering1 = -1;
//    int alphaSRC_A_Op1 = -1;
//    int alphaSRC_A_Src1 = -1;
//    int alphaSRC_B_Op1 = -1;
//    int alphaSRC_B_Src1 = -1;
//    int alphaSRC_C_Op1 = -1;
//    int alphaSRC_C_Src1 = -1;
    
//    int alphaCombinerEquation2 = -1;
//    int alphaCombinerScale2 = -1;
//    bool alphaIsBuffering2 = -1;
//    int alphaSRC_A_Op2 = -1;
//    int alphaSRC_A_Src2 = -1;
//    int alphaSRC_B_Op2 = -1;
//    int alphaSRC_B_Src2 = -1;
//    int alphaSRC_C_Op2 = -1;
//    int alphaSRC_C_Src2 = -1;
    
//    int alphaCombinerEquation3 = -1;
//    int alphaCombinerScale3 = -1;
//    bool alphaIsBuffering3 = -1;
//    int alphaSRC_A_Op3 = -1;
//    int alphaSRC_A_Src3 = -1;
//    int alphaSRC_B_Op3 = -1;
//    int alphaSRC_B_Src3 = -1;
//    int alphaSRC_C_Op3 = -1;
//    int alphaSRC_C_Src3 = -1;
    
//    int alphaCombinerEquation4 = -1;
//    int alphaCombinerScale4 = -1;
//    bool alphaIsBuffering4 = -1;
//    int alphaSRC_A_Op4 = -1;
//    int alphaSRC_A_Src4 = -1;
//    int alphaSRC_B_Op4 = -1;
//    int alphaSRC_B_Src4 = -1;
//    int alphaSRC_C_Op4 = -1;
//    int alphaSRC_C_Src4 = -1;
    
//    int alphaCombinerEquation5 = -1;
//    int alphaCombinerScale5 = -1;
//    bool alphaIsBuffering5 = -1;
//    int alphaSRC_A_Op5 = -1;
//    int alphaSRC_A_Src5 = -1;
//    int alphaSRC_B_Op5 = -1;
//    int alphaSRC_B_Src5 = -1;
//    int alphaSRC_C_Op5 = -1;
//    int alphaSRC_C_Src5 = -1;
//};



//--------------Texture--------------

//Texture2D Textures[4] : register(t63);

//Texture2D AlphaMaps[4] : register(t67);

#if defined(CGFXMESH)
Texture2D Texture0 : register(t63);
Texture2D Texture1 : register(t64);
Texture2D Texture2 : register(t65);
Texture2D Texture3 : register(t66);

//10, 11, 12, 13
SamplerState samplerSurface0 : register(s10);
SamplerState samplerSurface1 : register(s11);
SamplerState samplerSurface2 : register(s12);
SamplerState samplerSurface3 : register(s13);

Texture2D AlphaMap0 : register(t67);
Texture2D AlphaMap1 : register(t68);
Texture2D AlphaMap2 : register(t69);
Texture2D AlphaMap3 : register(t70);

Texture2D<float3> NormalMap : register(t71);
Texture2D<float3> SpeculerMap : register(t72);
#endif


#endif
