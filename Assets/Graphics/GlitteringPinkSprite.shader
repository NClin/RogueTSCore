Shader "Sprite/GlitteringPink"
{
    Properties
    {
    [PerRendererData]  _MainTex ("Sprite Texture", 2D) = "white" { }
    _InnerBand ("Band", Range(0,0.4)) = 0.333333
    _SquareSize ("SquareSize", float) = 0.167

    // colors top to bottom
    // todo: experiment with mathematical relationships to determine the colors.
    _ColorA ("ColorA", Color) = (1,1,1,1)
    _ColorB ("ColorB", Color) = (.8, .8, .8, 1)
    _ColorC ("ColorC", Color) = (.6, .6, .6, 1)
    _ColorD ("ColorD", Color) = (.4, .4, .4, 1)
    _ColorE ("ColorE", Color) =  (.2, .2, .2, 1)

    }
    SubShader
    {
        Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CanUseSpriteAtlas"="true" "PreviewType"="Plane" }
        Blend One Zero


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

            float _InnerBand;
            float _SquareSize;
            float4 _ColorA;
            float4 _ColorB;
            float4 _ColorC;
            float4 _ColorD;
            float4 _ColorE;

            float randomTimeFrac(float2 xy, float timeScale)
            {
                float rnd1 = 12.1134 + frac(_Time.y * timeScale);
                float rnd2 = 56.4 - frac(_Time.y * timeScale);
                float rnd3 = 34356 + frac(_Time.y * timeScale) * 155;
                return frac(sin(dot(xy.xy, float2(rnd1, rnd2))) * rnd3);
            }

            float randomTimeFrac(float2 xy)
            {
                float timeScale = .0000003;

                float rnd1 = 12.1134 + frac(_Time.y * timeScale);
                float rnd2 = 56.4 - frac(_Time.y * timeScale);
                float rnd3 = 34356 + frac(_Time.y * timeScale) * 155;
                return frac(sin(dot(xy.xy, float2(rnd1, rnd2))) * rnd3);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float scale = 10;
                o.uv = scale * v.uv;
                o.uv += 1;
                return o;
            }


            float4 frag (v2f i) : SV_Target
            {



               float4 col;
               //float steppoint = abs(sin(frac(_Time.y / 2)));

               //float2 bl = step(float2(steppoint, steppoint), frac(i.uv) / (3 * steppoint) );
               //// logical operations on normalized vectors with arithmetic?
               //// * == AND
               //// / == NOT
               //float pct = bl.y * bl.x;
               

               //float2 tr = step(float2(steppoint, steppoint), frac(1-i.uv));
               //// vector logic again
               //pct *= tr.x * tr.y;

               // first pass
               float2 ipos = floor(i.uv * 2.3 * 1+sin(_Time.y));

               float2 fpos = frac(i.uv);
               float pct = randomTimeFrac(ipos, 0.000005);
               
               col = float4(pct, pct, pct, pct * (1+sin(_Time.y * 3))*50);
               col *= randomTimeFrac(ipos);
               _ColorA += float4(0.6 * pct, 0.01 * pct, 0.1 * pct, 0);
               col *= _ColorA;

               // alpha of larger sections
               float2 iAlpha = floor(i.uv * 0.5);
               float pctA = randomTimeFrac(ipos, 0.000002);
               _ColorA *= float4(1,1,1,0);

               // second pass
               float2 ipos2 = floor(i.uv * 4);

               float2 fpos2 = frac(i.uv);
               float pct2 = randomTimeFrac(ipos2, 0.001);
               
               col *= float4(pct2, pct2, pct2, 1) * 5;
               col *= randomTimeFrac(ipos2);
               _ColorA += float4(0.5 * pct2, 0.02, 0.1 * pct2, 0);

               
               //float2 yBand = float2(i.uv.x / 2 , floor(i.uv.y));
               //float pct3 = randomTimeFrac(yBand, 0.00001);
               //float4 toAdd = lerp(_ColorB, _ColorC, pct3);
               //col *= toAdd;



               return col;




                // Initial attempt: paint regions manually. Works, but wouldn't
                // do well to animate or etc. Use functions instead.

                //if (i.uv.y >= 1 - _InnerBand)
                //{
                //col = _ColorA;
                //}
                //if (i.uv.y < 1 - _InnerBand && i.uv.y > 0.5)
                //{
                //col = _ColorB;
                //}

                //if (i.uv.y <= 0.5 &&  i.uv.y > _InnerBand)
                //{
                //col = _ColorC;
                //}

                //if (i.uv.y <= _InnerBand)
                //{
                //col = _ColorD;
                //}

                //// paint square
                //if (i.uv.y >= _InnerBand + _SquareSize
                //&& i.uv.x > 0.5 - (_SquareSize * 0.5)
                //&& i.uv.x < 0.5 + (_SquareSize * 0.5))
                //{
                //    col = _ColorE;
                //}

            }
            ENDCG
        }
    }
}
