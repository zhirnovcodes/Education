Shader "Unlit/Molecule"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        _SourcePushRange ("Source Push Range", Range(0, 100)) = 0
        
        _NoisePower ("Noise Power", Range(0, 1)) = 1
        _Color ("Color", Color) = (1, 1, 1, 1)
        _ColorTo ("Color To", Color) = (1, 1, 1, 1)
        _PerlinText ("Perlin Texture", 2D) = "black" {}
        _Speed ("Noise Speed", Vector) = (1, 1, 0, 0)
        _NoiseAmplitude ("Noise Amplitude", Vector) = (1, 1, 1, 0)
        _PerlinWidth ("Perlin Width", Float) = 100
        _Scale ("Scale", Float) = 1
        
        _FunctionPower ("Function Power", Range(0, 1)) = 1
        _Values ("Values Texture", 2D) = "black" {}
        _WaveColorValue ("Wave Color Value", Range(0, 1)) = 0
        _WaveColor ("Wave Color", Color) = (1, 1, 1, 1)
        _WaveScale ("Wave Scale", Range(0, 1)) = 0
        _IsRadiant ("Is Radian", Float) = 0
        _IsMirrored ("Is Mirrored", Range(0, 1)) = 0

        _WaveDecay ("Wave Decay Range", Range(0.01, 100)) = 5
        _WaveSpeed ("Wave Spread Speed", Range(0.1, 10)) = 1
        _SourcePos ("Source Position", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {        
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
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
                float4 color : TEXCOORD3;
                float4 wPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _PerlinText;
            float4 _MainTex_ST;
            float4 _WaveColor;
            float4 _Color;
            float4 _ColorTo;
            float4 _Speed;
            float _NoisePower;
            float _IsMirrored;
            float3 _NoiseAmplitude;
            float _FunctionPower;
            float _SourcePushRange;
            float _WaveSpeed;
            float _WaveScale;
            float _WaveColorValue;
            float _IsRadiant;
            float _WaveDecay;
            float _PerlinWidth;
            float _Scale;
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
                float t2 = t1 == t0 ? 0 : saturate( invLerp(t0, t1, t) ); 

                return lerp(v0, v1, t2);
			}

            float getPerlin(float2 pos){
                float4 uv = float4((pos / _PerlinWidth), 0,1);
                return tex2Dlod(_PerlinText, uv);
                //return snoise(pos);     
			}

            float4 getBufferValue(float3 sourcePosition, float3 worldPosition, float3 radiand) // _SourcePos, o.wPos, _IsRadiant
            {
            #ifdef HAS_VALUES
                float2 offDir = sourcePosition.xy - worldPosition.xy;
                float rad = saturate(radiand);
                float distance = lerp(abs(offDir.x), length(offDir), rad);
                float linearDir = float4(_IsMirrored == 0 ? 1 : -sign(offDir.x), 0, 0, 0);
                float4 funcDirection = lerp(float4(linearDir, 0, 0, 0), float4(normalize(offDir).xy, 0, 0), rad);
                float funcDistancePower = 1 - saturate(invLerp(0, _WaveDecay, distance));

                float timeOffset = distance / _WaveSpeed;
                timeOffset = clamp( timeOffset / 5, 0, 8);
                float funcValue = getValue(_Time.y - timeOffset) * funcDistancePower;
                funcDirection *= funcValue;

                funcDirection.a = funcValue;
                return funcDirection;
            #else
                return float4(0, 0, 0, 0);
            #endif
			}

            v2f vert (appdata v)
            {
                v2f o;
                
                float3 wPos = unity_ObjectToWorld._m03_m13_m23;
                float2 wPos2 = wPos.xy;


                float z = wPos.z;

                if (_SourcePushRange > 0)
                {                    
                    float2 offDir = _SourcePos.xy - wPos.xy;
                    float distanceToSource = lerp(abs(offDir.x), length(offDir), _IsRadiant);

                    float pushT = 1 - saturate(distanceToSource / _SourcePushRange);
                    z -= pushT;
                    wPos.z = z;
                    
                    v.vertex.z -= pushT;
				}

                o.wPos = float4(wPos, z);

                if (_NoisePower > 0)
                {

                    float noiseOffset = 1;
                    float noiseC = getPerlin(wPos2);
                    float noiseX = getPerlin(wPos2 + float2(noiseOffset,0)) - noiseC;
                    float noiseY = getPerlin(wPos2 + float2(0,noiseOffset)) - noiseC;
                    float noiseZ = noiseC * 2 - 1;

                    float3 noise3D = normalize( float3(noiseX, noiseY, noiseZ) );

                    float3 time = PI2 * _Time.x * noise3D;
                    float3 dist = time * _Speed;
                    float3 perlinOffset = float3( sin( dist.x ), cos( dist.y ), sin( dist.z ) );
                    perlinOffset *= _NoiseAmplitude * _NoisePower;
                    o.offset.xyz = perlinOffset;

                    v.vertex.xyz += perlinOffset;
				}
                else
                {
                    o.offset.xyz = float3(0,0,0);
				}

                float4 funcOffset = getBufferValue( _SourcePos, o.wPos.xyz, _IsRadiant ) * _FunctionPower;
                float funcValue = funcOffset.a;
                funcOffset.a = 0;
                o.color = lerp(float4(1,1,1,1), float4(lerp(_Color.xyz, _WaveColor.xyz, funcValue), 1), _WaveColorValue);

                v.vertex.xyz += funcOffset;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float scale = _Scale + lerp(0, _WaveScale, saturate(funcValue));
                float4 scaleOffset = scale * float4(normalize(o.uv - float2(0.5, 0.5)), 0, 0);
                v.vertex.xyz += scaleOffset;

                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float depth = i.offset.z + i.wPos.z;
                float tr = 1 - abs(saturate(depth));
                
                fixed4 texColor = tex2D(_MainTex, i.uv) * i.color;
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
