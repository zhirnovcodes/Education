Shader "Zhirnov/VinylAlphaCreator"
{
    Properties
    {
        _Color ("Color", Color) = (0.1,0.1,0,0)
    }

    SubShader
    {
        Lighting Off
        Blend SrcAlpha SrcAlpha
        //Blend One One

        Pass
        {
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            #define NORMAL_UP float4(0.5,0.5,1,1)

            float4      _Color;

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                if (IN.primitiveID == 0)
                {
                    float3 c = float3(1, 0.5, 0);
                    float3 p = IN.localTexcoord;
                    if (length(p - c) > 0.5)
                    {
                        discard;
                    }
                }
                if (IN.primitiveID == 2)
                {
                    float3 c = float3(0, 0.5, 0);
                    float3 p = IN.localTexcoord;
                    if (length(p - c) > 0.5)
                    {
                        discard;
                    }
				}

                return float4(_Color.xyz, 0);
            }
            ENDCG
        }
    }
}