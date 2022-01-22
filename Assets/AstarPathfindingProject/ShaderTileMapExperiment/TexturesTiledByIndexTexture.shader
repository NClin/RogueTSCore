Shader "Unlit/TexturesTiledByIndexTexture"
{
    Properties
    {
        // encoded TileTypeMap[,]
        _IndexTexture ("Texture", 2D) = "white" {}

        // indextexture dimensions must be updated by script when index texture is created.
        _IndexTextureDimensions("IndexTexture Dimensions", vector) = (2,2,0,0)

        // tile textures
        _Texture0 ("Tile Texture 0", 2D) = "black" {}
        _Texture1 ("Tile Texture 1", 2D) = "white" {}
        _Texture2 ("Tile Texture 2", 2D) = "black" {}

        
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

            sampler2D _IndexTexture;
            float4 _IndexTextureDimensions;
            sampler2D _Texture0;
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
                // Definitions

                float4 texture0Index = 0;
                float4 texture1Index = 1;
                float4 texture2Index = 2;


                // sort out UV tiling
                float2 tiledUV = i.uv * _IndexTextureDimensions.xy;

                float4 index = tex2D(_IndexTexture, i.uv);
                

                fixed4 col;
               

                col = float4(1,1,0,1);     // Default/failure color is yellow. 
                
                int indexDecoded = index.a * 1 + index.z * 2 + index.y * 4 + index.x * 8;

                if ( indexDecoded == 0)
                {
                    col = tex2D(_Texture0, frac(tiledUV));
                }
                if ( indexDecoded == 1)
                {
                    col = tex2D(_Texture1, frac(tiledUV));
                }
                if ( indexDecoded == 2)
                {
                    col = tex2D(_Texture2, frac(tiledUV));
                }

                return col;
            }
            ENDCG
        }
    }
}
