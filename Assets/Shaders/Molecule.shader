Shader "Unlit/Molecule"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Speed ("Noise Speed", Vector) = (1, 1, 0, 0)

        _WaveDecay ("Wave Decay Range", Range(0.01, 10)) = 5
        _WaveSpeed ("Wave Spread Speed", Range(0.1, 10)) = 1
        _SourcePos ("Source Position", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {        
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile HAS_VALUES

            #include "UnityCG.cginc"
            #include "NoiseSimplex.cginc"

            #define PI 3.1415
            #define PI2 (PI * 2)

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 offset : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _Speed;
            float _WaveSpeed;
            float _WaveDecay;
            float4 _SourcePos;
            
            sampler2D _Values;
            float _StartTime;
            float _DeltaTime;
            int _ValuesWidth;

            float invLerp(float a, float b, float v)
            {
                return (v - a) / (b - a);
			}

            float getValue(float t)
            {
                t -= _StartTime;
                if (t < 0)
                {
                    return 0;
				}

                float i0 = floor(t / _DeltaTime);

                i0 = min(_ValuesWidth - 1, i0);
                float i1 = min(_ValuesWidth - 1, i0 + 1);

                float u0 = i0 / _ValuesWidth; 
                float u1 = i1 / _ValuesWidth; 

                float v0 = tex2Dlod(_Values, float4(u0, 0, 0, 0)).r;
                float v1 = tex2Dlod(_Values, float4(u1, 0, 0, 0)).r;

                v0 = v0 == -9999999 ? 0 : v0;
                v1 = v1 == -9999999 ? 0 : v1;

                float t0 = i0 * _DeltaTime;
                float t1 = i1 * _DeltaTime;
                float t2 = saturate( t1 == t0 ? 0 : invLerp(t0, t1, t) ); 

                return lerp(v0, v1, t2);
			}

            v2f vert (appdata v)
            {
                v2f o;


                float3 wPos = unity_ObjectToWorld._m03_m13_m23;
                float2 wPos2 = wPos.xy;
                float z = sin(wPos.x) * cos(wPos.y);

                float noiseOffset = 0.01;
                float noiseC = snoise(wPos2);
                float noiseX = snoise(wPos2 + float2(noiseOffset,0)) - noiseC;
                float noiseY = snoise(wPos2 + float2(0,noiseOffset)) - noiseC;
                float noiseZ = snoise(wPos2 + float2(noiseOffset,noiseOffset)) - noiseC;

                float3 noise3D = normalize( float3(noiseX, noiseY, noiseZ) );

                float3 time = PI2 * _Time.x * noise3D;
                float4 perlinOffset = _Speed * float4( sin( time.x ), cos( time.y ), sin( time.z ) + z, 1 );
                o.offset = perlinOffset;


                // todo from source
                float4 funcOffset = float4(0, 0, 0, 0);
                #ifdef HAS_VALUES
                    funcOffset = float4(1, 0, 0, 0);
                    float timeOffset = abs((_SourcePos - wPos).x / _WaveSpeed);
                    timeOffset = clamp( timeOffset / 5, 0, 8);
                    funcOffset *= getValue(_Time.y - timeOffset) * (1 - saturate(invLerp(0, _WaveDecay, abs((_SourcePos - wPos).x))));
                #endif

                v.vertex += perlinOffset + funcOffset;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float depth = i.offset.z;
                float tr = depth >= 0 ? 1 : 1 + depth;
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                col.a = col.a * tr;

                //col = fixed4(depth, 0,0,1);
                return col;
                
            }
            ENDCG
        }
    }
}
