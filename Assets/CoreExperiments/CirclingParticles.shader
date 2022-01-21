Shader "Unlit/CirclingParticles"
{
    Properties
    {
        _ParticleColor ("Color", color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uvCentered: TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 uvCentered: TEXCOORD1;
            };

            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uvCentered = v.uv + float2(0.5, 0.5);
                return o;
            }

            float Box (float2 size, float2 xy)
            {
                size = float2(0.5, 0.5) - size*0.5;
                float2 uv = smoothstep(size, size + float2(0.001, 0.001), xy);
                uv *= smoothstep(size, size + float2(0.001, 0.001), float2(1,1) - xy);
                return uv.x * uv.y;
            }

            //float2x2 Rotate2d(float angle)
            //{
            //    return float2x2(cos(angle, -sin(_angle),
            //                    sin( angle), cos(angle)))
            //}

            fixed4 frag (v2f i) : SV_Target
            {

                float2 centeredUV = i.uv + float2(0.5, 0.5);

                float4 col = float4(step(distance(i.uv, float2(0.5, 0.5)), 0.5), 0,0,1);
                
                float boxRet = Box(float2(0.2, 0.2), i.uv);

                i.uv -= float2(0.5,0.5);


                col += float4(boxRet, boxRet, boxRet, 0);


                //float4 col; 
                //if (distance(i.uv, float2(0.5,0.5)) > 0.1)
                //{
                //col = float4(1,1,1,1);
                //}
                //else
                //{
                //col = float4(1,1,0,1);
                //}

                return col;
            }
            ENDCG
        }
    }
}
