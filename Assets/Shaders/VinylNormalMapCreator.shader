Shader "Zhirnov/VinylNormalMapCreator"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _RadiusV ("Sphere Size", Vector) = (0.1,0.1,0,0)
        _Point1 ("Point One", Vector) = (0,0,0,0)
        _Point2 ("Point Two", Vector) = (1,1,1,1)
    }

    SubShader
    {
        Lighting Off
        Blend One Zero

        Pass
        {
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            #define NORMAL_UP float4(0.5,0.5,1,1)

            float3      _RadiusV;
            float3      _Point1;
            float3      _Point2;
            sampler2D   _Tex;

            float getRadius(float3 direction, bool isNormalized)
            {
                float3 n = isNormalized ? direction : normalize(direction);
                float3 r = n * _RadiusV;
                return length(r);
			}

            float sign (float3 p1, float3 p2, float3 p3)
            {
                return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
            }

            bool pointInTriangle (float3 pt, float3 v1, float3 v2, float3 v3)
            {
                float d1, d2, d3;
                bool has_neg, has_pos;

                d1 = sign(pt, v1, v2);
                d2 = sign(pt, v2, v3);
                d3 = sign(pt, v3, v1);

                has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
                has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

                return !(has_neg && has_pos);
            }

            bool isInRect(float3 p, float3 point1, float3 point2)
            {
                float3 d = point1 - p;
                float3 l = point2 - point1;
                if (length(d) <= 0.000001 || length(l) <= 0.000001)
                {
                    return false;        
				}
                float3 n = normalize(cross(d, l));

                float3 r = normalize(cross(l, n));
                r *= getRadius(r, true);

                float3 p1 = point1 - r;
                float3 p2 = point1 + r;
                float3 p3 = point2 + r;
                float3 p4 = point2 - r;

                return pointInTriangle(p, p1, p2, p3) ||
                    pointInTriangle(p, p1, p3, p4);
			}

            float4 toCircleNormal(float3 distance)
            {
                float3 normI = float3(0, 0, -1);
                float3 c = normalize(cross(distance, normI));
                distance = normalize(distance);
                c = normalize(cross(distance * getRadius(distance, true) + normI, c));

                c = c / 2 + 0.5;

                return float4(c, 1);
			}

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                float3 p = IN.globalTexcoord;
                p.y = 1 - p.y;
                float3 distance1 = _Point1 - p;
                float3 distance2 = _Point2 - p;
                
                if (isInRect(p, _Point1, _Point2))
                {
                    float3 l = _Point2 - _Point1; 
                    float3 norm = cross(distance1, l);
                    float3 c = normalize(cross(l, norm));
                    c *= getRadius(c, true);

                    return toCircleNormal(c); 
				}

                if (length(distance1) <= getRadius(distance1, false))
                {
                    return toCircleNormal(distance1);
				}

                if (length(distance2) <= getRadius(distance2, false))
                {
                    return toCircleNormal(distance2);
				}
                discard;
                return NORMAL_UP;
            }
            ENDCG
        }
    }
}