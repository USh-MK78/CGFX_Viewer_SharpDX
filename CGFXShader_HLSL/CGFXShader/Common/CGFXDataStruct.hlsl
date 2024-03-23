#ifndef CGFXDATASTRUCTS_HLSL
#define CGFXDATASTRUCTS_HLSL
#pragma pack_matrix( row_major )

#include"DataStructs.hlsl"

struct VSInputCGFX
{
	float4 vs_p : POSITION;
	float3 vs_Nr : NORMAL;
	float3 vs_Tr : TANGENT;
	float3 vs_BiTr : BINORMAL; // bi-tangent
    
    float4 c : COLOR;
    
	float2 vs_t0 : TEXCOORD0;
    float2 vs_t1 : TEXCOORD1;
    float2 vs_t2 : TEXCOORD2;
    //float2 vs_t3 : TEXCOORD3;
	
	//Matrix
	float4 vs_m1 : TEXCOORD3;
	float4 vs_m2 : TEXCOORD4;
	float4 vs_m3 : TEXCOORD5;
    float4 vs_m4 : TEXCOORD6;
};

struct VSInstancingInputCGFX
{
    float4 vs_p : POSITION;
    float3 vs_Nr : NORMAL;
    float3 vs_Tr : TANGENT;
    float3 vs_BiTr : BINORMAL;
    
    float4 c : COLOR;
    
    float2 vs_t0 : TEXCOORD0;
    float2 vs_t1 : TEXCOORD1;
    float2 vs_t2 : TEXCOORD2;
    //float2 vs_t3 : TEXCOORD3;

    float4 vs_m1 : TEXCOORD3;
    float4 vs_m2 : TEXCOORD4;
    float4 vs_m3 : TEXCOORD5;
    float4 vs_m4 : TEXCOORD6;

    float4 diffuseC : COLOR1;
    float4 emissiveC : COLOR2;
    float2 tOffset : TEXCOORD7; //TEXCOORD8
};

struct PSInputCGFX
{
	float4 ps_vEye0 : POSITION0;
	float4 ps_vEye1 : POSITION1;

	float4 ps_p : SV_POSITION;
	float3 ps_Nr : NORMAL;	
	float3 ps_Tr : TANGENT;
	float3 ps_BiTr : BINORMAL; // bi-tangent

	float2 ps_t0 : TEXCOORD0; //Main (Texture Coordinate)
    float2 ps_t1 : TEXCOORD1; //TexCoordSlot2
    float2 ps_t2 : TEXCOORD2; //TexCoordSlot3
    //float2 ps_t3 : TEXCOORD3; //TexCoordSlot4
    float4 ps_sp : TEXCOORD3; //Light-clip Space

	float4 colorDebug : COLOR;
    float4 ps_colorDiffuse : COLOR2;
    float4 ps_colorEmission : COLOR1;

	//float4 p : SV_POSITION;
	//float3 Nr : NORMAL; // normal
	//float3 Tr : TANGENT;
	//float4 wp : POSITION1;
	//float4 sp : TEXCOORD1;
	//float2 t : TEXCOORD0; // tex coord	
	//float3 t1 : TANGENT; // tangent
	//float3 t2 : BINORMAL; // bi-tangent	
	//float4 c : COLOR; // solid color (for debug)
	//float4 c2 : COLOR1; //vMaterialEmissive
	//float4 cDiffuse : COLOR2; //vMaterialDiffuse
};

struct PSInputClipCGFX
{
    float4 ps_vEye0 : POSITION0;
    float4 ps_vEye1 : POSITION1;

    float4 ps_p : SV_POSITION;
    float3 ps_Nr : NORMAL;
    float3 ps_Tr : TANGENT;
    float3 ps_BiTr : BINORMAL; // bi-tangent

    float2 ps_t0 : TEXCOORD0; //Main (Texture Coordinate)
    float2 ps_t1 : TEXCOORD1; //TexCoordSlot2
    float2 ps_t2 : TEXCOORD2; //TexCoordSlot3
    //float2 ps_t3 : TEXCOORD3; //TexCoordSlot4
    float4 ps_sp : TEXCOORD3; //Light-clip Space
    
    float4 colorDebug : COLOR;
    float4 ps_colorDiffuse : COLOR2;
    float4 ps_colorEmission : COLOR1;
	
	//SV_ClipDistance
	float4 clipPlane : SV_ClipDistance0;
	float4 clipPlane5To8 : SV_ClipDistance1;

	//float4 p : SV_POSITION;
	//float3 Nr : NORMAL; // normal
	//float3 Tr : TANGENT;
	//float4 wp : POSITION1;
	//float4 sp : TEXCOORD1;
	//float2 t : TEXCOORD0; // tex coord	
	//float3 t1 : TANGENT; // tangent
	//float3 t2 : BINORMAL; // bi-tangent	
	//float4 c : COLOR; // solid color (for debug)
	//float4 c2 : COLOR1; //vMaterialEmissive
	//float4 cDiffuse : COLOR2; //vMaterialDiffuse
};
#endif