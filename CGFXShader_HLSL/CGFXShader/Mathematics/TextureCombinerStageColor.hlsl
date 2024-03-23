#ifndef TEXCOMBINERSTAGECOLOR_HLSL
#define TEXCOMBINERSTAGECOLOR_HLSL
#pragma pack_matrix( row_major )

#define CGFXMESH

#include"..\Common\CGFXBuffer.hlsl"
#include"../Common\ConstantValues.hlsl"

float3 TextureCombinerSOURCE(int source, float3 inputDiffuse, float4 vtColor, float4 SpcRefColor, float2 texcoord0, float2 texcoord1, float2 texcoord2, float3 PrevCombinerStage)
{
    float3 f = vMaterialDiffuse.rgb;
    if (source == OUTPUT_VERTEXSHADER)
    {
        f = lerp(inputDiffuse, vtColor.rgb, 1);
    }
    else if (source == PRIMARY_COLOR)
    {
        //Emission + Ambient
        f += vMaterialEmissive.rgb + vMaterialAmbient.rgb;

    }
    else if (source == SECONDARY_COLOR)
    {
        //SpecularColor0, 1
        //f *= lightSurface(inputDiffuse, V, N, vMaterialDiffuse, vMaterialSpecular, shininess, reflectColor);
        f *= SpcRefColor.rgb;
    }
    else if (source == TEXTURE0)
    {
        f = Texture0.Sample(samplerSurface, texcoord0).rgb;
    }
    else if (source == TEXTURE1)
    {
        f = Texture1.Sample(samplerSurface, texcoord1).rgb;
    }
    else if (source == TEXTURE2)
    {
        f = Texture2.Sample(samplerSurface, texcoord2).rgb;
    }
    else if (source == TEXTURE3)
    {
        f = Texture3.Sample(samplerSurface, texcoord0).rgb;
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
        f *= PrevCombinerStage;
    }
    
    return f;
}

float3 TextureCombinerOPERAND(int operand, float3 inputSOURCE)
{
    float3 output = inputSOURCE.rgb;
    if (operand == OPERAND_COLOR_RGB)
    {
        output.rgb = inputSOURCE.rgb;
    }
    else if (operand == OPERAND_COLOR_ONE_SUB_RGB)
    {
        output.rgb = 1 - inputSOURCE.rgb;
    }
    else if (operand == OPERAND_COLOR_A)
    {
        output = inputSOURCE;
    }
    else if (operand == OPERAND_COLOR_ONE_SUB_A)
    {
        output = 1 - inputSOURCE;
    }
    else if (operand == OPERAND_COLOR_R)
    {
        output.r = inputSOURCE.r;
    }
    else if (operand == OPERAND_COLOR_ONE_SUB_R)
    {
        output.r = 1 - inputSOURCE.r;
    }
    else if (operand == OPERAND_COLOR_G)
    {
        output.g = inputSOURCE.g;
    }
    else if (operand == OPERAND_COLOR_ONE_SUB_G)
    {
        output.g = 1 - inputSOURCE.g;
    }
    else if (operand == OPERAND_COLOR_B)
    {
        output.b = inputSOURCE.b;
    }
    else if (operand == OPERAND_COLOR_ONE_SUB_B)
    {
        output.b = 1 - inputSOURCE.b;
    }
    
    return output;
}

