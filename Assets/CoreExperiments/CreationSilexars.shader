Shader "Unlit/CreationSilexars"
{
    Properties
    {
        _AspectRatio ("Aspect Ratio", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }

        Blend SrcAlpha OneMinusSrcAlpha

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float _AspectRatio;

            fixed4 frag (v2f iin) : SV_Target
            {
                float t = _Time.y * .4;

                float3 c;
                float l, z=t;

                for (int i = 0; i < 3; i++)
                {
                    float2 uv = iin.uv;
                    float2 p = iin.uv -.5;

                    z += .07;

                    p.x *= _AspectRatio;
                    // distance from center, given the centered coordinate system.
                    // much better than distance method.
                    l = length(p);
                    
                    uv += p*8/l*(sin(z)+1)*abs(sin(l*9-z-z));

                    //float2 lengthvector = float2((uv.x % 1), (uv.y % 1)) - .5;
                    //float outp = 0.01/length(lengthvector);
                    float outp = 0.01/length((uv % 1) * .5);
                    c[i] = outp;
                }

                float3 col =  float3(c/l);

                if (length(col) < 0.5)
                {
                    return float4(1,1,1,0);
                }

                return float4(col.xyz, 1);


            }
            ENDCG
        }
    }
}
