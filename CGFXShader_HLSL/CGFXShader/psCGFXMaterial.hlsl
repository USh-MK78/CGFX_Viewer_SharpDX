#ifndef PSMESHCGFX_HLSL
#define PSMESHCGFX_HLSL

#define CGFXMESH //CGFXBuffer.hlsl => defined(DEFINEDNAME)
#define CLIPPLANE

#pragma pack_matrix( row_major )
#include"Common\Common.hlsl"
#include"Common\psCommon.hlsl"

#include"Common\CommonBuffers.hlsl"

#include"Common\CGFXBuffer.hlsl"
#include"Common\CGFXDataStruct.hlsl"
#include"Common\ConstantValues.hlsl"
#include"Mathematics\Combiner.hlsl"
#include"Mathematics\TextureCombinerStageColor.hlsl"
#include"Mathematics\TextureCombinerStageAlpha.hlsl"



//--------------------------------------------------------------------------------------
// normal mapping
//--------------------------------------------------------------------------------------
// This function returns the normal in world coordinates.
// The input struct contains tangent (t1), bitangent (t2) and normal (n) of the
// unperturbed surface in world coordinates. The perturbed normal in tangent space
// can be read from texNormalMap.
// The RGB values in this texture need to be normalized from (0, +1) to (-1, +1).
float3 calcNormal(PSInputCGFX input)
{
    float3 normal = bRenderFlat ? normalize(cross(ddy(input.ps_vEye1.xyz), ddx(input.ps_vEye1.xyz))) : normalize(input.ps_Nr);
    if (bIsFragmentLighting) //ETC1Encoding.HILO8
    {
        if (bAutoTengent)
        {
            float3 localNormal = BiasX2(texNormalMap.Sample(samplerSurface, input.ps_t0).xyz);
            normal = PeturbNormal(localNormal, input.ps_vEye1.xyz, normal, input.ps_t0);
        }
        else
        {
            // Normalize the per-pixel interpolated tangent-space
            float3 tangent = normalize(input.ps_Tr);
            float3 biTangent = normalize(input.ps_BiTr);

            // Sample the texel in the bump map.
            float3 bumpMap = texNormalMap.Sample(samplerSurface, input.ps_t0);
            // Expand the range of the normal value from (0, +1) to (-1, +1).
            bumpMap = mad(2.0f, bumpMap, -1.0f);
            // Calculate the normal from the data in the bump map.
            normal += mad(bumpMap.x, tangent, bumpMap.y * biTangent);
            normal = normalize(normal);
        }
    }
    return normal;
}


//--------------------------------------------------------------------------------------
// Blinn-Phong Lighting Reflection Model
//--------------------------------------------------------------------------------------
// Returns the sum of the diffuse and specular terms in the Blinn-Phong reflection model.
float4 calcBlinnPhongLighting(float4 LColor, float3 N, float4 diffuse, float3 L, float3 H, float4 specular, float shininess)
{
    //float4 Id = vMaterialTexture * diffuse * saturate(dot(N, L));
    //float4 Is = vMaterialSpecular * pow(saturate(dot(N, H)), sMaterialShininess);
    float4 f = lit(dot(N, L), dot(N, H), shininess);
    float4 Id = f.y * diffuse;
    float4 Is = min(f.z, diffuse.w) * specular;
    return (Id + Is) * LColor;
}


//--------------------------------------------------------------------------------------
// reflectance mapping
//--------------------------------------------------------------------------------------
float3 cubeMapReflection(float4 wp, float3 n, const in float3 I, const in float3 reflectColor)
{
    float3 v = normalize((float3)wp - vEyePos);
    float3 r = reflect(v, n);
    return (1.0f - reflectColor) * I + reflectColor * texCubeMap.Sample(samplerCube, r);
}

