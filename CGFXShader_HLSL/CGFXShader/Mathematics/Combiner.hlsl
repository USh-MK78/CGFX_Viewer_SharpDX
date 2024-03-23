#ifndef COMBINER_HLSL
#define COMBINER_HLSL
#pragma pack_matrix( row_major )

#include"..\Common\ConstantValues.hlsl"

float4 BlendElement(float4 color, float4 alpha, int type)
{
	float4 output = -1;
	
	if (type == ZERO)
	{
		//return famebuffer out
		
		//output = BlendElement(b, alpha, colorBlendDEST);
	}
	else if (type == ONE)
	{
		//b.r = 1;
		//b.g = 1;
		//b.b = 1;
		//b.a = 1;
	}
	else if (type == COMBINER_OUT_RGB)
	{
		output = color.rgba;
	}
	else if (type == ONE_SUB_COMBINER_RGB)
	{
		output = 1 - color.rgba;
	}
	else if (type == FRAMEBUFFER_OUT_RGB)
	{
		output = alpha.rgba;
	}
	else if (type == ONE_SUB_FRAMEBUFFER_RGB)
	{
		output = 1 - alpha.rgba;
	}
	else if (type == COMBINER_OUT_A)
	{
		output = color.a;
	}
	else if (type == ONE_SUB_COMBINER_A)
	{
		output = 1 - color.a;
	}
	else if (type == FRAMEBUFFER_OUT_A)
	{
		output = alpha.a;
	}
	else if (type == ONE_SUB_FRAMEBUFFER_A)
	{
		output = 1 - alpha.a;
	}
	else if (type == BLEND_RGB)
	{
		
	}
	else if (type == ONE_SUB_BLEND_RGB)
	{
		
	}
	else if (type == BLEND_A)
	{
		
	}
	else if (type == ONE_SUB_BLEND_A)
	{
		
	}
	else if (type == MIN_BRKT_COMBINER_OUT_A_ONE_SUB_FRAMEBUFFER_A_BRKT)
	{
		
	}
	
	return output;
}

////Conbiner OUT = TextureConbiner OUT
////
////input = (Conbiner OUT * BlendElement(MainTexture, colorBlendSRC)) + (FrameBuffer (ALPHAMAP) * BlendElement(AlphaMapTexture, colorBlendDEST));
//float4 Blend(float3 color, float alpha, int colorBlendEquation, int colorBlendSRC, int colorBlendDEST)
//{
//    //float4 color = input.rgba;
    
//    float4 output = -1;
	
//    if (colorBlendEquation == SRC_ADD_DEST)
//    {
//        //SRC+DEST
//        output = (color * BlendElement(color, alpha, colorBlendSRC)) + (alpha * BlendElement(color, alpha, colorBlendDEST));
//    }
//    else if (colorBlendEquation == SRC_SUB_DEST)
//    {
//        //SRC-DEST
//        output = (color * BlendElement(color, alpha, colorBlendSRC)) - (alpha * BlendElement(color, alpha, colorBlendDEST));
//    }
//    else if (colorBlendEquation == DEST_SUB_SRC)
//    {
//        //DEST-SRC
//        output = (color * BlendElement(color, alpha, colorBlendDEST)) - (alpha * BlendElement(color, alpha, colorBlendSRC));
//    }
//    else if (colorBlendEquation == MIN_SRC_DEST)
//    {
//        output = min((color * BlendElement(color, alpha, colorBlendSRC)), (alpha * BlendElement(color, alpha, colorBlendDEST)));
//    }
//    else if (colorBlendEquation == MAX_SRC_DEST)
//    {
//        output = max((color * BlendElement(color, alpha, colorBlendSRC)), (alpha * BlendElement(color, alpha, colorBlendDEST)));
//    }
    
//    return output;
//}

//Conbiner OUT = TextureConbiner OUT
//
//input = (Conbiner OUT * BlendElement(MainTexture, colorBlendSRC)) + (FrameBuffer (ALPHAMAP) * BlendElement(AlphaMapTexture, colorBlendDEST));
float4 Blend(float4 color, float4 alpha, int colorBlendEquation, int colorBlendSRC, int colorBlendDEST)
{
    //float4 color = input.rgba;
    
    float4 output = -1;
	
    if (colorBlendEquation == SRC_ADD_DEST)
    {
        //SRC+DEST
        output = (color * BlendElement(color, alpha, colorBlendSRC)) + (alpha * BlendElement(color, alpha, colorBlendDEST));
    }
    else if (colorBlendEquation == SRC_SUB_DEST)
    {
        //SRC-DEST
        output = (color * BlendElement(color, alpha, colorBlendSRC)) - (alpha * BlendElement(color, alpha, colorBlendDEST));
    }
    else if (colorBlendEquation == DEST_SUB_SRC)
    {
        //DEST-SRC
        output = (color * BlendElement(color, alpha, colorBlendDEST)) - (alpha * BlendElement(color, alpha, colorBlendSRC));
    }
    else if (colorBlendEquation == MIN_SRC_DEST)
    {
        output = min((color * BlendElement(color, alpha, colorBlendSRC)), (alpha * BlendElement(color, alpha, colorBlendDEST)));
    }
    else if (colorBlendEquation == MAX_SRC_DEST)
    {
        output = max((color * BlendElement(color, alpha, colorBlendSRC)), (alpha * BlendElement(color, alpha, colorBlendDEST)));
    }
    
    return output;
}

float4 LogicalBlend(float4 input, int type)
{
	if (type == SET_ALL_BIT_ZERO)
	{
		input.r = 0;
		input.g = 0;
		input.b = 0;
		input.a = 0;
	}
	else if (type == SET_ALL_BIT_ONE)
	{
		input.r = 1;
		input.g = 1;
		input.b = 1;
		input.a = 1;
	}

	return input;
}


#endif