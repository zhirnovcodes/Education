Shader "Zhirnov/VinylAlphaCreator"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1)
        //_ColorTo ("Color To", Color) = (0,0,0,0)
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
            //float4      _ColorTo;

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                float2 lCen = float2(0.5, 0.5);
                float d = length(IN.localTexcoord.xy - lCen);

                if (IN.primitiveID == 0)
                { 
                    //if (IN.localTexcoord.x > 0.51 || d > 0.5)
                    if (d >= 0.5)
                    {
                        discard;
					}
                }
                if (IN.primitiveID == 2)
                {
                    //if (IN.localTexcoord.x < 0.49 || d > 0.5)
                    if (d >= 0.5)
                    {
                        discard;
					}
				}
                if (IN.primitiveID == 1)
                {
                    d = abs(IN.localTexcoord.y - 0.5);
				}

                //d = 1 - saturate(d * 2);
                //float col = lerp(_ColorTo, _Color, d).x;

                float4 col = float4(_Color.xyz, 0);//d / 2);//d
                return col;
            }
            ENDCG
        }
    }
}