float4 lightSurface(float4 wp, in float3 V, in float3 N, float4 diffuse, float4 specular, float shininess, float4 reflectColor)
{
    float4 acc_color = 0;
    // compute lighting
    for (int i = 0; i < NumLights; ++i)
    {
        if (Lights[i].iLightType == 1) // directional
        {
            float3 d = normalize((float3) Lights[i].vLightDir); // light dir	
            float3 h = normalize(V + d);
            acc_color += calcBlinnPhongLighting(Lights[i].vLightColor, N, diffuse, d, h, specular, shininess);
        }
        else if (Lights[i].iLightType == 2)  // point
        {
            float3 d = (float3) (Lights[i].vLightPos - wp); // light dir
            float dl = length(d); // light distance
            if (Lights[i].vLightAtt.w < dl)
            {
                continue;
            }
            d = d / dl; // normalized light dir						
            float3 h = normalize(V + d); // half direction for specular
            float att = 1.0f / (Lights[i].vLightAtt.x + Lights[i].vLightAtt.y * dl + Lights[i].vLightAtt.z * dl * dl);
            acc_color = mad(att, calcBlinnPhongLighting(Lights[i].vLightColor, N, diffuse, d, h, specular, shininess), acc_color);
        }
        else if (Lights[i].iLightType == 3)  // spot
        {
            float3 d = (float3) (Lights[i].vLightPos - wp); // light dir
            float dl = length(d); // light distance
            if (Lights[i].vLightAtt.w < dl)
            {
                continue;
            }
            d = d / dl; // normalized light dir					
            float3 h = normalize(V + d); // half direction for specular
            float3 sd = normalize((float3) Lights[i].vLightDir); // missuse the vLightDir variable for spot-dir

            /* --- this is the OpenGL 1.2 version (not so nice) --- */
            //float spot = (dot(-d, sd));
            //if(spot > cos(vLightSpot[i].x))
            //	spot = pow( spot, vLightSpot[i].y );
            //else
            //	spot = 0.0f;	
            /* --- */

            /* --- this is the  DirectX9 version (better) --- */
            float rho = dot(-d, sd);
            float spot = pow(saturate((rho - Lights[i].vLightSpot.x) / (Lights[i].vLightSpot.y - Lights[i].vLightSpot.x)), Lights[i].vLightSpot.z);
            float att = spot / (Lights[i].vLightAtt.x + Lights[i].vLightAtt.y * dl + Lights[i].vLightAtt.z * dl * dl);
            acc_color = mad(att, calcBlinnPhongLighting(Lights[i].vLightColor, N, diffuse, d, h, specular, shininess), acc_color);
        }
    }
    //// multiply by vertex colors
    ////I = I * input.c;
    //// get reflection-color
    //if (bHasCubeMap)
    //{
    //    acc_color.rgb = cubeMapReflection(wp, N, acc_color.rgb, reflectColor.rgb);
    //}
    acc_color.a = diffuse.a;
    return saturate(acc_color);
}



//float4 TextureCombinerSOURCE(int source, float4 input, float4 vtColor, float2 texcoord)
//{
//    if (source == OUTPUT_VERTEXSHADER)
//    {
//        input = lerp(input, vtColor, 1);
//    }
//    else if (source == PRIMARY_COLOR)
//    {
        
//    }
//    else if (source == SECONDARY_COLOR)
//    {
        
//    }
//    else if (source == TEXTURE0)
//    {
//        input = Texture0.Sample(samplerSurface, texcoord);
//    }
//    else if (source == TEXTURE1)
//    {
//        input = Texture1.Sample(samplerSurface, texcoord);
//    }
//    else if (source == TEXTURE2)
//    {
//        input = Texture2.Sample(samplerSurface, texcoord);
//    }
//    else if (source == TEXTURE3)
//    {
//        input = Texture3.Sample(samplerSurface, texcoord);
//    }
//    else if (source == PREVIOUS_STEP_BUFFER)
//    {
        
//    }
//    else if (source == CONSTANTCOLOR)
//    {
//        //StageCount => ConstantColor

//        input = ConstantColor0;
//    }
//    else if (source == PREVIOUS_STEP_RESULT)
//    {
        
//    }
    
//    return input;
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
//        output.g = output.g;
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
//        output.b - 1 - inputSOURCE.b;
//    }
    
//    return output;
//}

//float4 TextureCombinerStage(float4 input, float4 vtColorInput, float2 texcoord, int stageType, int SRC_A, int OPERND_A, int SRC_B, int OPERND_B, int SRC_C, int OPERND_C, int Scale)
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
    
