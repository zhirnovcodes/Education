Shader "Unlit/Molecule"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurText ("Blur Texture", 2D) = "white" {}
        _PerlinText ("Perlin Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _ColorTo ("Color To", Color) = (1, 1, 1, 1)
        _Speed ("Noise Speed", Vector) = (1, 1, 0, 0)
        _PerlinWidth ("Perlin Width", Float) = 100

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
                float4 wPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _BlurText;
            sampler2D _PerlinText;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _ColorTo;
            float4 _Speed;
            float _WaveSpeed;
            float _WaveDecay;
            float _PerlinWidth;
            float4 _SourcePos;
            
            sampler2D _Values;
            float _StartTime;
            float _DeltaTime;
            int _ValuesWidth;
            int _MaxIndex;

            float invLerp(float a, float b, float v)
            {
                return (v - a) / (b - a);
			}

            float getValue(float t)
            {
                t -= _StartTime;

                float i0 = floor(t / _DeltaTime);

                i0 = min(_MaxIndex, i0);
                i0 = max(0, i0);

                float i1 = min(_MaxIndex, i0 + 1);
                i1 = max(0, i1);

                float u0 = saturate(i0 / _ValuesWidth); 
                float u1 = saturate(i1 / _ValuesWidth); 

                float v0 = tex2Dlod(_Values, float4(u0, 0, 0, 0)).r;
                float v1 = tex2Dlod(_Values, float4(u1, 0, 0, 0)).r;

                v0 = v0 <= -9999999 ? 0 : v0;
                v1 = v1 <= -9999999 ? v0 : v1;

                float t0 = i0 * _DeltaTime;
                float t1 = i1 * _DeltaTime;
                float t2 = saturate( t1 == t0 ? 0 : invLerp(t0, t1, t) ); 

                return lerp(v0, v1, t2);
			}

            float getPerlin(float2 pos){
                float4 uv = float4((abs(pos) / _PerlinWidth), 0,1);
                return tex2Dlod(_PerlinText, uv);
                //return snoise(pos);     
			}

            v2f vert (appdata v)
            {
                v2f o;


                float3 wPos = unity_ObjectToWorld._m03_m13_m23;
                float2 wPos2 = wPos.xy;
                float z = sin(wPos.x) * cos(wPos.y);

                o.wPos = float4(wPos, z);

                float noiseOffset = 0.01;
                float noiseC = getPerlin(wPos2);
                float noiseX = getPerlin(wPos2 + float2(noiseOffset,0)) - noiseC;
                float noiseY = getPerlin(wPos2 + float2(0,noiseOffset)) - noiseC;
                float noiseZ = getPerlin(wPos2 + float2(noiseOffset,noiseOffset)) - noiseC;

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
                float tr = 1 - abs(saturate(depth));
                
                fixed4 texColor = tex2D(_MainTex, i.uv);
                float blur = saturate(depth >= 0 ? 0 : abs(depth));
                float radius = length( i.uv - float2(0.5, 0.5));
                float blurStart = 0.5;
                float blurEnd = blurStart - blur / 2;
                float blurAlpha = saturate(invLerp(blurEnd, blurStart, radius));
                blurAlpha = lerp(1, 0, blurAlpha) * (1 - blur);

                float colNoise = saturate( (sin(i.wPos.x) * cos(i.wPos.y) + 1) / 2 );
                fixed4 color = lerp(_Color, _ColorTo, colNoise);
                fixed4 col = texColor * color * float4(1,1,1,blurAlpha);
                col.a = col.a * tr;

                //col = fixed4(depth, 0,0,1);
                return col;
                
            }
            ENDCG
        }
    }
}
