Shader "Unlit/Molecule"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Speed ("Speed", Vector) = (1, 1, 0, 0)
        _RotationSpeed ("Rotation Speed", Float) = 2.0
        _Amplitude ("Amplitude", Vector) = (1, 1, 0, 0)
		_BlurSize("Blur Size", Range(0.0, 0.8)) = 0.05
        
        _Values ("Values", 2D) = "white" {}
        _StartTime ("Start Time", Float) = 0
        _DeltaTime ("Delta Time", Float) = 0

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
            float4 _Amplitude;
            float _RotationSpeed;
            
            sampler2D _Values;
            float _StartTime;

			uniform float _BlurSize;

            v2f vert (appdata v)
            {
                v2f o;


                float3 worldPosition = unity_ObjectToWorld._m03_m13_m23;
                worldPosition.z = sin(worldPosition.x) * cos(worldPosition.y);

                float noiseOffset = 1;//0.01;
                float noiseX = snoise(worldPosition + float3(noiseOffset,0,0)) - snoise(worldPosition);
                float noiseY = snoise(worldPosition + float3(0,noiseOffset,0)) - snoise(worldPosition);
                float noiseZ = snoise(worldPosition + float3(0,0,noiseOffset)) - snoise(worldPosition);

                float3 noise3D = normalize( float3(noiseX, noiseY, noiseZ) );

                float3 time = _Speed * PI2 * _Time.x * noise3D;
                float4 offset = float4( sin( time.x ), cos( time.y ), cos( time.z ), 1 );
                o.offset = offset;

                v.vertex += o.offset;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

            /*
                float blurLevel = i.offset.z;

                //calculate aspect ratio
                float invAspect = 0.3;
                //init color variable
                float4 col = 0;
                //iterate over blur samples
                for(float index = 0; index < 10; index++){
                    //get uv coordinate of sample
                    float2 uv = i.uv + float2((index/9 - 0.5) * _BlurSize * blurLevel * invAspect, 0);
                    //add color at position to color
                    col += tex2D(_MainTex, uv);
                }
                //divide the sum of values by the amount of samples
                col = col / 10;
                col *= _Color;
                return col;
                */
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