//    float4 SRC_OUT_A = TextureCombinerOPERAND(OPERND_A, TextureCombinerSOURCE(SRC_A, input, vtColorInput, texcoord));
//    float4 SRC_OUT_B = TextureCombinerOPERAND(OPERND_B, TextureCombinerSOURCE(SRC_B, input, vtColorInput, texcoord));
//    float4 SRC_OUT_C = TextureCombinerOPERAND(OPERND_C, TextureCombinerSOURCE(SRC_C, input, vtColorInput, texcoord));
    
//    if (stageType == A)
//    {
//        input = SRC_OUT_A;
//    }
//    else if (stageType == A_MULTIPLY_B)
//    {
//        input = SRC_OUT_A * SRC_B;
//    }
//    else if (stageType == A_ADD_B)
//    {
//        input = SRC_OUT_A + SRC_OUT_B;
//    }
//    else if (stageType == A_ADD_B_SUB_NOUGHT_PT_FIVE)
//    {
//        input = SRC_OUT_A + SRC_OUT_B - 0.5;
//    }
//    else if (stageType == A_MULTIPLY_C_ADD_B_MULTIPLY_BRKT_ONE_SUB_C_BRKT)
//    {
//        input = SRC_OUT_A * SRC_OUT_C + SRC_OUT_B * (1 - SRC_OUT_C);
//    }
//    else if (stageType == A_SUB_B)
//    {
//        input = SRC_OUT_A - SRC_OUT_B;
//    }
//    else if (stageType == RGB_DOT_BRKT_A_B_BRKT)
//    {
        
//    }
//    else if (stageType == RGBA_DOT_BRKT_A_B_BRKT)
//    {
        
//    }
//    else if (stageType == BRKT_A_ADD_B_BRKT_MULTIPLY_C)
//    {
//        input = (SRC_OUT_A + SRC_OUT_B) * SRC_OUT_C;
//    }
//    else if (stageType == BRKT_A_MULTIPLY_B_BRKT_ADD_C)
//    {
//        input = (SRC_OUT_A * SRC_OUT_B) + SRC_OUT_C;
//    }
    
//    return input;
//    //return input * Scale;
//}


static float4 TexCombinerStageBuffer[6];