float3 TextureCombinerStage(float3 inputDiffuse, float4 SpcRefColor, float4 vtColorInput, float2 texcoord0, float2 texcoord1, float2 texcoord2, float3 PrevSteps, int stageType, int SRC_A, int OPERND_A, int SRC_B, int OPERND_B, int SRC_C, int OPERND_C, int Scale)
{
    //#define A 0
    //#define A_MULTIPLY_B 1
    //#define A_ADD_B 2
    //#define A_ADD_B_SUB_NOUGHT_PT_FIVE 3
    //#define A_MULTIPLY_C_ADD_B_MULTIPLY_BRKT_ONE_SUB_C_BRKT 4
    //#define A_SUB_B 5
    //#define RGB_DOT_BRKT_A_B_BRKT 6
    //#define RGBA_DOT_BRKT_A_B_BRKT 7
    //#define BRKT_A_ADD_B_BRKT_MULTIPLY_C 8
    //#define BRKT_A_MULTIPLY_B_BRKT_ADD_C 9
    
    //float4 SRC_OUT_A = TextureCombinerSOURCE(SRC_A, input, texcoord) + TextureCombinerOPERAND(OPERND_A, input);
    //float4 SRC_OUT_B = TextureCombinerSOURCE(SRC_B, input, texcoord) + TextureCombinerOPERAND(OPERND_B, input);
    //float4 SRC_OUT_C = TextureCombinerSOURCE(SRC_C, input, texcoord) + TextureCombinerOPERAND(OPERND_C, input);
    
    float3 SRC_OUT_A = TextureCombinerOPERAND(OPERND_A, TextureCombinerSOURCE(SRC_A, inputDiffuse, vtColorInput, SpcRefColor, texcoord0, texcoord1, texcoord2, PrevSteps));
    float3 SRC_OUT_B = TextureCombinerOPERAND(OPERND_B, TextureCombinerSOURCE(SRC_B, inputDiffuse, vtColorInput, SpcRefColor, texcoord0, texcoord1, texcoord2, PrevSteps));
    float3 SRC_OUT_C = TextureCombinerOPERAND(OPERND_C, TextureCombinerSOURCE(SRC_C, inputDiffuse, vtColorInput, SpcRefColor, texcoord0, texcoord1, texcoord2, PrevSteps));
    
    float3 f = vMaterialDiffuse.rgb;
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
        f.rgb = dot(SRC_OUT_A.rgb, SRC_OUT_B.rgb);
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


//float4 TextureCombinerSOURCE(int source, float4 inputDiffuse, float4 vtColor, float4 SpcRefColor, float2 texcoord0, float2 texcoord1, float2 texcoord2, float4 PrevCombinerStage)
//{
//    float4 f = vMaterialDiffuse;
//    if (source == OUTPUT_VERTEXSHADER)
//    {
//        f = lerp(inputDiffuse, vtColor, 1);
//    }
//    else if (source == PRIMARY_COLOR)
//    {
//        //Emission + Ambient
//        f += vMaterialEmissive + vMaterialAmbient;

//    }
//    else if (source == SECONDARY_COLOR)
//    {
//        //SpecularColor0, 1
//        //f *= lightSurface(inputDiffuse, V, N, vMaterialDiffuse, vMaterialSpecular, shininess, reflectColor);
//        f *= SpcRefColor;
//    }
//    else if (source == TEXTURE0)
//    {
//        f = Texture0.Sample(samplerSurface, texcoord0);
//    }
//    else if (source == TEXTURE1)
//    {
//        f = Texture1.Sample(samplerSurface, texcoord1);
//    }
//    else if (source == TEXTURE2)
//    {
//        f = Texture2.Sample(samplerSurface, texcoord2);
//    }
//    else if (source == TEXTURE3)
//    {
//        f = Texture3.Sample(samplerSurface, texcoord0);
//    }
//    else if (source == PREVIOUS_STEP_BUFFER)
//    {
//        //Buffer Setting
//    }
//    else if (source == CONSTANTCOLOR)
//    {
//        //StageCount => ConstantColor
//        //inputDiffuse = ConstantColor0;
//    }
//    else if (source == PREVIOUS_STEP_RESULT)
//    {
//        f = PrevCombinerStage;
//    }
    
//    return f;
//}

//float4 TextureCombinerOPERAND(int operand, float4 inputSOURCE)
//{
//    float4 output = inputSOURCE.rgba;
//    if (operand == OPERAND_COLOR_RGB)
//    {
//        output.rgb = inputSOURCE.rgb;
//    }
//    else if (operand == OPERAND_COLOR_ONE_SUB_RGB)
//    {
//        output.rgb = 1 - inputSOURCE.rgb;
//    }
//    else if (operand == OPERAND_COLOR_A)
//    {
//        output.a = inputSOURCE.a;
//    }
//    else if (operand == OPERAND_COLOR_ONE_SUB_A)
//    {
//        output.a = 1 - inputSOURCE.a;
//    }
//    else if (operand == OPERAND_COLOR_R)
//    {
//        output.r = inputSOURCE.r;
//    }
//    else if (operand == OPERAND_COLOR_ONE_SUB_R)
//    {
//        output.r = 1 - inputSOURCE.r;
//    }
//    else if (operand == OPERAND_COLOR_G)
//    {
//        output.g = inputSOURCE.g;
//    }
//    else if (operand == OPERAND_COLOR_ONE_SUB_G)
//    {
//        output.g = 1 - inputSOURCE.g;
//    }
//    else if (operand == OPERAND_COLOR_B)
//    {
//        output.b = inputSOURCE.b;
//    }
//    else if (operand == OPERAND_COLOR_ONE_SUB_B)
//    {
//        output.b = 1 - inputSOURCE.b;
//    }
    
//    return output;
//}

//float4 TextureCombinerStage(float4 inputDiffuse, float4 SpcRefColor, float4 vtColorInput, float2 texcoord0, float2 texcoord1, float2 texcoord2, float4 PrevSteps, int stageType, int SRC_A, int OPERND_A, int SRC_B, int OPERND_B, int SRC_C, int OPERND_C, int Scale)
//{
//    //#define A 0
//    //#define A_MULTIPLY_B 1
//    //#define A_ADD_B 2
//    //#define A_ADD_B_SUB_NOUGHT_PT_FIVE 3
//    //#define A_MULTIPLY_C_ADD_B_MULTIPLY_BRKT_ONE_SUB_C_BRKT 4
//    //#define A_SUB_B 5
//    //#define RGB_DOT_BRKT_A_B_BRKT 6
//    //#define RGBA_DOT_BRKT_A_B_BRKT 7
//    //#define BRKT_A_ADD_B_BRKT_MULTIPLY_C 8
//    //#define BRKT_A_MULTIPLY_B_BRKT_ADD_C 9
    
//    //float4 SRC_OUT_A = TextureCombinerSOURCE(SRC_A, input, texcoord) + TextureCombinerOPERAND(OPERND_A, input);
//    //float4 SRC_OUT_B = TextureCombinerSOURCE(SRC_B, input, texcoord) + TextureCombinerOPERAND(OPERND_B, input);
//    //float4 SRC_OUT_C = TextureCombinerSOURCE(SRC_C, input, texcoord) + TextureCombinerOPERAND(OPERND_C, input);
    
//    float4 SRC_OUT_A = TextureCombinerOPERAND(OPERND_A, TextureCombinerSOURCE(SRC_A, inputDiffuse, vtColorInput, SpcRefColor, texcoord0, texcoord1, texcoord2, PrevSteps));
//    float4 SRC_OUT_B = TextureCombinerOPERAND(OPERND_B, TextureCombinerSOURCE(SRC_B, inputDiffuse, vtColorInput, SpcRefColor, texcoord0, texcoord1, texcoord2, PrevSteps));
//    float4 SRC_OUT_C = TextureCombinerOPERAND(OPERND_C, TextureCombinerSOURCE(SRC_C, inputDiffuse, vtColorInput, SpcRefColor, texcoord0, texcoord1, texcoord2, PrevSteps));
    
//    float4 f = vMaterialDiffuse;
//    if (stageType == A)
//    {
//        f = SRC_OUT_A; //larp
//    }
//    else if (stageType == A_MULTIPLY_B)
//    {
//        f = SRC_OUT_A * SRC_OUT_B;
//    }
//    else if (stageType == A_ADD_B)
//    {
//        f = SRC_OUT_A + SRC_OUT_B;
//    }
//    else if (stageType == A_ADD_B_SUB_NOUGHT_PT_FIVE)
//    {
//        f = SRC_OUT_A + SRC_OUT_B - 0.5;
//    }
//    else if (stageType == A_MULTIPLY_C_ADD_B_MULTIPLY_BRKT_ONE_SUB_C_BRKT)
//    {
//        f = SRC_OUT_A * SRC_OUT_C + SRC_OUT_B * (1 - SRC_OUT_C);
//    }
//    else if (stageType == A_SUB_B)
//    {
//        f = SRC_OUT_A - SRC_OUT_B;
//    }
//    else if (stageType == RGB_DOT_BRKT_A_B_BRKT)
//    {
//        f.rgb = dot(SRC_OUT_A.rgb, SRC_OUT_B.rgb);
//    }
//    else if (stageType == RGBA_DOT_BRKT_A_B_BRKT)
//    {
//        f = dot(SRC_OUT_A, SRC_OUT_B);
//    }
//    else if (stageType == BRKT_A_ADD_B_BRKT_MULTIPLY_C)
//    {
//        f = (SRC_OUT_A + SRC_OUT_B) * SRC_OUT_C;
//    }
//    else if (stageType == BRKT_A_MULTIPLY_B_BRKT_ADD_C)
//    {
//        f = (SRC_OUT_A * SRC_OUT_B) + SRC_OUT_C;
//    }
    
//    return f;
//    //return input * Scale;
//}

#endif