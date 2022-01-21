Shader "Unlit/SquareShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [PerRendererData] _Color ("Color", color) = (0,0,0,1)
        [PerRendererData] _BorderWidth ("Border Width", float) = 5
        _BorderColor("Border Color", color) = (.04, .04, .04, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _BorderWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // draw borders
                if (step(_BorderWidth / 100, i.uv.x) == 0
                    || step(i.uv.x, (100 -_BorderWidth) / 100) == 0
                    || step(_BorderWidth / 100, i.uv.y) == 0
                    || step(i.uv.y, (100 -_BorderWidth) / 100) == 0)
                    {
                    //_Color += 1;
                    //_Color = clamp(_Color, 0, 1);
                    //col *= 0.5;
                    }

                return col *= _Color;
            }
            ENDCG
        }
    }
}
