#ifndef VSMESHCGFX_HLSL
#define VSMESHCGFX_HLSL

#define CGFXMESH
//#define INSTANCINGPARAM

#include"Common\Common.hlsl"
#include"Common\DataStructs.hlsl"
#include"Common\CGFXDataStruct.hlsl"
#include"Common\CGFXBuffer.hlsl"

#pragma pack_matrix( row_major )

#if !defined(INSTANCINGPARAM)
#if defined(CLIPPLANE)
PSInputClipCGFX main(VSInputCGFX input)
#endif
#if !defined(CLIPPLANE)
PSInputCGFX main(VSInputCGFX input)
#endif
#endif
#if defined(INSTANCINGPARAM)
PSInputCGFX main(VSInstancingInputCGFX input)
#endif
{
    #if !defined(CLIPPLANE)
    PSInputCGFX output = (PSInputCGFX)0;
    #endif
    #if defined(CLIPPLANE)
    PSInputClipCGFX output = (PSInputClipCGFX)0;
    #endif
    float4 inputPos = input.vs_p;
    float3 inputNr = input.vs_Nr;
    float3 inputTr = input.vs_Tr;
    float3 inputBiTr = input.vs_BiTr;
    if (bInvertNormal)
    {
        inputNr = -inputNr;
    }

    // compose instance matrix
    if (bHasInstances)
    {
        matrix mInstance =
        {
            input.vs_m1,
            input.vs_m2,
            input.vs_m3,
            input.vs_m4
        };
        inputPos = mul(input.vs_p, mInstance);
        inputNr = mul(inputNr, (float3x3) mInstance);
        if (bIsFragmentLighting) //p0
        {
            if (!bAutoTengent)
            {
                inputTr = mul(inputTr, (float3x3) mInstance);
                inputBiTr = mul(inputBiTr, (float3x3) mInstance);
            }
        }
    }

    //set position into world space	
    output.ps_p = mul(inputPos, mWorld);
    float3 vEye = vEyePos - output.ps_p.xyz;
    output.ps_vEye0 = float4(normalize(vEye), length(vEye)); //Use wp for camera->vertex direction
    //set normal for interpolation	
    output.ps_Nr = normalize(mul(inputNr, (float3x3) mWorld));
    //set color
    output.colorDebug = input.c;

    #if !defined(INSTANCINGPARAM)
    //set texture coords
    output.ps_t0 = mul(float2x4(uvTransformR10, uvTransformR20), float4(input.vs_t0, 0, 1)).xy; //ps_t0 => Texture[No. 0]
    output.ps_t1 = mul(float2x4(uvTransformR11, uvTransformR21), float4(input.vs_t1, 0, 1)).xy; //ps_t1 => Texture[No. 0]
    output.ps_t2 = mul(float2x4(uvTransformR12, uvTransformR22), float4(input.vs_t2, 0, 1)).xy; //ps_t2 => Texture[No. 0]
    
    output.ps_colorDiffuse = vMaterialDiffuse;
    output.ps_colorEmission = vMaterialEmissive;
    

    //output.ps_colorSpecular0
    #endif

    #if defined(INSTANCINGPARAM)
    if (!bHasInstanceParams)
    {
        output.ps_t0 = mul(float2x4(uvTransformR10, uvTransformR20), float4(input.vs_t0, 0, 1)).xy;
        output.ps_t1 = mul(float2x4(uvTransformR11, uvTransformR21), float4(input.vs_t1, 0, 1)).xy;
        output.ps_t2 = mul(float2x4(uvTransformR12, uvTransformR22), float4(input.vs_t2, 0, 1)).xy;
        output.ps_colorDiffuse = vMaterialDiffuse;
        if (!bRenderPBR)
        {
            output.ps_colorEmission = vMaterialEmissive;
        }
        else
        {
            output.ps_colorEmission = vMaterialSpecular;
        }
    }
    else
    {
        //set texture coords and color
        output.ps_t0 = mul(float2x4(uvTransformR10, uvTransformR20), float4(input.vs_t0, 0, 1)).xy + input.tOffset;
        output.ps_t1 = mul(float2x4(uvTransformR11, uvTransformR21), float4(input.vs_t1, 0, 1)).xy + input.tOffset;
        output.ps_t2 = mul(float2x4(uvTransformR12, uvTransformR22), float4(input.vs_t2, 0, 1)).xy + input.tOffset;
        output.ps_colorDiffuse = input.diffuseC;
        if (!bRenderPBR)
        {
            output.ps_colorEmission = input.emissiveC;
        }
        else
        {
            output.ps_colorEmission = input.emissiveC;
        }
    }
    #endif

    if (bHasDisplacementMap)
    {
        const float mipInterval = 20;
        float mipLevel = clamp((distance(output.ps_p.xyz, vEyePos) - mipInterval) / mipInterval, 0, 6);
        float3 h = texDisplacementMap.SampleLevel(samplerDisplace, output.ps_t0, mipLevel); //ps_t0 => Texture[No. 0]
        output.ps_p.xyz += output.ps_Tr * mul(h, displacementMapScaleMask.xyz);
    }
    output.ps_vEye1 = output.ps_p;
    //set position into clip space	
    output.ps_p = mul(output.ps_p, mViewProjection);

    //set position into light-clip space
    if (bHasShadowMap)
    {
        output.ps_sp = mul(output.ps_vEye1, mul(vLightView, vLightProjection));
    }

    //p1
    if (bIsFragmentLighting)
    {
        if (!bAutoTengent)
        {
            // transform the tangents by the world matrix and normalize
            output.ps_Tr = normalize(mul(inputTr, (float3x3) mWorld));
            output.ps_BiTr = normalize(mul(inputBiTr, (float3x3) mWorld));
        }
    }

    //CLIPLANE Setting (PSInputClip)
#if defined(CLIPPLANE)
    output.clipPlane = float4(0, 0, 0, 0);
    if (EnableCrossPlane.x)
    {
        float3 p = output.ps_vEye1.xyz - CrossPlane1Params.xyz * CrossPlane1Params.w;
        output.clipPlane.x = dot(CrossPlane1Params.xyz, p);
    }
    if (EnableCrossPlane.y)
    {
        float3 p = output.ps_vEye1.xyz - CrossPlane2Params.xyz * CrossPlane2Params.w;
        output.clipPlane.y = dot(CrossPlane2Params.xyz, p);
    }
    if (EnableCrossPlane.z)
    {
        float3 p = output.ps_vEye1.xyz - CrossPlane3Params.xyz * CrossPlane3Params.w;
        output.clipPlane.z = dot(CrossPlane3Params.xyz, p);
    }
    if (EnableCrossPlane.w)
    {
        float3 p = output.ps_vEye1.xyz - CrossPlane4Params.xyz * CrossPlane4Params.w;
        output.clipPlane.w = dot(CrossPlane4Params.xyz, p);
    }
    if (EnableCrossPlane5To8.x)
    {
        float3 p = output.ps_vEye1.xyz - CrossPlane5Params.xyz * CrossPlane5Params.w;
        output.clipPlane5To8.x = dot(CrossPlane5Params.xyz, p);
    }
    if (EnableCrossPlane5To8.y)
    {
        float3 p = output.ps_vEye1.xyz - CrossPlane6Params.xyz * CrossPlane6Params.w;
        output.clipPlane5To8.y = dot(CrossPlane6Params.xyz, p);
    }
    if (EnableCrossPlane5To8.z)
    {
        float3 p = output.ps_vEye1.xyz - CrossPlane7Params.xyz * CrossPlane7Params.w;
        output.clipPlane5To8.z = dot(CrossPlane7Params.xyz, p);
    }
    if (EnableCrossPlane5To8.w)
    {
        float3 p = output.ps_vEye1.xyz - CrossPlane8Params.xyz * CrossPlane8Params.w;
        output.clipPlane5To8.w = dot(CrossPlane8Params.xyz, p);
    }
    if (CuttingOperation == 1)
    {
        output.clipPlane.x = -(whenle(-output.clipPlane.x, 0) * whenle(-output.clipPlane.y, 0)
            * whenle(-output.clipPlane.z, 0) * whenle(-output.clipPlane.w, 0)
            * whenle(-output.clipPlane5To8.x, 0) * whenle(-output.clipPlane5To8.y, 0)
            * whenle(-output.clipPlane5To8.z, 0) * whenle(-output.clipPlane5To8.w, 0));
        output.clipPlane.yzw = float3(0, 0, 0);
        output.clipPlane5To8 = float4(0, 0, 0, 0);
    }
#endif
    return output;
}

#endif