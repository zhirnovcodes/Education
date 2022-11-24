Shader "Zhirnov/VinylAlphaCreator"
{
    Properties
    {
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

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                float2 lCen = float2(0.5, 0.5);
                float d = length(IN.localTexcoord.xy - lCen);

                if (IN.primitiveID == 0)
                { 
                    //if (IN.localTexcoord.x > 0.51 || d > 0.5)
                    if (d > 0.5)
                    {
                        discard;
					}
                }
                else if (IN.primitiveID == 2)
                {
                    //if (IN.localTexcoord.x < 0.49 || d > 0.5)
                    if (d > 0.5)
                    {
                        discard; 
					}
				}
                else if (IN.primitiveID == 1)
                {
                    d = abs(IN.localTexcoord.y - 0.5);
				}

                float4 col = float4(saturate(d),0,0, 0)*2;//d / 2);//d
                return col;
            }
            ENDCG
        }
    }
}