//--------------------------------------------------------------------------------------
// PER PIXEL LIGHTING - BLINN-PHONG
//--------------------------------------------------------------------------------------
float4 main(PSInputCGFX input) : SV_Target
{
    // renormalize interpolated vectors
    float3 N = calcNormal(input);

    // get per pixel vector to eye-position
    float3 V = normalize(input.ps_vEye0.xyz);

    // add diffuse sampling
    float4 diffuse = input.ps_colorDiffuse;
    
    
    //float shininess = 1.0;
    float shininess = 0.3;
    float4 SpcRefColor = lightSurface(input.ps_vEye1, V, N, diffuse, vMaterialSpecular, shininess, vMaterialReflect);
    
    //VertexColor
    //diffuse += lerp(input.ps_colorDiffuse, input.colorDebug, 1);
    
    //float alpha = 1;
    //if (bHasAlphaMap)
    //{
    //    float4 color = texAlphaMap.Sample(samplerSurface, input.t);
    //    alpha = color.a;
    //    diffuse.rgb *= color.rgb;
    //}
    

    float alphaFactor = 1;
       
    if (bIsFragmentLighting == true)
    {      
        
        float AlphaStage0 = TextureCombinerStageAlpha(input.ps_colorDiffuse.a, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, 0, alphaCombinerEquation0, alphaSRC_A_Src0, alphaSRC_A_Op0, alphaSRC_B_Src0, alphaSRC_B_Op0, alphaSRC_C_Src0, alphaSRC_C_Op0, alphaCombinerScale0);
        float AlphaStage1 = TextureCombinerStageAlpha(input.ps_colorDiffuse.a, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, AlphaStage0, alphaCombinerEquation1, alphaSRC_A_Src1, alphaSRC_A_Op1, alphaSRC_B_Src1, alphaSRC_B_Op1, alphaSRC_C_Src1, alphaSRC_C_Op1, alphaCombinerScale1);
        float AlphaStage2 = TextureCombinerStageAlpha(input.ps_colorDiffuse.a, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, AlphaStage1, alphaCombinerEquation2, alphaSRC_A_Src2, alphaSRC_A_Op2, alphaSRC_B_Src2, alphaSRC_B_Op2, alphaSRC_C_Src2, alphaSRC_C_Op2, alphaCombinerScale2);
        float AlphaStage3 = TextureCombinerStageAlpha(input.ps_colorDiffuse.a, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, AlphaStage2, alphaCombinerEquation3, alphaSRC_A_Src3, alphaSRC_A_Op3, alphaSRC_B_Src3, alphaSRC_B_Op3, alphaSRC_C_Src3, alphaSRC_C_Op3, alphaCombinerScale3);
        float AlphaStage4 = TextureCombinerStageAlpha(input.ps_colorDiffuse.a, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, AlphaStage3, alphaCombinerEquation4, alphaSRC_A_Src4, alphaSRC_A_Op4, alphaSRC_B_Src4, alphaSRC_B_Op4, alphaSRC_C_Src4, alphaSRC_C_Op4, alphaCombinerScale4);
        float AlphaStage5 = TextureCombinerStageAlpha(input.ps_colorDiffuse.a, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, AlphaStage4, alphaCombinerEquation5, alphaSRC_A_Src5, alphaSRC_A_Op5, alphaSRC_B_Src5, alphaSRC_B_Op5, alphaSRC_C_Src5, alphaSRC_C_Op5, alphaCombinerScale5);

        
        float3 Stage0 = TextureCombinerStage(input.ps_colorDiffuse.rgb, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, 0, colorCombinerEquation0, colorSRC_A_Src0, colorSRC_A_Op0, colorSRC_B_Src0, colorSRC_B_Op0, colorSRC_C_Src0, colorSRC_C_Op0, colorCombinerScale0);
        float3 Stage1 = TextureCombinerStage(input.ps_colorDiffuse.rgb, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, Stage0, colorCombinerEquation1, colorSRC_A_Src1, colorSRC_A_Op1, colorSRC_B_Src1, colorSRC_B_Op1, colorSRC_C_Src1, colorSRC_C_Op1, colorCombinerScale1);
        float3 Stage2 = TextureCombinerStage(input.ps_colorDiffuse.rgb, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, Stage1, colorCombinerEquation2, colorSRC_A_Src2, colorSRC_A_Op2, colorSRC_B_Src2, colorSRC_B_Op2, colorSRC_C_Src2, colorSRC_C_Op2, colorCombinerScale2);
        float3 Stage3 = TextureCombinerStage(input.ps_colorDiffuse.rgb, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, Stage2, colorCombinerEquation3, colorSRC_A_Src3, colorSRC_A_Op3, colorSRC_B_Src3, colorSRC_B_Op3, colorSRC_C_Src3, colorSRC_C_Op3, colorCombinerScale3);
        float3 Stage4 = TextureCombinerStage(input.ps_colorDiffuse.rgb, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, Stage3, colorCombinerEquation4, colorSRC_A_Src4, colorSRC_A_Op4, colorSRC_B_Src4, colorSRC_B_Op4, colorSRC_C_Src4, colorSRC_C_Op4, colorCombinerScale4);
        float3 Stage5 = TextureCombinerStage(input.ps_colorDiffuse.rgb, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, Stage4, colorCombinerEquation5, colorSRC_A_Src5, colorSRC_A_Op5, colorSRC_B_Src5, colorSRC_B_Op5, colorSRC_C_Src5, colorSRC_C_Op5, colorCombinerScale5);
        
        //Output
        //color.rgb += emissive.rgb + ambient.rgb;
        diffuse.a += AlphaStage5;
        diffuse.rgb *= Stage5.rgb;
        
        
        //        //alpha=>FrameBuffer
        ////acolor=>Conbiner
        //if (BlendMode == BLEND)
        //{
        //    //diffuse = colorBlend(diffuse, colorBlendEqation, colorBlendSRC, colorBlendDEST) * alphaBlend(alphaColor, alphaBlendEqation, alphaBlendSRC, alphaBlendDEST);
        //    diffuse = Blend(diffuse.rgba, diffuse.a, ColorBlendEqationType, ColorBlendSRC, ColorBlendDEST);
        //    alphaColor = Blend(diffuse.rgba, diffuse.a, AlphaBlendEqationType, AlphaBlendSRC, AlphaBlendDEST);
        //    diffuse *= alphaColor;
        //}
        //else if (BlendMode == SEPARATE)
        //{
        //    diffuse = Blend(diffuse.rgba, alphaColor.rgba, ColorBlendEqationType, ColorBlendSRC, ColorBlendDEST);
        //    alphaColor = Blend(diffuse.rgba, alphaColor.rgba, AlphaBlendEqationType, AlphaBlendSRC, AlphaBlendDEST);
        //    diffuse *= alphaColor;
        //}
        //else if (BlendMode == LOGICAL_OPERATION)
        //{
        //    diffuse = LogicalBlend(diffuse, 0); //type => 
        //}
        //else if (BlendMode == NONE)
        //{
        
        //}
    

        //diffuse = input.ps_colorDiffuse + 1 * (input.colorDebug * input.ps_colorDiffuse); //(?)
        //diffuse = input.ps_colorDiffuse * (input.colorDebug - input.ps_colorDiffuse);
        //diffuse * colorBlendEquation + alpha * alphaBlendEquation
        
        ////BackUp
        //float4 Stage0 = TextureCombinerStage(input.ps_colorDiffuse, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, 0, colorCombinerEquation0, colorSRC_A_Src0, colorSRC_A_Op0, colorSRC_B_Src0, colorSRC_B_Op0, colorSRC_C_Src0, colorSRC_C_Op0, colorCombinerScale0);
        //float4 Stage1 = TextureCombinerStage(input.ps_colorDiffuse, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, Stage0, colorCombinerEquation1, colorSRC_A_Src1, colorSRC_A_Op1, colorSRC_B_Src1, colorSRC_B_Op1, colorSRC_C_Src1, colorSRC_C_Op1, colorCombinerScale1);
        //float4 Stage2 = TextureCombinerStage(input.ps_colorDiffuse, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, Stage1, colorCombinerEquation2, colorSRC_A_Src2, colorSRC_A_Op2, colorSRC_B_Src2, colorSRC_B_Op2, colorSRC_C_Src2, colorSRC_C_Op2, colorCombinerScale2);
        //float4 Stage3 = TextureCombinerStage(input.ps_colorDiffuse, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, Stage2, colorCombinerEquation3, colorSRC_A_Src3, colorSRC_A_Op3, colorSRC_B_Src3, colorSRC_B_Op3, colorSRC_C_Src3, colorSRC_C_Op3, colorCombinerScale3);
        //float4 Stage4 = TextureCombinerStage(input.ps_colorDiffuse, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, Stage3, colorCombinerEquation4, colorSRC_A_Src4, colorSRC_A_Op4, colorSRC_B_Src4, colorSRC_B_Op4, colorSRC_C_Src4, colorSRC_C_Op4, colorCombinerScale4);
        //float4 Stage5 = TextureCombinerStage(input.ps_colorDiffuse, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, Stage4, colorCombinerEquation5, colorSRC_A_Src5, colorSRC_A_Op5, colorSRC_B_Src5, colorSRC_B_Op5, colorSRC_C_Src5, colorSRC_C_Op5, colorCombinerScale5);
        
        
        ////Output
        //diffuse *= Stage3;

        ////All
        //for (int i = 0; i < StageCount; i++)
        //{
        //    diffuse += TexCombinerStageBuffer[i];
        //}
    }
    else if (bIsFragmentLighting == false)
    {
        ////FragmentLighting = false
        float4 t0 = Texture0.Sample(samplerSurface, input.ps_t0);
        float4 t1 = Texture1.Sample(samplerSurface, input.ps_t1);
        float4 t2 = Texture2.Sample(samplerSurface, input.ps_t2);
        float4 t3 = Texture3.Sample(samplerSurface, input.ps_t0);
        
        //Debug(p0)
        diffuse *= t0 + t1 + t2 + t3;

        //float4 t0 = Texture0.Sample(samplerSurface0, input.ps_t0);
        //float4 t1 = Texture1.Sample(samplerSurface1, input.ps_t1);
        //float4 t2 = Texture2.Sample(samplerSurface2, input.ps_t2);
        //float4 t3 = Texture3.Sample(samplerSurface3, input.ps_t0);

        ////Debug(p0)
        //diffuse *= t0 + t1 + t2 + t3;

        ////input.ps_vEye1, VertColorBlendingFactorValue
        //float4 Stage0 = TextureCombinerStage(diffuse, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, 0, colorCombinerEquation0, colorSRC_A_Src0, colorSRC_A_Op0, colorSRC_B_Src0, colorSRC_B_Op0, colorSRC_C_Src0, colorSRC_C_Op0, colorCombinerScale0);
        //float4 Stage1 = TextureCombinerStage(diffuse, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, Stage0, colorCombinerEquation1, colorSRC_A_Src1, colorSRC_A_Op1, colorSRC_B_Src1, colorSRC_B_Op1, colorSRC_C_Src1, colorSRC_C_Op1, colorCombinerScale1);
        //float4 Stage2 = TextureCombinerStage(diffuse, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, Stage1, colorCombinerEquation2, colorSRC_A_Src2, colorSRC_A_Op2, colorSRC_B_Src2, colorSRC_B_Op2, colorSRC_C_Src2, colorSRC_C_Op2, colorCombinerScale2);
        //float4 Stage3 = TextureCombinerStage(diffuse, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, Stage2, colorCombinerEquation3, colorSRC_A_Src3, colorSRC_A_Op3, colorSRC_B_Src3, colorSRC_B_Op3, colorSRC_C_Src3, colorSRC_C_Op3, colorCombinerScale3);
        //float4 Stage4 = TextureCombinerStage(diffuse, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, Stage3, colorCombinerEquation4, colorSRC_A_Src4, colorSRC_A_Op4, colorSRC_B_Src4, colorSRC_B_Op4, colorSRC_C_Src4, colorSRC_C_Op4, colorCombinerScale4);
        //float4 Stage5 = TextureCombinerStage(diffuse, SpcRefColor, input.colorDebug, input.ps_t0, input.ps_t1, input.ps_t2, 0, colorCombinerEquation5, colorSRC_A_Src5, colorSRC_A_Op5, colorSRC_B_Src5, colorSRC_B_Op5, colorSRC_C_Src5, colorSRC_C_Op5, colorCombinerScale5);
        
        //TexCombinerStageBuffer[0] = Stage0;
        //TexCombinerStageBuffer[1] = Stage1;
        //TexCombinerStageBuffer[2] = Stage2;
        //TexCombinerStageBuffer[3] = Stage3;
        //TexCombinerStageBuffer[4] = Stage4;
        //TexCombinerStageBuffer[5] = Stage5;
        
        ////All
        //for (int i = 0; i < StageCount; i++)
        //{
        //    diffuse *= TexCombinerStageBuffer[i];
        //}
        
        ////Debug
        //diffuse *= TexCombinerStageBuffer[0] + TexCombinerStageBuffer[1] + TexCombinerStageBuffer[2] + TexCombinerStageBuffer[3] + TexCombinerStageBuffer[4] + TexCombinerStageBuffer[5]; //Original

        

    }

    //float alphaFactor = 1;
    //float4 alphaColor = float4(1, 1, 1, 1);
    ////float4 alphaColor = AlphaMap0.Sample(samplerSurface, input.ps_t0) + AlphaMap1.Sample(samplerSurface, input.ps_t0) + AlphaMap2.Sample(samplerSurface, input.ps_t0) + AlphaMap3.Sample(samplerSurface, input.ps_t0);
    //alphaFactor = alphaColor.a;
    //diffuse.rgb *= alphaColor.rgb;
    
    //float4 alphaColor = float4(1, 1, 1, 1);
    
    //float alphaFactor = 1;
    //float4 alphaColor = AlphaMap0.Sample(samplerSurface, input.ps_t0) + AlphaMap1.Sample(samplerSurface, input.ps_t0) + AlphaMap2.Sample(samplerSurface, input.ps_t0) + AlphaMap3.Sample(samplerSurface, input.ps_t0);
    //    //float4 alphaColor = AlphaMaps[0].Sample(samplerSurface, input.ps_t0) + AlphaMaps[1].Sample(samplerSurface, input.ps_t0) + AlphaMaps[2].Sample(samplerSurface, input.ps_t0) + AlphaMaps[3].Sample(samplerSurface, input.ps_t0);
    //diffuse.a *= alphaColor.a;
    
    
    
        
    //float4 StageAlpha0 = TextureCombinerStageAlpha(diffuse, input.colorDebug, input.ps_t0, alphaCombinerEquation0, alphaSRC_A_Src0, alphaSRC_A_Op0, alphaSRC_B_Src0, alphaSRC_B_Op0, alphaSRC_C_Src0, alphaSRC_C_Op0, alphaCombinerScale0);
    //float4 StageAlpha1 = TextureCombinerStageAlpha(diffuse, input.colorDebug, input.ps_t0, alphaCombinerEquation1, alphaSRC_A_Src1, alphaSRC_A_Op1, alphaSRC_B_Src1, alphaSRC_B_Op1, alphaSRC_C_Src1, alphaSRC_C_Op1, alphaCombinerScale1);
    //float4 StageAlpha2 = TextureCombinerStageAlpha(diffuse, input.colorDebug, input.ps_t0, alphaCombinerEquation2, alphaSRC_A_Src2, alphaSRC_A_Op2, alphaSRC_B_Src2, alphaSRC_B_Op2, alphaSRC_C_Src2, alphaSRC_C_Op2, alphaCombinerScale2);
    //float4 StageAlpha3 = TextureCombinerStageAlpha(diffuse, input.colorDebug, input.ps_t0, alphaCombinerEquation3, alphaSRC_A_Src3, alphaSRC_A_Op3, alphaSRC_B_Src3, alphaSRC_B_Op3, alphaSRC_C_Src3, alphaSRC_C_Op3, alphaCombinerScale3);
    //float4 StageAlpha4 = TextureCombinerStageAlpha(diffuse, input.colorDebug, input.ps_t0, alphaCombinerEquation4, alphaSRC_A_Src4, alphaSRC_A_Op4, alphaSRC_B_Src4, alphaSRC_B_Op4, alphaSRC_C_Src4, alphaSRC_C_Op4, alphaCombinerScale4);
    //float4 StageAlpha5 = TextureCombinerStageAlpha(diffuse, input.colorDebug, input.ps_t0, alphaCombinerEquation5, alphaSRC_A_Src5, alphaSRC_A_Op5, alphaSRC_B_Src5, alphaSRC_B_Op5, alphaSRC_C_Src5, alphaSRC_C_Op5, alphaCombinerScale5);
    
    
    //float4 alphaColor = StageAlpha0 * StageAlpha1 * StageAlpha2 * StageAlpha3 * StageAlpha4 * StageAlpha5; //* => +
    //diffuse.a *= alphaColor.a;
    
    

    ////TexCombinerStageBuffer[0] = StageAlpha0;
    ////TexCombinerStageBuffer[1] = StageAlpha1;
    ////TexCombinerStageBuffer[2] = StageAlpha2;
    ////TexCombinerStageBuffer[3] = StageAlpha3;
    ////TexCombinerStageBuffer[4] = StageAlpha4;
    ////TexCombinerStageBuffer[5] = StageAlpha5;
    
    ////TexCombinerStageBuffer[0].a *= StageAlpha0.a;
    ////TexCombinerStageBuffer[1].a *= StageAlpha1.a;
    ////TexCombinerStageBuffer[2].a *= StageAlpha2.a;
    ////TexCombinerStageBuffer[3].a *= StageAlpha3.a;
    ////TexCombinerStageBuffer[4].a *= StageAlpha4.a;
    ////TexCombinerStageBuffer[5].a *= StageAlpha5.a;
    
    
    //    //alpha=>FrameBuffer
    //    //acolor=>Conbiner
    //if (BlendMode == BLEND)
    //{
    //        //diffuse = colorBlend(diffuse, colorBlendEqation, colorBlendSRC, colorBlendDEST) * alphaBlend(alphaColor, alphaBlendEqation, alphaBlendSRC, alphaBlendDEST);
    //    diffuse = Blend(diffuse.rgba, alphaColor.rgba, ColorBlendEqationType, ColorBlendSRC, ColorBlendDEST);
    //    alphaColor = Blend(diffuse.rgba, alphaColor.rgba, AlphaBlendEqationType, AlphaBlendSRC, AlphaBlendDEST);
    //    diffuse *= alphaColor;
    //}
    //else if (BlendMode == SEPARATE)
    //{
    //    diffuse = Blend(diffuse.rgba, alphaColor.rgba, ColorBlendEqationType, ColorBlendSRC, ColorBlendDEST);
    //    alphaColor = Blend(diffuse.rgba, alphaColor.rgba, AlphaBlendEqationType, AlphaBlendSRC, AlphaBlendDEST);
    //    diffuse *= alphaColor;
    //}
    //else if (BlendMode == LOGICAL_OPERATION)
    //{
    //    diffuse = LogicalBlend(diffuse, 0); //type => 
    //}
    //else if (BlendMode == NONE)
    //{
        
    //}
    

    //diffuse = input.ps_colorDiffuse + 1 * (input.colorDebug * input.ps_colorDiffuse); //(?)
    //diffuse = input.ps_colorDiffuse * (input.colorDebug - input.ps_colorDiffuse);
    //diffuse * colorBlendEquation + alpha * alphaBlendEquation

    //Default (and Debug)
    float3 dir = normalize(vEyePos - input.ps_vEye1.xyz);
    float f = clamp(0.5 + 0.5 * abs(dot(dir, normalize(input.ps_Nr))), 0, 1);
    diffuse.rgb *= f;
    
    //if (bIsFragmentLighting)
    //{
    //    float4 specular = vMaterialSpecular;
    //    float shininess = sMaterialShininess;
    //    float4 reflectColor = vMaterialReflect;
    //    if (bBatched)
    //    {
    //        specular = FloatToRGB(input.colorDebug.z);
    //        shininess = input.colorDebug.x;
    //        reflectColor = FloatToRGB(input.colorDebug.w);
    //    }
    //    if (bIsFragmentLighting)
    //    {
    //        specular *= texSpecularMap.Sample(samplerSurface, input.ps_t0);
    //    }
    
    //    float4 color = lightSurface(input.ps_vEye1, V, N, diffuse, specular, shininess, reflectColor);
    
    //    //get shadow color
    //    float s = 1;
    //    float d = dot(getLookDir(vLightView), N);
    //    if (d > 0)
    //    {
    //        s = shadowStrength(input.ps_sp);
    //    }
    //    color.rgb *= s;
    
    //    float4 emissive = input.ps_colorEmission;
    //    float4 ambient = vLightAmbient * vMaterialAmbient;
    //    if (SSAOEnabled)
    //    {
    //        float2 quadTex = input.ps_p.xy * vResolution.zw;
    //        ambient.rgb *= texSSAOMap.SampleLevel(samplerSurface, quadTex, 0).r;
    //    }
    
    ////float2 quadTex = input.ps_p.xy * vResolution.zw;
    ////ambient.rgb *= texSSAOMap.SampleLevel(samplerSurface, quadTex, 0).r;
    
    ////emissive.rgb *= Texture0.Sample(samplerSurface, input.ps_t0).rgb;
    //    color.rgb += emissive.rgb + ambient.rgb;
    //    color.a = diffuse.a * alphaColor.a;
    //    diffuse = saturate(color);
    //}
    //else
    //{
    //    float3 dir = normalize(vEyePos - input.ps_vEye1.xyz);
    //    float f = clamp(0.5 + 0.5 * abs(dot(dir, normalize(input.ps_Nr))), 0, 1);
    //    diffuse.rgb *= f;
    //}

    return diffuse;
}

#endif