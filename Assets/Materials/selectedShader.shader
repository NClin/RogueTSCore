Shader "Unlit/selectedShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {

        Tags {
        "Queue"="Transparent"
            

        }


        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
        ZTest Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                half2 uv : TEXCOORD0;
            };

            struct v2f
            {
                half2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : COLOR
            {
                float4 col = tex2D(_MainTex, i.uv);

                return col;
            }
            ENDCG
        }
    }
}
