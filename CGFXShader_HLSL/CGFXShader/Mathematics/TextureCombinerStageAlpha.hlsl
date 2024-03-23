#ifndef TEXCOMBINERSTAGEALPHA_HLSL
#define TEXCOMBINERSTAGEALPHA_HLSL
#pragma pack_matrix( row_major )

#define CGFXMESH

#include"..\Common\CGFXBuffer.hlsl"
#include"../Common\ConstantValues.hlsl"
#include"../Common\CGFXDataStruct.hlsl"

float TextureCombinerSOURCE_Alpha(int source, float4 inputDiffuse, float4 vtColor, float4 SpcRefColor, float2 texcoord0, float2 texcoord1, float2 texcoord2, float PrevCombinerStage)
{
    float f = vMaterialDiffuse.a;
    if (source == OUTPUT_VERTEXSHADER)
    {
        f = lerp(inputDiffuse, vtColor, 1).a;
    }
    else if (source == PRIMARY_COLOR)
    {
        //Emission + Ambient
        //f = vMaterialEmissive.a + vMaterialAmbient.a;
        f = vMaterialEmissive.a * vMaterialAmbient.a;
    }
    else if (source == SECONDARY_COLOR)
    {
        //SpecularColor0, 1
        //f *= lightSurface(inputDiffuse, V, N, vMaterialDiffuse, vMaterialSpecular, shininess, reflectColor);
        f = SpcRefColor.a;
    }
    else if (source == TEXTURE0)
    {
        f = AlphaMap0.Sample(samplerSurface, texcoord0).a;
    }
    else if (source == TEXTURE1)
    {
        f = AlphaMap1.Sample(samplerSurface, texcoord1).a;
    }
    else if (source == TEXTURE2)
    {
        f = AlphaMap2.Sample(samplerSurface, texcoord2).a;
    }
    else if (source == TEXTURE3)
    {
        f = AlphaMap3.Sample(samplerSurface, texcoord0).a;
    }
    else if (source == PREVIOUS_STEP_BUFFER)
    {
        //Buffer Setting
    }
    else if (source == CONSTANTCOLOR)
    {
        //StageCount => ConstantColor
        //inputDiffuse = ConstantColor0;
    }
    else if (source == PREVIOUS_STEP_RESULT)
    {
        f += PrevCombinerStage;
    }
    
    return f;
}

float3 TextureCombinerOPERAND_Alpha(int operand, float3 inputSOURCE)
{
    float output = inputSOURCE;
    if (operand == OPERAND_COLOR_A)
    {
        output = inputSOURCE;
    }
    else if (operand == OPERAND_COLOR_ONE_SUB_A)
    {
        output = 1 - inputSOURCE;
    }
    else if (operand == OPERAND_COLOR_R)
    {
        output = inputSOURCE.r;
    }
    else if (operand == OPERAND_COLOR_ONE_SUB_R)
    {
        output = 1 - inputSOURCE.r;
    }
    else if (operand == OPERAND_COLOR_G)
    {
        output = inputSOURCE.g;
    }
    else if (operand == OPERAND_COLOR_ONE_SUB_G)
    {
        output = 1 - inputSOURCE.g;
    }
    else if (operand == OPERAND_COLOR_B)
    {
        output = inputSOURCE.b;
    }
    else if (operand == OPERAND_COLOR_ONE_SUB_B)
    {
        output = 1 - inputSOURCE.b;
    }
    
    return output;
}

float TextureCombinerStageAlpha(float inputDiffuse, float4 SpcRefColor, float4 vtColorInput, float2 texcoord0, float2 texcoord1, float2 texcoord2, float PrevSteps, int stageType, int SRC_A, int OPERND_A, int SRC_B, int OPERND_B, int SRC_C, int OPERND_C, int Scale)
{

    float SRC_OUT_A = TextureCombinerOPERAND_Alpha(OPERND_A, TextureCombinerSOURCE_Alpha(SRC_A, inputDiffuse, vtColorInput, SpcRefColor, texcoord0, texcoord1, texcoord2, PrevSteps));
    float SRC_OUT_B = TextureCombinerOPERAND_Alpha(OPERND_B, TextureCombinerSOURCE_Alpha(SRC_B, inputDiffuse, vtColorInput, SpcRefColor, texcoord0, texcoord1, texcoord2, PrevSteps));
    float SRC_OUT_C = TextureCombinerOPERAND_Alpha(OPERND_C, TextureCombinerSOURCE_Alpha(SRC_C, inputDiffuse, vtColorInput, SpcRefColor, texcoord0, texcoord1, texcoord2, PrevSteps));
    
    float4 f = vMaterialDiffuse.a;
    if (stageType == A)
    {
        f = SRC_OUT_A; //larp
    }
    else if (stageType == A_MULTIPLY_B)
    {
        f = SRC_OUT_A * SRC_OUT_B;
    }
    else if (stageType == A_ADD_B)
    {
        f = SRC_OUT_A + SRC_OUT_B;
    }
    else if (stageType == A_ADD_B_SUB_NOUGHT_PT_FIVE)
    {
        f = SRC_OUT_A + SRC_OUT_B - 0.5;
    }
    else if (stageType == A_MULTIPLY_C_ADD_B_MULTIPLY_BRKT_ONE_SUB_C_BRKT)
    {
        f = SRC_OUT_A * SRC_OUT_C + SRC_OUT_B * (1 - SRC_OUT_C);
    }
    else if (stageType == A_SUB_B)
    {
        f = SRC_OUT_A - SRC_OUT_B;
    }
    else if (stageType == RGB_DOT_BRKT_A_B_BRKT)
    {
        f.rgb = dot(SRC_OUT_A, SRC_OUT_B);
    }
    else if (stageType == RGBA_DOT_BRKT_A_B_BRKT)
    {
        f = dot(SRC_OUT_A, SRC_OUT_B);
    }
    else if (stageType == BRKT_A_ADD_B_BRKT_MULTIPLY_C)
    {
        f = (SRC_OUT_A + SRC_OUT_B) * SRC_OUT_C;
    }
    else if (stageType == BRKT_A_MULTIPLY_B_BRKT_ADD_C)
    {
        f = (SRC_OUT_A * SRC_OUT_B) + SRC_OUT_C;
    }
    
    return f;
    //return input * Scale;
}

#endif