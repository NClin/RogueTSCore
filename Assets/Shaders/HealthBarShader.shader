Shader "Unlit/HealthBarShader"
{
    Properties
    {
        _FullColor ("Full Color", Color) = (0,1,0,0)
        _EmptyColor ("Empty Color", Color) = (1, 0, 0, 0) 
        _HealthPcnt ("Health Percent", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolator
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _FullColor;
            float4 _EmptyColor;
            float _HealthPcnt;
            

            Interpolator vert (appdata v)
            {
                Interpolator o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (Interpolator i) : SV_Target
            {
                
                float4 col = lerp( float4(_EmptyColor.xyz, 1), float4(_FullColor.xyz, 1), _HealthPcnt);

                if (_HealthPcnt > .8)
                {
                    col = float4(_FullColor.xyz, 1);
                }

                if (_HealthPcnt < .2)
                {
                    col = float4(_EmptyColor.xyz, 1);
                }

                if (i.uv.x > _HealthPcnt)
                {
                    col = float4(0,0,0,0);
                }

                return col;
            }
            ENDCG
        }
    }
}
