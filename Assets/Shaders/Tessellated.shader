Shader "Zhirnov/Tesellated"
{
    Properties 
    {
        _Tess ("Tessellation", Range(1,32)) = 4
        _Gloss ("Gloss", Range(0,10)) = 1
        _Specular ("Specular", Range(0.02,1)) = 0.2
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _DispTex ("Disp Texture", 2D) = "gray" {}
        _NormalMap ("Normalmap", 2D) = "bump" {}
        _Displacement ("Displacement", Range(-5, 5)) = 0.3
        _Color ("Color", color) = (1,1,1,0)
        _SpecColor ("Spec color", color) = (0.5,0.5,0.5,0.5)
    }
    SubShader 
    {
        Tags { "RenderType"="Opaque" }
        LOD 300
            
        CGPROGRAM
        #pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:disp nolightmap tessellate:tessFixed
        #pragma target 4.6
        
        float _Tess;
        sampler2D _DispTex;
        float _Displacement;
        float _Gloss;
        float _Specular;
        
        float4 tessFixed()
        {
            return _Tess;
        }


        struct appdata {
            float4 vertex : POSITION;
            float4 tangent : TANGENT;
            float3 normal : NORMAL;
            float2 texcoord : TEXCOORD0;
        };

        void disp (inout appdata v)
        {
            float d = tex2Dlod(_DispTex, float4(v.texcoord.xy,0,0)).r;
            d = 2 * d - 1;
            d *= _Displacement;
            v.vertex.xyz += v.normal * d;
        }

        struct Input {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        sampler2D _NormalMap;
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutput o) {
            half4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            float2 uv0 = IN.uv_MainTex;
            float2 uv1 = IN.uv_MainTex + float2(0.001, 0);
            float2 uv2 = IN.uv_MainTex + float2(0, 0.001);

            float h0 = tex2D( _DispTex, uv0 ).r * _Displacement;
            float h1 = tex2D( _DispTex, uv1 ).r * _Displacement;
            float h2 = tex2D( _DispTex, uv2 ).r * _Displacement;

            float3 p0 = float3(uv0, h0);
            float3 p1 = float3(uv1, h1);
            float3 p2 = float3(uv2, h2);

            float3 n = -normalize(cross(p2 - p0, p1 - p0));
            o.Normal = n;

            o.Albedo = c.rgb;
            o.Specular = _Specular;
            o.Gloss = _Gloss;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
