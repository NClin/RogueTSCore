
Shader "Sprites/RadialPixelHealthBarSprite"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_PixelsX ("Pixel Scale", int) = 128
		_PixelsY ("Pixel Scale", int) = 128
		_PixelCenterOffset("Pixel Offset", Range(0, 1)) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 uv  : TEXCOORD0;
			};
			
			fixed4 _Color;
			int _PixelsX;
			int _PixelsY;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex =  UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;
				OUT.uv.x *= _PixelsX;
				OUT.uv.y *= _PixelsY;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;
			float _PixelCenterOffset;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				float xpix = floor(IN.uv.x);
				float ypix = floor(IN.uv.y);
				//xpix /= _PixelsX;

				//return fixed4(xpix / _PixelsX, xpix, xpix, 1);

				float2 pixelCenter = float2((xpix+_PixelCenterOffset)/_PixelsX, (ypix+_PixelCenterOffset)/_PixelsY);

				fixed4 colorPix = fixed4(pixelCenter.xxx, 1);
				return colorPix;
			}
		ENDCG
		}
	}
}