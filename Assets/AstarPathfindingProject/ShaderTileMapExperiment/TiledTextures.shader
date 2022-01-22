Shader "Unlit/TiledTextures"
{
    Properties
    {
        // indextexture dimensions must be updated by script when index texture is created.
        _Dimensions("IndexTexture Dimensions", vector) = (2,2,0,0)

        // tile textures
        _Texture1 ("Tile Texture 1", 2D) = "black" {}
        _Texture2 ("Tile Texture 2", 2D) = "white" {}

        
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

            Texture2D _IndexTexture;
            float4 _Dimensions;
            sampler2D _Texture1;
            sampler2D _Texture2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                float2 tiledUV = i.uv * _Dimensions.xy;
                float2 uv = frac(tiledUV);
                fixed4 col;


                // example checkerboard pattern.
                if ( (floor(tiledUV.x) + floor(tiledUV.y) ) % 2 == 0)
                {
                    col = tex2D(_Texture1, uv);
                }
                else
                {
                    col = tex2D(_Texture2, uv);
                }

                return col;
            }
            ENDCG
        }
    }
}
